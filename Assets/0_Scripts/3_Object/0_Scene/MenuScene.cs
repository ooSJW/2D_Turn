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

    public partial class MenuScene : BaseScene
    {

    }
    public partial class MenuScene : BaseScene
    {
        private void Allocate()
        {

        }
        public override void Initialize()
        {
            Allocate();
            Setup();
            MainSystem.Instance.PoolManager.Register();
            MainSystem.Instance.PlayerManager.SignUpPlayer(Player);
            MainSystem.Instance.ItemManager.SignUpItemController(ItemController);

            MainSystem.Instance.DataManager.LoadStageData();
            MainSystem.Instance.DataManager.LoadPlayerData();
            MainSystem.Instance.DataManager.LoadItemData();

            if (Player.knightList.Count == 0)
            {
                Player.knightList.Add("Alex");
            }
            MainSystem.Instance.UIManager.SignUpUIController(UIController);
            MainSystem.Instance.SoundManager.SignUpSoundController(SoundController);
        }
        private void Setup()
        {

        }
    }
}
