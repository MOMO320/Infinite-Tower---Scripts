using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitArrow : MonoBehaviour
{
    public GameObject arrowObject;  // 화살 프리팹
    public Transform arrowTrans;    // 화살 발사 위치
    private GameObject acherUnit;   // 아쳐 유닛

    public List<GameObject> arrowList = new List<GameObject>();
    public List<GameObject> EnemyUnitList = new List<GameObject>();

    public float fireTimeMin = 0f;  // 발사 주기(최소)
    public float fireTimeMax = 2.0f;// 발사 주기(최대)

    public float arrowSpeed = 5.0f;

    public float firingAngle = 45f;  // 각도
    public float gravity;      // 중력값
    public Transform Projectile;

    [SerializeField] private Transform Target;
    private GameObject aBolt;

    private Transform myTransform;

    private void Awake()
    {
       // acherUnit = this.transform.parent.gameObject;
        gravity = 9.8f;
        firingAngle = 45f;
        myTransform = transform;
    }

    float Radian(float degree)
    {
        return degree * Mathf.PI / 180.0f;
    }

    private void Start()
    {
        StartCoroutine("SimulateProject");
    }

    private void FixedUpdate()
    {
        fireTimeMin += Time.deltaTime;  // 발사 주기 갱신
        NearEnemy(this.transform.position);

        if(EnemyUnitList.Count != 0)
        {
            Target = EnemyUnitList[0].transform;
        }
        
    }

    protected void NearEnemy(Vector3 _pos)
    {
        // 주변에 있는 모든 unit 적을 추출하여 배열에 저장
        Collider[] colls = Physics.OverlapSphere(_pos, 8f, 1 << 10);


        for (int i = 0; i < colls.Length; ++i)
        {
            // Unit 콜라이더를 담고 잇는 녀석에게 "AttackUnit"이라는 함수를 가동시켜라.
            if(EnemyUnitList.Count == 0)
            {
              EnemyUnitList.Add(colls[i].gameObject);
            }

            if(EnemyUnitList.Count >0)
            {
                for(int j = 0;  j < EnemyUnitList.Count; j++)
                {
                    if(EnemyUnitList[j].name != colls[i].name)
                    {
                        EnemyUnitList.Add(colls[i].gameObject);
                    }

                }
            }
        }

    }

    public IEnumerator SimulateProject()
    {

        yield return new WaitForSeconds(1.5f);
        Projectile.position = myTransform.position + new Vector3(0, 0.0f, 0);

        float target_Distance = Vector3.Distance(Projectile.position, Target.position);

        float projectile_Velocity = target_Distance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / gravity);

        float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
        float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);

        float flightDuration = target_Distance / Vx;

       // Projectile.transform.LookAt(Target.transform.position);
         //Projectile.rotation = Quaternion.LookRotation(Target.position - Projectile.position);

        float elapse_time = 0;

        while(elapse_time < flightDuration)
        {
            Projectile.Translate( -Vx *  Time.deltaTime, (Vy - (gravity * elapse_time)) *  Time.deltaTime, Vx  * Time.deltaTime);

            elapse_time += Time.deltaTime;

            yield return null;
        }


    }
}
