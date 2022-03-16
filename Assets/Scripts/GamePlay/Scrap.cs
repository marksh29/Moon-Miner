using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scrap : MonoBehaviour
{
    public int id;
    [SerializeField] float life, getDamage, force, forceUp;
    public Material mat;
    [SerializeField] Material[] allMats;
    public bool damageOn;
    Transform player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
        GetComponent<MeshRenderer>().enabled = false;
    }
    void Start()
    {
        id = Random.Range(1, transform.childCount);
        transform.GetChild(id).gameObject.SetActive(true);
        mat = allMats[id < allMats.Length ? id : 0];
    }
    void Update()
    {
        if (damageOn)
        {
            life -= getDamage;
            if (life <= 0)
            {
                BoxControll.Instance.SpawnFlyScrap(this);
                OffTrigger();                
            }
        }
    }
    public void Damage(float id, bool bl)
    {
        if (bl)
        {
            Vector3 vect = transform.position - player.position;
            GetComponent<Rigidbody>().AddForce(new Vector3(vect.x, vect.y + forceUp, vect.z) * force, ForceMode.Impulse);
        }
        damageOn = bl;
        getDamage = id;
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Drop")
        {
            other.GetComponent<Build>().CleareOff();
        }
        if (other.gameObject.tag == "DropSand")
        {
            other.GetComponent<BuildLand>().CleareOff();
        }
    }
    void OffTrigger()
    {
        List<GameObject> list = new List<GameObject>(GameObject.FindGameObjectsWithTag("Drop"));
        for (int i = 0; i < list.Count; i++)
        {
            list[i].GetComponent<Build>().CleareOff();
        }
        List<GameObject> list2 = new List<GameObject>(GameObject.FindGameObjectsWithTag("DropSand"));
        for (int i = 0; i < list2.Count; i++)
        {
            list2[i].GetComponent<BuildLand>().CleareOff();
        }
        List<GameObject> list3 = new List<GameObject>(GameObject.FindGameObjectsWithTag("DropFish"));
        for (int i = 0; i < list3.Count; i++)
        {
            list3[i].GetComponent<BuildFish>().CleareOff();
        }
        gameObject.SetActive(false);
    }
}
