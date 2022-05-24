using UnityEngine;
using UnityEngine.Events;
using UnityStandardAssets.ImageEffects;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
namespace ApocalipseZ
{
    public class InventoryManager : MonoBehaviour,IInventoryManager
    {
       Canvas canvas;
       IFpsPlayer player;
       UIInventory uiInventory;
       [SerializeField] MotionBlur motionBlur;
       [SerializeField]Volume volume;

        public static bool showInventory = false;
        public bool isOpen = true;

        private void Start()
        {
            uiInventory = transform.Find ("HUD/InventoryPanel" ).GetComponent<UIInventory> ( );
            canvas = GetComponent<Canvas>();
            VolumeProfile proflile = volume.sharedProfile;
            volume.profile.TryGet(out motionBlur);
           
            
        }

        private void Update()
        {
            if (player == null)
            {
                return;
            }
            if (InputManager.instance.GetInventory() /*&& !PlayerStats.isPlayerDead*/)
            {
                showInventory = !showInventory;
            }

            if (showInventory)
            {
               
                InventoryOpen ();
            }
            else
            {
                InventoryClose();
            }
        }
        public void SetFpsPlayer(IFpsPlayer _player)
        {
            player = _player;
            uiInventory.SetFpsPlayer ( _player );
            uiInventory.SetInventory ( player .GetInventory());
            uiInventory.SetFastItems ( player.GetFastItems());
            uiInventory.SetWeaponManager ( player.GetWeaponManager());
            InventoryClose ( );
        }
        public void InventoryOpen()
        {
            if (isOpen)
                return;
            else
            {
                canvas.enabled = true;
                player.lockCursor = false;
                 motionBlur.active = true;
                Time.timeScale = 0;
                isOpen = true;

            }
        }
        public void InventoryClose()
        {
            if (!isOpen)
                return;
            else
            {
                canvas.enabled = false;
                player.lockCursor = true;
                motionBlur.active = false;
                Time.timeScale = 1;
                isOpen = false;
            }
        }

    }
}