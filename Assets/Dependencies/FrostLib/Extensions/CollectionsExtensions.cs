#if !UNITY_WINRT || UNITY_EDITOR || UNITY_WP8

#region License

// Copyright (c) 2007 James Newton-King
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.

#endregion

using System;
using System.Collections.Generic;

#if (UNITY_IPHONE || UNITY_IOS)
using System.Linq;
#endif

namespace FrostLib.Extensions
{
    public static class CollectionsExtensions
    {
        public static bool AddDistinct<T>(this IList<T> list, T value) =>
            list.AddDistinct(value, EqualityComparer<T>.Default);

        public static bool AddDistinct<T>(this IList<T> list, T value, IEqualityComparer<T> comparer)
        {
            if (list.ContainsValue(value, comparer))
                return false;

            list.Add(value);
            return true;
        }

        // this is here because LINQ Bridge doesn't support Contains with IEqualityComparer<T>
        public static bool ContainsValue<TSource>(this IEnumerable<TSource> source, TSource value,
            IEqualityComparer<TSource> comparer)
        {
            if (comparer == null)
                comparer = EqualityComparer<TSource>.Default;

            if (source == null)
                throw new ArgumentNullException("source");

#if (UNITY_IPHONE || UNITY_IOS)
			var sourceArray = source.ToArray();
			for (var i = 0; i < sourceArray.Length; i++)
			{
				if (comparer.Equals(sourceArray[i], value))
					return true;
			}

#else
            foreach (var local in source)
                if (comparer.Equals(local, value))
                    return true;
#endif

            return false;
        }

        public static bool AddRangeDistinct<T>(this IList<T> list, IEnumerable<T> values) =>
            list.AddRangeDistinct(values, EqualityComparer<T>.Default);

        public static bool AddRangeDistinct<T>(this IList<T> list, IEnumerable<T> values,
            IEqualityComparer<T> comparer)
        {
            var allAdded = true;

#if (UNITY_IPHONE || UNITY_IOS)
			var valueArray = values.ToArray();
			for (var i = 0; i < valueArray.Length; i++)
			{
				if (!list.AddDistinct(valueArray[i], comparer))
					allAdded = false;
			}
#else
            foreach (var value in values)
                if (!list.AddDistinct(value, comparer))
                    allAdded = false;
#endif
            return allAdded;
        }

        public static int IndexOf<T>(this IEnumerable<T> collection, Func<T, bool> predicate)
        {
#if (UNITY_IPHONE || UNITY_IOS)
			var collArray = collection.ToArray();
			for (var i = 0; i < collArray.Length; i++)
			{
				if (predicate(collArray[i]))
					return i;
			}

#else
            var index = 0;
            foreach (var value in collection)
            {
                if (predicate(value))
                    return index;

                index++;
            }
#endif


            return -1;
        }

        /// <summary>
        /// Returns the index of the first occurrence in a sequence by using the default equality comparer.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="list">A sequence in which to locate a value.</param>
        /// <param name="value">The object to locate in the sequence</param>
        /// <returns>The zero-based index of the first occurrence of value within the entire sequence, if found; otherwise, �1.</returns>
        public static int IndexOf<TSource>(this IEnumerable<TSource> list, TSource value)
            where TSource : IEquatable<TSource> =>
            list.IndexOf<TSource>(value, EqualityComparer<TSource>.Default);

        /// <summary>
        /// Returns the index of the first occurrence in a sequence by using a specified IEqualityComparer.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="list">A sequence in which to locate a value.</param>
        /// <param name="value">The object to locate in the sequence</param>
        /// <param name="comparer">An equality comparer to compare values.</param>
        /// <returns>The zero-based index of the first occurrence of value within the entire sequence, if found; otherwise, �1.</returns>
        public static int IndexOf<TSource>(this IEnumerable<TSource> list, TSource value,
            IEqualityComparer<TSource> comparer)
        {
#if (UNITY_IPHONE || UNITY_IOS)
			var listArray = list.ToArray();
			for (var i = 0; i < listArray.Length; i++)
			{
				if (comparer.Equals(listArray[i], value))
				{
					return i;
				}
			}
#else
            var index = 0;
            foreach (var item in list)
            {
                if (comparer.Equals(item, value))
                    return index;

                index++;
            }
#endif
            return -1;
        }
    }
}
#endif