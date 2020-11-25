using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    // Start is called before the first frame update
    Animator arrowAnim;

    void Start()
    {
        arrowAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Show()
    {
        if(gameObject.name=="arrowModel")
            arrowAnim.SetBool("pointingFloor", true);
        else if (gameObject.name == "gearModel")
            arrowAnim.SetBool("pointingUsable", true);
        else if (gameObject.name == "penModel")
            arrowAnim.SetBool("pointingStar", true);
        else if (gameObject.name == "handModel")
            arrowAnim.SetBool("pointingGrabable", true);
        else if (gameObject.name == "lupeModel")
            arrowAnim.SetBool("pointingZoomable", true);
    }
    public void Hide()
    {
        if (gameObject.name == "arrowModel")
            arrowAnim.SetBool("pointingFloor", false);
        else if (gameObject.name == "gearModel")
            arrowAnim.SetBool("pointingUsable", false);
        else if (gameObject.name == "penModel")
            arrowAnim.SetBool("pointingStar", false);
        else if (gameObject.name == "handModel")
            arrowAnim.SetBool("pointingGrabable", false);
        else if (gameObject.name == "lupeModel")
            arrowAnim.SetBool("pointingZoomable", false);
    }
}
