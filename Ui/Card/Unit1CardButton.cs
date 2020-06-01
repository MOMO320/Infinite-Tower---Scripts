using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class Unit1CardButton : MonoBehaviour
{
    private GameObject ParentCanvas;
    private Animation DescriptAni;
    public string name;

    private void OnEnable()
    {
       ParentCanvas = this.transform.parent.transform.parent.gameObject;
        ParentCanvas.transform.Find("UnitDescrition").gameObject.SetActive(true);
        DescriptAni = ParentCanvas.transform.Find("UnitDescrition").GetComponent<Animation>();
        DescriptAni.CrossFade("PaperUpAni");
    }

    public void Unit1Card()
    {
        UiManager.instance.UnitCardButton(GameManager.instance.Lv1_Units[0],
                                          GameManager.instance.Lv1_Icons[0],
                                          GameManager.instance.Lv1_Units[0].GetComponentInChildren<UnitBase>().Unit_Price,
                                          GameManager.instance.Lv1_Units[0].GetComponentInChildren<UnitBase>().Unit_Count);


        UiManager.instance.UnitIconPosition(UiManager.instance.iconTrans[GameManager.instance.Lv1_ChooseIcon.Count-1],
            GameManager.instance.Lv1_Icons[0]);
    }

    public void Unit1Descript()
    {
        UiManager.instance.DescriptText("병사",3 ,15, 2,2, "없음", "없음");
    }
}
