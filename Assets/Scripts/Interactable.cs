
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public float radius = 1.5f;
    protected bool isFocus = false;
    protected bool hasInteracted = false;
    protected Transform player;
    public Transform interactionPoint;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(interactionPoint.position, radius);
    }

    public virtual void Interact()
    {
        //this one is meant to be overwriten
        print($"Interacting with {transform.name}");
    }

    private void Update()
    {
        //Debug.Log(isFocus);
        if (isFocus && !hasInteracted)
        {
            float distanceToPlayer = Vector3.Distance(interactionPoint.position, player.position);
            if(distanceToPlayer <= radius)
            {
                Interact();
                hasInteracted = true;
            }
        }
    }

    public void OnFocused(Transform playerTransform)
    {
        isFocus = true;
        player = playerTransform;
        hasInteracted = false;
    }

    public void OnDefocus()
    {
        isFocus = false;
        player = null;
        hasInteracted = false;
    }
}
