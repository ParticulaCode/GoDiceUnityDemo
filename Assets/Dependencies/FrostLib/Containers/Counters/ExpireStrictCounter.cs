namespace FrostLib.Containers.Counters
{
    public class ExpireStrictCounter : StrictCounter
    {
        public override bool RequirementsMet => base.RequirementsMet && !_container.IsExpired;

        private readonly ExpireContainer _container;

        public ExpireStrictCounter(float ttl, int target) : base(target) =>
            _container = new ExpireContainer(ttl);

        public override void Accept()
        {
            base.Accept();
            _container.Refresh();
        }
    }
}