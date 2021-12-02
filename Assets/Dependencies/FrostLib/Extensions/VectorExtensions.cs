using System;
using UnityEngine;

namespace FrostLib.Extensions
{
    public static class VectorExtensions
    {
        public static Vector2 Rotate(this Vector2 v, float degrees)
        {
            var radians = degrees * Mathf.Deg2Rad;
            var sin = Mathf.Sin(radians);
            var cos = Mathf.Cos(radians);

            var tx = v.x;
            var ty = v.y;

            return new Vector2(cos * tx - sin * ty, sin * tx + cos * ty);
        }

        // ReSharper disable once InconsistentNaming
        public static Vector2 xy(this Vector3 v) => new Vector2(v.x, v.y);

        public static Vector3 With(this Vector3 original, float? x = null, float? y = null, float? z = null)
        {
            var result = Vector3.zero;
            result.x = x ?? original.x;
            result.y = y ?? original.y;
            result.z = z ?? original.z;
            return result;
        }

        public static bool Approximately(this Vector3 a, Vector3 b, float tolerance = 0.0001f) => 
            ApproxTolerance(a.x, b.x, tolerance) && ApproxTolerance(a.y, b.y, tolerance) && ApproxTolerance(a.z, b.z, tolerance);

        private static bool ApproxTolerance(float a, float b, float tolerance) => Math.Abs(a - b) < tolerance;

        public static Vector2 Snap(this Vector3 v)
        {
            v.x = Mathf.Round(v.x);
            v.y = Mathf.Round(v.y);
            v.z = Mathf.Round(v.z);
            return v;
        }
    }
}