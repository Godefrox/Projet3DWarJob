using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirSensor : MonoBehaviour
{
    Fighter myFighter = null;

    void Start()
    {
        Transform parent = transform.parent;
        if (parent != null)
        {
            GameObject parentObject = parent.gameObject;
            if (parentObject != null)
            {
                Fighter script = parentObject.GetComponent<Fighter>();
                if (script != null)
                {
                    this.myFighter = script;
                }
            }
        }

    }

    

    void OnTriggerEnter(Collider collide)
    {
        if (gameObject.tag != collide.gameObject.tag)
        {
            if (this.myFighter != null)
            {
                this.myFighter.addTarget(collide.gameObject);
            }
        }

    }

    void OnTriggerExit(Collider collide)
    {
        if (gameObject.tag != collide.gameObject.tag)
        {
            if (this.myFighter != null)
            {
                this.myFighter.removeTarget(collide.gameObject);
            }
        }
    }
}
