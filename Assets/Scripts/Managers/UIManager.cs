using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] Text _coinsText, _livesText;

    public void UpdateCoinsText(int amount)
    {
        _coinsText.text = "Coins: " + amount.ToString();
    }

    public void UpdateLivesText(int amount)
    {
        _livesText.text = "Lives " + amount.ToString();
    }
}
