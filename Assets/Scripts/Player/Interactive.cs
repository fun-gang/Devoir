using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactive : MonoBehaviour
{
    private bool isActing;
    private GameObject actingObject;
   
    public void Act(InputAction.CallbackContext value) {
        if (actingObject != null && Movement.control) {
            actingObject.SendMessage("Act");
            isActing = true;
            actingObject.BroadcastMessage("HideHint");
        }
    }

    public void Put(InputAction.CallbackContext value) {
        if (actingObject != null && isActing && Movement.control) {
            actingObject.SendMessage("Stop");
            isActing = false;
        }
    }
    void OnTriggerStay2D(Collider2D collision) {
        if (collision.gameObject.tag == "Interactive") {
            if (isActing) {
                actingObject.BroadcastMessage("HideHint");
            }
            else {
                actingObject = collision.gameObject;
                actingObject.BroadcastMessage("ShowHint");
            }
        }
    }
    void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.tag == "Interactive" && !isActing) {
            actingObject = collision.gameObject;
            actingObject.BroadcastMessage("HideHint");
            actingObject = null;
        }
    }
}
