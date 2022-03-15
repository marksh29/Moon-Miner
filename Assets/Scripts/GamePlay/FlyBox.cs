using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyBox : MonoBehaviour
{
    
    void Start()
    {
        
    }  
    public void SetSkin(int id)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(i == id ? true : false);
        }        
    }
    public void AddBox()
    {
        gameObject.SetActive(false);
        BoxControll.Instance.AddBox();
    }
}
