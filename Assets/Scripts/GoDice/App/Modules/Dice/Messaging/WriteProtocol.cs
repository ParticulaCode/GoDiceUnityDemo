namespace GoDice.App.Modules.Dice.Messaging
{
    public static class WriteProtocol
    {
        private const sbyte Max = unchecked((sbyte) 0xFF);
        
        public const sbyte Initialization = 0x19;
        public const sbyte Battery = 0x03;
        public const sbyte Sensitivity = 0x06;
        public const sbyte RollSetting = 0x65;
        
        public static class Led
        {
            public const sbyte Toggle = 0x10;
            public const sbyte Constant = 0x08;
            public const sbyte Off = 0x14;
            public const sbyte Infinite = Max;
            public const sbyte BothLeds = 0x00;
            public const sbyte Led1 = 0x01;
            public const sbyte Led2 = 0x02;
        }

        public static class Tap
        {
            public const sbyte Single = 0x31;
            public const sbyte Double = 0x32;

            public const sbyte Disable = 0x00;
            public const sbyte Enable = 0x01;
        }

        public static class Color
        {  
            public const sbyte Set = 0x27;
            public const sbyte Get = 0x17;
        }
    }
}