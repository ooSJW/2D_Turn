/*
	* Coder :
	* Last Update :
	* Information
*/
namespace project02
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public partial class UIController : MonoBehaviour // Data Field
    {
        [field: SerializeField] public MenuUI MenuUI { get; private set; }
        [field: SerializeField] public ScoreStar ScoreStar { get; private set; }
        [field: SerializeField] public AdventureUI AdventureUI { get; private set; }
        [field: SerializeField] public KnightManagementUI KnightManagementUI { get; private set; }

    }
    public partial class UIController : MonoBehaviour // Initialize
    {
        private void Allocate()
        {

        }
        public void Initialize()
        {
            Allocate();
            Setup();
            if (MenuUI != null) MenuUI.Initialize();
            if (ScoreStar != null) ScoreStar.Initialize();
            if (AdventureUI != null) AdventureUI.Initialize();
            if (KnightManagementUI != null) KnightManagementUI.Initialize();
        }
        private void Setup()
        {

        }
    }
}
