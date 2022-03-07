using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField] float startDamage, addDamage;
    float damage;
    //[SerializeField] GameObject[] updateObj;

    // --- Bag -- //
    [SerializeField] float xx, _rotSpeed;
    [SerializeField] int curBox, maxBox;
    //[SerializeField] List<Vector3> boxPos;
    [SerializeField] List<Transform> boxPos;
    [SerializeField] List<GameObject> boxObj;
    [SerializeField] Transform startBox;

    void Start()
    {
        for (int z = 0; z < 20; z++)
        {
            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    GameObject obj = new GameObject();
                    obj.transform.parent = startBox.transform.parent.transform;
                    obj.transform.localPosition = new Vector3(startBox.localPosition.x + (xx * x), startBox.localPosition.y + (xx * z), startBox.localPosition.z + (xx * y));
                    obj.transform.rotation = startBox.rotation;
                    boxPos.Add(obj.transform);
                }
            }
        }
        Player._upgrade += Upgrade;
        Upgrade();
    }   
    public void Upgrade()
    {       
        damage = startDamage  + (addDamage * PlayerPrefs.GetInt("upgrade1"));
        maxBox = 150 + (50 * PlayerPrefs.GetInt("upgrade0"));       
    }
    private void OnDisable()
    {
        Controll._upgrade -= Upgrade;
    }
   
    private void OnTriggerEnter(Collider coll)
    {
        if(coll.gameObject.tag == "Drop" && !coll.gameObject.GetComponent<Build>().ready)
        {
            Player.Instance.drop = true;
            DropBox(coll.gameObject.GetComponent<Build>());
        }
       
        if (coll.gameObject.tag == "HostDrop")
        {
            StartCoroutine(DropHost(coll.gameObject));
        }

        if (coll.gameObject.tag == "Box" && curBox < maxBox)
        {
            coll.gameObject.tag = "Untagged";
            AddBox(coll.gameObject);
        }
        if (coll.gameObject.tag == "Host" && curBox < maxBox)
        {
            coll.gameObject.tag = "BagHost";
            AddHost(coll.gameObject);
        }
    }
    private void OnTriggerStay(Collider coll)
    {
        if (coll.gameObject.tag == "Old")
        {
            coll.gameObject.GetComponent<Old>().Damage(damage);
        }
    }

    IEnumerator DropHost(GameObject target)
    {       
        for (int i = boxObj.Count - 1; i >= 0; i--)
        {
            boxObj[i].GetComponent<Host>().StartHomeMove(0.2f, target.transform);
            //boxObj.Remove(boxObj[i]);
            yield return new WaitForSeconds(0.1f);                   
        }       
    }
    void DropBox(Build _buld)
    {
        for (int i = boxObj.Count - 1; i >= 0; i--)
        {
            if(!_buld.ready)
            {
                Transform box = _buld.NextBlock();
                if (box != null)
                {
                    if (boxObj[i].tag != "BagHost")
                    {
                        StartCoroutine(box.transform.position.y < boxObj[i].transform.position.y ? boxObj[i].GetComponent<Box>().DoMove(0.3f, box) : boxObj[i].GetComponent<Box>().DoMove(0, box));
                    }
                    else
                    {
                        StartCoroutine(box.transform.position.y < boxObj[i].transform.position.y ? boxObj[i].GetComponent<Host>().DoMove(0.3f, box) : boxObj[i].GetComponent<Host>().DoMove(0, box));
                    }     
                    boxObj.Remove(boxObj[i]);
                    curBox--;
                }
                else
                    break;
            }                   
        }
        Player.Instance.drop = false;
    }

    void AddBox(GameObject obj) // - подбор кирпичей
    {
        boxObj.Add(obj);
        StartCoroutine(obj.GetComponent<Box>().DoMove(0.3f, boxPos[curBox]));
        curBox++;
    }
    void AddHost(GameObject obj) // - подбор кирпичей
    {
        boxObj.Add(obj);
        obj.GetComponent<Host>().StartMove(0.1f, boxPos[curBox]);
        //StartCoroutine(obj.GetComponent<Host>().DoMove(0.1f, boxPos[curBox]));
        curBox++;
    }

    //private void OnCollisionStay(Collision coll)
    //{        
    //    if(coll.gameObject.tag == "Old")
    //    {
    //        coll.gameObject.GetComponent<Old>().Damage(damage);
    //    }
    //}

}
