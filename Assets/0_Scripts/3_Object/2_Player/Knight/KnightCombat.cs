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
    using UnityEngine;

    public partial class KnightCombat : MonoBehaviour // Data Field
    {
        private Knight knight;
    }
    public partial class KnightCombat : MonoBehaviour // Initialize
    {
        private void Allocate()
        {

        }
        public void Initialize(Knight knightValue)
        {
            knight = knightValue;

            Allocate();
            Setup();
        }
        private void Setup()
        {

        }
    }

    public partial class KnightCombat : MonoBehaviour // Property
    {
        public void Attack()
        {
            CombatObjectBase target = knight.Target.gameObject.GetComponent<CombatObjectBase>();
            knight.SendDamage(target, knight.KnightStatInformation);
            int randomAttackSound = UnityEngine.Random.Range((int)AudioClipName.Sfx_Attack01, (int)AudioClipName.Sfx_Attack02 + 1);
            AudioClipName clipName = (AudioClipName)randomAttackSound;

            MainSystem.Instance.SoundManager.SoundController.Sfx.PlaySfx(clipName);
        }

        public void UseSkill()
        {
            SkillBase skill = knight.KnightSkillDict[knight.selectedSkillList[0]];
            SkillType skillType = Enum.Parse<SkillType>(skill.SkillInfo.skill_type);
            AudioClipName clipName = Enum.Parse<AudioClipName>(skill.SkillInfo.skill_sound);
            switch (skillType)
            {
                case SkillType.Attack:
                    List<Enemy> enemyList = new List<Enemy>(MainSystem.Instance.EnemyManager.enemyList);

                    for (int i = 0; i < enemyList.Count; i++)
                    {
                        knight.SendDamage(enemyList[i], knight.KnightStatInformation, skill.SkillDamage);
                    }
                    MainSystem.Instance.PoolManager.Spawn(skill.SkillInfo.skill_effect, null, knight.Target.position);
                    break;

                case SkillType.Heal:
                    List<Knight> knightList = MainSystem.Instance.PlayerManager.Player.activeKnightList;
                    for (int i = 0; i < knightList.Count; i++)
                    {
                        if (knightList[i].KnightState != KnightState.Death)
                        {
                            knight.Heal(knightList[i], skill.SkillDamage);
                            MainSystem.Instance.PoolManager.Spawn(skill.SkillInfo.skill_effect, knightList[i].transform);
                        }
                    }
                    break;
            }
            MainSystem.Instance.SoundManager.SoundController.Sfx.PlaySfx(clipName);
        }
    }
}