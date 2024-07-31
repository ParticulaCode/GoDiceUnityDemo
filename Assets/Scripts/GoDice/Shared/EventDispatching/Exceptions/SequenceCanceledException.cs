using System;

namespace GoDice.Shared.EventDispatching.Exceptions
{
    public class SequenceCanceledException : Exception
    {
        public SequenceCanceledException(string message) : base(message)
        {
        }
    }
}