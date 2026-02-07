using System.Globalization;
using System.Windows.Controls;

namespace TRPO_pr15.Validations;

public class MaxLengthValidation : ValidationRule
{
    public int MaxLength { get; set; }

    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        if (value == null)
            return ValidationResult.ValidResult;

        var s = value.ToString() ?? string.Empty;
        return s.Length > MaxLength
            ? new ValidationResult(false, $"Максимум {MaxLength} символов")
            : ValidationResult.ValidResult;
    }
}
