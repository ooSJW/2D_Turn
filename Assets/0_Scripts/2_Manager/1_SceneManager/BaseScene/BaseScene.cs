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

    public partial class BaseScene : MonoBehaviour // Data Field
    {
        public List<GameObject> poolableObject = new List<GameObject>();

        [SerializeField] private GameObject playerGroup;
        [SerializeField] private GameObject enemyGroup;

        [field: SerializeField] public Player Player { get; private set; }
        [field: SerializeField] public BackGround[] BackGround { get; private set; }
        [field: SerializeField] public List<Transform> EnemyParentList { get; private set; }
        [field: SerializeField] public ItemController ItemController { get; private set; }
        [field: SerializeField] public UIController UIController { get; private set; }
        [field: SerializeField] public SoundController SoundController { get; private set; }
    }
    public partial class BaseScene : MonoBehaviour // Initialize
    {
        private void Allocate()
        {

        }
        public virtual void Initialize()
        {
            Allocate();
            Setup();
        }
        private void Setup()
        {
            MainSystem.Instance.StageManager.BackGroundList.Clear();

            for (int i = 0; i < BackGround.Length; i++)
            {
                MainSystem.Instance.StageManager.SignUpBackGround(BackGround[i]);
            }
            MainSystem.Instance.PoolManager.Register();

            MainSystem.Instance.StageManager.SetEnemyParent(EnemyParentList);
            MainSystem.Instance.StageManager.SetGroup(playerGroup, enemyGroup);

            MainSystem.Instance.PlayerManager.SignUpPlayer(Player);
            MainSystem.Instance.ItemManager.SignUpItemController(ItemController);

            MainSystem.Instance.DataManager.LoadStageData();
            MainSystem.Instance.DataManager.LoadPlayerData();
            MainSystem.Instance.DataManager.LoadItemData();

            MainSystem.Instance.InGameManager.CombatState = CombatState.GoNextStage;
            MainSystem.Instance.UIManager.SignUpUIController(UIController);
            MainSystem.Instance.SoundManager.SignUpSoundController(SoundController);
        }
    }
    public partial class BaseScene : MonoBehaviour // Main
    {
        private void Awake()
        {
            MainSystem.Instance.SceneManager.SignUpActiveScene(this);
        }
    }
}
