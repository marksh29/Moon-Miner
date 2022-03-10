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
        transform.GetChild(id).gameObject.SetActive(true);
    }
    public void AddBox()
    {
        gameObject.SetActive(false);
        BoxControll.Instance.AddBox(GetComponent<MeshRenderer>().sharedMaterial);
    }
}
