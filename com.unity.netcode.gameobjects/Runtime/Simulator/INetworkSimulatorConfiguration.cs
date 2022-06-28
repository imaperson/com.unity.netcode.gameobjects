namespace Unity.Netcode
{
    public interface INetworkSimulatorConfiguration
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int PacketDelayMs { get; set; }
        public int PacketJitterMs { get; set; }
        public int PacketLossInterval { get; set; }
        public int PacketLossPercent { get; set; }
        public int PacketDuplicationPercent { get; set; }
    }
}
