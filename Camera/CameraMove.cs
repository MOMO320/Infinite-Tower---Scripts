using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public bool isMoveCamera = false;

    private void Update()
    {
        CameraMouseMove();
    }

    private void CameraMouseMove()
    {
        if (Input.GetMouseButton(2))
        {
            float t_posX = Input.GetAxis("Mouse X");
            float t_posZ = Input.GetAxis("Mouse Y");
            transform.position += new Vector3(t_posX, 0, t_posZ);
            isMoveCamera = true;
        }
        else isMoveCamera = false;
    }
}
