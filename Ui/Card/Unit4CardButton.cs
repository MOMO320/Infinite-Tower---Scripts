using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit4CardButton : MonoBehaviour
{
    public void Unit4Card()
    {
        UiManager.instance.UnitCardButton(GameManager.instance.Lv1_Units[3],
                                          GameManager.instance.Lv1_Icons[3],
                                          GameManager.instance.Lv1_Units[3].GetComponentInChildren<UnitBase>().Unit_Price,
                                          GameManager.instance.Lv1_Units[3].GetComponentInChildren<UnitBase>().Unit_Count);


        UiManager.instance.UnitIconPosition(UiManager.instance.iconTrans[GameManager.instance.Lv1_ChooseIcon.Count - 1],
            GameManager.instance.Lv1_Icons[3]);
    }

    public void Unit4Descript()
    {
        UiManager.instance.DescriptText("마병",1, 60, 5, 5, "몬스터 공격시 + 2", "타워 공격시 - 1");
    }
}
