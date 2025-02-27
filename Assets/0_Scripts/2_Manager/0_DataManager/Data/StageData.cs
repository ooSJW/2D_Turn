/*
	* Coder :
	* Last Update :
	* Information
*/
namespace project02
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public partial class StageData // Information
    {
        [System.Serializable]
        public class StageInformation : BaseInformation
        {
            public string chapter;
            public string start_stage_index;
            public string stage_name;
            public string reward_type;
            public string[] knight_name;

            public int stage;
            public int stage_score;
            public int is_last_stage;
            public int is_clear;
            public int[] spawn_count;
            public int reward_exp;
            public int reward_gold;
            public int reward_item_count;

            public string[] spawnable_enemy;
            public string[] enemy_type;
        }
    }
    public partial class StageData // Data Field
    {
        private Dictionary<string, StageInformation> stageInfoDict;
    }

    public partial class StageData // Initialize
    {
        private void Allocate()
        {
            stageInfoDict = new Dictionary<string, StageInformation>();
        }
        public void Initialize()
        {
            Allocate();
            Setup();
        }
        private void Setup()
        {
            MainSystem.Instance.DataManager.SetUpData<StageInformation>(stageInfoDict, "StageData");
        }
    }

    public partial class StageData // Property
    {
        public StageInformation GetData(string index)
        {
            return stageInfoDict[index];
        }
        public bool ContainsKey(string key)
        {
            return stageInfoDict.ContainsKey(key);
        }
    }
}
