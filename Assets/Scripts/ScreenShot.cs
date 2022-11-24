using UnityEngine;
using System.Collections;
using System.IO;
using System;
namespace ApocalipseZ
{


    public class ScreenShot : MonoBehaviour
    {
        public Camera cam;
        private InputManager inputManager;
        private string _caminho;
        void Start()
        {
            cam = GetComponent<Camera>();
            inputManager = GameController.Instance.InputManager;
            _caminho = Application.dataPath + "/capturas/";

            if (!Directory.Exists(_caminho))
            {
                Directory.CreateDirectory(_caminho);
            }
        }


        void Update()
        {
            if (inputManager.GetPrint())
            {
                string nomeImagem = _caminho + DateTime.Now.Ticks.ToString() + ".png";
                //O recurso Application.Cap...está obsoleta na versão 2017 da Unity.
                //ScreenCapture.CaptureScreenshot(nomeImagem, 6);//Unity < 2017
                //ScreenCapture.CaptureScreenshot(nomeImagem, 2); //Unity >= 2017
                SaveCameraView();
            }
        }

        void SaveCameraView()
        {
            RenderTexture screenTexture = new RenderTexture(Screen.width, Screen.height, 16);
            cam.targetTexture = screenTexture;
            RenderTexture.active = screenTexture;
            cam.Render();
            Texture2D renderedTexture = new Texture2D(Screen.width, Screen.height);
            renderedTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            RenderTexture.active = null;
            byte[] byteArray = renderedTexture.EncodeToPNG();
            System.IO.File.WriteAllBytes(_caminho + DateTime.Now.Ticks.ToString() + ".png", byteArray);
        }
    }
}