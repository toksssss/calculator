using System;

namespace SimpleCalculatorMVVM.Models
{
    public class Calculator
    {
        public double CurrentValue { get; set; }

        public void Add(double operand) => CurrentValue += operand;
        public void Subtract(double operand) => CurrentValue -= operand;
        public void Multiply(double operand) => CurrentValue *= operand;

        public void Divide(double operand)
        {
            if (operand == 0)
                throw new DivideByZeroException("Деление на ноль");
            CurrentValue /= operand;
        }
    }
}