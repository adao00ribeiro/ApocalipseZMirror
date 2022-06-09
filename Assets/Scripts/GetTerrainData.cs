using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetTerrainData : MonoBehaviour
{
    public TerrainData data;
    public  int alphamapWidht;
    public int alphamapHeight;
    public float [,,] msplatData;
    public int numTexture;
    public int numerotEXTURE;
   public Texture2D texture2d;
    // Start is called before the first frame update
    void Start()
    {
         data = GetComponent<Terrain>().terrainData;
         alphamapWidht = data.alphamapWidth;
         alphamapHeight = data.alphamapHeight;
         msplatData = data.GetAlphamaps (0,0,alphamapWidht , alphamapHeight );
         numTexture = msplatData.Length  / (alphamapWidht  * alphamapHeight);
    }
  
    private Vector3 ConvertToSplatMapCoordinate ( Vector3 playerPos )
    {
        Vector3 vecRet = new Vector3();
        Terrain ter = GetComponent<Terrain>();
        Vector3 terPositon = ter.transform.position;

        vecRet.x = ( ( playerPos.x - terPositon.x ) / ter.terrainData.size.x ) * ter.terrainData.alphamapWidth;
        vecRet.z = ( ( playerPos.z - terPositon.z ) / ter.terrainData.size.z ) * ter.terrainData.alphamapHeight;
        return vecRet;
    }

    public Texture GetTexture ( Vector3 PlayerPosition )
    {
        print ( "terreno ok" );
        int rec = 0;
        Vector3  playerPos = transform.position;
        Vector3 terrainCord = ConvertToSplatMapCoordinate(playerPos);
        float comp = 0 ;

        for ( int i = 0 ; i < numTexture ; i++ )
        {
            if ( comp < msplatData[( int ) terrainCord.z , ( int ) terrainCord.x , i] )
            {
                rec = i;
            }
        }
        numerotEXTURE = rec;
        texture2d = data.terrainLayers[rec].diffuseTexture;
        return data.terrainLayers[rec].diffuseTexture;
    }
}
