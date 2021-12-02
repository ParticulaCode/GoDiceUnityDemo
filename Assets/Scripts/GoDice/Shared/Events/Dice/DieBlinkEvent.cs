using System;
using UnityEngine;
using static GoDice.Shared.EventDispatching.Events.EventType;

namespace GoDice.Shared.Events.Dice
{
    public class DieBlinkEvent : DieEvent
    {
        public override EventDispatching.Events.EventType Type => BlinkDie;

        public readonly int BlinksAmount;
        public readonly Color Color;
        public readonly float OnDuration;
        public readonly float OffDuration;
        public readonly bool IsMixed;

        public DieBlinkEvent(Guid dieId, int blinksAmount, Color color, float onDuration,
            float offDuration, bool isMixed) : base(dieId)
        {
            BlinksAmount = blinksAmount;
            Color = color;
            OnDuration = onDuration;
            OffDuration = offDuration;
            IsMixed = isMixed;
        }
    }
}