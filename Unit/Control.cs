using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control : MonoBehaviour
{
    private Behavior behavior; // 캐릭터의 행동 스크립트
    private Camera mainCamera; // 메인 카메라
    private Vector3 targetPos; // 캐릭터의 이동 타켓 위치

    public bool isIdle { get; set;}        // Idle 상태 받아오기

    private void Start()
    {
        behavior = this.GetComponent<Behavior>();
        mainCamera = GameObject.Find("Camera").GetComponent<Camera>();
        isIdle = true;
    }

    private void Update()
    {
        // 마우스 클릭으로 오브젝트 받기



        // 마우스로 찍은 위치의 좌표 값을 가져온다.
        if(Input.GetMouseButtonDown(1))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, 10000f))
            {
                targetPos = hit.point;
                isIdle = false;
            }
        }

        // 캐릭터가 움직이고 있다면
        if(isIdle == false)
        {
            if (behavior.Run(targetPos))
            {
                // 회전도 같이 해준다.
                behavior.Turn(targetPos);
            }
        }
        else
        {
            isIdle = true;
        }
     
        
   

    }

}
