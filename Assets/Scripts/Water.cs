using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using System.Collections;


[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

//[RequireComponent(typeof(BoxCollider))]
public class Water : Singleton<Water> {

    //private Texture whatevercolor = Resources.Load("grassland2.jpg") as Texture;

    public void Start()
{
        //yield return new WaitForSeconds(3f);

        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        //MeshCollider[] meshColliders = GetComponentsInChildren<MeshCollider>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];
       // CombineInstance[] combinecolider = new CombineInstance[meshColliders.Length];
        int i = 0;
        while (i < meshFilters.Length) {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(true);
            i++;
        }

        Material mat = Resources.Load("water2") as Material;
        //renderer.material = mat;



        transform.GetComponent<MeshFilter>().mesh = new Mesh();
        transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
        gameObject.GetComponent<Renderer>().material = mat;
        //MeshCollider collider = transform.gameObject.AddComponent<MeshCollider>();
        //collider.size = new Vector3(1, 5, 1);
        //transform.gameObject.AddComponent<BoxCollider>();

        //BoxCollider.size = new Vector3(GetComponent<Collider>().size.x, ySize, GetComponent<Collider>().size.z);
        transform.GetComponent<MeshFilter>().mesh.Optimize();
        
        transform.gameObject.SetActive(true);
        this.gameObject.isStatic = true;
        
    }
}