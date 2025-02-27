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

    public partial class ItemManager : MonoBehaviour // Data Field
    {
        public ItemController ItemController { get; private set; } = default;
    }
    public partial class ItemManager : MonoBehaviour
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

    public partial class ItemManager : MonoBehaviour // Sign
    {
        public void SignUpItemController(ItemController itemControllerValue)
        {
            ItemController=itemControllerValue;
            ItemController.Initialize();
        }
        public void SignDownItemController()
        {
            ItemController=null;
        }
    }
}
