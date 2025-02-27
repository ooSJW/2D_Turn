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
    using static project02.StageData;

    public partial class MenuUI : MonoBehaviour
    {
        [field: SerializeField] public ClearUI ClearUI { get; private set; }

        [field: SerializeField] public GameObject FailUI { get; private set; }
        [field: SerializeField] public GameObject SkillGroup { get; private set; }
        [field: SerializeField] public GameObject PauseBtn { get; private set; }
        [field: SerializeField] public GameObject PauseUI { get; private set; }
        [field: SerializeField] public GameObject OptionUI { get; private set; }
        [field: SerializeField] public GameObject InitializeUI { get; private set; }
        [field: SerializeField] public GameObject QuitMessage { get; private set; }
        [field: SerializeField] public Button NextStageBtn { get; private set; }
        [field: SerializeField] public TextMeshProUGUI[] CoinText { get; private set; }

        [SerializeField] private TextMeshProUGUI stageText;
        [SerializeField] private Button[] graphicButtons;
    }
    public partial class MenuUI : MonoBehaviour
    {
        private void Allocate()
        {

        }
        public void Initialize()
        {
            Allocate();
            Setup();

            if (MainSystem.Instance.SceneManager.ActiveScene.name != SceneName.MainLobbyScene.ToString())
            {
                SkillBtnSpawn();
                ClearUI.Initialize();

                FailUI.gameObject.SetActive(false);
                PauseBtn.SetActive(true);
                PauseUI.SetActive(false);
                ClearUI.gameObject.SetActive(false);
                OptionUI.gameObject.SetActive(false);
                SetStageText();
            }
            else
            {
                for (int i = 0; i < CoinText.Length; i++)
                    CoinText[i].text = MainSystem.Instance.PlayerManager.Player.Coin.ToString();
                QuitMessage.SetActive(false);
            }
        }
        private void Setup()
        {

        }
    }

    public partial class MenuUI : MonoBehaviour // Private Property
    {
        private void SkillBtnSpawn()
        {
            for (int i = 0; i < MainSystem.Instance.PlayerManager.Player.activeKnightList.Count; i++)
            {
                GameObject skillBtnRoot = MainSystem.Instance.PoolManager.Spawn("SkillButtonBase", SkillGroup.transform);
                string[] useableSkill = MainSystem.Instance.PlayerManager.Player.activeKnightList[i].KnightStatInformation.useable_skill;
                for (int j = 0; j < useableSkill.Length; j++)
                {
                    SkillName skillName = Enum.Parse<SkillName>(useableSkill[j]);
                    SkillButton skillButton = MainSystem.Instance.PoolManager.Spawn("SkillButton", skillBtnRoot.transform).GetComponent<SkillButton>();
                    skillButton.Initialize(MainSystem.Instance.PlayerManager.Player.activeKnightList[i].KnightSkillDict[skillName]);
                }
            }
        }
    }

    public partial class MenuUI : MonoBehaviour
    {
        public void RefreshCoinText(int coin)
        {
            for (int i = 0; i < CoinText.Length; i++)
            {
                if (CoinText[i] != null)
                    CoinText[i].text = coin.ToString();
            }
        }
        public void ActiveOptionUI()
        {
            bool isActive = !OptionUI.gameObject.activeSelf;
            if (isActive)
                SetGraphicInteractable();

            OptionUI.gameObject.SetActive(isActive);
        }
        public void OnOffInitializeUI()
        {
            bool isActive = InitializeUI.activeSelf;
            InitializeUI.SetActive(!isActive);
        }
        public void InitializeGameData()
        {
            MainSystem.Instance.DataManager.ClearData();

        }
        public void SetInteractableNextStageBtn()
        {
            string index = MainSystem.Instance.StageManager.CurrentStage.ToString();
            if (MainSystem.Instance.DataManager.StageData.ContainsKey(index))
                NextStageBtn.interactable = true;
            else
                NextStageBtn.interactable = false;
        }

        public void NextStageBtnClick()
        {
            MainSystem.Instance.SceneManager.LoadScene(SceneName.CombatScene.ToString());
        }

        public void RetryBtnClick()
        {
            MainSystem.Instance.InGameManager.CombatState = CombatState.None;
            MainSystem.Instance.EnemyManager.enemyList.Clear();
            MainSystem.Instance.StageManager.MoveStage = false;
            MainSystem.Instance.StageManager.CurrentStage = int.Parse(MainSystem.Instance.StageManager.StageInfo.start_stage_index);
            MainSystem.Instance.SceneManager.LoadScene(SceneName.CombatScene.ToString());
        }
        public void SetStageText()
        {
            StageInformation stageInfo = MainSystem.Instance.StageManager.StageInfo;
            if (stageInfo != null)
            {
                string stage = stageInfo.chapter;
                string stageName = stageInfo.stage_name;
                stageText.text = stage + " " + stageName;
            }
        }

        public void PlayerLose()
        {
            FailUI.gameObject.SetActive(true);
        }

        public void PauseGame(bool isClear = false)
        {
            if (isClear)
            {
                SetInteractableNextStageBtn();
                ClearUI.gameObject.SetActive(true);
            }
            else
            {
                Time.timeScale = 0;
                PauseUI.SetActive(true);
                MainSystem.Instance.SoundManager.SoundController.Bgm.StopBgm();
            }
        }

        public void SetGraphicInteractable()
        {
            for (int i = 0; i < graphicButtons.Length; i++)
                graphicButtons[i].interactable = true;

            switch (QualitySettings.GetQualityLevel())
            {
                case 1:
                    graphicButtons[(int)GraphicQuality.Low].interactable = false;
                    break;
                case 3:
                    graphicButtons[(int)GraphicQuality.Medium].interactable = false;
                    break;
                case 5:
                    graphicButtons[(int)GraphicQuality.High].interactable = false;
                    break;
            }
        }

        public void SetGraphicQuality(int index)
        {
            PlayerPrefs.SetString("GraphicQuality", index.ToString());
            QualitySettings.SetQualityLevel(index, true);
            SetGraphicInteractable();
        }

        public void ResumeGame()
        {
            PauseUI.SetActive(false);
            Time.timeScale = 1;
            MainSystem.Instance.SoundManager.SoundController.Bgm.ResumeBgm();
        }

        public void LoadMainLobby()
        {
            MainSystem.Instance.InGameManager.CombatState = CombatState.None;
            MainSystem.Instance.EnemyManager.enemyList.Clear();
            MainSystem.Instance.StageManager.MoveStage = false;
            Time.timeScale = 1;
            MainSystem.Instance.SceneManager.LoadScene(SceneName.MainLobbyScene.ToString());
        }
        public void LoadCombatScene()
        {
            MainSystem.Instance.SceneManager.LoadScene(SceneName.CombatScene.ToString());
        }

        public void OnClickQuit()
        {
            bool isActive = QuitMessage.activeSelf;
            QuitMessage.SetActive(!isActive);
        }
        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
