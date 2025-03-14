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
    using System.Runtime.CompilerServices;
    using Unity.VisualScripting;
    using UnityEngine;
    using static project02.KnightStatData;

    public partial class Knight : CombatObjectBase // Data Property
    {
        private KnightStatInformation knightStatInformation;
        public KnightStatInformation KnightStatInformation
        {
            get => knightStatInformation;
            private set
            {
                knightStatInformation = new KnightStatInformation()
                {
                    index = value.index,
                    name = value.name,
                    level = value.level,
                    exp = value.exp,
                    max_hp = value.max_hp,
                    attack = value.attack,
                    defence = value.defence,
                    useable_skill = value.useable_skill,
                    critical_percent = value.critical_percent,
                    critical_increase = value.critical_increase,
                    knight_icon = value.knight_icon,
                    max_exp = value.max_exp,
                    max_level = value.max_level,
                    move_speed = value.move_speed,
                    attack_range = value.attack_range,
                };
                MaxHp = value.max_hp[CurrentIndex];
                Hp = value.max_hp[CurrentIndex];
                CalculateDamage(this);
                CalculateItemDamage();
            }
        }

        private int exp;
        public int Exp
        {
            get => exp;
            set
            {
                if (level < knightStatInformation.max_level)
                {
                    exp = value;
                    while (exp >= knightStatInformation.max_exp[CurrentIndex])
                    {
                        exp -= knightStatInformation.max_exp[CurrentIndex];
                        Level++;
                    }
                }
            }
        }

        public override int Hp
        {
            get => hp;
            set
            {
                if (hp != value)
                {
                    float hpTemp = hp;
                    hp = value;
                    if (hpTemp > hp)
                    {
                        if (hp > 0)
                            KnightState = KnightState.GetDamage;
                        else
                            KnightState = KnightState.Death;
                    }
                    infoUI.SetHpUI(hp, MaxHp);
                    infoUI.SetHpScale(MaxHp);
                }
            }
        }
        private KnightState knightState = KnightState.Idle;
        public KnightState KnightState
        {
            get => knightState;
            set
            {
                if (knightState != value)
                {
                    knightState = value;
                    switch (knightState)
                    {
                        case KnightState.Idle:
                            KnightAnimation.FlipX(false);
                            break;

                        case KnightState.Return:
                            KnightAnimation.FlipX(true);
                            break;
                        case KnightState.Death:
                            MainSystem.Instance.StageManager.Score--;
                            break;
                    }
                }
            }
        }

        public override int Level
        {
            get => level;
            set
            {
                if (value > 0 && value <= knightStatInformation.max_level)
                {
                    level = value;
                    CurrentIndex = level - 1;
                    infoUI.SetLevelText(level);
                    RefreshStat();
                }
            }
        }

        /*
        private Transform target = null;
        public Transform Target
        {
            get => target;
            set
            {
                if (target != value)
                {
                    if (value == null)
                    {
                        KnightState = KnightState.Return;
                        //KnightAnimation.UnFocusObject();
                    }
                    else
                    {
                        KnightState = KnightState.GoForward;
                        //KnightAnimation.FocusObject();
                    }
                    target = value;
                }
            }}

        */
        private Item weapon = null;
        public Item Weapon { get => weapon; set => weapon = value; }
        private Item armor = null;
        public Item Armor { get => armor; set => armor = value; }
    }
    public partial class Knight : CombatObjectBase // Data Field
    {
        public KnightName KnightName { get; private set; }

        public List<SkillName> selectedSkillList;

        public Dictionary<SkillName, SkillBase> KnightSkillDict { get; private set; }
        [field: SerializeField] public KnightMovement KnightMovement { get; private set; }
        [field: SerializeField] public KnightCombat KnightCombat { get; private set; }
        [field: SerializeField] public KnightAnimation KnightAnimation { get; private set; }
        public bool IsEquip = false;
    }
    public partial class Knight : CombatObjectBase // Initialize
    {
        private void Allocate()
        {
            selectedSkillList = new List<SkillName>();
            KnightName = Enum.Parse<KnightName>(gameObject.name);
            int index = (int)KnightName;
            KnightSkillDict = new Dictionary<SkillName, SkillBase>();
            KnightStatInformation = MainSystem.Instance.DataManager.KnightStatData.GetData(index.ToString());
            Level = 1;
        }
        public override void Initialize()
        {
            base.Initialize();

            Allocate();
            Setup();
            KnightMovement.Initialize(this);
            KnightCombat.Initialize(this);
            KnightAnimation.Initialize(this);
            SkillInitialize();
            InfoUIInitialize();
        }
        private void Setup()
        {

        }
    }

    public partial class Knight : CombatObjectBase // Private Property
    {
        private void CalculateItemDamage()
        {
            Attack = KnightStatInformation.attack[CurrentIndex];
            Defence = KnightStatInformation.defence[CurrentIndex];
            if (Weapon != null)
                Attack += weapon.Power;
            if (Armor != null)
                Defence += armor.Power;
        }
        private void SkillInitialize()
        {
            Type type = typeof(Knight);
            string nameSpace = type.Namespace;
            for (int i = 0; i < KnightStatInformation.useable_skill.Length; i++)
            {
                string skillName = KnightStatInformation.useable_skill[i];
                Type skill = Type.GetType($"{nameSpace}.{skillName}");

                if (skill != null)
                {
                    SkillBase skillBase = gameObject.AddComponent(skill) as SkillBase;
                    if (skillBase != null)
                    {
                        skillBase.Initialize(this);
                        KnightSkillDict.Add(skillBase.GetSkillName(), skillBase);
                    }
                }
            }
        }
        public void Despawn()
        {
            MainSystem.Instance.PoolManager.Despawn(gameObject);
        }

        public void RefreshStat()
        {
            Hp = knightStatInformation.max_hp[CurrentIndex];
            MaxHp = knightStatInformation.max_hp[CurrentIndex];
            CalculateDamage(this);
            CalculateItemDamage();
        }
    }

    public partial class Knight : CombatObjectBase // Main
    {
        private void Update()
        {
            SortingOrder();
            KnightMovement.Progress();
            KnightAnimation.Progress();
        }
    }

}
