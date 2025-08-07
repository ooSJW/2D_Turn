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

    public partial class EnemyManager : MonoBehaviour // Data Field
    {
        public List<Enemy> enemyList;
    }
    public partial class EnemyManager : MonoBehaviour // Initialize
    {
        private void Allocate()
        {
            enemyList = new List<Enemy>();
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
    public partial class EnemyManager : MonoBehaviour // Sign
    {
        public void SignUpEnemy(Enemy enemyValue)
        {
            enemyList.Add(enemyValue);
            enemyValue.Initialize();
        }
        public void SignOutEnemy(Enemy enemyValue)
        {
            enemyList.Remove(enemyValue);
        }
    }
}
