using System;
using UnityEngine;

// ReSharper disable InconsistentNaming

namespace FrostLib.Containers
{
    [Serializable]
    public struct IntPoint
    {
        public static readonly IntPoint zero = new IntPoint(0, 0);

        public int x;
        public int y;

        public IntPoint(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public static implicit operator Vector2Int(IntPoint point) => new Vector2Int(point.x, point.y);
        
        public static implicit operator Vector2(IntPoint point) => new Vector2(point.x, point.y);

        public static implicit operator IntPoint(Vector2Int vec) => new IntPoint(vec.x, vec.y);

        public override string ToString() => $"[{x}, {y}]";
    }
}