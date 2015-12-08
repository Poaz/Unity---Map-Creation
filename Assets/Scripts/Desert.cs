using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using System.Collections;


[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
//[RequireComponent(typeof(BoxCollider))]
public class Desert : Singleton<Desert> {
	
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

        Texture mat = Resources.Load("SandTexture") as Texture;
        //renderer.material = mat;
        transform.GetComponent<MeshFilter>().mesh = new Mesh();
        transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
        this.GetComponent<Renderer>().material.mainTexture = mat;
        transform.gameObject.AddComponent<BoxCollider>();
        transform.GetComponent<MeshFilter>().mesh.Optimize();
        
        transform.gameObject.SetActive(true);
        this.gameObject.isStatic = true;
        
    }
}