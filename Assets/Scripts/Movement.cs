using UnityEngine;
using System.Collections;

public class Movement : Singleton<Movement>
{
    public float speed;
    private Animator anim;

    // Use this for initialization
    private void Start()
    {
        anim = this.gameObject.GetComponent<Animator>();

}

    // Update is called once per frame
    private void Update()
    {
        float move_Horizontal = Input.GetAxis("Horizontal");
        float move_Vertical = Input.GetAxis("Vertical");
       
        Vector3 movement = new Vector3(move_Horizontal, 0.0f, move_Vertical);
        movement = Camera.main.transform.TransformDirection(movement);
        //transform.Rotate(0, 90, 0);
        transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward, Camera.main.transform.up);
        if (move_Horizontal != 0 || move_Vertical != 0)
        {
            anim.SetInteger("Speed", 2);
        }
        else
        {
            anim.SetInteger("Speed", 0);
        }

        GetComponent<Rigidbody>().velocity = movement*speed;
    }
}   
