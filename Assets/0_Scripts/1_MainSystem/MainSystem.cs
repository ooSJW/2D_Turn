/*
	* Coder :
	* Last Update :
	* Information
*/
namespace project02
{
    using System.Collections;
    using System.Collections.Generic;
    using Unity.VisualScripting;
    using UnityEngine;

    public partial class MainSystem : GenericSingleton<MainSystem> // Data Field
    {
        public DataManager DataManager { get; private set; } = default;
        public PoolManager PoolManager { get; private set; } = default;
        public SceneManager SceneManager { get; private set; } = default;
        public InGameManager InGameManager { get; private set; } = default;
        public StageManager StageManager { get; private set; } = default;
        public ItemManager ItemManager { get; private set; } = default;
        public PlayerManager PlayerManager { get; private set; } = default;
        public EnemyManager EnemyManager { get; private set; } = default;
        public UIManager UIManager { get; private set; } = default;
        public SoundManager SoundManager { get; private set; } = default;
    }

    public partial class MainSystem : GenericSingleton<MainSystem> // Initialize
    {
        private void Allocate()
        {
            DataManager = gameObject.AddComponent<DataManager>();
            PoolManager = gameObject.AddComponent<PoolManager>();
            SceneManager = gameObject.AddComponent<SceneManager>();
            InGameManager = gameObject.AddComponent<InGameManager>();
            StageManager = gameObject.AddComponent<StageManager>();
            ItemManager = gameObject.AddComponent<ItemManager>();
            PlayerManager = gameObject.AddComponent<PlayerManager>();
            EnemyManager = gameObject.AddComponent<EnemyManager>();
            UIManager = gameObject.AddComponent<UIManager>();
            SoundManager = gameObject.AddComponent<SoundManager>();
        }
        public void Initialize()
        {
            Allocate();
            Setup();

            DataManager.Initialize();
            PoolManager.Initialize();
            SceneManager.Initialize();
            InGameManager.Initialize();
            StageManager.Initialize();
            ItemManager.Initialize();
            PlayerManager.Initialize();
            EnemyManager.Initialize();
            UIManager.Initialize();
            SoundManager.Initialize();

            SetGraphicQuality();
        }
        private void Setup()
        {

        }
    }

    public partial class MainSystem : GenericSingleton<MainSystem> // Property
    {
        public void MainSystemStart()
        {
            Initialize();
            SceneManager.LoadScene(SceneName.MainLobbyScene.ToString());
        }
        private void SetGraphicQuality()
        {
            string graphicQuality = PlayerPrefs.GetString("GraphicQuality");

            if (int.TryParse(graphicQuality, out int qualityLevel))
                QualitySettings.SetQualityLevel(qualityLevel);
            else
                QualitySettings.SetQualityLevel(5);
        }
    }
}
