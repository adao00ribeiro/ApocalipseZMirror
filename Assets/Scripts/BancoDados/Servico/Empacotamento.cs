using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
public class Empacotamento 
{
    public static byte[] Empacotar<T>(T template)
    {
        
        string json = JsonUtility.ToJson(template);
        return Encoding.UTF8.GetBytes(json);
    }
    public static string EmpacotarParaString<T>(T template)
    {
        string json = JsonUtility.ToJson(template);
        return json;
    }
    public static T Desempacotar<T>(string json)
    {
        T novo = JsonUtility.FromJson<T>(json);

        return novo;
    }
}
