using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitSpawnSlide : MonoBehaviour
{
    private float oneSecond;

    private void Start()
    {
        oneSecond = 100 / GameManager.instance.selectSlotUnit[0][0].GetComponentInChildren<UnitBase>().Unit_SpawnTime;//GetComponentInChildren<UnitBase>().Unit_SpawnTime;
    }

    private void Update()
    {
        UiManager.instance.UnitSpawnTime(this.GetComponent<Slider>(), oneSecond);

        if(this.GetComponent<Slider>().value <= 0)
        {
            GameManager.instance.isUnitOut = true;
            GameManager.instance.SpwanUnit(GameManager.instance.selectSlotUnit[GameManager.instance.Lv1_ChooseIcon.Count-1], GameManager.instance.Lv1_ChooseIcon);
            Destroy(this.transform.parent.gameObject);
        }
    }

}
