using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneControll : MonoBehaviour
{
    public static SceneControll Instance;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        if (Instance == null)
            Instance = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
