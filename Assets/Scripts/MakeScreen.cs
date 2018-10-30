using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MakeScreen : MonoBehaviour
{

    public Transform[][] pixels;

    Vector3 offset;

    float cubeScale;

    public void CustomStart()
    {
        pixels = new Transform[28][];
        for (int i = 0; i < 28; i++)
        {
            pixels[i] = new Transform[28];
        }

        cubeScale = gameObject.transform.GetChild(0).GetChild(0).localScale.x;

        for (int i = 0; i < 28; i++)
        {
            Transform temp;
            temp = gameObject.transform.GetChild(i);

            for (int j = 0; j < 28; j++)
            {
                pixels[i][j] = temp.GetChild(j);
            }
        }

        for (int i = 0; i < 28; i++)
        {
            for (int j = 0; j < 28; j++)
            {
                offset.x = j * cubeScale;
                offset.y = -i * cubeScale;
                pixels[i][j].position = pixels[i][j].position + offset;
            }
        }
    }

    public void GiveCaller(ScreenControls caller)
    {
        caller.GetData(ref pixels);
    }

    public Transform[][] GetPixels()
    {
        return pixels;
    }
}
