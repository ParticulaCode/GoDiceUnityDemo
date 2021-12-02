namespace FrostLib.Services
{
    public interface IProvider
    {
        void Provide<T>(T service, string tag = "");
        void Remove<T>(string tag = "");
    }
}