namespace FrostLib.Containers.Limiters
{
    public interface ILimiter
    {
        int Limit(int value);
    }
}