using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelScale : MonoBehaviour
{
    [SerializeField] GameObject barrel;
    [SerializeField] float minScale, maxScale, addScale, curScale, scaleSpeed;
    bool scale_on;

    void Start()
    {
        curScale = minScale;
        SetScale(0);
    }
    void Update()
    {
        if(scale_on)
        {
            if (barrel.transform.localScale.x < curScale)
            {
                barrel.transform.localScale += new Vector3(scaleSpeed, scaleSpeed, scaleSpeed) * Time.deltaTime;
                if (barrel.transform.localScale.x > curScale)
                {
                    scale_on = false;
                    barrel.transform.localScale = new Vector3(curScale, curScale, curScale);
                }
                   
            }
            if (barrel.transform.localScale.x > curScale)
            {
                barrel.transform.localScale -= new Vector3(scaleSpeed, scaleSpeed, scaleSpeed) * Time.deltaTime;
                if (barrel.transform.localScale.x < curScale)
                {
                    scale_on = false;
                    barrel.transform.localScale = new Vector3(curScale, curScale, curScale);
                }                    
            }
        }        
    }
    public void SetScale(int count)
    {
        curScale = minScale + addScale * count;
        if (curScale > maxScale)
            curScale = maxScale;
        barrel.SetActive(curScale <= minScale ? false : true);
        scale_on = true;
    }
}
