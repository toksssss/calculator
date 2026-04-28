using System;
using System.Windows;
using System.Windows.Controls;

namespace SimpleCalculator
{
    public partial class MainWindow : Window
    {
        private readonly CalculatorEngine _engine;
        private double _lastNumber;
        private string _currentOperation;
        private bool _isNewEntry;

        public MainWindow()
        {
            InitializeComponent();
            _engine = new CalculatorEngine();
            _isNewEntry = true;
        }

        // Ввод цифр
        private void Digit_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            string digit = button.Content.ToString();

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

        // Ввод десятичной запятой
        private void Decimal_Click(object sender, RoutedEventArgs e)
        {
            if (_isNewEntry)
            {
                DisplayTextBox.Text = "0,";
                _isNewEntry = false;
            }
            else if (!DisplayTextBox.Text.Contains(","))
            {
                DisplayTextBox.Text += ",";
            }
        }

        // Выбор арифметической операции
        private void Operation_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;

            if (!_isNewEntry && !string.IsNullOrEmpty(_currentOperation))
            {
                Equals_Click(this, null);
            }

            _currentOperation = button.Content.ToString();
            if (double.TryParse(DisplayTextBox.Text, out _lastNumber))
            {
                _isNewEntry = true;
            }
        }

        // Нажатие кнопки "="
        private void Equals_Click(object sender, RoutedEventArgs e)
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

        // Полный сброс всех данных (С)
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            DisplayTextBox.Text = "0";
            _lastNumber = 0;
            _currentOperation = string.Empty;
            _isNewEntry = true;
        }

        // Очистка только текущего вводимого числа (CE)
        private void ClearEntry_Click(object sender, RoutedEventArgs e)
        {
            DisplayTextBox.Text = "0";
            _isNewEntry = true;
        }
    }
}