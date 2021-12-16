using System.Collections;
using GoDice.App.Modules.Dice.Core;
using GoDice.App.Modules.Dice.Data;
using GoDice.Shared.Data;
using UnityEngine;
using UnityEngine.UI;

namespace GoDice.App.Modules.Dice.Views
{
    [AddComponentMenu("GoDice/App/Dice/[App] Die2d View")]
    internal class Die2dView : MonoBehaviour
    {
        [SerializeField] private Image _value;
        [SerializeField] private DieValueSprites _dieValues;
        [SerializeField] private ColorMap _colorMap;

        public Die Die { get; private set; }

        private Coroutine _rollRoutine;

        public void SetDie(Die die)
        {
            Unsubscribe();

            Die = die;

            if (Die == null)
                return;

            Die.Value.OnChange.AddListener(UpdateValue);
            Die.Color.OnChange.AddListener(UpdateColor);

            UpdateColor(die.Color.Value);
        }

        private void UpdateColor(ColorType type) => _value.color = _colorMap.GetColor(type);

        private void UpdateValue(int newValue) =>
            SetValueSprite(_dieValues.GetValueSprite(newValue));

        private void SetValueSprite(Sprite spr) => _value.sprite = spr;

        private void OnDestroy() => Unsubscribe();

        private void Unsubscribe()
        {
            Die?.Value.OnChange.RemoveListener(UpdateValue);
            Die?.Color.OnChange.AddListener(UpdateColor);
        }

        public void StartRoll()
        {
            StopRollRoutine();

            if (gameObject.activeInHierarchy)
                _rollRoutine = StartCoroutine(RollRoutined());
        }

        private void StopRollRoutine()
        {
            if (_rollRoutine == null)
                return;

            StopCoroutine(_rollRoutine);
            _rollRoutine = null;
        }

        private IEnumerator RollRoutined()
        {
            var wait = new WaitForSeconds(0.1f);
            while (true)
            {
                var anotherSprite = _dieValues.GetRandomValueSpriteExcept(_value.sprite);
                SetValueSprite(anotherSprite);
                yield return wait;
            }
        }

        public void EndRoll(int value)
        {
            StopRollRoutine();
            UpdateValue(value);
        }
    }
}