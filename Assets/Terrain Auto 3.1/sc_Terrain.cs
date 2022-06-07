using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public enum Depth { Bit8 = 1, Bit16 = 2 }
public enum ByteOrder { Mac = 1, Windows = 2 }
public class sc_Terrain
{

    public float distance = 0.3f;
    private string path = string.Empty;
    private Vector2 montante;
    //config raw
    public Depth m_Depth = Depth.Bit16;
    public int m_Resolution = 1;
    public ByteOrder m_ByteOrder = ByteOrder.Windows;
    public bool m_FlipVertically = false;

    public void CreateTerrain(Transform parent, Vector2 montante, int width, int height, int lenght)
    {
        this.montante = montante;
        ValidatePath();

        int count = 0;
        for (int i = 0; i < montante.x; i++)
        {
            for (int j = 0; j < montante.y; j++)
            {

                TerrainData terrainData = new TerrainData();

                terrainData.baseMapResolution = 1024;
                terrainData.heightmapResolution = 513;
                terrainData.alphamapResolution = 512;
                terrainData.SetDetailResolution(1024, 8);
                terrainData.size = new Vector3(width, height, lenght);

                terrainData.name = count.ToString();
                GameObject terrain = (GameObject)Terrain.CreateTerrainGameObject(terrainData);

                terrain.name = count.ToString();
                terrain.transform.parent = parent.transform;


                terrain.transform.position = new Vector3(width * j, 0, -width * i);


                AssetDatabase.CreateAsset(terrainData, "Assets/" + path + count.ToString() + ".asset");
                count++;
            }
        }

    }

    private void ValidatePath()
    {

        if (path == string.Empty)
        {
            path = "TiledTerrain/TerrainData/";
        }
        Debug.Log(path);
        string pathToCheck = Application.dataPath + "/" + path;
        if (Directory.Exists(pathToCheck) == false)
        {
            Directory.CreateDirectory(pathToCheck);
        }
    }

    public bool ExistRootTerrain()
    {
        List<GameObject> rootObjects = new List<GameObject>();
        Scene scene = SceneManager.GetActiveScene();
        scene.GetRootGameObjects(rootObjects);

        // iterate root objects and do something
        for (int i = 0; i < rootObjects.Count; ++i)
        {
            if (rootObjects[i].name == "Terreno")
            {
                return true;

            }

        }
        return false;
    }
    public Transform ExistRootTerrain(string name)
    {
        List<GameObject> rootObjects = new List<GameObject>();
        Scene scene = SceneManager.GetActiveScene();
        scene.GetRootGameObjects(rootObjects);

        // iterate root objects and do something
        for (int i = 0; i < rootObjects.Count; ++i)
        {
            if (rootObjects[i].name == name)
            {
                return rootObjects[i].transform;

            }

        }
        return null;
    }

    public void LoadTerrain(string aFileName, TerrainData aTerrain)
    {

        int h = aTerrain.heightmapResolution;
        int w = aTerrain.heightmapResolution;
        float[,] data = new float[h, w];

        using (var file = System.IO.File.OpenRead(aFileName))
        {
            using (var reader = new System.IO.BinaryReader(file))
            {
                for (int y = 0; y < h; y++)
                {
                    for (int x = 0; x < w; x++)
                    {
                        float v = (float)reader.ReadUInt16() / 0xFFFF;
                        int destY = false ? h - 1 - y : y;
                        int destX = false ? w - 1 - x : x;
                        data[destY, destX] = v;
                 
                    }
                }
            }

        }

        aTerrain.SetHeights(0, 0, data);
    }

    public void setHeight(Transform listTerrain, int height, int width, Vector2 mont)
    {
        montante = mont;
        foreach (Transform t in listTerrain)
        {
            t.GetComponent<Terrain>().terrainData.size = new Vector3(width, height, width);

        }

        setPosition(listTerrain, width);

    }

    public void setMaterial(Transform listTerrain , Material mat)
    {
       
        foreach (Transform t in listTerrain)
        {
            t.GetComponent<Terrain>().materialTemplate = mat;

        }
    }

    public void rotacionaTerrain(TerrainData aTerrain, float angulo)
    {

        int nx, ny;
        float cs, sn;

        // heightmap rotation
        int tw = aTerrain.heightmapResolution;
        int th = aTerrain.heightmapResolution;
        float[,] origHeightMap = aTerrain.GetHeights(0, 0, aTerrain.heightmapResolution, aTerrain.heightmapResolution);
        float[,] newHeightMap = new float[tw, th];
        float angleRad = angulo * Mathf.Deg2Rad;
        float heightMiddle = (aTerrain.heightmapResolution) / 2.0f; // pivot at middle

        for (int y = 0; y < th; y++)
        {
            for (int x = 0; x < tw; x++)
            {
                cs = Mathf.Cos(angleRad);
                sn = Mathf.Sin(angleRad);

                nx = (int)((x - heightMiddle) * cs - (y - heightMiddle) * sn + heightMiddle);
                ny = (int)((x - heightMiddle) * sn + (y - heightMiddle) * cs + heightMiddle);

                if (nx < 0) nx = 0;
                if (nx > tw - 1) nx = tw - 1;
                if (ny < 0) ny = 0;
                if (ny > th - 1) ny = th - 1;

                newHeightMap[x, y] = origHeightMap[nx, ny];
            } // for x
        } // for y
        aTerrain.SetHeights(0, 0, newHeightMap);
    }

    public void saveRaw(Transform parent,string path)
    {
        int i = 0; 
        foreach (Transform t in parent)
        {
            // Write data
            int heightmapRes = t.GetComponent<Terrain>().terrainData.heightmapResolution;
            float[,] heights = t.GetComponent<Terrain>().terrainData.GetHeights(0, 0, heightmapRes, heightmapRes);
            byte[] data = new byte[heightmapRes * heightmapRes * (int)m_Depth];

            if (m_Depth == Depth.Bit16)
            {
                float normalize = (1 << 16);
                for (int x = 0; x < heightmapRes; ++x)
                   
                {
                    for (int y = 0; y < heightmapRes; ++y)
                    {
                        int index = x + y * heightmapRes;
                        int srcY = m_FlipVertically ? heightmapRes - 1 - y : y;
                        int height = Mathf.RoundToInt(heights[srcY, x] * normalize);
                        ushort compressedHeight = (ushort)Mathf.Clamp(height, 0, ushort.MaxValue);

                        byte[] byteData = System.BitConverter.GetBytes(compressedHeight);
                        // Yay, seems like this is the easiest way to swap bytes in C#. NUTS
                        if ((m_ByteOrder == ByteOrder.Mac) == System.BitConverter.IsLittleEndian)
                        {
                            data[index * 2 + 0] = byteData[1];
                            data[index * 2 + 1] = byteData[0];
                        }
                        else
                        {
                            data[index * 2 + 0] = byteData[0];
                            data[index * 2 + 1] = byteData[1];
                        }
                    }
                }
            }

            FileStream fs = new FileStream(path.Replace("terrain",i.ToString()), FileMode.Create);
            fs.Write(data, 0, data.Length);
            fs.Close();
            i++;
        }
    }
    public void setPosition(Transform parent, int width)
    {
        int count = 0;
        for (int i = 0; i < montante.x; i++)
        {
            for (int j = 0; j < montante.y; j++)
            {
                parent.GetChild(count).transform.position = new Vector3(width * j, 0, -width * i);

                count++;
            }
        }
    }

    public void LoadTerrain8(string path, TerrainData aTerrain)
    {

        float[,] height = new float[aTerrain.heightmapResolution, aTerrain.heightmapResolution];

        FileInfo hmFile = new FileInfo(path);
        FileStream hmFs = hmFile.OpenRead();
        const int BYTESIZE = 513 * 513 * 2;
        byte[] hmB = new byte[BYTESIZE];
        int result = hmFs.Read(hmB, 0, BYTESIZE);
        hmFs.Close();

        int i = 0;
        for (int x = 0; x < aTerrain.heightmapResolution; x++)
        {
            for (int y = 0; y < aTerrain.heightmapResolution; y++)
            {

                height[x, y] = (hmB[i++] * 256.0f) / 65535.0f;
            }
        }
        aTerrain.SetHeights(0, 0, height);

    }

    public void addPrefabTree(FileInfo inf, TerrainData data)
    {
        string text = Application.dataPath;

        text = text.Replace("/", "\\");

        GameObject objet = AssetDatabase.LoadAssetAtPath(inf.DirectoryName.Replace(text, "Assets") + "\\" + inf.Name, typeof(GameObject)) as GameObject;

        TreePrototype[] listTreeProt = data.treePrototypes;

        TreePrototype[] listTreeProt2 = new TreePrototype[listTreeProt.Length + 1];

        for (int index = 0; index < listTreeProt.Length; ++index)
        {
            listTreeProt2[index] = listTreeProt[index];
        }
        listTreeProt2[listTreeProt.Length] = new TreePrototype();
        listTreeProt2[listTreeProt.Length].prefab = objet;
        data.treePrototypes = listTreeProt2;

    }
    public void addPrefabDetails(FileInfo inf, TerrainData data)
    {
        string text = Application.dataPath;

        text = text.Replace("/", "\\");

        GameObject objet = AssetDatabase.LoadAssetAtPath(inf.DirectoryName.Replace(text, "Assets") + "\\" + inf.Name, typeof(GameObject)) as GameObject;

        DetailPrototype[] listTreeProt = data.detailPrototypes;
      
        DetailPrototype[] listTreeProt2 = new DetailPrototype[listTreeProt.Length + 1];

        for (int index = 0; index < listTreeProt.Length; ++index)
        {
            listTreeProt2[index] = listTreeProt[index];
        }
        listTreeProt2[listTreeProt.Length] = new DetailPrototype();
        listTreeProt2[listTreeProt.Length].usePrototypeMesh = true;
        listTreeProt2[listTreeProt.Length].prototype = objet;
        data.detailPrototypes = listTreeProt2;
     

    }
    public void removetreeall(TerrainData data)
    {
        data.treePrototypes = null;

    }
    public void setLayerTerrain(FileInfo[] inf, TerrainData data)
    {
        string text = Application.dataPath;

        text = text.Replace("/", "\\");
        TerrainLayer[] listLayer = new TerrainLayer[inf.Length];
        for (int i = 0; i < inf.Length; i++)
        {

            TerrainLayer layer = AssetDatabase.LoadAssetAtPath(inf[i].DirectoryName.Replace(text, "Assets") + "\\" + inf[i].Name, typeof(TerrainLayer)) as TerrainLayer;
            listLayer[i] = layer;
        }

        data.SetTerrainLayersRegisterUndo(listLayer, "layer");


    }

    public void addtree(List<GameObject> prefab, List<Vector3> checkpos, Terrain terrain)
    {

        TreeInstance[] tree = terrain.terrainData.treeInstances;


        for (int i = 0; i < tree.Length; i++)
        {
            for (int j = 0; j < checkpos.Count; j++)
            {

                if (tree[i].position.x == checkpos[j].x && tree[i].position.z == checkpos[j].z)
                {
                    checkpos.Remove(checkpos[j]);

                }
            }
        }

        int posicao = 0;
        for (int i = 0; i < checkpos.Count; i++)
        {
            Vector3 checkT = Vector3.Scale(checkpos[posicao], terrain.terrainData.size) + terrain.GetPosition();
            Vector3 checkt2 = Vector3.Scale(checkpos[i], terrain.terrainData.size) + terrain.GetPosition();
            if (Vector3.Distance(checkT, checkt2) < distance)
            {
                checkpos.Remove(checkpos[i]);
            }
            else
            {
                posicao = i;
            }
        }
        List<int> listIntProt = new List<int>(); 
        // pega o prototipo 
        TreePrototype[] treeprototyppo = terrain.terrainData.treePrototypes;
       
        for (int i = 0; i < treeprototyppo.Length; i++)
        {
            for (int J= 0; J < prefab.Count; J++)
            {
                if (treeprototyppo[i].prefab == prefab[J])
                {

                    listIntProt.Add(i); 
                  
                }
            }
        }

        //instancia tree(prefab) na posicao
        for (int i = 0; i < checkpos.Count; i++)
        {


            TreeInstance tempInstance = new TreeInstance();

            tempInstance.prototypeIndex = listIntProt[Random.Range(0,listIntProt.Count)];

            tempInstance.color = Color.white;

            tempInstance.heightScale = 1;

            tempInstance.widthScale = 1;
            tempInstance.position = checkpos[i];
            tempInstance.rotation = Random.Range(0, 2 * Mathf.PI);
            terrain.AddTreeInstance(tempInstance);


        }

        terrain.Flush();

    }



    public Vector2 GetTerrainPosition(Vector3 worldPos, Terrain terrain)
    {

        TerrainData terrainData = terrain.terrainData;
        Vector3 terrainPos = terrain.transform.position;
        int mapX = (int)(((worldPos.x - terrainPos.x) / terrainData.size.x) * terrainData.alphamapWidth);
        int mapZ = (int)(((worldPos.z - terrainPos.z) / terrainData.size.z) * terrainData.alphamapHeight);

        return new Vector2(mapX, mapZ);
    }

    private float[,,] GetAlphaMapsForPosition(Vector3 worldPos, int size, Terrain terrain)
    {
        // get the splat data for this cell as a 1x1xN 3d array (where N = number of textures)
        TerrainData terrainData = terrain.terrainData;
        Vector2 converted = GetTerrainPosition(worldPos, terrain);
        return terrainData.GetAlphamaps((int)converted.x - size / 2, (int)converted.y - size / 2, size, size);
    }


    public float[] GetTextureMix(Vector3 worldPos, Terrain terrain)
    {

        // returns an array containing the relative mix of textures
        // on the main terrain at this world position.

        // The number of values in the array will equal the number
        // of textures added to the terrain.

        float[,,] splatmapData = GetAlphaMapsForPosition(worldPos, 1, terrain);

        // extract the 3D array data to a 1D array:
        float[] cellMix = new float[splatmapData.GetUpperBound(2) + 1];
        for (int n = 0; n < cellMix.Length; ++n)
        {
            cellMix[n] = splatmapData[0, 0, n];
        }

        return cellMix;
    }

    public int GetMainTexture(Vector3 worldPos, Terrain terrain)
    {


        float[] mix = GetTextureMix(worldPos, terrain);


        float maxMix = 0;
        int maxIndex = 0;


        for (int n = 0; n < mix.Length; ++n)
        {
            if (mix[n] > maxMix)
            {
                maxIndex = n;
                maxMix = mix[n];
            }
        }

        return maxIndex;

    }
    public int getlayertexture(TerrainLayer terrainLayer, TerrainData data)
    {
        TerrainLayer[] layer = data.terrainLayers;
        int n = -1;
        for (int i = 0; i < layer.Length; i++)
        {
            if (terrainLayer == layer[i])
            {
                n = i;
            }
        }
        return n;
    }
}
