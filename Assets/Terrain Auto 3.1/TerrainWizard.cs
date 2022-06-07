using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace UnityEditor
{
    internal class TerrainWizard : ScriptableWizard
    {
        protected static sc_Terrain Terrain = new sc_Terrain();
        private static Transform p_RootTerrain;
        public static Transform RootTerrain
        {
            get
            {
                if (p_RootTerrain != null)
                    return p_RootTerrain;
                else
                    return null;
            }
            set
            {
                p_RootTerrain = value;
            }
        }


        internal virtual void OnWizardUpdate()
        {
          
        }
        public static Transform existRootTerrain()
        {

            return Terrain.ExistRootTerrain("Terreno");
        }
        internal void InitializeDefaults()
        {

        }

        internal void FlushHeightmapModification()
        {


        }

        internal static T DisplayTerrainWizard<T>(string title, string button) where T : TerrainWizard
        {
            var treeWizards = Resources.FindObjectsOfTypeAll<T>();
            if (treeWizards.Length > 0)
            {
                var wizard = (T)treeWizards[0];
                wizard.titleContent = EditorGUIUtility.TrTextContent(title);
                wizard.createButtonName = button;
                wizard.otherButtonName = "";
                wizard.Focus();
                return wizard;
            }
            return ScriptableWizard.DisplayWizard<T>(title, button);
        }
        internal static T DisplayTerrainWizard<T>(string title, string button , string button2) where T : TerrainWizard
        {
            var treeWizards = Resources.FindObjectsOfTypeAll<T>();
            if (treeWizards.Length > 0)
            {
                var wizard = (T)treeWizards[0];
                wizard.titleContent = EditorGUIUtility.TrTextContent(title);
                wizard.createButtonName = button;
                wizard.otherButtonName = button2;
                wizard.Focus();
                return wizard;
            }
            return ScriptableWizard.DisplayWizard<T>(title, button,button2);
        }
       
    }
    //criacao
    internal class CreateTerrain : TerrainWizard
    {
        public Vector2 tileAmount;
        public int Resolucao;
        public int Heigth;

        public void OnEnable()
        {
            maxSize = minSize = new Vector2(250, 250);
        }
        internal void OnWizardCreate()
        {
            if (!RootTerrain)
            {
                RootTerrain = new GameObject("Terreno").transform;
                Terrain.CreateTerrain(RootTerrain, tileAmount, Resolucao, Heigth, Resolucao);
            }

        }

        internal override void OnWizardUpdate()
        {

            isValid = true;
            errorString = "";
            if (tileAmount.x == 0 || tileAmount.y == 0)
            {
                errorString = "Tile Amount esta zerado";
                isValid = false;
            }
            if (Resolucao == 0)
            {
                errorString = "Resolucao esta zerado";
                isValid = false;
            }
            if (Heigth == 0)
            {
                errorString = "Heigth esta zerado";
                isValid = false;
            }


        }

    }
    //set map
    internal class setMapHeight:TerrainWizard
    {
        public UnityEngine.Object PastRaw;

        public void OnEnable()
        {
            maxSize = minSize = new Vector2(300, 150);
        }
        private void OnWizardCreate()
        {
            if (PastRaw == null)
            {
                Debug.Log("Pasta Nao Encontrada");
                return;
            }
            DirectoryInfo dir = new DirectoryInfo(AssetDatabase.GetAssetOrScenePath(PastRaw));
            FileInfo[] info = dir.GetFiles("*.raw");
            int count = 0;

            Debug.Log(info.Length);
            foreach (Transform terra in RootTerrain)
            {
                foreach (FileInfo inf in info)
                {
                    Debug.Log(inf.Name + "terra" + terra.name);
                    if (inf.Name == count + ".raw")
                    {
                        //Terrain.LoadTerrain8(inf.DirectoryName + "\\" + inf.Name, terra.GetComponent<Terrain>().terrainData);
                        Terrain.LoadTerrain(inf.DirectoryName + "\\" + inf.Name, terra.GetComponent<Terrain>().terrainData);
                        break;
                    }
                }
                count++;
            }

        }
        internal override void OnWizardUpdate()
        {

            isValid = true;
            
            errorString = "";
            if (PastRaw==null)
            {
                errorString = "pasta nao encontrada";
                isValid = false;
            }


        }
        public void inicialize(Transform root)
        {
            RootTerrain = root;
           
        }
    }
    //setmaterial
    internal class setMaterialTerrain : TerrainWizard
    {
        public Material MaterialTerrain;

        public void OnEnable()
        {
            maxSize = minSize = new Vector2(300, 150);
        }
        private void OnWizardCreate()
        {

            Terrain.setMaterial(RootTerrain, MaterialTerrain);

        }
        internal override void OnWizardUpdate()
        {

            isValid = true;

            errorString = "";
            if (MaterialTerrain == null)
            {
                errorString = "MaterialTerrain nao encontrada";
                isValid = false;
            }


        }
        public void inicialize(Transform root)
        {
            RootTerrain = root;

        }
    }
    //export raw
    internal class ExportRaw : TerrainWizard
    {
        

        public void OnEnable()
        {
            maxSize = minSize = new Vector2(300, 150);
        }
        private void OnWizardCreate()
        {

            // string saveLocation = EditorUtility.SaveFilePanel("Save Raw Heightmap", "", "terrain", "raw");
            // Debug.Log(saveLocation);    
            //   string saveLocation = "C:\\Users\\ADAO RIBEIRO\\Desktop\\RAW\\terrain.raw";
            string saveLocation = Application.dataPath + "/" + "raw" + "/terrain.raw";
            Debug.Log(saveLocation);
            Terrain.saveRaw(RootTerrain, saveLocation);

        }
        internal override void OnWizardUpdate()
        {

            isValid = true;

            errorString = "";
            if (RootTerrain == null)
            {
                errorString = "MaterialTerrain nao encontrada";
                isValid = false;
            }


        }
        public void inicialize(Transform root)
        {
            RootTerrain = root;

        }
    }
    internal class RotacionaTerrains : TerrainWizard
    {
        public float angulo;

        public void OnEnable()
        {
            maxSize = minSize = new Vector2(300, 150);
        }
        private void OnWizardCreate()
        {

           
            Debug.Log(angulo);
            foreach (Transform terra in RootTerrain)
            {
                Terrain.rotacionaTerrain(terra.GetComponent<Terrain>().terrainData, angulo);
            }

        }
        internal override void OnWizardUpdate()
        {

           


        }
        public void inicialize(Transform root)
        {
            RootTerrain = root;

        }
    }
    internal class setConfigTerrains : TerrainWizard
    {
        public float DetailDistance;
        public float DetailDensity;
        public float treeDistance;

        public void OnEnable()
        {
            maxSize = minSize = new Vector2(300, 200);
        }
        private void OnWizardCreate()
        {

            // Terrain.setHeight(RootTerrain, Height, resolucao, tileAmount);

        }
        internal override void OnWizardUpdate()
        {

            isValid = true;

            errorString = "";
            if (DetailDistance > 250  || DetailDistance < 0)
            {
                errorString = "detailDistance must be between 0 and 250 ";
                isValid = false;
            }


        }
        public void inicialize(Transform root)
        {
            RootTerrain = root;

        }
    }
    internal class addPrototiposTerrains : TerrainWizard
    {
        public UnityEngine.Object pastPrefab;
        public bool isGrass;

        public void OnEnable()
        {
            maxSize = minSize = new Vector2(300, 150);
        }
        private void OnWizardCreate()
        {



            DirectoryInfo dir = new DirectoryInfo(AssetDatabase.GetAssetOrScenePath(pastPrefab));
            FileInfo[] info = dir.GetFiles("*.prefab");

            foreach (Transform terra in (Transform)RootTerrain)
            {

                foreach (FileInfo inf in info)
                {
                    if (!isGrass)
                    {
                        Terrain.addPrefabTree(inf, terra.GetComponent<Terrain>().terrainData);

                    }
                    else
                    {
                        Debug.Log(inf.Name);
                        Terrain.addPrefabDetails(inf, terra.GetComponent<Terrain>().terrainData);
                    }

                }

            }

        }
        internal override void OnWizardUpdate()
        {

            isValid = true;

            errorString = "";
            if (pastPrefab == null)
            {
                errorString = "pastPrefab nao encontrada";
                isValid = false;
            }


        }
        private void OnWizardOtherButton()
        {
            foreach (Transform terra in (Transform)RootTerrain)
            {
                Terrain.removetreeall(terra.GetComponent<Terrain>().terrainData);
            }
        }
        public void inicialize(Transform root)
        {
            RootTerrain = root;

        }
    }
    internal class addLayerTerrains : TerrainWizard
    {
        public Object PastLayer;

        public void OnEnable()
        {
            maxSize = minSize = new Vector2(300, 150);
        }
        private void OnWizardCreate()
        {

           

            DirectoryInfo dir = new DirectoryInfo(AssetDatabase.GetAssetOrScenePath(PastLayer));
            FileInfo[] info = dir.GetFiles("*.terrainlayer");

            foreach (Transform terra in (Transform)RootTerrain)
            {


                Terrain.setLayerTerrain(info, terra.GetComponent<Terrain>().terrainData);


            }

        }
        internal override void OnWizardUpdate()
        {

            isValid = true;

            errorString = "";
            if (PastLayer == null)
            {
                errorString = "PastLayer nao encontrada";
                isValid = false;
            }


        }
        public void inicialize(Transform root)
        {
            RootTerrain = root;

        }
    }
    internal class AutoPaint : TerrainWizard
    {
        public float opacity;
        public float distance;
        public TerrainLayer TerrainLayer;
        public List<GameObject> PrefabTree;
        public List<Texture2D> ListTexture2dGrass;
        public AutoPaint(Transform root , List<GameObject> PrefabTree, float opacity, float distance, TerrainLayer terrainLayer)
        {
            RootTerrain = root;
            this.opacity = opacity;
            this.distance = distance;
            this.TerrainLayer = terrainLayer;
            this.PrefabTree = PrefabTree;
        }
        
        public  void Paint()
        {

            Terrain.distance = distance;
            List<Vector3> ListPossiveTree = new List<Vector3>();
            GameObject objt = Selection.activeGameObject;

            if (objt == null)
            {
                Debug.Log("Terreno Nao Selecionado");
                return;
            }
            Terrain terrain = objt.GetComponent<Terrain>();
            if (terrain == null)
            {
                Debug.Log("Objeto selecionado não é um terreno");
                return;
            }
            int terrainTextureIndexToPlaceTreeOn = Terrain.getlayertexture((TerrainLayer)TerrainLayer, terrain.terrainData);
            for (float x = 0; x < terrain.terrainData.size.x; x += 0.3f)
            {
                for (float z = 0; z < terrain.terrainData.size.z; z += 0.3f)
                {
                    Vector3 checkPos = new Vector3(x + terrain.GetPosition().x, 0, z + terrain.GetPosition().z);
                    int textureIndexAtCheckPos = Terrain.GetMainTexture(checkPos, terrain);

                    if (textureIndexAtCheckPos == terrainTextureIndexToPlaceTreeOn && Random.Range(0f, 100f) < opacity)
                    {

                        float x1 = x / terrain.terrainData.size.x;
                        float z1 = z / terrain.terrainData.size.z;

                        ListPossiveTree.Add(new Vector3(x1, 0, z1));


                    }


                }

            }
            Debug.Log("Total de possiveis " + ListPossiveTree.Count);
            //verifica distancia entre as posicoes
            Terrain.addtree(PrefabTree, ListPossiveTree, terrain);


        }
      
    }
}