/*
	* Coder :
	* Last Update :
	* Information
*/
namespace project02
{
    using JetBrains.Annotations;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Unity.VisualScripting;
    using UnityEngine;
    using static project02.ItemData;
    using static project02.SaveData;

    public partial class DataManager : MonoBehaviour // Data
    {
        public StageData StageData { get; private set; } = default;
        public KnightStatData KnightStatData { get; private set; } = default;
        public SkillData SkillData { get; private set; } = default;
        public EnemyStatData EnemyStatData { get; private set; } = default;
        public ItemData ItemData { get; private set; } = default;
        public SaveData SaveData { get; private set; } = default;
    }
    public partial class DataManager : MonoBehaviour // Initialize
    {
        private void Allocate()
        {
            StageData = new StageData();
            KnightStatData = new KnightStatData();
            SkillData = new SkillData();
            EnemyStatData = new EnemyStatData();
            ItemData = new ItemData();
            SaveData = new SaveData();
        }
        public void Initialize()
        {
            Allocate();
            Setup();

            StageData.Initialize();
            KnightStatData.Initialize();
            SkillData.Initialize();
            EnemyStatData.Initialize();
            ItemData.Initialize();
            SaveData.Initialize();
        }
        private void Setup()
        {

        }
    }
    public partial class DataManager : MonoBehaviour // Property
    {
        private Wrapper<T> LoadJson<T>(string path) where T : BaseInformation
        {
            string jsonStringData = Resources.Load<TextAsset>(path).ToString();
            return JsonConvert.DeserializeObject<Wrapper<T>>(jsonStringData);
        }

        public void SetUpData<T>(Dictionary<string, T> dataDict, string path) where T : BaseInformation
        {
            dataDict.Clear();
            Wrapper<T> jsonData = LoadJson<T>(path);

            foreach (T data in jsonData.array)
            {
                dataDict.Add(data.index, data);
            }
        }
        public void ClearData()
        {
            PlayerPrefs.DeleteAll();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
        public void SaveStageData(int stageIndex, int scoreValue)
        {
            JObject json = new JObject();
            JArray clearStage = new JArray();
            JArray stageScore = new JArray();
            string jsonString = PlayerPrefs.GetString("StageJson");
            int score = Mathf.Clamp(scoreValue, 0, 3);

            if (!string.IsNullOrEmpty(jsonString))
            {
                json = JsonConvert.DeserializeObject<JObject>(jsonString);
                if (json.TryGetValue("ClearStage", out JToken clearStageArray) && clearStageArray is JArray)
                {
                    clearStage = (JArray)clearStageArray;

                    if (json.TryGetValue("ScoreArray", out JToken scoreArray) && scoreArray is JArray)
                        stageScore = (JArray)scoreArray;

                    if (!clearStage.Any(value => JToken.DeepEquals(value, stageIndex)))
                    {
                        clearStage.Add(stageIndex);
                        stageScore.Add(score);
                    }
                    else
                    {
                        int index = -1;
                        for (int i = 0; i < clearStage.Count(); i++)
                        {
                            if ((int)clearStage[i] == stageIndex)
                            {
                                index = i;
                                break;
                            }
                        }
                        if (index != -1)
                        {
                            if (score > (int)stageScore[index])
                            {
                                stageScore[index] = score;
                                PlayerPrefs.SetString("StageJson", json.ToString());
                                return;
                            }
                        }
                    }
                }
            }
            else
            {
                clearStage.Add(stageIndex);
                stageScore.Add(score);
                json.Add("ClearStage", clearStage);
                json.Add("ScoreArray", stageScore);
            }
            PlayerPrefs.SetString("StageJson", json.ToString());
        }

        public void LoadStageData()
        {
            string jsonString = PlayerPrefs.GetString("StageJson");
            if (!string.IsNullOrEmpty(jsonString))
            {
                JObject json = JsonConvert.DeserializeObject<JObject>(jsonString);
                JArray stage;
                JArray score;

                if (json.TryGetValue("ClearStage", out JToken clearStage) && clearStage is JArray)
                {
                    stage = clearStage as JArray;
                    MainSystem.Instance.InGameManager.ClearStageCount = stage.Count;
                }
                if (json.TryGetValue("ScoreArray", out JToken scoreArray) && scoreArray is JArray)
                {
                    score = scoreArray as JArray;
                    MainSystem.Instance.StageManager.ScoreList.Clear();
                    for (int i = 0; i < score.Count; i++)
                        MainSystem.Instance.StageManager.ScoreList.Add((int)score[i]);
                }

            }
        }

        public void SavePlayerData()
        {
            JObject json = new JObject();
            JArray knightArray = new JArray();
            JArray activeKnightArray = new JArray();

            List<string> knightList = MainSystem.Instance.PlayerManager.Player.knightList;
            for (int i = 0; i < knightList.Count; i++)
                knightArray.Add(knightList[i]);

            List<Knight> activeKnightList = MainSystem.Instance.PlayerManager.Player.activeKnightList;
            for (int i = 0; i < activeKnightList.Count; i++)
                activeKnightArray.Add(activeKnightList[i].name);

            int coin = MainSystem.Instance.PlayerManager.Player.Coin;

            json.Add("Knight", knightArray);
            json.Add("ActiveKnight", activeKnightArray);
            json.Add("Coin", coin);
            PlayerPrefs.SetString("PlayerJson", json.ToString());
        }
        public void LoadPlayerData()
        {
            JObject json = new JObject();
            JArray knightArr = new JArray();
            JArray activeKnightArr = new JArray();

            string jsonString = PlayerPrefs.GetString("PlayerJson");
            if (!string.IsNullOrEmpty(jsonString))
            {
                json = JsonConvert.DeserializeObject<JObject>(jsonString);
                if (json.TryGetValue("Knight", out JToken knightToken) && knightToken is JArray)
                    knightArr = (JArray)knightToken;
                if (json.TryGetValue("ActiveKnight", out JToken activeKnightToken) && activeKnightToken is JArray)
                    activeKnightArr = (JArray)activeKnightToken;

                MainSystem.Instance.PlayerManager.Player.knightList.Clear();
                for (int i = 0; i < knightArr.Count; i++)
                    MainSystem.Instance.PlayerManager.Player.knightList.Add(knightArr[i].ToString());

                MainSystem.Instance.PlayerManager.Player.activeKnightList.Clear();
                for (int i = 0; i < activeKnightArr.Count; i++)
                {
                    Knight knight = MainSystem.Instance.PlayerManager.Player.SpawnKnight(activeKnightArr[i].ToString(), i);
                    string knightJsonString = PlayerPrefs.GetString(knight.name + "Json");
                    if (!string.IsNullOrEmpty(knightJsonString))
                    {
                        JObject knightJson = JsonConvert.DeserializeObject<JObject>(knightJsonString);
                        if (knightJson.TryGetValue("Level", out JToken levelToken) && (int)levelToken != 0)
                            knight.Level = (int)levelToken;
                        if (knightJson.TryGetValue("Exp", out JToken expToken) && (int)expToken != 0)
                            knight.Exp = (int)expToken;
                        if (knightJson.TryGetValue("Weapon", out JToken weaponToken) && weaponToken is JArray)
                        {
                            JArray jArray = weaponToken as JArray;
                            JObject itemObject = jArray[0] as JObject;
                            if (itemObject != null)
                            {
                                string index = itemObject["index"].ToString();
                                int strengthenLevel = (int)itemObject["strengthen_level"];
                                ItemInformation itemInformation = MainSystem.Instance.DataManager.ItemData.GetData(index);
                                int power = itemInformation.power + (itemInformation.growth_per_strengthen * strengthenLevel);
                                knight.Attack += power;
                            }
                        }
                        if (knightJson.TryGetValue("Armor", out JToken armorToken) && armorToken is JArray)
                        {
                            JArray jArray = armorToken as JArray;
                            JObject itemObject = jArray[0] as JObject;
                            if (itemObject != null)
                            {
                                string index = itemObject["index"].ToString();
                                int strengthenLevel = (int)itemObject["strengthen_level"];
                                ItemInformation itemInformation = MainSystem.Instance.DataManager.ItemData.GetData(index);
                                int power = itemInformation.power + (itemInformation.growth_per_strengthen * strengthenLevel);
                                knight.Defence += power;
                            }
                        }

                        knight.IsEquip = true;
                    }
                }
                if (json.TryGetValue("Coin", out JToken coinToken))
                    MainSystem.Instance.PlayerManager.Player.GetCoin((int)coinToken);
            }
        }

        public void SaveKnightData()
        {
            List<Knight> activeKnightList = MainSystem.Instance.PlayerManager.Player.activeKnightList;
            for (int i = 0; i < activeKnightList.Count; i++)
            {
                JObject json = new JObject();
                string jsonString = PlayerPrefs.GetString(activeKnightList[i].name + "Json");
                if (!string.IsNullOrEmpty(jsonString))
                {
                    json = JsonConvert.DeserializeObject<JObject>(jsonString);

                    if (json.ContainsKey("Level"))
                    {
                        json["Level"] = activeKnightList[i].Level;
                        json["Exp"] = activeKnightList[i].Exp;
                    }
                    else
                    {
                        json.Add("Level", activeKnightList[i].Level);
                        json.Add("Exp", activeKnightList[i].Exp);
                    }
                }
                else
                {
                    json.Add("Level", activeKnightList[i].Level);
                    json.Add("Exp", activeKnightList[i].Exp);
                }
                PlayerPrefs.SetString(activeKnightList[i].name + "Json", json.ToString());
            }
        }

        public void LoadKnightData(Knight knight)
        {
            string knightJsonString = PlayerPrefs.GetString(knight.name + "Json");
            if (!string.IsNullOrEmpty(knightJsonString))
            {
                JObject knightJson = JsonConvert.DeserializeObject<JObject>(knightJsonString);
                if (knightJson.TryGetValue("Level", out JToken levelToken) && (int)levelToken != 0)
                    knight.Level = (int)levelToken;
                if (knightJson.TryGetValue("Exp", out JToken expToken) && (int)expToken != 0)
                    knight.Exp = (int)expToken;
                if (knightJson.TryGetValue("Weapon", out JToken weaponToken) && weaponToken is JArray)
                {
                    JArray jArray = weaponToken as JArray;
                    JObject itemObject = jArray[0] as JObject;
                    if (itemObject != null)
                    {
                        string index = itemObject["index"].ToString();
                        int strengthenLevel = (int)itemObject["strengthen_level"];
                        ItemInformation itemInformation = MainSystem.Instance.DataManager.ItemData.GetData(index);
                        int power = itemInformation.power + (itemInformation.growth_per_strengthen * strengthenLevel);
                        knight.Attack = knight.KnightStatInformation.attack[knight.CurrentIndex] + power;
                    }
                }
                if (knightJson.TryGetValue("Armor", out JToken armorToken) && armorToken is JArray)
                {
                    JArray jArray = armorToken as JArray;
                    JObject itemObject = jArray[0] as JObject;
                    if (itemObject != null)
                    {
                        string index = itemObject["index"].ToString();
                        int strengthenLevel = (int)itemObject["strengthen_level"];
                        ItemInformation itemInformation = MainSystem.Instance.DataManager.ItemData.GetData(index);
                        int power = itemInformation.power + (itemInformation.growth_per_strengthen * strengthenLevel);
                        knight.Defence = knight.KnightStatInformation.defence[knight.CurrentIndex] + power;
                    }
                }
            }
        }

        public void EquipItem(string knightName, Item item)
        {
            JObject json = new JObject();
            string jsonString = PlayerPrefs.GetString(knightName + "Json");
            string itemType = string.Empty;

            if (item.ItemInformation.item_type == ItemType.Weapon.ToString())
                itemType = "Weapon";
            else if (item.ItemInformation.item_type == ItemType.Armor.ToString())
                itemType = "Armor";

            JArray jsonArray = new JArray(new JObject
                {
                    { "index", item.ItemInformation.index },
                    { "item_name", item.name },
                    { "strengthen_level", item.StrengthenLevel },
                    { "owner",knightName },
                });

            if (!string.IsNullOrEmpty(jsonString))
            {
                json = JsonConvert.DeserializeObject<JObject>(jsonString);
                if (itemType != string.Empty)
                {
                    if (json.ContainsKey(itemType))
                        json[itemType] = jsonArray;
                    else
                        json.Add(itemType, jsonArray);
                }
            }
            else
                json.Add(itemType, jsonArray);

            PlayerPrefs.SetString(knightName + "Json", json.ToString());
        }

        public void UnEquipItem(Item item)
        {
            JObject json = new JObject();
            string jsonString = PlayerPrefs.GetString(item.Owner + "Json");
            string itemType;

            if (item.ItemInformation.item_type == ItemType.Weapon.ToString())
                itemType = "Weapon";
            else
                itemType = "Armor";

            if (!string.IsNullOrEmpty(jsonString))
            {
                json = JsonConvert.DeserializeObject<JObject>(jsonString);
                if (json.ContainsKey(itemType))
                    json.Remove(itemType);
            }

            PlayerPrefs.SetString(item.Owner + "Json", json.ToString());
        }


        public void SaveItemData()
        {
            List<Item> itemList = MainSystem.Instance.PlayerManager.Player.itemList;
            JArray jsonArray = new JArray();
            foreach (Item item in itemList)
            {
                jsonArray.Add(new JObject
                {
                    { "index", item.ItemInformation.index },
                    { "item_name", item.name },
                    { "strengthen_level", item.StrengthenLevel },
                    { "owner",item.Owner },
                });
            }
            PlayerPrefs.SetString("SaveItemData", jsonArray.ToString());
        }

        public void LoadItemData()
        {
            string jsonString = PlayerPrefs.GetString("SaveItemData");
            JArray jsonArray = new JArray();

            if (!string.IsNullOrEmpty(jsonString))
            {
                jsonArray = JsonConvert.DeserializeObject<JArray>(jsonString);
                foreach (JToken itemData in jsonArray)
                {
                    string itemName = itemData["item_name"].ToString();
                    Item item = MainSystem.Instance.ItemManager.ItemController.SpawnItem(itemName);
                    item.Initialize();
                    item.StrengthenLevel = (int)itemData["strengthen_level"];

                    string owner = itemData["owner"].ToString();
                    item.Owner = owner != null ? owner : string.Empty;
                    MainSystem.Instance.PlayerManager.Player.itemList.Add(item);
                }
            }
        }
    }
}
