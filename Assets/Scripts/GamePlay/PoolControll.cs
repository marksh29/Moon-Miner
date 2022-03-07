using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PoolControll : MonoBehaviour
{
    public static PoolControll Instance;
    [SerializeField] private GameObject pl_bullet, en_bullet, boss_bullet, nerf_bullet;
    [SerializeField] private List<GameObject> player_stack, enemy_stack, boss_stack, nerf_stack;
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
            case ("Player"):
                new_obj = Spawn(player_stack, pl_bullet);
                break;
            case ("Enemy"):
                new_obj = Spawn(enemy_stack, en_bullet);
                break;
            case ("Boss"):
                new_obj = Spawn(boss_stack, boss_bullet);
                break;
            case ("Nerf"):
                new_obj = Spawn(nerf_stack, nerf_bullet);
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
        for (int i = 0; i < player_stack.Count; i++)
        {
            player_stack[i].SetActive(false);
        }
        for (int i = 0; i < enemy_stack.Count; i++)
        {
            enemy_stack[i].SetActive(false);
        }
        for (int i = 0; i < boss_stack.Count; i++)
        {
            boss_stack[i].SetActive(false);
        }
        for (int i = 0; i < boss_stack.Count; i++)
        {
            nerf_stack[i].SetActive(false);
        }
    }
}
