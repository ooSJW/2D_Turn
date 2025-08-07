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
                /*
                    owner 캐릭터가 스킬을 사용할 때 스킬 객체의 IsCoolDown = false로 변함.
                    스킬 객체의 IsCoolDown프로퍼티에서 해당 프로퍼티의 값을 바꾸는 방식
                    캐릭터 -> 스킬 객체 -> UI 순서로 메시지가 전달되는 방식으로 구현.
                */
                if (isCooltime)
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

                    // 코루틴 내부의 예기치 못한 무한루프를 방지하기 위해 한번 더 Stop하는 코드를 작성.
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
                        // 해당 스킬이 선택되었을 때 Owner객체의 공격, 공격 대기 등의 기능을 직접 호출하는 것이 아닌
                        // 행동 대기 list에 자신의 이름만을 추가.
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
            skillIcon.sprite = Resources.Load<Sprite>(Path_SkillIcon + skillBase.SkillInfo.skill_icon);
            coolTime = skillBase.SkillInfo.cool_time;

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
