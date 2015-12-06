using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MapInCorner : MonoBehaviour {

    public GameObject ImageOnPanel;  ///set this in the inspector
    private Texture CurrentMap;
    private RawImage img;

    void Start()
    {
        CurrentMap = Resources.Load("map") as Texture;
        img = (RawImage)ImageOnPanel.GetComponent<RawImage>();

        img.texture = (Texture)CurrentMap;

    }
}
