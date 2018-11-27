using System.Collections.Generic;
using Billow;
using TMPro;
using UnityEngine;

public class UIControl : MonoBehaviour
{
    public TextMeshProUGUI KeyYellow, KeyRed, KeyBlue, Attach, Defensive, Coin, Hp;

    void UpdateKey(KeyValuePair<string, int> pair)
    {
        
        switch (pair.Key)
        {
            case "Yellow":
                KeyYellow.text = pair.Value.ToString();
                break;
            case "Red":
                KeyRed.text = pair.Value.ToString();
                break;
            case "Blue":
                KeyBlue.text = pair.Value.ToString();
                break;
        }
    }

    void UpdateFighter(Fighter fighter)
    {
        Attach.text = fighter.AttachPower.ToString();
        Defensive.text = fighter.DefensivePower.ToString();
        Coin.text = fighter.CoinCount.ToString();
        Hp.text = fighter.Hp.ToString();
    }
}