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
    using DG.Tweening;
    public partial class KnightMovement : MonoBehaviour // Data Field
    {
        private Knight knight;
        private float moveSpeed;
        private Vector3 destPosition = default;
    }
    public partial class KnightMovement : MonoBehaviour // Initialize
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
            moveSpeed = knight.KnightStatInformation.move_speed;
        }
    }

    public partial class KnightMovement : MonoBehaviour // Main
    {
        public void Progress()
        {
            if (knight.IsMyTurn)
            {
                switch (knight.KnightState)
                {
                    case KnightState.Idle:
                        if (knight.Target != null)
                            knight.KnightState = KnightState.GoForward;
                        else
                            knight.KnightState = KnightState.Return;
                        break;

                    case KnightState.GoForward:
                        Movement(KnightState.GoForward);
                        break;

                    case KnightState.Return:
                        Movement(KnightState.Return);
                        break;
                }
            }
        }
    }

    public partial class KnightMovement : MonoBehaviour // Private property
    {
        private bool GetAttackableInRange()
        {
            if (knight.Target == null)
                return false;
            else
                return (knight.Target.position - transform.position).magnitude <= knight.KnightStatInformation.attack_range;
        }

        private bool GetUseableSkillRange()
        {
            if (knight.Target == null)
                return false;
            else
                return (knight.Target.position - transform.position).magnitude <= knight.KnightSkillDict[knight.selectedSkillList[0]].SkillInfo.range;
        }

    }

    public partial class KnightMovement : MonoBehaviour // Property
    {
        public void Movement(KnightState state)
        {
            if (knight.selectedSkillList.Count > 0 && knight.selectedSkillList[0] != SkillName.None)
            {
                if (GetUseableSkillRange())
                    knight.KnightState = KnightState.UseSkill;
                else
                {
                    switch (state)
                    {
                        case KnightState.GoForward:
                            if (knight.IsMyTurn)
                            {
                                destPosition = knight.Target.position;
                                destPosition.x -= knight.KnightStatInformation.attack_range;
                            }
                            break;
                        case KnightState.Return:
                            destPosition = knight.OriginPos;
                            if (Mathf.Approximately((transform.position - destPosition).magnitude, 0))
                            {
                                knight.KnightState = KnightState.Idle;
                                EndTurn();
                            }
                            break;
                    }
                    transform.position = Vector2.MoveTowards(transform.position, destPosition, moveSpeed * Time.deltaTime);

                }
            }

            else if (GetAttackableInRange())
            {
                knight.KnightState = KnightState.Attack;
            }

            else
            {
                switch (state)
                {
                    case KnightState.GoForward:
                        if (knight.IsMyTurn)
                        {
                            destPosition = knight.Target.position;
                            destPosition.x -= knight.KnightStatInformation.attack_range;
                        }
                        break;
                    case KnightState.Return:
                        destPosition = knight.OriginPos;
                        if (Mathf.Approximately((transform.position - destPosition).magnitude, 0))
                        {
                            knight.KnightState = KnightState.Idle;
                            EndTurn();
                        }
                        break;
                }
                transform.position = Vector2.MoveTowards(transform.position, destPosition, moveSpeed * Time.deltaTime);

            }
        }

        public void EndTurn()
        {
            knight.KnightState = KnightState.Idle;
            knight.IsMyTurn = false;
            knight.Target = null;
            MainSystem.Instance.InGameManager.EndTurn(knight);
        }
        public void EndAttack()
        {
            knight.Target = null;
            knight.KnightState = KnightState.Idle;
        }

        public void EndSkillUse()
        {
            knight.Target = null;
            knight.KnightState = KnightState.Idle;
            SkillName skillName = knight.selectedSkillList[0];
            knight.KnightSkillDict[skillName].IsCoolTime = true;
            knight.selectedSkillList.Remove(skillName);
        }
    }
}