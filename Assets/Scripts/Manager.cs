using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.UI;

public class Manager : Singleton<Manager> {
    [SerializeField] private Button Contrast;
    [SerializeField] private Button erosion;
    [SerializeField] private Button surprise;
    [SerializeField] private Button generate;
    //[SerializeField] private Button combi;
    //[SerializeField] private Button initate;
    //[SerializeField] private Button spawntrees;
    int width;
    int height;

    // Use this for initialization
    public void Start()
    {
        width = WorldGeneration.Instance.inputMap.width;
        height = WorldGeneration.Instance.inputMap.height;
        Contrast.onClick.AddListener(() => { Navigator(0); });
        erosion.onClick.AddListener(() => { Navigator(1); });
        surprise.onClick.AddListener(() => { Navigator(2); });
        generate.onClick.AddListener(() => { Navigator(3); });
       // initate.onClick.AddListener(() => { Navigator(4); });
       // spawntrees.onClick.AddListener(() => { Navigator(5); });
       // combi.onClick.AddListener(() => { Navigator(6); });
    }

    public void Navigator(int nr) {
        switch (nr) {
            case 0:
                Follow.Instance.first = true;
                Follow.Instance.second = false;
                Follow.Instance.third = false;
                WorldGeneration.Instance.CallRGB2Grayscale();
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
                Follow.Instance.first = false;
                Follow.Instance.second = false;
                Follow.Instance.third = true;
                Movement.Instance.transform.position = new Vector3(-width / 2, 1, -height / 2);
                break;
            case 3:
                WorldGeneration.Instance.CallGrassFire();
                WorldGeneration.Instance.CallCombineMesh(true);
                Follow.Instance.first = false;
                Follow.Instance.second = false;
                Follow.Instance.third = true;
                Movement.Instance.transform.position = new Vector3(-width / 2, 1, -height / 2);
                break;
            /*case 4:
                Debug.Log("Initate");
                Follow.Instance.first = false;
                Follow.Instance.second = false;
                Follow.Instance.third = true;

                break;
            case 5:
                Debug.Log("Generating Trees");
                Follow.Instance.first = false;
                Follow.Instance.second = true;
                Follow.Instance.third = false;

                break;
            case 6:
                Debug.Log("Combing meshes");

                break;*/
        }
    }
}
