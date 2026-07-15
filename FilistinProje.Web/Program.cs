global using FilistinProje.Core.Helpers;
global using FilistinProje.Service.Helpers;
global using FilistinProje.Core.Models;
global using FilistinProje.Service.Interfaces;

using FilistinProje.Data;
using Microsoft.EntityFrameworkCore;
using FilistinProje.Core.Interfaces;
using FilistinProje.Data.Repositories;
using FilistinProje.Service.Services;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using FilistinProje.Core.Varliklar;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using FilistinProje.Web.Attributes;
using FilistinProje.Web.Diagnostics;
using FilistinProje.Web.HealthChecks;
using FilistinProje.Web.Security;
using FilistinProje.Web.Services;
using System.Net;
using System.Threading.RateLimiting;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Hangfire;
using Hangfire.PostgreSql;
using Npgsql;
using System.Security.Claims;

// ============================================================
// EPPLUS LÄ°SANS AYARI (En Tepeye Eklenmeli)
// ============================================================
Environment.SetEnvironmentVariable("EPPlusLicenseContext", "NonCommercial");
// ============================================================

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("secrets.json", optional: true, reloadOnChange: true);
builder.Configuration.AddEnvironmentVariables();

var startupWarnings = new List<string>();
var defaultConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var isDatabaseAvailableAtStartup = CanConnectToPostgres(defaultConnectionString, out var databaseAvailabilityError);

if (!isDatabaseAvailableAtStartup && !string.IsNullOrWhiteSpace(databaseAvailabilityError))
{
    startupWarnings.Add($"PostgreSQL baglantisi kurulamadi. Hangfire ve zamanlanmis isler kapatildi. Detay: {databaseAvailabilityError}");
}

var dataProtectionKeysPath = Environment.GetEnvironmentVariable("DATA_PROTECTION_KEYS_PATH");
if (string.IsNullOrWhiteSpace(dataProtectionKeysPath))
{
    dataProtectionKeysPath = Path.Combine(builder.Environment.ContentRootPath, "App_Data", "DataProtectionKeys");
}

// 1. VeritabanÄ± BaÄŸlantÄ±sÄ±
builder.Services.AddSingleton<RequestSqlProfiler>();
builder.Services.AddDbContext<KanvasDbContext>((serviceProvider, options) =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));

    if (builder.Environment.IsDevelopment())
    {
        options.AddInterceptors(serviceProvider.GetRequiredService<RequestSqlProfiler>());
    }
});
builder.Services.AddScoped<DevelopmentTestProductImporter>();

builder.Services.AddDataProtection()
    .SetApplicationName("7ANRPS48")
    .PersistKeysToFileSystem(new DirectoryInfo(dataProtectionKeysPath));

// 2. Identity (Ãœyelik) Servisi
builder.Services.AddIdentity<AppUser, IdentityRole>(options => 
{
    // Åifre KurallarÄ±
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
    
    // Hesap Kilitleme (Brute-Force KorumasÄ±)
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
    options.Lockout.AllowedForNewUsers = true;
})
.AddErrorDescriber<FilistinProje.Core.Helpers.TurkceIdentityErrorDescriber>()
.AddEntityFrameworkStores<KanvasDbContext>()
.AddDefaultTokenProviders();

// 3. Cookie (Ã‡erez) AyarlarÄ±
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Hesap/GirisYap";
    options.LogoutPath = "/Hesap/CikisYap";
    options.AccessDeniedPath = "/Hesap/ErisimEngellendi";
    options.SlidingExpiration = true;
    options.ExpireTimeSpan = TimeSpan.FromDays(30);
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.Cookie.HttpOnly = true;
    options.Events = new CookieAuthenticationEvents
    {
        OnRedirectToLogin = context => HandleAuthRedirectAsync(
            context,
            StatusCodes.Status401Unauthorized,
            "admin_auth_required",
            "Admin paneline erismek icin giris yapilmasi gerekiyor."),
        OnRedirectToAccessDenied = context => HandleAuthRedirectAsync(
            context,
            StatusCodes.Status403Forbidden,
            "admin_access_denied",
            "Admin panelinde yetkisiz erisim denemesi tespit edildi."),
        OnValidatePrincipal = async context =>
        {
            if (context.Principal?.Identity?.IsAuthenticated != true)
            {
                return;
            }

            var userManager = context.HttpContext.RequestServices.GetRequiredService<UserManager<AppUser>>();
            var signInManager = context.HttpContext.RequestServices.GetRequiredService<SignInManager<AppUser>>();
            var user = await userManager.GetUserAsync(context.Principal);

            if (user == null)
            {
                context.RejectPrincipal();
                await signInManager.SignOutAsync();
                return;
            }

            var databaseRoles = await userManager.GetRolesAsync(user);
            var cookieRoles = context.Principal
                .FindAll(ClaimTypes.Role)
                .Select(x => x.Value)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            var rolesChanged = databaseRoles.Count != cookieRoles.Count ||
                databaseRoles.Any(role => !cookieRoles.Contains(role));

            if (rolesChanged)
            {
                context.ReplacePrincipal(await signInManager.CreateUserPrincipalAsync(user));
                context.ShouldRenew = true;
            }
        }
    };
});
// Serilog KonfigÃ¼rasyonu (Aktif)
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/7anrps48-log-.txt", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 30)
    .CreateLogger();
builder.Host.UseSerilog();

// Cache AltyapÄ±sÄ± Ekleme
builder.Services.AddMemoryCache();
builder.Services.AddScoped<ICacheService, CacheService>();

// Media AltyapÄ±sÄ±
builder.Services.AddScoped<IMediaService, LocalMediaService>();
builder.Services.AddScoped<IDosyaServisi, DosyaServisi>();

// Hangfire AltyapÄ±sÄ±
if (isDatabaseAvailableAtStartup)
{
    builder.Services.AddHangfire(configuration => configuration
        .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UsePostgreSqlStorage(options => options.UseNpgsqlConnection(defaultConnectionString)));

    builder.Services.AddHangfireServer();
}

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped(typeof(IService<>), typeof(Service<>));


// Email Servisini TanÄ±tÄ±yoruz
builder.Services.AddScoped<FilistinProje.Core.Interfaces.IEmailService, FilistinProje.Service.Services.SmtpEmailService>();


// 5. Session AyarlarÄ± - SADECE BÄ°R KERE EKLEYÄ°N!
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.Name = ".FilistinProje.Session";
});

// SEO Servisleri
builder.Services.AddScoped<ISeoService, SeoService>();
builder.Services.AddScoped<ISepetService, FilistinProje.Service.SepetService>(); // ğŸ›’ Database Cart Service
if (isDatabaseAvailableAtStartup)
{
    builder.Services.AddHostedService<AbandonedCartService>(); // 📧 Abandoned Cart Background Job
    builder.Services.AddHostedService<FavoriPriceDropService>(); // 🔔 Favori Fiyat Düşüş Bildirimi
    builder.Services.AddHostedService<StockAlertService>(); // 📦 Stok Uyarı E-postası (Adım 85)
}

builder.Services.AddScoped<ZiyaretciTakipAttribute>();

// 7. HTTP Context Accessor
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ISiteSettingsService, SiteSettingsService>();
builder.Services.AddScoped<IHomePageSettingsService, HomePageSettingsService>();
builder.Services.AddScoped<IHomePageSectionService, HomePageSectionService>();
builder.Services.AddScoped<IFavoriService, FavoriService>();
builder.Services.AddScoped<IKargoHesaplamaServisi, KargoHesaplamaServisi>();
builder.Services.AddScoped<IOrderPricingService, OrderPricingService>();
builder.Services.AddScoped<IPurchaseOrderService, PurchaseOrderService>();
builder.Services.AddSingleton<IAdminSecurityAuditService, AdminSecurityAuditService>();
builder.Services.AddScoped<IFirebaseNotificationService, FirebaseNotificationService>();
builder.Services.AddHttpClient("FirebaseFCM");
builder.Services.AddSingleton<IAdminSessionStateService, AdminSessionStateService>();
builder.Services.AddScoped<IFaturaPdfService, FilistinProje.Web.Services.FaturaPdfService>();
// Health Checks (Docker / Load Balancer / Monitoring)
var startupReadinessState = new StartupReadinessState();
builder.Services.AddSingleton(startupReadinessState);
builder.Services.AddHealthChecks()
    .AddDbContextCheck<KanvasDbContext>(name: "database", failureStatus: Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Unhealthy, tags: new[] { "ready" })
    .AddCheck<StartupReadinessHealthCheck>("startup", failureStatus: Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Unhealthy, tags: new[] { "ready" });

// Response SÄ±kÄ±ÅŸtÄ±rma (Gzip/Brotli)
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
});

// 8. MVC ve Session
builder.Services.AddControllersWithViews(options =>
{
    // Bu satÄ±r sayesinde siteye giren herkes otomatik kaydedilir
    options.Filters.Add<ZiyaretciTakipAttribute>(); 
});

// 9. Dil ayarlari
builder.Services.AddLocalization();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(AdminPolicyNames.AdminPanelAccess, policy =>
        policy.RequireRole(AdminSecurityRoles.AllAdminRoles));
});

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[]
    {
        new CultureInfo("ar"),
        new CultureInfo("en")
    };

    options.DefaultRequestCulture = new RequestCulture("ar");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
    options.RequestCultureProviders = new[]
    {
        new CookieRequestCultureProvider()
    };
});

// 10. Rate Limiting (Brute-force korumasÄ±)
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = 429;
    // GiriÅŸ/KayÄ±t iÃ§in brute-force korumasÄ±
    options.AddPolicy("auth", context =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 10,
                Window = TimeSpan.FromMinutes(5),
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = 0
            }));
    // Genel API istekleri iÃ§in
    options.AddPolicy("general", context =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 100,
                Window = TimeSpan.FromMinutes(1),
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = 0
            }));
});

// 11. Antiforgery gÃ¼venli ayarlar
builder.Services.AddAntiforgery(options =>
{
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.Strict;
});

// ==========================================
// BUILD AÅAMASI - Service Collection ArtÄ±k Read-Only!
// ==========================================
var app = builder.Build();

if (args.Contains("--import-test-products", StringComparer.OrdinalIgnoreCase))
{
    await using var scope = app.Services.CreateAsyncScope();
    await scope.ServiceProvider.GetRequiredService<DevelopmentTestProductImporter>().ImportAsync();
    return;
}
var runningInContainer = string.Equals(
    Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER"),
    "true",
    StringComparison.OrdinalIgnoreCase);

foreach (var startupWarning in startupWarnings)
{
    app.Logger.LogWarning(startupWarning);
}

// --- PIPELINE (SIRALAMA Ã–NEMLÄ°) ---

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); // HTTPS zorunluluÄŸu (production)
}

if (app.Environment.IsDevelopment())
{
    app.Use(async (context, next) =>
    {
        using var scope = RequestSqlProfiler.BeginScope();
        context.Response.OnStarting(() =>
        {
            var sql = RequestSqlProfiler.Snapshot();
            context.Response.Headers["X-Sql-Query-Count"] = sql.QueryCount.ToString(CultureInfo.InvariantCulture);
            context.Response.Headers["X-Sql-Elapsed-Ms"] = sql.Elapsed.TotalMilliseconds.ToString("0.###", CultureInfo.InvariantCulture);
            return Task.CompletedTask;
        });

        await next();

        var sql = RequestSqlProfiler.Snapshot();
        app.Logger.LogInformation(
            "SQL profile {Method} {Path}: {QueryCount} queries in {SqlElapsedMs} ms",
            context.Request.Method,
            context.Request.Path.Value,
            sql.QueryCount,
            sql.Elapsed.TotalMilliseconds);
    });
}

// GÃœVENLÄ°K HEADER'LARI
app.Use(async (context, next) =>
{
    context.Response.Headers["X-Content-Type-Options"] = "nosniff";
    context.Response.Headers["X-Frame-Options"] = "DENY";
    context.Response.Headers["X-XSS-Protection"] = "1; mode=block";
    context.Response.Headers["Referrer-Policy"] = "strict-origin-when-cross-origin";
    context.Response.Headers["Permissions-Policy"] = IsCameraAllowedPath(context.Request.Path)
        ? "camera=(self), microphone=(), geolocation=()"
        : "camera=(), microphone=(), geolocation=()";
    await next();
});

if (!runningInContainer)
{
    app.UseHttpsRedirection();
}

app.Use(async (context, next) =>
{
    if (IsLegacySensitiveUploadPath(context.Request.Path))
    {
        context.Response.StatusCode = StatusCodes.Status404NotFound;
        return;
    }

    await next();
});

app.UseStaticFiles();
app.UseRequestLocalization();

// Ozel Hata Sayfalari (404 vb.) - Guzel tasarimli sayfa gosterir
app.UseStatusCodePagesWithReExecute("/Hata/{0}");
app.Use(async (context, next) =>
{
    if (IsMaintenanceAllowedPath(context.Request.Path))
    {
        await next();
        return;
    }

    var siteSettingsService = context.RequestServices.GetRequiredService<ISiteSettingsService>();
    var siteSettings = siteSettingsService.GetSettings();

    if (!siteSettings.BakimModuAktif)
    {
        await next();
        return;
    }

    context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
    context.Response.ContentType = "text/html; charset=utf-8";

    var siteTitleText = string.IsNullOrWhiteSpace(siteSettings.MarkaAdi)
        ? siteSettings.SiteAdi
        : siteSettings.MarkaAdi;
    var siteTitle = WebUtility.HtmlEncode(siteTitleText);
    var requestCulture = context.Features.Get<IRequestCultureFeature>()?.RequestCulture.Culture.TwoLetterISOLanguageName
        ?? "ar";
    var maintenanceText = requestCulture switch
    {
        "ar" => new
        {
            Lang = "ar",
            Dir = "rtl",
            Title = "وضع الصيانة",
            Badge = "وضع الصيانة",
            Heading = $"{siteTitleText} قيد التحضير لفترة قصيرة",
            Message = "نعمل حالياً على تحسين تجربة التسوق لديكم. سنعود قريباً مع متجر 7ANRPS48.",
            Note = "طلباتكم وبيانات عضويتكم وسلة التسوق محفوظة بأمان."
        },
        "en" => new
        {
            Lang = "en",
            Dir = "ltr",
            Title = "Maintenance Mode",
            Badge = "Maintenance Mode",
            Heading = $"{siteTitleText} is getting ready for a short while",
            Message = "We are making a short maintenance update to improve your shopping experience. 7ANRPS48 will be back online soon.",
            Note = "Your orders, account details and cart are safely protected."
        },
        _ => new
        {
            Lang = "en",
            Dir = "ltr",
            Title = "Maintenance Mode",
            Badge = "Maintenance Mode",
            Heading = $"{siteTitleText} is getting ready for a short while",
            Message = "We are making a short maintenance update to improve your shopping experience. 7ANRPS48 will be back online soon.",
            Note = "Your orders, account details and cart are safely protected."
        }
    };
    var siteMessage = WebUtility.HtmlEncode(maintenanceText.Message);
    var themeColor = WebUtility.HtmlEncode(siteSettings.TemaRengi);
    var logoUrl = WebUtility.HtmlEncode(string.IsNullOrWhiteSpace(siteSettings.SiteLogoUrl)
        ? "/74anrps48logo2.svg"
        : siteSettings.SiteLogoUrl);
    var maintenanceLang = WebUtility.HtmlEncode(maintenanceText.Lang);
    var maintenanceDir = WebUtility.HtmlEncode(maintenanceText.Dir);
    var maintenanceTitle = WebUtility.HtmlEncode(maintenanceText.Title);
    var maintenanceBadge = WebUtility.HtmlEncode(maintenanceText.Badge);
    var maintenanceHeading = WebUtility.HtmlEncode(maintenanceText.Heading);
    var maintenanceNote = WebUtility.HtmlEncode(maintenanceText.Note);

    await context.Response.WriteAsync($$"""
<!doctype html>
<html lang="{{maintenanceLang}}" dir="{{maintenanceDir}}">
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title>{{siteTitle}} | {{maintenanceTitle}}</title>
    <style>
        * { box-sizing:border-box; }
        body { margin:0; font-family:"Segoe UI",Arial,sans-serif; background:#fcf9f3; color:#252515; min-height:100vh; display:flex; align-items:center; justify-content:center; padding:24px; }
        body:before { content:""; position:fixed; inset:0; background:radial-gradient(circle at top left, rgba(181,135,53,.16), transparent 34%), linear-gradient(135deg, rgba(49,53,17,.08), transparent 42%); pointer-events:none; }
        .card { position:relative; width:min(720px,100%); background:rgba(255,255,255,.72); border:1px solid #e5e2dc; border-radius:18px; padding:42px 38px; box-shadow:0 24px 70px rgba(49,53,17,.14); text-align:center; }
        .logo { width:156px; max-width:55vw; height:auto; margin:0 auto 24px; display:block; }
        .badge { display:inline-flex; align-items:center; gap:8px; background:rgba(49,53,17,.08); color:{{themeColor}}; border:1px solid rgba(49,53,17,.16); padding:8px 14px; border-radius:999px; font-size:12px; font-weight:700; letter-spacing:.05em; text-transform:uppercase; }
        .badge:before { content:""; width:7px; height:7px; border-radius:999px; background:#b58735; }
        h1 { margin:18px auto 12px; max-width:560px; font-size:34px; line-height:1.18; color:#313511; font-weight:700; }
        p { margin:0 auto; max-width:590px; color:#5d5b50; font-size:16px; line-height:1.75; }
        .note { margin-top:26px; padding-top:22px; border-top:1px solid #e5e2dc; color:#7a766a; font-size:13px; }
        @media (max-width:640px) { .card { padding:32px 22px; border-radius:14px; } h1 { font-size:26px; } p { font-size:15px; } }
    </style>
</head>
<body>
    <div class="card">
        <img src="{{logoUrl}}" alt="{{siteTitle}}" class="logo" onerror="this.style.display='none'">
        <span class="badge">{{maintenanceBadge}}</span>
        <h1>{{maintenanceHeading}}</h1>
        <p>{{siteMessage}}</p>
        <div class="note">{{maintenanceNote}}</div>
    </div>
</body>
</html>
""");
});
app.UseResponseCompression();
app.UseRouting();
app.UseRateLimiter();

// Ã–nce Session, Sonra Kimlik DoÄŸrulama
app.UseSession(); 
app.UseAuthentication(); // <--- GiriÅŸ yapmÄ±ÅŸ mÄ±?
app.UseAuthorization();  // <--- Yetkisi var mÄ±?

// GiriÅŸ Zorunluluk Middleware'i - SiteAyarlari.GirisZorunluMu=true iken
// oturum aÃ§mamÄ±ÅŸ kullanÄ±cÄ±larÄ± /Hesap/GirisYap sayfasÄ±na yÃ¶nlendirir.
app.Use(async (context, next) =>
{
    if (context.User.Identity?.IsAuthenticated == true)
    {
        await next();
        return;
    }

    if (IsLoginRequiredAllowedPath(context.Request.Path))
    {
        await next();
        return;
    }

    var siteSettingsService = context.RequestServices.GetRequiredService<ISiteSettingsService>();
    var siteSettings = siteSettingsService.GetSettings();

    if (!siteSettings.GirisZorunluMu)
    {
        await next();
        return;
    }

    var returnUrl = Uri.EscapeDataString(context.Request.Path + context.Request.QueryString);
    context.Response.Redirect($"/Hesap/GirisYap?returnUrl={returnUrl}");
});

// Controller route'larÄ±
app.MapControllers();
app.MapHealthChecks("/health/live", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    Predicate = _ => false,
    ResponseWriter = HealthCheckResponseWriter.WriteLiveness,
    AllowCachingResponses = false
});
app.MapHealthChecks("/health/ready", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("ready"),
    ResponseWriter = HealthCheckResponseWriter.WriteReadiness,
    AllowCachingResponses = false
});
app.MapHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("ready"),
    ResponseWriter = HealthCheckResponseWriter.WriteReadiness,
    AllowCachingResponses = false
});

app.MapGet("/Admin", (HttpContext context) =>
{
    // Admin route'ı için yetki kontrolü - admin rolü olan kullanıcılar erişebilir
    if (context.User?.Identity?.IsAuthenticated != true)
    {
        var returnUrl = Uri.EscapeDataString("/Admin/Home/Index");
        return Results.Redirect($"/Hesap/GirisYap?returnUrl={returnUrl}");
    }

    // Kullanıcının admin rolü var mı kontrol et
    var isAdmin = AdminSecurityRoles.AllAdminRoles.Any(role => context.User.IsInRole(role));
    if (!isAdmin)
    {
        return Results.Redirect("/Hesap/ErisimEngellendi");
    }

    return Results.Redirect("/Admin/Home/Index");
});

// Hangfire ArayÃ¼zÃ¼ (Åimdilik yetkisiz eriÅŸim aÃ§Ä±k; daha sonra yetkilendirilecek)
if (isDatabaseAvailableAtStartup)
{
    app.UseHangfireDashboard("/admin/hangfire", new DashboardOptions
    {
    // Authorization filter'Ä± ÅŸimdilik null veya basit tutuyoruz ki gÃ¶rebilelim.
    Authorization = new[] { new Hangfire.Dashboard.LocalRequestsOnlyAuthorizationFilter() } 
    });
}

// 1. Admin RotasÄ±
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

// 2. Standart Rota
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// --- VERÄ°TABANI GÃœNCELLEME VE BAÅLANGIÃ‡ VERÄ°LERÄ° ---
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    var isProduction = app.Environment.IsProduction();
    var migrationFailure = false;

    try
    {
        var context = services.GetRequiredService<KanvasDbContext>();

        if (!isDatabaseAvailableAtStartup)
        {
            startupReadinessState.Transition(StartupReadinessPhase.DatabaseUnavailable);
            logger.LogWarning("Veritabanina erisilemedigi icin migration ve seed adimlari atlandi. Phase=DatabaseUnavailable; /health/live=alive; /health/ready=503");
            return;
        }

        try
        {
            await EnsureKnownSchemaDriftAsync(context, logger);
        }
        catch (Exception ex)
        {
            startupReadinessState.Transition(StartupReadinessPhase.SchemaDriftFailed, ex);
            logger.LogError(ex, "Schema drift kontrolu basarisiz oldu.");
            migrationFailure = true;
            throw;
        }

        await EnsureMissingMarch2026SchemaAsync(context, logger);

        await EnsureMigrationHistoryConsistencyAsync(context, logger);

        try { await context.Database.MigrateAsync(); }
        catch (Exception ex)
        {
            startupReadinessState.Transition(StartupReadinessPhase.MigrationFailed, ex);
            logger.LogError(ex, "EF Migration sirasinda kritik hata olustu. Veritabanini uygulama semasina eslemek icin operasyon mudahalesi gerekiyor.");
            migrationFailure = true;
            throw;
        }

        try
        {
            await EnsureSensitiveUploadsMigratedAsync(context, app.Environment, logger);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Legacy hassas dosya migration tamamlanamadi; dosyalar public URL ile servis edilmiyor (route 404 guard).");
        }

        try
        {
            await FilistinProje.Web.Data.DbSeeder.VerileriYukle(app);
        }
        catch (Exception ex)
        {
            startupReadinessState.Transition(StartupReadinessPhase.SeedFailed, ex);
            logger.LogError(ex, "Seed verileri yuklenemedi.");
            migrationFailure = true;
            throw;
        }

        startupReadinessState.Transition(StartupReadinessPhase.Ready);
        logger.LogInformation("Startup readiness tamamlandi. Phase=Ready.");
    }
    catch (Exception ex)
    {
        if (migrationFailure && isProduction)
        {
            logger.LogCritical(ex, "PROD FAIL-FAST: Kritik migration veya seed hata nedeniyle uygulama baslatilmiyor. Veritabanini geri almak veya migrate'i manuel calistirmak gerekli.");
            app.Lifetime.StopApplication();
            return;
        }

        logger.LogError(ex, "Veritabani migration islemi sirasinda bir hata olustu. Development modunda uygulama calismaya devam ediyor.");
    }
}

app.Run();

static bool IsLoginRequiredAllowedPath(PathString path)
{
    var value = path.Value ?? string.Empty;

    return value.StartsWith("/admin", StringComparison.OrdinalIgnoreCase)
        || value.StartsWith("/hesap", StringComparison.OrdinalIgnoreCase)
        || value.StartsWith("/hata", StringComparison.OrdinalIgnoreCase)
        || value.StartsWith("/lib", StringComparison.OrdinalIgnoreCase)
        || value.StartsWith("/css", StringComparison.OrdinalIgnoreCase)
        || value.StartsWith("/js", StringComparison.OrdinalIgnoreCase)
        || value.StartsWith("/img", StringComparison.OrdinalIgnoreCase)
        || value.StartsWith("/fonts", StringComparison.OrdinalIgnoreCase)
        || value.StartsWith("/favicon", StringComparison.OrdinalIgnoreCase)
        || value.StartsWith("/logo", StringComparison.OrdinalIgnoreCase)
        || value.StartsWith("/EmailTemplates", StringComparison.OrdinalIgnoreCase)
        || value.StartsWith("/uploads", StringComparison.OrdinalIgnoreCase)
        || value.StartsWith("/health", StringComparison.OrdinalIgnoreCase)
        || value == "/"
        || value == string.Empty;
}

static bool IsCameraAllowedPath(PathString path)
{
    var value = path.Value ?? string.Empty;
    return value.Equals("/Siparis/Odeme", StringComparison.OrdinalIgnoreCase)
        || value.Equals("/Hesap/KayitOl", StringComparison.OrdinalIgnoreCase);
}

static bool IsLegacySensitiveUploadPath(PathString path)
{
    var value = path.Value ?? string.Empty;
    return value.StartsWith("/uploads/kimlikler", StringComparison.OrdinalIgnoreCase)
        || value.StartsWith("/uploads/receteler", StringComparison.OrdinalIgnoreCase);
}

static async Task EnsureSensitiveUploadsMigratedAsync(KanvasDbContext context, IWebHostEnvironment env, Microsoft.Extensions.Logging.ILogger logger)
{
    var changed = false;
    var deleteAfterSave = new List<string>();

    var users = await context.Users
        .Where(x => x.KimlikFotografYolu != null && x.KimlikFotografYolu.StartsWith("/uploads/kimlikler/"))
        .ToListAsync();

    foreach (var user in users)
    {
        var migrated = TryCopyLegacySensitiveFile(
            env,
            user.KimlikFotografYolu,
            "uploads/kimlikler",
            HassasBelgeKategorisi.Kimlik,
            out var privateReference,
            out var oldPath);

        if (migrated)
        {
            user.KimlikFotografYolu = privateReference;
            changed = true;
            if (!string.IsNullOrWhiteSpace(oldPath)) deleteAfterSave.Add(oldPath);
        }
    }

    var siparisler = await context.Siparisler
        .Where(x =>
            (x.KimlikFotoYolu != null && x.KimlikFotoYolu.StartsWith("/uploads/kimlikler/")) ||
            (x.ReceteDosyaYolu != null && x.ReceteDosyaYolu.StartsWith("/uploads/receteler/")))
        .ToListAsync();

    foreach (var siparis in siparisler)
    {
        if (TryCopyLegacySensitiveFile(
            env,
            siparis.KimlikFotoYolu,
            "uploads/kimlikler",
            HassasBelgeKategorisi.Kimlik,
            out var privateKimlikReference,
            out var oldKimlikPath))
        {
            siparis.KimlikFotoYolu = privateKimlikReference;
            changed = true;
            if (!string.IsNullOrWhiteSpace(oldKimlikPath)) deleteAfterSave.Add(oldKimlikPath);
        }

        if (TryCopyLegacySensitiveFile(
            env,
            siparis.ReceteDosyaYolu,
            "uploads/receteler",
            HassasBelgeKategorisi.Recete,
            out var privateReceteReference,
            out var oldRecetePath))
        {
            siparis.ReceteDosyaYolu = privateReceteReference;
            changed = true;
            if (!string.IsNullOrWhiteSpace(oldRecetePath)) deleteAfterSave.Add(oldRecetePath);
        }
    }

    if (!changed)
    {
        return;
    }

    await context.SaveChangesAsync();

    foreach (var oldPath in deleteAfterSave.Distinct(StringComparer.OrdinalIgnoreCase))
    {
        try
        {
            if (File.Exists(oldPath))
            {
                File.Delete(oldPath);
            }
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Legacy hassas dosya silinemedi. Dosya public URL ile servis edilmeyecek.");
        }
    }

    logger.LogInformation("Legacy hassas upload path migration tamamlandi. KayitSayisi={Count}", deleteAfterSave.Count);
}

static bool TryCopyLegacySensitiveFile(
    IWebHostEnvironment env,
    string? legacyReference,
    string expectedFolder,
    HassasBelgeKategorisi kategori,
    out string privateReference,
    out string? oldPath)
{
    privateReference = string.Empty;
    oldPath = null;

    if (string.IsNullOrWhiteSpace(legacyReference) || !legacyReference.StartsWith("/" + expectedFolder + "/", StringComparison.OrdinalIgnoreCase))
    {
        return false;
    }

    var fileName = Path.GetFileName(legacyReference.Replace('\\', '/'));
    if (!DosyaServisi.IsSafeStoredFileName(fileName))
    {
        return false;
    }

    var legacyRoot = Path.GetFullPath(Path.Combine(env.WebRootPath, expectedFolder));
    var legacyPath = Path.GetFullPath(Path.Combine(legacyRoot, fileName));
    if (!legacyPath.StartsWith(legacyRoot, StringComparison.OrdinalIgnoreCase) || !File.Exists(legacyPath))
    {
        return false;
    }

    var extension = Path.GetExtension(fileName).ToLowerInvariant();
    var secureRoot = Path.GetFullPath(Path.Combine(env.ContentRootPath, "secure-storage", "hassas", DosyaServisi.GetCategorySegment(kategori)));
    Directory.CreateDirectory(secureRoot);

    var newFileName = Guid.NewGuid().ToString("N") + extension;
    var securePath = Path.GetFullPath(Path.Combine(secureRoot, newFileName));
    if (!securePath.StartsWith(secureRoot, StringComparison.OrdinalIgnoreCase))
    {
        return false;
    }

    File.Copy(legacyPath, securePath, overwrite: false);
    privateReference = DosyaServisi.BuildPrivateReference(kategori, newFileName);
    oldPath = legacyPath;
    return true;
}

static bool IsMaintenanceAllowedPath(PathString path)
{
    var value = path.Value ?? string.Empty;

    return value.StartsWith("/admin", StringComparison.OrdinalIgnoreCase)
        || value.StartsWith("/hesap", StringComparison.OrdinalIgnoreCase)
        || value.StartsWith("/api/admin", StringComparison.OrdinalIgnoreCase);
}

static bool IsAdminSecuredPath(PathString path)
{
    var value = path.Value ?? string.Empty;

    return value.StartsWith("/admin", StringComparison.OrdinalIgnoreCase)
        || value.StartsWith("/api/admin", StringComparison.OrdinalIgnoreCase);
}

static bool IsAdminApiPath(PathString path)
{
    var value = path.Value ?? string.Empty;

    return value.StartsWith("/api/admin", StringComparison.OrdinalIgnoreCase);
}

static async Task HandleAuthRedirectAsync(
    RedirectContext<CookieAuthenticationOptions> context,
    int statusCode,
    string eventType,
    string message)
{
    if (IsAdminSecuredPath(context.Request.Path))
    {
        var auditService = context.HttpContext.RequestServices.GetService<IAdminSecurityAuditService>();
        if (auditService != null)
        {
            await auditService.LogAsync(
                context.HttpContext,
                eventType,
                message,
                context.Request.Path.Value);
        }

        if (IsAdminApiPath(context.Request.Path))
        {
            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsJsonAsync(new
            {
                error = statusCode == StatusCodes.Status401Unauthorized ? "auth_required" : "forbidden"
            });
            return;
        }
    }

    context.Response.Redirect(context.RedirectUri);
}

static bool CanConnectToPostgres(string? connectionString, out string? errorMessage)
{
    errorMessage = null;

    if (string.IsNullOrWhiteSpace(connectionString))
    {
        errorMessage = "DefaultConnection ayarlanmamis.";
        return false;
    }

    try
    {
        using var connection = new NpgsqlConnection(connectionString);
        connection.Open();
        return true;
    }
    catch (Exception ex)
    {
        errorMessage = ex.Message;
        return false;
    }
}

static async Task EnsureKnownSchemaDriftAsync(KanvasDbContext context, Microsoft.Extensions.Logging.ILogger<Program> logger)
{
    const string sql = """
CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

DO $$
BEGIN
    IF to_regclass('public."BultenAbonelikleri"') IS NULL THEN
        CREATE TABLE "BultenAbonelikleri" (
            "Id" integer GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
            "Email" text NOT NULL,
            "KayitTarihi" timestamp with time zone NOT NULL,
            "AktifMi" boolean NOT NULL
        );
    END IF;

    IF to_regclass('public."BultenAbonelikleri"') IS NOT NULL THEN
        ALTER TABLE "BultenAbonelikleri" ADD COLUMN IF NOT EXISTS "IpAdresi" text NULL;
    END IF;

    IF to_regclass('public."Urunler"') IS NOT NULL AND EXISTS (
        SELECT 1 FROM information_schema.columns 
        WHERE table_name = 'Urunler' AND column_name = 'Slug'
    ) THEN
        ALTER TABLE "Urunler" ADD COLUMN IF NOT EXISTS "SeoTitle" text NOT NULL DEFAULT '';
        ALTER TABLE "Urunler" ADD COLUMN IF NOT EXISTS "SeoDescription" text NOT NULL DEFAULT '';
        ALTER TABLE "Urunler" ADD COLUMN IF NOT EXISTS "SeoKeywords" text NOT NULL DEFAULT '';
        EXECUTE 'CREATE INDEX IF NOT EXISTS "IX_Urunler_Slug" ON "Urunler" ("Slug") WHERE "Slug" IS NOT NULL AND "Slug" <> ''''';
    END IF;

    IF to_regclass('public."Kategoriler"') IS NOT NULL AND EXISTS (
        SELECT 1 FROM information_schema.columns 
        WHERE table_name = 'Kategoriler' AND column_name = 'Slug'
    ) THEN
        EXECUTE 'CREATE INDEX IF NOT EXISTS "IX_Kategoriler_Slug" ON "Kategoriler" ("Slug") WHERE "Slug" IS NOT NULL AND "Slug" <> ''''';
    END IF;

    IF to_regclass('public."Siparisler"') IS NOT NULL THEN
        ALTER TABLE "Siparisler" ADD COLUMN IF NOT EXISTS "KargoFirmasi" text NULL;
        ALTER TABLE "Siparisler" ADD COLUMN IF NOT EXISTS "KargoFirmasiId" integer NULL;
    END IF;

    IF to_regclass('public."KargoFirmalari"') IS NULL THEN
        CREATE TABLE "KargoFirmalari" (
            "Id" integer GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
            "Ad" text NOT NULL,
            "Kod" text NOT NULL,
            "LogoUrl" text NULL,
            "Telefon" text NULL,
            "TakipUrl" text NULL,
            "GondericiUnvan" text NOT NULL DEFAULT '7ANRPS48',
            "GondericiAdres" text NOT NULL DEFAULT '',
            "GondericiTelefon" text NOT NULL DEFAULT '',
            "AktifMi" boolean NOT NULL DEFAULT true,
            "VarsayilanMi" boolean NOT NULL DEFAULT false,
            "Fiyat" numeric NOT NULL DEFAULT 0,
            "OlusturulmaTarihi" timestamp with time zone NOT NULL DEFAULT NOW(),
            "SilindiMi" boolean NOT NULL DEFAULT false
        );
    END IF;

    IF to_regclass('public."KargoFirmalari"') IS NOT NULL THEN
        EXECUTE 'CREATE UNIQUE INDEX IF NOT EXISTS "IX_KargoFirmalari_Kod" ON "KargoFirmalari" ("Kod")';
        ALTER TABLE "KargoFirmalari" ADD COLUMN IF NOT EXISTS "Fiyat" numeric NOT NULL DEFAULT 0;
    END IF;

    IF to_regclass('public."KargoBolgeler"') IS NULL THEN
        CREATE TABLE "KargoBolgeler" (
            "Id" integer GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
            "Ad" text NOT NULL,
            "Sira" integer NOT NULL DEFAULT 0,
            "OlusturulmaTarihi" timestamp with time zone NOT NULL DEFAULT NOW(),
            "SilindiMi" boolean NOT NULL DEFAULT false
        );
    END IF;

    IF to_regclass('public."KargoBolgeSehirler"') IS NULL THEN
        CREATE TABLE "KargoBolgeSehirler" (
            "Id" integer GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
            "BolgeId" integer NOT NULL REFERENCES "KargoBolgeler"("Id") ON DELETE CASCADE,
            "SehirAdi" text NOT NULL,
            "OlusturulmaTarihi" timestamp with time zone NOT NULL DEFAULT NOW(),
            "SilindiMi" boolean NOT NULL DEFAULT false
        );
        CREATE UNIQUE INDEX IF NOT EXISTS "IX_KargoBolgeSehirler_BolgeId_SehirAdi" ON "KargoBolgeSehirler" ("BolgeId", "SehirAdi");
    END IF;

    IF to_regclass('public."KargoBolgeSehirler"') IS NOT NULL THEN
        ALTER TABLE "KargoBolgeSehirler" ADD COLUMN IF NOT EXISTS "SehirAdiEn" text NULL;
        ALTER TABLE "KargoBolgeSehirler" ADD COLUMN IF NOT EXISTS "SehirAdiAr" text NULL;
    END IF;

    IF to_regclass('public."KargoBolgeFiyatlari"') IS NULL THEN
        CREATE TABLE "KargoBolgeFiyatlari" (
            "Id" integer GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
            "KargoFirmasiId" integer NOT NULL REFERENCES "KargoFirmalari"("Id") ON DELETE CASCADE,
            "BolgeId" integer NOT NULL REFERENCES "KargoBolgeler"("Id") ON DELETE CASCADE,
            "Fiyat" numeric NOT NULL DEFAULT 0,
            "OlusturulmaTarihi" timestamp with time zone NOT NULL DEFAULT NOW(),
            "SilindiMi" boolean NOT NULL DEFAULT false
        );
        CREATE UNIQUE INDEX IF NOT EXISTS "IX_KargoBolgeFiyatlari_KargoFirmasiId_BolgeId" ON "KargoBolgeFiyatlari" ("KargoFirmasiId", "BolgeId");
    END IF;

    IF to_regclass('public."SepetItems"') IS NOT NULL THEN
        ALTER TABLE "SepetItems" ADD COLUMN IF NOT EXISTS "MusteriNotu" character varying(500) NULL;
    END IF;


    IF to_regclass('public."SiparisDetaylari"') IS NOT NULL THEN
        IF EXISTS (
            SELECT 1 FROM information_schema.columns
            WHERE table_name = 'SiparisDetaylari' AND column_name = 'siparisId'
        ) AND NOT EXISTS (
            SELECT 1 FROM information_schema.columns
            WHERE table_name = 'SiparisDetaylari' AND column_name = 'SiparisId'
        ) THEN
            ALTER TABLE "SiparisDetaylari" RENAME COLUMN "siparisId" TO "SiparisId";
        END IF;

        ALTER TABLE "SiparisDetaylari" ADD COLUMN IF NOT EXISTS "MusteriNotu" character varying(500) NULL;
        ALTER TABLE "SiparisDetaylari" ALTER COLUMN "UrunSecenekId" DROP NOT NULL;
    END IF;
END
$$;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
SELECT '20260131211352_BultenTablosu', '8.0.4'
WHERE NOT EXISTS (SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260131211352_BultenTablosu')
  AND EXISTS (SELECT 1 FROM information_schema.tables WHERE table_name = 'BultenAbonelikleri');

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
SELECT '20260131213049_BultenIpEklendi', '8.0.4'
WHERE NOT EXISTS (SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260131213049_BultenIpEklendi')
  AND EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name = 'BultenAbonelikleri' AND column_name = 'IpAdresi');

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
SELECT '20260504111249_AddProductSeoFields', '8.0.4'
WHERE NOT EXISTS (SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260504111249_AddProductSeoFields')
  AND EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name = 'Urunler' AND column_name = 'SeoTitle')
  AND EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name = 'Urunler' AND column_name = 'SeoDescription')
  AND EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name = 'Urunler' AND column_name = 'SeoKeywords')
  AND EXISTS (SELECT 1 FROM information_schema.tables WHERE table_name = 'KargoFirmalari');

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
SELECT '20260508175141_MusteriNotuAlanlariEklendi', '8.0.4'
WHERE NOT EXISTS (SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260508175141_MusteriNotuAlanlariEklendi')
  AND EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name = 'SepetItems' AND column_name = 'MusteriNotu')
  AND EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name = 'SiparisDetaylari' AND column_name = 'MusteriNotu');

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
SELECT '20260508204726_Fix_NullableUrunSecenekId_And_SlugIndex', '8.0.4'
WHERE NOT EXISTS (SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260508204726_Fix_NullableUrunSecenekId_And_SlugIndex')
  AND EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name = 'SiparisDetaylari' AND column_name = 'UrunSecenekId' AND is_nullable = 'YES');

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
SELECT '20260523223159_AddFavoriPriceDropFields', '8.0.4'
WHERE NOT EXISTS (SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260523223159_AddFavoriPriceDropFields')
  AND EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name = 'Favoriler' AND column_name = 'FiyatDustugundaBildir')
  AND EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name = 'Favoriler' AND column_name = 'EskiFiyat')
  AND EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name = 'Favoriler' AND column_name = 'SonBildirimTarihi');

""";

    await context.Database.ExecuteSqlRawAsync(sql);
    logger.LogInformation("Bilinen schema drift kontrolleri tamamlandi.");
}

static async Task EnsureMissingMarch2026SchemaAsync(KanvasDbContext context, Microsoft.Extensions.Logging.ILogger<Program> logger)
{
    const string sql = """
DO $$
BEGIN
    ALTER TABLE "Kategoriler" ADD COLUMN IF NOT EXISTS "AltMetin" text NOT NULL DEFAULT '';
    ALTER TABLE "Kategoriler" ADD COLUMN IF NOT EXISTS "ReceteGerekliMi" boolean NOT NULL DEFAULT false;
    ALTER TABLE "Kategoriler" ADD COLUMN IF NOT EXISTS "BannerUrl" text NULL;
    ALTER TABLE "Kategoriler" ADD COLUMN IF NOT EXISTS "MenuGorselUrl" text NULL;
    ALTER TABLE "Kategoriler" ADD COLUMN IF NOT EXISTS "KampanyaEtiketi" text NOT NULL DEFAULT '';
    ALTER TABLE "Kategoriler" ADD COLUMN IF NOT EXISTS "KisaAciklama" text NOT NULL DEFAULT '';
    ALTER TABLE "Kategoriler" ADD COLUMN IF NOT EXISTS "ParentKategoriId" integer NULL;
    ALTER TABLE "Kategoriler" ADD COLUMN IF NOT EXISTS "SeoDescription" text NOT NULL DEFAULT '';
    ALTER TABLE "Kategoriler" ADD COLUMN IF NOT EXISTS "SeoTitle" text NOT NULL DEFAULT '';
    ALTER TABLE "Kategoriler" ADD COLUMN IF NOT EXISTS "Sira" integer NOT NULL DEFAULT 0;
    ALTER TABLE "Kategoriler" ADD COLUMN IF NOT EXISTS "Slug" text NULL;
    ALTER TABLE "Kategoriler" ADD COLUMN IF NOT EXISTS "UrunSiralamaTipi" text NOT NULL DEFAULT 'manual';
    ALTER TABLE "Kategoriler" ADD COLUMN IF NOT EXISTS "UstMetin" text NOT NULL DEFAULT '';
    ALTER TABLE "Kategoriler" ADD COLUMN IF NOT EXISTS "AdEn" text NOT NULL DEFAULT '';
    ALTER TABLE "Kategoriler" ADD COLUMN IF NOT EXISTS "AdAr" text NOT NULL DEFAULT '';
    ALTER TABLE "Kategoriler" ADD COLUMN IF NOT EXISTS "KisaAciklamaEn" text NOT NULL DEFAULT '';
    ALTER TABLE "Kategoriler" ADD COLUMN IF NOT EXISTS "KisaAciklamaAr" text NOT NULL DEFAULT '';
    ALTER TABLE "Kategoriler" ADD COLUMN IF NOT EXISTS "AciklamaEn" text NOT NULL DEFAULT '';
    ALTER TABLE "Kategoriler" ADD COLUMN IF NOT EXISTS "AciklamaAr" text NOT NULL DEFAULT '';
    ALTER TABLE "Kategoriler" ADD COLUMN IF NOT EXISTS "SeoTitleEn" text NOT NULL DEFAULT '';
    ALTER TABLE "Kategoriler" ADD COLUMN IF NOT EXISTS "SeoTitleAr" text NOT NULL DEFAULT '';
    ALTER TABLE "Kategoriler" ADD COLUMN IF NOT EXISTS "SeoDescriptionEn" text NOT NULL DEFAULT '';
    ALTER TABLE "Kategoriler" ADD COLUMN IF NOT EXISTS "SeoDescriptionAr" text NOT NULL DEFAULT '';
    ALTER TABLE "Kategoriler" ADD COLUMN IF NOT EXISTS "UstMetinEn" text NOT NULL DEFAULT '';
    ALTER TABLE "Kategoriler" ADD COLUMN IF NOT EXISTS "UstMetinAr" text NOT NULL DEFAULT '';
    ALTER TABLE "Kategoriler" ADD COLUMN IF NOT EXISTS "AltMetinEn" text NOT NULL DEFAULT '';
    ALTER TABLE "Kategoriler" ADD COLUMN IF NOT EXISTS "AltMetinAr" text NOT NULL DEFAULT '';
    ALTER TABLE "Kategoriler" ADD COLUMN IF NOT EXISTS "KampanyaEtiketiEn" text NOT NULL DEFAULT '';
    ALTER TABLE "Kategoriler" ADD COLUMN IF NOT EXISTS "KampanyaEtiketiAr" text NOT NULL DEFAULT '';

    ALTER TABLE "Slaytlar" ADD COLUMN IF NOT EXISTS "BaslikEn" text NOT NULL DEFAULT '';
    ALTER TABLE "Slaytlar" ADD COLUMN IF NOT EXISTS "BaslikAr" text NOT NULL DEFAULT '';
    ALTER TABLE "Slaytlar" ADD COLUMN IF NOT EXISTS "AltBaslikEn" text NOT NULL DEFAULT '';
    ALTER TABLE "Slaytlar" ADD COLUMN IF NOT EXISTS "AltBaslikAr" text NOT NULL DEFAULT '';
    ALTER TABLE "Slaytlar" ADD COLUMN IF NOT EXISTS "AciklamaEn" text NOT NULL DEFAULT '';
    ALTER TABLE "Slaytlar" ADD COLUMN IF NOT EXISTS "AciklamaAr" text NOT NULL DEFAULT '';
    ALTER TABLE "Slaytlar" ADD COLUMN IF NOT EXISTS "ButonMetni" text NOT NULL DEFAULT '';
    ALTER TABLE "Slaytlar" ADD COLUMN IF NOT EXISTS "ButonMetniEn" text NOT NULL DEFAULT '';
    ALTER TABLE "Slaytlar" ADD COLUMN IF NOT EXISTS "ButonMetniAr" text NOT NULL DEFAULT '';
    ALTER TABLE "Slaytlar" ADD COLUMN IF NOT EXISTS "BaglantiUrl" text NULL;

    ALTER TABLE "Urunler" ADD COLUMN IF NOT EXISTS "UrunTipi" text NOT NULL DEFAULT 'Genel';
    ALTER TABLE "Urunler" ADD COLUMN IF NOT EXISTS "AktifMi" boolean NOT NULL DEFAULT true;
    ALTER TABLE "Urunler" ADD COLUMN IF NOT EXISTS "YayindaMi" boolean NOT NULL DEFAULT true;
    ALTER TABLE "Urunler" ADD COLUMN IF NOT EXISTS "AnaSayfadaGoster" boolean NOT NULL DEFAULT false;
    ALTER TABLE "Urunler" ADD COLUMN IF NOT EXISTS "BakimTalimati" text NOT NULL DEFAULT '';
    ALTER TABLE "Urunler" ADD COLUMN IF NOT EXISTS "Barkod" text NOT NULL DEFAULT '';
    ALTER TABLE "Urunler" ADD COLUMN IF NOT EXISTS "Etiketler" text NOT NULL DEFAULT '';
    ALTER TABLE "Urunler" ADD COLUMN IF NOT EXISTS "FavoriSayisi" integer NOT NULL DEFAULT 0;
    ALTER TABLE "Urunler" ADD COLUMN IF NOT EXISTS "Fiyat" numeric NOT NULL DEFAULT 0;
    ALTER TABLE "Urunler" ADD COLUMN IF NOT EXISTS "GoruntulenmeSayisi" integer NOT NULL DEFAULT 0;
    ALTER TABLE "Urunler" ADD COLUMN IF NOT EXISTS "IndirimliFiyat" numeric NULL;
    ALTER TABLE "Urunler" ADD COLUMN IF NOT EXISTS "KargoyaVerilisSuresiGun" integer NOT NULL DEFAULT 0;
    ALTER TABLE "Urunler" ADD COLUMN IF NOT EXISTS "KisaAciklama" text NOT NULL DEFAULT '';
    ALTER TABLE "Urunler" ADD COLUMN IF NOT EXISTS "KisaAd" text NOT NULL DEFAULT '';
    ALTER TABLE "Urunler" ADD COLUMN IF NOT EXISTS "KdvOrani" numeric NOT NULL DEFAULT 20;
    ALTER TABLE "Urunler" ADD COLUMN IF NOT EXISTS "KampanyaliMi" boolean NOT NULL DEFAULT false;
    ALTER TABLE "Urunler" ADD COLUMN IF NOT EXISTS "Maliyet" numeric NOT NULL DEFAULT 0;
    ALTER TABLE "Urunler" ADD COLUMN IF NOT EXISTS "MalzemeBilgisi" text NOT NULL DEFAULT '';
    ALTER TABLE "Urunler" ADD COLUMN IF NOT EXISTS "Marka" text NOT NULL DEFAULT '';
    ALTER TABLE "Urunler" ADD COLUMN IF NOT EXISTS "MaxSiparisAdedi" integer NULL;
    ALTER TABLE "Urunler" ADD COLUMN IF NOT EXISTS "MinSiparisAdedi" integer NOT NULL DEFAULT 1;
    ALTER TABLE "Urunler" ADD COLUMN IF NOT EXISTS "OneCikanMi" boolean NOT NULL DEFAULT false;
    ALTER TABLE "Urunler" ADD COLUMN IF NOT EXISTS "PaketlemeBilgisi" text NOT NULL DEFAULT '';
    ALTER TABLE "Urunler" ADD COLUMN IF NOT EXISTS "SatisSayisi" integer NOT NULL DEFAULT 0;
    ALTER TABLE "Urunler" ADD COLUMN IF NOT EXISTS "SKU" text NOT NULL DEFAULT '';
    ALTER TABLE "Urunler" ADD COLUMN IF NOT EXISTS "Sira" integer NOT NULL DEFAULT 0;
    ALTER TABLE "Urunler" ADD COLUMN IF NOT EXISTS "StokDurumu" text NOT NULL DEFAULT 'Stokta';
    ALTER TABLE "Urunler" ADD COLUMN IF NOT EXISTS "TeknikOzellikler" text NOT NULL DEFAULT '';
    ALTER TABLE "Urunler" ADD COLUMN IF NOT EXISTS "TahminiTeslimSuresiGun" integer NOT NULL DEFAULT 0;
    ALTER TABLE "Urunler" ADD COLUMN IF NOT EXISTS "UretimSuresiGun" integer NOT NULL DEFAULT 0;
    ALTER TABLE "Urunler" ADD COLUMN IF NOT EXISTS "YeniUrunMu" boolean NOT NULL DEFAULT false;
    ALTER TABLE "Urunler" ADD COLUMN IF NOT EXISTS "TopFiyat" numeric NULL;
    ALTER TABLE "Urunler" ADD COLUMN IF NOT EXISTS "BaslikEn" text NOT NULL DEFAULT '';
    ALTER TABLE "Urunler" ADD COLUMN IF NOT EXISTS "BaslikAr" text NOT NULL DEFAULT '';
    ALTER TABLE "Urunler" ADD COLUMN IF NOT EXISTS "KisaAciklamaEn" text NOT NULL DEFAULT '';
    ALTER TABLE "Urunler" ADD COLUMN IF NOT EXISTS "KisaAciklamaAr" text NOT NULL DEFAULT '';
    ALTER TABLE "Urunler" ADD COLUMN IF NOT EXISTS "AciklamaEn" text NOT NULL DEFAULT '';
    ALTER TABLE "Urunler" ADD COLUMN IF NOT EXISTS "AciklamaAr" text NOT NULL DEFAULT '';
    ALTER TABLE "Urunler" ADD COLUMN IF NOT EXISTS "SeoTitleEn" text NOT NULL DEFAULT '';
    ALTER TABLE "Urunler" ADD COLUMN IF NOT EXISTS "SeoTitleAr" text NOT NULL DEFAULT '';
    ALTER TABLE "Urunler" ADD COLUMN IF NOT EXISTS "SeoDescriptionEn" text NOT NULL DEFAULT '';
    ALTER TABLE "Urunler" ADD COLUMN IF NOT EXISTS "SeoDescriptionAr" text NOT NULL DEFAULT '';
    ALTER TABLE "Urunler" ADD COLUMN IF NOT EXISTS "WhatsappSiparisVarMi" boolean NOT NULL DEFAULT false;
    ALTER TABLE "Urunler" ADD COLUMN IF NOT EXISTS "FiyatGizliMi" boolean NOT NULL DEFAULT false;
    ALTER TABLE "Urunler" ADD COLUMN IF NOT EXISTS "KampanyaBitisTarihi" timestamp;

    ALTER TABLE "UrunSecenekleri" ADD COLUMN IF NOT EXISTS "AktifMi" boolean NOT NULL DEFAULT true;
    ALTER TABLE "UrunSecenekleri" ADD COLUMN IF NOT EXISTS "CerceveKalinligi" text NOT NULL DEFAULT '';
    ALTER TABLE "UrunSecenekleri" ADD COLUMN IF NOT EXISTS "CerceveRengi" text NOT NULL DEFAULT '';
    ALTER TABLE "UrunSecenekleri" ADD COLUMN IF NOT EXISTS "Desi" numeric NOT NULL DEFAULT 0;
    ALTER TABLE "UrunSecenekleri" ADD COLUMN IF NOT EXISTS "FiyatFarki" numeric NOT NULL DEFAULT 0;
    ALTER TABLE "UrunSecenekleri" ADD COLUMN IF NOT EXISTS "GorselUrl" text NOT NULL DEFAULT '';
    ALTER TABLE "UrunSecenekleri" ADD COLUMN IF NOT EXISTS "KisilestirmeMetni" text NOT NULL DEFAULT '';
    ALTER TABLE "UrunSecenekleri" ADD COLUMN IF NOT EXISTS "MalzemeTuru" text NOT NULL DEFAULT '';
    ALTER TABLE "UrunSecenekleri" ADD COLUMN IF NOT EXISTS "OnSipariseAcikMi" boolean NOT NULL DEFAULT false;
    ALTER TABLE "UrunSecenekleri" ADD COLUMN IF NOT EXISTS "OzelTasarimNotu" text NOT NULL DEFAULT '';
    ALTER TABLE "UrunSecenekleri" ADD COLUMN IF NOT EXISTS "ParcaSayisi" integer NOT NULL DEFAULT 1;
    ALTER TABLE "UrunSecenekleri" ADD COLUMN IF NOT EXISTS "Sira" integer NOT NULL DEFAULT 0;
    ALTER TABLE "UrunSecenekleri" ADD COLUMN IF NOT EXISTS "TukeninceGizle" boolean NOT NULL DEFAULT false;
    ALTER TABLE "UrunSecenekleri" ADD COLUMN IF NOT EXISTS "UretimSuresiGun" integer NOT NULL DEFAULT 0;
    ALTER TABLE "UrunSecenekleri" ADD COLUMN IF NOT EXISTS "VaryantSku" text NOT NULL DEFAULT '';
    ALTER TABLE "UrunSecenekleri" ADD COLUMN IF NOT EXISTS "VarsayilanMi" boolean NOT NULL DEFAULT false;
    ALTER TABLE "UrunSecenekleri" ADD COLUMN IF NOT EXISTS "Yon" text NOT NULL DEFAULT '';

    ALTER TABLE "SepetItems" ADD COLUMN IF NOT EXISTS "CerceveModeli" text NOT NULL DEFAULT '';
    ALTER TABLE "SepetItems" ADD COLUMN IF NOT EXISTS "HediyePaketi" boolean NOT NULL DEFAULT false;
    ALTER TABLE "SepetItems" ADD COLUMN IF NOT EXISTS "HediyePaketFiyati" numeric NOT NULL DEFAULT 0;
    ALTER TABLE "SiparisDetaylari" ADD COLUMN IF NOT EXISTS "CerceveModeli" text NOT NULL DEFAULT '';
    ALTER TABLE "SiparisDetaylari" ADD COLUMN IF NOT EXISTS "HediyePaketi" boolean NOT NULL DEFAULT false;
    ALTER TABLE "SiparisDetaylari" ADD COLUMN IF NOT EXISTS "HediyePaketFiyati" numeric NOT NULL DEFAULT 0;

    ALTER TABLE "Siparisler" ADD COLUMN IF NOT EXISTS "KimlikFotoYolu" text NULL;
    ALTER TABLE "Siparisler" ADD COLUMN IF NOT EXISTS "OdemeYontemi" text NOT NULL DEFAULT 'BankaHavalesi';
    ALTER TABLE "Siparisler" ADD COLUMN IF NOT EXISTS "KapidaOdemeHizmetBedeli" numeric NOT NULL DEFAULT 0;
    ALTER TABLE "Siparisler" ADD COLUMN IF NOT EXISTS "ReceteDosyaYolu" text NULL;
    ALTER TABLE "Siparisler" ADD COLUMN IF NOT EXISTS "ReceteOnayDurumu" integer NOT NULL DEFAULT 0;
    ALTER TABLE "Siparisler" ADD COLUMN IF NOT EXISTS "ReceteRedSebebi" text NULL;
    ALTER TABLE "Siparisler" ADD COLUMN IF NOT EXISTS "TeslimatTipi" text NOT NULL DEFAULT 'AdreseTeslim';

    CREATE TABLE IF NOT EXISTS "BankaHesaplari" (
        "Id" integer GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
        "BankaAdi" text NOT NULL,
        "HesapSahibi" text NOT NULL,
        "IBAN" text NOT NULL,
        "SubeKodu" text NULL,
        "HesapNo" text NULL,
        "AktifMi" boolean NOT NULL DEFAULT true,
        "Sira" integer NOT NULL DEFAULT 0,
        "OlusturulmaTarihi" timestamp with time zone NOT NULL,
        "SilindiMi" boolean NOT NULL DEFAULT false
    );

    ALTER TABLE "Urunler" ADD COLUMN IF NOT EXISTS "HediyePaketiVarMi" boolean NOT NULL DEFAULT false;
    ALTER TABLE "Urunler" ADD COLUMN IF NOT EXISTS "HediyePaketFiyati" numeric NOT NULL DEFAULT 0;

    CREATE TABLE IF NOT EXISTS "UrunOzellikTanimlari" (
        "Id" integer GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
        "Ad" text NOT NULL,
        "Kod" text NOT NULL,
        "UrunTipi" text NOT NULL,
        "AlanTipi" text NOT NULL,
        "YardimMetni" text NOT NULL,
        "Secenekler" text NOT NULL,
        "FiltredeGoster" boolean NOT NULL,
        "DetaySayfasindaGoster" boolean NOT NULL,
        "TeknikTablodaGoster" boolean NOT NULL,
        "AktifMi" boolean NOT NULL,
        "Sira" integer NOT NULL,
        "OlusturulmaTarihi" timestamp with time zone NOT NULL,
        "SilindiMi" boolean NOT NULL
    );

    CREATE TABLE IF NOT EXISTS "UrunOzellikDegerleri" (
        "Id" integer GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
        "UrunId" integer NOT NULL REFERENCES "Urunler"("Id") ON DELETE CASCADE,
        "UrunOzellikTanimiId" integer NOT NULL REFERENCES "UrunOzellikTanimlari"("Id") ON DELETE CASCADE,
        "Deger" text NOT NULL,
        "OlusturulmaTarihi" timestamp with time zone NOT NULL,
        "SilindiMi" boolean NOT NULL
    );

    ALTER TABLE "UrunResimleri" ADD COLUMN IF NOT EXISTS "AltMetin" text NOT NULL DEFAULT '';
    ALTER TABLE "UrunResimleri" ADD COLUMN IF NOT EXISTS "Baslik" text NOT NULL DEFAULT '';
    ALTER TABLE "UrunResimleri" ADD COLUMN IF NOT EXISTS "Etiketler" text NOT NULL DEFAULT '';
    ALTER TABLE "UrunResimleri" ADD COLUMN IF NOT EXISTS "MedyaAlani" text NOT NULL DEFAULT 'Galeri';
    ALTER TABLE "UrunResimleri" ADD COLUMN IF NOT EXISTS "MedyaTipi" text NOT NULL DEFAULT 'Gorsel';
    ALTER TABLE "UrunResimleri" ADD COLUMN IF NOT EXISTS "MobilResimYolu" text NOT NULL DEFAULT '';
    ALTER TABLE "UrunResimleri" ADD COLUMN IF NOT EXISTS "Sira" integer NOT NULL DEFAULT 0;
    ALTER TABLE "UrunResimleri" ADD COLUMN IF NOT EXISTS "ThumbnailYolu" text NOT NULL DEFAULT '';
    ALTER TABLE "UrunResimleri" ADD COLUMN IF NOT EXISTS "UrunSecenekId" integer NULL;
    ALTER TABLE "UrunResimleri" ADD COLUMN IF NOT EXISTS "VideoUrl" text NOT NULL DEFAULT '';

    ALTER TABLE "AspNetUsers" ADD COLUMN IF NOT EXISTS "Adres" text NOT NULL DEFAULT '';
    ALTER TABLE "AspNetUsers" ADD COLUMN IF NOT EXISTS "WholesaleStatus" integer NOT NULL DEFAULT 0;
    ALTER TABLE "AspNetUsers" ADD COLUMN IF NOT EXISTS "BasvuruTarihi" timestamp with time zone NULL;

    ALTER TABLE "SiteAyarlari" ADD COLUMN IF NOT EXISTS "GirisZorunluMu" boolean NOT NULL DEFAULT false;
    ALTER TABLE "SiteAyarlari" ADD COLUMN IF NOT EXISTS "StokBiteniGriGoster" boolean NOT NULL DEFAULT true;
    ALTER TABLE "SiteAyarlari" ADD COLUMN IF NOT EXISTS "KapidaOdemeAktifMi" boolean NOT NULL DEFAULT true;
    ALTER TABLE "SiteAyarlari" ADD COLUMN IF NOT EXISTS "KapidaOdemeHizmetBedeli" numeric NOT NULL DEFAULT 15;
    ALTER TABLE "SiteAyarlari" ADD COLUMN IF NOT EXISTS "KapidaOdemeLimiti" numeric NOT NULL DEFAULT 2000;
    ALTER TABLE "SiteAyarlari" ADD COLUMN IF NOT EXISTS "ToptanciMinSiparisTutari" numeric NOT NULL DEFAULT 500;
    ALTER TABLE "SiteAyarlari" ADD COLUMN IF NOT EXISTS "IptalSuresiSaat" integer NOT NULL DEFAULT 3;
    ALTER TABLE "SiteAyarlari" ADD COLUMN IF NOT EXISTS "FooterAciklamasiEn" text NOT NULL DEFAULT 'A Palestinian e-commerce site offering varied products at competitive prices with fast delivery to all cities.';
    ALTER TABLE "SiteAyarlari" ADD COLUMN IF NOT EXISTS "FooterAciklamasiAr" text NOT NULL DEFAULT 'متجر إلكتروني فلسطيني يقدم منتجات متنوعة بأسعار منافسة وتوصيل سريع لجميع المدن';
    ALTER TABLE "SiteAyarlari" ADD COLUMN IF NOT EXISTS "FooterAciklamasiTr" text NOT NULL DEFAULT 'Rekabetçi fiyatlarla çeşitli ürünler sunan ve tüm şehirlere hızlı teslimat yapan bir Filistin e-ticaret sitesi.';
    ALTER TABLE "SiteAyarlari" ADD COLUMN IF NOT EXISTS "HeroBaslikAr" text NOT NULL DEFAULT 'جلب الفن الفلسطيني إلى منزلك';
    ALTER TABLE "SiteAyarlari" ADD COLUMN IF NOT EXISTS "HeroBaslikEn" text NOT NULL DEFAULT 'Bring Palestinian Art to Your Home';
    ALTER TABLE "SiteAyarlari" ADD COLUMN IF NOT EXISTS "HeroBaslikTr" text NOT NULL DEFAULT 'Filistin Sanatını Evinize Getirin';
    ALTER TABLE "SiteAyarlari" ADD COLUMN IF NOT EXISTS "HeroAltBaslikAr" text NOT NULL DEFAULT 'تصاميم فريدة تجمع بين التراث والحداثة';
    ALTER TABLE "SiteAyarlari" ADD COLUMN IF NOT EXISTS "HeroAltBaslikEn" text NOT NULL DEFAULT 'Unique designs blending heritage and modernity';
    ALTER TABLE "SiteAyarlari" ADD COLUMN IF NOT EXISTS "HeroAltBaslikTr" text NOT NULL DEFAULT 'Mirasa modern bir dokunuş katan özel tasarımlar';
    ALTER TABLE "SiteAyarlari" ADD COLUMN IF NOT EXISTS "HeroGorselUrl" text NOT NULL DEFAULT '/slider-demo.jpg';

    -- Filistin kargo bölgeleri: Ulke, Aciklama ve Fiyat alanları
    ALTER TABLE "KargoBolgeler" ADD COLUMN IF NOT EXISTS "Ulke" character varying(100) NULL;
    ALTER TABLE "KargoBolgeler" ADD COLUMN IF NOT EXISTS "Aciklama" character varying(500) NULL;
    ALTER TABLE "KargoBolgeler" ADD COLUMN IF NOT EXISTS "Fiyat" numeric NOT NULL DEFAULT 0;

    -- Toptancı ürün grupları ve iskonto tabloları
    ALTER TABLE "Urunler" ADD COLUMN IF NOT EXISTS "ToptanciUrunGrubuId" integer NULL;

    CREATE TABLE IF NOT EXISTS "ToptanciUrunGruplari" (
        "Id" integer GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
        "Ad" text NOT NULL,
        "Aciklama" text NULL,
        "AktifMi" boolean NOT NULL DEFAULT true,
        "Sira" integer NOT NULL DEFAULT 0,
        "OlusturulmaTarihi" timestamp with time zone NOT NULL DEFAULT now(),
        "GuncellemeTarihi" timestamp with time zone NULL,
        "SilindiMi" boolean NOT NULL DEFAULT false
    );

    CREATE INDEX IF NOT EXISTS "IX_ToptanciUrunGruplari_AktifMi" ON "ToptanciUrunGruplari" ("AktifMi");

    CREATE TABLE IF NOT EXISTS "ToptanciIskontoOranlari" (
        "Id" integer GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
        "ToptanciUrunGrubuId" integer NOT NULL,
        "MinAdet" integer NOT NULL DEFAULT 1,
        "IskontoYuzdesi" numeric NOT NULL DEFAULT 0,
        "AktifMi" boolean NOT NULL DEFAULT true,
        "OlusturulmaTarihi" timestamp with time zone NOT NULL DEFAULT now(),
        "GuncellemeTarihi" timestamp with time zone NULL,
        "SilindiMi" boolean NOT NULL DEFAULT false,
        CONSTRAINT "FK_ToptanciIskontoOranlari_ToptanciUrunGruplari" FOREIGN KEY ("ToptanciUrunGrubuId") REFERENCES "ToptanciUrunGruplari" ("Id") ON DELETE CASCADE
    );

    CREATE INDEX IF NOT EXISTS "IX_ToptanciIskontoOranlari_ToptanciUrunGrubuId" ON "ToptanciIskontoOranlari" ("ToptanciUrunGrubuId");

    IF NOT EXISTS (
        SELECT 1 FROM pg_constraint
        WHERE conname = 'FK_Urunler_ToptanciUrunGruplari'
          AND conrelid = '"Urunler"'::regclass
    ) THEN
        ALTER TABLE "Urunler" ADD CONSTRAINT "FK_Urunler_ToptanciUrunGruplari" FOREIGN KEY ("ToptanciUrunGrubuId") REFERENCES "ToptanciUrunGruplari" ("Id") ON DELETE SET NULL;
    END IF;
    CREATE INDEX IF NOT EXISTS "IX_Urunler_ToptanciUrunGrubuId" ON "Urunler" ("ToptanciUrunGrubuId");

    -- Favori fiyat düşüş bildirimi alanları
    ALTER TABLE "Favoriler" ADD COLUMN IF NOT EXISTS "FiyatDustugundaBildir" boolean NOT NULL DEFAULT false;
    ALTER TABLE "Favoriler" ADD COLUMN IF NOT EXISTS "EskiFiyat" numeric NULL;
    ALTER TABLE "Favoriler" ADD COLUMN IF NOT EXISTS "SonBildirimTarihi" timestamp with time zone NULL;

    CREATE TABLE IF NOT EXISTS "SiteDegerlendirmeleri" (
        "Id" integer GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
        "AdSoyad" text NOT NULL,
        "Eposta" text NULL,
        "Puan" integer NOT NULL DEFAULT 5,
        "Baslik" text NULL,
        "YorumMetni" text NOT NULL DEFAULT '',
        "OnayliMi" boolean NOT NULL DEFAULT false,
        "OlusturulmaTarihi" timestamp with time zone NOT NULL,
        "SilindiMi" boolean NOT NULL DEFAULT false
    );

    IF NOT EXISTS (
        SELECT 1
        FROM information_schema.table_constraints
        WHERE constraint_schema = 'public'
          AND table_name = 'Kategoriler'
          AND constraint_name = 'FK_Kategoriler_Kategoriler_ParentKategoriId'
    ) THEN
        ALTER TABLE "Kategoriler"
            ADD CONSTRAINT "FK_Kategoriler_Kategoriler_ParentKategoriId"
            FOREIGN KEY ("ParentKategoriId") REFERENCES "Kategoriler"("Id") ON DELETE RESTRICT;
    END IF;

    CREATE TABLE IF NOT EXISTS "PushAbonelikleri" (
        "Id" integer GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
        "Token" text NOT NULL,
        "AppUserId" text NOT NULL,
        "Tarayici" text NOT NULL DEFAULT '',
        "Platform" text NOT NULL DEFAULT '',
        "AktifMi" boolean NOT NULL DEFAULT true,
        "SonGorulmeTarihi" timestamp with time zone NOT NULL DEFAULT NOW(),
        "Tercihler" text NOT NULL DEFAULT '',
        "OlusturulmaTarihi" timestamp with time zone NOT NULL DEFAULT NOW(),
        "SilindiMi" boolean NOT NULL DEFAULT false
    );

    CREATE INDEX IF NOT EXISTS "IX_PushAbonelikleri_AppUserId" ON "PushAbonelikleri" ("AppUserId");
    CREATE INDEX IF NOT EXISTS "IX_PushAbonelikleri_Token" ON "PushAbonelikleri" ("Token");

    IF NOT EXISTS (
        SELECT 1 FROM pg_constraint
        WHERE conname = 'FK_PushAbonelikleri_AspNetUsers_AppUserId'
    ) THEN
        ALTER TABLE "PushAbonelikleri" ADD CONSTRAINT "FK_PushAbonelikleri_AspNetUsers_AppUserId" FOREIGN KEY ("AppUserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE;
    END IF;

    CREATE TABLE IF NOT EXISTS "StokBildirimLoglari" (
        "Id" integer GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
        "UrunId" integer NULL REFERENCES "Urunler"("Id") ON DELETE SET NULL,
        "UrunSecenekId" integer NULL REFERENCES "UrunSecenekleri"("Id") ON DELETE SET NULL,
        "KalanStok" integer NOT NULL DEFAULT 0,
        "StokEsigi" integer NOT NULL DEFAULT 5,
        "BildirimTipi" text NOT NULL DEFAULT 'eposta',
        "GonderildiMi" boolean NOT NULL DEFAULT false,
        "OlusturulmaTarihi" timestamp with time zone NOT NULL DEFAULT NOW(),
        "SilindiMi" boolean NOT NULL DEFAULT false
    );

    CREATE INDEX IF NOT EXISTS "IX_StokBildirimLoglari_UrunId" ON "StokBildirimLoglari" ("UrunId");
    CREATE INDEX IF NOT EXISTS "IX_StokBildirimLoglari_UrunSecenekId" ON "StokBildirimLoglari" ("UrunSecenekId");
    CREATE INDEX IF NOT EXISTS "IX_StokBildirimLoglari_GonderildiMi" ON "StokBildirimLoglari" ("GonderildiMi");
END
$$;

CREATE INDEX IF NOT EXISTS "IX_Kategoriler_ParentKategoriId" ON "Kategoriler" ("ParentKategoriId");
CREATE INDEX IF NOT EXISTS "IX_Kategoriler_Slug" ON "Kategoriler" ("Slug");
CREATE INDEX IF NOT EXISTS "IX_Urunler_SKU" ON "Urunler" ("SKU");
CREATE UNIQUE INDEX IF NOT EXISTS "IX_UrunOzellikTanimlari_UrunTipi_Kod" ON "UrunOzellikTanimlari" ("UrunTipi", "Kod");
CREATE INDEX IF NOT EXISTS "IX_UrunOzellikDegerleri_UrunId" ON "UrunOzellikDegerleri" ("UrunId");
CREATE INDEX IF NOT EXISTS "IX_UrunOzellikDegerleri_UrunOzellikTanimiId" ON "UrunOzellikDegerleri" ("UrunOzellikTanimiId");
CREATE INDEX IF NOT EXISTS "IX_UrunResimleri_UrunId_Sira" ON "UrunResimleri" ("UrunId", "Sira");

UPDATE "UrunResimleri"
SET
    "Baslik" = CASE WHEN COALESCE("Baslik", '') = '' THEN 'Galeri' ELSE "Baslik" END,
    "ThumbnailYolu" = CASE WHEN COALESCE("ThumbnailYolu", '') = '' THEN "ResimYolu" ELSE "ThumbnailYolu" END,
    "Sira" = CASE WHEN "Sira" = 0 THEN "Id" ELSE "Sira" END;

UPDATE "Urunler" u
SET "Fiyat" = src."Fiyat"
FROM (
    SELECT "UrunId", MIN("SatisFiyati") AS "Fiyat"
    FROM "UrunSecenekleri"
    WHERE "SatisFiyati" > 0
    GROUP BY "UrunId"
) src
WHERE src."UrunId" = u."Id"
  AND COALESCE(u."Fiyat", 0) = 0;
""";

    await context.Database.ExecuteSqlRawAsync(sql);
    logger.LogInformation("Eksik Mart 2026 katalog semasi kontrol edildi.");
}

static async Task EnsureMigrationHistoryConsistencyAsync(KanvasDbContext context, Microsoft.Extensions.Logging.ILogger<Program> logger)
{
    await context.Database.ExecuteSqlRawAsync(@"
INSERT INTO ""__EFMigrationsHistory"" (""MigrationId"", ""ProductVersion"")
SELECT m, productVersion FROM (VALUES
    ('20260131211352_BultenTablosu', '8.0.4'),
    ('20260131213049_BultenIpEklendi', '8.0.4'),
    ('20260504111249_AddProductSeoFields', '8.0.4'),
    ('20260508175141_MusteriNotuAlanlariEklendi', '8.0.4'),
    ('20260508204726_Fix_NullableUrunSecenekId_And_SlugIndex', '8.0.4'),
    ('20260523223159_AddFavoriPriceDropFields', '8.0.4'),
    ('20260615140117_AddWhatsappSiparisFields', '8.0.4'),
    ('20260623204005_AddKampanyaBitisTarihi', '8.0.4'),
    ('20260624190456_AddPushAbonelik', '8.0.4'),
    ('20260624195919_AddIptalSuresiSaat', '8.0.4'),
    ('20260624200430_AddStokBildirimLog', '8.0.4'),
    ('20260624201857_AddYayindaMiToUrunler', '8.0.4'),
    ('20260624204621_AddReceteOnayDurumu', '8.0.4'),
    ('20260707195000_AddKategoriMenuGorselUrl', '8.0.4'),
    ('20260709172141_AddKargoBolgeSehirMultilingual', '8.0.4')
) AS t(m, productVersion)
WHERE NOT EXISTS (
    SELECT 1 FROM ""__EFMigrationsHistory"" WHERE ""MigrationId"" = t.m
);
");
    logger.LogInformation("Migration history tutarlilik kontrolu tamamlandi.");
}
