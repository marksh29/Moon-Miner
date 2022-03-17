using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



public class Tower : MonoBehaviour
{
    public enum towerState
    { 
       Player, Enemy, Neutral
    }

    public towerState _state;
    public int count, maxCount, levelCount;
    [SerializeField] TextMeshPro countText;

    [SerializeField] GameObject down;
    [SerializeField] GameObject[] levels;
    [SerializeField] Material[] playerMat, enemyMat, neutralMat;

    //public TowerFire towerFire;
    public float startRadius, addRadius, fireTimer;

    void Start()
    {
        _state = towerState.Neutral;
        CountText();
        LevelEnable();
    }
    void Update()
    {
        
    }

    public int FullCount(string name, int id)
    {
        int full = new int();
        switch (name) 
        {
            case ("Player"):
                switch(_state)
                {
                    case (towerState.Player):
                        full = id < maxCount - count ? id : maxCount - count;                       
                        break;
                    case (towerState.Enemy):
                    case (towerState.Neutral):
                        full = id;
                        break;
                }                
                break;
            case ("Enemy"):
                switch (_state)
                {
                    case (towerState.Enemy):
                        full = id < maxCount - count ? id : maxCount - count;
                        break;
                    case (towerState.Player):
                    case (towerState.Neutral):
                        full = id;
                        break;
                }
                break;
        }
        return full;
    }

    public void Enter(string state)
    {
        switch(_state)
        {
            case (towerState.Neutral):
                if(count > 0)
                    RemoveCount();
                else
                {
                    if(state == "Player")
                    {
                        _state = towerState.Player;
                    }                      
                    else
                    {
                        _state = towerState.Enemy;
                    }                        
                    //Controll.Instance.AddWinCount(_state.ToString());
                    //towerFire.SetState(_state.ToString());
                    AddCount();
                }

                break;
            case (towerState.Player):
            case (towerState.Enemy):
                if (state == _state.ToString())
                    AddCount();
                else
                    RemoveCount();
                break;
        }         
    }
    void AddCount()
    {
        count++;
        CountText();
        LevelEnable();
        
        if (count >= 1)
            Recolor();
    }
    void RemoveCount()
    {
        count--;
        CountText();
        LevelEnable();
        
        if (count == 0)
        {
            //Controll.Instance.RemoveWinCount(_state.ToString());
            //towerFire.SetState(_state.ToString());
            _state = towerState.Neutral;           
            Recolor();
        }
    }
    void CountText()
    {
        if (count > maxCount) count = maxCount;
        countText.text = count.ToString();// + "/" + maxCount;
    }

    void LevelEnable()
    {
        for (int i = 0; i < levels.Length; i++)
        {
            levels[i].SetActive(i < Mathf.FloorToInt(count/levelCount) ? true : false);
        }
    }

    void Recolor()
    {
        switch (_state)
        {
            case (towerState.Player):
                down.GetComponent<MeshRenderer>().material = playerMat[0];
                for (int i = 0; i < levels.Length; i++)
                {
                    Material[] materials = levels[i].GetComponent<MeshRenderer>().materials;
                    materials[0] = playerMat[0];
                    materials[1] = playerMat[1];
                    levels[i].GetComponent<MeshRenderer>().materials = materials;
                }
                //towerFire.SetFireState(true, Mathf.FloorToInt(count/5));
                break;
            case (towerState.Enemy):
                down.GetComponent<MeshRenderer>().material = enemyMat[0];
                for (int i = 0; i < levels.Length; i++)
                {
                    Material[] materials = levels[i].GetComponent<MeshRenderer>().materials;
                    materials[0] = enemyMat[0];
                    materials[1] = enemyMat[1];
                    levels[i].GetComponent<MeshRenderer>().materials = materials;
                }
                //towerFire.SetFireState(true, Mathf.FloorToInt(count / 5));
                break;
            case (towerState.Neutral):
                down.GetComponent<MeshRenderer>().material = neutralMat[0];
                for (int i = 0; i < levels.Length; i++)
                {
                    Material[] materials = levels[i].GetComponent<MeshRenderer>().materials;
                    materials[0] = neutralMat[0];
                    materials[1] = neutralMat[1];
                    levels[i].GetComponent<MeshRenderer>().materials = materials;
                }
                //towerFire.SetFireState(false, 0);
                break;
        }
    }
}
