using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine.AI;

public class EnemyBase : MonoBehaviour
{
    // 에너미 요소 ( HP/ Attack / SpawnTime / Count(용병 갯수)
    public int enemy_Hp;
    public int enemy_Attack;
    public float enemy_SpawnTime;
    public int enemy_Count;
    public int enemy_DeathMoney;

    // 애니메이션 연결
    public Animator animator;
    [SerializeField] public AniStatus aniStaus;

    // 공격모션에 따른 무기 collider 활성화
    private BoxCollider boxCollider;
    private EnemyWaepone enemyWaepone;

    // hp 값에 따른 hpBar값
    public Slider hpSlider;
    private float fillAmountHp;

    // enemyUnit이 죽으면 alpha처리 해준다.
    private SkinnedMeshRenderer[] enemyMeshList;
    private bool isAlpha = false;
    private float alphaValue = 1f;

    // Money 파티클 
    public ParticleSystem moneyParticle;
    private bool isAlphaDone = false;

    // 네비메쉬 에이젠트
    NavMeshAgent agent;
    public List<GameObject> enemyUnit = new List<GameObject>();

    [SerializeField] private Transform tf_Destination;
    // 가장 가까이에 있는 playerUnit을 공격한다.\
    [SerializeField]private GameObject target;
    private bool isTarget;

    public enum AniStatus
    {
        IDLE , ATTACK , DIE , WALK
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        boxCollider = GetComponentInChildren<BoxCollider>();
        enemyWaepone = GetComponentInChildren<EnemyWaepone>();
        enemyMeshList = GetComponentsInChildren<SkinnedMeshRenderer>();
        agent = GetComponent<NavMeshAgent>();
        isTarget = false;
        //moneyParticle = GetComponent<ParticleSystem>();
    }

    private void OnEnable()
    {
        fillAmountHp = 100 / enemy_Hp;
        moneyParticle.Stop(true);
        agent.SetDestination(tf_Destination.position);
        aniStaus = AniStatus.WALK;

    }

    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.clear;
        //Gizmos.DrawSphere(transform.position, 5f);
    }

    protected void FixedUpdate()
    {
        StatusChange(this.transform.position);
        AnimStatus(aniStaus);

        if (enemyUnit.Count != 0)
        {
            agent.isStopped = true;
            //aniStaus = AniStatus.IDLE;
        }

        if (enemy_Hp <=  0 )
        {
            aniStaus = AniStatus.DIE;
            hpSlider.gameObject.SetActive(false);

            if (isAlpha == true)
            {
                alphaValue -= Time.deltaTime * 02f;
                foreach(SkinnedMeshRenderer mesh in enemyMeshList)
                {
                    mesh.material.color = new Color(mesh.material.color.r, mesh.material.color.g, mesh.material.color.b, alphaValue);
                }
            }
        }

        if(alphaValue <= 0 && isAlphaDone == false)
        {
            moneyParticle.Play(true);
            isAlphaDone = true;
        }
        if(isAlphaDone == true)
        {
            StartCoroutine("MoneyEffect");
        }

        // 만약 타겟이 생기면 타겟한테 향하자
        if (enemy_Hp > 0)
        {
            if (isTarget)
            {
                float moveSpeed = 2.0f;

                transform.LookAt(target.transform);

                 Vector3 dirToTarget = enemyUnit[0].transform.position - this.transform.position;
                 Vector3 look = Vector3.Slerp(this.transform.forward, dirToTarget.normalized, Time.deltaTime);
                 this.transform.rotation = Quaternion.LookRotation(look, Vector3.up);

                //if (Vector3.Distance(target.transform.position, transform.position) > 13f)
                //{
                //    aniStaus = AniStatus.WALK;
                //    transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
                //}

                //if (Vector3.Distance(target.transform.position, transform.position) <= 13f)
                //{
                   aniStaus = AniStatus.ATTACK;
                //}
            }
        }
        else isTarget = false;

        if(target != null)
        {
            if(target.GetComponent<UnitBase>().Unit_Hp <= 0)
            {
                aniStaus = AniStatus.IDLE;
            }
        }

        if(enemyUnit.Count == 0)
        {
            agent.isStopped = false;
            aniStaus = AniStatus.WALK;
        }
    }

    #region Animation 관련
    // 애니메이션 status를 바꾸는 상황
    protected void StatusChange(Vector3 _pos)
    {
        // 주변에 있는 모든 unit 적을 추출하여 배열에 저장
        Collider[] colls = Physics.OverlapSphere(_pos, 3f, 1 << 9);
        Collider[] CheckAround = Physics.OverlapSphere(_pos, 3f, 1 << 9);

        for(int i = 0; i < colls.Length; ++i)
        {
          // Unit 콜라이더를 담고 잇는 녀석에게 "AttackUnit"이라는 함수를 가동시켜라.
            if(enemy_Hp > 0)
            {
                colls[i].SendMessage("UnitStatus");
                colls[i].SendMessage("UnitAnimStatus");
              //  aniStaus = AniStatus.ATTACK;
               
            }
            if (colls[i].GetComponent<UnitBase>().Unit_Hp <= 0f)
            {
                enemyUnit.Remove(colls[i].gameObject);
                aniStaus = AniStatus.IDLE;
            }
        }

        for (int i = 0; i < CheckAround.Length; ++i)
        {
            if(enemyUnit.Count == 0)
            {
                enemyUnit.Add(CheckAround[i].gameObject);
            }

            for(int j = 0; j < enemyUnit.Count; j++)
            {
                if(enemyUnit[j].name != CheckAround[i].name)
                {
                    enemyUnit.Add(CheckAround[i].gameObject);
                }
            }

            target = CheckAround[0].gameObject;
             isTarget = true;
            //}
        }
        if (CheckAround.Length == 0)
        {
            enemyUnit.Clear();
            isTarget = false;
        }
    }

    protected void AnimStatus(AniStatus _animStatus)
    {
        switch(_animStatus)
        {
            case AniStatus.IDLE:
                animator.SetBool("isWalk", false);
                animator.SetBool("isAttack",false);

                break;

            case AniStatus.ATTACK:
                animator.SetBool("isWalk", false);
                animator.SetBool("isAttack",true);

                break;

            case AniStatus.DIE:
                animator.SetBool("isDeath", true);
                StartCoroutine("DeathUnit");
                break;

            case AniStatus.WALK:
                animator.SetBool("isWalk", true);
                break;
        }
    }
    #endregion Animation 관련

    #region playerUnit에 sendMessage 보내는 함수
    private void OnTriggerEnter(Collider other)
    {
        // enemy의 hp가 0 이상일때만 공격 신호를 보낸다.
        if(enemy_Hp > 0)
        {
            if(other.gameObject.tag == "PlayerUnit")
            {
                // Unit 콜라이더를 담고 잇는 녀석에게 "AttackUnit"이라는 함수를 가동시켜라.
                other.SendMessage("UnitStatus");
                other.SendMessage("UnitAnimStatus");
                aniStaus = AniStatus.ATTACK;

                Debug.Log("playerUnit을 발견 했다.");
            }
        }

        if(other.gameObject.tag == "PlayerTower")
        {
            aniStaus = AniStatus.ATTACK;
        }
    
    }
    #endregion playerUnit에 sendMessage 보내는 함수

    #region UnitWeaponeCollider

    public void EnemyDamage(int _unitDamage)
    {
        enemy_Hp -= _unitDamage;
        hpSlider.GetComponent<Slider>().value = (enemy_Hp * fillAmountHp) / 100;
        Debug.Log("enemyHp : " + enemy_Hp);
    }

   public IEnumerator DeathUnit()
   {
        yield return new WaitForSeconds(1.7f);
        isAlpha = true;
   }


    #endregion UnitWeaponeCollider

    #region enemyUnit이 죽었을 때 &&  money 증가
    IEnumerator MoneyEffect()
    {
        yield return new WaitForSeconds(1.5f);
        moneyParticle.Stop(true);
        GameManager.instance.Money += enemy_DeathMoney;
        isAlphaDone = false;
        Destroy(transform.parent.gameObject);
    }



    #endregion enemyUnit이 죽었을 때 &&  money 증가

    #region 오브젝트 pool
    
    IEnumerator DestroyCube()
    {
        yield return null;
        ObjectPool.instance.InsertQueue(gameObject);
    }


    #endregion 오브젝트 pool

}
