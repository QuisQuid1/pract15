using System.Globalization;
using System.Windows.Controls;

namespace TRPO_pr15.Validations;

public class RatingValidation : ValidationRule
{
    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            return new ValidationResult(false, "Поле обязательно для заполнения");

        if (!decimal.TryParse(value.ToString(), CultureInfo.InvariantCulture, out var result))
            return new ValidationResult(false, "Введите числовое значение (разделитель .)");

        if (decimal.Round(result, 1) != result)
            return new ValidationResult(false, "Один знак после запятой");

        if (result < 0 || result > 5)
            return new ValidationResult(false, "Рейтинг должен быть от 0 до 5");

        return ValidationResult.ValidResult;
    }
}
