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
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public partial class AdventureUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler // Data Field
    {

        [SerializeField] private Scrollbar scrollbar;
        [SerializeField] private GameObject content;
        [SerializeField] private StageButton[] stageButton;
        [SerializeField] private GameObject knightEmptyMessage;

        private int contentChildCount;
        private float[] scrollValueArray;
        private float valuePerSpace;
        private float targetValue;
        private float currentValue;

        private int targetIndex;

        private bool isDrag = false;
    }
    public partial class AdventureUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler // Initlaize
    {
        private void Allocate()
        {
            contentChildCount = content.transform.childCount;
            scrollValueArray = new float[contentChildCount];

            valuePerSpace = 1f / (contentChildCount - 1);
            for (int i = 0; i < contentChildCount; i++) scrollValueArray[i] = valuePerSpace * i;
        }
        public void Initialize()
        {
            Allocate();
            Setup();
            for (int i = 0; i < stageButton.Length; i++)
                stageButton[i].Initialize();
            knightEmptyMessage.SetActive(false);
        }
        private void Setup()
        {

        }
    }
    public partial class AdventureUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler // Interface
    {
        public void OnBeginDrag(PointerEventData eventData)
        {
            isDrag = true;
            currentValue = GetValue();
        }

        public void OnDrag(PointerEventData eventData)
        {
            isDrag = true;

        }

        public void OnEndDrag(PointerEventData eventData)
        {
            isDrag = false;
            targetValue = GetValue();
            if (currentValue == targetValue)
            {
                if (eventData.delta.x > 15 && currentValue - valuePerSpace >= 0)
                {
                    targetIndex--;
                    targetValue = scrollValueArray[targetIndex];
                }
                else if (eventData.delta.x < -15 && currentValue + valuePerSpace <= 1.01f)
                {
                    targetIndex++;
                    targetValue = scrollValueArray[targetIndex];
                }
            }
        }
    }
    public partial class AdventureUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler // Main
    {
        private void Update()
        {
            if (!isDrag)
                scrollbar.value = Mathf.Lerp(scrollbar.value, targetValue, 0.1f);
        }
    }
    public partial class AdventureUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler // Property
    {
        public void SetActive(bool value)
        {
            if (value)
                ActiveStage();
            gameObject.SetActive(value);
        }
        public void SetStage(int index)
        {
            if (MainSystem.Instance.PlayerManager.Player.activeKnightList.Count > 0)
            {
                MainSystem.Instance.StageManager.CurrentStage = index;
                MainSystem.Instance.SceneManager.LoadScene(SceneName.CombatScene.ToString());
            }
            else
                knightEmptyMessage.SetActive(true);
        }
        private void ActiveStage()
        {
            int clearStageCount = MainSystem.Instance.InGameManager.ClearStageCount;
            List<int> scoreList = MainSystem.Instance.StageManager.ScoreList;

            stageButton[0].ActiveStarImage(scoreList.Count > 0 ? scoreList[0] : 0);

            for (int i = 0; i < clearStageCount; i++)
            {
                if (i < stageButton.Length - 1)
                    stageButton[i + 1].ActiveStarImage(scoreList.Count > i + 1 ? scoreList[i + 1] : 0);
            }


        }
        private float GetValue()
        {
            for (int i = 0; i < contentChildCount; i++)
            {
                if (scrollbar.value < scrollValueArray[i] + valuePerSpace * 0.5f && scrollbar.value > scrollValueArray[i] - valuePerSpace * 0.5f)
                {
                    targetIndex = i;
                    return scrollValueArray[i];
                }
            }
            return 0;
        }
    }
}
