using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoxControll : MonoBehaviour
{
    public static BoxControll Instance;
    [Header("---------------Bag---------------")]// --- Bag -- //
    [SerializeField] float scale;
    [SerializeField] int count_x, count_y, count_z;
    [Header("---------------Game---------------")]

    [SerializeField] Text boxText;
    [SerializeField] float dropSpeed, damage;
    [SerializeField] int curBox, maxBox;
    [SerializeField] List<Transform> boxPos;
    [SerializeField] List<GameObject> boxObj;
    [SerializeField] Transform startBox;
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
        if (coll.gameObject.tag == "DropSand" && !coll.gameObject.GetComponent<Build>().ready)
        {
            drop_sand = true;
            sandCouroutine = StartCoroutine(DropSand(coll.gameObject.GetComponent<BuildLand>()));
        }
        if (coll.gameObject.tag == "DropScrap")
        {
            drop_scrap = true;
            scarpCouroutine = StartCoroutine(DropScrap(coll.gameObject.transform.parent.gameObject.GetComponent<Factoria>().dropScrapPos));
        }
        if (coll.gameObject.tag == "GetSand")
        {
            coll.gameObject.GetComponent<Factoria>().GetSand(true);
        }
        if (coll.gameObject.tag == "Box")// && curBox < maxBox)
        {
            coll.gameObject.GetComponent<Scrap>().Damage(damage, true);
        }
    }
    //private void OnCollisionStay(Collision coll)
    //{
    //    if (coll.gameObject.tag == "Box")// && curBox < maxBox)
    //    {
    //        coll.gameObject.GetComponent<Scrap>().Damage(damage);           
    //    }
    //}
    public void SpawnFlyScrap(GameObject objt)
    {
        GameObject obj = PoolControll.Instance.Spawn("FlyBox");
        obj.transform.parent = transform;
        obj.GetComponent<MeshRenderer>().sharedMaterial = objt.GetComponent<Scrap>().mat;
        obj.transform.position = transform.position;
    }


    private void OnTriggerExit(Collider coll)
    {
        if (coll.gameObject.tag == "Drop")
        {
            drop_scrap = false;
            if(lastRoutine != null)
                StopCoroutine(lastRoutine);
        }
        if (coll.gameObject.tag == "DropScrap")
        {
            drop_scrap = false;
            if(scarpCouroutine != null)
                StopCoroutine(scarpCouroutine);
        }
        if (coll.gameObject.tag == "DropSand")
        {
            drop_sand = false;
            if (scarpCouroutine != null)
                StopCoroutine(sandCouroutine);
        }
        if (coll.gameObject.tag == "GetSand")
        {
            coll.gameObject.GetComponent<Factoria>().GetSand(false);
        }
        if (coll.gameObject.tag == "Box")// && curBox < maxBox)
        {
            coll.gameObject.GetComponent<Scrap>().Damage(damage, false);
        }
    }

    IEnumerator DropBox(Build _build)
    {
        for (int i = boxObj.Count - 1; i >= 0; i--)
        {            
            if (_build.buildCount > 0 && boxObj[i].tag == "Scrap")
            {
                Transform box = _build.NextBlock();
                StartCoroutine(box.transform.position.y < boxObj[i].transform.position.y ? boxObj[i].GetComponent<Box>().DoMove(0.3f, box) : boxObj[i].GetComponent<Box>().DoMove(0, box));
                _build.BuildCount();
                boxObj.Remove(boxObj[i]);
            }
            else
                break;
            Txt();
            yield return new WaitForSeconds(dropSpeed);
        }       
    }
    IEnumerator DropSand(BuildLand _build)
    {
        for (int i = boxObj.Count - 1; i >= 0; i--)
        {
            if (_build.buildCount > 0 && boxObj[i].tag == "Sand")
            {
                Transform box = _build.NextBlock();
                StartCoroutine(box.transform.position.y < boxObj[i].transform.position.y ? boxObj[i].GetComponent<Box>().DoMove(0.3f, box) : boxObj[i].GetComponent<Box>().DoMove(0, box));
                _build.BuildCount();
                boxObj.Remove(boxObj[i]);
            }
            else
                break;
            Txt();
            yield return new WaitForSeconds(dropSpeed);
        }
    }
    IEnumerator DropScrap(Transform _pos)
    {
        for (int i = boxObj.Count - 1; i >= 0; i--)
        {
            if(boxObj[i].tag == "Scrap")
            {
                StartCoroutine(_pos.transform.position.y > boxObj[i].transform.position.y ? boxObj[i].GetComponent<Box>().DoMove(0.3f, _pos) : boxObj[i].GetComponent<Box>().DoMove(0.01f, _pos));
                boxObj.Remove(boxObj[i]);                
            }
            Txt();
            yield return new WaitForSeconds(dropSpeed);           
        }
    }
    public void AddBox(Material mat)  // - подбор кирпичей
    {
        GameObject obj = PoolControll.Instance.Spawn("Box");
        obj.GetComponent<MeshRenderer>().sharedMaterial = mat;       
        boxObj.Add(obj);
        obj.transform.position = boxPos[curBox].position;
        obj.transform.parent = boxPos[curBox];
        obj.transform.rotation = boxPos[curBox].rotation;
        Txt();
    }       
    public Transform AddSand(GameObject obj)
    {
        boxObj.Add(obj);
        Txt();
        return boxPos[curBox - 1];
    }
    void Txt()
    {
        curBox = boxObj.Count;
        boxText.text = curBox.ToString();
    }
}
