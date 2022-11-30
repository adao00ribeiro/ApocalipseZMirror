using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ApocalipseZ
{
    public class UiPrimaryAndSecondWeapons : MonoBehaviour
    {
        [SerializeField] private UISlotItem UiPrimaryWeapon;
        [SerializeField] private UISlotItem UiSecondWeapon;


        private void Awake()
        {
            UiPrimaryWeapon = transform.Find("Container/Primary Weapon Slot").GetComponent<UISlotItem>();
            UiSecondWeapon = transform.Find("Container/Second Weapon Slot").GetComponent<UISlotItem>();
            UiPrimaryWeapon.HUD = transform.parent;
            UiSecondWeapon.HUD = transform.parent;
            UpdatePrimaryWeapon(new SlotInventoryTemp());
            UpdateSecundaryWeapon(new SlotInventoryTemp());
        }
        public void SetInventory(Inventory _inventory)
        {

            UiPrimaryWeapon.SetInventory(_inventory);
            UiSecondWeapon.SetInventory(_inventory);
        }
        public void SetWeaponManager(WeaponManager _weaponmanager)
        {
            UiPrimaryWeapon.SetWeaponManager(_weaponmanager);
            UiSecondWeapon.SetWeaponManager(_weaponmanager);
        }
        public void UpdatePrimaryWeapon(SlotInventoryTemp newItem)
        {
            if (newItem.Compare(new SlotInventoryTemp()))
            {
                UiPrimaryWeapon.SetIsEmpty(true);
                UiPrimaryWeapon.SetImage(null);
                UiPrimaryWeapon.SetTextQuantidade("");
                return;
            }
            DataItem dataItem = GameController.Instance.DataManager.GetDataItemById(newItem.guidid);

            if (dataItem == null)
            {
                UiPrimaryWeapon.SetIsEmpty(true);
                UiPrimaryWeapon.SetImage(null);
                UiPrimaryWeapon.SetTextQuantidade("");
            }
            else
            {

                UiPrimaryWeapon.SetIsEmpty(false);
                UiPrimaryWeapon.SetImage(dataItem.Thumbnail);
                UiPrimaryWeapon.SetTextQuantidade("");
            }
        }
        public void UpdateSecundaryWeapon(SlotInventoryTemp newItem)
        {
            if (newItem.Compare(new SlotInventoryTemp()))
            {
                UiSecondWeapon.SetIsEmpty(true);
                UiSecondWeapon.SetImage(null);
                UiSecondWeapon.SetTextQuantidade("");
                return;
            }
            DataItem dataItem = GameController.Instance.DataManager.GetDataItemById(newItem.guidid);
            if (dataItem == null)
            {
                UiSecondWeapon.SetIsEmpty(true);
                UiSecondWeapon.SetImage(null);
                UiSecondWeapon.SetTextQuantidade("");
            }
            else
            {
                UiSecondWeapon.SetIsEmpty(false);
                UiSecondWeapon.SetImage(dataItem.Thumbnail);
                UiSecondWeapon.SetTextQuantidade("");
            }
        }

    }
}