using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    public GameObject m_target = null;                  //타겟
    public Vector3 targetPosition = Vector3.zero;       //타겟의 위치
    public int damage = 5;
    public ParticleSystem boomParticle;
    public bool isConnect = false;
    private TowerTargetingRE towerTarget;

    private void Start()
    {
        towerTarget = transform.parent.GetComponent<TowerTargetingRE>();
    }

    private void FixedUpdate()
    {
       
      //  transform.up = GetComponent<Rigidbody>().velocity;      // y축(머리)를 해당 방향으로 설정
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "PlayerUnit")
        {
            if(isConnect == false)
            {
                boomParticle.Play(true);
                other.SendMessage("UnitDamage", damage);
                towerTarget.bulletList.Remove(gameObject);
            }

            isConnect = true;
            this.gameObject.GetComponent<MeshRenderer>().enabled = false;

            StartCoroutine("DestroyBullet");
        }

        if (other.gameObject.tag == "Ground")
        {
            if (isConnect == false)
            {
                boomParticle.Play(true);
                other.SendMessage("UnitDamage", damage);
                towerTarget.bulletList.Remove(gameObject);
            }

            isConnect = true;
            this.gameObject.GetComponent<MeshRenderer>().enabled = false;

            StartCoroutine("DestroyBullet");
        }
        Debug.Log(other.name);
        Debug.Log(other.tag);

    }

    IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(1.0f);
        isConnect = false;
        this.gameObject.GetComponent<MeshRenderer>().enabled = true;
        Destroy(gameObject);
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if(collision.transform.CompareTag("EnemyUnit")) //EnemyUnit태그가 붙은 객체와 충돌했을때
    //    {
    //        boomParticle.Play(true);
    //        Destroy(gameObject);        // 자신을 삭제
    //    }

    //    //boomParticle.Play(true);
    //    //Destroy(gameObject);
    //}

}
