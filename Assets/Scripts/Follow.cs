using UnityEngine;
using System.Collections;

public class Follow : Singleton<Follow>
{

    public Transform target;
    public Vector3 offset = new Vector3(0,4,-10);
    public bool first, second, third;
    float speed = 2.0f;
    private float distance = 5.0f;
    private float xSpeed = 120.0f;
    private float ySpeed = 120.0f;

    private float yMinLimit = -20f;
    private float yMaxLimit = 80f;

    private float distanceMin = .5f;
    private float distanceMax = 45f;

    private Rigidbody rigidbody;

    float x = 0.0f;
    float y = 0.0f;




    // Use this for initialization
    void Start()
{
    first = true;

        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        rigidbody = GetComponent<Rigidbody>();

        // Make the rigid body not change rotation
        if (rigidbody != null)
        {
            rigidbody.freezeRotation = true;
        }
    }

// Update is called once per frame
    private void Update()
    {

        if (first)
        {
            transform.position = new Vector3(0, 15, 0);
        }
        else if (second)
        {
            transform.position = new Vector3(-250, 300, -250);
        }
        else if (third)
        {
            transform.position = new Vector3(target.transform.position.x, target.transform.position.y, target.transform.position.z) + offset;
            transform.rotation = Quaternion.Euler(22, 180, 4);

            if (target)
            {
                x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
                y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

                y = ClampAngle(y, yMinLimit, yMaxLimit);

                Quaternion rotation = Quaternion.Euler(y, x, 0);

                distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel") * 5, distanceMin, distanceMax);

                RaycastHit hit;
                if (Physics.Linecast(target.position, transform.position, out hit))
                {
                    distance -= hit.distance;
                }
                Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
                Vector3 position = rotation * negDistance + target.position;

                transform.rotation = rotation;
                transform.position = position;
            }
        }
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }
}