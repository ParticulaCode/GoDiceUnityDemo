using System;
using System.Collections.Generic;

namespace FrostLib.Services
{
    public class ServiceLocator : ILocator, IProvider
    {
        public static ServiceLocator Instance => _instance ??= new ServiceLocator();

        private static ServiceLocator _instance;
        
        private readonly IDictionary<Type, IDictionary<string, object>> _services = new Dictionary<Type, IDictionary<string, object>>();

        public T Get<T>(string tag = "")
        { 
            var result = Get(typeof(T), tag);
            return result != null ? (T)result : default(T);
        }
        
        public object Get(Type type, string tag = "")
        {
            _services.TryGetValue(type, out var dict);
            
            object service = null;
            dict?.TryGetValue(tag, out service);
            return service;
        }
        
        public void Provide<T>(T service, string tag = "")
        {
            Remove<T>(tag);

            var type = typeof(T);
            if (!_services.ContainsKey(type))
                _services.Add(type, new Dictionary<string, object>());
            
            var dict = _services[type];
            dict.Add(tag, service);
        }
        
        public void Remove<T>(string tag = "")
        {
            var service = Get<T>(tag);
            if (service == null)
                return;

            _services[typeof(T)].Remove(tag);

            TryDispose(service);
        }

        private static void TryDispose<T>(T service)
        {
            switch (service)
            {
                case IDisposable disposable:
                    disposable.Dispose();
                    return;
                case IReadOnlyList<IDisposable> disposables:
                {
                    foreach (var d in disposables)
                        d.Dispose();

                    return;
                }
            }
        }

        public void ResetAll()
        {
            foreach (var dict in _services)
            {
                foreach (var pair in dict.Value)
                {
                    if (pair.Value is IDisposable disposable)
                        disposable.Dispose();
                }
            }

            _services.Clear();
        }
    }
}