using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Controll : MonoBehaviour
{
    public static Controll Instance;
    public string _state;
    [SerializeField] GameObject[] panels;

    [SerializeField] int money;
    [SerializeField] Text moneyText;

    //--- Upgarde---//

    [SerializeField] int[] cena, curUpdate, maxUpdate;
    [SerializeField] Image[] updateImage;
    [SerializeField] TextMeshProUGUI[] cenaText;

    public static event Action _upgrade;


    private void Awake()
    {
        //PlayerPrefs.DeleteAll();
        if (Instance == null) Instance = this;
    }
    void Start()
    {
        money = PlayerPrefs.GetInt("money", 5000);
        Set_state("Menu");
        MoneyText();
        ChangeText();
    }
  
    public void Set_state(string name)
    {
        _state = name;
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(panels[i].name == name ? true : false);
        }  
        
        switch(_state)
        {          
            case ("Win"):
                
                break;
            case ("Lose"):

                break;
        }
    }  

    public void StartLevel()
    {
        Set_state("Game");
    }
    public void Next_level()
    {
        SceneManager.LoadScene(Application.loadedLevel);
    }
    public void Restart()
    {
        SceneManager.LoadScene(Application.loadedLevel);
    }
    
    public IEnumerator Win()
    {
        yield return new WaitForSeconds(2);
        Set_state("Win");
    }
    public IEnumerator Lose()
    {
        yield return new WaitForSeconds(1);
        Set_state("Lose");
    }
       
    public void ChangeMoney(int id)
    {
        money += id;
        MoneyText();
    }
    void MoneyText()
    {
        //moneyText.text = money.ToString();
        PlayerPrefs.SetInt("money", money);
    }  

    public void UpgradeOn(bool on)
    {
        if(panels[4].activeSelf != on)
            panels[4].SetActive(on);
    }

    public void Buy(int id)
    {
        if(money >= cena[id] + (cena[id] * curUpdate[id]) && curUpdate[id] < maxUpdate[id])
        {
            PlayerPrefs.SetInt("upgrade" + id, (curUpdate[id] + 1));
            ChangeMoney(-(cena[id] + (cena[id] * curUpdate[id])));
            ChangeText();

            _upgrade.Invoke();
        }
    }
    void ChangeText()
    {
        for (int i = 0; i < maxUpdate.Length; i++)
        {
            curUpdate[i] = PlayerPrefs.GetInt("upgrade" + i);
            updateImage[i].fillAmount = (float)curUpdate[i] / (float)maxUpdate[i];
            cenaText[i].text = (cena[i] + (cena[i] * curUpdate[i])).ToString();
        }
    }    
}
