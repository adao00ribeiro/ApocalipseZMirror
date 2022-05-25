using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
namespace ApocalipseZ
{

    public enum SlotType { SLOTINVENTORY, SLOTFASTITEMS, SLOTWEAPONS }
    public class UISlotItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerUpHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]private int Id;
        [SerializeField]private SlotType AcceptedType;
        [SerializeField]private Image Image;
        [SerializeField]private Text TextQuantidade;
        [SerializeField]private SSlotInventory slot;
        [SerializeField]private UISlotItem PrefabUiSlotItem;

        public static UISlotItem SlotSelecionado;
        public static UISlotItem SlotEnter;
        Transform HUD;


        private Vector2 offset;
        IFpsPlayer player;
        private void Awake ( )
        {
            Image = transform.Find ( "Image" ).GetComponent<Image> ( );
            Image.sprite = null;
            Image.color = Color.clear;
            TextQuantidade = transform.Find ( "Image/TextQuantidade" ).GetComponent<Text> ( );
            TextQuantidade.text = "";
        }
        // Start is called before the first frame update
        private void Start ( )
        {
            HUD = GameObject.Find ( "HUD" ).transform;

        }
        public void UpdateSlot ( int id )
        {
            this.Id = id;
            if ( AcceptedType == SlotType.SLOTINVENTORY)
            {
                slot = player.GetInventory ( ).GetSlotInventory ( id );
            }
            else if ( AcceptedType == SlotType.SLOTFASTITEMS )
            {
                slot = player.GetFastItems ( ).GetSlotFastItems ( id );
            }
            else if ( AcceptedType == SlotType.SLOTWEAPONS )
            {
                slot = player.GetWeaponManager ( ).GetSlot ( id );
            }
           
            if ( slot.ItemIsNull ( ) )
            {
                Image.sprite = null;
                Image.color = Color.clear;
                TextQuantidade.text = "";
                return;
            }
            Image.sprite = slot.GetSItem ( ).Thumbnail;
            Image.color = Color.white;
            TextQuantidade.text = slot.GetQuantity ( ).ToString ( );

        }
        public bool RemoveSlot ( )
        {
            UISlotItemTemp slotui = new UISlotItemTemp(Id,slot);

            if ( AcceptedType == SlotType.SLOTINVENTORY )
            {
                print ( "remove inventory" );
                player.CmdRemoveSlotInventory ( slotui );
                return true;
            }
            else if ( AcceptedType == SlotType.SLOTFASTITEMS )
            {
                print ( "remove slotfast" );
                player.CmdRemoveSlotFastItems ( slotui );
                return true;
            }
            else if ( AcceptedType == SlotType.SLOTWEAPONS )
            {
                print ( "remove weapon" );
                // player.CmdRemoveSlotFastItems ( "remove slot weapons" );
                return true;
            }

            return false;

        }
        public bool AddSlot ( SSlotInventory _slot )
        {

            UISlotItemTemp slotui = new UISlotItemTemp(Id,_slot);

            if ( AcceptedType == SlotType.SLOTINVENTORY )
            {
                player.CmdAddSlotInventory ( slotui );
                return true;
            }
            else if ( AcceptedType == SlotType.SLOTFASTITEMS )
            {
                print ( "add slot fastitem");
                player.CmdAddSlotFastItems ( slotui );
                return true;
            }
            else if ( AcceptedType == SlotType.SLOTWEAPONS )
            {
                // player.CmdRemoveSlotFastItems ( "remove slot weapons" );
                return true;
            }
            return true;
        }

        public void OnBeginDrag ( PointerEventData eventData )
        {
            if ( SlotSelecionado )
            {
                //this.transform.SetParent(this.transform.parent.parent);
                //this.transform.position = eventData.position - offset;
                SlotSelecionado.GetComponent<Image> ( ).raycastTarget = false;
            }
        }

        public void OnDrag ( PointerEventData eventData )
        {
            if ( SlotSelecionado )
            {
                SlotSelecionado.transform.position = eventData.position - offset;
            }
        }

        public void OnEndDrag ( PointerEventData eventData )
        {
            //this.transform.SetParent(ui.painel[slotId].transform);
            //this.transform.position = ui.painel[slotId].transform.position;
            //GetComponent<CanvasGroup>().blocksRaycasts = true;
        }

        public void OnPointerUp ( PointerEventData eventData )
        {
            if ( SlotSelecionado == null || SlotEnter == null )
            {
                Destroy ( SlotSelecionado.gameObject );
                return;
            }
            if ( SlotEnter.AcceptedType == SlotSelecionado.AcceptedType )
            {

                if ( SlotEnter.AcceptedType == SlotType.SLOTINVENTORY )
                {
                    player.CmdMoveSlotInventory ( SlotSelecionado.Id , SlotEnter.Id );
                   
                }
                else if ( SlotEnter.AcceptedType == SlotType.SLOTFASTITEMS )
                {
                    print ( "move fast slots");
                    player.CmdMoveSlotFastItems ( SlotSelecionado.Id , SlotEnter.Id );
                }
                else if ( SlotEnter.AcceptedType == SlotType.SLOTWEAPONS )
                {
                    // player.CmdMoveSlotInventory ( SlotSelecionado.Id , SlotEnter.Id );
                }

            }
            else
            {

                if ( SlotEnter.AddSlot ( SlotSelecionado.slot ) )
                {
                  
                    RemoveSlot ( );
                }

            }
            Destroy ( SlotSelecionado.gameObject );
        }
        public void OnPointerDown ( PointerEventData eventData )
        {
            if ( slot.ItemIsNull ( ) )
            {
                return;
            }
            if ( eventData.button == PointerEventData.InputButton.Right )
            {
                // GameObject Slotoptions = PlayerController.CreateUi(controller.Prefab_PainelSlotOptions);
                // Slotoptions.transform.position = eventData.position;
            }
            if ( eventData.button == PointerEventData.InputButton.Left )
            {
                SlotSelecionado = Instantiate ( PrefabUiSlotItem , HUD );
                SlotSelecionado.AcceptedType = AcceptedType;
                SlotSelecionado.SetFpsPlayer ( player);
                SlotSelecionado.UpdateSlot ( Id);
                SlotSelecionado.GetComponent<RectTransform> ( ).sizeDelta = new Vector2 ( 70 , 70 );
                SlotSelecionado.transform.position = eventData.position;
            }
            offset = eventData.position - new Vector2 ( this.transform.position.x , this.transform.position.y );
        }

        public void OnPointerEnter ( PointerEventData eventData )
        {
            SlotEnter = this;
            SetImagemColor ( Color.red );
            //     tooltip.Activate(item);

        }

        public void OnPointerExit ( PointerEventData eventData )
        {
            SlotEnter = null;
            //   tooltip.Deactivate();
        }



        public void SetFpsPlayer ( IFpsPlayer _player )
        {
            player = _player;
        }

        public int GetId ( )
        {
            return Id;
        }

        public void SetImagemColor ( Color color )
        {
            Image.color = color;
        }
    }
}