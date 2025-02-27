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

    public partial class KnightAnimation : MonoBehaviour // Data Field
    {
        private Knight knight;
        [SerializeField] private Animator animator;
        [SerializeField] private SpriteRenderer spriteRenderer;
    }
    public partial class KnightAnimation : MonoBehaviour // Initialize
    {
        private void Allocate()
        {

        }
        public void Initialize(Knight knightValue)
        {
            knight = knightValue;

            Allocate();
            Setup();
        }
        private void Setup()
        {

        }
    }

    public partial class KnightAnimation : MonoBehaviour // Property
    {
        public void Progress()
        {
            SetAnimationState();
        }
    }

    public partial class KnightAnimation : MonoBehaviour // Property
    {
        public void SetAnimationState()
        {
            animator.SetInteger(CombatObjectAnimationParam.State.ToString(), (int)knight.KnightState);
        }

        public void ReturnState()
        {
            knight.KnightState = KnightState.Idle;
        }

        public void FlipX(bool value)
        {
            spriteRenderer.flipX = value;
        }

        public void FocusObject()
        {
            spriteRenderer.sortingLayerName = "FocusObject";
        }

        public void UnFocusObject()
        {
            spriteRenderer.sortingLayerName = "Default";
        }
    }
}
