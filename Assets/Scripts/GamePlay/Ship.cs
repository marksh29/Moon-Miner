using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    [SerializeField] float moveTimeToSpawn, stayTime, endMoveTime, waitTimer;
    [SerializeField] Transform startPos, spawnPos, endPos;
    Transform target;
    float timer;
    string status;
    void Start()
    {
        timer = waitTimer;
        status = "Wait";
    }

    // Update is called once per frame
    void Update()
    {
        if(status == "Wait" || status == "Stay")
            timer -= Time.deltaTime;
        if(timer <= 0)
            ChangeStatus();
    }
    void ChangeStatus()
    {
        switch (status)
        {
            case ("Wait"):
                StartCoroutine(DoMove(moveTimeToSpawn, new Vector3(spawnPos.position.x, transform.position.y, startPos.position.z)));
                status = "MoveSpawn";
                break;
            case ("Stay"):
                StartCoroutine(DoMove(moveTimeToSpawn, new Vector3(endPos.position.x, transform.position.y, endPos.position.z)));
                status = "MoveEnd";
                break;
        }
    }
    private IEnumerator DoMove(float time, Vector3 targetPosition)
    {
        Vector3 startPosition = transform.position;
        float startTime = Time.realtimeSinceStartup;
        float fraction = 0f;
        while (fraction < 1f)
        {
            fraction = Mathf.Clamp01((Time.realtimeSinceStartup - startTime) / time);
            transform.position = Vector3.Lerp(startPosition, targetPosition, fraction);
            yield return null;
        }
        switch (status)
        {
            case ("MoveSpawn"):
                timer = stayTime;                
                spawnPos.gameObject.GetComponent<HostSpawner>().SpawnHosts();
                status = "Stay";
                break;
            case ("MoveEnd"):
                timer = waitTimer;
                transform.position = new Vector3(startPos.position.x, transform.position.y, startPos.position.z);
                status = "Wait";
                break;
        }
    }
}
