/*
	* Coder :
	* Last Update :
	* Information
*/
namespace project02
{
    using System.Collections;
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine;

    public partial class ItemController : MonoBehaviour // Data Field
    {
        [SerializeField] private Item[] itemArray;
        [SerializeField] private RectTransform itemParent;
    }
    public partial class ItemController : MonoBehaviour // Initialize
    {
        private void Allocate()
        {

        }
        public void Initialize()
        {
            Allocate();
            Setup();
            for (int i = 0; i < itemArray.Length; i++)
            {
                itemArray[i].Initialize();
            }
        }
        private void Setup()
        {

        }
    }

    public partial class ItemController : MonoBehaviour // Property
    {
        public void SpawnCoin(Transform parent, int amount)
        {
            TextMeshProUGUI coinText = MainSystem.Instance.PoolManager.Spawn("Coin", parent).GetComponentInChildren<TextMeshProUGUI>();
            coinText.text = amount.ToString();
        }
        public Item SpawnRandomItem(Transform parent)
        {
            int random = Random.Range(0, itemArray.Length);
            Item item = MainSystem.Instance.PoolManager.Spawn(itemArray[random].ItemInformation.prefab_name, parent).GetComponent<Item>();
            item.Initialize();
            return item;
        }

        public Item SpawnItem(string itemName)
        {
            Item item = MainSystem.Instance.PoolManager.Spawn(itemName, itemParent).GetComponent<Item>();
            item.Initialize();
            return item;
        }
    }
}
