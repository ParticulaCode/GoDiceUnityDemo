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
                [1] = new Vector3(-64, 0, -22),
                [2] = new Vector3(42, -42, 40),
                [3] = new Vector3(0, 22, -64),
                [4] = new Vector3(0, 22, 64),
                [5] = new Vector3(-42, -42, 42),
                [6] = new Vector3(22, 64, 0),
                [7] = new Vector3(-42, -42, -42),
                [8] = new Vector3(64, 0, -22),
                [9] = new Vector3(-22, 64, 0),
                [10] = new Vector3(42, -42, -42),
                [11] = new Vector3(-42, 42, 42),
                [12] = new Vector3(22, -64, 0),
                [13] = new Vector3(-64, 0, 22),
                [14] = new Vector3(42, 42, 42),
                [15] = new Vector3(-22, -64, 0),
                [16] = new Vector3(42, 42, -42),
                [17] = new Vector3(0, -22, -64),
                [18] = new Vector3(0, -22, 64),
                [19] = new Vector3(-42, 42, -42),
                [20] = new Vector3(64, 0, 22)
            };

        private static Dictionary<int, Vector3> D24Values() =>
            new Dictionary<int, Vector3>()
            {
                [1] = new Vector3(20, -60, -20),
                [2] = new Vector3(20, 0, 60),
                [3] = new Vector3(-40, -40, 40),
                [4] = new Vector3(-60, 0, 20),
                [5] = new Vector3(40, 20, 40),
                [6] = new Vector3(-20, -60, -20),
                [7] = new Vector3(20, 60, 20),
                [8] = new Vector3(-40, 20, -40),
                [9] = new Vector3(-40, 40, 40),
                [10] = new Vector3(-20, 0, 60),
                [11] = new Vector3(-20, -60, 20),
                [12] = new Vector3(60, 0, 20),
                [13] = new Vector3(-60, 0, -20),
                [14] = new Vector3(20, 60, -20),
                [15] = new Vector3(20, 0, -60),
                [16] = new Vector3(40, -20, -40),
                [17] = new Vector3(-20, 60, -20),
                [18] = new Vector3(-40, -40, -40),
                [19] = new Vector3(40, -20, 40),
                [20] = new Vector3(20, -60, 20),
                [21] = new Vector3(60, 0, -20),
                [22] = new Vector3(40, 20, -40),
                [23] = new Vector3(-20, 0, -60),
                [24] = new Vector3(-20, 60, 20)
            };

        private static Dictionary<int, int> D4Transform() =>
            new Dictionary<int, int>()
            {
                [1] = 3,
                [2] = 1,
                [3] = 4,
                [4] = 1,
                [5] = 4,
                [6] = 4,
                [7] = 1,
                [8] = 4,
                [9] = 2,
                [10] = 3,
                [11] = 1,
                [12] = 1,
                [13] = 1,
                [14] = 4,
                [15] = 2,
                [16] = 3,
                [17] = 3,
                [18] = 2,
                [19] = 2,
                [20] = 2,
                [21] = 4,
                [22] = 1,
                [23] = 3,
                [24] = 2 
            };

        private static Dictionary<int, int> D8Transform() =>
            new Dictionary<int, int>()
            {
                [1] = 3,
                [2] = 3,
                [3] = 6,
                [4] = 1,
                [5] = 2,
                [6] = 8,
                [7] = 1,
                [8] = 1,
                [9] = 4,
                [10] = 7,
                [11] = 5,
                [12] = 5,
                [13] = 4,
                [14] = 4,
                [15] = 2,
                [16] = 5,
                [17] = 7,
                [18] = 7,
                [19] = 8,
                [20] = 2,
                [21] = 8,
                [22] = 3,
                [23] = 6,
                [24] = 6
            };

        private static Dictionary<int, int> D10Transform() =>
            new Dictionary<int, int>()
            {
                [1] = 8,
                [2] = 2,
                [3] = 6,
                [4] = 1,
                [5] = 4,
                [6] = 3,
                [7] = 9,
                [8] = 0,
                [9] = 7,
                [10] = 5,
                [11] = 5,
                [12] = 7,
                [13] = 0,
                [14] = 9,
                [15] = 3,
                [16] = 4,
                [17] = 1,
                [18] = 6,
                [19] = 2,
                [20] = 8
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
                [1] = 80,
                [2] = 20,
                [3] = 60,
                [4] = 10,
                [5] = 40,
                [6] = 30,
                [7] = 90,
                [8] = 0,
                [9] = 70,
                [10] = 50,
                [11] = 50,
                [12] = 70,
                [13] = 0,
                [14] = 90,
                [15] = 30,
                [16] = 40,
                [17] = 10,
                [18] = 60,
                [19] = 20,
                [20] = 80
            };
    }
}