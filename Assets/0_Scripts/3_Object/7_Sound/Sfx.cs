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
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public partial class Sfx : MonoBehaviour // Data Field
    {
        [SerializeField] private AudioClip[] sfxClips;
        [SerializeField] private Slider sfxSlider;
        private AudioSource[] sfxPlayers;
        private float sfxVolume = 0.2f;
    }
    public partial class Sfx : MonoBehaviour // Initialize
    {
        private void Allocate()
        {
            sfxPlayers = new AudioSource[sfxClips.Length];
        }
        public void Initialize()
        {
            Allocate();
            Setup();
            AddBtnClickEent();
        }
        private void Setup()
        {
            if (PlayerPrefs.HasKey("sfxVolume"))
                sfxVolume = PlayerPrefs.GetFloat("sfxVolume");

            for (int i = 0; i < sfxPlayers.Length; i++)
            {
                sfxPlayers[i] = gameObject.AddComponent<AudioSource>();
                sfxPlayers[i].loop = false;
                sfxPlayers[i].clip = sfxClips[i];
                sfxPlayers[i].playOnAwake = false;
                sfxPlayers[i].volume = sfxVolume;
            }
            sfxSlider.value = sfxVolume;
        }
    }

    public partial class Sfx : MonoBehaviour // Private Property
    {
        private void AddBtnClickEent()
        {
            Button[] buttons = FindObjectsByType<Button>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            
            foreach (Button button in buttons)
            {
                button.onClick.AddListener(() => PlayButtonClickSound());
            }
        }
    }
    public partial class Sfx : MonoBehaviour // Property
    {
        public void PlayButtonClickSound()
        {
            sfxPlayers[(int)AudioClipName.Sfx_Click].Play();
        }
        public void PlayReinforceSound()
        {
            sfxPlayers[(int)AudioClipName.Sfx_Reinforce].Play();
        }
        public void PlaySfx(AudioClipName clipName)
        {
            sfxPlayers[(int)clipName].Play();
        }
        public void SetSfxVolume(float volume)
        {
            sfxVolume = volume;

            for (int i = 0; i < sfxPlayers.Length; i++)
                sfxPlayers[i].volume = volume;

            PlayerPrefs.SetFloat("sfxVolume", sfxVolume);
        }
    }
}
