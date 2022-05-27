using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
namespace ApocalipseZ {
    public class CanvasFpsPlayer : MonoBehaviour
    {
        [SerializeField]UiInventory UiInventory;
        [SerializeField]UiFastItems UiFastItems;
        [SerializeField]UiFpsScopeCursorReticles UiFpsScopeCursorReticles;
        [SerializeField]UiPlayerStats UiPlayerStats;
        [SerializeField]MotionBlur motionBlur;
        [SerializeField]Volume volume;
        private void Awake ( )
        {
            volume = GameObject.Find ( "PostProcessing" ).GetComponent<Volume> ( );
            VolumeProfile proflile = volume.sharedProfile;
            volume.profile.TryGet ( out motionBlur );
        }

        bool IsInventoryOpen = false;
        public void Init ( IFpsPlayer player )
        {
            UiInventory = transform.Find ( "HUD/UiInventory" ).GetComponent<UiInventory> ();
            UiFastItems = transform.Find ( "HUD/UiFastItems" ).GetComponent<UiFastItems> ( );
            UiFpsScopeCursorReticles = transform.Find ( "HUD/UiFpsScopeCursorReticles" ).GetComponent<UiFpsScopeCursorReticles> ( );
            UiPlayerStats = transform.Find ( "HUD/UiPlayerStats" ).GetComponent<UiPlayerStats> ( );

            UiInventory.SetFpsPlayer ( player );
            UiFastItems.SetFpsPlayer ( player );

            UiInventory.gameObject.SetActive ( IsInventoryOpen );
            UiFastItems.gameObject.SetActive ( IsInventoryOpen );
            UiPlayerStats.gameObject.SetActive( IsInventoryOpen );
            ActiveMotionBlur ( IsInventoryOpen );
        }

      


        // Update is called once per frame
        void Update ( )
        {
            if ( InputManager.GetInventory ( ) /*&& !PlayerStats.isPlayerDead*/)
            {
                IsInventoryOpen = !IsInventoryOpen;
                UiInventory.gameObject.SetActive ( IsInventoryOpen );
                UiFastItems.gameObject.SetActive ( IsInventoryOpen );
                ActiveMotionBlur ( IsInventoryOpen );
            }
        }

        public void ActiveMotionBlur (bool active )
        {
            //player.lockCursor = false;
                Cursor.visible = active;
                Cursor.lockState = active ? CursorLockMode.None : CursorLockMode.Locked;
                motionBlur.active = active;
                Time.timeScale = active ? 0 : 1;
           
        }

        private InputManager PInputManager;
        public InputManager InputManager
        {
            get
            {
                if ( PInputManager == null )
                {
                    PInputManager = GameObject.Find ( "InputManager" ).GetComponent<InputManager> ( );
                }
                return PInputManager;
            }
        }
    }
}