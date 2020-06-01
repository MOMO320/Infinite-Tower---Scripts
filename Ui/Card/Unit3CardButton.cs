using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit3CardButton : MonoBehaviour
{
    public void Unit3Card()
    {
        UiManager.instance.UnitCardButton(GameManager.instance.Lv1_Units[2],
                                       GameManager.instance.Lv1_Icons[2],
                                       GameManager.instance.Lv1_Units[2].GetComponentInChildren<UnitBase>().Unit_Price,
                                       GameManager.instance.Lv1_Units[2].GetComponentInChildren<UnitBase>().Unit_Count);


        UiManager.instance.UnitIconPosition(UiManager.instance.iconTrans[GameManager.instance.Lv1_ChooseIcon.Count - 1],
            GameManager.instance.Lv1_Icons[2]);
    }

    public void Unit3Descript()
    {
        UiManager.instance.DescriptText("창사", 2, 30, 3, 2.5f, "몬스터 공격시 + 1", "타워 공격시 - 2");
    }
}
