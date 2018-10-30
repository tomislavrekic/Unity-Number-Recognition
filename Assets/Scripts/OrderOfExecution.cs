using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using test;

public class OrderOfExecution : MonoBehaviour {

    [SerializeField] GameObject PixelScreen;

    MakeScreen makeScreen;
    ScreenControls screenControls;
    UbyteParser ubyteParser;
    NeuralNetwork neuralNetwork;
    UINNResults UINNResults;


    private void Awake()
    {
        makeScreen = PixelScreen.GetComponent<MakeScreen>();
        screenControls = PixelScreen.GetComponent<ScreenControls>();
        ubyteParser = gameObject.GetComponent<UbyteParser>();
        neuralNetwork = gameObject.GetComponent<NeuralNetwork>();
        UINNResults = gameObject.GetComponent<UINNResults>();

        StartCoroutine(StartUp());
    }

    IEnumerator StartUp()
    {
        neuralNetwork.InitializeNNVariables();
        yield return null;
        neuralNetwork.LoadNumbersFromfile();
        yield return null;
        makeScreen.CustomStart();
        yield return null;
        screenControls.CustomStart();
        yield return null;
        ubyteParser.CustomStart();
        yield return null;
        UINNResults.CustomStart();
        
        

    }
}
