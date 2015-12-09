using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text nr;
    public bool takeTime = false;
    public bool endTimer = false;
    private List<Vector2> point = new List<Vector2>();
    private float time;
    public Button start;
    public Button stop;
    String fileName;

    void Start()
    {
        start.onClick.AddListener(() => { takeTime = true; Debug.Log("Starting"); });
        stop.onClick.AddListener(() => { endTimer = true; Debug.Log("Ending"); });
        fileName = nr + ".txt";
    }

    void Update()
    {
        if (takeTime)
        {
            time += Time.deltaTime;

            var minutes = time / 60; //Divide the guiTime by sixty to get the minutes.
            var seconds = time % 60; //Use the euclidean division for the seconds.
            if (Input.GetKeyDown("p"))
            {
                Debug.Log("Logging...");
                point.Add(new Vector2(minutes, seconds));
            }
            if (endTimer)
            {
                Debug.Log("Ending");
                point.Add(new Vector2(minutes, seconds));
                Debug.Log(point.Count);
                takeTime = false;
            }
        }
        else {
            if (point.Count > 0)
            {
                var sr = File.CreateText(fileName);
                for (int w = 0; w < point.Count; w++)
                {
                    if (w == point.Count - 1)
                    {
                        sr.WriteLine("End Time: " + string.Format("{0:00} : {1:00} ", point[w].x, point[w].y));
                    }
                    else {
                        sr.WriteLine("Point " + (w + 1) + ": " + string.Format("{0:00} : {1:00} ", point[w].x, point[w].y) + "\n");
                    }
                }
                sr.Close();
            }
        }
    }
}