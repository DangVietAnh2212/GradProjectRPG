
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{
    public GameObject groundItemPrefab;
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
                if (EventSystem.current.IsPointerOverGameObject()) 
                {
                    return;//If mouse click outside of UI, do nothing
                }
                else if (MouseData.itemOnMouseDisplay != null)
                {
                    MouseData.itemOnMouse = false;
                    Destroy(MouseData.itemOnMouseDisplay);
                    Vector3 dropLocation = transform.position + Vector3.up * 3 + 
                        new Vector3(Random.Range(-2f, 2f), 0f, Random.Range(-2f,2f));
                    GameObject obj = Instantiate(groundItemPrefab, transform.position + Vector3.up * 3, Quaternion.identity);
                    obj.GetComponent<GroundItem>().item = MouseData.tempSlot.ItemSO;
                    obj.GetComponent<BillBoard>().cam = Camera.main;
                    obj.GetComponent<Rigidbody>().isKinematic = false;
                    MouseData.tempSlot.RemoveItem();
                    //If mouse click outside of UI but you have smt on the mouse, drop it on ground
                }

                Interactible interactible = hit.collider.GetComponent<Interactible>();
                if(interactible != null)
                {
                    print("Set Focus");
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
