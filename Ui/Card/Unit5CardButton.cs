using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit5CardButton : MonoBehaviour
{
    public void Unit5Card()
    {
        UiManager.instance.UnitCardButton(GameManager.instance.Lv1_Units[4],
                                          GameManager.instance.Lv1_Icons[4],
                                          GameManager.instance.Lv1_Units[4].GetComponentInChildren<UnitBase>().Unit_Price,
                                          GameManager.instance.Lv1_Units[4].GetComponentInChildren<UnitBase>().Unit_Count);


        UiManager.instance.UnitIconPosition(UiManager.instance.iconTrans[GameManager.instance.Lv1_ChooseIcon.Count - 1],
            GameManager.instance.Lv1_Icons[4]);
    }

    public void Unit5Descript()
    {
        UiManager.instance.DescriptText("투석기",1, 150, 20, 1.2f, "타워 공격시 + 3", "몬스터 공격시 -2");
    }
}
