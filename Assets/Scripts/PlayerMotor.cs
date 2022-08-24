using UnityEngine.AI;
using UnityEngine;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerMotor : MonoBehaviour
{
    Transform target;// Target to follow
    NavMeshAgent agent;
    float rotateSpeed;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if(target != null)
        {
            agent.SetDestination(target.position);
            FaceTarget();
        }
    }

    public void MoveTo(Vector3 point)
    {
        agent.isStopped = false;
        agent.SetDestination(point);
    }

    public void RotateWithoutAgent(Vector3 currentPoint)
    {
        agent.isStopped = true;
        rotateSpeed = 30f;
        Vector3 directionToTarget = (currentPoint - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(directionToTarget.x, 0, directionToTarget.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotateSpeed);
    }

    public void FollowTarget(Interactible newTarget)
    {
        agent.stoppingDistance = newTarget.radius * 0.8f;
        //come closer to target by 20%
        //For some reason with radius of 1f this doesn't work
        agent.updateRotation = false;
        target = newTarget.interactionPoint;
    }

    public void StopFollowingTarget()
    {
        agent.stoppingDistance = 0f;
        agent.updateRotation = true;
        target = null;
    }

    public void FaceTarget()
    {
        rotateSpeed = 5f;
        Vector3 directionToTarget = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(directionToTarget.x, 0, directionToTarget.z));
        //using new Vector3 to avoid looking up and down
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        //5f is the speed of the rotation (smooth the rotation)
    }
}
