using UnityEngine;
using System;
using System.Collections.Generic;

public class GrassGeneration : Singleton<GrassGeneration> {
    public GameObject[] Grass;
    public Texture2D area;
    public GameObject Prefab;
    private Texture2D tex;
    List<Coords> labels;
    private bool[,] spawn;
    private int theWidth;
    private int theHeight;
    Color[,] image;
    private Boolean second = true;
    private bool first;
    Color ColorWeAreLookingFor = new Color(0f / 255, 0f / 255, 0f / 255);
    private int spreadG = 30;


    // Use this for initialization
    void Start ()
    {
        area = Resources.Load("map") as Texture2D;
        first = true;
        theWidth = area.width;
        theHeight = area.height;
        spawn = new bool[area.width, area.height];
        labels = new List<Coords>();
        tex = new Texture2D(area.width, area.height);
        image = GetPixels2D(area);
        //Contrast(image, 1.1f);

        SetPixels2D(image, tex);
        this.GetComponent<Renderer>().material.mainTexture = tex;
	}

    public void CallGenerateGrass() {
        Erosion(image, 8);
        GenerateGrass();
    }

    public void GenerateGrass() {
        for (int w = 0; w < area.width; w++) {
            for (int h = 0; h < area.height; h++) {
                if (image[w, h].grayscale * 255 == 255) {
                    if (second) {
                        SequentialGrassFire(w, h);
                        second = false;
                    }
                }
            }
        }
        int count = 0;
        for (int w = 0; w < area.width; w++) {
            for (int h = 0; h < area.height; h++) {
                if (!spawn[w, h]) {
                    count++;
                    int tmp_int = (int)UnityEngine.Random.Range(1, 50);
                    if (tmp_int == 5) {
                        int whatGrass = 0;//(int)UnityEngine.Random.Range(1, 1);
                        GameObject tmp_object = Grass[whatGrass];
                        tmp_object.transform.position = new Vector3(10.5f - w, 0, 10.5f - h);
                        Instantiate(tmp_object);
                    }
                }
            }
        }
    }

    public Color[,] GetPixels2D(Texture2D t) {
        Color[,] texture2d = new Color[t.width, t.height];
        Color[] texture1d = t.GetPixels();

        for (int h = 0; h < t.height; h++) {
            for (int w = 0; w < t.width; w++) {
                texture2d[w, h] = texture1d[h * t.width + w];
            }
        }
        return texture2d;
    }

    public void SetPixels2D(Color[,] i, Texture2D t) {
        Color[] texture1d = new Color[i.Length];
        int width = i.GetLength(0);
        int height = i.GetLength(1);

        for (int h = 0; h < height; h++) {
            for (int w = 0; w < width; w++) {
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
        while (labels.Count > 0) //as long as I have new position to test, this loop will run. 
        {
            h = labels[0].getH(); //Gets the x and y position for the pixel we are doing right now. 
            w = labels[0].getW(); //^
            if (!spawn[w, h])
            {
                //imageValues[w, h] = new Color(0.5f, 0.5f, 0.5f); //Change the color so I can't add it again. 
                spawn[w, h] = true;
                //This I will use later, when I have to spawn cubes for each pixel that was hit by the grassfire function. True meaning that it was hit, and therefore shouldn't get spawned later on.
                if (h - 1 >= 0 && image[w, h - 1].grayscale * 255 == 255) //if we have a pixel above us. 
                    labels.Add(new Coords(w, h - 1));
                if (h + 1 < theHeight && image[w, h + 1].grayscale * 255 == 255) //if we have a pixel below us. 
                    labels.Add(new Coords(w, h + 1));
                if (w - 1 >= 0 && image[w - 1, h].grayscale * 255 == 255) //if we have a pixel to the right. 
                    labels.Add(new Coords(w - 1, h));
                if (w + 1 < theWidth && image[w + 1, h].grayscale * 255 == 255) //if we have a pixel to the left. 
                    labels.Add(new Coords(w + 1, h));
                labels.RemoveAt(0);
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
    public Color[,] Erosion(Color[,] i, int k)
    {
        //set source image to grayscale
        i = Treshold(i, 0.4f);

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
        Callwhiteborder();
        return result;
    }
    public void Callwhiteborder()
    {
        image = whiteborder(image);
        SetPixels2D(image, tex);
        //this.GetComponent<Renderer>().material.mainTexture = tex;
    }

    public Color[,] whiteborder(Color[,] i)
    {
        //i = Treshold(i, 0.5f);
        Color[,] result2 = new Color[i.GetLength(0), i.GetLength(1)];
        for (int w = 0; w < i.GetLength(0); w++)
        {
            for (int h = 0; h < i.GetLength(1); h++)
            {
                if (w < 10 || h < 10 || w > i.GetLength(0) - 10 || h > i.GetLength(1) - 10)
                {
                    i[w, h].r = 1;
                    i[w, h].g = 1;
                    i[w, h].b = 1;
                }
            }
        }
        return i;
    }

    public bool DetectColor(Color i, Color targetColor, int spread) {
        bool tmp_bool;
        Color pix = i;
        if (Mathf.Abs(pix.r - targetColor.r) * 255f < spread && Mathf.Abs(pix.g - targetColor.g) * 255f < spread &&
            Mathf.Abs(pix.b - targetColor.b) * 255f < spread) {
            tmp_bool = true;
        } else {
            tmp_bool = false;
        }
        return tmp_bool;
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
}
