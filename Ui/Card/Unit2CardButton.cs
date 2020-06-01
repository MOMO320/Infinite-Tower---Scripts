using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit2CardButton : MonoBehaviour
{
    public void Unit2Card()
    {
        UiManager.instance.UnitCardButton(GameManager.instance.Lv1_Units[1],
                                   GameManager.instance.Lv1_Icons[1],
                                   GameManager.instance.Lv1_Units[1].GetComponentInChildren<UnitBase>().Unit_Price,
                                   GameManager.instance.Lv1_Units[1].GetComponentInChildren<UnitBase>().Unit_Count);


        UiManager.instance.UnitIconPosition(UiManager.instance.iconTrans[GameManager.instance.Lv1_ChooseIcon.Count - 1],
            GameManager.instance.Lv1_Icons[1]);
    }

    public void Unit2Descript()
    {
        UiManager.instance.DescriptText("궁사",2, 25, 4, 3, "타워 공격 시 + 1", "몬스터 공격 시 - 1");
    }
}
