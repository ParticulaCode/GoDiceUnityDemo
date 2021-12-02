using UnityEngine;

namespace FrostLib.Extensions
{
    public static class TransformExtensions
    {
        public static Vector3 DirectionTo(this Transform transform, Vector3 destination) =>
            Vector3.Normalize(destination - transform.position);

        public static void SetPositionY(this Transform t, float y)
        {
            var pos = t.position;
            pos.y = y;
            t.position = pos;
        }

        public static void SetPositionX(this Transform t, float x)
        {
            var pos = t.position;
            pos.x = x;
            t.position = pos;
        }

        public static void SetPositionZ(this Transform t, float z)
        {
            var pos = t.position;
            pos.z = z;
            t.position = pos;
        }

        public static void SetLocalPositionY(this Transform t, float y)
        {
            var pos = t.localPosition;
            pos.y = y;
            t.localPosition = pos;
        }

        public static void SetLocalPositionX(this Transform t, float x)
        {
            var pos = t.localPosition;
            pos.x = x;
            t.localPosition = pos;
        }

        public static void SetLocalPositionZ(this Transform t, float z)
        {
            var pos = t.localPosition;
            pos.z = z;
            t.localPosition = pos;
        }

        public static void SetLocalPosition2D(this Transform t, Vector2 vector2)
        {
            var pos = (Vector3) vector2;
            pos.z = t.localPosition.z;
            t.localPosition = pos;
        }

        public static void SetPosition2D(this Transform t, Vector2 vector2)
        {
            var pos = (Vector3) vector2;
            pos.z = t.position.z;
            t.position = pos;
        }

        public static void SetAngles(this Transform t, float z)
        {
            var angles = t.localEulerAngles;
            angles.z = z;
            t.localEulerAngles = angles;
        }

        public static void SetScaleX(this Transform t, float f)
        {
            var scale = t.localScale;
            scale.x = f;
            t.localScale = scale;
        }

        public static void SetLayerRecursive(this Transform t, int value)
        {
            foreach (Transform child in t)
                SetLayerRecursive(child, value);

            t.gameObject.layer = value;
        }

        public static Transform[] GetDirectChildren(this Transform t)
        {
            var children = new Transform[t.childCount];
            for (var i = 0; i < children.Length; i++)
                children[i] = t.GetChild(i);

            return children;
        }

        public static void ResetLocalPosition(this Transform t) => t.localPosition = Vector3.zero;
        
        public static void SnapLocalPosition(this Transform t) => t.localPosition = t.localPosition.Snap();

        public static void DestroyChildren(this Transform t)
        {
            var isPlaying = Application.isPlaying;

            while (t.childCount != 0)
            {
                var child = t.GetChild(0);

                if (isPlaying)
                {
                    child.parent = null;
                    Object.Destroy(child.gameObject);
                }
                else
                {
                    Object.DestroyImmediate(child.gameObject);
                }
            }
        }

        public static T AddChild<T>(this GameObject parent, GameObject prefab) where T : Component =>
            AddChild(parent, prefab).GetComponent<T>();

        public static GameObject AddChild(this GameObject parent, GameObject prefab) =>
            AddChild(parent.transform, prefab);

        public static T AddChild<T>(this Transform parent, GameObject prefab) where T : Component =>
            AddChild(parent, prefab).GetComponent<T>();
        
        public static GameObject AddChild(this Transform parent, GameObject prefab)
        {
            var go = parent.AddChildIgnoreTransform(prefab);
            var t = go.transform;
            t.localPosition = Vector3.zero;
            t.localRotation = Quaternion.identity;
            t.localScale = Vector3.one;
            return go;
        }

        public static GameObject AddChildIgnoreTransform(this Transform parent, GameObject prefab)
        {
            var go = Object.Instantiate(prefab, parent);
            var t = go.transform;
            t.SetParent(parent, false);
            go.SetActive(true);
            return go;
        }
    }
}