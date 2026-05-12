using System;
using System.Windows.Input;
using SimpleCalculatorMVVM.Facades;

namespace SimpleCalculatorMVVM.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly CalculatorFacade _facade;
        private string _display;
        private string _selectedOperationSymbol; // Поле для индикатора
        private string _currentOperation;
        private bool _isNewEntry;

        public MainViewModel()
        {
            _facade = new CalculatorFacade();
            Display = "0";
            SelectedOperationSymbol = ""; // Изначально пусто
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

        // Свойство для привязки индикатора операции
        public string SelectedOperationSymbol
        {
            get => _selectedOperationSymbol;
            set { _selectedOperationSymbol = value; OnPropertyChanged(); }
        }

        public ICommand DigitCommand { get; }
        public ICommand OperationCommand { get; }
        public ICommand EqualsCommand { get; }
        public ICommand ClearCommand { get; }
        public ICommand UndoCommand { get; }

        private void ExecuteDigit(object parameter)
        {
            string digit = parameter?.ToString();
            if (Display == "0" || _isNewEntry)
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
            if (double.TryParse(Display, out double currentNumber))
            {
                if (!string.IsNullOrEmpty(_currentOperation) && !_isNewEntry)
                {
                    ExecuteEquals(null);
                }

                _currentOperation = operation;
                SelectedOperationSymbol = operation; // Показываем символ в индикаторе
                _isNewEntry = true;

                if (_facade.CurrentValue == 0 && currentNumber != 0)
                {
                    _facade.ExecuteOperation("+", currentNumber);
                }
            }
        }

        private void ExecuteEquals(object parameter)
        {
            if (!string.IsNullOrEmpty(_currentOperation) && double.TryParse(Display, out double currentNumber))
            {
                try
                {
                    _facade.ExecuteOperation(_currentOperation, currentNumber);
                    Display = _facade.CurrentValue.ToString();
                    _isNewEntry = true;
                    _currentOperation = string.Empty;
                    SelectedOperationSymbol = ""; // Убираем индикатор после вычисления
                }
                catch (DivideByZeroException)
                {
                    Display = "Ошибка";
                    SelectedOperationSymbol = "!";
                    _isNewEntry = true;
                }
            }
        }

        private void ExecuteClear(object parameter)
        {
            string type = parameter?.ToString();
            if (type == "C")
            {
                _facade.ClearAll();
                SelectedOperationSymbol = "";
                _currentOperation = string.Empty;
            }
            Display = "0";
            _isNewEntry = true;
        }

        private void ExecuteUndo(object parameter)
        {
            _facade.Undo();
            Display = _facade.CurrentValue.ToString();
            _isNewEntry = true;
        }
    }
}