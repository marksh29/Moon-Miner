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
   
    [Header("---------------Joystic---------------")]
    public joystickType _joystick;
    public Rigidbody _rigidbody;
    [SerializeField] private FixedJoystick _joystickS;
    [SerializeField] private DynamicJoystick _joystickD;

    [Header("---------------Player---------------")]
    [SerializeField] GameObject waterPrticle;
    [SerializeField] private float _moveSpeed, _wallSpeed;
    [SerializeField] Transform body, wall;   

    [Header("---------------Attack---------------")]
    [SerializeField] float damage;
    [SerializeField] bool attack;
      
    private void Start()
    {
        if (Instance == null) Instance = this;
        if (_joystick == joystickType.Static)
            _joystickS.gameObject.SetActive(true);
        else
            _joystickD.gameObject.SetActive(true);
    }
    private void Update()
    {
    }
    private void FixedUpdate()
    {
        if (Controll.Instance._state == "Game")
        {        
            _rigidbody.velocity = new Vector3(Joyctick("X") * _moveSpeed, 0, Joyctick("Y") * _moveSpeed);
            if (Joyctick("X") >= 0.1f || Joyctick("Y") >= 0.1f || Joyctick("X") <= -0.1f || Joyctick("Y") <= -0.1f)
            {
                transform.rotation = Quaternion.LookRotation(_rigidbody.velocity);
                wall.Rotate(Vector3.right * _wallSpeed * Time.deltaTime);
                waterPrticle.SetActive(true);
            }
            else
            {
                wall.Rotate(Vector3.right * _wallSpeed/5 * Time.deltaTime);
                waterPrticle.SetActive(false);
            }
        }
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
}