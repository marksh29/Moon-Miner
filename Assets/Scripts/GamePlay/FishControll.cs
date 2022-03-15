using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishControll : MonoBehaviour
{
    [Header("Options")]
    [SerializeField] float speed;
    [SerializeField] float rotSpeed;

    [Header("Gameplay")]
    public Vector3 target;
    Vector3 relativePos;
    [SerializeField] Transform[] list;
      
    public void StartMove(Transform[] ls)
    {
        transform.localPosition = new Vector3(0, 0, 0);
        list = ls;
        //transform.position = new Vector3(Random.Range(-list[0].localPosition.x, list[0].localPosition.x), 0, Random.Range(-list[0].localPosition.z, list[0].localPosition.z));
        NewPosition();
    }

    void Update()
    {
        relativePos = new Vector3(target.x, target.y, target.z) - transform.localPosition;
        Quaternion toRotation = Quaternion.LookRotation(relativePos);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, toRotation, rotSpeed * Time.deltaTime);

        transform.localPosition = Vector3.MoveTowards(transform.localPosition, target, speed * Time.deltaTime);
        if (transform.localPosition == target)
            NewPosition();
    }
    public void NewPosition()
    {
        target = new Vector3(Random.Range(-list[0].localPosition.x, list[0].localPosition.x), 0, Random.Range(-list[1].localPosition.z, list[1].localPosition.z));
    }
}
