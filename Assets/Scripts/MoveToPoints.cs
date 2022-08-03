using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToPoints : MonoBehaviour, IResetable
{
    [SerializeField] float movementSpeed = 15f;
    [SerializeField] float deadzone = 5f;
    [SerializeField] Transform prefMark;
    [SerializeField] LineRenderer prefTrajectory;

    Queue<TrajectoryPoint> points = new Queue<TrajectoryPoint>();
    Vector3 lastPointPosition;
    bool isMoving = true;

    void LateUpdate()
    {
        MoveToNextPoint();

        AddNextPoint();
    }

    void MoveToNextPoint()
    {
        if (isMoving && points.Count > 0)
        {
            TrajectoryPoint nextPoint = points.Peek();

            Vector3 nextPosition = nextPoint.position;

            Vector3 direction = (nextPosition - transform.position).normalized;
            transform.Translate(direction * movementSpeed * Time.fixedDeltaTime);

            nextPoint.UpdateTrajectory(transform.position);

            if (Vector3.Distance(transform.position, nextPosition) < deadzone)
            {
                points.Dequeue().DestroyUnityObjects();
            }
        }
    }

    void AddNextPoint()
    {
        if (!GameOverScreen.isGameOverScreenActive && Input.GetMouseButtonDown(0))
        {
            Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            clickPosition.Scale(new Vector3(1,1,0));


            TrajectoryPoint trajectoryPoint
                = new TrajectoryPoint(point: Instantiate(prefMark, clickPosition, Quaternion.identity),
                                      trajectory: Instantiate(prefTrajectory, clickPosition, Quaternion.identity),
                                      previousPosition: (points.Count > 0) ? lastPointPosition : transform.position);

            points.Enqueue(trajectoryPoint);
            lastPointPosition = trajectoryPoint.position;
        }
    }

    public void Reset()
    {
        foreach ( var item in points )
        {
            item.DestroyUnityObjects();
        }

        points = new Queue<TrajectoryPoint>();
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

    struct TrajectoryPoint
    {
        Transform point;
        LineRenderer trajectory;

        public Vector3 position => point.position;

        public TrajectoryPoint(Transform point, LineRenderer trajectory, Vector3 previousPosition)
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
