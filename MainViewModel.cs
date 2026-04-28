using System;
using System.Windows.Input;
using SimpleCalculatorMVVM.Models;

namespace SimpleCalculatorMVVM.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly CalculatorEngine _engine;
        private string _display;
        private double _lastNumber;
        private string _currentOperation;
        private bool _isNewEntry;

        public MainViewModel()
        {
            _engine = new CalculatorEngine();
            Display = "0";
            _isNewEntry = true;

            // Инициализация команд
            DigitCommand = new RelayCommand(ExecuteDigit);
            OperationCommand = new RelayCommand(ExecuteOperation);
            EqualsCommand = new RelayCommand(ExecuteEquals);
            ClearCommand = new RelayCommand(ExecuteClear);
        }

        // Свойство, привязанное к экрану. При изменении автоматически обновляет UI.
        public string Display
        {
            get => _display;
            set
            {
                _display = value;
                OnPropertyChanged();
            }
        }

        public ICommand DigitCommand { get; }
        public ICommand OperationCommand { get; }
        public ICommand EqualsCommand { get; }
        public ICommand ClearCommand { get; }

        private void ExecuteDigit(object parameter)
        {
            string digit = parameter.ToString();
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
            string operation = parameter.ToString();

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
                    double result = _engine.Calculate(_lastNumber, currentNumber, _currentOperation);
                    Display = result.ToString();
                    _lastNumber = result;
                    _isNewEntry = true;
                    _currentOperation = string.Empty;
                }
                catch (DivideByZeroException)
                {
                    Display = "Ошибка";
                    _isNewEntry = true;
                    _currentOperation = string.Empty;
                }
            }
        }

        private void ExecuteClear(object parameter)
        {
            string type = parameter?.ToString();
            if (type == "CE")
            {
                Display = "0";
                _isNewEntry = true;
            }
            else // Для команды "C"
            {
                Display = "0";
                _lastNumber = 0;
                _currentOperation = string.Empty;
                _isNewEntry = true;
            }
        }
    }
}