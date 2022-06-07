using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class TerrainScript : EditorWindow
{
   
    sc_Terrain Terrain = new sc_Terrain();
    private static EditorWindow window;
   

    public Transform RootTerrain;
    


    public TerrainLayer TerrainLayer;
    public List<GameObject> PrefabTree;

    public float opacity;
    public float distance;
   
    Vector2 scrollPos;
   
    [MenuItem("Terrain/Terrain Auto")]
    public static void CreateWindow()
    {

        // Get existing open window or if none, make a new one:
        window = (TerrainScript)EditorWindow.GetWindow(typeof(TerrainScript));
        window.Show();
        Texture icon = AssetDatabase.LoadAssetAtPath<Texture>("Assets/Terrain Auto/web_2-09-512.png");
        GUIContent titleContent = new GUIContent("Terrain Auto", icon);
        window.titleContent = titleContent;

        // window.minSize = new Vector2(500f, 700f);
    }

    void OnGUI()
    {
        
        GUILayout.Label("TERRAIN", EditorStyles.boldLabel);

        ScriptableObject target = this;
        SerializedObject so = new SerializedObject(target);

        // serialized variaveis
       
        SerializedProperty RootTerrainProperty = so.FindProperty("RootTerrain");
        SerializedProperty caminhoProperty = so.FindProperty("caminho");
        SerializedProperty pastaprefabtreeProperty = so.FindProperty("pastaprefabtree");
        SerializedProperty pastLayerProperty = so.FindProperty("pastLayer");
        SerializedProperty TerrainLayerProperty = so.FindProperty("TerrainLayer");
        SerializedProperty PrefabTreeProperty = so.FindProperty("PrefabTree");
        SerializedProperty Texture2DTreeProperty = so.FindProperty("Texture2D");
        SerializedProperty opacityProperty = so.FindProperty("opacity");
        SerializedProperty distanceProperty = so.FindProperty("distance");
        SerializedProperty MaterialProperty = so.FindProperty("matTerrain");

        EditorGUILayout.PropertyField(RootTerrainProperty, true); // True means show children
        RootTerrain = TerrainWizard.existRootTerrain();
        

        if (RootTerrain == null)
        {
            if (GUILayout.Button("Create Terrains!"))
            {
                CreateTerrain wizzard = TerrainWizard.DisplayTerrainWizard<CreateTerrain>("Create Terrains", "Create");
            
            }
           
        }
        else
        {
            if (GUILayout.Button("Set Height Map!"))
            {
                setMapHeight wizzard = TerrainWizard.DisplayTerrainWizard<setMapHeight>("Set Map", "Set Map");
                wizzard.inicialize(RootTerrain);
            }
           
            if (GUILayout.Button("Rotation Terrain!"))
            {

                RotacionaTerrains wizzard = TerrainWizard.DisplayTerrainWizard<RotacionaTerrains>("Rotation", "OK");
                wizzard.inicialize(RootTerrain);

            }
            if (GUILayout.Button("Export Raw!"))
            {
                ExportRaw wizzard = TerrainWizard.DisplayTerrainWizard<ExportRaw>("Export Raw", "Export");
                wizzard.inicialize(RootTerrain);
            }
            if (GUILayout.Button("Material!"))
            {
                setMaterialTerrain wizzard = TerrainWizard.DisplayTerrainWizard<setMaterialTerrain>("Set Material", "Set Material");
                wizzard.inicialize(RootTerrain);

            }
            /*
            if (GUILayout.Button("Set Config!"))
            {

                setConfigTerrains wizzard = TerrainWizard.DisplayTerrainWizard<setConfigTerrains>("CONFIG TERRAINS", "OK");
                wizzard.inicialize(RootTerrain);
            }
      */

            GUILayout.Label("ADD PROTOTYPE IN TERRAINS", EditorStyles.boldLabel);

           
            if (GUILayout.Button("Prototype!"))
            {
                addPrototiposTerrains wizzard = TerrainWizard.DisplayTerrainWizard<addPrototiposTerrains>("Prototipos", "Set Prot","remove Prot");
                wizzard.inicialize(RootTerrain);

            }

            GUILayout.Label("ADD LAYERS IN TERRAIN", EditorStyles.boldLabel);
           
            if (GUILayout.Button("Layers!"))
            {
                addLayerTerrains wizzard = TerrainWizard.DisplayTerrainWizard<addLayerTerrains>("Layer", "Set Layer", "Remove Layer");
                wizzard.inicialize(RootTerrain);
            }
           

            GUILayout.Label("AUTO PAINT", EditorStyles.boldLabel);

            EditorGUILayout.PropertyField(TerrainLayerProperty, true); // True means show children

         
            scrollPos =  GUILayout.BeginScrollView(scrollPos,GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(false));
            
            EditorGUILayout.PropertyField(PrefabTreeProperty, true); // True means show children

            //EditorGUILayout.PropertyField ( Texture2DTreeProperty , true ); // True means show children

            EditorGUILayout.EndScrollView();
          
            EditorGUILayout.Slider(opacityProperty, 0, 1);
            EditorGUILayout.Slider(distanceProperty, 0, 100);


            if (GUILayout.Button("PAINT!"))
            {

                AutoPaint wizzard = new AutoPaint(RootTerrain, PrefabTree, opacity,distance,TerrainLayer);
                wizzard.Paint();

            }

        }
        so.ApplyModifiedProperties(); // Remember to apply modified properties
    }



}