using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CartesianAxis : MonoBehaviour // Game Object 
{
    // constants for axis
    public float axisLength = 10f;
    public float axisWidth = 0.05f;
    public float labelOffset = 0.5f;
    public Font labelFont;
    public int fontSize = 14;

    // declarations for the dynamic scaling of the axis w.r.t. Meta SDK camera rig
    public Transform xrRig; // Camera Rig
    public float minScale = 0.5f;
    public float maxScale = 2f;
    public float optimalDistance = 5f;

    private void Start()
    {   //Start method calls CreateAxis to generate all 3 vectors for 3D Cartesian System
        CreateAxis(Vector3.right, Color.red, "X"); 
        CreateAxis(Vector3.up, Color.green, "Y");
        CreateAxis(Vector3.forward, Color.blue, "Z");
    }

    private void CreateAxis(Vector3 direction, Color color, string label)
    {
        // Create cylinder for axis
        GameObject axis = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        axis.transform.SetParent(transform);
        axis.transform.localScale = new Vector3(axisWidth, axisLength / 2, axisWidth);
        axis.transform.position = direction * axisLength / 2;
        axis.transform.up = direction;

        // Set axis color
        Renderer axisRenderer = axis.GetComponent<Renderer>();
        axisRenderer.material = new Material(Shader.Find("Standard"));
        axisRenderer.material.color = color;

        // Create label
        GameObject labelObj = new GameObject($"{label}Label");
        labelObj.transform.SetParent(transform);
        labelObj.transform.position = direction * (axisLength + labelOffset);

        // Add TextMeshPro component for better text rendering in 3D space
        TextMeshPro textMesh = labelObj.AddComponent<TextMeshPro>();
        textMesh.text = label;
        textMesh.color = color;
        textMesh.fontSize = fontSize;
        textMesh.alignment = TextAlignmentOptions.Center;

        // Make text face the camera
        textMesh.transform.rotation = Quaternion.LookRotation(textMesh.transform.position - transform.position);
    }

    // Like Start, Update method always runs, here we continiously check for Camera Rig to 
    private void Update()
    {
        if (xrRig != null)
        {
            // Finds the distance between camera rig position (player) and the axis, finds the scaling factor given optimal distance and the two limits. 
            //      The min scale and max scale limits correspond to the camera rig's discrete movement, if it's too small of a movement then scale won't be updated.
            //              Finally, it transforms the axis w.r.t. the player.
            float distance = Vector3.Distance(xrRig.position, transform.position);
            float scale = Mathf.Clamp(distance / optimalDistance, minScale, maxScale);
            transform.localScale = Vector3.one * scale;
        }
    }
}
