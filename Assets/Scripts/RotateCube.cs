using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RotateCube : MonoBehaviour
{
    public float spinForce;
    bool isHeld = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, spinForce * Time.deltaTime,0);

        if(isHeld)
        {
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
            RaycastHit hit;
            transform.position = ray.GetPoint(3);
        }
    }
    public void ChangeSpin()
    {
        spinForce = -spinForce;
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
