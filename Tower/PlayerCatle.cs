using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCatle : MonoBehaviour
{
    [SerializeField]
    private float Hp = 1000;
    private Slider hpCanvasObj;
    private float hpRate;

    private void Start()
    {
        hpCanvasObj = this.transform.parent.Find("HpCanvas_Catle").GetComponentInChildren<Slider>();
        Hp = 1000;
        hpRate = 100 / Hp;
    }

    private void FixedUpdate()
    {
        hpCanvasObj.value = (Hp * hpRate) / 100;
    }

    public void CastleDamage(int damage)
    {
        Hp -= damage;
    }
}
