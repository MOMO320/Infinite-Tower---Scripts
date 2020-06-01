using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindOfUnit : MonoBehaviour
{

    // 몬스터의 정보를 담을 List
    List<GameObject> UnitListInRoom = new List<GameObject>();
    public bool UnitInThisRoom = false;
    public bool isClearRoom = false;

    private void Start()
    {
        
    }

    private void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "PlayerUnit")
        {
            UnitInThisRoom = true;
            
        }

       // if(other.gameObject)
    }
}
