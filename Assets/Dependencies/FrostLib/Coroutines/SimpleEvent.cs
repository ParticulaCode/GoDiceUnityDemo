namespace FrostLib.Coroutines
{
    public class SimpleEvent
    {
        private System.Action _delegate = () => { };

        public void Add(System.Action aDelegate) => _delegate += aDelegate;

        public void Remove(System.Action aDelegate)
        {
            if (_delegate != null)
                _delegate -= aDelegate;
        }

        public void Run() => _delegate();
    }
}