using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndText : MonoBehaviour
{
    public static EndText Instance;
    [SerializeField] Text[] endText;
    [SerializeField] Text[] endMoney, bonusText;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    void Start()
    {
        
    }   
    public void SetEndText(int curHost, int maxHost, int money, int bonus)
    {
        TinySauce.OnGameFinished(curHost);

        for (int i = 0; i < endText.Length; i++)
        {
            endText[i].text = "HOSTS: " + curHost + "/" + maxHost;
            endMoney[i].text = "+" + money;
            bonusText[i].text = "BONUS: +" + bonus;
        }
    }
}
