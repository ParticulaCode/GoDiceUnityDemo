using UnityEngine;

namespace GoDice.Utils
{
    public static class Colorizer
    {
        public static string AsError(string s) => Wrap(s, Color.red);

        public static string AsAddress(string s) => Wrap(s, Color.yellow);

        public static string AsDie(string s) => Wrap(s, Color.white);

        public static string AsDevice(string s) => Wrap(s, Color.green);

        public static string AsOperation(string address, string op)
        {
            var die = AsAddress($"[{address}]");
            var operation = Wrap(op, Color.magenta);
            return $"{die} {operation} operation";
        }

        public static string Wrap(string s, Color color)
        {
            if (!Debug.isDebugBuild)
                return s;

            var hex = ColorUtility.ToHtmlStringRGBA(color);
            return $"<color=#{hex}>{s}</color>";
        }
    }
}