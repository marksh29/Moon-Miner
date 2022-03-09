using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Build : MonoBehaviour
{
    public bool ready;
    [SerializeField] TextMeshPro txt;
    [SerializeField] GameObject readyBuild, dropSandObj;
    [SerializeField] GameObject levels;

    int count;
    public int buildCount;
    Transform block;

    void Start()
    {
        if (ready)
        {
            levels.SetActive(false);
            FinalBuild();
        }
        buildCount = levels.transform.childCount;
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
            FinalBuild();
        }
        return block;
    }  
    void FinalBuild()
    {
        levels.SetActive(false);
        dropSandObj.SetActive(true);
        readyBuild.SetActive(true);
        txt.text = "GET SAND";
        gameObject.tag = "GetSand";
    }    
}
