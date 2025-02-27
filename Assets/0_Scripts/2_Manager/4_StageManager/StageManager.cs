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
    using System.Linq;
    using Unity.VisualScripting;
    using UnityEngine;
    using static project02.StageData;

    public partial class StageManager : MonoBehaviour // Data Property
    {
        private bool moveStage = false;
        public bool MoveStage
        {
            get => moveStage;
            set
            {
                if (moveStage != value)
                {
                    moveStage = value;
                    for (int i = 0; i < BackGroundList.Count; i++)
                    {
                        BackGroundList[i].MoveBackground = value;
                    }
                    if (moveStage)
                    {
                        StageInfo = MainSystem.Instance.DataManager.StageData.GetData(currentStage.ToString());

                        for (int i = 0; i < stageInfo.spawnable_enemy.Length; i++)
                        {
                            for (int j = 0; j < stageInfo.spawn_count[i]; j++)
                            {
                                Transform parent = GetEnemyParent(i);
                                Enemy enemy =
                                MainSystem.Instance.PoolManager.Spawn(stageInfo.spawnable_enemy[i], parent, parent.position).GetComponent<Enemy>();
                                MainSystem.Instance.EnemyManager.SignUpEnemy(enemy);
                            }
                        }

                        Transform bg = BackGroundList.Find(elem => elem.gameObject.name == "Tile").GetLastBackGround();
                        enemyGroup.transform.SetParent(bg, true);
                        enemyGroup.transform.position = bg.position;
                    }
                    else
                    {
                        MainSystem.Instance.InGameManager.CombatState = CombatState.Start;
                    }
                }
            }
        }

        private StageInformation stageInfo;
        public StageInformation StageInfo
        {
            get => stageInfo;
            set
            {
                stageInfo = new StageInformation
                {
                    index = value.index,
                    start_stage_index = value.start_stage_index,
                    chapter = value.chapter,
                    stage = value.stage,
                    stage_name = value.stage_name,
                    stage_score = value.stage_score,
                    spawnable_enemy = value.spawnable_enemy,
                    spawn_count = value.spawn_count,
                    enemy_type = value.enemy_type,
                    reward_type = value.reward_type,
                    reward_exp = value.reward_exp,
                    reward_gold = value.reward_gold,
                    knight_name = value.knight_name,
                    reward_item_count = value.reward_item_count,
                    is_last_stage = value.is_last_stage,
                    is_clear = value.is_clear,
                };
            }
        }

        private int currentStage = 0;
        public int CurrentStage
        {
            get => currentStage;
            set { currentStage = value; }
        }

    }
    public partial class StageManager : MonoBehaviour // Data Field
    {
        [SerializeField] private List<Transform> enemyParentList;
        [SerializeField] private LayerMask enemyLayer;
        public List<BackGround> BackGroundList { get; private set; } = default;
        public GameObject playerGroup;
        public GameObject enemyGroup;
        public int Score { get; set; } = 0;
        public List<int> ScoreList { get; private set; }
    }

    public partial class StageManager : MonoBehaviour // Initialize 
    {
        private void Allocate()
        {
            BackGroundList = new List<BackGround>();
            StageInfo = MainSystem.Instance.DataManager.StageData.GetData(currentStage.ToString());
            enemyLayer = 1 << LayerMask.NameToLayer("Enemy");
            ScoreList = new List<int>();
        }
        public void Initialize()
        {
            Allocate();
            Setup();
        }
        private void Setup()
        {

        }
    }

    public partial class StageManager : MonoBehaviour // Main 
    {
        private void Update()
        {
            if (MoveStage)
            {
                if (Vector3.Distance(playerGroup.transform.position, enemyGroup.transform.position) <= 20)
                    MoveStage = false;
            }
        }


    }

    public partial class StageManager : MonoBehaviour // Property 
    {
        public void SetGroup(GameObject player, GameObject enemy)
        {
            playerGroup = player;
            enemyGroup = enemy;
        }

        public void SetEnemyParent(List<Transform> enemyParent)
        {
            enemyParentList = enemyParent;
        }
    }

    public partial class StageManager : MonoBehaviour // Property 
    {
        private Transform GetEnemyParent(int index)
        {
            if (stageInfo.enemy_type[index] == EnemyType.Boss.ToString())
                return enemyParentList[0];
            else
            {
                if (Physics2D.OverlapCircle(enemyParentList[1].position, 1f, enemyLayer))
                    return enemyParentList[2];
                else
                    return enemyParentList[1];
            }
        }
    }

    public partial class StageManager : MonoBehaviour // Property 
    {
        public bool IsLastStage()
        {
            return Convert.ToBoolean(stageInfo.is_last_stage);
        }
        public bool IsCleared()
        {
            return Convert.ToBoolean(stageInfo.is_clear);
        }
        public void SetIsClear(bool value)
        {
            stageInfo.is_clear = value ? 1 : 0;
        }
    }

    public partial class StageManager : MonoBehaviour // Sign 
    {
        public void SignUpBackGround(BackGround backGroundValue)
        {
            BackGroundList.Add(backGroundValue);
            BackGroundList.Last().Initialize();
        }
        public void SignDownBackGround(BackGround backGroundValue)
        {
            BackGroundList.Remove(backGroundValue);
        }
    }
}
