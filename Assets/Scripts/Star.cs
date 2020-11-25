using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    // Start is called before the first frame update
    public List<Star> connectedStars;
    public List<Star> currentConnectedStars;
    public bool currentValid = false;
    public bool alreadyFinished = false;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public bool areStarConnectionsCorrect()
    {
        foreach (Star s in connectedStars)
        {
            if (!currentConnectedStars.Contains(s))
                return false;
        }
        foreach (Star s in currentConnectedStars)
        {
            if (!connectedStars.Contains(s))
                return false;
        }
        return true;
    }
    public void connectStar(Star s)
    {
        if (!currentConnectedStars.Contains(s))
            currentConnectedStars.Add(s);
        else
            Debug.LogError("connectStar, star already connected");
    }
    public void clearConnections()
    {
        currentConnectedStars.Clear();
    }
}
