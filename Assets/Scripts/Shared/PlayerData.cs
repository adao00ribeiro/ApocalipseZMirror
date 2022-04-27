using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkRift;

public struct PlayerData : IDarkRiftSerializable
{

    public float Health;
    public float Stamina;
    public float Nutrition;
    public float Hydration;

    public Vector3 position;
    public Vector3 rotation;

    public Vector3 positionSpringArms;
    public Vector3 rotationSpringArms;

    public void Deserialize(DeserializeEvent e)
    {

        Health = e.Reader.ReadSingle();
        Stamina = e.Reader.ReadSingle();
        Nutrition = e.Reader.ReadSingle();
        Hydration = e.Reader.ReadSingle();

        position = new Vector3(e.Reader.ReadSingle(), e.Reader.ReadSingle(), e.Reader.ReadSingle());
        rotation = new Vector3(e.Reader.ReadSingle(), e.Reader.ReadSingle(), e.Reader.ReadSingle());
        positionSpringArms = new Vector3(e.Reader.ReadSingle(), e.Reader.ReadSingle(), e.Reader.ReadSingle());
        rotationSpringArms = new Vector3(e.Reader.ReadSingle(), e.Reader.ReadSingle(), e.Reader.ReadSingle());


    }

    public void Serialize(SerializeEvent e)
    {

        e.Writer.Write(Health);
        e.Writer.Write(Stamina);
        e.Writer.Write(Nutrition);
        e.Writer.Write(Hydration);

        e.Writer.Write(position.x);
        e.Writer.Write(position.y);
        e.Writer.Write(position.z);

        e.Writer.Write(rotation.x);
        e.Writer.Write(rotation.y);
        e.Writer.Write(rotation.z);

        e.Writer.Write(positionSpringArms.x);
        e.Writer.Write(positionSpringArms.y);
        e.Writer.Write(positionSpringArms.z);

        e.Writer.Write(rotationSpringArms.x);
        e.Writer.Write(rotationSpringArms.y);
        e.Writer.Write(rotationSpringArms.z);


    }
}