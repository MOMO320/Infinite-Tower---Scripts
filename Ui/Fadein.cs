using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fadein : MonoBehaviour
{
    private float alpha = 1.0f;
    private MeshRenderer mesh;


    private void Start()
    {
        mesh = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
      if(alpha > 0)
      {
            alpha -= Time.deltaTime * 1f;
            mesh.material.color = new Color(1, 1, 1, alpha);
      }
    }

}
