using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoxControll : MonoBehaviour
{
    public static BoxControll Instance;
    [Header("Bag")]// --- Bag -- //
    [SerializeField] float xx;
    [SerializeField] int count_x, count_y, count_z;
    [Header("Game")]
    [SerializeField] Text boxText;
    [SerializeField] float dropSpeed;
    [SerializeField] int curBox, maxBox;
    [SerializeField] List<Transform> boxPos;
    [SerializeField] List<GameObject> boxObj;
    [SerializeField] Transform startBox;
    bool drop_on;

    Coroutine lastRoutine;

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
                    obj.transform.localPosition = new Vector3(startBox.localPosition.x + (xx * x), startBox.localPosition.y + (xx * z), startBox.localPosition.z + (xx * y));
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
        if (coll.gameObject.tag == "Box" && curBox < maxBox)
        {
            coll.gameObject.SetActive(false);
            GameObject obj = PoolControll.Instance.Spawn("FlyBox");
            obj.transform.parent = transform;
            obj.GetComponent<MeshRenderer>().sharedMaterial = coll.gameObject.GetComponent<MeshRenderer>().sharedMaterial;
            obj.transform.position = transform.position;
        }
    }
    private void OnTriggerExit(Collider coll)
    {
        if (coll.gameObject.tag == "Drop")
        {
            drop_on = false;
            StopCoroutine(lastRoutine);
        }
    }

    IEnumerator DropBox(Build _build)
    {
        for (int i = boxObj.Count - 1; i >= 0; i--)
        {
            Transform box = _build.NextBlock();
            if (box != null)
            {
                StartCoroutine(box.transform.position.y < boxObj[i].transform.position.y ? boxObj[i].GetComponent<Box>().DoMove(0.3f, box) : boxObj[i].GetComponent<Box>().DoMove(0, box));
                boxObj.Remove(boxObj[i]);
                curBox = boxObj.Count;
                Txt();
            }
            else
                break;
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
        curBox = boxObj.Count;
        Txt();
    }
    void Txt()
    {
        boxText.text = boxObj.Count.ToString();
    }
}
