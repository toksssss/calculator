namespace SimpleCalculatorMVVM.Models
{
    public interface ICalculatorCommand
    {
        void Execute();
        void Undo();
    }
}