using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMove : MonoBehaviour
{
    [SerializeField]
    Transform[] waypoints;
    [SerializeField]
    float speed;
    [SerializeField]
    float turnSpeed;
    [SerializeField]
    float WaitTime;

    void Start()
    {
        Vector3[] waypointsPos = new Vector3[waypoints.Length];
        for (int i = 0; i < waypoints.Length; i++)
        {
            waypointsPos[i] = waypoints[i].position;
        }

        StartCoroutine(MovingWay(waypointsPos));
    }

    IEnumerator MovingWay(Vector3[] waypoint)
    {
        transform.position = waypoint[0];
        int waypointIndex = 1;
        Vector3 targetWay = waypoint[waypointIndex];
        transform.LookAt(targetWay);

        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetWay, speed * Time.deltaTime);
            if(transform.position == targetWay)
            {
                waypointIndex = (waypointIndex + 1) % waypoint.Length;
                targetWay = waypoint[waypointIndex];
                yield return new WaitForSeconds(WaitTime);

            }
            yield return null;
            yield return StartCoroutine(TurnFace(targetWay));
        }
    }

    IEnumerator TurnFace(Vector3 Target)
    {
        Vector3 dirTarget = (Target - transform.position).normalized;
        float targetAngle = 90 - Mathf.Atan2(dirTarget.z, dirTarget.x) * Mathf.Rad2Deg;

        while(Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle)) > 0.005f)
        {
            float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, turnSpeed * Time.deltaTime);
            transform.eulerAngles = Vector3.up * angle;
            yield return null;
        }
    }
}
