using System;

namespace FilistinProje.Service.Services
{
    public static class BusinessTimeZoneService
    {
        private static readonly Lazy<TimeZoneInfo> StoreTimeZoneLazy = new Lazy<TimeZoneInfo>(() =>
        {
            var candidateIds = new[]
            {
                "Asia/Gaza",
                "Asia/Hebron",
                "Asia/Jerusalem",
                "Israel Standard Time",
                "Middle East Standard Time",
                "Arabic Standard Time"
            };

            foreach (var id in candidateIds)
            {
                try
                {
                    return TimeZoneInfo.FindSystemTimeZoneById(id);
                }
                catch
                {
                    // Try next candidate timezone ID
                }
            }

            return TimeZoneInfo.CreateCustomTimeZone("StoreDefaultZone", TimeSpan.FromHours(3), "Store Time Zone", "Store Time Zone");
        });

        public static TimeZoneInfo StoreTimeZone => StoreTimeZoneLazy.Value;

        /// <summary>
        /// Admin datetime-local input (Unspecified) in store local time converted to UTC for DB storage.
        /// </summary>
        public static DateTime? ConvertStoreLocalToUtc(DateTime? localUnspecified)
        {
            if (!localUnspecified.HasValue)
            {
                return null;
            }

            var dt = localUnspecified.Value;
            if (dt.Kind == DateTimeKind.Utc)
            {
                return dt;
            }

            var unspecified = DateTime.SpecifyKind(dt, DateTimeKind.Unspecified);
            return TimeZoneInfo.ConvertTimeToUtc(unspecified, StoreTimeZone);
        }

        /// <summary>
        /// DB UTC time converted back to store local Unspecified time for admin datetime-local display.
        /// </summary>
        public static DateTime? ConvertUtcToStoreLocal(DateTime? utcValue)
        {
            if (!utcValue.HasValue)
            {
                return null;
            }

            var dt = utcValue.Value;
            var utc = dt.Kind == DateTimeKind.Utc ? dt : DateTime.SpecifyKind(dt, DateTimeKind.Utc);
            var local = TimeZoneInfo.ConvertTimeFromUtc(utc, StoreTimeZone);
            return DateTime.SpecifyKind(local, DateTimeKind.Unspecified);
        }
    }
}
