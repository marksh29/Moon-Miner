using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Build : MonoBehaviour
{
    public bool ready, cleareArea;
    [SerializeField] TextMeshPro txt;
    [SerializeField] GameObject workBuild, readyBuild, dropSandObj;
    [SerializeField] GameObject levels;

    [SerializeField] int count;
    public int buildCount;
    Transform block;

    public List<Collider> colliders = new List<Collider>(); 

    void Start()
    {
        if (ready)
        {
            levels.SetActive(false);
            FinalBuild();
        }
        buildCount = levels.transform.childCount;
        CleareOff();
    }
    void Update()
    {

    }
    public void BuildCount()
    {
        buildCount--;
    }
    public Transform NextBlock()
    {
        block = levels.transform.GetChild(count);
        count++;
        if (count == levels.transform.childCount)
        {
            // FinalBuild();
            StartCoroutine(End());
        }
        return block;
    }  
    IEnumerator End()
    {
        yield return new WaitForSeconds(0.5f);
        FinalBuild();
    }

    void FinalBuild()
    {
        workBuild.SetActive(false);
        levels.SetActive(false);
        dropSandObj.SetActive(true);
        readyBuild.SetActive(true);
        //txt.text = "GET SAND";
        //gameObject.tag = "GetSand";
        GetComponent<BoxCollider>().enabled = false;
    }


    private void OnTriggerStay(Collider other)
    {
        if (!colliders.Contains(other) && other.gameObject.tag == "Box")
        {
            colliders.Add(other);
            if(colliders.Count > 0)
            {
                workBuild.SetActive(false);
                cleareArea = false;
            }           
        }
    } 
    public void CleareOff()
    {      
        colliders.Clear();
        StartCoroutine(Off());
    }
    IEnumerator Off()
    {
        yield return new WaitForSeconds(0.5f);
        if (colliders.Count == 0)
        {
            workBuild.SetActive(true);
            cleareArea = true;
        }
    }
}
