using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scrap : MonoBehaviour
{
    
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
        int i = Random.Range(0, transform.childCount);
        transform.GetChild(i).gameObject.SetActive(true);
        mat = allMats[i < allMats.Length ? i : 0];
    }

    // Update is called once per frame
    void Update()
    {
        if(damageOn)
        {
            life -= getDamage;
            if (life <= 0)
            {
                BoxControll.Instance.SpawnFlyScrap(gameObject);
                gameObject.SetActive(false);
            }
        }
        //if (GetComponent<Rigidbody>().velocity.sqrMagnitude > 10 || GetComponent<Rigidbody>().velocity.sqrMagnitude < -10)
        //{
        //    GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x/10, 0, GetComponent<Rigidbody>().velocity.z/10);
        //}           
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
