using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ApocalipseZ
{
    public class UiPrimaryAndSecondWeapons : MonoBehaviour
    {
        public UISlotItem PrefabSlot;
        private List<UISlotItem> WeaponSlots = new List<UISlotItem>();
        private Transform Container;

        private void Awake()
        {
            Container = transform.Find("Container").transform;
        }

        public void AddSlots(FpsPlayer FpsPlayer)
        {
            Inventory inventory = FpsPlayer.GetInventory();
            WeaponManager weaponManager = FpsPlayer.GetWeaponManager();
            FastItemsManager fastItemsManager = FpsPlayer.GetFastItemsManager();
            foreach (UISlotItem item in WeaponSlots)
            {
                Destroy(item.gameObject);
            }
            WeaponSlots.Clear();
            for (int i = 0; i < 2; i++)
            {
                UISlotItem instance = Instantiate(PrefabSlot, Container);
                instance.HUD = transform.parent;
                instance.SetTypeContainer(TypeContainer.WEAPONS);
                instance.SetSlotIndex(i);
                instance.SetInventory(inventory);
                instance.SetWeaponManager(weaponManager);
                instance.SetFastItemsManager(fastItemsManager);
                WeaponSlots.Add(instance);
            }
        }


        internal void UpdateSlot(int index, SlotInventoryTemp newItem)
        {
            DataItem dataItem = GameController.Instance.DataManager.GetDataItemById(newItem.guidid);
            if (dataItem == null)
            {
                WeaponSlots[index].SetIsEmpty(true);
                WeaponSlots[index].SetImage(null);
                WeaponSlots[index].SetTextQuantidade("");
            }
            else
            {
                WeaponSlots[index].SetIsEmpty(false);
                WeaponSlots[index].SetImage(dataItem.Thumbnail);
                WeaponSlots[index].SetTextQuantidade(newItem.Quantity.ToString());
            }
        }
    }
}