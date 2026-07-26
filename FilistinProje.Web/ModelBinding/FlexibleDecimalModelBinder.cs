using System.Globalization;
using System.Text;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FilistinProje.Web.ModelBinding;

public sealed class FlexibleDecimalModelBinderProvider : IModelBinderProvider
{
    private static readonly IModelBinder Binder = new FlexibleDecimalModelBinder();

    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        var modelType = Nullable.GetUnderlyingType(context.Metadata.ModelType) ?? context.Metadata.ModelType;
        return modelType == typeof(decimal) ? Binder : null;
    }
}

public sealed class FlexibleDecimalModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        ArgumentNullException.ThrowIfNull(bindingContext);

        var valueResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
        if (valueResult == ValueProviderResult.None)
        {
            return Task.CompletedTask;
        }

        bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueResult);
        var value = valueResult.FirstValue;
        var isNullable = Nullable.GetUnderlyingType(bindingContext.ModelMetadata.ModelType) != null;

        if (string.IsNullOrWhiteSpace(value))
        {
            if (isNullable)
            {
                bindingContext.Result = ModelBindingResult.Success(null);
            }
            else
            {
                bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, "A numeric value is required.");
            }

            return Task.CompletedTask;
        }

        if (FlexibleDecimalParser.TryParse(value, out var parsed))
        {
            bindingContext.Result = ModelBindingResult.Success(parsed);
        }
        else
        {
            bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, "Invalid numeric value.");
        }

        return Task.CompletedTask;
    }
}

public static class FlexibleDecimalParser
{
    public static bool TryParse(string? value, out decimal result)
    {
        result = default;
        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        var normalized = NormalizeDigitsAndSeparators(value.Trim());
        if (normalized.Length == 0 ||
            normalized.Any(c => !char.IsDigit(c) && c is not '.' and not ',' and not '-' and not '+') ||
            normalized.Skip(1).Any(c => c is '-' or '+'))
        {
            return false;
        }

        var separatorIndex = Math.Max(normalized.LastIndexOf('.'), normalized.LastIndexOf(','));
        var builder = new StringBuilder(normalized.Length);

        for (var i = 0; i < normalized.Length; i++)
        {
            var c = normalized[i];
            if (char.IsDigit(c) || ((c == '-' || c == '+') && i == 0))
            {
                builder.Append(c);
            }
            else if ((c == '.' || c == ',') && i == separatorIndex)
            {
                builder.Append('.');
            }
        }

        return decimal.TryParse(
            builder.ToString(),
            NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint,
            CultureInfo.InvariantCulture,
            out result);
    }

    private static string NormalizeDigitsAndSeparators(string value)
    {
        var builder = new StringBuilder(value.Length);
        foreach (var c in value)
        {
            builder.Append(c switch
            {
                >= '٠' and <= '٩' => (char)('0' + c - '٠'),
                >= '۰' and <= '۹' => (char)('0' + c - '۰'),
                '٫' => '.',
                '٬' => ',',
                ' ' or ' ' or ' ' => '\0',
                _ => c
            });
        }

        return builder.Replace("\0", string.Empty).ToString();
    }
}
