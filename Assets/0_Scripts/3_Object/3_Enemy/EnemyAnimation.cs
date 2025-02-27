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

    public partial class EnemyAnimation : MonoBehaviour // Data Field
    {
        private Enemy enemy;
        [SerializeField] private Animator animator;
        [SerializeField] private SpriteRenderer spriteRenderer;
    }

    public partial class EnemyAnimation : MonoBehaviour // Initialize
    {
        private void Allocate()
        {

        }
        public void Initialize(Enemy enemyValue)
        {
            enemy = enemyValue;

            Allocate();
            Setup();
        }
        private void Setup()
        {

        }
    }

    public partial class EnemyAnimation : MonoBehaviour // Main
    {
        public void Progress()
        {
            SetAnimationState();
        }
    }

    public partial class EnemyAnimation : MonoBehaviour // Property
    {
        public void SetAnimationState()
        {
            animator.SetInteger(CombatObjectAnimationParam.State.ToString(), (int)enemy.EnemyState);
        }

        public void FlipX(bool value)
        {
            spriteRenderer.flipX = value;
        }

        public void ReturnState()
        {
            enemy.EnemyState = EnemyState.Idle;
        }
    }
}
