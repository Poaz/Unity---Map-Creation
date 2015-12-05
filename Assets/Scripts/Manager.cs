using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.UI;

public class Manager : Singleton<Manager> {
    [SerializeField] private Button Contrast;
    [SerializeField] private Button erosion;
    [SerializeField] private Button Reset;
    [SerializeField] private Button generate;
    [SerializeField] private Button ResetIngame;
    [SerializeField] private Button Respawn;
    public InputField ContrastAmount;
    public InputField ErosionAmount;
    public InputField TresholdAmount;
    //[SerializeField] private Button spawntrees;
    //GameObject canvas = GameObject.Find("Canvas");
    int width, height;
    public GameObject MainMenu;
    public GameObject PlayingMenu;
    int currentErosionAmount = 3;
    float currentContrastAmount = 1.0f;
    float currentTresholdAmount = 0.4f;

    // Use this for initialization
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
        ResetIngame.onClick.AddListener(() => { Navigator(5); });
        // combi.onClick.AddListener(() => { Navigator(6); });

        ContrastAmount.onEndEdit.AddListener(updateContrast);
        ErosionAmount.onEndEdit.AddListener(updateErosion);
        TresholdAmount.onEndEdit.AddListener(updateTreshold);

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
                WorldGeneration.Instance.resetImage();
                currentErosionAmount = 3;
                currentContrastAmount = 1.0f;
                currentTresholdAmount = 0.4f;
                break;
           /* case 6:
                Debug.Log("Combing meshes");

                break;*/
        }
    }
}
