using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellAir : MonoBehaviour
{
    int damage = 100;
    void OnCollisionEnter(Collision collision)
    {
        bool land = false;
        Turret turret = collision.gameObject.GetComponent<Turret>();
        Tank tank = collision.gameObject.GetComponent<Tank>();
        Transform transformParent = collision.gameObject.transform.parent;
        Debug.Log("FILS DE PUTE");
        if (tank != null)
        {
            Debug.Log("NON");
            land = true;
            tank.loseHp(this.damage);
        }
        else if (turret != null)
        {
            Debug.Log("LA");
            land = true;
            turret.loseHp(this.damage);
        }else if(transformParent != null)
        {
            Debug.Log("CONNARD");
            GameObject parent = transformParent.gameObject;
            if(parent != null)
            {
                Fighter fighter = parent.GetComponent<Fighter>();
                if(fighter != null)
                {
                    Debug.Log("AVION !");
                }
            }
        }
        if (land) { 
        Instantiate(Resources.Load("Models/Shell-Exposion"), transform.position, transform.rotation);
        }
        Destroy(gameObject);
    }

    public void setDamage(int damage)
    {
        this.damage = damage;
    }
}
