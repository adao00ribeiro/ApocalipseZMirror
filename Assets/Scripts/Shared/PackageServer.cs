using DarkRift;

public struct PackageServer : IDarkRiftSerializable
{

    public ServerPlayerData[] ServerPlayerDatas;
   // public ItemData[] itensMap;
    public ushort[] DespawPlayer;
    public int[] DespawItens;
    public BulletData[] bullets;
    public int[] DespawBullets;
    //lista de player armas com status e inventario

    public void Deserialize(DeserializeEvent e)
    {

        ServerPlayerDatas = e.Reader.ReadSerializables<ServerPlayerData>();
     //   itensMap = e.Reader.ReadSerializables<ItemData>();
        DespawPlayer = e.Reader.ReadUInt16s();
        DespawItens = e.Reader.ReadInt32s();
        bullets = e.Reader.ReadSerializables<BulletData>();
        DespawBullets = e.Reader.ReadInt32s();
    }

    public void Serialize(SerializeEvent e)
    {

        e.Writer.Write(ServerPlayerDatas);
     //   e.Writer.Write(itensMap);
        e.Writer.Write(DespawPlayer);
        e.Writer.Write(DespawItens);
        e.Writer.Write(bullets);
        e.Writer.Write(DespawBullets);
    }
}