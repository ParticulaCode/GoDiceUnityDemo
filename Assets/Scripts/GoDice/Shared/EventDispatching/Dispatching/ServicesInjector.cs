using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FrostLib.Services;
using GoDice.Shared.EventDispatching.Injections;
using UnityEngine;

namespace GoDice.Shared.EventDispatching.Dispatching
{
    /// 1) T is required to define a base class to optimize reflection inheretance analyzes.
    /// It will stop when reach the T type. T can be replaced with object for an universal solution
    /// 2) Inject looks for Proprties only.
    /// 3) Concrete implementation can be injected even if it's provided by base class or interface
    public class ServicesInjector<T>
    {
        private static ServiceLocator Servicer => ServiceLocator.Instance;

        private class Record
        {
            public readonly PropertyInfo PropInfo;
            public readonly string ServiceTag;

            public Record(PropertyInfo propInfo, string serviceTag)
            {
                PropInfo = propInfo;
                ServiceTag = serviceTag;
            }
        }

        private Record[] _mandatoryProperties;
        private Record[] _optionalProperties;
        private T _target;

        public void Inject(T target)
        {
            _target = target;

            CollectProperties();
            ResolveInjections();
            ClearCache();
        }

        private void CollectProperties()
        {
            _mandatoryProperties = Collect<InjectAttribute>();
            _optionalProperties = Collect<OptionalInjectAttribute>();
        }

        private Record[] Collect<TA>() where TA : TaggedAttribute
        {
            var targetType = _target.GetType();
            var infos = new List<Record>();
            var baseType = typeof(T);
            while (targetType != baseType && targetType != null)
            {
                infos.AddRange(targetType.GetProperties(
                        BindingFlags.Instance
                        | BindingFlags.NonPublic
                        | BindingFlags.Public)
                    .Where((propInfo, _) => IsAttribute<TA>(propInfo))
                    .Select(propInfo => new Record(propInfo, GetTag(propInfo))));

                targetType = targetType.BaseType;
            }

            return infos.ToArray();
        }

        private static bool IsAttribute<TA>(MemberInfo prop) =>
            Attribute.GetCustomAttribute(prop, typeof(TA)) is TA;

        private static string GetTag(MemberInfo prop) =>
            ((TaggedAttribute) Attribute.GetCustomAttribute(prop, typeof(TaggedAttribute))).Tag;

        private void ResolveInjections()
        {
            Inject(_mandatoryProperties, false);
            Inject(_optionalProperties, true);
        }

        private void Inject(IEnumerable<Record> records, bool ignoreNull)
        {
            foreach (var record in records)
            {
                var service = FetchService(record.PropInfo, record.ServiceTag);
                if (service == null)
                {
                    if (ignoreNull)
                        continue;

                    Debug.LogError(
                        $"No service found of type {record.PropInfo.PropertyType} to inject in {_target}");
                }

                record.PropInfo.SetValue(_target, service, null);
            }
        }

        private static object FetchService(PropertyInfo prop, string serviceTag)
        {
            object service;
            var targetType = prop.PropertyType;
            var locator = Servicer;
            while (targetType != null && targetType != typeof(object))
            {
                service = locator.Get(targetType, serviceTag);
                if (service != null)
                    return service;

                targetType = targetType.BaseType;
            }

            var interfaces = prop.PropertyType.GetInterfaces();
            foreach (var face in interfaces)
            {
                service = locator.Get(face, serviceTag);

                if (service != null)
                    return service;
            }

            return null;
        }

        private void ClearCache()
        {
            _target = default;
            _mandatoryProperties = null;
            _optionalProperties = null;
        }
    }
}