using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

namespace test
{

    public class DigitImage
    {
        public byte[][] pixels;
        public byte[] arrayPixels;
        public byte label;

        public DigitImage(byte[][] _pixels, byte _label, byte[] _arrayPixels)
        {
            this.pixels = new byte[28][];
            for (int i = 0; i < this.pixels.Length; ++i)
                this.pixels[i] = new byte[28];

            for (int i = 0; i < 28; ++i)
                for (int j = 0; j < 28; ++j)
                    this.pixels[i][j] = _pixels[i][j];


            this.label = _label;           


            this.arrayPixels = new byte[784];
            for (int z = 0; z < 784; z++)
            {
                this.arrayPixels[z] = _arrayPixels[z];
            }
        }

        public override string ToString()
        {
            string s = "";
            for (int i = 0; i < 28; ++i)
            {
                for (int j = 0; j < 28; ++j)
                {
                    if (this.pixels[i][j] == 0)
                        s += " "; // white
                    else if (this.pixels[i][j] == 255)
                        s += "O"; // black
                    else
                        s += "."; // gray
                }
                s += "\n";
            }
            s += this.label.ToString();
            return s;
        } // ToString

    }

    public class UbyteParser : MonoBehaviour
    {
        NeuralNetwork neuralNetwork;

        byte[][] pixels;
        byte[] arrayPixels;
        int k = 0;

        int fileDataCount = 60000;

        int numberOfIterations;

        ScreenControls screen;

        [SerializeField] int picturePerUpdate;

        FileStream ifsLabels;
        FileStream ifsImages;
        BinaryReader brLabels;
        BinaryReader brImages;

        int batchCounter;

        DigitImage[] allImages;

        int miniBatchSize = 8;
        int[] randomIndexes;


        bool ready = false;

        bool start = false;

        public bool IsRunning()
        {
            return start;
        }

        private void Update()
        {
            if (!ready)
            {
                return;
            }

            if (start)
            {
                FeedImagesToNN(picturePerUpdate);
            }

        }

        public void FeedNext()
        {
            if (!ready)
            {
                return;
            }
            FeedImagesToNN(1);
        }

        public void StartLearning()
        {
            start = true;
        }

        public void PauseLearning()
        {
            start = false;
        }

        public void CustomStart()
        {
            neuralNetwork = gameObject.GetComponent<NeuralNetwork>();
            screen = GameObject.Find("PixelScreen").GetComponent<ScreenControls>();

            pixels = new byte[28][];

            for (int i = 0; i < pixels.Length; ++i)
            {
                pixels[i] = new byte[28];
            }
            
            arrayPixels = new byte[784];

            randomIndexes = new int[miniBatchSize];
            GenerateRandomIndex();

            allImages = new DigitImage[fileDataCount];

            OpenFiles();
            ParseImages(fileDataCount);
            CloseFiles();

            ready = true;

        }

        private void OpenFiles()
        {
            ifsLabels = new FileStream(@"PictureData\train-labels.idx1-ubyte", FileMode.Open); // test labels
            ifsImages = new FileStream(@"PictureData\train-images.idx3-ubyte", FileMode.Open); // test images

            brLabels = new BinaryReader(ifsLabels);
            brImages = new BinaryReader(ifsImages);

            int magic1 = brImages.ReadInt32(); // discard
            int numImages = brImages.ReadInt32();
            int numRows = brImages.ReadInt32();
            int numCols = brImages.ReadInt32();

            int magic2 = brLabels.ReadInt32();
            int numLabels = brLabels.ReadInt32();
        }

        private void CloseFiles()
        {
            ifsImages.Close();
            brImages.Close();
            ifsLabels.Close();
            brLabels.Close();
        }

        private void GenerateRandomIndex()
        {
            for (int i= 0;i<miniBatchSize; i++)
            {
                randomIndexes[i] = (int)UnityEngine.Random.Range(0, fileDataCount);
            }
        }

        private void FeedImagesToNN(int pictureCount)
        {
            for (int i = 0; i < pictureCount; i++)
            {
                screen.PaintFromDatabase(allImages[randomIndexes[batchCounter]]);
                neuralNetwork.StartCycle(allImages[randomIndexes[batchCounter]]);

                batchCounter++;

                if(batchCounter >= miniBatchSize)
                {
                    batchCounter = 0;
                    GenerateRandomIndex();
                }
               
                numberOfIterations++;
            }

        }


        public void ParseImages(int pictureCount)
        {

            // each test image
            for (int di = 0; di < pictureCount; di++)
            {
                for (int i = 0; i < 28; i++)
                {
                    for (int j = 0; j < 28; j++)
                    {
                        byte b = brImages.ReadByte();
                        pixels[i][j] = b;
                        arrayPixels[k] = b;
                        k++;
                    }
                }
                k = 0;                

                byte lbl = brLabels.ReadByte();

                allImages[di] = new DigitImage(pixels, lbl, arrayPixels);


            } // each image        

        }

        private void OnApplicationQuit()
        {
            CloseFiles();
        }

    }


}

