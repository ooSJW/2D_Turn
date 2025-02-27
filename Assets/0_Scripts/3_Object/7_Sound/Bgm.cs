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

    public partial class Bgm : MonoBehaviour // Data Field
    {
        [SerializeField] private AudioClip bgm;
        [SerializeField] private Slider bgmSlider;
        private AudioSource bgmPlayer;
        private float bgmVolume;
    }
    public partial class Bgm : MonoBehaviour // Initialize
    {
        private void Allocate()
        {
            bgmPlayer = gameObject.AddComponent<AudioSource>();
        }
        public void Initialize()
        {
            Allocate();
            Setup();
            bgmPlayer.Play();
        }
        private void Setup()
        {
            if (PlayerPrefs.HasKey("bgmVolume"))
                bgmVolume = PlayerPrefs.GetFloat("bgmVolume");
            else
                bgmVolume = 0.3f;

            bgmPlayer.loop = true;
            bgmPlayer.playOnAwake = true;
            bgmPlayer.clip = bgm;
            bgmPlayer.volume = bgmVolume;

            bgmSlider.value = bgmVolume;
        }
    }
    public partial class Bgm : MonoBehaviour // Property
    {
        public void SetBgmVolume(float volume)
        {
            bgmVolume = volume;
            bgmPlayer.volume = volume;
            PlayerPrefs.SetFloat("bgmVolume", bgmVolume);
        }
        public void StopBgm()
        {
            bgmPlayer.Pause();
        }
        public void ResumeBgm()
        {
            bgmPlayer.UnPause();
        }
    }
}
