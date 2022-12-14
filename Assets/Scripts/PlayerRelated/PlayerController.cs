
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{
    public GameObject groundItemPrefab;
    Camera cam;
    public LayerMask movementMask;
    PlayerMotor motor;
    Animator animator;
    public Interactable focus;
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
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100) && 
                !animator.GetCurrentAnimatorStateInfo(0).IsName("BaseAnimation.BasicSpellAttack") &&
                !animator.GetCurrentAnimatorStateInfo(0).IsName("BaseAnimation.MainHandCast"))
            {
                if (EventSystem.current.IsPointerOverGameObject()) 
                {
                    return;//If mouse click outside of UI, do nothing
                }
                else if (MouseData.itemOnMouseDisplay != null)
                {
                    MouseData.itemOnMouseRef.GetComponentInChildren<Image>().color = MouseData.colorOfItemOnMouse;
                    MouseData.itemOnMouse = false;
                    Destroy(MouseData.itemOnMouseDisplay);//Destroy the item on mouse and 
                    //give back the color of the inventory slot

                    for (int i = 0; i < MouseData.tempSlot.amount; i++)
                    {
                        Vector3 dropLocation = transform.position + Vector3.up * 3 +
                        new Vector3(Random.Range(-2f, 2f), 0f, Random.Range(-2f, 2f));
                        GameObject obj = Instantiate(groundItemPrefab, dropLocation, Quaternion.identity);
                        obj.GetComponent<GroundItem>().item = MouseData.tempSlot.ItemSO;
                        obj.GetComponent<GroundItem>().isNew = false;
                        obj.GetComponent<GroundItem>().inventoryItem = MouseData.tempSlot.inventoryItem;
                        obj.GetComponent<BillBoard>().cam = cam;
                        obj.GetComponent<Rigidbody>().isKinematic = false;
                    }
                    MouseData.tempSlot.RemoveItem();
                    //If mouse click outside of UI but you have smt on the mouse, drop it on ground
                }

                Interactable interactible = hit.collider.GetComponent<Interactable>();
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

        if (Input.GetMouseButtonDown(1) && !PauseMenu.gameIsPause)
        {
            CastXSpellToMousePos("basicSpellAtk");
        }
    }
    public void CastXSpellToMousePos(string xSpellTrigger)
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            RemoveFocus();
            motor.RotateWithoutAgent(hit.point);
            animator.SetTrigger(xSpellTrigger);
        }
    }
    void SetFocus(Interactable newFocus)
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
