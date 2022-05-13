using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

namespace ApocalipseZ
{
    public enum SlotType { SLOTINVENTORY,SLOTFASTITEMS,SLOTWEAPONS}
    public class UISlotItem : MonoBehaviour,  IPointerClickHandler, IPointerEnterHandler,IPointerDownHandler,IPointerUpHandler,IPointerExitHandler
    {
        public int id;
        [SerializeField]private SlotType AcceptedType;
        [SerializeField]private Image Image;
        [SerializeField]private Text TextQuantidade;
        [SerializeField]private SSlotInventory slot;
        [SerializeField]private UISelectItem selectedItem;

        IInventory inventory;
        IFastItems fastItems;
        IWeaponManager weaponmanager;
        public static UISlotItem enterslotitem;
      
        private void Awake ( )
        {
          
            selectedItem = GameObject.Find ("SelectedItem").GetComponent<UISelectItem> ( );
            Image = transform.Find ( "Image").GetComponent<Image> ( );
            Image.sprite = null;
            Image.color = Color.clear;
            TextQuantidade = transform.Find ( "Image/TextQuantidade" ).GetComponent<Text> ( );
            TextQuantidade.text = "";
        }
      
        public void UpdateSlot ( int id)
        {
            this.id = id;
            slot = inventory.GetSlotInventory(id) ;
            if ( slot.item == null)
            {
                TextQuantidade.text = "";
                return;
            }
            Image.sprite = slot.item.Thumbnail;
            Image.color = Color.white;
            TextQuantidade.text = slot.Quantity.ToString();
        }

        internal void SetInventory(IInventory inventory)
        {
            this.inventory = inventory;
        }
        internal void SetFastItems ( IFastItems fastitems )
        {
            this.fastItems = fastitems;
        }
        internal void SetWeaponManager ( IWeaponManager weaponManager )
        {
            this.weaponmanager = weaponManager;
        }
        public void OnPointerClick ( PointerEventData eventData )
        {
         
        }

        public void OnPointerEnter ( PointerEventData eventData )
        {
            if ( selectedItem.enabled )
            {
                enterslotitem = this;
               
            }

        }

        public void OnPointerDown ( PointerEventData eventData )
        {
            if ( slot.item == null)
            {
                return;
            }

            if ( eventData.button == PointerEventData.InputButton.Left)
            {
                selectedItem.id = id;
                selectedItem.enabled = true;
                selectedItem.SetSlot ( slot );
            }
            else if ( eventData.button == PointerEventData.InputButton.Right )
            {
                inventory.UseItem ( slot,false);
            }
          
        }

        public void OnPointerUp ( PointerEventData eventData )
        {

            if ( selectedItem.enabled)
            {
                if ( enterslotitem != null )
                {
                    switch ( enterslotitem.AcceptedType )
                    {
                        case SlotType.SLOTINVENTORY:

                            inventory.MoveItem ( selectedItem.id , enterslotitem.GetId ( ) );
                            break;
                        case SlotType.SLOTFASTITEMS:
                            fastItems.SetFastSlots ( enterslotitem.GetId ( ) , selectedItem .GetSlot());
                            inventory.RemoveItem ( selectedItem.GetSlot ( ) ,false);
                            break;
                        case SlotType.SLOTWEAPONS:
                            print ( "setar weapons" );
                            
                            break;
                        default:
                            break;
                    }
                    
                }
               
                selectedItem.enabled = false;
            }
        }

        public void OnPointerExit ( PointerEventData eventData )
        {
            enterslotitem = null;
          
        }

        public int GetId ( )
        {
            return id;
        }

        public void SetImagemColor ( Color color )
        {
            Image.color = color;
        }

    }
}