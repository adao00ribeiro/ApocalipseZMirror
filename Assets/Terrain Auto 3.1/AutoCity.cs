using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class AutoCity : EditorWindow
{
    private static EditorWindow window;
    public GameObject[] buildings;
    private int mapWidth = 20;
    private int mapHeight = 20 ;
    public float  buildFootPrint = 3;
   
    Color32[] Data;
    public Texture2D image;
    [MenuItem("City/Auto City")]
    public static void CreateWindow()
    {
     
           // Get existing open window or if none, make a new one:
           window = (AutoCity)EditorWindow.GetWindow(typeof(AutoCity));
        window.Show();
        // Texture icon = AssetDatabase.LoadAssetAtPath<Texture>("Assets/Terrain Auto/web_2-09-512.png");
        // GUIContent titleContent = new GUIContent("Terrain Auto", icon);
        //  window.titleContent = titleContent;
    }
    void OnGUI()
    {
       
        GUILayout.Label("CRIAÇÃO", EditorStyles.boldLabel);
        ScriptableObject target = this;
        SerializedObject so = new SerializedObject(target);
        SerializedProperty stringsProperty = so.FindProperty("buildings");
        SerializedProperty ImageProperty = so.FindProperty("image");
        SerializedProperty buildFootPrintProperty = so.FindProperty("buildFootPrint");
        EditorGUILayout.PropertyField(stringsProperty, true); // True means show children
        EditorGUILayout.PropertyField(ImageProperty, true); // True means show children
        EditorGUILayout.PropertyField(buildFootPrintProperty, true); // True means show children
        so.ApplyModifiedProperties(); // Remember to apply modified properties

       // buildFootPrint = EditorGUILayout.IntField("buildFootPrint", buildFootPrint);
      //  image = (Sprite)EditorGUILayout.ObjectField("image", image, typeof(Sprite), true);
        
       
        if (GUILayout.Button("Auto City!"))
        {
            Transform parent = new GameObject("City").transform;
            mapWidth = image.width;
            mapHeight = image.height;
           
            Color32 write = new Color32(255 , 255, 255,255);
            Color32 red = new Color32(255, 0, 0, 255);
            float seed = Random.Range(0,100);
          
            for (int i = 0; i < mapWidth; i++)
            {
                for (int j = 0; j < mapHeight;j++)
                {
                      Color32 color = image.GetPixel(i,j);
                 
                    Vector3 pos = new Vector3(i * buildFootPrint, 0, j * buildFootPrint);
                    if (color.Equals(write))
                    {
                      
                        Instantiate((GameObject)buildings[Random.Range(0,buildings.Length - 1 )], pos, Quaternion.identity,parent);
                    }else if (color.Equals(red))
                    {
                       
                        Instantiate((GameObject)buildings[buildings.Length-1], pos, Quaternion.identity, parent);
                    }
                   
                    /*
                    int result = (int)(Mathf.PerlinNoise(i / 10.0f + seed,j/10.0f + seed) * 10);
                  
                 
                   
                    if (result < 2 )
                    {
                        
                        Instantiate((Transform)buildings , pos , Quaternion.identity);
                    }else if (result < 4)
                    {

                    }
                    else if (result < 5)
                    {

                    }
                    else if (result < 6)
                    {

                    }
                    else if (result < 7)
                    {

                    }
                    else if (result < 10)
                    {

                    }
                   */
                }
            }
          

        }
    }
}
