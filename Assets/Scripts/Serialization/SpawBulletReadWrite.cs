using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public struct SpawBulletTransform
{
    
    public Vector3 Position;
    public Quaternion Rotation;

    public SpawBulletTransform ( Vector3 _position, Quaternion _rotation)
    {
        Position = _position;
        Rotation = _rotation;
    }
}
public static class SpawBulletReadWrite
{
    public static void WriteSpawBulletTransform ( this NetworkWriter writer , SpawBulletTransform value )
    {
        writer.WriteVector3 ( value .Position);
        writer.WriteQuaternion ( value.Rotation);
    }

    public static SpawBulletTransform ReadSpawBulletTransform ( this NetworkReader reader )
    {
        return new SpawBulletTransform ( reader.ReadVector3(), reader.ReadQuaternion());
    }

}