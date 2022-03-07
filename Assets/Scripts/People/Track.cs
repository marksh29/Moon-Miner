using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track : MonoBehaviour
{
    [SerializeField] float speed, _rotateSpeed;
    public Transform _target;
    Vector3 relativePos;
    public bool move, tower, stay;
    [SerializeField] GameObject effect;

    void Start()
    {
        //Player.Instance.AddTrack(gameObject);
    }
    void Update()
    {
        if (move && !tower && !stay && (transform.position - _target.position).sqrMagnitude > 0.2f)
        {
            GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            relativePos = _target.position - transform.position;
            Quaternion toRotation = Quaternion.LookRotation(relativePos);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, _rotateSpeed * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(_target.position.x, _target.position.y, _target.position.z), speed * Time.deltaTime);
        }
        if (tower)
        {
            relativePos = new Vector3(_target.position.x, _target.position.y, _target.position.z) - transform.position;
            Quaternion toRotation = Quaternion.LookRotation(relativePos);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, _rotateSpeed * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(_target.position.x, _target.position.y, _target.position.z), speed * Time.deltaTime);
        }
    }

    public void StartMove(Transform target)
    {
        move = true;
        _target = target;
    }
    public void MoveToPlayer(Transform playerPos)
    {
        gameObject.layer = 8;
        GetComponent<Rigidbody>().isKinematic = false;
        speed = speed + 2;
        _target = playerPos;
        tower = true;
    }

    private void OnCollisionEnter(Collision coll)
    {
        if(coll.gameObject.tag == "Player" && tower)
        {            
            StartCoroutine(Player.Instance.AddScale());
            Destroy(gameObject);
        }
    }
}
