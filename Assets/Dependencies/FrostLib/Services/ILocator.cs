using System;

namespace FrostLib.Services
{
    public interface ILocator
    {
        T Get<T>(string tag = "");
        object Get(Type type, string tag = "");
    }
}