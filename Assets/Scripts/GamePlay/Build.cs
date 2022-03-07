using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Build : MonoBehaviour
{
    public bool ready;
    [SerializeField] TextMeshPro txt;
    [SerializeField] GameObject[] levels, readyLevels;
    int curLevel, count;
    Transform block;

    void Start()
    {
        if (ready)
        {
            for (int i = 0; i < levels.Length; i++)
            {
                levels[i].SetActive(false);
                readyLevels[i].SetActive(true);
            }            
            FinalBuild();
        }            
    }
    void Update()
    {

    }
    public Transform NextBlock()
    {
        block = levels[curLevel].transform.GetChild(count);
        count++;
        if (count == levels[curLevel].transform.childCount)
        {
            //levels[curLevel].SetActive(false);
            //readyLevels[curLevel].SetActive(true);
            NextLevel();
        }
        return block;
    }
    void NextLevel()
    {
        count = 0;
        curLevel++;
        if (curLevel >= levels.Length)
        {
            ready = true;
            //GetComponent<EnemySpawner>().enabled = true;
            FinalBuild();
        }
    }
    void FinalBuild()
    {
        txt.text = "READY";
        gameObject.tag = "Untagged";
        //dropZone.SetActive(false);
    }    
}
