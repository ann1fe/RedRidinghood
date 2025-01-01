using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// By pressing "E", the player picks up the object it just collided with and holds it in front of himself (in holdPos).
/// Object needs to have "canPickUp" tag.
/// Pressing "E" again will drop the object
/// </summary>
public class PickupScript : MonoBehaviour
{
    public TextMeshProUGUI collectPrompt;
    public Transform holdPos; // location where we will hold the object
    private GameObject collidedObject; //object which we pick up
    private GameObject heldObj; //object which we pick up

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (collidedObject && heldObj == null)
            {
                heldObj = collidedObject;
                PickUpObject(heldObj);
            }
            else if (heldObj)
            {
                DropObject(heldObj);
                heldObj = null;
            }
        }

    }
    void PickUpObject(GameObject held)
    {
        if (held.GetComponent<Rigidbody>()) //make sure the object has a RigidBody
        {
            var rb = held.GetComponent<Rigidbody>();
            rb.isKinematic = true; // disable physics so object will not fall
            rb.transform.position = holdPos.transform.position;
            rb.transform.parent = holdPos.transform; //parent object to holdposition
        }
    }
    void DropObject(GameObject held)
    {
        var rb = held.GetComponent<Rigidbody>();
        rb.isKinematic = false; // re-enable physics to drop the object
        held.transform.parent = null; //unparent object
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("canPickUp"))
        {
            collidedObject = other.gameObject; // remember the gameObject what we collided
            collectPrompt.gameObject.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("canPickUp"))
        {
            collidedObject = null;
            collectPrompt.gameObject.SetActive(false);
        }
    }
}