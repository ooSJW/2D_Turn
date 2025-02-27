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

    public partial class StageButton : MonoBehaviour // Data Field
    {
        private Button button;
        [SerializeField] private Image[] starImage;
    }
    public partial class StageButton : MonoBehaviour // Initialize
    {
        private void Allocate()
        {
            button = GetComponent<Button>();
        }
        public void Initialize()
        {
            Allocate();
            Setup();
            button.interactable = false;
            for (int i = 0; i < starImage.Length; i++)
                starImage[i].gameObject.SetActive(false);
        }
        private void Setup()
        {

        }
    }

    public partial class StageButton : MonoBehaviour // Property
    {
        public void ActiveStarImage(int score)
        {
            button.interactable = true;

            for (int i = 0; i < score; i++)
                starImage[i].gameObject.SetActive(true);
        }
    }
}
