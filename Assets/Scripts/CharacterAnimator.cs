using UnityEngine.AI;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    const float locomotionAnimationSmooth = .1f;
    Animator animator;
    NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float speedPercent = agent.velocity.magnitude / agent.speed;
        animator.SetFloat("speedPercent", speedPercent, locomotionAnimationSmooth, Time.deltaTime);
    }
}
