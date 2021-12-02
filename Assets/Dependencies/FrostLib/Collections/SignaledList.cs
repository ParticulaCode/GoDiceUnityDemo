using System.Collections.Generic;
using FrostLib.Signals.impl;

namespace FrostLib.Collections
{
    public class SignaledList<T> : List<T>
    {
        public readonly Signal<T> OnItemAddedSignal = new Signal<T>();
        public readonly Signal<T> OnItemRemovedSignal = new Signal<T>();
        public readonly Signal<int> OnItemsAmountChangedSignal = new Signal<int>();
        public readonly Signal OnClearSignal = new Signal();

        public new void Add(T item)
        {
            base.Add(item);
            OnItemAddedSignal.Dispatch(item);
            NotifyAmountChanged();
        }

        private void NotifyAmountChanged() => OnItemsAmountChangedSignal.Dispatch(Count);

        public new void AddRange(IEnumerable<T> items)
        {
            foreach (var item in items)
                Add(item);
        }

        public new bool Remove(T item)
        {
            if(!base.Remove(item))
                return false;

            OnItemRemovedSignal.Dispatch(item);
            NotifyAmountChanged();

            return true;
        }

        public T[] RemoveAll()
        {
            var items = ToArray();
            foreach (var item in items)
                Remove(item);

            return items;
        }

        public new void Clear()
        {
            base.Clear();
            NotifyAmountChanged();
            OnClearSignal.Dispatch();
        }
    }
}