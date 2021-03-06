using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PoolControll : MonoBehaviour
{
    public static PoolControll Instance;
    [SerializeField] private GameObject box, fly_box, sand, drop_sand, barrelDrop, fish;//, en_bullet, boss_bullet, nerf_bullet;
    [SerializeField] private List<GameObject> box_stack, fly_box_stack, sand_stack, drop_sand_stack, barrelDrop_stack, fish_stack;//, enemy_stack, boss_stack, nerf_stack;
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
            case ("Sand"):
                new_obj = Spawn(sand_stack, sand);
                break;
            case ("DropSand"):
                new_obj = Spawn(drop_sand_stack, drop_sand);
                break;
            case ("BarrelDrop"):
                new_obj = Spawn(barrelDrop_stack, barrelDrop);
                break;
            case ("Fish"):
                new_obj = Spawn(fish_stack, fish);
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
        for (int i = 0; i < fly_box_stack.Count; i++)
        {
            fly_box_stack[i].SetActive(false);
        }
        for (int i = 0; i < drop_sand_stack.Count; i++)
        {
            drop_sand_stack[i].SetActive(false);
        }
        for (int i = 0; i < sand_stack.Count; i++)
        {
            sand_stack[i].SetActive(false);
        }
        for (int i = 0; i < barrelDrop_stack.Count; i++)
        {
            barrelDrop_stack[i].SetActive(false);
        }
        for (int i = 0; i < fish_stack.Count; i++)
        {
            fish_stack[i].SetActive(false);
        }
    }
}
