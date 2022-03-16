using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Factoria : MonoBehaviour
{
    [SerializeField] bool fish;
    [SerializeField] int scrapCount;
    [Header("---------------Bag---------------")]// --- Bag -- //
    [SerializeField] float scale;
    [SerializeField] float yy;
    [SerializeField] int count_x, count_y, count_z;
    [Header("---------------Game---------------")]
    [SerializeField] float dropTime;
    [SerializeField] int curBox, maxBox;
    [SerializeField] List<Transform> boxPos;
    [SerializeField] List<GameObject> boxObj;
    public Transform startBox, dropScrapPos;
    bool drop_on, drop_sand;

    Coroutine sandCoroutine, fishCouroutine;

    [Header("---------------Animation---------------")]
    [SerializeField] Animator anim;
    [SerializeField] GameObject effect;

    [Header("---------------SandSpawn---------------")]
    [SerializeField] GameObject dropSandprefab;
    [SerializeField] Transform spawnPos;
    [SerializeField] float spawnTimer;
    float timer;
    void Awake()
    {
        for (int z = 0; z < count_y; z++)
        {
            for (int y = 0; y < count_z; y++)
            {
                for (int x = 0; x < count_x; x++)
                {
                    GameObject obj = new GameObject();
                    obj.transform.parent = startBox.transform.parent.transform;
                    obj.transform.localPosition = new Vector3(startBox.localPosition.x + (scale * x), startBox.localPosition.y + (yy * z), startBox.localPosition.z + (scale * y));
                    obj.transform.rotation = startBox.rotation;
                    boxPos.Add(obj.transform);
                }
            }
        }
        maxBox = count_y * (count_x * count_z);
    }

    void Update()
    {
        if(scrapCount > 0)
        {
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                timer = spawnTimer;
                SpawnSand();
            }
        }
    }
    public void AddScrap()
    {
        scrapCount++;
        Animation(true);
    }
    public void SpawnSand()
    {
        scrapCount--;
        GameObject obj = PoolControll.Instance.Spawn("DropSand");
        obj.transform.position = spawnPos.position;
        obj.GetComponent<DropSand>().SetState(fish ? "fish" : "sand");
        if (scrapCount <= 0)
            Animation(false);
    }
    public void AddSand() 
    {
        GameObject obj = PoolControll.Instance.Spawn("Sand");
        boxObj.Add(obj);
        obj.GetComponent<Box>().SetState(fish ? "fish" : "sand", true);
        obj.transform.position = boxPos[curBox].position;
        obj.transform.parent = boxPos[curBox];
        obj.transform.rotation = boxPos[curBox].rotation;
        curBox = boxObj.Count;
    }

    private void Animation(bool state)
    {
        anim.enabled = state;
        effect.SetActive(state);
    } 

    public void GetSand(bool id)
    {
        if(id)
        {
            sandCoroutine = StartCoroutine(SandCouroutine());
        }
        else
        {
            if(sandCoroutine != null)
                StopCoroutine(sandCoroutine);
        }
    }
    IEnumerator SandCouroutine()
    {
        while (boxObj.Count > 0)
        {
            if(!fish)
                boxObj[boxObj.Count - 1].GetComponent<Box>().sand = true;
            else
                boxObj[boxObj.Count - 1].GetComponent<Box>().fish = true;
            StartCoroutine(boxObj[boxObj.Count - 1].GetComponent<Box>().MoveSand(0.2f, BoxControll.Instance.SandPos()));
            boxObj.Remove(boxObj[boxObj.Count - 1]);
            curBox = boxObj.Count;
            yield return new WaitForSeconds(dropTime);
        }
    }
}
