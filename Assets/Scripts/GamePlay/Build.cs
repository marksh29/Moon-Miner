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

    [SerializeField] int count;
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
        levels.SetActive(false);
        if(dropSandObj != null)
            dropSandObj.SetActive(true);
        readyBuild.SetActive(true);
        txt.text = "GET SAND";
        gameObject.tag = "GetSand";
    }    
}
