using System.Collections.Generic;
using UnityEngine;

namespace ApocalipseZ
{
    public class UiInventory : MonoBehaviour
    {
        public UISlotItem PrefabSlot;
        [SerializeField] private List<UISlotItem> UIItems = new List<UISlotItem>();
        private Transform slotPanel;

        void Awake()
        {
            slotPanel = transform.Find("SlotPanel").transform;
        }

        public void AddSlots(FpsPlayer FpsPlayer)
        {
            Inventory inventory = FpsPlayer.GetInventory();
            WeaponManager weaponManager = FpsPlayer.GetWeaponManager();
            FastItemsManager fastItemsManager = FpsPlayer.GetFastItemsManager();

            foreach (UISlotItem item in UIItems)
            {
                Destroy(item.gameObject);
            }
            UIItems.Clear();
            for (int i = 0; i < inventory.GetMaxSlots(); i++)
            {
                UISlotItem instance = Instantiate(PrefabSlot, slotPanel);
                instance.HUD = transform.parent;
                instance.SetSlotIndex(i);
                instance.SetInventory(inventory);
                instance.SetWeaponManager(weaponManager);
                instance.SetFastItemsManager(fastItemsManager);
                UIItems.Add(instance);
            }
        }
        public void ClearSlot(int index)
        {
            foreach (UISlotItem item in UIItems)
            {
                if (item.GetSlotIndex() == index)
                {
                    item.SetImage(null);
                    item.SetTextQuantidade("");
                }
            }
        }
        internal void UpdateSlot(int index, SlotInventoryTemp newItem)
        {

            DataItem dataItem = GameController.Instance.DataManager.GetDataItemById(newItem.guidid);
            if (dataItem == null)
            {
                UIItems[index].SetIsEmpty(true);
                UIItems[index].SetImage(null);
                UIItems[index].SetTextQuantidade("");
            }
            else
            {
                UIItems[index].SetIsEmpty(false);
                UIItems[index].SetImage(dataItem.Thumbnail);
                UIItems[index].SetTextQuantidade(newItem.Quantity.ToString());
            }


        }
    }
}