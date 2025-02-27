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

    public partial class HpParticle : MonoBehaviour // Data Field
    {
        [SerializeField] private TextMesh damageText;

        [SerializeField] private Color originColor;
        [SerializeField] private Color criticalColor;
        [SerializeField] private Color healColor;
        private Vector3 destPos = Vector3.zero;
    }
    public partial class HpParticle : MonoBehaviour // Initialize
    {
        private void Allocate()
        {
            destPos = transform.position + transform.up;
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
    public partial class HpParticle : MonoBehaviour // Main
    {
        private void Update()
        {
            if (Mathf.Approximately(transform.position.y, destPos.y))
                MainSystem.Instance.PoolManager.Despawn(gameObject);

            transform.position = Vector3.MoveTowards(transform.position, destPos, 2f * Time.deltaTime);
        }
    }
    public partial class HpParticle : MonoBehaviour // Property
    {
        public void SetHpText(int damage, bool isAttack, bool isCritical = false)
        {
            if (isAttack && isCritical)
                damageText.color = criticalColor;
            else if (!isAttack)
                damageText.color = healColor;
            else
                damageText.color = originColor;

            damageText.text = damage.ToString();
        }
    }
}
