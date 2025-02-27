/*
	* Coder :
	* Last Update :
	* Information
*/
namespace project02
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using static project02.ItemData;

    public partial class ItemSlot : MonoBehaviour // Data Field
    {
        [SerializeField] private Image itemImage;
        [SerializeField] private ItemType allowedType;
        private Item item;
    }
    public partial class ItemSlot : MonoBehaviour // Initialize
    {
        private void Allocate()
        {

        }
        public void Initialize()
        {
            Allocate();
            Setup();
        }
        private void Setup()
        {

        }
    }

    public partial class ItemSlot : MonoBehaviour // Property
    {
        public void AddItem(Item itemValue)
        {
            ItemInformation itemInfo = itemValue.ItemInformation;
            ItemType itemType = Enum.Parse<ItemType>(itemInfo.item_type);
            if (allowedType == itemType)
            {
                item = itemValue;
                itemImage.sprite = Resources.Load<Sprite>("Item/" + itemInfo.item_icon);
            }
        }
        public void RemoveItem()
        {
            item = null;
            itemImage.sprite = null;
        }
    }
}
