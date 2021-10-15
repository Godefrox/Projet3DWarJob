using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LandBataillon : MonoBehaviour
{
    GameObject leader;
    List<GameObject> squads;
    GameObject newPosition;
    GameObject formation;
    GameObject firstAgent = null;
    public string role = "Movement";
    bool onMove = false;
    bool firstInit = false;
    int distanceIn = 40;

   
    void Start()
    {
        this.squads = new List<GameObject>();

        GameObject squads = transform.Find("Squads").gameObject;

        Transform formation = transform.Find("Formation");
        Transform movementTransform = null;

        GameObject movement = null;
        this.formation = formation.gameObject;

        if (squads != null)
        {
            for (int i = 0; i < squads.transform.childCount; i++)
            {
                GameObject newSquad = squads.transform.GetChild(i).gameObject;
                this.squads.Add(newSquad);
            }

            if(this.squads.Count > 0)
            {
                LandSquad script = this.squads[0].GetComponent<LandSquad>();
                if(script != null)
                {
                    this.firstAgent = script.getFirstMember();
                }
            }
        }

        if (this.newPosition != null)
        {
            setFormation("Movement");
            Vector3 targetDir = this.newPosition.transform.position - this.formation.transform.position;
            float angle = Vector3.SignedAngle(targetDir, Vector3.forward, -Vector3.up);
            Quaternion target = Quaternion.Euler(0, angle, 0);
            this.formation.transform.rotation = Quaternion.Slerp(transform.rotation, target, Mathf.Infinity);
            this.formation.transform.position = Vector3.MoveTowards(this.formation.transform.position, this.newPosition.transform.position, Mathf.Infinity);
            this.onMove = true;
            this.newPosition = null;
            
        }

        
    }

    
    void Update()
    {
        this.squads.RemoveAll(squad => squad == null);
        
        if(this.squads.Count <= 0)
        {
            if(this.leader != null)
            {
                Army script = this.leader.GetComponent<Army>();
                if(script != null)
                {
                    script.removeMember(gameObject);
                }
            }
            Destroy(gameObject);
        }
        else
        {
            LandSquad script = this.squads[0].GetComponent<LandSquad>();
            if (script != null)
            {
                this.firstAgent = script.getFirstMember();
            }
        }

        if(this.newPosition != null)
        {
            if(this.formation != null)
            {
                /*
                 * Détermine la nouvelle position du bataillon et l'oriente en fonction de la position actuelle permettant aux escouades d'être orientés en fonction de la direction de l'ennemis. 
                 */
                setFormation("Movement");
                if (this.newPosition != this.firstAgent)
                {
                    Vector3 targetDir = this.newPosition.transform.position - this.formation.transform.position;
                    float angle = Vector3.SignedAngle(targetDir, Vector3.forward, -Vector3.up);
                    Quaternion target = Quaternion.Euler(0, angle, 0);
                    this.formation.transform.rotation = Quaternion.Slerp(transform.rotation, target, Mathf.Infinity);
                }
                else
                {
                    this.formation.transform.rotation = this.firstAgent.transform.rotation;
                }
                this.formation.transform.position = Vector3.MoveTowards(this.formation.transform.position, this.newPosition.transform.position, Mathf.Infinity);
                this.onMove = true;
                this.newPosition = null;
            }
        }

        
            bool stop = true;
            this.squads.ForEach(squad =>
            {
                LandSquad script = squad.GetComponent<LandSquad>();
                if (script != null)
                {
                    if (!script.isDestination())
                    {
                        stop = false;
                        
                    }
                }
            });
            this.onMove = !stop;
            if (stop)
            {
                setFormation(this.role);
            }
        
    }

    void setFormation(string mission)
    {
        this.onMove = true;
        Transform missionTransform = null;

        GameObject goal = null;

        if (this.formation != null)
        {
            missionTransform = this.formation.transform.Find(mission);


            if (missionTransform != null)
            {

                goal = missionTransform.gameObject;

            }
        }

        if (this.squads != null)
        {
            int i = 0;
            this.squads.ForEach(squad =>
            {
                if (goal != null)
                {
                    LandSquad script = squad.GetComponent<LandSquad>();

                    if (script != null)
                    {
                        script.setRole(mission);
                        script.setDestination(goal.transform.GetChild(i++).gameObject);
                    }

                }
            });
        }
    }

    void addMember(GameObject newSquad)
    {
        if (!this.squads.Contains(newSquad)){
            this.squads.Add(newSquad);
        }
    }

    public void removeMember(GameObject exSquad)
    {
        if (this.squads.Contains(exSquad)){
            this.squads.Remove(exSquad);
        }
    }

    public void setRole(string role)
    {
        this.role = role;
        this.setFormation(role);
    }

    public void setLeader(GameObject newLeader)
    {
        this.leader = newLeader;
    }

    public void setPosition(GameObject newPosition)
    {
        if (newPosition != null && this.firstAgent != null)
        {
            /*
             * Détermine s'il y a besoin d'un mouvement de la part du bataillon en fonction de l'éléments le plus en avant au sein du bataillon. 
             */
            float distance = Vector3.Distance(newPosition.transform.position, this.firstAgent.transform.position);         
            
                if (Math.Abs(distance) > this.distanceIn)
                {
                    
                    this.distanceIn = 40;
                    this.newPosition = newPosition;
                    this.firstInit = true;
                }
                else
                {
                    if (this.firstInit) {
                        this.distanceIn = 60;
                        this.firstInit = false;
                        this.newPosition = this.firstAgent;
                    }
                }
            
        }
    }
}
