using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UnitBase : MonoBehaviour, IRotateLookAt
{
    // unit 요소 ( Hp / Attack / SpawnTime / Count(용병 갯수)
    public float Unit_Hp = 10;
    public int Unit_Attack;
    public float Unit_SpawnTime;
    public int Unit_Count;
    public int Unit_Price;

    // hp값에 따른 hpBar값
    public Slider hpSlider;
    private float fillAmountHp;

    // 바라보는 방향으로 전환
    private Transform from;
    private Transform to;
    private float turnSpeed = 0.1f;

    // 가장 가까운 enemyUnit
    private Transform closeEnemyTrans;
    private float FirstDistance;          // 비교를 위한 Distance 변수1
    private float SecondDistance = 100.0f;  // 비교를 위한 Distance 변수2
    
    // 가까이에 있는 Enemy탐색
    public List<GameObject> EnemyList = new List<GameObject>();
    private bool getATarget = false;
    private float currentDist = 0;     // 현재 거리
    private float closeDist = 100f;    // 가까운 거리
    private float targetDist = 100f;   // 타겟 거리
    private int closeDistIndex = 0;    // 가장 가까운 인덱스
    private int targetIndex = -1;      //  타겟팅 할 인덱스
    public LayerMask layerMask = 1 << 10;     // 타겟팅 할 레이어    

    private UnitController unitController;

    // Unit이 죽으면 alpha 랜더 해준다.
    private SkinnedMeshRenderer[] unitMeshList;
    private float alphaValue = 1f;
    private bool isAlpha = false;

    // 가까이에 있는 tower탐색
    public List<GameObject> towerList = new List<GameObject>();
    private bool isTarget= false;
    public float towerDistance = 5.0f;


    // 죽으면 부모 오브젝트를 꺼준다.
    private GameObject parentObject;
    private bool isDeath = false;


    public enum AniStatus
    {
        IDLE , WALK , ATTACK , DIE
    }
    // 애니메이션 연결
    public Animator animator;
    public AniStatus aniStaus { get; set; }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        aniStaus = AniStatus.IDLE;
        unitController = GetComponent<UnitController>();
        unitMeshList = GetComponentsInChildren<SkinnedMeshRenderer>();
        layerMask = 1 << 10;
       // parentObject = GetComponentInParent<GameObject>();
    }

    private void OnEnable()
    {
        fillAmountHp = 100 / Unit_Hp; 
    }


    //private void OnDrawGizmos()
    //{
    //    if (getATarget)
    //    {
    //        for (int i = 0; i < EnemyList.Count; i++)
    //        {
    //            RaycastHit hit;
    //            bool isHit = Physics.Raycast(transform.position, EnemyList[i].transform.position - transform.position,
    //                                         out hit, 20f, layerMask);

    //            // 수정할 부분이 많음
    //            if (isHit && hit.transform.CompareTag("EnemyUnit"))
    //            {
    //                Gizmos.color = Color.green;
    //            }
    //            else
    //            {
    //                Gizmos.color = Color.red;
    //            }
    //            Gizmos.DrawRay(transform.position, EnemyList[i].transform.position - transform.position);
    //        }
    //    }

    //    for (int i = 0; i < towerList.Count; i++)
    //    {
    //        RaycastHit hit;
    //        bool isHit = Physics.Raycast(transform.position, towerList[i].transform.position - transform.position,
    //                                     out hit, 20f, 1 << 12);

    //        // 수정할 부분이 많음
    //        if (isHit && hit.transform.CompareTag("Cannon") || isHit && hit.transform.CompareTag("Balista"))
    //        {
    //            Gizmos.color = Color.green;
    //        }
    //        else
    //        {
    //            Gizmos.color = Color.red;
    //        }
    //        Gizmos.DrawRay(transform.position, EnemyList[i].transform.position - transform.position);
    //    }
    //}



    private void FixedUpdate()
    {
        RotateLookAt(this.transform.position);
        NearEnemy(transform.position);
        
        if(unitController.isWalk == false && aniStaus != AniStatus.ATTACK)
        {
            aniStaus = AniStatus.IDLE;
        }
        if(unitController.isWalk == true)
        {
            aniStaus = AniStatus.WALK;
        }

        if (Unit_Hp <= 0 && alphaValue > 0)
        {
            aniStaus = AniStatus.DIE;
            this.gameObject.GetComponent<UnitController>().SetSelected(false);
            
            if (isAlpha == true)
            {
                alphaValue -= Time.deltaTime * 2f;
                foreach (SkinnedMeshRenderer mesh in unitMeshList)
                {
                    mesh.material.color = new Color(mesh.material.color.r, mesh.material.color.g, mesh.material.color.b, alphaValue);
                }
            }
        }
        if (alphaValue <= 0)
        {
            this.gameObject.SetActive(false);
     
            if(isDeath == false)
            {
                GameManager.instance.UnitCount -= 1;
                isDeath = true;
            }
            Destroy(transform.parent.gameObject);
        }

        UnitAnimStatus();


        // 가까운적 타겟팅
        if(EnemyList.Count != 0)
        {
            currentDist = 0f;
            closeDistIndex = 0;
            targetIndex -= 1;

            for(int i = 0; i < EnemyList.Count; i++)
            {
                currentDist = Vector3.Distance(transform.position, EnemyList[i].transform.position);

                RaycastHit hit;
                bool isHit = Physics.Raycast(transform.position, EnemyList[i].transform.position - transform.position,
                                    out hit, 20f, layerMask);

                if(isHit && hit.transform.CompareTag("EnemyUnit"))
                {
                    if(targetDist >= currentDist)
                    {
                        targetIndex = i;
                        targetDist = currentDist;
                    }
                }

                if(closeDist >= currentDist)
                {
                    closeDistIndex = i;
                    closeDist = currentDist;
                }
            }

            if(targetIndex == -1)
            {
                targetIndex = closeDistIndex;
            }

            closeDist = 100f;
            targetDist = 100f;
            getATarget = true;
        }

        if(getATarget)
        {
            // 최단 거리에 있는 유닛을 바라 보게 한다.
            //Vector3 dirToTarget = EnemyList[targetIndex].transform.position - this.transform.position;
           //Vector3 look = Vector3.Slerp(this.transform.forward, dirToTarget.normalized, Time.deltaTime);
          //  this.transform.rotation = Quaternion.LookRotation(look, Vector3.up);

             transform.LookAt(new Vector3(EnemyList[targetIndex].transform.position.x,
            transform.position.y, EnemyList[targetIndex].transform.position.z));
        }

        if(towerList.Count != 0)
        {
            if(towerList[0].tag == "Cannon")
            {
                if(towerList[0].transform.parent.Find("SCALED").GetComponentInChildren<TowerTargetingRE>().tower_Hp <= 0)
                {
                    towerList.Clear();
                    aniStaus = AniStatus.IDLE;
                }
            }

            if(towerList[0].tag == "Balista")
            {
                if (towerList[0].transform.parent.Find("Balista_Head").GetComponent<BalistaTargetingRE>().tower_Hp <= 0)
                {
                    towerList.Clear();
                    aniStaus = AniStatus.IDLE;
                }
            }
        }

        if(EnemyList.Count == 0)
        {
            aniStaus = AniStatus.IDLE;
        }

        NearTower(transform.position);
        TowerAttack(transform.position);
    }


    public void RotateLookAt(Vector3 _pos)
    {
        Collider[] colls = Physics.OverlapSphere
                            (_pos, 0.3f, 1 << 10);

        for(int i = 0; i < colls.Length; ++i)
        {   // enemy Unit에게 공격을 명령함
            colls[i].SendMessage("AnimStatus", AniStatus.ATTACK);
            // enemy중 Unit과 가장 가까운 애를 바라보기 위해 비교 해준다.
            FirstDistance = Vector3.Distance(colls[i].transform.position,
                                        this.transform.position);
           
            // 만약 기존 FisrstDistance거리보다 SecondDistance짧다면 바꿔준다.
            if(FirstDistance < SecondDistance)
            {
                SecondDistance = FirstDistance;
                closeEnemyTrans = colls[i].transform;
                transform.LookAt(closeEnemyTrans);
            }
        }
    }

    #region Animation 관련

    public void UnitStatus()
    {
        aniStaus = AniStatus.ATTACK;
    }

    public void UnitAnimStatus()
    {
        switch (aniStaus)
        {
            case AniStatus.IDLE:
                animator.SetBool("isWalk", false);
                animator.SetBool("isAttack", false);
                break;

            case AniStatus.WALK:
                animator.SetBool("isWalk", true);
                break;

            case AniStatus.ATTACK:
                animator.SetBool("isWalk", false);
                animator.SetBool("isAttack", true);
                break;

            case AniStatus.DIE:
                animator.SetBool("isDeath", true);
                StartCoroutine("DeathUnit");
                break;
        }
    }

    #endregion Animation 관련

    #region hp관련
    public void UnitDamage(int _enemyDamage)
    {
        Unit_Hp -= _enemyDamage;
        hpSlider.GetComponent<Slider>().value = (Unit_Hp * fillAmountHp) / 100;
       // Debug.Log("UnitHp : " + Unit_Hp);
    }

    public IEnumerator DeathUnit()
    {
        yield return new WaitForSeconds(1.2f);
        
        isAlpha = true;
    }
    #endregion hp관련

    #region 유닛이 적 바라보게 하기

     // 가까이에 있는 enemy유닛 탐색
     private void NearEnemy(Vector3 Pos)
     {
        Collider[] colls = Physics.OverlapSphere(Pos,5f, 1 << 10);

        for(int i = 0; i < colls.Length; i++)
        {
            EnemyList.Add(colls[i].gameObject);
        }

        if(colls.Length == 0)
        {
            EnemyList.Clear();
        }
     }

    // 가까이에 있는 Tower 탐색
    private void NearTower(Vector3 Pos)
    {
        Collider[] colls = Physics.OverlapSphere(Pos, 7f, 1 << 12);

        for(int i = 0; i < colls.Length; i++)
        {
            if(towerList.Count == 0)
            towerList.Add(colls[i].gameObject);
        }

        if(colls.Length == 0)
        {
            towerList.Clear();
        }
    }


    #endregion 유닛이 적 바라보게 하기

    #region tower공격 하기

    private void TowerAttack(Vector3 pos)
    {
        if (towerList.Count != 0)
        {
            // 가까운적 타겟팅
            float currentDist = 0f;

            for (int i = 0; i < towerList.Count; i++)
            {
                currentDist = Vector3.Distance(pos, towerList[i].transform.position);

                if(currentDist < towerDistance)
                {
                    isTarget = true;
                }
            }
        }

        if (isTarget)
        {
            // 최단 거리에 있는 유닛을 바라 보게 한다.

            Vector3 dirToTarget = towerList[0].transform.position - this.transform.position;
            Vector3 look = Vector3.Slerp(this.transform.forward, dirToTarget.normalized, Time.deltaTime);
            this.transform.rotation = Quaternion.LookRotation(look, Vector3.up);


            //transform.LookAt(new Vector3(towerList[0].transform.position.x,
            //                             pos.y, 
            //                             towerList[0].transform.position.z));
            aniStaus = AniStatus.ATTACK;
        }
    }
       
    #endregion tower공격 하기

}
