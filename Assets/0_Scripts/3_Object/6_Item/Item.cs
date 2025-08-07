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
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;
    using static project02.ItemData;

    public partial class Item : MonoBehaviour // Data Property
    {
        private ItemInformation itemInformation;
        public ItemInformation ItemInformation
        {
            get => itemInformation;
            set
            {
                itemInformation = new ItemInformation()
                {
                    index = value.index,
                    item_name = value.item_name,
                    item_icon = value.item_icon,
                    item_type = value.item_type,
                    prefab_name = value.prefab_name,
                    power = value.power,
                    growth_per_strengthen = value.growth_per_strengthen,
                    strengthen_level = value.strengthen_level,
                    max_strengthen_level = value.max_strengthen_level,
                    reinforce_cost = value.reinforce_cost,
                };
                StrengthenLevel = value.strengthen_level;
                Power = value.power;
            }
        }

        private int strengthenlevel;
        public int StrengthenLevel
        {   // 해당 프로퍼티의 값 변경 시 아이템 능력치 향상 및 UI초기화
            get => strengthenlevel;
            set
            {
                if (value != strengthenlevel)
                {
                    if (value <= ItemInformation.max_strengthen_level)
                        strengthenlevel = value;
                    Power = ItemInformation.power + ItemInformation.growth_per_strengthen * strengthenlevel;
                    SetInfoText();
                }
            }
        }

        private int power;
        public int Power { get => power; private set => power = value; }

        private string owner = string.Empty;
        public string Owner
        {
            get => owner;
            set
            {
                if (owner != value)
                {
                    owner = value;
                    if (owner != string.Empty)
                        IsEquip = true;
                    else
                        IsEquip = false;
                }
            }
        }
    }
    public partial class Item : MonoBehaviour // Data Field
    {
        [SerializeField] private Image itemIcon;
        [SerializeField] private TextMeshProUGUI itemNameText;
        [SerializeField] private TextMeshProUGUI itemInfoText;

        public bool IsEquip { get; private set; } = false;
        private ItemName itemName;
    }
    public partial class Item : MonoBehaviour // Initialize
    {
        private void Allocate()
        {
            itemName = Enum.Parse<ItemName>(gameObject.name);
            int index = (int)itemName;
            ItemInformation = MainSystem.Instance.DataManager.ItemData.GetData(index.ToString());
            Power = Power += ItemInformation.growth_per_strengthen * strengthenlevel;
        }
        public void Initialize()
        {
            Allocate();
            Setup();
            SetInfoText();
        }
        private void Setup()
        {

        }
    }

    public partial class Item : MonoBehaviour // Private Property
    {
        private void SetInfoText()
        {
            itemIcon.sprite = Resources.Load<Sprite>("Item/" + ItemInformation.item_icon);
            itemNameText.text = itemInformation.item_name;

            string power = string.Empty;
            if (itemInformation.item_type == ItemType.Armor.ToString())
                power = "방어력";
            else
                power = "공격력";

            itemInfoText.text = $"{power} : {Power}\n강화단계 : {StrengthenLevel}";
        }
    }

    public partial class Item : MonoBehaviour // Property
    {
        public void ItemBtnClick()
        {
            MainSystem.Instance.UIManager.UIController.KnightManagementUI.ItemBtnClick(this);
            MainSystem.Instance.SoundManager.SoundController.Sfx.PlayButtonClickSound();
        }
    }
}
