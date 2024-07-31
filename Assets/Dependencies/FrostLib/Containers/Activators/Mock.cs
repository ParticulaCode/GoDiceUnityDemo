namespace FrostLib.Containers.Activators
{
    public class Mock : IActivable
    {
        public bool IsActive { get; }

        public Mock(bool isActive) => IsActive = isActive;

        public void Activate()
        {
        }

        public void Deactivate()
        {
        }
    }
}