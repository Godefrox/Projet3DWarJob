using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour
{
    Turret turret;

    void Start()
    {
        this.turret = transform.parent.gameObject.GetComponent<Turret>();
    }
    
    void OnTriggerEnter(Collider collision)
    {
        if(turret != null)
        {
            GameObject colliObject = collision.gameObject;
            if (colliObject != null && LayerMask.LayerToName(colliObject.layer) == "Land")
            {
                turret.addTarget(colliObject);
            }
        }
    }

    void OnTriggerExit(Collider collision)
    {
        if (turret != null)
        {
            GameObject colliObject = collision.gameObject;
            if (colliObject != null && LayerMask.LayerToName(colliObject.layer) == "Land")
            {
                turret.removeTarget(colliObject);
            }
        }
    }
}