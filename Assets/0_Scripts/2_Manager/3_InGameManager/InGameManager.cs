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
    using TMPro;
    using Unity.VisualScripting;
    using UnityEngine;
    using static project02.ItemData;
    using static project02.StageData;

    public partial class InGameManager : MonoBehaviour // Data Property
    {
        private CombatState combatState = CombatState.None;
        public CombatState CombatState
        {
            get => combatState;
            set
            {
                if (combatState != value)
                {
                    combatState = value;

                    switch (combatState)
                    {
                        case CombatState.None:
                            turnObject.Clear();
                            break;
                        case CombatState.Start:
                            MainSystem.Instance.StageManager.MoveStage = false;
                            MainSystem.Instance.StageManager.Score = 0;
                            for (int i = 0; i < knightList.Count; i++)
                            {
                                if (knightList[i].KnightState != KnightState.Death)
                                    knightList[i].KnightState = KnightState.Idle;
                            }
                            StartCoroutine(PlayerTurn());
                            break;

                        case CombatState.PlayerTurn:
                            //knightList.Cast<CombatObjectBase>().ToList();
                            turnObject.Clear();
                            for (int i = 0; i < knightList.Count; i++)
                            {
                                if (knightList[i].KnightState != KnightState.Death)
                                    turnObject.Add(knightList[i]);
                            }

                            if (turnObject.Count > 0)
                            {
                                if (MainSystem.Instance.SceneManager.ActiveScene.name == SceneName.CombatScene.ToString())
                                    turnObject[0].Active(enemyList[UnityEngine.Random.Range(0, enemyList.Count)].transform);
                            }

                            break;

                        case CombatState.EnemyTurn:
                            turnObject = enemyList.Cast<CombatObjectBase>().ToList();
                            Transform aliverKnight = RandomAliveKnight();
                            if (aliverKnight != null)
                                turnObject[0].Active(aliverKnight);
                            else
                                CombatState = CombatState.PlayerLose;
                            break;

                        case CombatState.GoNextStage:
                            MainSystem.Instance.StageManager.MoveStage = true;

                            knightList = MainSystem.Instance.PlayerManager.Player.activeKnightList;
                            enemyList = MainSystem.Instance.EnemyManager.enemyList;
                            for (int i = 0; i < knightList.Count; i++)
                            {
                                if (knightList[i].KnightState != KnightState.Death)
                                    knightList[i].KnightState = KnightState.GoNextStage;
                            }
                            break;

                        case CombatState.AllStageClear:
                            MainSystem.Instance.StageManager.Score += 3;
                            RewardPayment();
                            MainSystem.Instance.DataManager.SaveStageData(MainSystem.Instance.StageManager.CurrentStage, MainSystem.Instance.StageManager.Score);
                            MainSystem.Instance.DataManager.SaveItemData();
                            MainSystem.Instance.DataManager.SavePlayerData();
                            MainSystem.Instance.DataManager.SaveKnightData();
                            StageClear();
                            break;

                        case CombatState.PlayerLose:
                            StartCoroutine(PlayerLose());
                            break;
                    }
                }
            }
        }


    }
    public partial class InGameManager : MonoBehaviour // Data Field
    {
        public List<CombatObjectBase> turnObject;
        private List<Knight> knightList;
        private List<Enemy> enemyList;

        public int ClearStageCount { get; set; }

        private float delayTime = 1.5f;
    }

    public partial class InGameManager : MonoBehaviour // Initialize
    {
        private void Allocate()
        {
            turnObject = new List<CombatObjectBase>();
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

    public partial class InGameManager : MonoBehaviour // Coroutine
    {
        IEnumerator ActivateMenuUI()
        {
            yield return new WaitForSeconds(delayTime);
            MainSystem.Instance.UIManager.UIController.MenuUI.PauseGame(true);
            MainSystem.Instance.UIManager.UIController.ScoreStar.ActiveStar(MainSystem.Instance.StageManager.Score);
            yield return new WaitForSeconds(delayTime);
            MainSystem.Instance.UIManager.UIController.MenuUI.ClearUI.NextBtnInteractable();
        }
        IEnumerator PlayerLose()
        {
            yield return new WaitForSeconds(0.5f);
            MainSystem.Instance.SoundManager.SoundController.Bgm.StopBgm();
            yield return new WaitForSeconds(1f);
            MainSystem.Instance.UIManager.UIController.MenuUI.PlayerLose();
            MainSystem.Instance.SoundManager.SoundController.Sfx.PlaySfx(AudioClipName.Sfx_PlayerLose);
        }
        IEnumerator PlayerTurn()
        {
            yield return new WaitForSeconds(0.5f);
            CombatState = CombatState.PlayerTurn;
        }
    }
    public partial class InGameManager : MonoBehaviour // Private Property
    {
        private void StageClear()
        {
            StartCoroutine(ActivateMenuUI());
            if (!MainSystem.Instance.StageManager.IsCleared())
                MainSystem.Instance.StageManager.SetIsClear(true);
        }

        private Transform RandomAliveKnight()
        {
            List<Knight> aliveKnightList = new List<Knight>();
            foreach (Knight knight in knightList)
            {
                if (knight.KnightState != KnightState.Death)
                    aliveKnightList.Add(knight);
            }
            if (aliveKnightList.Count > 0)
            {
                int random = UnityEngine.Random.Range(0, aliveKnightList.Count);
                return aliveKnightList[random].transform;
            }
            else
                return null;
        }

        private void RewardPayment()
        {
            StageInformation stageInfo = MainSystem.Instance.StageManager.StageInfo;
            MainSystem.Instance.PlayerManager.Player.GetCoin(stageInfo.reward_gold);
            List<Knight> knightList = MainSystem.Instance.PlayerManager.Player.activeKnightList;
            for (int i = 0; i < knightList.Count; i++)
                knightList[i].Exp += stageInfo.reward_exp;

            RewardType rewardType = Enum.Parse<RewardType>(stageInfo.reward_type);
            RectTransform itemParent = MainSystem.Instance.UIManager.UIController.MenuUI.ClearUI.GetItemParent();

            MainSystem.Instance.ItemManager.ItemController.SpawnCoin(itemParent, stageInfo.reward_gold);

            for (int i = 0; i < stageInfo.reward_item_count; i++)
            {
                switch (rewardType)
                {
                    case RewardType.Knight:
                        string knightName = stageInfo.knight_name[i];
                        Player player = MainSystem.Instance.PlayerManager.Player;
                        if (player.knightList.Find(elem => elem == knightName) == null)
                        {
                            MainSystem.Instance.PoolManager.Spawn(knightName + "UI", itemParent);
                            player.knightList.Add(knightName);
                        }

                        break;

                    case RewardType.Item:

                        Item item = MainSystem.Instance.ItemManager.ItemController.SpawnRandomItem(itemParent);
                        MainSystem.Instance.PlayerManager.Player.itemList.Add(item);
                        break;
                }
            }
        }
        private void SetTarget(CombatObjectBase endTurnObject)
        {
            if (turnObject.Count > 0)
            {
                if (endTurnObject is Knight)
                {
                    if (enemyList.Count > 0)
                        turnObject[0].Active(enemyList[UnityEngine.Random.Range(0, enemyList.Count)].transform);
                }
                else
                {
                    Transform aliveKnight = RandomAliveKnight();
                    if (aliveKnight != null)
                        turnObject[0].Active(aliveKnight);
                    else
                        CombatState = CombatState.PlayerLose;
                }
            }
        }
        private void TurnOver()
        {
            if (enemyList.Count <= 0)
            {
                MainSystem.Instance.StageManager.CurrentStage++;

                if (MainSystem.Instance.StageManager.IsLastStage())
                    CombatState = CombatState.AllStageClear;
                else
                    CombatState = CombatState.GoNextStage;
            }

            else if (!MainSystem.Instance.PlayerManager.Player.IsAlive())
                CombatState = CombatState.PlayerLose;

            else
            {
                if (turnObject.Count <= 0)
                {
                    switch (CombatState)
                    {
                        case CombatState.PlayerTurn:
                            CombatState = CombatState.EnemyTurn;
                            break;
                        case CombatState.EnemyTurn:
                            if (MainSystem.Instance.PlayerManager.Player.IsAlive())
                                CombatState = CombatState.PlayerTurn;
                            else
                                CombatState = CombatState.PlayerLose;
                            break;
                    }
                }
            }
        }
    }
    public partial class InGameManager : MonoBehaviour // Property
    {
        public void EndTurn(CombatObjectBase endTurnObject)
        {
            turnObject.Remove(endTurnObject);
            SetTarget(endTurnObject);
            TurnOver();
        }
    }
}