using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using test;


public class NeuralNetwork : MonoBehaviour
{
    [SerializeField] float learnMultiplier;

    int pixelNum = 784;

    int firstLayerSize = 16;
    int secondLayerSize = 16;
    int outputLayerSize = 10;

    int miniBatchSize = 8;
    int miniBatchCounter;

    float accuracy;
    int guesses;
    int guessedCorrect;

    double[] firstLayerBias;
    double[] secondLayerBias;
    double[] outputLayerBias;

    double[][] input_FirstLayerWeights;
    double[][] firstLayer_SecondLayerWeights;
    double[][] secondLayer_OutputWeights;  

    double[] firstLayerZ;
    double[] secondLayerZ;
    double[] outputLayerZ;

    double[] firstLayerActivations;
    double[] secondLayerActivations;
    double[] outputLayerActivations;


    double[] firstLayerBiasCosts;
    double[] secondLayerBiasCosts;
    double[] outputLayerBiasCosts;

    double[][] input_FirstLayerWeightCosts;
    double[][] firstLayer_SecondLayerWeightCosts;
    double[][] secondLayer_OutputWeightCosts;


    double[] firstLayerBiasCostsMiniBatch;
    double[] secondLayerBiasCostsMiniBatch;
    double[] outputLayerBiasCostsMiniBatch;

    double[][] input_FirstLayerWeightCostsMiniBatch;
    double[][] firstLayer_SecondLayerWeightCostsMiniBatch;
    double[][] secondLayer_OutputWeightCostsMiniBatch;


    double[] firstLayerActivationCosts;
    double[] secondLayerActivationCosts;

    double[] expectedValues;



    string[] tempData;

    double tempValue;

    byte currentLabel;

    TextReader nNData;
    StreamWriter dataWrite;

    double gradientClippingMax = 50;
    double gradientClippingMin = -50;


    public void InitializeNNVariables()
    {
        accuracy = 0;

        firstLayerBias = new double[16];
        secondLayerBias = new double[16];
        outputLayerBias = new double[outputLayerSize];

        firstLayerActivations = new double[16];
        secondLayerActivations = new double[16];
        outputLayerActivations = new double[outputLayerSize];

        firstLayerZ = new double[16];
        secondLayerZ = new double[16];
        outputLayerZ = new double[outputLayerSize];

        firstLayerBiasCosts = new double[16];
        secondLayerBiasCosts = new double[16];
        outputLayerBiasCosts = new double[10];

        firstLayerBiasCostsMiniBatch = new double[16];
        secondLayerBiasCostsMiniBatch = new double[16];
        outputLayerBiasCostsMiniBatch = new double[10];

        firstLayerActivationCosts = new double[16];
        secondLayerActivationCosts = new double[16];
        

        expectedValues = new double[outputLayerSize];


        input_FirstLayerWeights = new double[pixelNum][];//weights of lines between input and firstlayer: 768x16
        for (int i = 0; i < pixelNum; i++)
        {
            input_FirstLayerWeights[i] = new double[firstLayerSize];
        }

        firstLayer_SecondLayerWeights = new double[firstLayerSize][];//weights of lines between first and second layer: 16x16
        for (int i = 0; i < firstLayerSize; i++)
        {
            firstLayer_SecondLayerWeights[i] = new double[secondLayerSize];
        }

        secondLayer_OutputWeights = new double[secondLayerSize][];//weights of lines between second and output layer: 16x10
        for (int i = 0; i < secondLayerSize; i++)
        {
            secondLayer_OutputWeights[i] = new double[outputLayerSize];
        }


        input_FirstLayerWeightCosts = new double[pixelNum][];//Costs of weights of lines between input and firstlayer: 768x16
        for (int i = 0; i < pixelNum; i++)
        {
            input_FirstLayerWeightCosts[i] = new double[firstLayerSize];
        }

        firstLayer_SecondLayerWeightCosts = new double[firstLayerSize][];//Costs of weights of lines between first and second layer: 16x16
        for (int i = 0; i < firstLayerSize; i++)
        {
            firstLayer_SecondLayerWeightCosts[i] = new double[secondLayerSize];
        }

        secondLayer_OutputWeightCosts = new double[secondLayerSize][];//Costs of weights of lines between second and output layer: 16x10
        for (int i = 0; i < secondLayerSize; i++)
        {
            secondLayer_OutputWeightCosts[i] = new double[outputLayerSize];
        }

        input_FirstLayerWeightCostsMiniBatch = new double[pixelNum][];//Costs of weights of lines between input and firstlayer: 768x16
        for (int i = 0; i < pixelNum; i++)
        {
            input_FirstLayerWeightCostsMiniBatch[i] = new double[firstLayerSize];
        }

        firstLayer_SecondLayerWeightCostsMiniBatch = new double[firstLayerSize][];//Costs of weights of lines between first and second layer: 16x16
        for (int i = 0; i < firstLayerSize; i++)
        {
            firstLayer_SecondLayerWeightCostsMiniBatch[i] = new double[secondLayerSize];
        }

        secondLayer_OutputWeightCostsMiniBatch = new double[secondLayerSize][];//Costs of weights of lines between second and output layer: 16x10
        for (int i = 0; i < secondLayerSize; i++)
        {
            secondLayer_OutputWeightCostsMiniBatch[i] = new double[outputLayerSize];
        }

    }

    public void LoadNumbersFromfile()
    {
        nNData = File.OpenText("NNData/data.txt");
        int dataCounter = 0;

        if (nNData == null)
        {
            Debug.Log("NNFileNotFound");
            return;
        }

        tempData = nNData.ReadToEnd().Split(' ');

        nNData.Close();

        for (int i = 0; i < firstLayerSize; i++)
        {
            firstLayerBias[i] = double.Parse(tempData[dataCounter]);
            dataCounter++;
        }

        for (int i = 0; i < secondLayerSize; i++)
        {
            secondLayerBias[i] = double.Parse(tempData[dataCounter]);
            dataCounter++;
        }

        for (int i = 0; i < outputLayerSize; i++)
        {
            outputLayerBias[i] = double.Parse(tempData[dataCounter]);
            dataCounter++;
        }

        for (int i = 0; i < pixelNum; i++)
        {
            for (int j = 0; j < firstLayerSize; j++)
            {
                input_FirstLayerWeights[i][j] = double.Parse(tempData[dataCounter], System.Globalization.CultureInfo.InvariantCulture);
                dataCounter++;
            }
        }

        for (int i = 0; i < firstLayerSize; i++)
        {
            for (int j = 0; j < secondLayerSize; j++)
            {
                firstLayer_SecondLayerWeights[i][j] = double.Parse(tempData[dataCounter], System.Globalization.CultureInfo.InvariantCulture);
                dataCounter++;
            }
        }

        for (int i = 0; i < secondLayerSize; i++)
        {
            for (int j = 0; j < outputLayerSize; j++)
            {
                secondLayer_OutputWeights[i][j] = double.Parse(tempData[dataCounter], System.Globalization.CultureInfo.InvariantCulture);
                dataCounter++;
            }
        }

    }

    public void StartCycle(DigitImage digitImage)
    {
        currentLabel = digitImage.label;

        ForwardPropagation(digitImage);

        BackwardPropagation(digitImage);
    }
    
    public void ScreenDataGuess(byte[] pixelData)
    {
        for (int i = 0; i < firstLayerSize; i++)
        {

            for (int j = 0; j < pixelNum; j++)
            {
                tempValue += (double)(pixelData[j] / 255f) * input_FirstLayerWeights[j][i];
            }
            tempValue += firstLayerBias[i];

            if (tempValue > gradientClippingMax)
            {
                tempValue = gradientClippingMax;
            }
            else if (tempValue < gradientClippingMin)
            {
                tempValue = gradientClippingMin;
            }
            firstLayerZ[i] = tempValue;
            firstLayerActivations[i] = Sigmoid(tempValue);
            tempValue = 0;
        }
        for (int i = 0; i < secondLayerSize; i++)
        {

            for (int j = 0; j < firstLayerSize; j++)
            {
                tempValue += firstLayerActivations[j] * firstLayer_SecondLayerWeights[j][i];
            }
            tempValue += secondLayerBias[i];
            secondLayerZ[i] = tempValue;
            secondLayerActivations[i] = Sigmoid(tempValue);
            tempValue = 0;
        }

        for (int i = 0; i < outputLayerSize; i++)
        {

            for (int j = 0; j < secondLayerSize; j++)
            {
                tempValue += secondLayerActivations[j] * secondLayer_OutputWeights[j][i];
            }
            tempValue += outputLayerBias[i];
            outputLayerZ[i] = tempValue;
            outputLayerActivations[i] = Sigmoid(tempValue);
            tempValue = 0;
        }
    }

    public void ForwardPropagation(DigitImage digitImage)
    {

        for (int i = 0; i < firstLayerSize; i++)
        {

            for (int j = 0; j < pixelNum; j++)
            {
                tempValue += (double)(digitImage.arrayPixels[j]/255f) * input_FirstLayerWeights[j][i];
            }
            tempValue += firstLayerBias[i];

            if(tempValue > gradientClippingMax)
            {
                tempValue = gradientClippingMax;
            }
            else if (tempValue < gradientClippingMin)
            {
                tempValue = gradientClippingMin;
            }
            firstLayerZ[i] = tempValue;
            firstLayerActivations[i] = Sigmoid(tempValue);
            tempValue = 0;
        }
        for (int i = 0; i < secondLayerSize; i++)
        {

            for (int j = 0; j < firstLayerSize; j++)
            {
                tempValue += firstLayerActivations[j] * firstLayer_SecondLayerWeights[j][i];
            }
            tempValue += secondLayerBias[i];
            secondLayerZ[i] = tempValue;
            secondLayerActivations[i] = Sigmoid(tempValue);
            tempValue = 0;
        }

        for (int i = 0; i < outputLayerSize; i++)
        {

            for (int j = 0; j < secondLayerSize; j++)
            {
                tempValue += secondLayerActivations[j] * secondLayer_OutputWeights[j][i];
            }
            tempValue += outputLayerBias[i];
            outputLayerZ[i] = tempValue;
            outputLayerActivations[i] = Sigmoid(tempValue);
            tempValue = 0;
        }

        SeeIfCorrect();
    }
    
    public double[] GetOutputData()
    {
        return outputLayerActivations;
    }

    public byte GetExpectedValue()
    {
        return currentLabel;
    }

    private void SeeIfCorrect()
    {
        if(GetRecognizedValue() != currentLabel)
        {
            guesses++;
        }
        else
        {
            guesses++;
            guessedCorrect++;
        }
    }

    public float GetAccuracy()
    {
        return guessedCorrect/(float)guesses;
    }

    public int GetRecognizedValue()
    {
        int recognized=0;

        for(int i= 0; i < outputLayerSize; i++)
        {
            if (outputLayerActivations[recognized] < outputLayerActivations[i])
            {
                recognized = i;
            }
        }

        return recognized;
    }

    public void BackwardPropagation(DigitImage digitImage)
    {
        for (int i = 0; i < outputLayerSize; i++)
        {
            if (i == digitImage.label)
            {
                expectedValues[i] = 1;
            }
            else
            {
                expectedValues[i] = 0;
            }
        }
       

        for ( int i = 0; i< outputLayerSize; i++)
        {
            outputLayerBiasCosts[i] = SigmoidDerivative(outputLayerZ[i]) * 2 * (outputLayerActivations[i] - expectedValues[i]);

            for(int j = 0; j< secondLayerSize; j++)
            {
                secondLayer_OutputWeightCosts[j][i] = outputLayerBiasCosts[i] * secondLayerActivations[j];
            }
        }
        for (int j = 0; j<secondLayerSize; j++)
        {
            for(int i = 0; i<outputLayerSize; i++)
            {
                secondLayerActivationCosts[j] += secondLayer_OutputWeights[j][i] * outputLayerBiasCosts[i];
            }
            
        }

        //L

        for (int i = 0; i < secondLayerSize; i++)
        {
            secondLayerBiasCosts[i] = SigmoidDerivative(secondLayerZ[i]) * secondLayerActivationCosts[i];
            for (int j = 0; j < firstLayerSize; j++)
            {
                firstLayer_SecondLayerWeightCosts[j][i] = secondLayerBiasCosts[i] * firstLayerActivations[j];
            }
        }

        for (int j = 0; j < firstLayerSize; j++)
        {
            for (int i = 0; i < secondLayerSize; i++)
            {
                firstLayerActivationCosts[j] += firstLayer_SecondLayerWeights[j][i] * secondLayerBiasCosts[i];
            }

        }

        //L-1

        for(int i = 0; i < firstLayerSize; i++)
        {
            firstLayerBiasCosts[i] = SigmoidDerivative(firstLayerZ[i]) * firstLayerActivationCosts[i];
            for (int j = 0; j < pixelNum; j++)
            {
                input_FirstLayerWeightCosts[j][i] = firstLayerBiasCosts[i] * digitImage.arrayPixels[j];
            }
        }

        //L-2

        AddUpToMiniBatch();
        miniBatchCounter++;
        if(miniBatchCounter >= miniBatchSize)
        {
            ApplyCostFunction();
            miniBatchCounter = 0;            
        }
        
        ResetData();//reset activation costs, as they are added up in iterations


    }

    private void AddUpToMiniBatch()
    {
        for (int i = 0; i < outputLayerSize; i++)
        {
            outputLayerBiasCostsMiniBatch[i] += outputLayerBiasCosts[i];

            for (int j = 0; j < secondLayerSize; j++)
            {
                secondLayer_OutputWeightCostsMiniBatch[j][i] += secondLayer_OutputWeightCosts[j][i];
            }
        }

        for (int i = 0; i < secondLayerSize; i++)
        {
            secondLayerBiasCostsMiniBatch[i] += secondLayerBiasCosts[i];

            for (int j = 0; j < firstLayerSize; j++)
            {
                firstLayer_SecondLayerWeightCostsMiniBatch[j][i] += firstLayer_SecondLayerWeightCosts[j][i];
            }
        }

        for (int i = 0; i < firstLayerSize; i++)
        {
            firstLayerBiasCostsMiniBatch[i] += firstLayerBiasCosts[i];

            for (int j = 0; j < pixelNum; j++)
            {
                input_FirstLayerWeightCostsMiniBatch[j][i] += input_FirstLayerWeightCosts[j][i];
            }
        }

    }

    private void ResetData()
    {
        for(int i = 0; i<firstLayerSize; i++)
        {
            firstLayerActivationCosts[i] = 0;
        }
        for (int i = 0; i < secondLayerSize; i++)
        {
            secondLayerActivationCosts[i] = 0;
        }


    }

    public void ApplyCostFunction()
    {
        for (int i = 0; i < outputLayerSize; i++)
        {
            outputLayerBias[i] -= outputLayerBiasCostsMiniBatch[i] * learnMultiplier / miniBatchSize;
            outputLayerBiasCostsMiniBatch[i] = 0;
            

            for (int j = 0; j < secondLayerSize; j++)
            {
                secondLayer_OutputWeights[j][i] -= secondLayer_OutputWeightCostsMiniBatch[j][i] * learnMultiplier / miniBatchSize;
                secondLayer_OutputWeightCostsMiniBatch[j][i] = 0;
            }
        }

        for (int i = 0; i < secondLayerSize; i++)
        {
            secondLayerBias[i] -= secondLayerBiasCostsMiniBatch[i] * learnMultiplier / miniBatchSize;
            secondLayerBiasCostsMiniBatch[i] = 0;
            

            for (int j = 0; j < firstLayerSize; j++)
            {
                firstLayer_SecondLayerWeights[j][i] -= firstLayer_SecondLayerWeightCostsMiniBatch[j][i] * learnMultiplier / miniBatchSize;
                firstLayer_SecondLayerWeightCostsMiniBatch[j][i] = 0;
            }
        }

        for (int i = 0; i < firstLayerSize; i++)
        {
            firstLayerBias[i] -= firstLayerBiasCostsMiniBatch[i] * learnMultiplier / miniBatchSize;
            firstLayerBiasCostsMiniBatch[i] = 0;

            for (int j = 0; j < pixelNum; j++)
            {
                input_FirstLayerWeights[j][i] -= input_FirstLayerWeightCostsMiniBatch[j][i] * learnMultiplier / miniBatchSize;
                input_FirstLayerWeightCostsMiniBatch[j][i] = 0;
            }
        }        
    }

    public static double Sigmoid(double value)
    {
        double k = System.Math.Exp(value);
        return k / (1.0f + k);
    }

    public static double SigmoidDerivative(double value)
    {
        return (Sigmoid(value) * (1 - Sigmoid(value)));
    }

    private void SaveData()
    {
        int dataCounter = 0;


        for (int i = 0; i < firstLayerSize; i++)
        {
            tempData[dataCounter] = firstLayerBias[i].ToString() + " ";
            dataCounter++;
        }

        for (int i = 0; i < secondLayerSize; i++)
        {
            tempData[dataCounter] = secondLayerBias[i].ToString() + " ";
            dataCounter++;
        }

        for (int i = 0; i < outputLayerSize; i++)
        {
            tempData[dataCounter] = outputLayerBias[i].ToString() + " ";
            dataCounter++;
        }

        for (int i = 0; i < pixelNum; i++)
        {
            for (int j = 0; j < firstLayerSize; j++)
            {
                tempData[dataCounter] = input_FirstLayerWeights[i][j].ToString() + " ";
                dataCounter++;
            }
        }

        for (int i = 0; i < firstLayerSize; i++)
        {
            for (int j = 0; j < secondLayerSize; j++)
            {
                tempData[dataCounter] = firstLayer_SecondLayerWeights[i][j].ToString() + " ";
                dataCounter++;
            }
        }

        for (int i = 0; i < secondLayerSize; i++)
        {
            for (int j = 0; j < outputLayerSize; j++)
            {
                tempData[dataCounter] = secondLayer_OutputWeights[i][j].ToString() + " ";
                dataCounter++;
            }
        }

        dataWrite = new StreamWriter("NNData/data.txt");

        if (dataWrite == null)
        {
            Debug.Log("NNFileNotFound");
            return;
        }

        foreach (string word in tempData)
        {
            dataWrite.Write(word);
        }

        dataWrite.Close();
    }

    private void OnApplicationQuit()
    {
        SaveData();

    }

}
