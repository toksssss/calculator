using System;
using System.Windows.Input;
using SimpleCalculatorMVVM.Models;

namespace SimpleCalculatorMVVM.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly Calculator _calculator;
        private readonly CalculatorInvoker _invoker;
        private string _display;
        private double _lastNumber;
        private string _currentOperation;
        private bool _isNewEntry;

        public MainViewModel()
        {
            _calculator = new Calculator();
            _invoker = new CalculatorInvoker();
            Display = "0";
            _isNewEntry = true;

            DigitCommand = new RelayCommand(ExecuteDigit);
            OperationCommand = new RelayCommand(ExecuteOperation);
            EqualsCommand = new RelayCommand(ExecuteEquals);
            ClearCommand = new RelayCommand(ExecuteClear);
            UndoCommand = new RelayCommand(ExecuteUndo);
        }

        public string Display
        {
            get => _display;
            set { _display = value; OnPropertyChanged(); }
        }

        public ICommand DigitCommand { get; }
        public ICommand OperationCommand { get; }
        public ICommand EqualsCommand { get; }
        public ICommand ClearCommand { get; }
        public ICommand UndoCommand { get; }

        private void ExecuteDigit(object parameter)
        {
            string digit = parameter?.ToString();
            if (Display == "0" || Display == "Ошибка" || _isNewEntry)
            {
                Display = digit;
                _isNewEntry = false;
            }
            else
            {
                Display += digit;
            }
        }

        private void ExecuteOperation(object parameter)
        {
            string operation = parameter?.ToString();

            if (!_isNewEntry && !string.IsNullOrEmpty(_currentOperation))
            {
                ExecuteEquals(null);
            }

            _currentOperation = operation;
            if (double.TryParse(Display, out _lastNumber))
            {
                _isNewEntry = true;
            }
        }

        private void ExecuteEquals(object parameter)
        {
            if (string.IsNullOrEmpty(_currentOperation)) return;

            if (double.TryParse(Display, out double currentNumber))
            {
                try
                {
                    _calculator.CurrentValue = _lastNumber;
                    var command = new CalculatorCommand(_calculator, _currentOperation, currentNumber);
                    _invoker.ExecuteCommand(command);

                    Display = _calculator.CurrentValue.ToString();
                    _lastNumber = _calculator.CurrentValue;
                    _isNewEntry = true;
                    _currentOperation = string.Empty;
                }
                catch (DivideByZeroException ex)
                {
                    Display = "Ошибка";
                    _isNewEntry = true;
                    _currentOperation = string.Empty;
                }
            }
        }

        private void ExecuteUndo(object parameter)
        {
            _invoker.Undo();
            _lastNumber = _calculator.CurrentValue;
            Display = _calculator.CurrentValue.ToString();
            _isNewEntry = true;
            _currentOperation = string.Empty;
        }

        private void ExecuteClear(object parameter)
        {
            string type = parameter?.ToString();
            if (type == "CE")
            {
                Display = "0";
                _isNewEntry = true;
            }
            else // "C"
            {
                Display = "0";
                _lastNumber = 0;
                _currentOperation = string.Empty;
                _isNewEntry = true;
                _calculator.CurrentValue = 0;
                _invoker.ClearHistory();
            }
        }
    }
}