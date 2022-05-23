using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using ApocalipseZ;
public static class SItemReadWrite 
{
    public static void WriteSItem ( this NetworkWriter writer , SItem value )
    {

     
    }
    public static SItem ReadSItem ( this NetworkReader reader )
    {
      
        return new SItem (  );
    }
}
