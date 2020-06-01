using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickOn : MonoBehaviour
{
    private MeshRenderer myRend;
    private Outline outLine;


    [HideInInspector]
    public bool currentlySelected = false;

    private void Start()
    {
        outLine = GetComponent<Outline>();
        Camera.main.gameObject.GetComponent<Click>().selectableObjects.Add(this.gameObject);
        ClickMe();
    }

    public void ClickMe()
    {
        if(currentlySelected == false)
        {
            outLine.enabled = false;
        }
        else
        {
            outLine.enabled = true;
        }
    }

}
