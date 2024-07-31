using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace FrostLib.Containers
{
    [Serializable]
    public class Chance
    {
        [Range(0f, 1f)]
        [SerializeField]
        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        private float _probability;

        public float Probability
        {
            get => _probability;
            set => _probability = value;
        }

        public Chance(float probability) => _probability = probability;

        public bool RollSuccess() => Random.value <= _probability;

        public bool RollSuccess(float bonus) => Random.value <= (_probability + bonus);

        public override string ToString() => $"{_probability * 100f}%";
    }
}