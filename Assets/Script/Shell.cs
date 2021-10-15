using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour
{
    int damage = 100;
    void OnCollisionEnter(Collision collision)
    {
        bool land = false;
        Turret turret = collision.gameObject.GetComponent<Turret>();
        Tank tank = collision.gameObject.GetComponent<Tank>();
        Transform transformParent = collision.gameObject.transform.parent;
        if (tank != null)
        {
            land = true;
            tank.loseHp(this.damage);
        }
        else if (turret != null)
        {
            land = true;
            turret.loseHp(this.damage);
        }
        else if (transformParent != null)
        {
            GameObject parent = transformParent.gameObject;
            if (parent != null)
            {
                Transform secondTransformParent = parent.transform.parent;
                if(secondTransformParent != null) {
                    GameObject secondParent = secondTransformParent.gameObject;
                    if (secondParent != null)
                    {
                        Fighter fighter = secondParent.GetComponent<Fighter>();
                        if (fighter != null)
                        {
                            fighter.loseHp(Mathf.RoundToInt((this.damage * 1.5f)));
                        }
                    }
                }

            }
        }
        if (land)
        {
            Instantiate(Resources.Load("Models/Shell-Exposion"), transform.position, transform.rotation);
        }
        Destroy(gameObject);
    }

    public void setDamage(int damage)
    {
        this.damage = damage;
    }

    public void setAutoDestruction(int timeSecond)
    {
        StartCoroutine(WaitDestruction(timeSecond));
    }

    private IEnumerator WaitDestruction(int timeSecond)
    {
        yield return new WaitForSeconds(timeSecond);
        Destroy(gameObject);
    }
}
