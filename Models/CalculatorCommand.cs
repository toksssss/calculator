namespace SimpleCalculatorMVVM.Models
{
    public class CalculatorCommand : ICalculatorCommand
    {
        private readonly Calculator _calculator;
        private readonly string _operator;
        private readonly double _operand;

        public CalculatorCommand(Calculator calculator, string @operator, double operand)
        {
            _calculator = calculator;
            _operator = @operator;
            _operand = operand;
        }

        public void Execute()
        {
            switch (_operator)
            {
                case "+": _calculator.Add(_operand); break;
                case "-": _calculator.Subtract(_operand); break;
                case "*": _calculator.Multiply(_operand); break;
                case "/": _calculator.Divide(_operand); break;
            }
        }

        public void Undo()
        {
            switch (_operator)
            {
                case "+": _calculator.Subtract(_operand); break;
                case "-": _calculator.Add(_operand); break;
                case "*": _calculator.Divide(_operand); break;
                case "/": _calculator.Multiply(_operand); break;
            }
        }
    }
}