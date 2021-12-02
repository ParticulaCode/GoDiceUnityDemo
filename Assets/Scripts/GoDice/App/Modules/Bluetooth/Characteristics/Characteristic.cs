namespace GoDice.App.Modules.Bluetooth.Characteristics
{
    public readonly struct Characteristic
    {
        public string Address { get; }
        public string Service { get; }
        public string Id { get; }

        public Characteristic(string address, string service, string id)
        {
            Address = address;
            Service = service;
            Id = id;
        }
    }
}