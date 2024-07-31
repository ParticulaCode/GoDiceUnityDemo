namespace FrostLib.Containers.Counters
{
    public class StrictCounter : IStrictCounter
    {
        public virtual bool RequirementsMet => _target == _accepted;

        private readonly int _target;
        private int _accepted;

        public StrictCounter(int target) => _target = target;

        public virtual void Accept() => _accepted++;

        public void Reset() => _accepted = 0;
    }
}