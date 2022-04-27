using DarkRift;
using UnityEngine;
public struct InputData : IDarkRiftSerializable
{
    public ushort Id;
    public float Horizontal;
    public float Vertical;
    public bool IsFire;
    public bool IsRun;
    public bool IsJump;
    public bool IsGround;
    public bool IsAiming;
    public bool IsCrouching;
    public bool IsDead;
    public bool IsInventory;
    public bool IsInteract;
    public bool IsSwitch;
    public float AnguloAim;
    public Vector2 MouseXY;


    public void Deserialize(DeserializeEvent e)
    {
        Id = e.Reader.ReadUInt16();
        Horizontal = e.Reader.ReadSingle();
        Vertical = e.Reader.ReadSingle();
        IsFire = e.Reader.ReadBoolean();
        IsRun = e.Reader.ReadBoolean();
        IsJump = e.Reader.ReadBoolean();
        IsGround = e.Reader.ReadBoolean();
        IsAiming = e.Reader.ReadBoolean();
        IsCrouching = e.Reader.ReadBoolean();
        IsDead = e.Reader.ReadBoolean();
        IsInventory = e.Reader.ReadBoolean();
        IsInteract = e.Reader.ReadBoolean();
        IsSwitch = e.Reader.ReadBoolean();
        AnguloAim = e.Reader.ReadSingle();
        MouseXY = new Vector2(e.Reader.ReadSingle(), e.Reader.ReadSingle());


    }

    public void Serialize(SerializeEvent e)
    {
        e.Writer.Write(Id);
        e.Writer.Write(Horizontal);
        e.Writer.Write(Vertical);
        e.Writer.Write(IsFire);
        e.Writer.Write(IsRun);
        e.Writer.Write(IsJump);
        e.Writer.Write(IsGround);
        e.Writer.Write(IsAiming);
        e.Writer.Write(IsCrouching);
        e.Writer.Write(IsDead);
        e.Writer.Write(IsInventory);
        e.Writer.Write(IsInteract);
        e.Writer.Write(IsSwitch);
        e.Writer.Write(AnguloAim);
        e.Writer.Write(MouseXY.x);
        e.Writer.Write(MouseXY.y);

    }
}
