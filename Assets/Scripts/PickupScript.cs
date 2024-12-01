using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PickupScript : MonoBehaviour
{
    public TextMeshProUGUI collectPrompt;
    public Transform holdPos;
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
            rb.isKinematic = true;
            rb.transform.position = holdPos.transform.position;
            rb.transform.parent = holdPos.transform; //parent object to holdposition
        }
    }
    void DropObject(GameObject held)
    {
        var rb = held.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        held.transform.parent = null; //unparent object
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("canPickUp"))
        {
            collidedObject = other.gameObject;
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