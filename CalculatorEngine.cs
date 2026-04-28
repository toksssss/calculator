using System;

namespace SimpleCalculatorFactory
{
    public class CalculatorEngine
    {
        public double Calculate(double operand1, double operand2, string operation)
        {
            switch (operation)
            {
                case "+": return operand1 + operand2;
                case "-": return operand1 - operand2;
                case "*": return operand1 * operand2;
                case "/":
                    if (operand2 == 0) throw new DivideByZeroException("Деление на ноль невозможно.");
                    return operand1 / operand2;
                default:
                    throw new InvalidOperationException("Неизвестная операция.");
            }
        }
    }
}