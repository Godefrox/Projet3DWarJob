using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirSensorFront : MonoBehaviour
{
    /*
     * Le sensor permettant la d�tection d'un avion ennemis � l'avant. Indique s'il est utile de faire feu. 
     */

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

    Fighter getFighter(Collider collide)
    {
        Fighter script = null;
        Transform onFire = collide.transform;
        if (onFire != null)
        {
            Transform parent = onFire.parent;
            if (parent != null)
            {
                Transform grandParent = parent.parent;
                if (grandParent != null)
                {
                    GameObject grandParentObject = grandParent.gameObject;
                    if (grandParentObject != null)
                    {
                        script = grandParentObject.GetComponent<Fighter>();
                    }
                }
            }
        }
        return script;
    }

    /*
     * Lorsqu'un avion est d�tect� en sortie ou en entr�e, il est indispensable de v�rifier si celui-ci fait partie de la m�me faction.  
     */
    void OnTriggerEnter(Collider collide)
    {
        if (gameObject.tag != collide.gameObject.tag)
        {
            Fighter script = getFighter(collide);
            if (script != null)
            {
                script.addFire();
                this.myFighter.addEnemyOnFire();
            }
        }

    }

    void OnTriggerExit(Collider collide)
    {
        if (gameObject.tag != collide.gameObject.tag)
        {
            Fighter script = getFighter(collide);
            Transform onFire = collide.transform;
            if (script != null)
            {
                script.removeFire();
                this.myFighter.removeEnemyOnFire();
            }
        }
    }
}
