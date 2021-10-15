using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class Tank : MonoBehaviour
{
    NavMeshAgent agent;
    GameObject target;
    public int hp = 0;
    bool waitReturn = false;
    LandSquad leader;


    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    void Update()
    {
        if (waitReturn)
        {
           isDestination();
        }
        if (target != null)
        {
            agent.SetDestination(target.transform.position);
            
        }
        if(hp <= 0)
        {
            Turret turret = gameObject.GetComponentInChildren<Turret>();
            if(turret != null)
            {
                turret.death();
            }
            if (waitReturn)
            {
                leader.lessMemberMovement();
            }
            Destroy(gameObject);
        }
    }

    /*
     * permet d'obtenir la cible en cours du tank. 
    */
    public GameObject getTarget()
    {
        return this.target;
    }

    /*
     * Permet de définir la cible actuelle du tank. 
    */
    public void setTarget(GameObject o)
    {
        this.target = o;
    }

    /*
     * Définis si le leader attends un retour sur la position actuelle du tank. Principe de l'observateur, le chef d'escouade préviens qu'il attends un retour et dès que les conditinos sont réunis, le tank lui fournis.
     */
    public void needReturn()
    {
        this.waitReturn = true;
    }


    public void loseHp(int hp)
    {
        this.hp -= hp;
    }

    /*
    * Vérifie s'il le tank est arrivé à destination.
    */
    public bool isDestination()
    {
        bool destinationReach = false;
        if (agent != null && target != null && gameObject != null)
        {
            try { 
            destinationReach = agent.remainingDistance <= agent.stoppingDistance;
            }
            catch (Exception e)
            {
                destinationReach = false;
            }
        }
        if (destinationReach && leader != null)
        {
            leader.lessMemberMovement();
        }
        return destinationReach;
    }

    public void setLeader(LandSquad script)
    {
        this.leader = script;
    }
}
