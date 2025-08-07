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
            // 스테이지 보상 지급 시 사용
            // 반환한 Item객체를 보여주고, list에 담아 playerPrefs로 저장할 목적의 메서드
            int random = Random.Range(0, itemArray.Length);
            Item item = 
                MainSystem.Instance.PoolManager.Spawn(itemArray[random].ItemInformation.prefab_name, parent).GetComponent<Item>();
            item.Initialize();
            return item;
        }

        public Item SpawnItem(string itemName)
        {
            // 데이터 로드 시 사용 
            // PlayerPrefs에서 이름을 받아 생성 후 반환
            // 외부에서 해당 메서드를 통해 객체를 반환 받아 멤버 값을 playerPrefs에서 불러와 적용시킬 목적의 메서드
            Item item = MainSystem.Instance.PoolManager.Spawn(itemName, itemParent).GetComponent<Item>();
            item.Initialize();
            return item;
        }
    }
}
