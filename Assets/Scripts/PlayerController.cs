using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;





public class PlayerController : MonoBehaviour
{
    public Material levelMat1;
    public Material levelMat2;
    public Material levelMat3;

    public GameObject envLight1;
    public GameObject envLight2;
    public GameObject envLight3;
    public GameObject arrowObject;
    public Line linePrefab;
    float playerSpeed = 10.0f;
    Vector3 m_targetPosition;
    public List<Line> m_savedLines;
    public List<Line> m_currentLines;
    public Line m_currentLine = null;
    bool isDrawing = false;
    bool m_drawingsOn = false;
    bool zoomingIn = false;
    float m_timerElapsedNotDrawing = 0;
    Star m_currentLineFirstPos;
    Vector3 m_currentLineSecondPos;
    public GameObject m_constelations;

    float minFov = 20f;
    float maxFov;
    // Start is called before the first frame update

    void Awake()
    {
        
    }
    void Start()
    {
        maxFov = Camera.main.fieldOfView;
        m_targetPosition = transform.position;
        m_constelations = GameObject.Find("Constelations");
        envLight1 = GameObject.Find("Env1");
        envLight2 = GameObject.Find("Env2");
        envLight3 = GameObject.Find("Env3");
        arrowObject = GameObject.Find("penModel");
        envLight1.SetActive(true);
        envLight2.SetActive(false);
        envLight3.SetActive(false);
        RenderSettings.skybox = levelMat1;
        //arrowAnim = gameObject.transform.Find("ArrowModel").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(zoomingIn)
        {
            if (Camera.main.fieldOfView > minFov)
                Camera.main.fieldOfView -= Time.deltaTime * 200;
        }
        else
        {
            if (Camera.main.fieldOfView < maxFov)
                Camera.main.fieldOfView += Time.deltaTime * 200;
        }
        if(m_targetPosition != transform.position)
        {
            float step = playerSpeed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, m_targetPosition, step);

            //transform.position = m_targetPosition;
        }
        if(isDrawing)
        {
            m_currentLine.pos1 = m_currentLineFirstPos.transform.position;
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                //print("I'm looking at " + hit.point);
                m_currentLineSecondPos = hit.point;
                m_currentLine.pos2 = m_currentLineSecondPos;
            }
            else
            {
                //print("I'm looking at nothing!");
            }
            m_timerElapsedNotDrawing = 0;
            m_drawingsOn = true;
        }
        else
        {
            if(m_drawingsOn)
            {
                m_timerElapsedNotDrawing += Time.deltaTime;
            }
        }
        if(m_drawingsOn && m_timerElapsedNotDrawing > 5)
        {
            ValidateConstelations();
            m_drawingsOn = false;
            m_timerElapsedNotDrawing = 0;
        }
        
    }
    public void SetMoveTarget(BaseEventData eventData)
    {
        PointerEventData ped = eventData as PointerEventData;
        if (ped != null)
        {
            Vector3 p = ped.pointerCurrentRaycast.worldPosition;
            
            if(p.magnitude < 15)
            {
                m_targetPosition.x = p.x;
                m_targetPosition.z = p.z;
                m_targetPosition.y = p.y+4;
            }

        }
    }
    public void StarStartDrag(BaseEventData eventData)
    {
        Debug.Log("StartDrag nen");
        if (isDrawing)
            return;
        PointerEventData ped = eventData as PointerEventData;
        if (ped.pointerCurrentRaycast.gameObject.GetComponent<Star>().alreadyFinished)
            return;
        isDrawing = true;
        foreach (Line l in m_currentLines)
        {
            l.state = Line.State.Building;
        }
        m_drawingsOn = true;
        //m_currentLineFirstPos = ped.selectedObject.transform.position;
        m_currentLineFirstPos = ped.pointerCurrentRaycast.gameObject.GetComponent<Star>();
        m_currentLine = Instantiate(linePrefab);
        m_currentLine.pos1 = m_currentLineFirstPos.transform.position;
        m_currentLine.pos2 = m_currentLineFirstPos.transform.position;
        m_currentLine.enabled = true;

    }
    public void ClearConstelationCurrentLines()
    {
        int numConstelations = m_constelations.transform.childCount;
        for (int i = 0; i < numConstelations; ++i)
        {
            GameObject currConst = m_constelations.transform.GetChild(i).gameObject;

            int numStars = currConst.transform.childCount;
            bool isValid = true;
            for (int j = 0; j < numStars; ++j)
            {
                GameObject currStar = currConst.transform.GetChild(j).gameObject.transform.GetChild(0).gameObject;
                currStar.GetComponent<Star>().clearConnections();
            }
            
        }
    }
    public void ValidateConstelations()
    {
        int numConstelations = m_constelations.transform.childCount;
        for (int i = 0; i < numConstelations; ++i)
        {
            GameObject currConst = m_constelations.transform.GetChild(i).gameObject;

            int numStars = currConst.transform.childCount;
            bool isValid = true;
            for (int j = 0; j < numStars; ++j)
            {
                GameObject currStar = currConst.transform.GetChild(j).gameObject.transform.GetChild(0).gameObject;
                if (currStar.GetComponent<Star>().alreadyFinished)
                    break;
                if (!currStar.GetComponent<Star>().areStarConnectionsCorrect())
                {
                    Debug.Log("Connections incorrect for star: "+ currConst.transform.GetChild(j).gameObject.name);
                    isValid = false;
                }
            }
            for (int j = 0; j < numStars; ++j)
            {
                
                GameObject currStar = currConst.transform.GetChild(j).gameObject.transform.GetChild(0).gameObject;
                if (currStar.GetComponent<Star>().alreadyFinished)
                    break;
                currStar.GetComponent<Star>().currentValid = isValid;
                currStar.GetComponent<Star>().alreadyFinished = isValid;
            }
        }
        ClearConstelationCurrentLines();
        foreach (Line l in m_currentLines)
        {
            if (l.IsLineValid())
            {
                Debug.Log("Line is valid, adding to saved lines");
                l.state = Line.State.Saved;
                m_savedLines.Add(l);
            }
            else
            {
                l.state = Line.State.FadingOut;
                Destroy(l.gameObject, 3);
            }
                

        }
        m_currentLines.Clear();
    }
    public void StarEnter(BaseEventData eventData)
    {
        Debug.Log("StarEnter nen");

        PointerEventData ped = eventData as PointerEventData;
        Vector3 starPos = ped.pointerCurrentRaycast.gameObject.transform.position;
        if(!ped.pointerCurrentRaycast.gameObject.GetComponent<Star>().alreadyFinished)
            arrowObject.GetComponent<Animator>().SetBool("pointingStar", true);
        if (!isDrawing)
            return;
        
        
        if (ped.pointerCurrentRaycast.gameObject.GetComponent<Star>().alreadyFinished)
            return;
        //Vector3 starPos = 

        if (starPos == m_currentLineFirstPos.transform.position)
            return;

        
        Line newline = Instantiate(linePrefab);
        newline.pos1 = m_currentLineFirstPos.transform.position;
        newline.pos2 = starPos;
        newline.enabled = true;
        foreach (Line l in m_savedLines)
        {
            if (l.IsEqual(newline))
            {
                Destroy(newline.gameObject);
                return;
            }
        }
        foreach (Line l in m_currentLines)
        {
            if (l.IsEqual(newline))
            {
                Destroy(newline.gameObject);
                return;
            }
        }
        newline.star1 = m_currentLineFirstPos;
        newline.star2 = ped.pointerCurrentRaycast.gameObject.GetComponent<Star>();

        newline.star1.connectStar(newline.star2);
        newline.star2.connectStar(newline.star1);
        m_currentLines.Add(newline);
        m_currentLineFirstPos = ped.pointerCurrentRaycast.gameObject.GetComponent<Star>();
        m_currentLine.pos1 = m_currentLineFirstPos.transform.position;
        //Gizmos.DrawLine(transform.position, target.position);
    }
    public void StarExit(BaseEventData eventData)
    {
        arrowObject.GetComponent<Animator>().SetBool("pointingStar", false);
        Debug.Log("StarExit nen");
    }

    public void StarEndDrag(BaseEventData eventData)
    {
        Debug.Log("EndDrag nen");
        foreach(Line l in m_currentLines)
        {
            l.state = Line.State.Validating;
        }
        isDrawing = false;
        if(m_currentLine)
            Destroy(m_currentLine.gameObject);
        m_currentLine = null;
        /*
        PointerEventData ped = eventData as PointerEventData;

        isDrawing = true;
        m_drawingsOn = true;
        m_currentLineFirstPos = ped.selectedObject.transform.position;
        */
    }

    public void SkyMove(BaseEventData eventData)
    {
        Debug.Log("Sky nen");

    }

    public void MapZoomIn(BaseEventData eventData)
    {
        zoomingIn = true;

    }
    public void MapZoomOut(BaseEventData eventData)
    {
        zoomingIn = false;
    }

    public void SetSkybox1()
    {
        RenderSettings.skybox = levelMat1;
        m_constelations.SetActive(true);
        envLight1.SetActive(true);
        envLight2.SetActive(false);
        envLight3.SetActive(false);
        RenderSettings.fog = true;
    }
    public void SetSkybox2()
    {
        RenderSettings.skybox = levelMat2;
        m_constelations.SetActive(true);
        envLight1.SetActive(false);
        envLight2.SetActive(true);
        envLight3.SetActive(false);
        RenderSettings.fog = false;
    }
    public void SetSkybox3()
    {
        RenderSettings.skybox = levelMat3;
        m_constelations.SetActive(false);
        envLight1.SetActive(false);
        envLight2.SetActive(false);
        envLight3.SetActive(true);
        RenderSettings.fog = false;
    }
}
