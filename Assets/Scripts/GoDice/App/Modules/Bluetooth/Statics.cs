// ReSharper disable InconsistentNaming
namespace GoDice.App.Modules.Bluetooth
{
    public static class Statics
    {
        public static readonly string ServiceUUID = FullUUID("0001");
        public static readonly string ReadCharacteristicUUID = FullUUID("0003");
        public static readonly string WriteCharacteristicUUID = FullUUID("0002");
        
        private static string FullUUID(string uuid) => "6E40" + uuid + "-B5A3-F393-E0A9-E50E24DCCA9E";
    }
}