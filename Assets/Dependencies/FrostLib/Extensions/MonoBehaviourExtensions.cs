using UnityEngine;

namespace FrostLib.Extensions
{
    public static class MonoBehaviourExtensions
    {
        public static void Deactivate(this MonoBehaviour behaviour) => behaviour.gameObject.SetActive(false);

        public static void Activate(this MonoBehaviour behaviour) => behaviour.gameObject.SetActive(true);
        
        public static void DestroySelf(this MonoBehaviour behaviour) => Object.Destroy(behaviour.gameObject);
    }
}