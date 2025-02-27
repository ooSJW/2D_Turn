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
                    IsSetected = false;
                }
                else
                {
                    skillButton.interactable = true;
                    hideImage.gameObject.SetActive(false);
                }
            }
        }

        private bool isSetected = false;
        public bool IsSetected
        {
            get => isSetected;
            private set
            {
                if (isSetected != value)
                {
                    isSetected = value;
                    if (isSetected)
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

        private Knight owner;
        private SkillBase skillBase;
        private SkillName skillName;

        private float intervalTime;
        private float coolTime;
    }
    public partial class SkillButton : MonoBehaviour // Initialize
    {
        private void Allocate()
        {
            skillName = Enum.Parse<SkillName>(skillBase.SkillInfo.index);
            hideImage.gameObject.SetActive(false);
            coolTimeText.text = string.Empty;

            skillIcon.sprite = Resources.Load<Sprite>("Skill/" + skillBase.SkillInfo.skill_icon);
            coolTime = skillBase.SkillInfo.cool_time;
        }
        public void Initialize(SkillBase skillBaseValue)
        {
            skillBase = skillBaseValue;
            skillBase.SkillButton = this;
            owner = MainSystem.Instance.PlayerManager.Player.activeKnightList.Find(elem => elem.KnightStatInformation.name == skillBase.SkillInfo.owner);
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
        private void Update()
        {
            if (IsCoolTime)
            {
                intervalTime += Time.deltaTime;
                if (intervalTime >= coolTime)
                {
                    intervalTime = 0;
                    IsCoolTime = false;
                }
                else
                {
                    coolTimeText.text = (coolTime - intervalTime).ToString("F0");
                    float time = intervalTime / coolTime;
                    hideImage.fillAmount = 1 - time;
                }
            }
        }
    }

    public partial class SkillButton : MonoBehaviour // Property
    {
        public void Selected()
        {
            IsSetected = !IsSetected;
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
