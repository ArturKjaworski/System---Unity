using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RayCast : MonoBehaviour
{
    private GameObject raycastedObj;

    [Header("Raycast Settings")]
    [SerializeField] private float rayLength;
    [SerializeField] private LayerMask newLayerMask;

    [Header("References")]
    [SerializeField] private PlayerVitals playerVitals;
    [SerializeField] private Image crossHair;
    [SerializeField] private Text itemNameText;



    private void Update()
    {
        RaycastHit hit;
        Vector3 fwd = transform.TransformDirection(Vector3.forward);

        if (Physics.Raycast(transform.position, fwd, out hit, rayLength, newLayerMask.value))
        {
            if (hit.collider.CompareTag("Consumable"))
            {
                CrosshairActive();
                raycastedObj = hit.collider.gameObject;

                WorldObject item = raycastedObj.GetComponent<WorldObject>();
                itemNameText.text = item.name;

                //PickUp
                if (Input.GetMouseButtonDown(0))
                {
                    item.Interaction();
                }
            }
        }
    else
        {
            CrosshairNormal();
            itemNameText.text = null;
            //item name back to normal
        }
    }

    void CrosshairActive()
    {
        if (crossHair.color != Color.red)
        crossHair.color = Color.red;
    }

    void CrosshairNormal()
    {
        if (crossHair.color != Color.white)
            crossHair.color = Color.white;
    }
}
