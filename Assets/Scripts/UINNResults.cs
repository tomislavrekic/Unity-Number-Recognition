using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UINNResults : MonoBehaviour {
    
    [SerializeField] GameObject NNResultsUI, expectedValueUI, recognizedValueUI, accuracyValueUI;

    TextMeshProUGUI NNresultText;

    TextMeshProUGUI expectedValues;

    TextMeshProUGUI recognizedValue;

    TextMeshProUGUI accuracyValue;

    NeuralNetwork neuralNetwork;

    
    bool started;

    double[] data;

    byte label;


    public void CustomStart()
    {
        started = true;
        NNresultText = NNResultsUI.GetComponent<TextMeshProUGUI>();
        expectedValues = expectedValueUI.GetComponent<TextMeshProUGUI>();
        neuralNetwork = gameObject.GetComponent<NeuralNetwork>();
        recognizedValue = recognizedValueUI.GetComponent<TextMeshProUGUI>();
        accuracyValue = accuracyValueUI.GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (!started)
        {
            return;
        }

        data = neuralNetwork.GetOutputData();
        NNresultText.text = "";

        
        for(int i = 0; i<data.Length; i++)
        {
            NNresultText.text += data[i].ToString("F6") + "\n";
        }

        label = neuralNetwork.GetExpectedValue();

        expectedValues.text = label.ToString();

        recognizedValue.text = neuralNetwork.GetRecognizedValue().ToString();

        if(label != neuralNetwork.GetRecognizedValue())
        {
            recognizedValue.color = Color.red;
        }
        else
        {
            recognizedValue.color = Color.green;
        }

        accuracyValue.text = neuralNetwork.GetAccuracy().ToString("P2");
        

    }
}
