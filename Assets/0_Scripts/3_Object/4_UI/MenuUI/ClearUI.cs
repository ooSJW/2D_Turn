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
    using UnityEngine.UI;

    public partial class ClearUI : MonoBehaviour // Data Field
    {
        [SerializeField] private RectTransform itemGroup;
        [SerializeField] private GameObject battleInfo;
        [SerializeField] private GameObject acquiredInfo;
        [SerializeField] private GameObject vecticalGroup;
        [SerializeField] private Button nextBtn;

    }
    public partial class ClearUI : MonoBehaviour // Initialize
    {
        private void Allocate()
        {

        }
        public void Initialize()
        {
            Allocate();
            Setup();
            battleInfo.SetActive(true);
            acquiredInfo.SetActive(false);
            vecticalGroup.SetActive(false);
        }
        private void Setup()
        {

        }
    }

    public partial class ClearUI : MonoBehaviour // Property
    {
        public RectTransform GetItemParent()
        {
            return itemGroup;
        }

        public void NextBtnClick()
        {
            battleInfo.SetActive(false);
            acquiredInfo.SetActive(true);
            vecticalGroup.SetActive(true);
        }

        public void NextBtnInteractable()
        {
            nextBtn.interactable = true;
        }
    }
}
