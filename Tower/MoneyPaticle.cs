using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyPaticle : MonoBehaviour
{
    public ParticleSystem moneyParticle;
    public int Money;

    private void OnEnable()
    {
        moneyParticle.Play(true);
        GameManager.instance.Money += Money;
    }

}
