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
    public partial class SkillData // Inforamtion
    {
        [System.Serializable]
        public class SkillInformation : BaseInformation
        {
            public float power;
            public float power_growth;
            public float cool_time;
            public float range;

            public int level;
            public int max_level;

            public string skill_type;
            public string skill_icon;
            public string skill_effect;
            public string skill_sound;
            public string owner;
        }
    }

    public partial class SkillData // Data Field
    {
        private Dictionary<string, SkillInformation> skillDataDict;
    }

    public partial class SkillData // Initialize
    {
        private void Allocate()
        {
            skillDataDict = new Dictionary<string, SkillInformation>();
        }
        public void Initialize()
        {
            Allocate();
            Setup();
        }
        private void Setup()
        {
            MainSystem.Instance.DataManager.SetUpData<SkillInformation>(skillDataDict, "SkillData");
        }
    }

    public partial class SkillData // Property
    {
        public SkillInformation GetData(string index)
        {
            return skillDataDict[index];
        }
    }
}
