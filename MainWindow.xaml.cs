using System;
using System.Windows;
using System.Windows.Controls;

namespace SimpleCalculatorFactory
{
    public partial class MainWindow : Window
    {
        private readonly ButtonFactory _factory;
        private readonly CalculatorEngine _engine;

        private double _lastNumber;
        private string _currentOperation;
        private bool _isNewEntry;

        public MainWindow()
        {
            InitializeComponent();
            _factory = new ButtonFactory();
            _engine = new CalculatorEngine();
            _isNewEntry = true;
        }

        // Универсальный обработчик для всех кнопок
        // Универсальный обработчик для всех кнопок
        private void AnyButton_Click(object sender, RoutedEventArgs e)
        {
            var uiButton = (Button)sender;
            string buttonText = uiButton.Content.ToString();

            // 1. Используем Фабричный метод для создания логического объекта кнопки
            CalculatorButton logicalButton = _factory.CreateButton(buttonText);

            // 2. Получаем результат нажатия логической кнопки
            string action = logicalButton.Press();

            // 3. Обрабатываем действие в зависимости от типа созданной кнопки
            if (logicalButton is DigitButton)
            {
                HandleDigit(action);
            }
            else if (logicalButton is OperatorButton)
            {
                HandleOperator(action);
            }
            else if (logicalButton is EqualsButton)
            {
                HandleEquals();
            }
            else if (logicalButton is ClearButton)
            {
                HandleClear();
            }
            else if (logicalButton is ClearEntryButton)
            {
                HandleClearEntry();
            }
        }

        private void HandleDigit(string digit)
        {
            if (DisplayTextBox.Text == "0" || _isNewEntry)
            {
                DisplayTextBox.Text = digit;
                _isNewEntry = false;
            }
            else
            {
                DisplayTextBox.Text += digit;
            }
        }

        private void HandleOperator(string operation)
        {
            if (!_isNewEntry && !string.IsNullOrEmpty(_currentOperation))
            {
                HandleEquals(); // Вычисляем промежуточный результат
            }

            _currentOperation = operation;
            if (double.TryParse(DisplayTextBox.Text, out _lastNumber))
            {
                _isNewEntry = true;
            }
        }

        private void HandleEquals()
        {
            if (string.IsNullOrEmpty(_currentOperation)) return;

            if (double.TryParse(DisplayTextBox.Text, out double currentNumber))
            {
                try
                {
                    double result = _engine.Calculate(_lastNumber, currentNumber, _currentOperation);
                    DisplayTextBox.Text = result.ToString();
                    _lastNumber = result;
                    _isNewEntry = true;
                    _currentOperation = string.Empty;
                }
                catch (DivideByZeroException ex)
                {
                    DisplayTextBox.Text = "Ошибка";
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    _isNewEntry = true;
                }
            }
        }

        private void HandleClear()
        {
            DisplayTextBox.Text = "0";
            _lastNumber = 0;
            _currentOperation = string.Empty;
            _isNewEntry = true;
        }

        private void HandleClearEntry()
        {
            DisplayTextBox.Text = "0";
            _isNewEntry = true;
        }
    }
}