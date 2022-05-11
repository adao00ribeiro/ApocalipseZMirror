using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace ApocalipseZ
{
    public class UISlotItem : MonoBehaviour
    {
        [SerializeField]private Image Image;
        [SerializeField]private Text TextQuantidade;
        [SerializeField]SSlotInventory slot;

        private void Awake ( )
        {
            Image = transform.Find ( "Image").GetComponent<Image> ( );
            TextQuantidade = transform.Find ( "Image/TextQuantidade" ).GetComponent<Text> ( );
        }
        public void UpdateSlot ( SSlotInventory slotItem )
        {
            slot = slotItem ;
            Image.sprite = slot.item.Thumbnail;
            TextQuantidade.text = slot.Quantity.ToString();
        }
    }
}