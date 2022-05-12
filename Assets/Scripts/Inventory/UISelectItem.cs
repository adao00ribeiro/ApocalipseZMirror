using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace ApocalipseZ
{
    public class UISelectItem : MonoBehaviour
    {
        [SerializeField]SSlotInventory slot;
       
        private Image image;

        Vector3 positionInitial;
        // Start is called before the first frame update
        void Awake ( )
        {
            positionInitial = transform.position;
            image = GetComponent<Image> ( );
           enabled = false;
        }

        // Update is called once per frame
        void Update ( )
        {
            transform.position = InputManager.instance.GetMousePosition ( );
                        
        }
    
        private void OnDisable ( )
        {
            transform.position = positionInitial;
            SetSlot ( new SSlotInventory("NONE"));
        }
        public void SetSlot ( SSlotInventory _slot )
        {
            slot = _slot;
            if ( _slot .Compare(new SItem("NONE")))
            {
                image.sprite = null;
                image.color = Color.clear;
                return;
            }
            image.sprite = slot.item.Thumbnail;
            image.color = Color.white;
        }
      
    
    }
}