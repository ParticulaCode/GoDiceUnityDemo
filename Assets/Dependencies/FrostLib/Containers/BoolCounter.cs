using System;

namespace FrostLib.Containers
{
    [Serializable]
    public class BoolCounter
    {
        public bool Value
        {
            get => _counter > 0;
            set
            {
                if (value)
                    _counter++;
                else
                    _counter--;
            }
        }

        private int _counter;

        public BoolCounter(bool initialState = false) => Value = initialState;
        
        public static implicit operator bool(BoolCounter me) => me.Value;

        public override string ToString() => $"{nameof(BoolCounter)}(Value = {Value}, _counter = {_counter})";
    }
}