using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;


public class WorldGeneration : Singleton<WorldGeneration>
{
    public GameObject pixPrefab;
    //public GameObject WaterPrefab;
    public GameObject showingcolour;
    public Texture2D inputMap;
    public Texture2D ColorInputMap;
    private Texture2D tex;
    private Texture2D tex2;
    private Color[,] imageValues;
    private Color[,] ColorimageValues;
    private int[] counter = new int[2];
    GameObject[,] image;
    GameObject[,] image2;
    List<Coords> labels;

    public int scale = 17;
    private List<Coords> tmp_colorCoords = new List<Coords>();
    public GameObject mountainModel;
    List<GameObject> gameObjects = new List<GameObject>();
    int[] tmp_coords;
    int counter2 = 0;
    private bool[,] spawn2;
    bool secondmountain = false;

    private bool first = true;
    private int[,] spawn;
    int theWidth;
    int theHeight;
    int numberofCombiMeshes = 1;
    public List<GameObject> parentsIsland;
    public List<GameObject> parentsDirt;
    public List<GameObject> parentsWater;
    public List<GameObject> parentsDesert;
    public GameObject[] Grass;
    public GameObject[] trees;
    public GameObject[] Cactus;
    public GameObject[] WaterPlants;
    Texture mat;
    int ErosionAmount = 3;
    int WBamount = 20;
    float ContrastAmount = 1.2f;
    float TresholdAmount = 0.6f;
    

    int ColorSpreadGreen = 70;
    int ColorSpreadBlue = 100;
    int ColorSpreadYellow = 100;
    int ColorSpreadbrown = 55;
    int ColorSpreadRed = 40;

    float greenR = 100;
    float greenG = 180;
    float greenB = 100;

    float blueR = 90;
    float blueG = 130;
    float blueB = 210;

    float yellowR = 200;
    float yellowG = 180;
    float yellowB = 70;

    float brownR = 160;
    float brownG = 30;
    float brownB = 40;

    Color LookingForGreen;
    Color LookingForBlue;
    Color LookingForYellow;
    Color LookingForBrown;
    Color LookingForRed = new Color(200f / 255, 50f / 255, 50f / 255);
    public int spawnX, spawnZ;

    void Start()
    {

        parentsIsland = new List<GameObject>();
        parentsDirt = new List<GameObject>();
        parentsWater = new List<GameObject>();
        parentsDesert = new List<GameObject>();
        labels = new List<Coords>();
       // mat = Resources.Load("water") as Texture;
        resetImage();
        resetColorImage();
        FindSpawnPoint();

    } //start

    public void InitailizeGrassfire()
    {
        for (int w = 0; w < theWidth; w++)
        {
            for (int h = 0; h < theHeight; h++)
            {
                if (imageValues[w, h].grayscale * 255 == 255 && first)
                {
                    SequentialGrassFire4(w, h);
                    SequentialGrassFire3(w, h);
                    SequentialGrassFire2(w, h);
                    SequentialGrassFire(w, h); //Runs the grassfire function on the first white cube. 
                    first = false; //So it only runs the grassfire function once. 
                }
            }
        }
        for (int w = 0; w < theWidth; w++)
        {
            for (int h = 0; h < theHeight; h++)
            {
                if (!DetectColor(ColorimageValues[w, h], LookingForBrown, ColorSpreadbrown) && !spawn2[w, h])
                {
                    tmp_colorCoords = SequentialGrassFire5(w, h);
                    //Debug.Log("Red box: " + tmp_colorCoords.Count);

                    //Debug.Log("X = " + (tmp_coords[0] - tmp_coords[1]) + " Y = " + (tmp_coords[2] - tmp_coords[3]));
                   // if (secondmountain)
                    //{
                        tmp_coords = FindHighestValue(tmp_colorCoords);
                        PlaceMountain((tmp_coords[0] - tmp_coords[1]), (tmp_coords[2] - tmp_coords[3]), FindMiddle(tmp_colorCoords)[0], FindMiddle(tmp_colorCoords)[1], counter2);
                    //}
                    counter2++;
                    secondmountain = true;
                    }
                
            }
        }
        Destroy(gameObjects[0]);


        for (int w = 0; w < theWidth; w++)
        {
            for (int h = 0; h < theHeight; h++)
            {
                if (spawn[w, h] == 3) //Here it the spawn arrays is true at that position.
                {
                    image[w, h] = (GameObject)Instantiate(pixPrefab, new Vector3(10.5f - w, -14.5f, 10.5f - h), Quaternion.identity);
                    int tmp_int = (int)UnityEngine.Random.Range(0, 30);
                    if (tmp_int == 5)
                    {
                        int whatGrass = (int)UnityEngine.Random.Range(0, 2);
                        GameObject tmp_object = Grass[whatGrass];
                        tmp_object.transform.position = new Vector3(10.5f - w, 0.5f, 10.5f - h);
                        tmp_object.transform.Rotate(0, 20, 0);
                        Instantiate(tmp_object);

                    }
                }

                if(spawn[w,h] == 2)
                {
                    image[w, h] = (GameObject)Instantiate(pixPrefab, new Vector3(10.5f - w, -14.5f, 10.5f - h), Quaternion.identity);
                    int tmp_int = (int)UnityEngine.Random.Range(0, 10);
                    if (tmp_int == 5)
                    {
                        int whatTree = (int)UnityEngine.Random.Range(0, 17);
                        GameObject tmp_object = trees[whatTree];
                        tmp_object.transform.position = new Vector3(10.5f - w, 0.5f, 10.5f - h);
                        Instantiate(tmp_object);

                    }
                }

                if (spawn[w, h] == 1)
                {
                    image[w, h] = (GameObject)Instantiate(pixPrefab, new Vector3(10.5f - w, -15, 10.5f - h), Quaternion.identity);
                    int tmp_int = (int)UnityEngine.Random.Range(1, 150);
                    if (tmp_int == 5)
                    {
                        int whatPlants = (int)UnityEngine.Random.Range(0, 7);
                        GameObject tmp_object = WaterPlants[whatPlants];
                        tmp_object.transform.position = new Vector3(10.5f - w, 0.0f, 10.5f - h);
                        Instantiate(tmp_object);

                    }
                }

                if(spawn[w,h] == 0)
                {
                    image[w, h] = (GameObject)Instantiate(pixPrefab, new Vector3(10.5f - w, -14.5f, 10.5f - h), Quaternion.identity);
                    int tmp_int = (int)UnityEngine.Random.Range(1, 500);
                    if (tmp_int == 5)
                    {
                        int whatCactus = (int)UnityEngine.Random.Range(0, 14);
                        GameObject tmp_object = Cactus[whatCactus];
                        tmp_object.transform.position = new Vector3(10.5f - w, 0, 10.5f - h);
                        Instantiate(tmp_object);
                    }
                }
            }
            }
    }
    
    public void SequentialGrassFire(int x, int y)
    {
        int h;
        int w;
        labels.Add(new Coords(x, y)); //Adds the coords for the first position.
        while (labels.Count > 0) //as long as I have new position to test, this loop will run. 
        {
            h = labels[0].getH(); //Gets the x and y position for the pixel we are doing right now. 
            w = labels[0].getW(); //^
            if (spawn[w, h] == 3)
            {
                //imageValues[w, h] = new Color(0.5f, 0.5f, 0.5f); //Change the color so I can't add it again. 
                spawn[w, h] = 4;
                //This I will use later, when I have to spawn cubes for each pixel that was hit by the grassfire function. True meaning that it was hit, and therefore shouldn't get spawned later on.
                if (h - 1 >= 0 && imageValues[w, h - 1].grayscale * 255 == 255) //if we have a pixel above us. 
                    labels.Add(new Coords(w, h - 1));
                if (h + 1 < theHeight && imageValues[w, h + 1].grayscale * 255 == 255) //if we have a pixel below us. 
                    labels.Add(new Coords(w, h + 1));
                if (w - 1 >= 0 && imageValues[w - 1, h].grayscale * 255 == 255) //if we have a pixel to the right. 
                    labels.Add(new Coords(w - 1, h));
                if (w + 1 < theWidth && imageValues[w + 1, h].grayscale * 255 == 255) //if we have a pixel to the left. 
                    labels.Add(new Coords(w + 1, h));
                labels.RemoveAt(0);
            }
            else
            {
                labels.RemoveAt(0);
            }
        }
    }

    public void SequentialGrassFire2(int x, int y)
    {
        int h;
        int w;
        labels.Add(new Coords(x, y)); //Adds the coords for the first position.
        while (labels.Count > 0) //as long as I have new position to test, this loop will run. 
        {
            h = labels[0].getH(); //Ge+ts the x and y position for the pixel we are doing right now. 
            w = labels[0].getW(); //^
            if (spawn[w, h] == 2)
            {
                spawn[w, h] = 3;
                
                if (h - 1 >= 0 && !DetectColor(ColorimageValues[w, h - 1], LookingForGreen, ColorSpreadGreen)) //if we have a pixel above us. 
                    labels.Add(new Coords(w, h - 1));
                if (h + 1 < theHeight && !DetectColor(ColorimageValues[w, h + 1], LookingForGreen, ColorSpreadGreen)) //if we have a pixel below us. 
                    labels.Add(new Coords(w, h + 1));
                if (w - 1 >= 0 && !DetectColor(ColorimageValues[w - 1, h], LookingForGreen, ColorSpreadGreen)) //if we have a pixel to the right. 
                    labels.Add(new Coords(w - 1, h));
                if (w + 1 < theWidth && !DetectColor(ColorimageValues[w + 1, h], LookingForGreen, ColorSpreadGreen)) //if we have a pixel to the left. 
                    labels.Add(new Coords(w + 1, h));
                labels.RemoveAt(0);
            }
            else
            {
                labels.RemoveAt(0);
                
            }
        }
    }

    public void SequentialGrassFire3(int x, int y)
    {
        int h;
        int w;
        labels.Add(new Coords(x, y)); //Adds the coords for the first position.
        while (labels.Count > 0) //as long as I have new position to test, this loop will run. 
        {
            h = labels[0].getH(); //Gets the x and y position for the pixel we are doing right now. 
            w = labels[0].getW(); //^
            if (spawn[w, h] == 1)
            {
                spawn[w, h] = 2;

                if (h - 1 >= 0 && !DetectColor(ColorimageValues[w, h - 1], LookingForBlue, ColorSpreadBlue)) //if we have a pixel above us. 
                    labels.Add(new Coords(w, h - 1));
                if (h + 1 < theHeight && !DetectColor(ColorimageValues[w, h + 1], LookingForBlue, ColorSpreadBlue)) //if we have a pixel below us. 
                    labels.Add(new Coords(w, h + 1));
                if (w - 1 >= 0 && !DetectColor(ColorimageValues[w - 1, h], LookingForBlue, ColorSpreadBlue)) //if we have a pixel to the right. 
                    labels.Add(new Coords(w - 1, h));
                if (w + 1 < theWidth && !DetectColor(ColorimageValues[w + 1, h], LookingForBlue, ColorSpreadBlue)) //if we have a pixel to the left. 
                    labels.Add(new Coords(w + 1, h));
                labels.RemoveAt(0);
            }
            else
            {
                labels.RemoveAt(0);
            }
        }
    }

    public void SequentialGrassFire4(int x, int y)
    {
        int h;
        int w;
        labels.Add(new Coords(x, y)); //Adds the coords for the first position.
        while (labels.Count > 0) //as long as I have new position to test, this loop will run. 
        {
            h = labels[0].getH(); //Gets the x and y position for the pixel we are doing right now. 
            w = labels[0].getW(); //^
            if (spawn[w, h] == 0)
            {
                spawn[w, h] = 1;

                if (h - 1 >= 0 && !DetectColor(ColorimageValues[w, h - 1], LookingForYellow, ColorSpreadYellow)) //if we have a pixel above us. 
                    labels.Add(new Coords(w, h - 1));
                if (h + 1 < theHeight && !DetectColor(ColorimageValues[w, h + 1], LookingForYellow, ColorSpreadYellow)) //if we have a pixel below us. 
                    labels.Add(new Coords(w, h + 1));
                if (w - 1 >= 0 && !DetectColor(ColorimageValues[w - 1, h], LookingForYellow, ColorSpreadYellow)) //if we have a pixel to the right. 
                    labels.Add(new Coords(w - 1, h));
                if (w + 1 < theWidth && !DetectColor(ColorimageValues[w + 1, h], LookingForYellow, ColorSpreadYellow)) //if we have a pixel to the left. 
                    labels.Add(new Coords(w + 1, h));
                labels.RemoveAt(0);
            }
            else
            {
                labels.RemoveAt(0);

            }
        }
    }

    public List<Coords> SequentialGrassFire5(int x, int y)
    {
        List<Coords> tmp_list = new List<Coords>();
        int h;
        int w;
        labels.Add(new Coords(x, y));
        tmp_list.Add(new Coords(x, y));
        while (labels.Count > 0)
        {
            h = labels[0].getH();
            w = labels[0].getW();
            if (!spawn2[w, h])
            {
                spawn2[w, h] = true;
                if (h - 1 >= 0 && !DetectColor(ColorimageValues[w, h - 1], LookingForBrown, ColorSpreadbrown))
                {
                    labels.Add(new Coords(w, h - 1));
                    tmp_list.Add(new Coords(w, h - 1));
                }
                if (h + 1 < theHeight && !DetectColor(ColorimageValues[w, h + 1], LookingForBrown, ColorSpreadbrown))
                {
                    labels.Add(new Coords(w, h + 1));
                    tmp_list.Add(new Coords(w, h + 1));
                }
                if (w - 1 >= 0 && !DetectColor(ColorimageValues[w - 1, h], LookingForBrown, ColorSpreadbrown))
                {
                    labels.Add(new Coords(w - 1, h));
                    tmp_list.Add(new Coords(w - 1, h));
                }
                if (w + 1 < theWidth && !DetectColor(ColorimageValues[w + 1, h], LookingForBrown, ColorSpreadbrown))
                {
                    labels.Add(new Coords(w + 1, h));
                    tmp_list.Add(new Coords(w + 1, h));
                }
                labels.RemoveAt(0);
            }
            else
            {
                labels.RemoveAt(0);
            }
        }
        return tmp_list;
    }

    public void CallCombineMesh(bool callmemaybe)
    {
        if (callmemaybe)
        {
            int countisland = 0, counttree = 0, countWater = 0, countDesert = 0;
            int parentsUsedisland = 0, parentsUsedDirt = 0, parentsUsedWater = 0, parentsUsedDesert = 0;
            parentsIsland.Add(new GameObject("Grassland"));
            parentsDirt.Add(new GameObject("Dirt"));
            parentsWater.Add(new GameObject("Water"));
            parentsDesert.Add(new GameObject("Desert"));
            for (int w = 0; w < theWidth; w++)
            {
                for (int h = 0; h < theHeight; h++)
                {
                    if (spawn[w, h] == 3)
                    {
                        image[w, h].transform.parent = parentsIsland.ElementAt(parentsUsedisland).transform;
                        
                        countisland++;
                        if ((countisland) / (parentsUsedisland + 1) >= 2000)
                        {
                            parentsIsland.ElementAt(parentsUsedisland).AddComponent<Grass>();
                            parentsUsedisland++;
                            parentsIsland.Add(new GameObject("Grassland"));
                        }
                    }
                    if (spawn[w, h] == 2)
                    {
                        image[w, h].transform.parent = parentsDirt.ElementAt(parentsUsedDirt).transform;
                        counttree++;
                        if ((counttree) / (parentsUsedDirt + 1) >= 2000)
                        {
                            parentsDirt.ElementAt(parentsUsedDirt).AddComponent<Dirt>();
                            parentsUsedDirt++;
                            parentsDirt.Add(new GameObject("Dirt"));
                        }
                    }
                    if (spawn[w, h] == 1)
                    {
                        image[w, h].transform.parent = parentsWater.ElementAt(parentsUsedWater).transform;
                        countWater++;
                        if ((countWater) / (parentsUsedWater + 1) >= 2000)
                        {
                            parentsWater.ElementAt(parentsUsedWater).AddComponent<Water>();
                            parentsWater.ElementAt(parentsUsedWater).AddComponent<WaterBasic>();
                            parentsUsedWater++;
                            parentsWater.Add(new GameObject("Water"));
                        }
                    }
                    if (spawn[w, h] == 0)
                    {

                        image[w, h].transform.parent = parentsDesert.ElementAt(parentsUsedDesert).transform;
                        countDesert++;
                        if ((countDesert) / (parentsUsedDesert + 1) >= 2000)
                        {
                            parentsDesert.ElementAt(parentsUsedDesert).AddComponent<Desert>();
                            parentsUsedDesert++;
                            parentsDesert.Add(new GameObject("Desert"));
                        }
                    }
                }
            }
            parentsIsland.ElementAt(parentsUsedisland).AddComponent<Grass>();
            parentsDirt.ElementAt(parentsUsedDirt).AddComponent<Dirt>();
            parentsWater.ElementAt(parentsUsedWater).AddComponent<Water>();
            parentsWater.ElementAt(parentsUsedWater).AddComponent<WaterBasic>();
            parentsDesert.ElementAt(parentsUsedDesert).AddComponent<Desert>();
            deleteMeshes();
        }
    }

    public void Callwhiteborder()
    {
        imageValues = whiteborder(imageValues);
        SetPixels2D(imageValues, tex);
        this.GetComponent<Renderer>().material.mainTexture = tex;
    }

    public void CallErosion()
    {
        resetImage();

        imageValues = Contrast(imageValues, ContrastAmount);
        Treshold(imageValues, TresholdAmount);
        imageValues = Erosion(imageValues, ErosionAmount);
        SetPixels2D(imageValues, tex);
        this.GetComponent<Renderer>().material.mainTexture = tex;
    }

    public void CallRGB2Grayscale()
    {
        imageValues = Rgb2Grayscale(imageValues);
        SetPixels2D(imageValues, tex);
        this.GetComponent<Renderer>().material.mainTexture = tex;
    }

    public void CallContrast()
    {
        resetImage();
        imageValues = Contrast(imageValues, ContrastAmount);
        SetPixels2D(imageValues, tex);
        this.GetComponent<Renderer>().material.mainTexture = tex;
    }

    public void CallGrassFire()
    {
        resetImage();
        resetColorImage();

        Treshold(imageValues, TresholdAmount);
        imageValues = Contrast(imageValues, ContrastAmount);
        imageValues = Erosion(imageValues, ErosionAmount);
        imageValues = whiteborder(imageValues);
        ColorimageValues = NormalizedRgb(ColorimageValues);
        //ColorimageValues = LookForColors(ColorimageValues);
        InitailizeGrassfire();
        SetPixels2D(imageValues, tex);
        this.GetComponent<Renderer>().material.mainTexture = tex;
    }

    public void CallLookForColors()
    {
        resetColorImage();
        ColorimageValues = NormalizedRgb(ColorimageValues); 
        ColorimageValues = LookForColors(ColorimageValues);
        SetPixels2D(ColorimageValues, tex2);
        showingcolour.GetComponent<Renderer>().material.mainTexture = tex2;
    }

    public Color[,] LookForColors(Color[,] i)
    {
        for (int w = 0; w < theWidth; w++)
        {
            for (int h = 0; h < theHeight; h++)
            {
                if (DetectColor(ColorimageValues[w, h], LookingForBlue, ColorSpreadBlue))
                {
                    i[w, h].r = 0;
                    i[w, h].g = 0;
                    i[w, h].b = 255;
                }
                else if (DetectColor(ColorimageValues[w, h], LookingForGreen, ColorSpreadGreen))
                {
                    i[w, h].r = 0;
                    i[w, h].g = 255;
                    i[w, h].b = 0;
                }
                else if (DetectColor(ColorimageValues[w, h], LookingForYellow, ColorSpreadYellow))
                {
                    i[w, h].r = 255;
                    i[w, h].g = 255;
                    i[w, h].b = 0;
                }
                else if (DetectColor(ColorimageValues[w, h], LookingForRed, ColorSpreadRed))
                {
                    i[w, h].r = 255;
                    i[w, h].g = 0;
                    i[w, h].b = 0;
                }
                else if (DetectColor(ColorimageValues[w, h], LookingForBrown, ColorSpreadbrown))
                {
                    i[w, h].r = 0;
                    i[w, h].g = 0;
                    i[w, h].b = 0;
                }
                else
                {
                        i[w, h].r = 255;
                        i[w, h].g = 255;
                        i[w, h].b = 255;
                }
            }
        }
        return i;

    }

    public Color[,] Treshold(Color[,] i, float t)
    {
        for (int w = 0; w < i.GetLength(0); w++)
        {
            for (int h = 0; h < i.GetLength(1); h++)
            {
                float bw = (i[w, h].r + i[w, h].g + i[w, h].b) / 3;
                bw = bw < t ? 0f : 1f;
                i[w, h].r = bw;
                i[w, h].g = bw;
                i[w, h].b = bw;
            }
        }
        return i;
    }

    public Color[,] Rgb2Grayscale(Color[,] i)
    {
        for (int w = 0; w < i.GetLength(0); w++)
        {
            for (int h = 0; h < i.GetLength(1); h++)
            {
                float grey = (i[w, h].r + i[w, h].g + i[w, h].b) / 3;
                i[w, h].r = grey;
                i[w, h].g = grey;
                i[w, h].b = grey;
            }
        }
        return i;
    }

    public Color[,] Contrast(Color[,] i, float a)
    {
        for (int w = 0; w < i.GetLength(0); w++)
        {
            for (int h = 0; h < i.GetLength(1); h++)
            {
                if ((i[w, h].r + i[w, h].g + i[w, h].b) / 3 < 0.4)
                {
                    i[w, h].r /= a;
                    i[w, h].g /= a;
                    i[w, h].b /= a;
                }
                if (((i[w, h].r + i[w, h].g + i[w, h].b) / 3) > 0.4)
                {
                    i[w, h].r *= a;
                    i[w, h].g *= a;
                    i[w, h].b *= a;
                }
            }
        }
        return i;
    }

    public Color[,] whiteborder(Color[,] i)
    {
        for (int w = 0; w < i.GetLength(0); w++)
        {
            for (int h = 0; h < i.GetLength(1); h++)
            {
                if (w < WBamount || h < WBamount || w > i.GetLength(0) - WBamount || h > i.GetLength(1) - WBamount)
                {
                    i[w, h].r = 1;
                    i[w, h].g = 1;
                    i[w, h].b = 1;
                }
            }
        }
        return i;
    }

    public Color[,] Erosion(Color[,] i, int k)
    {
        Color[,] result = new Color[i.GetLength(0), i.GetLength(1)];

        for (int w = 0 + k; w < i.GetLength(0) - k; w++)
        {
            for (int h = 0 + k; h < i.GetLength(1) - k; h++)
            {
                float sum = 0f;
                for (int j = -k / 2; j <= +k / 2; j++)
                {
                    for (int l = -k / 2; l <= +k / 2; l++)
                    {
                        sum += i[w + j, h + l].r;
                    }
                }

                if (w + k < 10 || h + k < 10 || w > (i.GetLength(0) - (k + 10)) || h > (i.GetLength(1) - (k + 10)))
                {
                    result[w, h].r = 1;
                    result[w, h].g = 1;
                    result[w, h].b = 1;
                }
                else
                {
                    float res = sum < k * k ? 0f : 1f;
                    result[w, h].r = res;
                    result[w, h].g = res;
                    result[w, h].b = res;
                }
            }
        }
        //Callwhiteborder();
        return result;
    }

    public Color[,] Dilation(Color[,] i, int k)
    {
        //set source image to grayscale
        //i = Treshold(i, 0.5f);

        Color[,] result = new Color[i.GetLength(0), i.GetLength(1)];

        for (int w = 0 + k; w < i.GetLength(0) - k; w++)
        {
            for (int h = 0 + k; h < i.GetLength(1) - k; h++)
            {
                float sum = 0f;
                for (int j = -k / 2; j <= +k / 2; j++)
                {
                    for (int l = -k / 2; l <= +k / 2; l++)
                    {
                        sum += i[w + j, h + l].r;
                    }
                }

                float res = sum > 0 ? 1f : 0f;
                result[w, h].r = res;
                result[w, h].g = res;
                result[w, h].b = res;
            }
        }
        return result;
    }

    public Color[,] GetPixels2D(Texture2D t)
    {
        Color[,] texture2d = new Color[t.width, t.height];
        Color[] texture1d = t.GetPixels();

        for (int h = 0; h < t.height; h++)
        {
            for (int w = 0; w < t.width; w++)
            {
                texture2d[w, h] = texture1d[h * t.width + w];
            }
        }
        return texture2d;
    }

    public Color[,] NormalizedRgb(Color[,] i)
    {
        for (int w = 0; w < i.GetLength(0); w++)
        {
            for (int h = 0; h < i.GetLength(1); h++)
            {
                Color pix = i[w, h];
                float sum = pix.r + pix.g + pix.b;
                if (sum == 0f)
                {
                    i[w, h] = Color.black;
                }
                else
                {
                    i[w, h] = new Color(i[w, h].r / sum, i[w, h].g / sum, i[w, h].b / sum);
                }
            }
        }
        return i;
    }

    public void SetPixels2D(Color[,] i, Texture2D t)
    {
        Color[] texture1d = new Color[i.Length];

        int width = i.GetLength(0);
        int height = i.GetLength(1);

        for (int h = 0; h < height; h++)
        {
            for (int w = 0; w < width; w++)
            {
                texture1d[h * width + w] = i[w, h];
            }
        }
        t.SetPixels(texture1d);
        t.Apply();
    }

    public bool DetectColor(Color i, Color targetColor, int spread)
    {
        bool tmp_bool;
        Color pix = i;
        if (Mathf.Abs(pix.r - targetColor.r) * 255f < spread && Mathf.Abs(pix.g - targetColor.g) * 255f < spread &&
            Mathf.Abs(pix.b - targetColor.b) * 255f < spread)
        {
            tmp_bool = true;
        }
        else
        {
            tmp_bool = false;


        }
        return tmp_bool;
    }

    public void FindSpawnPoint()
    {
        int found = 0;
        for (int w = 0; w < theWidth; w++)
        {
            for (int h = 0; h < theHeight; h++)
            {
                if (DetectColor(ColorimageValues[w, h], LookingForRed, ColorSpreadRed))
                {
                    found++;
                    spawnX = spawnX + w;
                    spawnZ = spawnZ + h;
                    //Debug.Log("spawnpoint = " + spawnX + " , " + spawnZ);
                }
            }
        }
        spawnX = spawnX / found;
        spawnZ = spawnZ / found;


    }

    private void deleteMeshes()
    {
        for (int y = 0; y < image.GetLength(0); y++)
        {
            for (int x = 0; x < image.GetLength(1); x++)
            {
                Destroy(image[y, x]);
            }
        }
    }

    public void resetImage()
    {
        //erosion picture
        inputMap = Resources.Load("map") as Texture2D;
        theWidth = inputMap.width;
        theHeight = inputMap.height;
        tex = new Texture2D(theWidth, theHeight);
        spawn = new int[theWidth, theHeight];
        image = new GameObject[theWidth, theHeight];
        imageValues = GetPixels2D(inputMap);
        SetPixels2D(imageValues, tex);
        this.GetComponent<Renderer>().material.mainTexture = tex;
    }

    public void resetColorImage()
    {
        //colour picture
        ColorInputMap = Resources.Load("map") as Texture2D;
        image2 = new GameObject[theWidth, theHeight];
        tex2 = new Texture2D(theWidth, theHeight);
        ColorimageValues = GetPixels2D(ColorInputMap);
        SetPixels2D(ColorimageValues, tex2);
        showingcolour.GetComponent<Renderer>().material.mainTexture = tex2;
        LookingForGreen = new Color(greenR / 255, greenG / 255, greenB / 255);
        LookingForBlue = new Color(blueR / 255, blueG / 255, blueB / 255);
        LookingForYellow = new Color(yellowR / 255, yellowG / 255, yellowB / 255);
        LookingForBrown = new Color(brownR / 255, brownG / 255, brownB / 255);
        spawn2 = new bool[ColorInputMap.width, ColorInputMap.height];
    }


    public int[] FindHighestValue(List<Coords> list)
    {
        int tmp_int = 0;
        int[] coords = new int[4];
        //Find highest w value
        for (int i = 0; i < list.Count; i++)
        {
            if (list.ElementAt(i).getW() > tmp_int)
            {
                tmp_int = list.ElementAt(i).getW();
            }
        }
        coords[0] = tmp_int;
        tmp_int = ColorInputMap.width;
        //find the lowest w value
        for (int i = 0; i < list.Count; i++)
        {
            if (list.ElementAt(i).getW() < tmp_int)
            {
                tmp_int = list.ElementAt(i).getW();
            }
        }
        coords[1] = tmp_int;
        tmp_int = 0;
        //Find highest h value
        for (int i = 0; i < list.Count; i++)
        {
            if (list.ElementAt(i).getH() > tmp_int)
            {
                tmp_int = list.ElementAt(i).getH();
            }
        }
        coords[2] = tmp_int;
        tmp_int = ColorInputMap.height;
        //Find the lowest h value
        for (int i = 0; i < list.Count; i++)
        {
            if (list.ElementAt(i).getH() < tmp_int)
            {
                tmp_int = list.ElementAt(i).getH();
            }
        }
        coords[3] = tmp_int;
        return coords;
    }

    public int[] FindMiddle(List<Coords> list)
    {
        int[] tmp_array = new int[2];
        int totalXValue = 0;
        int totalYValue = 0;
        for (int w = 0; w < list.Count; w++)
        {
            totalXValue += list[w].getW();
            totalYValue += list[w].getH();
        }
        tmp_array[0] = (totalXValue / list.Count);
        tmp_array[1] = (totalYValue / list.Count);
        return tmp_array;
    }

    public void PlaceMountain(int width, int length, int x, int y, int nr)
    {
        x = -x;
        y = -y;

        int height;
        if (width < length)
        {
            length = width;
            height = width;
        }
        else
        {
            width = length;
            height = length;
        }
        //Debug.Log("Placing mountain @ " + x + ":" + y);

            gameObjects.Add((GameObject)Instantiate(mountainModel, new Vector3(x, (height / scale) * 7, y), Quaternion.identity) as GameObject);            
       // if (!secondmountain)
        //{
            gameObjects[nr].transform.localScale = new Vector3(width / scale, length / scale, height / scale);

       // }
    }


    public void updateContrast(float _ContrastAmount)
    {
        ContrastAmount = _ContrastAmount;
    }
    public void updateTreshold(float _TresholdAmount)
    {
        TresholdAmount = _TresholdAmount;
    }
    public void updateErosion(int _ErosionAmount)
    {
        ErosionAmount = _ErosionAmount;
    }
    public void updateWhiteBorder(int _WhiteBorderAmount)
    {
        WBamount = _WhiteBorderAmount;
    }
    public void updategreenSpread(int _greenSpread)
    {
        ColorSpreadGreen = _greenSpread;
    }
    public void updategreenR(float _greenR)
    {
        greenR = _greenR;
    }
    public void updategreenG(float _greenG)
    {
        greenG = _greenG;
    }
    public void updategreenB(float _greenB)
    {
        greenB = _greenB;
    }
    public void updateblueSpread(int _BlueSpread)
    {
        ColorSpreadBlue = _BlueSpread;
    }
    public void updateblueR(float _blueR)
    {
        blueR = _blueR;
    }
    public void updateblueG(float _blueG)
    {
        blueG = _blueG;
    }
    public void updateblueB(float _blueB)
    {
        blueB = _blueB;
    }
    public void updateyellowSpread(int _yellowSpread)
    {
        ColorSpreadYellow = _yellowSpread;
    }
    public void updateyellowR(float _yellowR)
    {
        yellowR = _yellowR;
    }
    public void updateyellowG(float _yellowG)
    {
        yellowG = _yellowG;
    }
    public void updateyellowB(float _yellowB)
    {
        yellowB = _yellowB;
    }
    public void updatebrownSpread(int _brownSpread)
    {
        ColorSpreadbrown = _brownSpread;
    }
    public void updatebrownR(float _brownR)
    {
        brownR = _brownR;
    }
    public void updatebrownG(float _brownG)
    {
        brownG = _brownG;
    }
    public void updatebrownB(float _brownB)
    {
        brownB = _brownB;
    }
}

public class Coords
{
    int w;
    int h;

    public Coords(int w, int h)
    {
        this.w = w;
        this.h = h;
    }

    public int getW()
    {
        return w;
    }
    public int getH()
    {
        return h;
    }

    public String getInfo()
    {
        return "W: " + w + " H: " + h;
    }
}