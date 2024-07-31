using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace FrostLib.Containers
{
    [Serializable]
    public class Area
    {
        [SerializeField]
        public Vector3 Center;
        [SerializeField]
        public Vector3 Bounds;

        public Area() { }

        public Area(Vector3 center, Vector3 bounds)
        {
            Center = center;
            Bounds = bounds;
        }

        public Area(Area area)
        {
            Center = area.Center;
            Bounds = area.Bounds;
        }

        public virtual Vector3 PickPoint()
        {
            var point = Center;
            point.x += Random.Range(-Bounds.x, Bounds.x);
            point.y += Random.Range(-Bounds.y, Bounds.y);
            point.z += Random.Range(-Bounds.z, Bounds.z);
            return point;
        }

        public void DrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(Center, Bounds * 2f);
        }

        public override string ToString() => $"{typeof(Area)}: Center {Center}, Bounds {Bounds}";
    }
}
