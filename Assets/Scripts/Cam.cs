using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] float speed;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    void Update()
    {
        if(Controll.Instance._state == "Game")
        {
            if (Camera.main.WorldToScreenPoint(player.transform.position).x > Screen.width * 0.6f)
            {
                transform.Translate(Vector3.right * speed * Time.deltaTime);
            }
            if (Camera.main.WorldToScreenPoint(player.transform.position).x < Screen.width * 0.4f)
            {
                transform.Translate(Vector3.left * speed * Time.deltaTime);
            }
            if (Camera.main.WorldToScreenPoint(player.transform.position).y > Screen.height * 0.5f)
            {
                transform.Translate(Vector3.forward * speed * Time.deltaTime);
            }
            if (Camera.main.WorldToScreenPoint(player.transform.position).y < Screen.height * 0.4f)
            {
                transform.Translate(-Vector3.forward * speed * Time.deltaTime);
            }
        }       
    }
}
