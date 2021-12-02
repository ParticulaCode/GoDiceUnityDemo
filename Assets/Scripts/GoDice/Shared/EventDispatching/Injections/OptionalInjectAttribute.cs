namespace GoDice.Shared.EventDispatching.Injections
{
    public class OptionalInjectAttribute : TaggedAttribute
    {
        public OptionalInjectAttribute(string tag = "") : base(tag)
        {
        }
    }
}