using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using TMPro;
namespace ApocalipseZ
{


    public class UISlotItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerUpHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private TypeContainer AcceptedType;
        [SerializeField] private int SlotIndex;
        [SerializeField] private Image Image;
        [SerializeField] private TextMeshProUGUI TextQuantidade;
        private Vector2 offset;
        public Transform HUD;
        public static UISlotItem SlotSelecionado;
        public static UISlotItem SlotEnter;

        [SerializeField] Inventory inventory;
        [SerializeField] WeaponManager weaponManager;
        public bool isEmpty;
        public bool IsLocked;
        public void SetInventory(Inventory _inventory)
        {
            inventory = _inventory;
        }
        public void SetWeaponManager(WeaponManager _weaponmanager)
        {
            weaponManager = _weaponmanager;
        }
        // Start is called before the first frame update
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (SlotSelecionado)
            {
                //this.transform.SetParent(this.transform.parent.parent);
                //this.transform.position = eventData.position - offset;
                SlotSelecionado.GetComponent<Image>().raycastTarget = false;
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (SlotSelecionado)
            {
                SlotSelecionado.transform.position = eventData.position - offset;
            }
        }


        public void OnEndDrag(PointerEventData eventData)
        {

        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (SlotSelecionado != null)
            {
                if (SlotEnter != null && SlotSelecionado != SlotEnter && IsLocked == false)
                {
                    if (SlotEnter.AcceptedType == TypeContainer.INVENTORY)
                    {

                        //sslotselecaosdjaosdja
                        if (SlotSelecionado.AcceptedType == TypeContainer.INVENTORY)
                        {
                            inventory.CmdInsertItem(SlotEnter.SlotIndex, SlotSelecionado.SlotIndex);
                        }
                        else if (SlotSelecionado.AcceptedType == TypeContainer.WEAPONS)
                        {
                            // weaponManager.CmdMove();
                        }
                        else
                        {

                        }


                        //sslotselecaosdjaosdja
                    }
                    if (SlotEnter.AcceptedType == TypeContainer.FASTITEMS)
                    {
                        //sslotselecaosdjaosdja



                        //sslotselecaosdjaosdja
                    }
                    if (SlotEnter.AcceptedType == TypeContainer.WEAPONS)
                    {
                        //sslotselecaosdjaosdja
                        if (SlotSelecionado.AcceptedType == TypeContainer.INVENTORY)
                        {
                            weaponManager.CmdAddWeaponRemoveInventory(SlotEnter.SlotIndex, SlotSelecionado.SlotIndex);
                        }
                        else if (SlotSelecionado.AcceptedType == TypeContainer.WEAPONS)
                        {
                            weaponManager.CmdMove();
                        }
                        else
                        {

                        }
                        //sslotselecaosdjaosdja
                    }
                }
                Destroy(SlotSelecionado.gameObject);
            }

        }
        public void OnPointerDown(PointerEventData eventData)
        {
            if (isEmpty)
            {
                return;
            }
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                // GameObject Slotoptions = PlayerController.CreateUi(controller.Prefab_PainelSlotOptions);
                // Slotoptions.transform.position = eventData.position;
            }
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                SlotSelecionado = Instantiate(this, GameController.Instance.CanvasFpsPlayer.HUD);
                SlotSelecionado.GetComponent<Image>().raycastTarget = false;
                SlotSelecionado.SetSlotIndex(SlotIndex);
                SlotSelecionado.SetImage(Image.sprite);
                SlotSelecionado.GetComponent<RectTransform>().sizeDelta = new Vector2(70, 70);
                SlotSelecionado.transform.position = eventData.position;

            }
            offset = eventData.position - new Vector2(this.transform.position.x, this.transform.position.y);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (SlotSelecionado != null)
            {
                SlotEnter = this;
                SlotEnter.IsLocked = true;
                if (SlotEnter.AcceptedType == TypeContainer.FASTITEMS)
                {
                    if (SlotSelecionado.GetTypeItem() == ItemType.weapon)
                    {
                        SlotEnter.Image.color = Color.red;
                        return;
                    }
                }
                if (SlotEnter.AcceptedType == TypeContainer.WEAPONS)
                {
                    if (SlotSelecionado.GetTypeItem() != ItemType.weapon)
                    {
                        SlotEnter.Image.color = Color.red;
                        return;
                    }
                }
                if (!isEmpty && SlotEnter.AcceptedType != SlotSelecionado.AcceptedType)
                {
                    SlotEnter.Image.color = Color.red;
                    return;
                }
                if (SlotSelecionado != null && SlotSelecionado != SlotEnter)
                {
                    SlotEnter.IsLocked = false;
                    SlotEnter.Image.color = Color.green;
                }
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (isEmpty)
            {
                Image.color = Color.clear;
            }
            SlotEnter = null;

            //   tooltip.Deactivate();
        }
        public void SetIsEmpty(bool empty)
        {
            isEmpty = empty;
        }
        public void SetTextQuantidade(string text)
        {
            TextQuantidade.text = text;
        }
        public void SetImage(Sprite image)
        {
            Image.sprite = image;
            if (image == null)
            {
                Image.color = Color.clear;
                return;
            }
            Image.color = Color.white;
        }
        public void SetSlotIndex(int index)
        {
            SlotIndex = index;
        }
        public int GetSlotIndex()
        {
            return SlotIndex;
        }
        public ItemType GetTypeItem()
        {
            if (AcceptedType == TypeContainer.INVENTORY)
            {
                return inventory.GetTypeItem(SlotIndex);
            }

            return weaponManager.GetTypeItem(SlotIndex);
        }



    }
}