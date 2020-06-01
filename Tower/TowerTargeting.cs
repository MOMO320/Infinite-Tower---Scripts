using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerTargeting : MonoBehaviour
{
    public static TowerTargeting Instance
    {
        get
        {
            if( instance == null)
            {
                instance = FindObjectOfType<TowerTargeting>();
                if (instance == null)
                {
                    var instanceContainer = new GameObject("TowerTargeting");
                    instance = instanceContainer.AddComponent<TowerTargeting>();
                }
            }
            return instance;
        }
    }
    private static TowerTargeting instance;

    [SerializeField] Transform m_tfGunBody = null;
    [SerializeField] float m_range = 10f;
    [SerializeField] LayerMask m_layerMask = 0;

    private List<GameObject> UnitList = new List<GameObject>();

    Transform m_tfTarget = null;
     
    void SearchEnemy()
    {
        Collider[] colls = Physics.OverlapSphere(transform.position, m_range, m_layerMask);
        Transform shortesTarget = null;

        for(int i = 0; i < colls.Length; i++)
        {
            UnitList.Add(colls[i].gameObject);
        }

        if(UnitList.Count > 0)
        {
            float shorestDistance = Mathf.Infinity;
               
            foreach(Collider colTarget in colls)
            {
                float distance = Vector3.SqrMagnitude(transform.position - colTarget.transform.position);
                if(shorestDistance > distance)
                {
                    shorestDistance = distance;
                    shortesTarget = colTarget.transform;
                }
            }
        }

        m_tfTarget = shortesTarget;
    
    }


}
