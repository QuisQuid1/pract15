using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace TRPO_pr15.Validations
{
    public class DecimalValidation : ValidationRule
    {
        public decimal? MinValue { get; set; } = 0;

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
                return ValidationResult.ValidResult;

            if (!decimal.TryParse(value.ToString(), CultureInfo.InvariantCulture, out decimal result))
                return new ValidationResult(false, "Введите числовое значение (разделитель .)");

            if (decimal.Round(result, 2) != result)
                return new ValidationResult(false, $"Два знака после запятой");

            if (MinValue.HasValue && result < MinValue.Value)
                return new ValidationResult(false, $"Значение не должно быть меньше {MinValue}");

            return ValidationResult.ValidResult;
        }

    }
}
