using UnityEngine;
using System.Collections;

public class PlatformsController : MonoBehaviour
{
    public GameObject mainPlatform;
    public GameObject[] objects;
    public float radius = 3,
                 maxSpeed = 70,
                 minAcceleration = 150;
    public float turns = 2;
    float speed = 0;
    float currentPosition = 0,
          targetPosition = 0;
    [SerializeField][HideInInspector]
    float deltaAngle = 72;
    bool moving = false;
    
    public void PositionObjects()
    {
        float angle;
        mainPlatform.transform.rotation = Quaternion.AngleAxis(currentPosition, Vector3.down);
        for (int n = 0; n < objects.Length; ++n)
        {
            angle = currentPosition + n * deltaAngle;
            objects[n].transform.rotation = Quaternion.AngleAxis(angle * turns, Vector3.up);
            angle *= Mathf.Deg2Rad;
            objects[n].transform.localPosition = mainPlatform.transform.position +
                                                 radius * (Vector3.back * Mathf.Cos(angle) +
                                                           Vector3.right * Mathf.Sin(angle));
            
        }
    }

    IEnumerator Move()
    {
        moving = true;
        float acceleration = 0;
        bool breaking = false;

        while (targetPosition != currentPosition)
        {
            float distance = targetPosition - currentPosition,
                  absDistance = Mathf.Abs(distance),
                  direction = Mathf.Sign(distance);

            if (absDistance < 0.25f)
            {
                targetPosition = currentPosition;
                break;
            }
            else if (direction != Mathf.Sign(speed))
            {
                acceleration = direction * minAcceleration;
            }
            else
            {
                if (!breaking)
                {
                    float breakAcceleration = (speed * speed) / (2 * Mathf.Abs(distance));
                    if (breakAcceleration > minAcceleration)
                    {
                        acceleration = -direction * breakAcceleration;
                        breaking = true;
                    }
                    else if (Mathf.Abs(speed) < maxSpeed)
                    {
                        acceleration = direction * minAcceleration;
                    }
                }
            }

            currentPosition += speed * Time.deltaTime;

            if (Mathf.Sign(targetPosition - currentPosition) != direction)
                currentPosition = targetPosition;
            else
            {
                speed += acceleration * Time.deltaTime;
                if (Mathf.Abs(speed) > maxSpeed)
                    speed = Mathf.Sign(speed) * maxSpeed;
            }

            PositionObjects();
            yield return null;
        }

        speed = 0;
        currentPosition = targetPosition;
        PositionObjects();

        moving = false;
    }

    void Start()
    {
        if (objects.Length != 0)
            deltaAngle = 360 / objects.Length;
        else
            deltaAngle = 360;
    }

    public void Move(int steps)
    {
        targetPosition += steps * deltaAngle;

        if (!moving)
            StartCoroutine("Move");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            Move(-1);

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            Move(1);
    }
}
