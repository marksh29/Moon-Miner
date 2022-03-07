using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Build : MonoBehaviour
{
    public bool ready;
    [SerializeField] TextMeshPro txt, nide;
    [SerializeField] GameObject[] levels, readyLevels;
    int curLevel, count;
    Transform block;

    [SerializeField] GameObject moneyPrefab;
    public int moneyCount, nidePeople, curPeple;


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
            levels[curLevel].SetActive(false);
            readyLevels[curLevel].SetActive(true);
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
        Player.Instance.drop = false;
        nide.gameObject.SetActive(true);
        nide.text = curPeple + "/" + nidePeople;
        txt.text = "Drop people";
        gameObject.tag = "HostDrop";
        //dropZone.SetActive(false);
    }
    public void AddHost()
    {
        curPeple++;
        nide.text = curPeple + "/" + nidePeople;
        if(curPeple == nidePeople)
        {
            txt.gameObject.transform.parent.gameObject.SetActive(false);
        }
        StartCoroutine(DropMoney());
    }
    IEnumerator DropMoney()
    {
        for (int i = 0; i < moneyCount; i ++)
        {
            Instantiate(moneyPrefab, txt.gameObject.transform.position, transform.rotation);
            yield return new WaitForSeconds(0.1f);
        }       
    }
}
