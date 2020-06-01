using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBuyButton : MonoBehaviour
{

    public void UnitCardButton(bool _isActive)
    {
        _isActive = true;
        UiManager.instance.BuyButtonCLick(_isActive);
    }

}