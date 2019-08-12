using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class ItemFiller : MonoBehaviour
{

    public GameObject fillerItem;
    
    public void Start()
    {
        FillItem();
    }

    public void FillItem()
    {
        var arcLength = 1f;
        
        var radius = transform.GetChild(0).localScale.x;
        float angle = arcLength / radius;
        Vector3 centre = transform.position;
        
        Debug.Log("r : " + radius + " s : " + arcLength + " a : " + angle + " d : " + 2 * Mathf.PI / angle);

        float i = 0;
        while(i < 2 * Mathf.PI)
        {
            Vector3 location = centre + new Vector3( 0.55f * radius * Mathf.Sin(i), 0,0.55f * radius * Mathf.Cos(i));
            var tempObj = Instantiate(fillerItem, location, Quaternion.identity, gameObject.transform);
            tempObj.transform.LookAt(transform);
            tempObj.transform.eulerAngles = new Vector3(0, tempObj.transform.eulerAngles.y, 0);
            tempObj.transform.localScale = Vector3.one * arcLength * 0.55f;
            i += angle;
            Debug.Log("cube");
        }
    }
}