using System.Collections.Generic;
using GoDice.Shared.Data;
using UnityEngine;

namespace GoDice.App.Modules.Dice.Shells
{
    public static class ShellHolder
    {
        private const int Sensitivity = 30;

        private static readonly Dictionary<ShellType, IShellTransformer> Shells;

        static ShellHolder()
        {
            var d6Shell = new Shell(D6Values(), Sensitivity);
            var d20Shell = new Shell(D20Values(), Sensitivity);
            var d24Shell = new Shell(D24Values(), Sensitivity);

            Shells = new Dictionary<ShellType, IShellTransformer>
            {
                { ShellType.D6, d6Shell },
                { ShellType.D20, d20Shell },
                { ShellType.D4, new ShellTransform(d24Shell, D4Transform()) },
                { ShellType.D8, new ShellTransform(d24Shell, D8Transform()) },
                { ShellType.D10, new ShellTransform(d20Shell, D10Transform()) },
                { ShellType.D12, new ShellTransform(d24Shell, D12Transform()) },
                { ShellType.D10X, new ShellTransform(d20Shell, D10XTransform()) }
            };
        }

        public static IShellTransformer GetShell(ShellType type)
        {
            if (Shells.ContainsKey(type))
                return Shells[type];

            UnityEngine.Debug.LogError("Could not find <IShell> for ShellType " + type);
            return null;
        }

        private static Dictionary<int, Vector3> D6Values() =>
            new Dictionary<int, Vector3>()
            {
                [1] = new Vector3(-64, 0, 0),
                [2] = new Vector3(0, 0, 64),
                [3] = new Vector3(0, 64, 0),
                [4] = new Vector3(0, -64, 0),
                [5] = new Vector3(0, 0, -64),
                [6] = new Vector3(64, 0, 0)
            };

        private static Dictionary<int, Vector3> D20Values() =>
            new Dictionary<int, Vector3>()
            {
                [1] = new Vector3(-42, -42, 42),
                [2] = new Vector3(0, 22, -64),
                [3] = new Vector3(0, 22, 64),
                [4] = new Vector3(42, -42, -42),
                [5] = new Vector3(-42, -42, -42),
                [6] = new Vector3(64, 0, 22),
                [7] = new Vector3(-64, 0, 22),
                [8] = new Vector3(22, 64, 0),
                [9] = new Vector3(42, -42, 40),
                [10] = new Vector3(-22, 64, 0),
                [11] = new Vector3(22, -64, 0),
                [12] = new Vector3(-42, 42, -42),
                [13] = new Vector3(-22, -64, 0),
                [14] = new Vector3(64, 0, -22),
                [15] = new Vector3(-64, 0, -22),
                [16] = new Vector3(42, 42, 42),
                [17] = new Vector3(-42, 42, 42),
                [18] = new Vector3(0, -22, -64),
                [19] = new Vector3(0, -22, 64),
                [20] = new Vector3(42, 42, -42)
            };

        private static Dictionary<int, Vector3> D24Values() =>
            new Dictionary<int, Vector3>()
            {
                [1] = new Vector3(40, -34, -42),
                [2] = new Vector3(-44, -34, 36),
                [3] = new Vector3(18, 55, -21),
                [4] = new Vector3(18, -60, -23),
                [5] = new Vector3(-22, -2, -63),
                [6] = new Vector3(59, -3, 19),
                [7] = new Vector3(39, -33, 39),
                [8] = new Vector3(-44, -32, -43),
                [9] = new Vector3(-24, -60, -24),
                [10] = new Vector3(-24, 54, -22),
                [11] = new Vector3(-64, -4, 16),
                [12] = new Vector3(17, -2, -61),
                [13] = new Vector3(-23, -4, 58),
                [14] = new Vector3(59, -2, -18),
                [15] = new Vector3(19, -60, 19),
                [16] = new Vector3(18, 55, 20),
                [17] = new Vector3(38, 27, 40),
                [18] = new Vector3(-43, 26, -43),
                [19] = new Vector3(-64, -4, -24),
                [20] = new Vector3(17, -2, 60),
                [21] = new Vector3(-24, 54, 18),
                [22] = new Vector3(-24, -60, 19),
                [23] = new Vector3(38, 27, -41),
                [24] = new Vector3(-44, 26, 37)
            };

        private static Dictionary<int, int> D4Transform() =>
            new Dictionary<int, int>()
            {
                [1] = 1,
                [2] = 2,
                [3] = 3,
                [4] = 4,
                [5] = 1,
                [6] = 2,
                [7] = 3,
                [8] = 4,
                [9] = 1,
                [10] = 2,
                [11] = 3,
                [12] = 4,
                [13] = 1,
                [14] = 2,
                [15] = 3,
                [16] = 4,
                [17] = 1,
                [18] = 2,
                [19] = 3,
                [20] = 4,
                [21] = 1,
                [22] = 2,
                [23] = 3,
                [24] = 4
            };

        private static Dictionary<int, int> D8Transform() =>
            new Dictionary<int, int>()
            {
                [1] = 1,
                [2] = 2,
                [3] = 3,
                [4] = 4,
                [5] = 5,
                [6] = 6,
                [7] = 7,
                [8] = 8,
                [9] = 1,
                [10] = 2,
                [11] = 3,
                [12] = 4,
                [13] = 5,
                [14] = 6,
                [15] = 7,
                [16] = 8,
                [17] = 1,
                [18] = 2,
                [19] = 3,
                [20] = 4,
                [21] = 5,
                [22] = 6,
                [23] = 7,
                [24] = 8
            };

        private static Dictionary<int, int> D10Transform() =>
            new Dictionary<int, int>()
            {
                [1] = 1,
                [2] = 2,
                [3] = 3,
                [4] = 4,
                [5] = 5,
                [6] = 6,
                [7] = 7,
                [8] = 8,
                [9] = 9,
                [10] = 10,
                [11] = 1,
                [12] = 2,
                [13] = 3,
                [14] = 4,
                [15] = 5,
                [16] = 6,
                [17] = 7,
                [18] = 8,
                [19] = 9,
                [20] = 10
            };

        private static Dictionary<int, int> D12Transform() =>
            new Dictionary<int, int>()
            {
                [1] = 1,
                [2] = 2,
                [3] = 3,
                [4] = 4,
                [5] = 5,
                [6] = 6,
                [7] = 7,
                [8] = 8,
                [9] = 9,
                [10] = 10,
                [11] = 11,
                [12] = 12,
                [13] = 1,
                [14] = 2,
                [15] = 3,
                [16] = 4,
                [17] = 5,
                [18] = 6,
                [19] = 7,
                [20] = 8,
                [21] = 9,
                [22] = 10,
                [23] = 11,
                [24] = 12
            };

        private static Dictionary<int, int> D10XTransform() =>
            new Dictionary<int, int>()
            {
                [1] = 0,
                [2] = 10,
                [3] = 20,
                [4] = 30,
                [5] = 40,
                [6] = 50,
                [7] = 60,
                [8] = 70,
                [9] = 80,
                [10] = 90,
                [11] = 0,
                [12] = 10,
                [13] = 20,
                [14] = 30,
                [15] = 40,
                [16] = 50,
                [17] = 60,
                [18] = 70,
                [19] = 80,
                [20] = 90
            };
    }
}