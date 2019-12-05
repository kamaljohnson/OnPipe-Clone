using UnityEngine;

public class ItemFiller : MonoBehaviour
{

    public GameObject fillerItem;
    
    public int FillerLifeSpan;
    public void Start()
    {
        FillItem();
        Destroy(gameObject, FillerLifeSpan);
    }

    public void FillItem()
    {
        var arcLength = fillerItem.transform.GetChild(0).localScale.x;
        
        var radius = transform.GetChild(0).localScale.x;
        float angle = arcLength / radius;
        Vector3 centre = transform.position;
        
        float i = 0;
        while(i < 2 * Mathf.PI)
        {
            Vector3 location = centre + new Vector3( 0.55f * radius * Mathf.Sin(i), 0,0.55f * radius * Mathf.Cos(i));
            var tempObj = Instantiate(fillerItem, location, Quaternion.identity, gameObject.transform);
            tempObj.transform.LookAt(transform);
            tempObj.transform.eulerAngles = new Vector3(0, tempObj.transform.eulerAngles.y, 0);
            i += angle;
        }
    }
}