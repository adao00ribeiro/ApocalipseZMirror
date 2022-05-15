using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace ApocalipseZ
{
    public class UISelectItem : MonoBehaviour
    {
        public int id;
        public SlotType AcceptedType;
        [SerializeField]SSlotInventory slot;
       
        private Image image;

        Vector3 positionInitial;
        // Start is called before the first frame update
        void Awake ( )
        {
          
            positionInitial = transform.position;
            image = GetComponent<Image> ( );
            enabled = false;
            SetSlot ( new SSlotInventory ( ) );
        }

        // Update is called once per frame
        void Update ( )
        {
            transform.position = InputManager.instance.GetMousePosition ( );
          
        }
    
        private void OnDisable ( )
        {
            SetSlot ( new SSlotInventory ( ) );
            transform.position = positionInitial;
           
        }
        public void SetSlot ( SSlotInventory _slot )
        {
            if ( _slot.item == null)
            {
                slot = _slot;
                image.sprite = null;
                image.color = Color.clear;
                return;
            }

            slot.item = _slot.item;
            slot.Quantity = _slot.Quantity;

            image.sprite = slot.item.Thumbnail;
            image.preserveAspect = true;
            image.color = Color.white;
        }

        public SSlotInventory GetSlot ( )
        {
            return slot;
        }
      
    
    }
}