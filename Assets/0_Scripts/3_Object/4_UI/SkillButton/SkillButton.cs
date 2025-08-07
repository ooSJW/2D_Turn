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
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;
    using static project02.SkillData;

    public partial class SkillButton : MonoBehaviour // Data Property
    {
        private bool isCooltime = false;
        public bool IsCoolTime
        {
            get => isCooltime;
            set
            {
                isCooltime = value;
                if (IsCoolTime)
                {
                    skillButton.interactable = false;
                    hideImage.gameObject.SetActive(true);
                    IsSelected = false;
                    cooldownRoutine = StartCoroutine(CoolingSkill());
                }
                else
                {
                    skillButton.interactable = true;
                    hideImage.gameObject.SetActive(false);
                    if (cooldownRoutine is not null)
                        StopCoroutine(cooldownRoutine);

                    cooldownRoutine = null;
                }
            }
        }

        private bool isSelected = false;
        public bool IsSelected
        {
            get => isSelected;
            private set
            {
                if (isSelected != value)
                {
                    isSelected = value;
                    if (isSelected)
                    {
                        owner.selectedSkillList.Add(skillName);
                        SetSkillImageColor(0.5f);
                    }
                    else
                    {
                        if (owner.selectedSkillList.Contains(skillName))
                        {
                            owner.selectedSkillList.Remove(skillName);
                            SetSkillImageColor(1);
                        }
                    }

                }
            }
        }
    }

    public partial class SkillButton : MonoBehaviour // Data Field
    {
        [SerializeField] private Image skillIcon;
        [SerializeField] private Image hideImage;
        [SerializeField] private TextMeshProUGUI coolTimeText;
        [SerializeField] private Button skillButton;

        private const string Path_SkillIcon = "Skill/";
        private Knight owner;
        private SkillBase skillBase;
        private SkillName skillName;

        private float intervalTime;
        private float coolTime;
        private Coroutine cooldownRoutine;
    }
    public partial class SkillButton : MonoBehaviour // Initialize
    {
        private void Allocate()
        {
            skillName = Enum.Parse<SkillName>(skillBase.SkillInfo.index);
            hideImage.gameObject.SetActive(false);
            coolTimeText.text = string.Empty;

            skillIcon.sprite = Resources.Load<Sprite>(Path_SkillIcon + skillBase.SkillInfo.skill_icon);
            coolTime = skillBase.SkillInfo.cool_time;
        }
        public void Initialize(SkillBase skillBaseValue)
        {
            // 스킬 버튼UI 초기화 시 스킬 객체를 받고, 스킬 객체에게 자신을 전달해 상호작용 할 수 있도록 만듦
            // 스킬 객체의 정보를 통해 이미지, 쿨타임 등의 정보가 동적으로 정해짐.
            skillBase = skillBaseValue;
            skillBase.SkillButton = this;
            owner = MainSystem.Instance.PlayerManager.Player.
                activeKnightList.Find(elem => elem.KnightStatInformation.name == skillBase.SkillInfo.owner);
            Allocate();
            Setup();
        }
        private void Setup()
        {
            skillButton.interactable = true;
        }
    }

    public partial class SkillButton : MonoBehaviour // Main
    {
        //private void Update()
        //{
        //    if (IsCoolTime)
        //    {
        //        intervalTime += Time.deltaTime;
        //        if (intervalTime >= coolTime)
        //        {
        //            intervalTime = 0;
        //            IsCoolTime = false;
        //        }
        //        else
        //        {
        //            coolTimeText.text = (coolTime - intervalTime).ToString("F0");
        //            float time = intervalTime / coolTime;
        //            hideImage.fillAmount = 1 - time;
        //        }
        //    }
        //}
    }

    public partial class SkillButton : MonoBehaviour // Property
    {
        public void Selected()
        {
            IsSelected = !IsSelected;
        }

        private IEnumerator CoolingSkill()
        {
            intervalTime = 0;
            while (IsCoolTime)
            {
                intervalTime += Time.deltaTime;
                if (intervalTime >= coolTime)
                {
                    intervalTime = 0;
                    IsCoolTime = false;
                    yield break;
                }
                else
                {
                    coolTimeText.text = (coolTime - intervalTime).ToString("F0");
                    float fillAmount = intervalTime / coolTime;
                    hideImage.fillAmount = 1 - fillAmount;
                    yield return null;
                }
            }
        }
    }

    public partial class SkillButton : MonoBehaviour // Private Property
    {
        private void SetSkillImageColor(float alpha)
        {
            Color color = skillIcon.color;
            color.a = alpha;
            skillIcon.color = color;
        }
    }
}
