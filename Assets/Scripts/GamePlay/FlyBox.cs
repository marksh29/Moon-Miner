using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyBox : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }   
    public void AddBox()
    {
        gameObject.SetActive(false);
        BoxControll.Instance.AddBox(GetComponent<MeshRenderer>().sharedMaterial);
    }
}
