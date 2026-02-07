using System.Globalization;
using System.Windows.Controls;

namespace TRPO_pr15.Validations;

public class DateNotFutureValidation : ValidationRule
{
    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        if (value == null)
            return new ValidationResult(false, "Выберите дату");

        if (value is DateTime dt)
        {
            if (dt.Date > DateTime.Today)
                return new ValidationResult(false, "Дата не может быть в будущем");
            return ValidationResult.ValidResult;
        }

        return new ValidationResult(false, "Некорректная дата");
    }
}
