using UnityEngine;

namespace FrostLib.Containers
{
    /// Check out TimeThreshold
    public class ExpireContainer
    {
        // ReSharper disable once InconsistentNaming
        public readonly float TTL;
        public bool IsExpired => Time.time >= _lastTime + TTL;

        private float _lastTime;

        public ExpireContainer(float ttl)
        {
            TTL = ttl;
            Refresh();
        }

        public void Refresh() => _lastTime = Time.time;
    }

    public class ExpireContainer<T> : ExpireContainer
    {
        public T Item;

        public ExpireContainer(T item, float ttl) : base(ttl) => Item = item;
    }
}