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

    public partial class CombatStatInformationBase : BaseInformation
    {
        public string name;

        public int level;
        public int[] max_hp;
        public int[] attack;
        public int[] defence;
        public string[] useable_skill;

        public float critical_percent;
        public float critical_increase;
        public float move_speed;
        public float attack_range;
        public int max_level;

    }
}
