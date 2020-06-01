using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaepone : MonoBehaviour
{
    public bool isConnectPlayerUnit;
    private EnemyBase enemyBase;

    private void Awake()
    {
        enemyBase = GetComponentInParent<EnemyBase>();
    }

    private void OnTriggerExit(Collider other)
    {
        if(enemyBase.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            if (other.gameObject.tag == "PlayerUnit" && isConnectPlayerUnit == false)
            {
                other.SendMessage("UnitDamage", enemyBase.enemy_Attack);
          //    Debug.Log("enemyAttack : " + enemyBase.enemy_Attack);
                isConnectPlayerUnit = true;
                StartCoroutine("WaitAttack");
            }
            if(other.gameObject.tag == "PlayerTower" && isConnectPlayerUnit == false)
            {
                other.SendMessage("CastleDamage", enemyBase.enemy_Attack);
                isConnectPlayerUnit = true;
                StartCoroutine("WaitAttack");
            }
        }
    }


    IEnumerator WaitAttack()
    {
        yield return new WaitForSeconds(1f);
        isConnectPlayerUnit = false;
    }
}




































































































































