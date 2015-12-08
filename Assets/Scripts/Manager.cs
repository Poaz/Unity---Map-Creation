using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.UI;

public class Manager : Singleton<Manager> {
    [SerializeField] private Button Contrast;
    [SerializeField] private Button erosion;
    [SerializeField] private Button Reset;
    [SerializeField] private Button generate;
    [SerializeField] private Button LookForBlue;
    [SerializeField] private Button Respawn;
    [SerializeField] private Button ResetColors;



    int currentErosionAmount = 3;
    float currentContrastAmount = 1.2f;
    float currentTresholdAmount = 0.6f;

    public InputField ContrastAmount;
    public InputField ErosionAmount;
    public InputField TresholdAmount;

    public InputField greenSpread;
    public InputField greenR;
    public InputField greenG;
    public InputField greenB;

    public InputField blueSpread;
    public InputField blueR;
    public InputField blueG;
    public InputField blueB;

    public InputField yellowSpread;
    public InputField yellowR;
    public InputField yellowG;
    public InputField yellowB;

    int currentgreenSpread = 45;
    int currentblueSpread = 50;
    int currentyellowSpread = 50;

    float currentgreenR = 90;
    float currentgreenG = 180;
    float currentgreenB = 80;

    float currentblueR = 90;
    float currentblueG = 130;
    float currentblueB = 210;

    float currentyellowR = 200;
    float currentyellowG = 180;
    float currentyellowB = 70;

    int width, height;
    public GameObject MainMenu;
    public GameObject PlayingMenu;

    public void Start()
    {
        PlayingMenu.SetActive(false);
        width = WorldGeneration.Instance.inputMap.width;
        height = WorldGeneration.Instance.inputMap.height;
        Contrast.onClick.AddListener(() => { Navigator(0); });
        erosion.onClick.AddListener(() => { Navigator(1); });
        Reset.onClick.AddListener(() => { Navigator(2); });
        generate.onClick.AddListener(() => { Navigator(3); });
        Respawn.onClick.AddListener(() => { Navigator(4); });
        LookForBlue.onClick.AddListener(() => { Navigator(5); });
        ResetColors.onClick.AddListener(() => { Navigator(6); });

        ContrastAmount.onEndEdit.AddListener(updateContrast);
        ErosionAmount.onEndEdit.AddListener(updateErosion);
        TresholdAmount.onEndEdit.AddListener(updateTreshold);

        greenSpread.onEndEdit.AddListener(updategreenSpread);
        greenR.onEndEdit.AddListener(updategreenR);
        greenG.onEndEdit.AddListener(updategreenG);
        greenB.onEndEdit.AddListener(updategreenB);
        blueSpread.onEndEdit.AddListener(updateblueSpread);
        blueR.onEndEdit.AddListener(updateblueR);
        blueG.onEndEdit.AddListener(updateblueG);
        blueB.onEndEdit.AddListener(updateblueB);
        yellowSpread.onEndEdit.AddListener(updateyellowSpread);
        yellowR.onEndEdit.AddListener(updateyellowR);
        yellowG.onEndEdit.AddListener(updateyellowG);
        yellowB.onEndEdit.AddListener(updateyellowB);



    }
    public void updateContrast(string hmm)
    {
        currentContrastAmount = float.Parse(hmm);
        WorldGeneration.Instance.updateContrast(currentContrastAmount);
    }
    public void updateErosion(string hmm)
    {
        currentErosionAmount = int.Parse(hmm);
        WorldGeneration.Instance.updateErosion(currentErosionAmount);
    }
    public void updateTreshold(string hmm)
    {
        currentTresholdAmount = float.Parse(hmm);
        WorldGeneration.Instance.updateTreshold(currentTresholdAmount);
    }

    public void updategreenSpread(string hmm)
    {
        currentgreenSpread = int.Parse(hmm);
        WorldGeneration.Instance.updategreenSpread(currentgreenSpread);
    }
    public void updategreenR(string hmm)
    {
        currentgreenR = float.Parse(hmm);
        WorldGeneration.Instance.updategreenR(currentgreenR);
    }
    public void updategreenG(string hmm)
    {
        currentgreenG = float.Parse(hmm);
        WorldGeneration.Instance.updategreenG(currentgreenG);
    }
    public void updategreenB(string hmm)
    {
        currentgreenB = float.Parse(hmm);
        WorldGeneration.Instance.updategreenB(currentgreenB);
    }

    public void updateblueSpread(string hmm)
    {
        currentblueSpread = int.Parse(hmm);
        WorldGeneration.Instance.updateblueSpread(currentblueSpread);
    }
    public void updateblueR(string hmm)
    {
        currentblueR = float.Parse(hmm);
        WorldGeneration.Instance.updateblueR(currentblueR);
    }
    public void updateblueG(string hmm)
    {
        currentblueG = float.Parse(hmm);
        WorldGeneration.Instance.updateblueG(currentblueG);
    }
    public void updateblueB(string hmm)
    {
        currentblueB = float.Parse(hmm);
        WorldGeneration.Instance.updateblueB(currentblueB);
    }

    public void updateyellowSpread(string hmm)
    {
        currentyellowSpread = int.Parse(hmm);
        WorldGeneration.Instance.updateyellowSpread(currentyellowSpread);
    }
    public void updateyellowR(string hmm)
    {
        currentyellowR = float.Parse(hmm);
        WorldGeneration.Instance.updateyellowR(currentyellowR);
    }
    public void updateyellowG(string hmm)
    {
        currentyellowG = float.Parse(hmm);
        WorldGeneration.Instance.updateyellowG(currentyellowG);
    }
    public void updateyellowB(string hmm)
    {
        currentyellowB = float.Parse(hmm);
        WorldGeneration.Instance.updateyellowB(currentyellowB);
    }

    public void Navigator(int nr) {
        switch (nr) {
            case 0:
                Follow.Instance.first = true;
                Follow.Instance.second = false;
                Follow.Instance.third = false;
                //WorldGeneration.Instance.CallRGB2Grayscale();
                WorldGeneration.Instance.CallContrast();
                break;
            case 1:
                Follow.Instance.first = true;
                Follow.Instance.second = false;
                Follow.Instance.third = false;
                WorldGeneration.Instance.CallErosion();
                WorldGeneration.Instance.Callwhiteborder();
                break;
            case 2:
                Follow.Instance.first = true;
                Follow.Instance.second = false;
                Follow.Instance.third = false;
                WorldGeneration.Instance.resetImage();
                currentErosionAmount = 3;
                currentContrastAmount = 1.0f;
                currentTresholdAmount = 0.4f;
                //eventually it will reset the picture!
                break;
            case 3:
                WorldGeneration.Instance.CallGrassFire();
                WorldGeneration.Instance.CallCombineMesh(true);
                Follow.Instance.first = false;
                Follow.Instance.second = false; 
                Follow.Instance.third = true;
                Movement.Instance.transform.position = new Vector3(-(WorldGeneration.Instance.spawnX), 1, -(WorldGeneration.Instance.spawnZ));
                MainMenu.SetActive(false);
                PlayingMenu.SetActive(true);

                break;
            case 4:
                Follow.Instance.first = false;
                Follow.Instance.second = false;
                Follow.Instance.third = true;
                Movement.Instance.transform.position = new Vector3(-(WorldGeneration.Instance.spawnX), 1, -(WorldGeneration.Instance.spawnZ));

                break;
            case 5:
                Follow.Instance.first = true;
                Follow.Instance.second = false;
                Follow.Instance.third = false;
                WorldGeneration.Instance.CallLookForColors();
                break;
            case 6:
                Follow.Instance.first = true;
                Follow.Instance.second = false;
                Follow.Instance.third = false;
                WorldGeneration.Instance.resetColorImage();
                break;
        }
    }
}
