using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Old : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    //[SerializeField] int count;
    //[SerializeField] float spawnTime, xyz;

    [SerializeField] float life, oldLife;

    bool end;
    void Start()
    {
        
    }
    public void Damage(float damage)
    {
        if (!end)
        {
            life -= damage;
            SpawnBox();
            if (life <= 0)
                Destroy();
        }
    }
    void Destroy()
    {
        end = true;
        //gameObject.SetActive(false);
    }
    //void Spawn()
    //{
    //    int widght = Mathf.FloorToInt(transform.localScale.z / prefab.transform.localScale.x);
    //    int height = Mathf.FloorToInt(transform.localScale.y / prefab.transform.localScale.y);
       
    //    float xx = prefab.transform.localScale.x;
        
    //    Vector3 startPos = new Vector3();
    //    if (transform.rotation.y == 0)
    //        startPos = new Vector3(transform.position.x, transform.position.y - (xx * (height/2)), transform.position.z - (xx * (widght / 2) - xx/2));
    //    else
    //        startPos = new Vector3(transform.position.x - (xx * (widght / 2) - xx / 2), transform.position.y - (xx * (height/2)), transform.position.z);

    //    SpawnPos(startPos, widght, height, Mathf.FloorToInt(count / (height * widght)), xx, transform.rotation.y == 0 ? false : true);
    //}
    //void SpawnPos(Vector3 startPos, float height, float widght, int deep, float xx, bool yy)
    //{
    //    for (int z = 0; z < deep + 1; z++)
    //    {
    //        for (int y = 0; y < widght; y++)
    //        {
    //            for (int x = 0; x < height; x++)
    //            {
    //                if (count > 0)
    //                {
    //                    GameObject obj = Instantiate(prefab) as GameObject;
    //                    if (!yy)
    //                        obj.transform.position = new Vector3(startPos.x + (xx * z) + xyz, startPos.y + (xx * y) + (xyz * y), startPos.z + (xx * x));
    //                    else
    //                        obj.transform.position = new Vector3(startPos.x + (xx * x) + (xyz * y), startPos.y + (xx * y) + (xyz * y), startPos.z + (xx * z) + xyz);

    //                    count--;
    //                }
    //                else
    //                    break;
    //            }
    //        }
    //    }
    //    if(count <= 0)
    //        gameObject.SetActive(false);                  
    //}
    void SpawnBox()
    {

    }
}
