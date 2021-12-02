using System;

namespace GoDice.Shared.EventDispatching.Injections
{
    public abstract class TaggedAttribute : Attribute
    {
        public readonly string Tag;

        protected TaggedAttribute(string tag = "") => Tag = tag;
    }
}