using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;
public class Player : MonoBehaviour
{
    public enum joystickType
    {
        Static, Dinamic
    }

    public static Player Instance;
   
    [Header("Joystic")]
    public joystickType _joystick;
    public Rigidbody _rigidbody;
    [SerializeField] private FixedJoystick _joystickS;
    [SerializeField] private DynamicJoystick _joystickD;
   
    [Header("Player")]
    [SerializeField] Transform cylindr;
    [SerializeField] Transform[] well;
    [SerializeField] private float _moveSpeed, _rotSpeed;   
    public bool move, drop;

    [Header("Track")]
    
    [SerializeField] int _lineWidth;
    [SerializeField] List<GameObject> _trackList;
    [SerializeField] Transform[] _trackPos;
    Vector3 oldPositions;

    [Header("Scale")]
    [SerializeField] Transform[] startPos;
    [SerializeField] float addScale, maxScale;
    public static event Action _upgrade;

    [Header("-------Camera-------")]
    [SerializeField] CinemachineVirtualCamera cam;
    [SerializeField] float camScaleAdd, camScale, maxCamScale;

    private void Awake()
    {       
        if (Instance == null) Instance = this;
        if (_joystick == joystickType.Static)
            _joystickS.gameObject.SetActive(true);
        else
            _joystickD.gameObject.SetActive(true);       
    }
    private void Start()
    {
        
    }
    private void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //    StartCoroutine(AddScale());
        //if (Input.GetKeyDown(KeyCode.Q))
        //    MoveToPlayer();
    }
    private void FixedUpdate()
    {
        if (Controll.Instance._state == "Game" && !drop)
        {
            //cylindr.Rotate(-Vector3.up * _rotSpeed * Time.deltaTime);
            _rigidbody.velocity = new Vector3(Joyctick("X") * _moveSpeed, 0, Joyctick("Y") * _moveSpeed);

            if (Joyctick("X") >= 0.1f || Joyctick("Y") >= 0.1f || Joyctick("X") <= -0.1f || Joyctick("Y") <= -0.1f)
            {
                transform.rotation = Quaternion.LookRotation(_rigidbody.velocity);

                cylindr.Rotate(-Vector3.up * _rotSpeed * Time.deltaTime);
                for (int i = 0; i < well.Length; i++)
                {
                    well[i].Rotate(-Vector3.right * _rotSpeed * Time.deltaTime);
                }              
            }
            else
            {
                cylindr.Rotate(-Vector3.up * (_rotSpeed/3) * Time.deltaTime);
            }

            if (cam.GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance < camScale)
                cam.GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance += camScaleAdd;
            if (cam.GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance > camScale)
                cam.GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance -= camScaleAdd;
        }
    }
    void MoveToPlayer()
    {
        for (int i = 0; i < _trackList.Count; i++)
        {
            if (i < 2)
                _trackList[i].transform.parent = null;
            _trackList[i].GetComponent<Track>().MoveToPlayer(gameObject.transform);           
        }
        _trackList.Clear();
    }
    public void AddTrack(GameObject obj)
    {
        int id = _trackList.Count;
        if (_trackList.Count < 2)
        {
            obj.GetComponent<Track>().stay = true;
            obj.transform.parent = startPos[_trackList.Count + 1].transform;
            obj.transform.localPosition = new Vector3(0, 0, 0);
            obj.transform.localRotation = startPos[_trackList.Count + 1].transform.localRotation;
        }
        else if (_trackList.Count >= 2 && _trackList.Count < 5)
        {
            obj.GetComponent<Track>().StartMove(startPos[id - 2].transform.GetChild(0));
            obj.transform.position = startPos[id - 2].transform.GetChild(0).position;
            obj.transform.rotation = startPos[id - 2].transform.GetChild(0).rotation;
            //obj.transform.LookAt(startPos[id - 2].transform);
        }
        else
        {
            obj.GetComponent<Track>().StartMove(_trackList[id - 3].transform.GetChild(0));
            obj.transform.position = _trackList[id - 3].transform.GetChild(0).position;
            obj.transform.rotation = _trackList[id - 3].transform.GetChild(0).rotation;
            //obj.transform.LookAt(_trackList[id - 3].transform);
        }
        _trackList.Add(obj);
        //hostCountText.text = _hostageList.Count > 0 ? (_hostageList.Count + 1).ToString() : "1";
    }  

    float Joyctick(string name)
    {
        float XY = new float();
        switch (_joystick)
        {
            case (joystickType.Static):
                if (name == "X")
                    XY = _joystickS.Horizontal;
                else
                    XY = _joystickS.Vertical;
                break;
            case (joystickType.Dinamic):
                if (name == "X")
                    XY = _joystickD.Horizontal;
                else
                    XY = _joystickD.Vertical;
                break;
        }
        return XY;
    }

    private void OnTriggerEnter(Collider coll)
    {
        if(coll.gameObject.tag =="Update")
        {
            Controll.Instance.UpgradeOn(true);
        }
        if (coll.gameObject.tag == "Multiple")
        {
            MoveToPlayer();
            //coll.gameObject.SetActive(false);
        }
        if(coll.gameObject.tag == "Boost")
        {
            coll.gameObject.GetComponent<Boost>().SetBoost((_lineWidth * startPos.Length) - _trackList.Count);
        }
    }
    private void OnTriggerExit(Collider coll)
    {
        if (coll.gameObject.tag == "Update")
        {
            Controll.Instance.UpgradeOn(false);
        }
    }   
    public IEnumerator AddScale()
    {
        if(camScale < maxCamScale)
            camScale += camScaleAdd;

        PlayerPrefs.SetInt("upgrade0", PlayerPrefs.GetInt("upgrade0") + 1);
        PlayerPrefs.SetInt("upgrade1", PlayerPrefs.GetInt("upgrade1") + 1);

        transform.localScale += new Vector3(addScale, addScale, addScale);
        transform.position = new Vector3(transform.position.x, transform.position.y + (0.01f * PlayerPrefs.GetInt("upgrade0")), transform.position.z);

        yield return new WaitForSeconds(0.2f);
        if(transform.localScale.x > maxScale)
            transform.localScale -= new Vector3(addScale, addScale, addScale);
                   
        _upgrade.Invoke();
        
    }
}