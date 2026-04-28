using System;

namespace SimpleCalculatorFactory
{
    // Общий родительский класс для всех типов кнопок
    public abstract class CalculatorButton
    {
        public abstract string Press();
    }

    // 1. Цифровые кнопки (представляют цифры от 0 до 9)
    public class DigitButton : CalculatorButton
    {
        private int _digit;
        public DigitButton(int digit)
        {
            _digit = digit;
        }
        public override string Press() => _digit.ToString();
    }

    // 2. Операционные кнопки (+, -, *, /)
    public class OperatorButton : CalculatorButton
    {
        private string _operation;
        public OperatorButton(string operation)
        {
            _operation = operation;
        }
        public override string Press() => _operation;
    }

    // 3. Функциональные кнопки
    public class EqualsButton : CalculatorButton
    {
        public override string Press() => "=";
    }

    public class ClearButton : CalculatorButton
    {
        public override string Press() => "C";
    }

    public class ClearEntryButton : CalculatorButton
    {
        public override string Press() => "CE";
    }
}