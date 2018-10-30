using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using test;



public class ScreenControls : MonoBehaviour
{
    [SerializeField] GameObject manager;
    UbyteParser parser;
    NeuralNetwork NN;

    MakeScreen screen;
    Transform[][] screenData;

    Color eraseColor;
    byte[] pixelData;
    int kCounter;

   public void CustomStart()
    {
        NN = manager.GetComponent<NeuralNetwork>();
        parser = manager.GetComponent<UbyteParser>();
        screen = gameObject.GetComponent<MakeScreen>();
        eraseColor = Color.white;
        screen.GiveCaller(this);

        pixelData = new byte[784];
    }

    public void GetData(ref Transform[][] pixels)
    {
        screenData = pixels;
    }
    public void ClearScreen()
    {
        screen.GiveCaller(this);
        for (int i = 0; i < 28; i++)
        {
            for (int j = 0; j < 28; j++)
            {                
                screenData[i][j].GetComponent<SpriteRenderer>().color = eraseColor;
            }
        }
    }
    public void PaintFromDatabase(DigitImage image)
    {       
        screen.GiveCaller(this);
        for (int i = 0; i < 28; i++)
        {
            for (int j = 0; j < 28; j++)
            {
                float temp = image.pixels[i][j];                
                temp = (255 - temp)/255f;
                screenData[i][j].GetComponent<SpriteRenderer>().color = new Color(temp, temp, temp);
            }
        }
    }

    public void GuessFromScreen()
    {
        if (parser.IsRunning())
        {
            return;
        }

        screen.GiveCaller(this);

        for (int i = 0; i < 28; i++)
        {
            for (int j = 0; j < 28; j++)
            {
                pixelData[kCounter] = (byte)(255 - (screenData[i][j].GetComponent<SpriteRenderer>().color.b * 255f));
                kCounter++;
            }
        }
        kCounter = 0;
        NN.ScreenDataGuess(pixelData);

    }

}
