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
    using static project02.EnemyStatData;
    using static project02.KnightStatData;

    public partial class Enemy : CombatObjectBase // Data Property
    {
        private EnemyStatInformation enemyStatInformation;
        public EnemyStatInformation EnemyStatInformation
        {
            get => enemyStatInformation;
            set
            {
                enemyStatInformation = new EnemyStatInformation()
                {
                    index = value.index,
                    name = value.name,
                    level = value.level,
                    max_hp = value.max_hp,
                    attack = value.attack,
                    defence = value.defence,
                    useable_skill = value.useable_skill,
                    critical_percent = value.critical_percent,
                    critical_increase = value.critical_increase,
                    max_level = value.max_level,
                    move_speed = value.move_speed,
                    attack_range = value.attack_range,
                };
                MaxHp = value.max_hp[CurrentIndex];
                Hp = value.max_hp[CurrentIndex];
                Attack = value.attack[CurrentIndex];
                Defence = value.defence[CurrentIndex];
                CalculateDamage(this);
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
                            EnemyState = EnemyState.GetDamage;
                        else
                            EnemyState = EnemyState.Death;
                    }
                    infoUI.SetHpUI(hp, MaxHp);
                    infoUI.SetHpScale(MaxHp);
                }
            }
        }

        private EnemyState enemyState = EnemyState.Idle;
        public EnemyState EnemyState
        {
            get => enemyState;
            set
            {
                if (enemyState != value)
                {
                    enemyState = value;

                    switch (enemyState)
                    {
                        case EnemyState.Idle:
                            EnemyAnimation.FlipX(true);
                            break;
                        case EnemyState.Return:
                            EnemyAnimation.FlipX(false);
                            break;
                        case EnemyState.Death:
                            Death();
                            break;
                    }
                }
            }
        }
    }

    public partial class Enemy : CombatObjectBase // Data Field
    {
        private EnemyName enemyName;

        [field: SerializeField] public EnemyMovement EnemyMovement { get; private set; } = default;
        [field: SerializeField] public EnemyCombat EnemyCombat { get; private set; } = default;
        [field: SerializeField] public EnemyAnimation EnemyAnimation { get; private set; } = default;
    }

    public partial class Enemy : CombatObjectBase // Initialize
    {
        private void Allocate()
        {
            EnemyState = EnemyState.Idle;
            enemyName = Enum.Parse<EnemyName>(gameObject.name);
            int index = (int)enemyName;
            int average = 0;
            if (MainSystem.Instance.PlayerManager.Player != null)
                average = MainSystem.Instance.PlayerManager.Player.GetAverage();

            CurrentIndex = average - 2 > 0 ? average - 2 : 0;
            EnemyStatInformation = MainSystem.Instance.DataManager.EnemyStatData.GetData(index.ToString());
            level = CurrentIndex + 1;
        }

        public override void Initialize()
        {
            base.Initialize();

            Allocate();
            Setup();
            EnemyMovement.Initialize(this);
            EnemyCombat.Initialize(this);
            EnemyAnimation.Initialize(this);
            InfoUIInitialize();
        }

        private void Setup()
        {

        }
    }

    public partial class Enemy : CombatObjectBase // Main
    {
        private void Update()
        {
            SortingOrder();
            EnemyMovement.Progress();
            EnemyAnimation.Progress();
        }
    }

    public partial class Enemy : CombatObjectBase // Property
    {
        public void Death()
        {
            MainSystem.Instance.EnemyManager.SignDownEnemy(this);
        }
        public void Despawn()
        {
            MainSystem.Instance.PoolManager.Despawn(gameObject);
        }
    }
}
