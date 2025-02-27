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

    public partial class BackGround : MonoBehaviour // Data Field
    {
        private float viewWidth;
        [SerializeField] private float speed;
        [SerializeField] private Transform[] backgroundArray;

        private int leftSpriteIndex;
        private int rightSpriteIndex;

        private bool moveBackground = false;
        public bool MoveBackground { get => moveBackground; set => moveBackground = value; }

    }
    public partial class BackGround : MonoBehaviour // Initialize
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
            viewWidth = 50;
            leftSpriteIndex = 0;
            rightSpriteIndex = backgroundArray.Length - 1;
        }
    }

    public partial class BackGround : MonoBehaviour // Main
    {
        private void Update()
        {
            if (MoveBackground)
            {
                Vector3 nextPos = Vector3.left * speed * Time.deltaTime;
                transform.position = transform.position + nextPos;

                if (backgroundArray[leftSpriteIndex].position.x < viewWidth * -1.5f)
                {
                    Vector3 rigthSpritePos = backgroundArray[rightSpriteIndex].localPosition;
                    backgroundArray[leftSpriteIndex].localPosition = rigthSpritePos + Vector3.right * viewWidth;

                    int temp = leftSpriteIndex;
                    leftSpriteIndex = temp + 1 > backgroundArray.Length - 1 ? 0 : temp + 1;
                    rightSpriteIndex = temp;
                }
            }
        }
    }
    public partial class BackGround : MonoBehaviour // Property
    {
        public Transform GetLastBackGround()
        {
            return backgroundArray[rightSpriteIndex];
        }
    }
}
