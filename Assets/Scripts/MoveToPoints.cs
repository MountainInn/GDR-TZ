using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToPoints : MonoBehaviour, IResetable
{
    [SerializeField] float movementSpeed = 15f;
    [SerializeField] float deadzone = 5f;
    [SerializeField] Transform prefMark;
    [SerializeField] LineRenderer prefTrajectory;

    Queue<Waypoint> waypoints = new Queue<Waypoint>();
    Vector3 lastWaypointPosition;
    bool isMoving = true;

    void LateUpdate()
    {
        MoveToNextWaypoint();

        AddNextWaypoint();
    }

    void MoveToNextWaypoint()
    {
        if (isMoving && waypoints.Count > 0)
        {
            Waypoint nextWaypoint = waypoints.Peek();

            Vector3 nextPosition = nextWaypoint.position;

            Vector3 direction = (nextPosition - transform.position).normalized;
            transform.Translate(direction * movementSpeed * Time.fixedDeltaTime);

            nextWaypoint.UpdateTrajectory(transform.position);

            if (Vector3.Distance(transform.position, nextPosition) < deadzone)
            {
                waypoints.Dequeue().DestroyUnityObjects();
            }
        }
    }

    void AddNextWaypoint()
    {
        if (!GameOverScreen.isGameOverScreenActive && Input.GetMouseButtonDown(0))
        {
            Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            clickPosition.Scale(new Vector3(1,1,0));


            Waypoint waypoint
                = new Waypoint(point: Instantiate(prefMark, clickPosition, Quaternion.identity),
                                      trajectory: Instantiate(prefTrajectory, clickPosition, Quaternion.identity),
                                      previousPosition: (waypoints.Count > 0) ? lastWaypointPosition : transform.position);

            waypoints.Enqueue(waypoint);
            lastWaypointPosition = waypoint.position;
        }
    }

    public void Reset()
    {
        foreach ( var item in waypoints )
        {
            item.DestroyUnityObjects();
        }

        waypoints = new Queue<Waypoint>();
        StartMoving();
    }

    void StartMoving()
    {
        isMoving = true;
    }
    public void StopMoving()
    {
        isMoving = false;
    }

    struct Waypoint
    {
        Transform point;
        LineRenderer trajectory;

        public Vector3 position => point.position;

        public Waypoint(Transform point, LineRenderer trajectory, Vector3 previousPosition)
        {
            this.point = point;
            this.trajectory = trajectory;

            trajectory.positionCount = 2;
            trajectory.SetPositions(new Vector3[]{ point.position,  previousPosition });
        }

        /// Передвигает вторую точку LineRenderer'a
        public void UpdateTrajectory(Vector3 position)
        {
            trajectory.SetPosition(1, position);
        }

        /// Уничтожает содержащиеся игровые объекты
        public void DestroyUnityObjects()
        {
            Destroy(point.gameObject);
            Destroy(trajectory.gameObject);
        }
    }
}
