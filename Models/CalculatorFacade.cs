using System;
using SimpleCalculatorMVVM.Models;

namespace SimpleCalculatorMVVM.Facades
{
    public class CalculatorFacade
    {
        private readonly Calculator _calculator;
        private readonly CalculatorInvoker _invoker;

        public CalculatorFacade()
        {
            _calculator = new Calculator();
            _invoker = new CalculatorInvoker();
        }

        // Предоставляем доступ к текущему значению
        public double CurrentValue
        {
            get => _calculator.CurrentValue;
        }

        // Инкапсулируем создание и выполнение команды
        public void ExecuteOperation(string operation, double operand)
        {
            var command = new CalculatorCommand(_calculator, operation, operand);
            _invoker.ExecuteCommand(command);
        }

        // Делегируем отмену операции
        public void Undo()
        {
            _invoker.Undo();
        }

        // Делегируем очистку
        public void ClearAll()
        {
            _calculator.CurrentValue = 0;
            _invoker.ClearHistory();
        }
    }
}