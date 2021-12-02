using System;
using System.Collections.Generic;
using System.Linq;

namespace FrostLib.Extensions
{
    public static class EnumerableExtensions
    {
        public static T First<T>(this T[] source)
        {
            if (source == null || source.Length == 0)
                return default(T);

            return source[0];
        }

        public static T Last<T>(this T[] source)
        {
            if (source == null || source.Length == 0)
                return default(T);

            return source[source.Length - 1];
        }

        public static T First<T>(this IList<T> source)
        {
            if (source == null || source.Count == 0)
                return default(T);

            return source[0];
        }

        public static T Last<T>(this IList<T> source)
        {
            if (source == null || source.Count == 0)
                return default(T);

            return source[source.Count - 1];
        }

        public static T PickRandom<T>(this IList<T> source)
        {
            if (source == null || source.Count == 0)
                return default(T);

            return source.Count == 1 ? source[0] : source[UnityEngine.Random.Range(0, source.Count)];
        }

        public static T RandomByWeight<T>(this IEnumerable<T> sequence, Func<T, float> weightSelector)
        {
            if (sequence == null)
                return default(T);

            var enumerable = sequence as T[] ?? sequence.ToArray();

            switch (enumerable.Length)
            {
                case 0:
                    return default(T);
                case 1:
                    return enumerable[0];
            }

            var totalWeight = enumerable.Sum(weightSelector);

            // The weight we are after...
            var itemWeightIndex = UnityEngine.Random.value * totalWeight;
            float currentWeightIndex = 0;

            foreach (var item in from weightedItem in enumerable
                select new { Value = weightedItem, Weight = weightSelector(weightedItem) })
            {
                currentWeightIndex += item.Weight;

                // If we've hit or passed the weight we are after for this item then it's the one we want....
                if (currentWeightIndex >= itemWeightIndex)
                    return item.Value;
            }

            return default(T);
        }

        public static void Shuffle<T>(this IList<T> list)
        {
            var total = list.Count;
            var n = list.Count;
            while (n > 1)
            {
                n--;
                var k = UnityEngine.Random.Range(0, total);
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static T PickRandom<T>(this T[] arr)
        {
            if (arr == null || arr.Length == 0)
                return default(T);

            var idx = UnityEngine.Random.Range(0, arr.Length);
            return arr[idx];
        }

        public static TSource[] RemoveAt<TSource>(this TSource[] arr, int idx)
        {
            var list = new List<TSource>(arr);
            list.RemoveAt(idx);
            return list.ToArray();
        }

        public static TSource[] Remove<TSource>(this TSource[] arr, TSource item)
        {
            var list = new List<TSource>(arr);
            list.Remove(item);
            return list.ToArray();
        }

        public static TSource[] Add<TSource>(this TSource[] arr, TSource item) =>
            new List<TSource>(arr) { item }.ToArray();

        public static KeyValuePair<TKey, TValue> PickRandom<TKey, TValue>(
            this Dictionary<TKey, TValue> dict)
        {
            if (dict == null || dict.Count == 0)
                return default(KeyValuePair<TKey, TValue>);

            var idx = UnityEngine.Random.Range(0, dict.Count);
            var enumerator = dict.GetEnumerator();
            for (var i = 0; i < idx; i++)
                enumerator.MoveNext();

            var result = enumerator.Current;
            enumerator.Dispose();

            return result;
        }

        public static void RemoveAny<T>(this List<T> source, Func<T, bool> action)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            foreach (var element in source.Where(action))
            {
                source.Remove(element);
                return;
            }
        }

        public static bool IsEmpty<T>(this T[] source) => source == null || source.Length == 0;

        public static bool IsEmpty<T>(this IList<T> source) => source == null || source.Count == 0;
    }
}