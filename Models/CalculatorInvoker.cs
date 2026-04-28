using System.Collections.Generic;

namespace SimpleCalculatorMVVM.Models
{
    public class CalculatorInvoker
    {
        private readonly Stack<ICalculatorCommand> _history = new Stack<ICalculatorCommand>();

        public void ExecuteCommand(ICalculatorCommand command)
        {
            command.Execute();
            _history.Push(command);
        }

        public void Undo()
        {
            if (_history.Count > 0)
            {
                var command = _history.Pop();
                command.Undo();
            }
        }

        public void ClearHistory()
        {
            _history.Clear();
        }
    }
}