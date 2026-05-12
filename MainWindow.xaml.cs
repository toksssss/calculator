using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using SimpleCalculatorMVVM.ViewModels;

namespace SimpleCalculatorMVVM
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var viewModel = new MainViewModel();
            DataContext = viewModel;

            CreateButtons(viewModel);
        }

        private void CreateButtons(MainViewModel vm)
        {
            // Массив кнопок: Текст, Команда, Параметр команды
            var buttonDefs = new (string Content, string CommandName, object Parameter)[]
            {
                ("C", "ClearCommand", "C"), ("CE", "ClearCommand", "CE"), ("Undo", "UndoCommand", null), ("/", "OperationCommand", "/"),
                ("7", "DigitCommand", "7"), ("8", "DigitCommand", "8"), ("9", "DigitCommand", "9"), ("*", "OperationCommand", "*"),
                ("4", "DigitCommand", "4"), ("5", "DigitCommand", "5"), ("6", "DigitCommand", "6"), ("-", "OperationCommand", "-"),
                ("1", "DigitCommand", "1"), ("2", "DigitCommand", "2"), ("3", "DigitCommand", "3"), ("+", "OperationCommand", "+"),
                ("±", null, null), ("0", "DigitCommand", "0"), (",", "DigitCommand", ","), ("=", "EqualsCommand", null)
            };

            foreach (var def in buttonDefs)
            {
                Button btn = new Button
                {
                    Content = def.Content,
                    Margin = new Thickness(2),
                    FontSize = 18
                };

                // Настраиваем привязку команды, если она указана
                if (def.CommandName != null)
                {
                    Binding cmdBinding = new Binding(def.CommandName);
                    btn.SetBinding(Button.CommandProperty, cmdBinding);

                    if (def.Parameter != null)
                    {
                        btn.CommandParameter = def.Parameter;
                    }
                }

                ButtonsContainer.Children.Add(btn);
            }
        }
    }
}