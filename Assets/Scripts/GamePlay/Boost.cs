using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Boost : MonoBehaviour
{
    enum BoostVariant
    {
        plus, minus, multiply, divide
    }
    [SerializeField] BoostVariant boostType;
    [SerializeField] float count;
    [SerializeField] TextMeshPro boostText;
    [SerializeField] GameObject prefab;
    void Start()
    {
        switch(boostType)
        {
            case (BoostVariant.plus):
                boostText.text = "+" + count;
                break;
            case (BoostVariant.minus):
                boostText.text = "-" + count;
                break;
            case (BoostVariant.multiply):
                boostText.text = "x" + count;
                break;
            case (BoostVariant.divide):
                boostText.text = "/" + count;
                break;
        }
    }       
    public void SetBoost(int id)
    {
        count = id < count ? id : count;

        switch (boostType)
        {
            case (BoostVariant.plus):
                Spawn();
                break;
            //case (BoostVariant.minus):
            //    Player.Instance.RemoveHosts(Mathf.FloorToInt(count < cnt ? count : cnt));
            //    Off();
            //    break;
            case (BoostVariant.multiply):               
                Spawn();
                break;
            //case (BoostVariant.divide):
            //    int ct = Mathf.FloorToInt(cnt - (cnt/count));
            //    Player.Instance.RemoveHosts(ct < cnt ? ct : cnt);
            //    Off();
            //    break;
        }
    }
    void Spawn()
    {
        for (int i = 0; i < count; i++)
        {
            GameObject obj = Instantiate(prefab, transform.position, transform.rotation) as GameObject;
            Player.Instance.AddTrack(obj);
        }
        gameObject.SetActive(false);
    }   
}
