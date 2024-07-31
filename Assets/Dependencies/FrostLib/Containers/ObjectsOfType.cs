using System;
using MoreLinq;
using UnityEngine;

namespace FrostLib.Containers
{
    public class ObjectsOfType<T> where T : MonoBehaviour
    {
        public bool IsEmpty => _elements.Length == 0;

        private readonly T[] _elements;

        public ObjectsOfType() =>
            _elements = UnityEngine.Object.FindObjectsByType<T>(FindObjectsSortMode.None);

        public void ForEach(Action<T> action) => _elements.ForEach(action.Invoke);
    }
}