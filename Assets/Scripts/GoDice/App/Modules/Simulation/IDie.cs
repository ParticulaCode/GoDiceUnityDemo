namespace GoDice.App.Modules.Simulation
{
    public interface IDie
    {
        string Address { get; }
        
        void Roll();
        void Roll(int value);
        
        void Rotate();
        
        void Tap();
        void DoubleTap();
    }
}