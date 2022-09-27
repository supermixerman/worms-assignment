using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VictoryUI : MonoBehaviour
{
    [SerializeField] Text winnerName;
    public void SetWinText(string text){
        winnerName.text = text + " Win!";
    }
}
