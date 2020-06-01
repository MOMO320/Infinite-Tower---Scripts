using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCatle : MonoBehaviour
{
    public Transform spawnTrans;
    public GameObject goblinObj;
    public Transform goalTrans;

    public float minTime;
    public float maxTime;

    private List<GameObject> goblinObjList = new List<GameObject>(); 

    private void Awake()
    {
        minTime = 0f;
        maxTime = 5f;
       
    }

    private void FixedUpdate()
    {
        minTime += Time.deltaTime;  
        
        if(minTime > maxTime)
        {
            minTime = 0;
            StartCoroutine("CreateCoroutine");
            //GameObject enemyGoblin = ObjectPool.
        }
    }

    IEnumerator CreateCoroutine()
    {
       yield return null;
       GameObject t_object = ObjectPool.instance.GetQueue();
       t_object.transform.position = spawnTrans.transform.position; 
    }

}
