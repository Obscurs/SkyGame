using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Grabable : MonoBehaviour
{
    bool isHeld = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if(isHeld)
        {
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
            RaycastHit hit;
            transform.position = ray.GetPoint(3);
        }
    }
    public void PointerDragStart(BaseEventData eventData)
    {
        isHeld = true;
    }
    public void PointerDragEnd(BaseEventData eventData)
    {
        isHeld = false;
    }
}
