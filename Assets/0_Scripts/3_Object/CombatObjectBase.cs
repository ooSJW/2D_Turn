/*
	* Coder :
	* Last Update :
	* Information
*/
namespace project02
{
    using DG.Tweening;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.InteropServices.WindowsRuntime;
    using UnityEngine;

    public partial class CombatObjectBase : MonoBehaviour // Data Field
    {
        protected int level;
        public virtual int Level { get => level; set => level = value; }

        protected int hp;
        public virtual int Hp { get => hp; set => hp = value; }

        protected int maxHp;
        public virtual int MaxHp { get => maxHp; set => maxHp = value; }

        protected int attack;
        public virtual int Attack { get => attack; set => attack = value; }

        protected int defence;
        public int Defence { get => defence; set => defence = value; }

        protected int currentIndex = 0;
        public int CurrentIndex { get => currentIndex; set => currentIndex = value >= 0 ? value : 0; }

        protected bool isMyturn = false;
        public bool IsMyTurn
        {
            get => isMyturn;
            set
            {
                if (isMyturn != value)
                {
                    isMyturn = value;
                    if (isMyturn)
                        OriginPos = transform.position;
                }
            }
        }
        [SerializeField] protected Transform damageTextTransform;


        public Vector2 OriginPos { get; private set; } = Vector2.zero;

        protected float totalDamage;

        protected Transform target;
        public Transform Target { get => target; set => target = value; }

        [SerializeField] protected InfoUI infoUI;
        private SpriteRenderer spriteRenderer;
    }

    public partial class CombatObjectBase : MonoBehaviour // Initialize
    {
        private void Allocate()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        public virtual void Initialize()
        {
            Allocate();
            Setup();
        }
        private void Setup()
        {

        }
    }

    public partial class CombatObjectBase : MonoBehaviour // Property
    {
        protected void InfoUIInitialize()
        {
            infoUI.Initialize();
            infoUI.SetLevelText(Level);
            infoUI.SetHpUI(Hp, MaxHp);
            infoUI.SetHpScale(MaxHp);
        }

        protected void SortingOrder()
        {
            if (spriteRenderer != null)
                spriteRenderer.sortingOrder = Mathf.RoundToInt(transform.position.y * -1);
        }

        public virtual void CalculateDamage(CombatObjectBase combatObjectBase)
        {
            totalDamage = combatObjectBase.Attack;
        }

        public void Active(Transform targetValue)
        {
            IsMyTurn = true;
            target = targetValue;
        }

        public virtual void SendDamage(CombatObjectBase target, CombatStatInformationBase sender, float skillDamage = 0)
        {
            float resultDamage = totalDamage;
            bool isCritical = false;

            if (UnityEngine.Random.Range(0f, 1f) <= sender.critical_percent)
            {
                resultDamage += resultDamage * sender.critical_increase;
                isCritical = true;
            }

            resultDamage += resultDamage * skillDamage;

            target.ReceiveDamage((int)resultDamage, isCritical);
        }

        public virtual void Heal(CombatObjectBase target, float skillDamage)
        {
            float heal = totalDamage;
            heal += heal * skillDamage;
            int healingAmount = 0;
            if (target.Hp + heal > target.MaxHp)
            {
                healingAmount = target.MaxHp - target.Hp;
                target.Hp = target.MaxHp;
            }
            else
            {
                target.Hp += (int)heal;
                healingAmount = (int)heal;
            }
            target.SetHpText(healingAmount, false);
        }

        public virtual void ReceiveDamage(int damage, bool isCritical = false)
        {
            if (Hp <= 0)
                return;

            float modifier = Mathf.Max(1 - (0.01f * Defence), 0.1f);
            int actualDamage = Mathf.Max((int)((damage - Defence) * modifier), 0);
            // 5 == minDamage
            int finalDamage = Mathf.Max(actualDamage, 5);

            Hp -= finalDamage;

            SetHpText(finalDamage, true, isCritical);
        }
        public void SetHpText(int amount, bool isAttack, bool isCritical = false)
        {
            HpParticle hpParticle =
                MainSystem.Instance.PoolManager.Spawn(PoolObject.HpParticle.ToString(),
                damageTextTransform,
                damageTextTransform.position)
               .GetComponent<HpParticle>();
            hpParticle.Initialize();
            hpParticle.SetHpText(amount, isAttack, isCritical);
        }
    }

}
