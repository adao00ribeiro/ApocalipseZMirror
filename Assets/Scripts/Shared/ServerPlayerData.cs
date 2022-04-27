using DarkRift;

public struct ServerPlayerData : IDarkRiftSerializable
{
    public ushort id;
    public InputData inputData;
    public PlayerData playerData;
    public InventoryData inventoryData;


    public void Deserialize(DeserializeEvent e)
    {
        id = e.Reader.ReadUInt16();
        inputData = e.Reader.ReadSerializable<InputData>();
        playerData = e.Reader.ReadSerializable<PlayerData>();
        inventoryData = e.Reader.ReadSerializable<InventoryData>();

    }

    public void Serialize(SerializeEvent e)
    {
        e.Writer.Write(id);
        e.Writer.Write(inputData);
        e.Writer.Write(playerData);
        e.Writer.Write(inventoryData);
    }
}