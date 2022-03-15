using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropSand : MonoBehaviour
{


    private void OnEnable()
    {
        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
    }
    void Start()
    {
        
    }
    public void SetState(string name)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(transform.GetChild(i).gameObject.name == name ? true : false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "AddSand")
        {
            gameObject.SetActive(false);
            other.gameObject.transform.parent.parent.GetComponent<Factoria>().AddSand();
        }
    }
}
