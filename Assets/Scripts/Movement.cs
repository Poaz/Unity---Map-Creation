using UnityEngine;
using System.Collections;

public class Movement : Singleton<Movement>
{
    public float speed;

    // Use this for initialization
    private void Start()
    {
        print("Starting up...");
    }

    // Update is called once per frame
    private void Update()
    {
        float move_Horizontal = Input.GetAxis("Horizontal");
        float move_Vertical = Input.GetAxis("Vertical");
       
        Vector3 movement = new Vector3(move_Horizontal, 0.0f, move_Vertical);
        movement = Camera.main.transform.TransformDirection(movement);

        GetComponent<Rigidbody>().velocity = movement*speed;
    }
}   
