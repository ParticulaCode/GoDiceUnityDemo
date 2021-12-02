using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace FrostLib.Containers
{
    [Serializable]
    public class FloatRange
    {
        public float Min;
        public float Max;

        public FloatRange() { }

        public FloatRange(float min, float max)
        {
            Min = min;
            Max = max;
        }

        public float Roll() => Random.Range(Min, Max);

        public float Clamp(float value) => Mathf.Clamp(value, Min, Max);

        public float Lerp(float lerpStep) => Mathf.Lerp(Min, Max, lerpStep);

        public float InverseLerp(float value) => Mathf.InverseLerp(Min, Max, value);

        public FloatRange Clone() => new FloatRange(Min, Max);

        public override string ToString() => $"[{Min}; {Max}]";

        public bool IsInRange(float value) => Min <= value && value <= Max;

        public static FloatRange operator/ (FloatRange range, float value)
        {
            range.Min /= value;
            range.Max /= value;
            return range;
        }
    }

    [Serializable]
    public class NormalRange
    {
        [Range(0f, 1f)]
        public float Min;
        [Range(0f, 1f)]
        public float Max;

        public NormalRange() { }

        public NormalRange(float min, float max)
        {
            Min = min;
            Max = max;
        }

        public float Roll() => Random.Range(Min, Max);

        public float Clamp(float value) => Mathf.Clamp01(value);

        public float Lerp(float lerpStep) => Mathf.Lerp(Min, Max, lerpStep);

        public NormalRange Clone() => new NormalRange(Min, Max);

        public override string ToString() => $"[{Min}; {Max}]";

        public static NormalRange operator /(NormalRange range, float value)
        {
            range.Min = Mathf.Clamp01(range.Min / value);
            range.Max = Mathf.Clamp01(range.Max / value);
            return range;
        }
    }

    [Serializable]
    public class IntRange
    {
        public int Min;
        public int Max;

        public int Length => Max - Min;

        public IntRange() { }

        public IntRange(int min, int max)
        {
            Min = min;
            Max = max;
        }

        public int Roll() => Random.Range(Min, Max);

        public int Clamp(int value) => Mathf.Clamp(value, Min, Max);

        public float Lerp(float lerpStep) => Mathf.Lerp(Min, Max, lerpStep);

        public float InverseLerp(float value) => Mathf.InverseLerp(Min, Max, value);
        
        public bool IsInRange(int value) => Min <= value && value <= Max;

        public IntRange Clone() => new IntRange(Min, Max);

        public override string ToString() => $"[{Min}; {Max}]";
    }
}