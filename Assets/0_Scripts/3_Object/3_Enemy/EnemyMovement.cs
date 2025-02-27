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

    public partial class EnemyMovement : MonoBehaviour // Data Field
    {
        private Enemy enemy;
        private float moveSpeed;
        private Vector3 destPosition = default;
    }
    public partial class EnemyMovement : MonoBehaviour // Initialize
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
            moveSpeed = enemy.EnemyStatInformation.move_speed;
        }
    }

    public partial class EnemyMovement : MonoBehaviour // Main
    {
        public void Progress()
        {
            if (enemy.IsMyTurn)
            {
                switch (enemy.EnemyState)
                {
                    case EnemyState.Idle:
                        if (enemy.Target != null)
                            enemy.EnemyState = EnemyState.GoForward;
                        else
                            enemy.EnemyState = EnemyState.Return;
                        break;

                    case EnemyState.GoForward:
                        Movement(EnemyState.GoForward);
                        break;

                    case EnemyState.Return:
                        Movement(EnemyState.Return);
                        break;
                }
            }
        }
    }
    public partial class EnemyMovement : MonoBehaviour // Property
    {
        public bool GetAttackableInRange()
        {
            if (enemy.Target == null)
                return false;
            else
                return (enemy.Target.position - transform.position).magnitude <= enemy.EnemyStatInformation.attack_range;
        }

        public void Movement(EnemyState state)
        {
            if (GetAttackableInRange())
                enemy.EnemyState = EnemyState.Attack;

            else
            {
                switch (state)
                {
                    case EnemyState.GoForward:
                        if (enemy.IsMyTurn)
                        {
                            destPosition = enemy.Target.position;
                            destPosition.x += enemy.EnemyStatInformation.attack_range;
                        }
                        break;
                    case EnemyState.Return:
                        destPosition = enemy.OriginPos;
                        if (Mathf.Approximately((transform.position - destPosition).magnitude, 0))
                        {
                            enemy.EnemyState = EnemyState.Idle;
                            EndTurn();
                        }
                        break;
                }
                transform.position = Vector2.MoveTowards(transform.position, destPosition, moveSpeed * Time.deltaTime);
            }
        }

        public void EndTurn()
        {
            enemy.EnemyState = EnemyState.Idle;
            enemy.IsMyTurn = false;
            enemy.Target = null;
            MainSystem.Instance.InGameManager.EndTurn(enemy);
        }

        public void EndAttack()
        {
            enemy.Target = null;
            enemy.EnemyState = EnemyState.Idle;
        }
    }
}
