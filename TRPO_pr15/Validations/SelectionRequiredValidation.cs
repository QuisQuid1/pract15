using System.Globalization;
using System.Windows.Controls;

namespace TRPO_pr15.Validations;

public class SelectionRequiredValidation : ValidationRule
{
    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        return value == null
            ? new ValidationResult(false, "Выберите значение")
            : ValidationResult.ValidResult;
    }
}
