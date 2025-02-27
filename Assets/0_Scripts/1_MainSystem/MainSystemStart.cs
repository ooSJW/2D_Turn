/*
	* Coder :
	* Last Update :
	* Information
*/
namespace project02
{
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using UnityEngine;

    public partial class MainSystemStart : MonoBehaviour
    {
        private void Awake()
        {
            MainSystem.Instance.MainSystemStart();
        }
    }
}
