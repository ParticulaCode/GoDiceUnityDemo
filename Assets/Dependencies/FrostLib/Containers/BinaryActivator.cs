using System;

namespace FrostLib.Containers
{
    public class BinaryActivator : IActivable
    {
        public bool IsActive { get; private set; }
        
        private readonly Action _onActivate;
        private readonly Action _onDeactivate;

        public BinaryActivator(Action onActivate, Action onDeactivate, bool isActive = false)
        {
            _onActivate = onActivate;
            _onDeactivate = onDeactivate;
            IsActive = isActive;
        }

        void IActivable.Activate()
        {
            if (IsActive)
                return;

            IsActive = true;
            _onActivate?.Invoke();
        }

        void IActivable.Deactivate()
        {
            if (!IsActive)
                return;

            IsActive = false;
            _onDeactivate?.Invoke();
        }
    }
}