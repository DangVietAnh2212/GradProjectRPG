
using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{
    Camera cam;
    public LayerMask movementMask;
    PlayerMotor motor;
    Animator animator;
    public Interactible focus;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        motor = GetComponent<PlayerMotor>();
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100) && 
                !animator.GetCurrentAnimatorStateInfo(0).IsName("BaseAnimation.BasicSpellAttack"))
            {
                Interactible interactible = hit.collider.GetComponent<Interactible>();
                if(interactible != null)
                {
                    SetFocus(interactible);
                }
                else if (Physics.Raycast(ray, out hit, 100, movementMask))
                {
                    //Move player where doesnt have interactible objects(IObj)
                    motor.MoveTo(hit.point);
                    RemoveFocus();
                }
            }
        }

        if (Input.GetMouseButton(1))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, 100))
            {
                RemoveFocus();
                motor.RotateWithoutAgent(hit.point);
                animator.SetTrigger("basicSpellAtk");
            }
        }
               
    }

    void SetFocus(Interactible newFocus)
    {
        if(newFocus != focus)
        {
            if (focus != null)
                focus.OnDefocus();
            focus = newFocus;
            motor.FollowTarget(newFocus);
        }
        newFocus.OnFocused(transform);
    }

    void RemoveFocus()
    {
        if(focus != null)
            focus.OnDefocus();
        focus = null;
        motor.StopFollowingTarget();
    }
}
