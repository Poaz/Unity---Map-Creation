using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;


public class WorldGeneration : Singleton<WorldGeneration>
{
    public GameObject pixPrefab;
    public Texture2D inputMap;
    public Texture2D ColorInputMap;
    private Texture2D tex;
    private Color[,] imageValues;
    private Color[,] ColorimageValues;
    private int[] counter = new int[2];
    GameObject[,] image;
    List<Coords> labels;
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
    public int ErosionAmount = 3;
    public float ContrastAmount = 1.0f;
    public float TresholdAmount = 0.4f;

    int ColorSpreadGreen = 30;
    int ColorSpreadBlue = 42;
    int ColorSpreadYellow = 40;
    int ColorSpreadRed = 40;
    Color LookingForGreen = new Color(110f / 255, 170f / 255, 120f  / 255);
    Color LookingForBlue =  new Color(110f / 255, 150f / 255, 210f / 255);
    Color LookingForYellow = new Color(200f / 255, 180f / 255, 70f / 255);
    Color LookingForRed = new Color(215f / 255, 90f / 255, 85f / 255);
    public int spawnX, spawnZ;

    void Start()
    {
        resetImage();
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
                if (spawn[w, h] == 3) //Here it the spawn arrays is true at that position.
                {
                    image[w, h] = (GameObject)Instantiate(pixPrefab, new Vector3(10.5f - w, 0, 10.5f - h), Quaternion.identity);
                    int tmp_int = (int)UnityEngine.Random.Range(1, 10);
                    if (tmp_int == 5)
                    {
                        int whatGrass = 0;//(int)UnityEngine.Random.Range(1, 1);
                        GameObject tmp_object = Grass[whatGrass];
                        tmp_object.transform.position = new Vector3((float)UnityEngine.Random.Range(0, 21) - w, 0.5f, (float)UnityEngine.Random.Range(0, 21) - h);
                        tmp_object.transform.Rotate(0, 20, 0);
                        Instantiate(tmp_object);
                    }
                }

                if(spawn[w,h] == 2)
                {
                    image[w, h] = (GameObject)Instantiate(pixPrefab, new Vector3(10.5f - w, 0, 10.5f - h), Quaternion.identity);
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
                    image[w, h] = (GameObject)Instantiate(pixPrefab, new Vector3(10.5f - w, 0, 10.5f - h), Quaternion.identity);
                    int tmp_int = (int)UnityEngine.Random.Range(1, 150);
                    if (tmp_int == 5)
                    {
                        int whatPlants = (int)UnityEngine.Random.Range(0, 7);
                        GameObject tmp_object = WaterPlants[whatPlants];
                        tmp_object.transform.position = new Vector3(10.5f - w, 0.5f, 10.5f - h);
                        Instantiate(tmp_object);
                    }
                }

                if(spawn[w,h] == 0)
                {
                    image[w, h] = (GameObject)Instantiate(pixPrefab, new Vector3(10.5f - w, 0, 10.5f - h), Quaternion.identity);
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
            h = labels[0].getH(); //Gets the x and y position for the pixel we are doing right now. 
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
        imageValues = Contrast(imageValues, ContrastAmount);
        SetPixels2D(imageValues, tex);
        this.GetComponent<Renderer>().material.mainTexture = tex;
    }

    public void CallGrassFire()
    {
        resetImage();
        Treshold(imageValues, TresholdAmount);
        imageValues = Erosion(imageValues, ErosionAmount);
        imageValues = Contrast(imageValues, ContrastAmount);
        imageValues = whiteborder(imageValues);
        InitailizeGrassfire();
        SetPixels2D(imageValues, tex);
        this.GetComponent<Renderer>().material.mainTexture = tex;
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
                i[w, h].r *= a;
                i[w, h].g *= a;
                i[w, h].b *= a;
            }
        }
        return i;
    }

    public Color[,] whiteborder(Color[,] i)
    {
        //i = Treshold(i, 0.5f);
        Color[,] result2 = new Color[i.GetLength(0), i.GetLength(1)];
        for (int w = 0; w < i.GetLength(0); w++)
        {
            for (int h = 0; h < i.GetLength(1); h++)
            {
                if (w < 20 || h < 20 || w > i.GetLength(0) - 20 || h > i.GetLength(1) - 20)
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
        //set source image to grayscale

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
                    Debug.Log("spawnpoint = " + spawnX + " , " + spawnZ);
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
        inputMap = Resources.Load("map") as Texture2D;
        ColorInputMap = Resources.Load("map") as Texture2D;
        parentsIsland = new List<GameObject>();
        parentsDirt = new List<GameObject>();
        parentsWater = new List<GameObject>();
        parentsDesert = new List<GameObject>();
        theWidth = inputMap.width;
        theHeight = inputMap.height;
        tex = new Texture2D(theWidth, theHeight);
        labels = new List<Coords>();
        spawn = new int[theWidth, theHeight];
        image = new GameObject[theWidth, theHeight];
        imageValues = GetPixels2D(inputMap);
        ColorimageValues = GetPixels2D(ColorInputMap);
        SetPixels2D(imageValues, tex);
        this.GetComponent<Renderer>().material.mainTexture = tex;

    }

    public void updateContrast(float _ContrastAmount)
    {
        ContrastAmount = _ContrastAmount;
        Debug.Log(ContrastAmount);
        Debug.Log(ErosionAmount);
        Debug.Log(TresholdAmount);
    }

    public void updateTreshold(float _TresholdAmount)
    {
        TresholdAmount = _TresholdAmount;
        Debug.Log(ContrastAmount);
        Debug.Log(ErosionAmount);
        Debug.Log(TresholdAmount);
    }

    public void updateErosion(int _ErosionAmount)
    {
        ErosionAmount = _ErosionAmount;
        Debug.Log(ContrastAmount);
        Debug.Log(ErosionAmount);
        Debug.Log(TresholdAmount);
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