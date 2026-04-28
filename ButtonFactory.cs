using System;

namespace SimpleCalculatorFactory
{
    public class ButtonFactory
    {
        // Фабричный метод порождения кнопок
        public CalculatorButton CreateButton(string input)
        {
            // Если ввод является числом, создаем цифровую кнопку
            if (int.TryParse(input, out int digit))
            {
                return new DigitButton(digit);
            }

            // В противном случае создаем операционную или функциональную кнопку
            switch (input)
            {
                case "+":
                case "-":
                case "*":
                case "/":
                    return new OperatorButton(input);
                case "=":
                    return new EqualsButton();
                case "C":
                    return new ClearButton();
                case "CE":
                    return new ClearEntryButton();
                default:
                    throw new ArgumentException($"Неизвестный тип кнопки: {input}");
            }
        }
    }
}