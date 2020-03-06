﻿using System.Collections.Generic;
using UnityEngine;

public class LaserGame : MonoBehaviour
{
    public GameObject startObject;
    public GameObject reflectObject;
    public GameObject splitObject;
    public GameObject endObject;
    public GameObject borderObject;

    public int pointerLength = 10;

    public Material laserMaterial;

    //List<GameObject> hitObjects;
    List<GameObject> lineContainers;

    void Start()
    {
        LineRenderer lineRenderer = startObject.AddComponent<LineRenderer>();
        lineRenderer.material = laserMaterial;
        //hitObjects = new List<GameObject>();
        lineContainers = new List<GameObject>();
    }

    GameObject RunLaser(GameObject inputObject, LineRenderer lineRenderer, Vector3 startDirection)
    {
        if (inputObject == null || lineRenderer == null)
        {
            return null;
        }

        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, inputObject.transform.position);

        RaycastHit hit;
        Vector3 direction = startDirection;

        while (Physics.Raycast(lineRenderer.GetPosition(lineRenderer.positionCount - 1), direction, out hit))
        {
            lineRenderer.positionCount++;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.point);

            GameObject hitObject = hit.collider.gameObject;

            if (hitObject == inputObject)
            {
                lineRenderer.positionCount = 1;
                lineRenderer.SetPosition(0, startObject.transform.position);
                return null;
            }

            if (hit.collider.transform.parent == borderObject.transform)
            {
                return borderObject;
            }

            if (hitObject != reflectObject)
            {
                return hitObject;
            }

            direction = Vector3.Reflect(direction, hit.normal);
        }

        lineRenderer.positionCount++;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, (lineRenderer.GetPosition(lineRenderer.positionCount - 2) + direction * pointerLength));

        return null;
    }

    void Update()
    {

        foreach (GameObject go in lineContainers)
        {
            Destroy(gameObject);
        }

        GameObject result = RunLaser(startObject, startObject.GetComponent<LineRenderer>(),Vector3.forward);

        if (result == splitObject)
        {
            GameObject leftContainer = new GameObject();
            leftContainer.transform.parent = splitObject.transform;
            LineRenderer leftRenderer = leftContainer.AddComponent<LineRenderer>();
            leftRenderer.material = laserMaterial;
            lineContainers.Add(leftContainer);
            GameObject leftResult = RunLaser(splitObject, leftRenderer, Vector3.left);

            GameObject rightContainer = new GameObject();
            rightContainer.transform.parent = splitObject.transform;
            LineRenderer rightRenderer = rightContainer.AddComponent<LineRenderer>();
            rightRenderer.material = laserMaterial;
            lineContainers.Add(rightContainer);
            GameObject rightResult = RunLaser(splitObject, rightRenderer, Vector3.right);

            GameObject centerContainer = new GameObject();
            centerContainer.transform.parent = splitObject.transform;
            LineRenderer centerRenderer = centerContainer.AddComponent<LineRenderer>();
            centerRenderer.material = laserMaterial;
            lineContainers.Add(centerContainer);
            GameObject centerResult = RunLaser(splitObject, centerRenderer, Vector3.forward);
        }

        //if (hit.collider.gameObject == endObject)
        //{
        //    endObject.GetComponent<Renderer>().material.color = Color.green;
        //    return;
        //}
        //else
        //{
        //    endObject.GetComponent<Renderer>().material.color = Color.red;
        //}

        //if (hit.collider.transform.parent == borderObject.transform)
        //{
        //    return;
        //}
    }
}