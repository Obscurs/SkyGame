using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    public enum State
    {
        Building,
        Saved,
        Validating,
        FadingOut
    }
    // Start is called before the first frame update
    public Vector3 pos1;
    public Vector3 pos2;
    public Star star1 = null;
    public Star star2 = null;
    public bool isValid = true;
    public State state = State.Building;
    float widthLine = 0.3f;
    Color colorLine = new Color(1,1,1,1);
    protected LineRenderer line;

    public bool IsEqual(Line l)
    {
        return ((l.pos1 == pos1 && l.pos2 == pos2) || (l.pos1 == pos2 && l.pos2 == pos1));
    }
    void Start()
    {
        line = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        line.SetPosition(0, pos1);
        line.SetPosition(1, pos2);
        switch(state)
        {
            case State.Building:
                widthLine = 0.5f;
                break;
            case State.Saved:
                if (widthLine != 0.3f)
                {
                    if(widthLine > 0.3f)
                    {
                        widthLine -= Time.deltaTime;
                        if (widthLine < 0.3f)
                        {
                            widthLine = 0.3f;
                        }
                    }
                    else
                    {
                        widthLine += Time.deltaTime;
                        if (widthLine > 0.3f)
                        {
                            widthLine = 0.3f;
                        }
                    }
                }
                break;
            case State.Validating:
                widthLine = Mathf.Sin(Time.time*3)*0.5f+0.7f;
                break;
            case State.FadingOut:
                if(widthLine > 0)
                {
                    widthLine -= Time.deltaTime;
                    if (widthLine < 0)
                    {
                        widthLine = 0;
                    }
                }
                break;
        }
        line.SetWidth(widthLine, widthLine);
    }
    public bool IsLineValid()
    {
        return star1.currentValid && star2.currentValid;
    }



}
