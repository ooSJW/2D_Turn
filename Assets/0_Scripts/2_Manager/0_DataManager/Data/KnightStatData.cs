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

    public partial class KnightStatData // Information
    {
        [System.Serializable]
        public class KnightStatInformation : CombatStatInformationBase
        {
            public int exp;
            public int[] max_exp;
            public string knight_icon;
        }
    }

    public partial class KnightStatData // Data Field
    {
        private Dictionary<string, KnightStatInformation> knightStatInfoDict = default;
    }

    public partial class KnightStatData // Initialize
    {
        private void Allocate()
        {
            knightStatInfoDict = new Dictionary<string, KnightStatInformation>();
        }
        public void Initialize()
        {
            Allocate();
            Setup();
        }
        private void Setup()
        {
            MainSystem.Instance.DataManager.SetUpData<KnightStatInformation>(knightStatInfoDict, "KnightStatData");
        }
    }

    public partial class KnightStatData // property
    {
        public KnightStatInformation GetData(string index)
        {
            return knightStatInfoDict[index];
        }
    }
}
