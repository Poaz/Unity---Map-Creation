using UnityEngine;
using System;
using System.Collections.Generic;

public class CactusGeneration : Singleton<CactusGeneration>{
    public GameObject[] Cactus;
    public Texture2D area;
    public GameObject Prefab;
    private Texture2D tex;
    List<Coords> labels;
    private bool[,] spawn;
    private int theWidth;
    private int theHeight;
    Color[,] image;
    private bool first;
    Color ColorWeAreLookingFor = new Color(200f / 255, 180f / 255, 70f / 255);
    private int spreadG = 40;


    // Use this for initialization
    void Start()
    {
        area = Resources.Load("map") as Texture2D;
        first = true;
        theWidth = area.width;
        theHeight = area.height;
        spawn = new bool[area.width, area.height];
        labels = new List<Coords>();
        tex = new Texture2D(area.width, area.height);
        image = GetPixels2D(area);
        Contrast(image, 1.1f);

        SetPixels2D(image, tex);
        this.GetComponent<Renderer>().material.mainTexture = tex;
    }

    public void CallGenerateCactus()
    {
        GenerateCactus();
    }

    public void GenerateCactus()
    {
        for (int w = 0; w < area.width; w++)
        {
            for (int h = 0; h < area.height; h++)
            {
                if (!DetectColor(image[w, h], ColorWeAreLookingFor, spreadG))
                {
                    if (first)
                    {
                        SequentialGrassFire(w, h);
                        first = false;
                    }
                }
            }
        }
        int count = 0;
        for (int w = 0; w < area.width; w++)
        {
            for (int h = 0; h < area.height; h++)
            {
                if (!spawn[w, h])
                {
                    count++;
                    int tmp_int = (int)UnityEngine.Random.Range(1, 700);
                    if (tmp_int == 5)
                    {
                        int whatCactus = (int)UnityEngine.Random.Range(0, 18);
                        GameObject tmp_object = Cactus[whatCactus];
                        tmp_object.transform.position = new Vector3(10.5f - w, 0, 10.5f - h);
                        Instantiate(tmp_object);
                    }
                }
            }
        }
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

    public void SequentialGrassFire(int x, int y)
    {
        int h;
        int w;
        labels.Add(new Coords(x, y)); //Adds the coords for the first position.
        while (labels.Count > 0)
        {
            h = labels[0].getH(); //Gets the x and y position for the pixel we are doing right now. 
            w = labels[0].getW(); //^
            if (!spawn[w, h])
            {
                spawn[w, h] = true;
                //This I will use later, when I have to spawn cubes for each pixel that was hit by the grassfire function. True meaning that it was hit, and therefore shouldn't get spawned later on.
                if (h - 1 >= 0 && !DetectColor(image[w, h - 1], ColorWeAreLookingFor, spreadG)) //if we have a pixel above us. 
                    labels.Add(new Coords(w, h - 1));
                if (h + 1 < theHeight && !DetectColor(image[w, h + 1], ColorWeAreLookingFor, spreadG)) //if we have a pixel below us. 
                    labels.Add(new Coords(w, h + 1));
                if (w - 1 >= 0 && !DetectColor(image[w - 1, h], ColorWeAreLookingFor, spreadG)) //if we have a pixel to the right. 
                    labels.Add(new Coords(w - 1, h));
                if (w + 1 < theWidth && !DetectColor(image[w + 1, h], ColorWeAreLookingFor, spreadG)) //if we have a pixel to the left. 
                    labels.Add(new Coords(w + 1, h));
                labels.RemoveAt(0);
                //Removes the pixel we just worked with from the List, so next time we run the while loop, it should work with the next one in line. 
                //Debug.Log("Size: " + labels.Count + "Coord" + labels[0].getInfo() + " The boo value is: " + spawn[w, h]); //Just to check what is going on.
            }
            else
            {
                labels.RemoveAt(0);
            }
        }
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
}