using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    public Vector3 upPos;
    public bool sand, scrapLand, fish, buildFish;
    bool move;
    [Header("---------New Move----------")]
    public float speed;
    Transform target;
    public float arcHeight;

    Vector3 _startPosition;
    float _stepScale;
    float _progress;

    public BuildLand land;
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
    public void SetState(string name)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(transform.GetChild(i).gameObject.name == name ? true : false);
        }
    }
    private void Update()
    {
        if (move)
        {
            _progress = Mathf.Min(_progress + Time.deltaTime * _stepScale, 1.0f);
            float parabola = 1.0f - 4.0f * (_progress - 0.5f) * (_progress - 0.5f);       
            Vector3 nextPos = Vector3.Lerp(_startPosition, target.position, _progress);
            nextPos.y += parabola * arcHeight;
            transform.LookAt(nextPos, transform.forward);
            transform.position = nextPos;
            if (_progress == 1.0f)
            {
                if (sand)
                {                   
                    BoxControll.Instance.AddSand();
                    sand = false;
                    gameObject.SetActive(false);
                }
                if(scrapLand)
                {
                    scrapLand = false;                   
                    transform.rotation = Quaternion.Euler(0, 0, 90);
                    transform.parent = target;
                }                
                if (buildFish)
                {
                    buildFish = false;
                    target.GetComponent<BuildFish>().SpawnFish();
                    gameObject.SetActive(false);
                }
            }     
        }       
    }

    public IEnumerator DoMove(float time, Transform trgt)
    {
        target = trgt;
        _progress = 0;
        _startPosition = transform.position;
        float distance = Vector3.Distance(_startPosition, target.position);
        _stepScale = speed / distance;       
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
            BoxControll.Instance.AddSand();
            sand = false;
            gameObject.SetActive(false);
        }
        if (fish)
        {
            BoxControll.Instance.AddFish();
            fish = false;
            gameObject.SetActive(false);
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
