using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PoolControll : MonoBehaviour
{
    public static PoolControll Instance;
    [SerializeField] private GameObject box, fly_box;//, en_bullet, boss_bullet, nerf_bullet;
    [SerializeField] private List<GameObject> box_stack, fly_box_stack;//, enemy_stack, boss_stack, nerf_stack;
    GameObject new_obj, obj;

    private void Start()
    {
        
    }
    private void Awake()
    {
        if (Instance == null)
            Instance = this;        
    }   
    public GameObject Spawn(string name)
    {
        switch (name)
        {
            case ("Box"):
                new_obj = Spawn(box_stack, box);
                break;
            case ("FlyBox"):
                new_obj = Spawn(fly_box_stack, fly_box);
                break;
        }
        return new_obj;       
    }
    GameObject Spawn(List<GameObject> list, GameObject prefab)
    {
        bool not_empty = false;
        for (int i = 0; i < list.Count; i++)
        {
            if (!list[i].activeSelf)
            {
                list[i].SetActive(true);
                obj = list[i];
                not_empty = true;
                break;
            }
        }
        if (not_empty == false)
        {
            GameObject new_obj = Instantiate(prefab) as GameObject;
            new_obj.SetActive(true);
            obj = new_obj;
            list.Add(new_obj);
        }
        return obj;
    } 
    public void DisableAll()
    {
        for (int i = 0; i < box_stack.Count; i++)
        {
            box_stack[i].SetActive(false);
        }
        for (int i = 0; i < box_stack.Count; i++)
        {
            fly_box_stack[i].SetActive(false);
        }
    }
}
