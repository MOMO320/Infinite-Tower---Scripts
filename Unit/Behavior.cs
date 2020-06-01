using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behavior : MonoBehaviour
{
    public float speed = 2.0f;
    public Control control;
    private Rigidbody rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        control = this.GetComponent<Control>();
    }

    public bool Run(Vector3 targetPos)
    {
        //이동하고자 하는 좌표 값과 현재 내위치의 차이를 구한다.
        float dis = Vector3.Distance(transform.position, targetPos);
        if (dis >= 0.5f) // 차이가 아직 있다면
        {
            transform.Translate(transform.forward * speed * Time.deltaTime);
            return true;
        }
        else if(dis < 0.5f)
        {
            control.isIdle = true;
        }
        return false;
    }

    public void Turn(Vector3 targetPos)
    {
        // 캐릭터를 이동하고자 하는 좌표값 방향으로 회전시킨다.
        Vector3 dir = targetPos - transform.position;
        Vector3 dirXZ = new Vector3(dir.x, 0f, dir.z);
        Quaternion targetRot = Quaternion.LookRotation(dirXZ);
        rigid.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, 550.0f * Time.deltaTime);
    }
}
