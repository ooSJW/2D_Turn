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
    using static project02.KnightStatData;

    public partial class EnemyStatData  // Information
    {
        [System.Serializable]
        public class EnemyStatInformation : CombatStatInformationBase
        {

        }

    }
    public partial class EnemyStatData  // Data Field
    {
        private Dictionary<string, EnemyStatInformation> enemyStatInfoDict = default;
    }

    public partial class EnemyStatData  // Initialize
    {
        private void Allocate()
        {
            enemyStatInfoDict = new Dictionary<string, EnemyStatInformation>();
        }
        public void Initialize()
        {
            Allocate();
            Setup();
        }
        private void Setup()
        {
            MainSystem.Instance.DataManager.SetUpData<EnemyStatInformation>(enemyStatInfoDict, "EnemyStatData");
        }
    }

    public partial class EnemyStatData // Property
    {
        public EnemyStatInformation GetData(string index)
        {
            return enemyStatInfoDict[index];
        }
    }
}
