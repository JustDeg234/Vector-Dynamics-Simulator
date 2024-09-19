using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VectorUI : MonoBehaviour
{
    public DynamicVectors vector1; // Instantiations from DynamicVectors GameObject
    public DynamicVectors vector2;
    public DynamicVectors resultVector;
    public Slider vector1Slider;
    public Slider vector2Slider;
    public TMP_Dropdown operationDropdown;
    public TextMeshProUGUI resultText;

    private Vector3 v1Direction = Vector3.right;
    private Vector3 v2Direction = Vector3.forward;

    private void Start()
    {
        vector1Slider.onValueChanged.AddListener(UpdateVector1);
        vector2Slider.onValueChanged.AddListener(UpdateVector2);
        operationDropdown.onValueChanged.AddListener(PerformOperation);

        UpdateVector1(vector1Slider.value);
        UpdateVector2(vector2Slider.value);
    }

    private void UpdateVector1(float magnitude)
    {
        vector1.SetVector(v1Direction, magnitude);
    }

    private void UpdateVector2(float magnitude)
    {
        vector2.SetVector(v2Direction, magnitude);
    }

    private void PerformOperation(int operationIndex)
    {
        Vector3 v1 = v1Direction * vector1Slider.value;
        Vector3 v2 = v2Direction * vector2Slider.value;
        Vector3 result = Vector3.zero;
        string operationName = "";

        switch (operationIndex)
        {
            case 0: // Addition
                result = v1 + v2;
                operationName = "Addition";
                break;
            case 1: // Subtraction
                result = v1 - v2;
                operationName = "Subtraction";
                break;
            case 2: // Dot Product
                float dotProduct = Vector3.Dot(v1, v2);
                resultText.text = $"Dot Product: {dotProduct}";
                resultVector.gameObject.SetActive(false);
                return;
            case 3: // Cross Product
                result = Vector3.Cross(v1, v2);
                operationName = "Cross Product";
                break;
        }

        resultVector.gameObject.SetActive(true);
        resultVector.SetVector(result.normalized, result.magnitude);
        resultText.text = $"{operationName} Result: ({result.x}, {result.y}, {result.z})";
    }
}
