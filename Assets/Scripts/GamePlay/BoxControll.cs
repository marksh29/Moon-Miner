using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoxControll : MonoBehaviour
{
    public static BoxControll Instance;
    [SerializeField] BarrelScale barCtrl, fishCtrl;
    [SerializeField] bool scrapLand;
    [Header("---------------Bag---------------")]// --- Bag -- //
    [SerializeField] float scale;
    [SerializeField] int count_x, count_y, count_z;
    [Header("---------------Game---------------")]
    [SerializeField] Transform startBox, barrel;
    [SerializeField] Text scrapText, sandText, fishText;
    [SerializeField] float dropSpeed, damage;
    [SerializeField] int curBox, maxBox, scrapCount, sandCount, fishCount;
    [SerializeField] List<Transform> boxPos;
    [SerializeField] List<GameObject> boxObj;

    Coroutine lastRoutine, scarpCouroutine, sandCouroutine, fishCouroutine;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        for (int z = 0; z < count_y; z++)
        {
            for (int y = 0; y < count_z; y++)
            {
                for (int x = 0; x < count_x; x++)
                {
                    GameObject obj = new GameObject();
                    obj.transform.parent = startBox.transform.parent.transform;
                    obj.transform.localPosition = new Vector3(startBox.localPosition.x + (scale * x), startBox.localPosition.y + (scale * z), startBox.localPosition.z + (scale * y));
                    obj.transform.rotation = startBox.rotation;
                    boxPos.Add(obj.transform);
                }
            }
        }
        maxBox = count_y * (count_x * count_z);
        Txt();
    }  
       
    private void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "Drop" && !coll.gameObject.GetComponent<Build>().ready && coll.GetComponent<Build>().cleareArea)
        {
            lastRoutine = StartCoroutine(DropBox(coll.gameObject.GetComponent<Build>()));
        }
        if (coll.gameObject.tag == "DropSand" && coll.GetComponent<BuildLand>().cleareArea)
        {
            if(!scrapLand && sandCount > 0)
            {
                sandCouroutine = StartCoroutine(DropSand(coll.gameObject.GetComponent<BuildLand>()));
            }
            else if(scrapLand)
            {
                scarpCouroutine = StartCoroutine(DropScrap(coll.gameObject.transform));
            }           
        }
        if (coll.gameObject.tag == "DropFish" && coll.GetComponent<BuildFish>().cleareArea)
        {
            fishCouroutine = StartCoroutine(DropFish(coll.gameObject.GetComponent<BuildFish>()));
        }
        if (coll.gameObject.tag == "DropZone")
        {
            if (scrapCount > 0)
            {
                scarpCouroutine = StartCoroutine(DropScrap(coll.gameObject.transform.parent));
            }           
            coll.gameObject.transform.parent.gameObject.GetComponent<Factoria>().GetSand(true);
        }      
        if (coll.gameObject.tag == "Box")
        {
            coll.gameObject.GetComponent<Scrap>().Damage(damage, true);
        }
    }
    private void OnTriggerExit(Collider coll)
    {
        if (coll.gameObject.tag == "Drop")
        {
            if(lastRoutine != null)
                StopCoroutine(lastRoutine);
        }      
        if (coll.gameObject.tag == "DropSand")
        {
            if(!scrapLand)
            {
                if (sandCouroutine != null)
                    StopCoroutine(sandCouroutine);
            }            
        }
        if (coll.gameObject.tag == "DropFish")
        {
            if (fishCouroutine != null)
                StopCoroutine(fishCouroutine);
        }
        if (coll.gameObject.tag == "DropZone")
        {           
            if (scarpCouroutine != null)
                StopCoroutine(scarpCouroutine);
            coll.gameObject.transform.parent.gameObject.GetComponent<Factoria>().GetSand(false);
        }       
        if (coll.gameObject.tag == "Box")
        {
            coll.gameObject.GetComponent<Scrap>().Damage(damage, false);
        }
    }
    public void SpawnFlyScrap(Scrap objt)
    {
        GameObject obj = PoolControll.Instance.Spawn("FlyBox");
        obj.transform.parent = transform;
        obj.GetComponent<FlyBox>().SetSkin(objt.id);
        obj.transform.position = transform.position;
    }

    IEnumerator DropBox(Build _build)
    {        
        for (int i = scrapCount; i > 0; i--)
        {
            if (_build.buildCount > 0)
            {
                Transform box = _build.NextBlock();
                GameObject scrap = PoolControll.Instance.Spawn("Box");
                scrap.GetComponent<Box>().SetState("barrel", false);
                scrap.transform.position = startBox.transform.parent.position;                  
                StartCoroutine(scrap.GetComponent<Box>().DoMove(0.3f, box));
                _build.BuildCount();
                scrapCount--;
                Txt();
                barCtrl.SetScale(scrapCount);
            }
            else
                if(lastRoutine !=  null)
                    StopCoroutine(lastRoutine);         
            yield return new WaitForSeconds(dropSpeed);
        }
    }
    IEnumerator DropSand(BuildLand _build)
    {
        for (int i = sandCount; i > 0; i--)
        {
            if (_build.buildCount > 0)
            {
                GameObject sand = PoolControll.Instance.Spawn("Sand");
                sand.GetComponent<Box>().buildSand = true;
                sand.GetComponent<Box>().SetState("sand", false);
                sand.transform.position = startBox.transform.parent.position;              
                StartCoroutine(sand.GetComponent<Box>().DoMove(0.3f, _build.NextBlock()));
                sandCount--;
                _build.BuildCount();
                Txt();
                yield return new WaitForSeconds(dropSpeed);
            }
            else
                 if (sandCouroutine != null)
                    StopCoroutine(sandCouroutine);
        }       
    }
    IEnumerator DropFish(BuildFish _target)
    {
        for (int i = fishCount; i > 0; i--)
        {
            if (_target.fishCount > 0)
            {
                GameObject sand = PoolControll.Instance.Spawn("Sand");
                sand.GetComponent<Box>().SetState("fish", false);
                sand.GetComponent<Box>().buildFish = true;
                sand.transform.position = startBox.transform.parent.position;
                StartCoroutine(sand.GetComponent<Box>().DoMove(0.3f, _target.transform));
                fishCount--;                
                _target.RemoveFishCount();
                fishCtrl.SetScale(fishCount);
                Txt();
                yield return new WaitForSeconds(dropSpeed);
            }
            else
                if (fishCouroutine != null)
                    StopCoroutine(fishCouroutine);
        }
    }

    IEnumerator DropScrap(Transform _pos)
    {
        GameObject scrap = PoolControll.Instance.Spawn("BarrelDrop");
        scrap.transform.parent = barrel;
        scrap.transform.position = barrel.position;
        scrap.transform.rotation = barrel.rotation;
        scrap.transform.parent = null;
        scrap.GetComponent<BarrelDrop>().StartDrop(_pos, scrapCount, barrel.localScale.x, scrapLand);
        scrapCount = 0;
        barCtrl.SetScale(scrapCount);
        Txt();
        yield return new WaitForSeconds(dropSpeed);       
    }
    public void AddBox() 
    {      
        scrapCount++;
        barCtrl.SetScale(scrapCount);
        Txt();
    }
    public void AddFish()
    {
        fishCount++;
        fishCtrl.SetScale(fishCount);
        Txt();
    }
    public Transform SandPos()
    {
        return startBox.transform.parent;
    }
    public void AddSand()
    {
        sandCount++;
        Txt();
    }
    void Txt()
    {
        scrapText.text = scrapCount.ToString();
        sandText.text = sandCount.ToString();
        fishText.text = fishCount.ToString();
    }
}
