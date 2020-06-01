using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    public Outline outLine;

    void Start()
    {
        outLine = GetComponent<Outline>();    
    }

    // Update is called once per frame
    public void SetSelected(bool _isSelected)
    {
        if(UiManager.instance.isSelected == false)
        {
            outLine.enabled = _isSelected;
            UiManager.instance.isSelected = _isSelected;
            UiManager.instance.UiBuyActive(_isSelected);
        }
    }
}
