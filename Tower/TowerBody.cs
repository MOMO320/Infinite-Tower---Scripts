using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerBody : MonoBehaviour
{
    public Slider hpSlider;
    private TowerTargetingRE towerInfo;

    private float hpRate;
    
    private void Awake()
    {
        hpRate = 0f;
        hpSlider = this.transform.parent.Find("HpCanvas_Tower").GetComponentInChildren<Slider>();
        towerInfo = this.transform.parent.Find("SCALED").GetComponentInChildren<TowerTargetingRE>();
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

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "UnitWeapone")
        {
        }
    }
}
