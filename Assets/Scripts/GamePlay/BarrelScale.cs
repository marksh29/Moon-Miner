using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelScale : MonoBehaviour
{
    [SerializeField] GameObject barrel;
    [SerializeField] float minScale, maxScale, addScale, curScale, scaleSpeed;
        

    void Start()
    {
        curScale = minScale;
        SetScale(0);
    }
    void Update()
    {
        if (barrel.transform.localScale.x < curScale)
        {
            barrel.transform.localScale += new Vector3(scaleSpeed, scaleSpeed, scaleSpeed) * Time.deltaTime;
            if (barrel.transform.localScale.x > maxScale)
                barrel.transform.localScale = new Vector3(maxScale, maxScale, maxScale);
        }
        if (barrel.transform.localScale.x > curScale)
        {
            barrel.transform.localScale -= new Vector3(scaleSpeed, scaleSpeed, scaleSpeed) * Time.deltaTime;
            if (barrel.transform.localScale.x < minScale)
                barrel.transform.localScale = new Vector3(minScale, minScale, minScale);
        }
    }
    public void SetScale(int count)
    {
        curScale = minScale + addScale * count;
        //if (curScale > maxScale)
        //    curScale = maxScale;
        barrel.SetActive(curScale == minScale ? false : true);
    }
}
