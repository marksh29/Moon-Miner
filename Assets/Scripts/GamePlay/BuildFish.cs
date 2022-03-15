using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuildFish : MonoBehaviour
{
    [Header("Options")]
    [SerializeField] int count;
    [Header("GamePlay")]
    public int fishCount;
    public bool cleareArea;
    public List<Collider> colliders = new List<Collider>();
    [SerializeField] Transform[] fishParent;
    bool buildOn;

    void Start()
    {
        fishCount = count;
        CleareOff();
    }
    void Update()
    {
        
    }   
   
    public void SpawnFish()
    {
        GameObject fish = PoolControll.Instance.Spawn("Fish");
        fish.transform.parent = fishParent[0].transform.parent;      
        fish.GetComponent<FishControll>().StartMove(fishParent);
    }
    public void RemoveFishCount()
    {
        fishCount--;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!colliders.Contains(other) && other.gameObject.tag == "Box")
        {
            colliders.Add(other);
            if (colliders.Count > 0)
            {
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
            cleareArea = true;
        }
    }
}
