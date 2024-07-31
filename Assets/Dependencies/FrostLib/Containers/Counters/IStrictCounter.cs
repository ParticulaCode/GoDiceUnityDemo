namespace FrostLib.Containers.Counters
{
    public interface IStrictCounter
    {
        bool RequirementsMet { get; }

        void Accept();
        void Reset();
    }
}