using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHpBar : MonoBehaviour
{
    public Transform player;

    private void Start()
    {
        
    }

    private void Update()
    {
        transform.position = new Vector3(player.position.x, player.position.y + 2.8f, player.position.z);

    }
}
