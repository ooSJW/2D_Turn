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
    using TMPro;
    using UnityEngine.UI;
    using static project02.KnightStatData;
    using System;
    using static UnityEngine.Rendering.VolumeComponent;

    public partial class KnightIcon : MonoBehaviour // Data Field
    {
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private Button knightButton;
        [SerializeField] private RectTransform rectTransform;

        public KnightName KnightName { get; private set; }
        public Knight Knight { get; private set; }
        public Vector2 originSize = Vector2.zero;
    }
    public partial class KnightIcon : MonoBehaviour // Initialize
    {
        private void Allocate()
        {
            knightButton = GetComponent<Button>();
            originSize = rectTransform.sizeDelta;
        }
        public void Initialize(string knightName)
        {
            KnightName = Enum.Parse<KnightName>(knightName);
            Knight = MainSystem.Instance.PlayerManager.Player.activeKnightList.Find(elem => elem.name == KnightName.ToString());
            Allocate();
            Setup();
            SetInformation();
        }
        private void Setup()
        {

        }
    }

    public partial class KnightIcon : MonoBehaviour // Property
    {
        public void EquipKnight(int index)
        {
            Knight = MainSystem.Instance.PlayerManager.Player.SpawnKnight(KnightName.ToString(), index);
            Knight.IsEquip = true;
        }

        public void UnEquipKnight()
        {
            MainSystem.Instance.PlayerManager.Player.DeSpawnKnight(Knight.name);
            Knight.IsEquip = false;
            Knight = null;
        }
    }

    public partial class KnightIcon : MonoBehaviour // Private Property
    {
        private void SetInformation()
        {
            int index = (int)KnightName;
            string path = MainSystem.Instance.DataManager.KnightStatData.GetData(index.ToString()).knight_icon;
            icon.sprite = Resources.Load<Sprite>("Knight/" + path);
            nameText.text = KnightName.ToString();

            knightButton.onClick.AddListener(() => KnightIconBtnClick());
            knightButton.onClick.AddListener(() => OnOffKnightInfoUI());
            knightButton.onClick.AddListener(() => PlayClickSound());
        }

        private void KnightIconBtnClick()
        {
            if (Knight == null)
                Knight = MainSystem.Instance.PlayerManager.Player.SpawnKnight(KnightName.ToString());
            
            BlinkImage blinkImage = MainSystem.Instance.UIManager.UIController.KnightManagementUI.BlinkImage;
            if (blinkImage != null)
                blinkImage.Initialize();
        }
        public void KnightIconQuitBtnClick()
        {
            if (!MainSystem.Instance.PlayerManager.Player.activeKnightList.Find(elem => elem.name == Knight.name))
            {
                MainSystem.Instance.PoolManager.Despawn(Knight.gameObject);
                Knight = null;
            }
        }

        private void OnOffKnightInfoUI()
        {
            MainSystem.Instance.UIManager.UIController.KnightManagementUI.OnOffKnightInfoUI(this);
        }
        private void PlayClickSound()
        {
            MainSystem.Instance.SoundManager.SoundController.Sfx.PlayButtonClickSound();
        }
    }
}
