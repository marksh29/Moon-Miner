using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scrap : MonoBehaviour
{
    public Material mat;
    [SerializeField] Material[] allMats;

    private void Awake()
    {
        transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
        GetComponent<MeshRenderer>().enabled = false;
    }
    void Start()
    {
        int i = Random.Range(0, transform.childCount);
        transform.GetChild(i).gameObject.SetActive(true);
        mat = allMats[i];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
