using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class Test : Singleton<Test> {
    List<Vector3> pos = new List<Vector3>();
    List<Vector3> SavePoints = new List<Vector3>();
    //public GameObject player;
    public bool isRunning = false;
    public Texture2D inputMap;
    public Texture2D tempMap;
    Color[,] tempColor;
    public bool haveColored = true;
    public GameObject showingPath;
    public int TestNr = 0;
    

    // Use this for initialization
    void Start () {
        inputMap = Resources.Load("map") as Texture2D;
        tempMap = new Texture2D(inputMap.width, inputMap.height);
        tempColor = WorldGeneration.Instance.GetPixels2D(inputMap);

    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (isRunning)
        {
            pos.Add(new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z));
            if (Input.GetKeyDown("p")) {
                SavePoints.Add(new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z));
            }
        }
        if (!isRunning && !haveColored) {

            for (int i = 0; i < pos.Count; i++)
            {
                int x = (int)pos[i].x;
                int y = (int)pos[i].z;
                 x = -x;
                 y = -y;
                if (x > 0 && x < tempMap.width && y > 0 && y < tempMap.height) {
                    for (int w = x - 1; w < x + 1; w++)
                    {
                        for (int h = y - 1; h < y + 1; h++)
                        {
                            tempColor[w, h] = Color.red;
                        }
                    }
                }
                
            }
            for (int i = 0; i < SavePoints.Count; i++)
            {
                int x = (int)SavePoints[i].x;
                int y = (int)SavePoints[i].z;
                x = -x;
                y = -y;
                if (x > 0 && x < tempMap.width && y > 0 && y < tempMap.height)
                {
                    for (int w = x - 5; w < x + 5; w++)
                    {
                        for (int h = y - 5; h < y + 5; h++)
                        {
                            tempColor[w, h] = Color.cyan;
                        }
                    }
                }

            }
            WorldGeneration.Instance.SetPixels2D(tempColor, tempMap);
            showingPath.GetComponent<Renderer>().material.mainTexture = tempMap;
            System.IO.File.WriteAllBytes(Application.dataPath + "/" + "testing"+TestNr+".png", tempMap.EncodeToPNG());
            haveColored = true;
        }
    }



}

