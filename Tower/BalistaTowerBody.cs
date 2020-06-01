using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BalistaTowerBody : MonoBehaviour
{
    public Slider hpSlider;
    private BalistaTargetingRE towerInfo;

    private float hpRate;

    private void Awake()
    {
        hpRate = 0f;
       // hpSlider = this.transform.parent.Find("HpCanvas_Tower").GetComponentInChildren<Slider>();
        towerInfo = this.transform.parent.Find("Balista_Head").GetComponent<BalistaTargetingRE>();
    }

    private void Start()
    {

        hpRate = 100 / towerInfo.tower_Hp;
    }

    private void Update()
    {
        hpSlider.value = (hpRate * towerInfo.tower_Hp) / 100f;
    }

    public void TowerDamage(float _damage)
    {
        towerInfo.tower_Hp -= _damage;
    }

}
