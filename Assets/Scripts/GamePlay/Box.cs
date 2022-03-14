using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    //[SerializeField] Rigidbody rb;
    //[SerializeField] float toTargetTime;
    //[SerializeField] Transform target;
    public Vector3 upPos;
    public bool sand;
    bool move;

    [Header("---------New Move----------")]
    public float speed;
    Vector3 target;
    public float arcHeight;

    Vector3 _startPosition;
    float _stepScale;
    float _progress;


    private void OnEnable()
    {
        move = false;
        int id = Random.Range(0, transform.childCount);
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(i == id ? true : false);
        }
    }
    void Start()
    {
        
    }
    private void Update()
    {
        if (move)
        {
            // Increment our progress from 0 at the start, to 1 when we arrive.
            _progress = Mathf.Min(_progress + Time.deltaTime * _stepScale, 1.0f);

            // Turn this 0-1 value into a parabola that goes from 0 to 1, then back to 0.
            float parabola = 1.0f - 4.0f * (_progress - 0.5f) * (_progress - 0.5f);

            // Travel in a straight line from our start position to the target.        
            Vector3 nextPos = Vector3.Lerp(_startPosition, target, _progress);

            // Then add a vertical arc in excess of this.
            nextPos.y += parabola * arcHeight;

            // Continue as before.
            transform.LookAt(nextPos, transform.forward);
            transform.position = nextPos;

            // I presume you disable/destroy the arrow in Arrived so it doesn't keep arriving.
            if (_progress == 1.0f)
            {
                if (sand)
                {                   
                    BoxControll.Instance.AddSand();
                    sand = false;
                    gameObject.SetActive(false);
                }
                //gameObject.SetActive(false);
            }     
        }       
    }

    public IEnumerator DoMove(float time, Transform trgt)
    {
        _progress = 0;
        _startPosition = transform.position;
        float distance = Vector3.Distance(_startPosition, target);
        _stepScale = speed / distance;
        target = trgt.position;
        move = true;
        yield return new WaitForSeconds(0);
    }
    public IEnumerator MoveSand(float time, Transform trgt)
    {
        transform.parent = trgt;
        transform.rotation = trgt.rotation;

        Vector3 startPosition = transform.localPosition;
        Vector3 targetPosition = new Vector3(0, 0, 0);

        float startTime = Time.realtimeSinceStartup;
        float fraction = 0f;
        while (fraction < 1f)
        {
            fraction = Mathf.Clamp01((Time.realtimeSinceStartup - startTime) / time);
            transform.localPosition = Vector3.Lerp(startPosition, targetPosition, fraction);
            yield return null;
        }
        if (sand)
        {
            gameObject.SetActive(false);
            BoxControll.Instance.AddSand();
            sand = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "AddScrap")
        {
            gameObject.SetActive(false);
            other.gameObject.transform.parent.parent.GetComponent<Factoria>().AddScrap();
        }        
    }  
}
