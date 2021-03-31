using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] Text _coinsText;

    public void UpdateCoinsText(int amount)
    {
        _coinsText.text = "Coins: " + amount.ToString();
    }
}
