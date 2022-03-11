using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoxControll : MonoBehaviour
{
    public static BoxControll Instance;
    [SerializeField] BarrelScale barCtrl;
    [Header("---------------Bag---------------")]// --- Bag -- //
    [SerializeField] float scale;
    [SerializeField] int count_x, count_y, count_z;
    [Header("---------------Game---------------")]
    [SerializeField] Transform startBox, barrel;
    [SerializeField] Text scrapText, sandText;
    [SerializeField] float dropSpeed, damage;
    [SerializeField] int curBox, maxBox, scrapCount, sandCount;
    [SerializeField] List<Transform> boxPos;
    [SerializeField] List<GameObject> boxObj;
    
    bool drop_on, drop_scrap, drop_sand;

    Coroutine lastRoutine, scarpCouroutine, sandCouroutine;

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
        if (coll.gameObject.tag == "Drop" && !coll.gameObject.GetComponent<Build>().ready)
        {
            drop_on = true;
            lastRoutine = StartCoroutine(DropBox(coll.gameObject.GetComponent<Build>()));
        }
        if (coll.gameObject.tag == "DropSand" && sandCount > 0)
        {
            drop_sand = true;
            sandCouroutine = StartCoroutine(DropSand(coll.gameObject.GetComponent<BuildLand>()));
        }
        //if (coll.gameObject.tag == "DropScrap")
        //{
        //    drop_scrap = true;
        //    scarpCouroutine = StartCoroutine(DropScrap(coll.gameObject.transform.parent.gameObject.GetComponent<Factoria>().dropScrapPos));
        //}
        //if (coll.gameObject.tag == "GetSand")
        //{
        //    coll.gameObject.GetComponent<Factoria>().GetSand(true);
        //}
        if (coll.gameObject.tag == "DropZone")
        {
            if (scrapCount > 0)
            {
                drop_scrap = true;
                scarpCouroutine = StartCoroutine(DropScrap(coll.gameObject.transform.parent));
            }           
            coll.gameObject.transform.parent.gameObject.GetComponent<Factoria>().GetSand(true);
        }      
        if (coll.gameObject.tag == "Box")// && curBox < maxBox)
        {
            coll.gameObject.GetComponent<Scrap>().Damage(damage, true);
        }
    }
    private void OnTriggerExit(Collider coll)
    {
        if (coll.gameObject.tag == "Drop")
        {
            drop_scrap = false;
            if(lastRoutine != null)
                StopCoroutine(lastRoutine);
        }
        //if (coll.gameObject.tag == "DropScrap")
        //{
        //    drop_scrap = false;
        //    if(scarpCouroutine != null)
        //        StopCoroutine(scarpCouroutine);
        //}
        //if (coll.gameObject.tag == "GetSand")
        //{
        //    coll.gameObject.GetComponent<Factoria>().GetSand(false);
        //}
        if (coll.gameObject.tag == "DropSand")
        {
            drop_sand = false;
            if (sandCouroutine != null)
                StopCoroutine(sandCouroutine);
        }
        if (coll.gameObject.tag == "DropZone")
        {           
            drop_scrap = false;
            if (scarpCouroutine != null)
                StopCoroutine(scarpCouroutine);
            coll.gameObject.transform.parent.gameObject.GetComponent<Factoria>().GetSand(false);
        }       
        if (coll.gameObject.tag == "Box")// && curBox < maxBox)
        {
            coll.gameObject.GetComponent<Scrap>().Damage(damage, false);
        }
    }
    public void SpawnFlyScrap(Scrap objt)
    {
        GameObject obj = PoolControll.Instance.Spawn("FlyBox");
        obj.transform.parent = transform;
        obj.GetComponent<FlyBox>().SetSkin(objt.id);
        //obj.GetComponent<MeshRenderer>().sharedMaterial = objt.GetComponent<Scrap>().mat;
        obj.transform.position = transform.position;
    }

    IEnumerator DropBox(Build _build)
    {
        //for (int i = boxObj.Count - 1; i >= 0; i--)
        //{            
        //    if (_build.buildCount > 0 && boxObj[i].tag == "Scrap")
        //    {
        //        Transform box = _build.NextBlock();
        //        StartCoroutine(box.transform.position.y < boxObj[i].transform.position.y ? boxObj[i].GetComponent<Box>().DoMove(0.3f, box) : boxObj[i].GetComponent<Box>().DoMove(0, box));
        //        _build.BuildCount();
        //        boxObj.Remove(boxObj[i]);
        //    }
        //    else
        //        break;
        //    Txt();
        //    yield return new WaitForSeconds(dropSpeed);
        //} 
        for (int i = scrapCount; i > 0; i--)
        {
            if (_build.buildCount > 0)
            {
                Transform box = _build.NextBlock();
                GameObject scrap = PoolControll.Instance.Spawn("Box");
                scrap.transform.position = startBox.transform.parent.position;
                //StartCoroutine(box.transform.position.y < scrap.transform.position.y ? scrap.GetComponent<Box>().DoMove(0.3f, box) : scrap.GetComponent<Box>().DoMove(0, box));
                StartCoroutine(scrap.GetComponent<Box>().DoMove(0.3f, box));
                _build.BuildCount();
                scrapCount--;
                Txt();
                barCtrl.SetScale(scrapCount);
            }
            else
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
                sand.transform.position = startBox.transform.parent.position;
                //StartCoroutine(_build.transform.position.y > sand.transform.position.y ? sand.GetComponent<Box>().DoMove(0.3f, _build.transform) : sand.GetComponent<Box>().DoMove(0.01f, _build.transform));
                StartCoroutine(sand.GetComponent<Box>().DoMove(0.3f, _build.NextBlock()));
                sandCount--;
                _build.BuildCount();
                Txt();
                yield return new WaitForSeconds(dropSpeed);
            }
            else
                StopCoroutine(sandCouroutine);
        }
        //for (int i = boxObj.Count - 1; i >= 0; i--)
        //{
        //    if (_build.buildCount > 0 && boxObj[i].tag == "Sand")
        //    {
        //        Transform box = _build.NextBlock();
        //        StartCoroutine(box.transform.position.y < boxObj[i].transform.position.y ? boxObj[i].GetComponent<Box>().DoMove(0.3f, box) : boxObj[i].GetComponent<Box>().DoMove(0, box));
        //        _build.BuildCount();
        //        boxObj.Remove(boxObj[i]);
        //    }
        //    else
        //        break;
        //    //Txt();
        //    yield return new WaitForSeconds(dropSpeed);
        //}
    }
    IEnumerator DropScrap(Transform _pos)
    {
        GameObject scrap = PoolControll.Instance.Spawn("BarrelDrop");
        scrap.transform.parent = barrel;
        scrap.transform.position = barrel.position;
        scrap.transform.rotation = barrel.rotation;
        scrap.transform.parent = null;
        scrap.GetComponent<BarrelDrop>().StartDrop(_pos, scrapCount, barrel.localScale.x);
        scrapCount = 0;
        barCtrl.SetScale(scrapCount);
        yield return new WaitForSeconds(dropSpeed);
        //for (int i = scrapCount; i > 0; i--)
        //{
        //    GameObject scrap = PoolControll.Instance.Spawn("Box");
        //    scrap.transform.position = startBox.transform.parent.position;
        //    //StartCoroutine(_pos.transform.position.y > scrap.transform.position.y ? scrap.GetComponent<Box>().DoMove(0.3f, _pos) : scrap.GetComponent<Box>().DoMove(0.01f, _pos));
        //    StartCoroutine(scrap.GetComponent<Box>().DoMove(0.3f, _pos));
        //    scrapCount--;
        //    barCtrl.SetScale(scrapCount);
        //    Txt();
        //    yield return new WaitForSeconds(dropSpeed);
        //}
        //for (int i = boxObj.Count - 1; i >= 0; i--)
        //{
        //    if(boxObj[i].tag == "Scrap")
        //    {
        //        StartCoroutine(_pos.transform.position.y > boxObj[i].transform.position.y ? boxObj[i].GetComponent<Box>().DoMove(0.3f, _pos) : boxObj[i].GetComponent<Box>().DoMove(0.01f, _pos));
        //        boxObj.Remove(boxObj[i]);                
        //    }
        //    Txt();
        //    yield return new WaitForSeconds(dropSpeed);           
        //}
    }
    public void AddBox(Material mat)  // - ������ ��������
    {
        //GameObject obj = PoolControll.Instance.Spawn("Box");
        //obj.GetComponent<MeshRenderer>().sharedMaterial = mat;       
        //boxObj.Add(obj);
        //obj.transform.position = boxPos[curBox].position;
        //obj.transform.parent = boxPos[curBox];
        //obj.transform.rotation = boxPos[curBox].rotation;
        //Txt();
        scrapCount++;
        barCtrl.SetScale(scrapCount);
        Txt();
    }
    //public Transform AddSand(GameObject obj)
    //{
    //    boxObj.Add(obj);
    //    Txt();
    //    return boxPos[curBox - 1];
    //}
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
    }
}
