using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathwayController : MonoBehaviour
{
    public Transform activator;
    public float activatorRadius;
    public Transform GFXParent;
    public float offsetDistance;
    public float timeToFinish;
    public float timeToFinishRandomness;

    Vector3[] realPositions;
    Vector3 [] originalPositions;
    Vector3 [] startPositions;
    float[] completedAmounts;
    float[] startTimes;
    // Start is called before the first frame update
    void Start()
    {
        int pathCount = GFXParent.childCount;
        realPositions = new Vector3[pathCount];
        originalPositions = new Vector3[pathCount];
        startPositions = new Vector3[pathCount];
        completedAmounts = new float[pathCount];
        startTimes = new float[pathCount];
        int i = 0;

        foreach(Transform child in GFXParent)
		{
            realPositions[i] = child.position;
            originalPositions[i] = child.localPosition;
            Vector3 newPos = child.localPosition;
            newPos.y -= offsetDistance;
            child.localPosition = newPos;
            startPositions[i] = child.localPosition;
            completedAmounts[i] = 0;
            startTimes[i] = -1;
            i++;
		}
    }

    // Update is called once per frame
    void Update()
    {
        int i = 0;
        foreach(Transform child in GFXParent)
        {
            if(startTimes[i] < 0)
			{
                if (Vector3.Distance(realPositions[i], activator.position) < activatorRadius)
				{
                    Debug.Log(realPositions[i]);
                    startTimes[i] = Time.time + Random.Range(-timeToFinishRandomness, timeToFinishRandomness);
                } 
			} else
			{
                float timePassed = Time.time - startTimes[i];
                if(timePassed > timeToFinish)
                {
                    timePassed = timeToFinish;
                }

                child.localPosition = Vector3.Lerp(startPositions[i], originalPositions[i], timePassed / timeToFinish);
            }
            i++;
        }
    }
}
