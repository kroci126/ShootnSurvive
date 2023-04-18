using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;

[RequireComponent (typeof (Rigidbody2D))]
[RequireComponent(typeof(Seeker))]
public class EnemyAI : MonoBehaviour
{
    //AI target
    public Transform target;

    // Times/second update
    public float updateRate = 2f;

    private Seeker seeker;
    private Rigidbody2D rb;
    
    //the calculated path
    public Path path;

    //AI speed per second
    public float speed = 300f; 
    public ForceMode2D fMode;

    [HideInInspector]
    public bool pathIsEnded = false;

    //Max distance from the AI to a waypoint to continue to the next waypoint
    public float nextWaypointDistance = 3;

    //The waypoint we are currently moving towards
    private int currentWaypoint = 0;

    private bool searchingForPlayer = false;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        if(target == null)
        {
            if(!searchingForPlayer) 
            {
                searchingForPlayer = true;
                StartCoroutine(SearchForPlayer());
            }
            return;
        }

        //Start a  new path to the target position, return the result to the OnPathComplete  method
        seeker.StartPath(transform.position, target.position, OnPathComplete);

        StartCoroutine(UpdatePath());
    }

    IEnumerator SearchForPlayer()
    {
        GameObject sResult = GameObject.FindGameObjectWithTag("Player");
        if (sResult == null)
        {
            yield return new WaitForSeconds(1f);
            StartCoroutine(SearchForPlayer());
        }else
        {
            target = sResult.transform;
            searchingForPlayer = false;
            StartCoroutine(UpdatePath());
            yield return false;
        }
    }

    IEnumerator UpdatePath()
    {
        if (target == null)
        {
            if (!searchingForPlayer)
            {
                searchingForPlayer = true;
                StartCoroutine(SearchForPlayer());
            }
            yield return false;
        }

        seeker.StartPath(transform.position, target.position, OnPathComplete);

        yield return new WaitForSeconds(1f / updateRate);
        StartCoroutine(UpdatePath());
    }

    public void OnPathComplete (Path p)
    {
        Debug.Log("We got a path. Did it have an error?" + p.error);
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    void FixedUpdate ()
    {
        if (target == null)
        {
            if (!searchingForPlayer)
            {
                searchingForPlayer = true;
                StartCoroutine(SearchForPlayer());
            }
            return;
        }

        if (path == null)
        {
            return;
        }

        if(currentWaypoint >= path.vectorPath.Count)
        {
            if (pathIsEnded)
            {
                return;
            }

            Debug.Log("End  of path reached.");
            pathIsEnded = true;
            return;
        }

        pathIsEnded = false;

        Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        dir *= speed * Time.fixedDeltaTime;

        //Move the AI
        rb.AddForce(dir, fMode);

        float dist = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
        if(dist < nextWaypointDistance)
        {
            currentWaypoint++;
            return;
        }
    }
}
