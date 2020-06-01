using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticFunction
{
    enum UnitAniStatus
    {
        IDEL , ATTACK , DIE
    }

    public static void LookRotate
    (Transform ToTrans, Transform fromTras , float turnSpeed)
    {
        fromTras.rotation = Quaternion.Slerp
            (fromTras.rotation, ToTrans.rotation,
            Time.deltaTime * turnSpeed);
    }

    
}
