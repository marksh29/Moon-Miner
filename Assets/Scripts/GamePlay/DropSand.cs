using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropSand : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnEnable()
    {
        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
