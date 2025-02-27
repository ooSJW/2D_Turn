/*
	* Coder :
	* Last Update :
	* Information
*/
namespace project02
{
    using JetBrains.Annotations;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using static project02.ItemData;
    using static project02.KnightStatData;

    public partial class Player : MonoBehaviour
    {
        [SerializeField] private Transform[] knightParentArray;
        [SerializeField] private Transform hideParent;

        private int coin = 0;
        public int Coin
        {
            get => coin;
            private set
            {
                if (coin != value)
                {
                    coin = value;
                    MainSystem.Instance.DataManager.SavePlayerData();
                    if (MainSystem.Instance.UIManager.UIController != null)
                        MainSystem.Instance.UIManager.UIController.MenuUI.RefreshCoinText(coin);
                }
            }
        }
        public List<string> knightList;
        public List<Knight> activeKnightList;
        public List<Item> itemList;
    }
    public partial class Player : MonoBehaviour
    {
        private void Allocate()
        {
            itemList = new List<Item>();
            activeKnightList = new List<Knight>();
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

    public partial class Player : MonoBehaviour // Property
    {
        public int GetAverage()
        {
            int value = 0;
            if (activeKnightList.Count > 0)
            {
                for (int i = 0; i < activeKnightList.Count; i++)
                    value += activeKnightList[i].Level;
            }
            return value / activeKnightList.Count;
        }
        public void GetCoin(int coinValue)
        {
            Coin += coinValue;
        }
        public void UseCoin(int coinValue)
        {
            Coin -= coinValue;
        }
        public Knight SpawnKnight(string name)
        {
            Knight knight = MainSystem.Instance.PoolManager.Spawn(name, hideParent, hideParent.position).GetComponent<Knight>();
            knight.Initialize();
            MainSystem.Instance.DataManager.LoadKnightData(knight);
            return knight;
        }
        public Knight SpawnKnight(string name, int index)
        {
            if (knightList.Contains(name))
            {
                Knight activeKnight = MainSystem.Instance.PoolManager.Spawn(name, knightParentArray[index], knightParentArray[index].position).GetComponent<Knight>();
                activeKnight.Initialize();
                activeKnight.IsEquip = true;
                MainSystem.Instance.DataManager.LoadKnightData(activeKnight);
                Knight duplicateKnight = activeKnightList.Find(elem => elem.name == name);

                if (duplicateKnight != null)
                {
                    MainSystem.Instance.PoolManager.Despawn(duplicateKnight.gameObject);
                    activeKnightList.Remove(duplicateKnight);
                }

                if (activeKnightList.Count > index)
                    activeKnightList.Insert(index, activeKnight);
                else
                    activeKnightList.Add(activeKnight);
                return activeKnight;
            }
            return null;
        }
        public void DeSpawnKnight(string name)
        {
            Knight target = activeKnightList.Find(elem => elem.name == name).GetComponent<Knight>();
            if (target != null)
            {
                activeKnightList.Remove(target);
                target.IsEquip = false;
                MainSystem.Instance.PoolManager.Despawn(target.gameObject);
            }
        }
        public bool IsAlive()
        {
            for (int i = 0; i < activeKnightList.Count; i++)
            {
                if (activeKnightList[i].KnightState != KnightState.Death)
                    return true;
            }
            return false;
        }
    }
}
