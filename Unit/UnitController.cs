using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitController : MonoBehaviour
{
    private NavMeshAgent navAgent;
    private Transform currentTarget;
    private Transform minusCurrentTarget;
    private Outline outline;

    public bool isWalk;

    private void Start()
    {
        outline = GetComponent<Outline>();
        navAgent = GetComponent<NavMeshAgent>();
        isWalk = false;
    }

    private void Update()
    {
        if(currentTarget != null)
        {
            minusCurrentTarget.position = new Vector3(currentTarget.position.x, currentTarget.position.y, currentTarget.position.z);
            // navAgent.destination -> 타겟 대상의 위치 설정. 위치 설정시 바로 추적 시작.
            navAgent.destination = minusCurrentTarget.position;
            
        }

        if (Vector3.Distance(navAgent.destination, this.transform.position) <= 1.5f)
        {   // 현재 이동 위치 와 유닛의 거리가 1.5f 차이난다면 walk애니메이션을 꺼라
            isWalk = false;
        }

    }

    public void MoveUnit(Vector3 dest)
    {
        currentTarget = null;
        navAgent.destination = dest;
        isWalk = true;
    }

    public void SetSelected(bool isSelected)
    {
        outline.enabled = isSelected;
        transform.Find("Highlight").gameObject.SetActive(isSelected);
    }

    public void SetNewTarget(Transform enemy)
    {
        currentTarget = enemy;
    }

}
