using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    public GameObject platform;

    public GameObject[] myWaypoints;

    [Range(0.0f, 10.0f)]
    public float moveSpeed = 5f; // enemy move speed
    public float waitAtWaypointTime = 1f; 

    public bool loop = true; // should it loop through the waypoints

    private Transform _transform;
    private int _myWaypointIndex = 0;
    private float _moveTime;
    private bool _moving = true;

    void Start()
    {
        _transform = platform.transform;
        _moveTime = 0f;
        _moving = true;
    }

    void Update()
    {
        // if beyond _moveTime, then start moving
        if (Time.time >= _moveTime)
        {
            Movement();
        }
    }

    void Movement()
    {
        // if there isn't anything in My_Waypoints
        if ((myWaypoints.Length != 0) && (_moving))
        {

            // move towards waypoint
            _transform.position = Vector3.MoveTowards(_transform.position, myWaypoints[_myWaypointIndex].transform.position, moveSpeed * Time.deltaTime);

            // if the enemy is close enough to waypoint, make it's new target the next waypoint
            if (Vector3.Distance(myWaypoints[_myWaypointIndex].transform.position, _transform.position) <= 0)
            {
                _myWaypointIndex++;
                _moveTime = Time.time + waitAtWaypointTime;
            }

            // reset waypoint back to 0 for looping, otherwise flag not moving for not looping
            if (_myWaypointIndex >= myWaypoints.Length)
            {
                if (loop)
                    _myWaypointIndex = 0;
                else
                    _moving = false;
            }
        }
    }
}
