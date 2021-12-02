using System.Collections;
using System.Collections.Generic;

namespace FrostLib.Containers
{
    public class CircularIndex : IEnumerator<int>
    {
        public int Current { get; private set; }

        public int Previous
        {
            get
            {
                var value = Current - 1;
                if (value < _min)
                    value = _max;

                return value;
            }
        }

        public int Next
        {
            get
            {
                var value = Current + 1;
                if (value > _max)
                    value = _min;
                return value;
            }
        }

        private readonly int _min;
        private readonly int _max;

        /// <param name="min">Inclusive</param>
        /// <param name="max">Inclusive</param>
        public CircularIndex(int current, int min, int max)
        {
            Current = current;
            _max = max;
            _min = min;
        }

        public static implicit operator int(CircularIndex d) => d.Current;

        public bool MoveNext()
        {
            Current = Next;
            return true;
        }

        public bool MovePrevious()
        {
            Current = Previous;
            return true;
        }

        public void Reset() => Current = _min;

        object IEnumerator.Current => Current;

        public void Dispose()
        {
        }
    }
}