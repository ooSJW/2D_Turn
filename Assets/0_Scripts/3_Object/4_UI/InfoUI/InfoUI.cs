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
    using TMPro;

    public partial class InfoUI : MonoBehaviour // Data Field
    {
        [SerializeField] private Image fillImage;
        [SerializeField] private GameObject hpLineFolder;
        [SerializeField] private TextMeshProUGUI levelText;
    }
    public partial class InfoUI : MonoBehaviour // Initialize
    {
        private void Allocate()
        {

        }
        public void Initialize()
        {
            Allocate();
            Setup();

            if (MainSystem.Instance.SceneManager.ActiveScene.name != SceneName.CombatScene.ToString())
                gameObject.SetActive(false);
            else
                gameObject.SetActive(true);
        }
        private void Setup()
        {

        }
    }

    public partial class InfoUI : MonoBehaviour // Property
    {
        public void SetHpScale(float maxHp)
        {
            //float scaleX = Mathf.Clamp((400 / 80) / (maxHp / 80), 0.1f, 0.5f);

            float scaleX = Mathf.Clamp(75 / maxHp, 0.12f, 0.45f);
            hpLineFolder.GetComponent<HorizontalLayoutGroup>().gameObject.SetActive(false);
            foreach (Transform child in hpLineFolder.transform)
            {
                child.gameObject.transform.localScale = new Vector3(scaleX, 1.2f, 1);
            }
            hpLineFolder.GetComponent<HorizontalLayoutGroup>().gameObject.SetActive(true);

        }
        public void SetHpUI(float currentHP, float maxHP)
        {
            fillImage.fillAmount = currentHP / maxHP;
        }
        public void SetLevelText(int level)
        {
            levelText.text = level.ToString();
        }
    }
}
