using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

namespace ApocalipseZ
{
    public class UISlotItem : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler,IPointerDownHandler,IPointerUpHandler
    {
        [SerializeField]private Image Image;
        [SerializeField]private Text TextQuantidade;
        [SerializeField]SSlotInventory slot;
        [SerializeField]private UISelectItem selectedItem;

        IInventory inventory;

        private void Awake ( )
        {

            selectedItem = GameObject.Find ("SelectedItem").GetComponent<UISelectItem> ( );
            Image = transform.Find ( "Image").GetComponent<Image> ( );
            TextQuantidade = transform.Find ( "Image/TextQuantidade" ).GetComponent<Text> ( );
        }
        public void UpdateSlot ( int id)
        {
            slot = inventory.GetSlotInventory(id) ;
            Image.sprite = slot.item.Thumbnail;
            TextQuantidade.text = slot.Quantity.ToString();
        }

        internal void SetInventory(IInventory inventory)
        {
            this.inventory = inventory;
        }

        public void OnPointerClick ( PointerEventData eventData )
        {
         
        }

        public void OnPointerEnter ( PointerEventData eventData )
        {
          
           
        }

        public void OnPointerDown ( PointerEventData eventData )
        {
            if (slot.Compare(new SItem("NONE")))
            {
                return;
            }
            selectedItem.enabled = true;
            selectedItem.SetSlot ( slot );
        }

        public void OnPointerUp ( PointerEventData eventData )
        {
            selectedItem.enabled = false;
        }
    }
}