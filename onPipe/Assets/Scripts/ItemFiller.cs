using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ItemFiller : MonoBehaviour
{
    public GameObject fillerItem;
    private List<GameObject> _fillerItems = new List<GameObject>();
    
    private bool _plucked = false;
    
    public void Start()
    {
        FillItem();
    }

    public void Update()
    {
        if(Game.gameState != GameStatus.Playing)
            return;
        
        if (!_plucked)
        {
            if (Ring.RingClosed && Math.Abs(Vector3.Distance(transform.position,Vector3.zero)) < 0.2f)
            {   
                Game.bucketFill++;
                _plucked = true;
                foreach (var t in _fillerItems)
                {
                    t.GetComponent<ItemHolder>().fillerAnimator.Play("pluckAnimation", 0, 0);
                }
            }
        }
    }

    public void FillItem()
    {
        var arcLength = fillerItem.transform.GetChild(0).localScale.x;
        
        var radius = transform.GetChild(0).localScale.x;
        float angle = arcLength / radius + 0.3f;
        Vector3 centre = transform.position;
        
        float i = 0;
        while(i < 2 * Mathf.PI)
        {
            Vector3 location = centre + new Vector3( 0.55f * radius * Mathf.Sin(i), 0,0.55f * radius * Mathf.Cos(i));
            var tempObj = Instantiate(fillerItem, location, Quaternion.identity, gameObject.transform);
            tempObj.transform.LookAt(transform);
            tempObj.transform.eulerAngles = new Vector3(0, tempObj.transform.eulerAngles.y, 0);
            i += angle;
            _fillerItems.Add(tempObj);
        }
    }
}