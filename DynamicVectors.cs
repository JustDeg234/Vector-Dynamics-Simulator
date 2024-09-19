using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicVectors : MonoBehaviour
{
    public float maxLength = 10f;
    public float arrowHeadLength = 0.25f;
    public float arrowWidth = 0.1f;

    private Vector3 direction;
    private float magnitude;
    private GameObject arrowObject;
    private LineRenderer lineRenderer;
    private Transform arrowHead;

    public void SetVector(Vector3 newDirection, float newMagnitude)
    {
        direction = newDirection.normalized;
        magnitude = Mathf.Clamp(newMagnitude, 0, maxLength);
        UpdateVisualization();
    }

    private void Start()
    {
        CreateArrowObjects();
    }

    private void CreateArrowObjects()
    {
        arrowObject = new GameObject("Arrow");
        arrowObject.transform.SetParent(transform);

        lineRenderer = arrowObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = arrowWidth;
        lineRenderer.endWidth = arrowWidth;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));

        // Create arrow head using a cylinder
        arrowHead = GameObject.CreatePrimitive(PrimitiveType.Cylinder).transform;
        arrowHead.SetParent(arrowObject.transform);
        // Scale the cylinder to make it look like an arrow head
        arrowHead.localScale = new Vector3(arrowWidth * 2, arrowHeadLength, arrowWidth * 2);
        // Rotate it to point along the arrow direction
        arrowHead.Rotate(90, 0, 0);
    }

    private void UpdateVisualization()
    {
        Vector3 endPoint = direction * magnitude;

        lineRenderer.SetPosition(0, Vector3.zero);
        lineRenderer.SetPosition(1, endPoint);

        arrowHead.position = endPoint;
        arrowHead.rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(90, 0, 0);
        arrowHead.Translate(0, arrowHeadLength / 2, 0, Space.Self);
    }
}
