/*
	* Coder :
	* Last Update :
	* Information
*/
namespace project02
{
    using DG.Tweening;
    using System.Collections;
    using System.Collections.Generic;
    using Unity.VisualScripting;
    using UnityEngine;
    using UnityEngine.UI;

    public partial class ScoreStar : MonoBehaviour
    {
        [SerializeField] private Image[] starImage;
    }
    public partial class ScoreStar : MonoBehaviour
    {

    }
    public partial class ScoreStar : MonoBehaviour
    {
        private void Allocate()
        {

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
    public partial class ScoreStar : MonoBehaviour // Property
    {
        public void ActiveStar(int scoreValue)
        {
            foreach (var item in starImage)
            {
                item.gameObject.SetActive(false);
            }
            int score = Mathf.Clamp(scoreValue, 0, 3);
            for (int i = 0; i < score; i++)
            {
                RectTransform starRectTransform = starImage[i].rectTransform;

                starRectTransform.DOScale(Vector3.one * 3, 0.5f)
                .SetEase(Ease.Linear)
                .From()
                .SetDelay(i * 0.5f)
                .OnStart(() => starRectTransform.gameObject.SetActive(true));

                starRectTransform.DORotate(new Vector3(0, 0, 360), 0.5f, RotateMode.FastBeyond360)
                    .SetEase(Ease.Linear).SetDelay(i * 0.5f).OnComplete(() => MainSystem.Instance.SoundManager.SoundController.Sfx.PlaySfx(AudioClipName.Sfx_Star));
            }
        }
    }
}
