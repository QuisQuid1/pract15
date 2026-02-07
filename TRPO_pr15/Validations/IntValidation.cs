using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace TRPO_pr15.Validations
{
    public class IntValidation : ValidationRule
    {
        public int? MinValue { get; set; } = 0;
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
                return new ValidationResult(false, "Поле обязательно для заполнения");

            if (!int.TryParse(value.ToString(), out int result))
                return new ValidationResult(false, "Введите целое число");

            if (MinValue.HasValue && result < MinValue.Value)
                return new ValidationResult(false, $"Значение не должно быть меньше {MinValue}");

            return ValidationResult.ValidResult;
        }
    }
}
