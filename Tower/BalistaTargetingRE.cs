using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BalistaTargetingRE : MonoBehaviour
{
    public bool getATarget = false;
    public LayerMask layerMask;     // 타겟팅 할 레이어
    public float radius;            // 반지름의 길이
    public float bulletMoveSpeed;   // 불렛의 속도

    public List<GameObject> UnitList = new List<GameObject>();
    // Monster를 담는 List

    //==========================//
    //       bullet 발사        //
    //==========================//
    public GameObject bullet = null;        // 총알 프리팹
    public Transform firePos = null;       // 발사 위치
    public ParticleSystem fireParticle;
    public ParticleSystem boomParticle;
    public ParticleSystem moneyParticle;

    [SerializeField] private float fireTimeMin = 0f;    // 발사 주기(최소)
    [SerializeField] private float fireTimeMax = 1.5f;  // 발사 주기(최대)  // 5초 마다 쏘겠다.

    // 타워 Hp
    public float tower_Hp;
    private Slider hpSlider;

    private GameObject towerBody;
    private GameObject rockObject;
    private GameObject hpCanvas;
    private bool isOnce;

    private GameObject aBolt;

    public GameObject target;
    Transform bulletRotation;
    Transform originBulletTrans;
    Transform rotateTrans;

    // bullet 담는 list 
    public List<GameObject> bulletList = new List<GameObject>();

    private float rotateSpeed = 30.0f;

    private void OnDrawGizmos()
    {
        if (getATarget)
        {
            for (int i = 0; i < UnitList.Count; i++)
            {
                RaycastHit hit;
                bool isHit = Physics.Raycast(transform.position, UnitList[i].transform.position - transform.position,
                                             out hit, 20f, layerMask);

                // 수정할 부분이 많음
                if (isHit && hit.transform.CompareTag("PlayerUnit"))
                {
                    Gizmos.color = Color.green;
                }
                else
                {
                    Gizmos.color = Color.red;
                }
                Gizmos.DrawRay(transform.position, UnitList[i].transform.position - transform.position);
            }
        }
    }

    private void Start()
    {
        bulletMoveSpeed = 30f;
        tower_Hp = 30f;
        hpCanvas = this.gameObject.transform.parent.parent.parent.Find("HpCanvas_Tower").gameObject;
        hpSlider = this.gameObject.transform.parent.parent.parent.Find("HpCanvas_Tower").GetComponentInChildren<Slider>();
        towerBody = this.gameObject.transform.parent.Find("TowerBody").gameObject;
        isOnce = false;
        rockObject = this.gameObject.transform.parent.Find("rock_pile_big_02_LOD_Group").gameObject;
        originBulletTrans = this.transform;
    }

    private void Update()
    {
        NearUnitAttack(transform.position);

        // 만약에 unit이 검출이 안될땐 45씩 움직여 준다.
        if (UnitList.Count == 0)
        {
            this.transform.Rotate(new Vector3(0, 45, 0) * Time.deltaTime);
            //this.transform.rotation.x = 0f;
            getATarget = false;
            for(int i = 0; i < bulletList.Count; i++)
            {
                Destroy(bulletList[i].gameObject);
            }
            bulletList.Clear();
        }

        if (UnitList.Count != 0)
        {
            getATarget = true;
            // 포탄 발사 //
            fireTimeMin += Time.deltaTime;  // 발사주기 갱신

            target = UnitList[0];            //첫번째로 충돌한 객체를 타겟으로 넣는다.

            if (target != null)
            {
                // 최단 거리에 있는 유닛을 바라 보게 한다.
                Vector3 dirToTarget = target.transform.position - this.transform.position;
                Vector3 look = Vector3.Slerp(this.transform.forward, dirToTarget.normalized, Time.deltaTime);
                this.transform.rotation = Quaternion.LookRotation(look, Vector3.up);

                // 최단 거리에 있는 유닛과 포지션 z 와 비슷 해지면 발사 한다.

                if (fireTimeMin > fireTimeMax)
                {
                     this.transform.Find("GB_ballista_bullet2").GetComponent<MeshRenderer>().enabled = false;

                    fireParticle.Play(true);
                    fireTimeMin = 0.0f;

                    // 미사일 생성
                    aBolt = Instantiate(bullet, firePos.position, this.transform.rotation, transform);
                    bulletList.Add(aBolt);
                    var arrow = aBolt.GetComponent<Arrow>();
                    var rigidbody = aBolt.GetComponent<Rigidbody>();
                    Vector3 vecAddPos = (firePos.forward * bulletMoveSpeed);
                    arrow.m_target = target;                    // 타겟의 정보를 넣어준다.
                    aBolt.transform.Rotate(new Vector3(10, 0, 0));

                }

                if (aBolt != null && aBolt.GetComponent<Arrow>().isConnect == false)
                {
                    bullet.transform.position -= (firePos.forward * bulletMoveSpeed * Time.deltaTime);      // 포탄위치 박아주기
                    aBolt.transform.position -= (firePos.forward * bulletMoveSpeed * Time.deltaTime);      // 포탄위치 박아주기
                }

            }
            if (target == null)  // 타겟이 없으면 리스트의 첫번째에 담은 녀석을 지운다.
            {
                UnitList.Remove(target);
            }
        }

        if (hpSlider.value <= 0)
        {
            Debug.Log("hp = 0");
            if (isOnce == false)
            {
                boomParticle.Play(true);
                isOnce = true;
            }
            this.gameObject.SetActive(false);
            towerBody.gameObject.layer = LayerMask.NameToLayer("Default");
            this.transform.parent.Find("Balista_Body").gameObject.SetActive(false);
            rockObject.SetActive(true);
            hpCanvas.SetActive(false);
        }
    }

    public void NearUnitAttack(Vector3 Pos)
    {
        Collider[] colls = Physics.OverlapSphere(Pos, radius, 1 << 9);

        for (int i = 0; i < colls.Length; i++)
        {
            if (UnitList.Count == 0)
            {
                UnitList.Add(colls[i].gameObject);
            }
        }
    }
}
