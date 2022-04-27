using System.Collections;
using System.Collections.Generic;
using DarkRift;

public struct PackageClient : IDarkRiftSerializable
{
    public InputData inputData;
    public void Deserialize(DeserializeEvent e)
    {
        inputData = e.Reader.ReadSerializable<InputData>();


    }

    public void Serialize(SerializeEvent e)
    {
        e.Writer.Write(inputData);

    }
}