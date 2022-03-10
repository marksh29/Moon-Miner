using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scrap : MonoBehaviour
{
    public int id;
    [SerializeField] float life, getDamage, force;
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
        id = Random.Range(0, transform.childCount);
        transform.GetChild(id).gameObject.SetActive(true);
        mat = allMats[id < allMats.Length ? id : 0];
    }
    void Update()
    {
        if(damageOn)
        {
            life -= getDamage;
            if (life <= 0)
            {
                BoxControll.Instance.SpawnFlyScrap(this);
                gameObject.SetActive(false);
            }
        }             
    }
    public void Damage(float id, bool bl)
    {
        if(bl)
        {
            Vector3 vect = player.position - transform.position;
            GetComponent<Rigidbody>().AddForce(vect * force, ForceMode.Impulse);
        }
        damageOn = bl;
        getDamage = id;
    }
}
