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
    Vector3[] startPositions;
    Quaternion[] startRotation;
    Quaternion[] originalRotation;
    float[] completedAmounts;
    float[] startTimes;
    float timeToFinishRot;
    // Start is called before the first frame update
    void Start()
    {
        int pathCount = GFXParent.childCount;
        realPositions = new Vector3[pathCount];
        originalPositions = new Vector3[pathCount];
        startPositions = new Vector3[pathCount];
        completedAmounts = new float[pathCount];
        startTimes = new float[pathCount];
        startRotation = new Quaternion[pathCount];
        originalRotation = new Quaternion[pathCount];
        timeToFinishRot = timeToFinish / 6;
        int i = 0;

        foreach(Transform child in GFXParent)
		{
            realPositions[i] = child.position;
            originalPositions[i] = child.localPosition;
            originalRotation[i] = child.localRotation;
            startRotation[i] = Random.rotation;
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
                float moveAmount = timePassed / timeToFinish;
                moveAmount = Mathf.Clamp(moveAmount, 0, 1);
                child.localPosition = Vector3.Lerp(startPositions[i], originalPositions[i], moveAmount);

                float startRot = startTimes[i] + timeToFinish - timeToFinishRot;
                float timePassedRot = Time.time - startRot;
                float rotAmount = timePassedRot / timeToFinishRot;
                rotAmount = Mathf.Clamp(rotAmount, 0, 1);
                child.localRotation = Quaternion.Lerp(startRotation[i], originalRotation[i], rotAmount);
            }
            i++;
        }
    }
}
