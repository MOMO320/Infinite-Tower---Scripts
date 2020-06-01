using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public GameObject m_target = null;                  //타겟
    public Vector3 targetPosition = Vector3.zero;       //타겟의 위치
    public int damage = 5;
    public ParticleSystem boomParticle;
    public bool isConnect = false;
    private BalistaTargetingRE balistaTarget;

    private void Start()
    {
        balistaTarget = transform.parent.GetComponent<BalistaTargetingRE>();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "PlayerUnit")
        {
            if (isConnect == false)
            {
                boomParticle.Play(true);
                other.SendMessage("UnitDamage", damage);
                balistaTarget.bulletList.Remove(gameObject);
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
                balistaTarget.bulletList.Remove(gameObject);
            }

            isConnect = true;
            this.gameObject.GetComponent<MeshRenderer>().enabled = false;

            StartCoroutine("DestroyBullet");
        }
        Debug.Log(other.name);
        Debug.Log(other.tag);

    }

   public IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(1.0f);
        isConnect = false;
        this.gameObject.GetComponent<MeshRenderer>().enabled = true;
        Destroy(gameObject);
    }
}

