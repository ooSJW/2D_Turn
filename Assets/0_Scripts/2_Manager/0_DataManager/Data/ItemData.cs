/*
	* Coder :
	* Last Update :
	* Information
*/
namespace project02
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public partial class ItemData  // Information
    {
        [System.Serializable]
        public class ItemInformation : BaseInformation
        {
            public string item_name;
            public string item_icon;
            public string item_type;
            public string prefab_name;

            public int power;
            public int amount;
            public int growth_per_strengthen;
            public int strengthen_level;
            public int max_strengthen_level;
            public int[] reinforce_cost;
        }
    }
    public partial class ItemData  // Data Field
    {
        private Dictionary<string, ItemInformation> itemDataDict;
    }
    public partial class ItemData  // Initialize
    {
        private void Allocate()
        {
            itemDataDict = new Dictionary<string, ItemInformation>();
        }
        public void Initialize()
        {
            Allocate();
            Setup();
        }
        private void Setup()
        {
            MainSystem.Instance.DataManager.SetUpData<ItemInformation>(itemDataDict, "ItemData");
        }
    }

    public partial class ItemData  // Property
    {
        public ItemInformation GetData(string index)
        {
            return itemDataDict[index];
        }
    }
}
