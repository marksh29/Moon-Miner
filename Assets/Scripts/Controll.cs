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
    [SerializeField] int winCount;
    private void Awake()
    {
        if (Instance == null) Instance = this;
    }
    void Start()
    {
        Set_state("Menu");

        List<GameObject> list_1 = new List<GameObject>(GameObject.FindGameObjectsWithTag("DropSand"));
        List<GameObject> list_2 = new List<GameObject>(GameObject.FindGameObjectsWithTag("DropFish"));
        winCount = list_1.Count + list_2.Count;
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
    
    public void AddWin()
    {
        winCount--;
        if (winCount == 0)
            StartCoroutine(Win());
    }


    public void StartLevel()
    {
        Set_state("Game");
    }
    public void Next_level()
    {
        if(Application.loadedLevel == SceneManager.sceneCount - 1)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(Application.loadedLevel + 1);
        }
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
}
