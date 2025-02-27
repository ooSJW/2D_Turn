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

    public partial class EnemyCombat : MonoBehaviour // Data Field
    {
        private Enemy enemy;
    }
    public partial class EnemyCombat : MonoBehaviour // Initialize
    {
        private void Allocate()
        {

        }
        public void Initialize(Enemy enemyValue)
        {
            enemy = enemyValue;

            Allocate();
            Setup();
        }
        private void Setup()
        {

        }
    }

    public partial class EnemyCombat : MonoBehaviour // Property
    {
        public void Attack()
        {
            CombatObjectBase target = enemy.Target.gameObject.GetComponent<CombatObjectBase>();
            enemy.SendDamage(target, enemy.EnemyStatInformation);
            int randomAttackSound = UnityEngine.Random.Range((int)AudioClipName.Sfx_Attack01, (int)AudioClipName.Sfx_Attack02 + 1);
            AudioClipName clipName = (AudioClipName)randomAttackSound;

            MainSystem.Instance.SoundManager.SoundController.Sfx.PlaySfx(clipName);
        }
    }
}
