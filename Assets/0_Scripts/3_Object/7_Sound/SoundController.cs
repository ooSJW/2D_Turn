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

    public partial class SoundController : MonoBehaviour
    {
        [field: SerializeField] public Bgm Bgm {get; set;}
        [field: SerializeField] public Sfx Sfx {get; set;}
    }
    public partial class SoundController : MonoBehaviour
    {
        private void Allocate()
        {

        }
        public void Initialize()
        {
            Allocate();
            Setup();
            Bgm.Initialize();
            Sfx.Initialize();
        }
        private void Setup()
        {

        }
    }
}
