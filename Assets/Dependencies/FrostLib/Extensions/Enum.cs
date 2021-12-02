using System;
using System.Linq;

namespace FrostLib.Extensions
{
    /// <summary> Enum Extension Methods </summary>
    /// <typeparam name="T"> type of Enum </typeparam>
    public class Enum<T> where T : System.Enum
    {
        public static T[] GetEnumValues()
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("T must be an enumerated type");

            return typeof(T).GetEnumValues().Cast<T>().ToArray();
        }

        public static int Count
        {
            get
            {
                if (!typeof(T).IsEnum)
                    throw new ArgumentException("T must be an enumerated type");

                return typeof(T).GetEnumNames().Length;
            }
        }
    }
}