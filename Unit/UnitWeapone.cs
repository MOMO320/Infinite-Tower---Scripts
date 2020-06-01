using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitWeapone : MonoBehaviour
{
    public bool isConnectEnemyUnit;
    private UnitBase unitBase;
    private BoxCollider boxCollider;

    private void Awake()
    {
        unitBase = GetComponentInParent<UnitBase>();
        boxCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (unitBase.animator.GetBool("isAttack") == true && unitBase.animator.GetBool("isWalk") == false) 
        {
            if (unitBase.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                if (other.gameObject.tag == "EnemyUnit" && isConnectEnemyUnit == false)
                {
                    other.SendMessage("EnemyDamage", unitBase.Unit_Attack);
                    isConnectEnemyUnit = true;
                    boxCollider.enabled = false;
                    StartCoroutine("WaitAttackEnemy");

                }
                if (other.gameObject.tag == "Cannon" || other.gameObject.tag == "Balista")
                {
                    StartCoroutine("WaitDamageTower");
                    other.SendMessage("TowerDamage", unitBase.Unit_Attack);
                    boxCollider.enabled = false;
                    StartCoroutine("WaitDamageTower");
                }

            }
        }
        if (other != null && other.tag == "EnemyUnit")
        {
            // 만약 적이 죽으면 idle 상태로 바꿔준다.
            if (other.gameObject.GetComponent<EnemyBase>().enemy_Hp <= 0)
            {
                unitBase.aniStaus = UnitBase.AniStatus.IDLE;
                boxCollider.enabled = false;
            }
        }
    }

   public IEnumerator WaitAttackEnemy()
    {
        yield return new WaitForSeconds(0.5f);
        isConnectEnemyUnit = false;
        boxCollider.enabled = true;
    }

    public IEnumerator WaitDamageTower()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        boxCollider.enabled = true;
    }
}
