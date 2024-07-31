using FrostLib.Signals.impl;

namespace FrostLib.Containers.Rx
{
    public class Reactive<T> : IReactive<T>
    {
        public static implicit operator T(Reactive<T> self) => self.Value;

        public T Value { get; private set; }
        public Signal<T> OnChange { get; } = new();

        private readonly bool _setOnlyNewValue;

        public Reactive(T value = default, bool setOnlyNewValue = true)
        {
            Value = value;
            _setOnlyNewValue = setOnlyNewValue;
        }

        public void Set(T newValue)
        {
            if (_setOnlyNewValue && Value != null && Value.Equals(newValue))
                return;

            Value = newValue;
            OnChange.Dispatch(Value);
        }

        public void Dispose() => OnChange.ClearListeners();

        public override string ToString() => Value.ToString();

        public ReadonlyReactive<T> ToReadonly() => new(this);
    }
}