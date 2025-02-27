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
    using static project02.SkillData;

    public partial class SkillBase : MonoBehaviour // Data Field
    {
        private bool isCoolTime = false;
        public bool IsCoolTime
        {
            get => isCoolTime;
            set
            {
                isCoolTime = value;
                SkillButton.IsCoolTime = isCoolTime;
            }
        }
    }


    public partial class SkillBase : MonoBehaviour // Data Field
    {
        protected SkillName skillName;

        protected CombatObjectBase owner;

        protected SkillInformation skillInfo = default;
        public SkillInformation SkillInfo { get => skillInfo; set => skillInfo = value; }

        public SkillButton SkillButton { get; set; }

        public float SkillDamage { get; private set; }
    }
    public partial class SkillBase : MonoBehaviour // Intialize
    {
        private void Allocate()
        {
            skillName = Enum.Parse<SkillName>(GetType().Name);
            SkillInformation originInfo = MainSystem.Instance.DataManager.SkillData.GetData(skillName.ToString());
            SkillInfo = new SkillInformation()
            {
                index = originInfo.index,
                power = originInfo.power,
                power_growth = originInfo.power_growth,
                cool_time = originInfo.cool_time,
                range = originInfo.range,
                level = originInfo.level,
                max_level = originInfo.max_level,
                skill_type = originInfo.skill_type,
                skill_effect = originInfo.skill_effect,
                skill_icon = originInfo.skill_icon,
                skill_sound= originInfo.skill_sound,
                owner = originInfo.owner,
            };
            CalculateDamage(SkillInfo);
        }
        public virtual void Initialize(CombatObjectBase combatObjectBase)
        {
            owner = combatObjectBase;
            Allocate();
            Setup();
        }
        private void Setup()
        {

        }
    }

    public partial class SkillBase : MonoBehaviour //Property
    {
        public SkillName GetSkillName() { return skillName; }
        public void CalculateDamage(SkillInformation skillInfo)
        {
            SkillDamage = skillInfo.power_growth * skillInfo.level + skillInfo.power;
        }

    }
}
