/*
	* Coder :
	* Last Update :
	* Information
*/
namespace project02
{
    using DG.Tweening;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;
    using static project02.ItemData;
    using static project02.KnightStatData;

    public partial class KnightManagementUI : MonoBehaviour // Data Field
    {
        [SerializeField] private Transform knightParent;
        [SerializeField] private Transform[] activeKnightParent;
        [SerializeField] private GameObject knightInfoUI;
        [SerializeField] private Button equipButton;
        [SerializeField] private Button unEquipButton;
        [SerializeField] private Image expFillImage;

        [SerializeField] private GameObject inventory;
        [SerializeField] private Transform itemParent;
        [SerializeField] private Button equipItemButton;
        [SerializeField] private Button unEquipItemButton;
        [SerializeField] private Image weaponImage;
        [SerializeField] private Image armorImage;
        [SerializeField] private TextMeshProUGUI weaponText;
        [SerializeField] private TextMeshProUGUI armorText;
        [SerializeField] private GameObject reinforceFailMessage;

        [SerializeField] private GameObject itemInfoField;
        [SerializeField] private Image itemInfoImage;
        [SerializeField] private TextMeshProUGUI reinforceMessage;
        [SerializeField] private TextMeshProUGUI strengthenLevelText;
        [SerializeField] private Button reinforceButton;
        [SerializeField] private ScrollRect scrollRect;

        [SerializeField] private Image knightImage;
        [SerializeField] private TextMeshProUGUI knightNameText;
        [SerializeField] private TextMeshProUGUI knightInfoText;

        [SerializeField] private BlinkImage blinkImage;
        public BlinkImage BlinkImage { get => blinkImage; }
        public KnightIcon SelectedKnightIcon { get; private set; } = null;
        public Item SelectedItem { get; private set; } = null;
    }
    public partial class KnightManagementUI : MonoBehaviour // Initialize
    {
        private void Allocate()
        {

        }
        public void Initialize()
        {
            Allocate();
            Setup();
            SpawnKnightImage();
        }
        private void Setup()
        {

        }
    }

    public partial class KnightManagementUI : MonoBehaviour // Private Property
    {
        private void SpawnKnightImage()
        {
            List<string> knightList = MainSystem.Instance.PlayerManager.Player.knightList;
            List<Knight> activeKnightList = MainSystem.Instance.PlayerManager.Player.activeKnightList;
            Transform resultParent;
            KnightStatInformation resultInfo;
            int parentIndex = 0;
            KnightIcon knightIcon;
            KnightName knightName;

            for (int i = 0; i < knightList.Count; i++)
            {
                Knight activeKnight = activeKnightList.Find(elem => elem.name == knightList[i]);
                knightName = Enum.Parse<KnightName>(knightList[i]);
                int index = (int)knightName;

                if (activeKnight != null)
                {
                    parentIndex = activeKnightList.IndexOf(activeKnight);
                    activeKnight.IsEquip = true;
                    resultParent = activeKnightParent[parentIndex];
                    resultInfo = activeKnight.KnightStatInformation;
                }
                else
                {
                    resultParent = knightParent;
                    resultInfo = MainSystem.Instance.DataManager.KnightStatData.GetData(index.ToString());
                }

                knightIcon = MainSystem.Instance.PoolManager.Spawn("KnightUIButton", resultParent).GetComponent<KnightIcon>();

                knightIcon.Initialize(resultInfo.name);
            }
        }

        private void SetInfoText(Knight knight)
        {
            int level = knight.Level;

            knightImage.sprite = Resources.Load<Sprite>("Knight/" + knight.KnightStatInformation.knight_icon);
            knightNameText.text = knight.name;
            knightInfoText.text = $"LV : {level}\n생명력 : {knight.MaxHp}\n공격력 : {knight.Attack}\n방어력 : {knight.Defence}";
        }

        private void RefreshUI(int index = 0)
        {
            RectTransform knightIconrectTransform = SelectedKnightIcon.GetComponent<RectTransform>();
            if (SelectedKnightIcon.Knight != null)
            {
                knightIconrectTransform.SetParent(activeKnightParent[index]);
                knightIconrectTransform.anchorMin = Vector2.zero;
                knightIconrectTransform.anchorMax = Vector2.one;
                knightIconrectTransform.offsetMin = Vector2.zero;
                knightIconrectTransform.offsetMax = Vector2.zero;
            }
            else
            {
                knightIconrectTransform.SetParent(knightParent);
                knightIconrectTransform.sizeDelta = SelectedKnightIcon.originSize;
            }
            knightIconrectTransform.localScale = Vector2.one;
        }

        private void SetInteractableKnightChild(bool value)
        {
            int childCount = knightParent.childCount;
            Button[] buttons = new Button[childCount];

            for (int i = 0; i < childCount; i++)
            {
                buttons[i] = knightParent.GetChild(i).GetComponent<Button>();
                buttons[i].interactable = value;
            }
        }
        private void SetItemIcon()
        {
            /*
            string jsonString = PlayerPrefs.GetString(SelectedKnightIcon.KnightName.ToString() + "Json");

            string weapon = string.Empty;
            string armor = string.Empty;
            string path = string.Empty;

            if (!string.IsNullOrEmpty(jsonString))
            {
                JObject json = JsonConvert.DeserializeObject<JObject>(jsonString);
                if (json.TryGetValue("Weapon", out JToken weaponToken) && weaponToken is JArray)
                {
                    JArray jArray = weaponToken as JArray;
                    JObject itemObject = jArray[0] as JObject;
                    if (itemObject != null)
                    {
                        if (itemObject.TryGetValue("item_name", out JToken weaponElemToken))
                            weapon = weaponElemToken.ToString();
                    }
                }
                if (json.TryGetValue("Armor", out JToken armorToken) && armorToken is JArray)
                {
                    JArray jArray = armorToken as JArray;
                    JObject itemObject = jArray[0] as JObject;
                    if (itemObject != null)
                    {
                        if (itemObject.TryGetValue("item_name", out JToken armorElemToken))
                            armor = armorElemToken.ToString();
                    }

                }
            }

            if (weapon != string.Empty)
            {
                print(weapon);
                ItemName itemName = Enum.Parse<ItemName>(weapon);
                int itemIndex = (int)itemName;
                path = MainSystem.Instance.DataManager.ItemData.GetData(itemIndex.ToString()).item_icon;
                weaponImage.sprite = Resources.Load<Sprite>("Item/" + path);
                weaponImage.gameObject.SetActive(true);
            }
            else
                weaponImage.gameObject.SetActive(false);

            if (armor != string.Empty)
            {
                ItemName itemName = Enum.Parse<ItemName>(armor);
                int itemIndex = (int)itemName;
                path = MainSystem.Instance.DataManager.ItemData.GetData(itemIndex.ToString()).item_icon;
                armorImage.sprite = Resources.Load<Sprite>("Item/" + path);
                armorImage.gameObject.SetActive(true);
            }
            else
                armorImage.gameObject.SetActive(false);
            */
            Item weapon = SelectedKnightIcon.Knight.Weapon;
            Item armor = SelectedKnightIcon.Knight.Armor;

            if (weapon == null)
            {
                weaponText.gameObject.SetActive(true);
                weaponImage.gameObject.SetActive(false);
            }
            else if (weapon != null)
            {
                weaponText.gameObject.SetActive(false);
                weaponImage.sprite = Resources.Load<Sprite>("Item/" + weapon.ItemInformation.item_icon);
                weaponImage.gameObject.SetActive(true);
            }

            if (armor == null)
            {
                armorText.gameObject.SetActive(true);
                armorImage.gameObject.SetActive(false);
            }
            else if (armor != null)
            {
                armorText.gameObject.SetActive(false);
                armorImage.sprite = Resources.Load<Sprite>("Item/" + armor.ItemInformation.item_icon);
                armorImage.gameObject.SetActive(true);
            }


        }
    }

    public partial class KnightManagementUI : MonoBehaviour // Property
    {
        public void OnOffKnightUI()
        {
            if (SelectedKnightIcon != null)
                SelectedKnightIcon.KnightIconQuitBtnClick();

            SelectedKnightIcon = null;
            MainSystem.Instance.DataManager.SavePlayerData();
            MainSystem.Instance.DataManager.SaveItemData();
            gameObject.SetActive(!gameObject.activeSelf);
            knightInfoUI.SetActive(false);
            inventory.SetActive(false);
            blinkImage.Initialize();
        }
        public void OnOffKnightInfoUI()
        {
            if (SelectedKnightIcon != null)
                SelectedKnightIcon.KnightIconQuitBtnClick();

            SelectedKnightIcon = null;
            knightInfoUI.SetActive(false);
        }

        public void OnOffKnightInfoUI(KnightIcon knightIconValue)
        {
            SelectedKnightIcon = knightIconValue;

            SetInfoText(knightIconValue.Knight);

            bool isEquip = MainSystem.Instance.PlayerManager.Player.activeKnightList.Find(elem => elem.name == knightIconValue.Knight.KnightStatInformation.name);
            equipButton.gameObject.SetActive(!isEquip);
            unEquipButton.gameObject.SetActive(isEquip);

            int maxEpx = SelectedKnightIcon.Knight.KnightStatInformation.max_exp[SelectedKnightIcon.Knight.CurrentIndex];
            int currentExp = SelectedKnightIcon.Knight.Exp;
            expFillImage.fillAmount = Mathf.Clamp01((float)currentExp / maxEpx);


            string knightName = knightIconValue.Knight.name;
            if (SelectedKnightIcon.Knight.Weapon == null)
            {
                Item item = MainSystem.Instance.PlayerManager.Player.itemList.Find(elem => elem.Owner == knightName && elem.ItemInformation.item_type == ItemType.Weapon.ToString());
                if (item != null)
                    SelectedKnightIcon.Knight.Weapon = item;
            }
            if (SelectedKnightIcon.Knight.Armor == null)
            {
                Item item = MainSystem.Instance.PlayerManager.Player.itemList.Find(elem => elem.Owner == knightName && elem.ItemInformation.item_type == ItemType.Armor.ToString());
                if (item != null)
                    SelectedKnightIcon.Knight.Armor = item;
            }

            SetItemIcon();

            knightInfoUI.SetActive(!knightInfoUI.gameObject.activeSelf);
        }
        public void SetCurrentKnightIcon(KnightIcon knightIconValue)
        {
            SelectedKnightIcon = knightIconValue;
        }

        public void EquipBtnClick()
        {
            knightInfoUI.SetActive(false);
            blinkImage.Initialize(true);
        }

        public void EquipKnight(int index)
        {
            if (SelectedKnightIcon != null)
            {
                SelectedKnightIcon.KnightIconQuitBtnClick();
                SelectedKnightIcon.EquipKnight(index);
                SelectedKnightIcon.GetComponent<Button>().interactable = true;
                RefreshUI(index);
                blinkImage.Initialize();
                SelectedKnightIcon = null;
            }
        }
        public void UnEquipKnight()
        {
            if (SelectedKnightIcon != null)
            {
                SelectedKnightIcon.UnEquipKnight();
                RefreshUI();
                knightInfoUI.SetActive(false);
                SelectedKnightIcon = null;
            }
        }

        public void VisibleInventory(int itemTypeValue)
        {
            List<Item> itemList = MainSystem.Instance.PlayerManager.Player.itemList;
            ItemType itemType = Enum.Parse<ItemType>(itemTypeValue.ToString());
            for (int i = 0; i < itemList.Count; i++)
            {
                ItemInformation itemInfo = itemList[i].ItemInformation;
                if (itemInfo.item_type == itemType.ToString())
                {
                    if (itemList[i].Owner == string.Empty || itemList[i].Owner == SelectedKnightIcon.Knight.name)
                        itemList[i].gameObject.SetActive(true);
                    else
                        itemList[i].gameObject.SetActive(false);
                }
                else
                    itemList[i].gameObject.SetActive(false);
            }

            itemInfoField.SetActive(false);
            scrollRect.normalizedPosition = new Vector2(0, 1);

            equipItemButton.gameObject.SetActive(false);
            unEquipItemButton.gameObject.SetActive(false);
            inventory.SetActive(true);
        }

        public void UnVisibleInventory()
        {
            SelectedItem = null;
            inventory.SetActive(false);
        }
        public void ItemBtnClick(Item itemValue)
        {
            /*
            SelectedItem = itemValue;
            string json = PlayerPrefs.GetString(SelectedKnightIcon.Knight.name + "Json");
            string itemType = itemValue.ItemInformation.item_type;
            string item = string.Empty;

            if (!string.IsNullOrEmpty(json))
            {
                JObject jobject = JsonConvert.DeserializeObject<JObject>(json);
                if (jobject.TryGetValue(itemType, out JToken itemToken) && itemToken is JArray)
                {
                    JArray jArray = itemToken as JArray;
                    JObject itemObject = jArray[0] as JObject;
                    if (itemObject != null)
                    {
                        if (itemObject.TryGetValue("item_name", out JToken itemNameToken))
                            item = itemNameToken.ToString();
                    }
                }
            }
            if (item == itemValue.name)
                unEquipItemButton.gameObject.SetActive(true);

            bool isEquip = Convert.ToBoolean(SelectedItem.IsEquip);
            equipItemButton.gameObject.SetActive(!isEquip);
            */
            SelectedItem = itemValue;
            if (SelectedItem.Owner == SelectedKnightIcon.Knight.name)
                unEquipItemButton.gameObject.SetActive(true);
            else if (string.IsNullOrEmpty(SelectedItem.Owner))
            {
                unEquipItemButton.gameObject.SetActive(false);
                equipItemButton.gameObject.SetActive(true);
            }
            else
            {
                unEquipItemButton.gameObject.SetActive(false);
                equipItemButton.gameObject.SetActive(false);
            }

            bool isActive = SelectedItem != null ? true : false;
            if (isActive)
            {
                itemInfoImage.sprite = Resources.Load<Sprite>("Item/" + SelectedItem.ItemInformation.item_icon);
                RefreshReinforceUI();
            }
            itemInfoField.SetActive(isActive);
        }
        public void RefreshReinforceUI()
        {
            if (SelectedItem.StrengthenLevel < SelectedItem.ItemInformation.max_strengthen_level)
            {
                reinforceMessage.text = $"강화하기\n{SelectedItem.ItemInformation.reinforce_cost[SelectedItem.StrengthenLevel]}Coin";
                reinforceButton.interactable = true;
            }
            else
            {
                reinforceMessage.text = $"최대레벨입니다";
                reinforceButton.interactable = false;
            }
            strengthenLevelText.text = $"강화단계 : {SelectedItem.StrengthenLevel}";
        }
        public void EquipItem()
        {
            if (SelectedItem.ItemInformation.item_type == ItemType.Weapon.ToString())
            {
                if (SelectedKnightIcon.Knight.Weapon != null)
                    SelectedKnightIcon.Knight.Weapon.Owner = string.Empty;

                SelectedKnightIcon.Knight.Weapon = SelectedItem;
            }
            else if (SelectedItem.ItemInformation.item_type == ItemType.Armor.ToString())
            {
                if (SelectedKnightIcon.Knight.Armor != null)
                    SelectedKnightIcon.Knight.Armor.Owner = string.Empty;

                SelectedKnightIcon.Knight.Armor = SelectedItem;
            }

            MainSystem.Instance.DataManager.EquipItem(SelectedKnightIcon.Knight.name, SelectedItem);
            SelectedItem.Owner = SelectedKnightIcon.KnightName.ToString();
            SelectedKnightIcon.Knight.RefreshStat();
            SetInfoText(SelectedKnightIcon.Knight);
            SetItemIcon();
            inventory.SetActive(false);
        }
        public void UnEquipItem()
        {
            if (SelectedItem.ItemInformation.item_type == ItemType.Weapon.ToString())
                SelectedKnightIcon.Knight.Weapon = null;
            else if (SelectedItem.ItemInformation.item_type == ItemType.Armor.ToString())
                SelectedKnightIcon.Knight.Armor = null;

            MainSystem.Instance.DataManager.UnEquipItem(SelectedItem);
            SelectedItem.Owner = string.Empty;
            SelectedKnightIcon.Knight.RefreshStat();
            SetInfoText(SelectedKnightIcon.Knight);
            SetItemIcon();
            inventory.SetActive(false);
        }

        public void ReinforceItem()
        {
            ItemInformation itemInfo = SelectedItem.ItemInformation;
            int coin = MainSystem.Instance.PlayerManager.Player.Coin;
            int reinforceCost = itemInfo.reinforce_cost[SelectedItem.StrengthenLevel];

            if (SelectedItem.StrengthenLevel < itemInfo.max_strengthen_level)
            {
                if (coin >= reinforceCost)
                {   // 아이템을 강화했을 때
                    MainSystem.Instance.PlayerManager.Player.UseCoin(reinforceCost);
                    SelectedItem.StrengthenLevel++; // 아이템 객체 능력치 업데이트
                    SelectedKnightIcon.Knight?.RefreshStat(); // Item객체의 Owner이 있을 때 Owner객체의 능력치 업데이트
                    MainSystem.Instance.DataManager.EquipItem(SelectedKnightIcon.Knight.name, SelectedItem);
                    RefreshReinforceUI();
                    SetInfoText(SelectedKnightIcon.Knight);
                    MainSystem.Instance.SoundManager.SoundController.Sfx.PlayReinforceSound();
                }
                else
                {
                    reinforceFailMessage.gameObject.SetActive(true);
                    MainSystem.Instance.SoundManager.SoundController.Sfx.PlayButtonClickSound();
                }
            }
        }
    }
}
