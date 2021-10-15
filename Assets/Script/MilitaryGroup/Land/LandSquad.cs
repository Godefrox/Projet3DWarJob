using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandSquad : MonoBehaviour
{
    GameObject leader;
    List<GameObject> members;
    GameObject newPosition;
    string role = "Attack";
    GameObject formation;
    int onMoveMember = 0;
    bool inPosition = false;

    void Start()
    {
        this.members = new List<GameObject>();

        GameObject members = transform.Find("Members").gameObject;

        Transform formation = transform.Find("Formation");
        Transform movementTransform = null;

        GameObject movement = null;
        this.formation = formation.gameObject;

        if (members != null)
        {
            for (int i = 0; i < members.transform.childCount; i++)
            {
                GameObject newMember = members.transform.GetChild(i).gameObject;
                this.members.Add(newMember);
                newMember.GetComponent<Tank>().setLeader(this);
            }
        }

        if (this.newPosition != null)
        {
            setFormation("Movement");
            this.inPosition = false;
            this.formation.transform.rotation = this.newPosition.transform.rotation;
            this.formation.transform.position = Vector3.MoveTowards(this.formation.transform.position, this.newPosition.transform.position, Mathf.Infinity);
            this.newPosition = null;
        }
    }

    
    void Update()
    {
        /*
        * Nettoyage des listes avant leurs utilisations, indispensable, car on ne désire pas d'éléments null lors de nos appels. 
        */
        this.members.RemoveAll(member => member == null);
        if (this.members.Count <= 0)
        {
            if (this.leader != null)
            {
                LandBataillon script = this.leader.GetComponent<LandBataillon>();
                if (script != null)
                {
                    script.removeMember(gameObject);
                }
            }
            Destroy(gameObject);
        }

        if (this.newPosition != null)
        {
            if (this.formation != null)
            {

                setFormation("Movement");
                this.inPosition = false;
                this.formation.transform.rotation = this.newPosition.transform.rotation;
                this.formation.transform.position = Vector3.MoveTowards(this.formation.transform.position, this.newPosition.transform.position, Mathf.Infinity);
                this.newPosition = null;
            }
        }



        if (this.onMoveMember == 0 && !this.inPosition)
        {

            this.inPosition = true;
            setFormation(this.role);
        }

    }

    /*
     * Établis la formation à adopter par les membres de l'escouade. 
     */
    void setFormation(string mission)
    {
        this.members.RemoveAll(member => member == null);
        this.onMoveMember = this.members.Count;
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

        if (this.members != null)
        {
            int i = 0;
            this.members.ForEach(member =>
            {
                if (goal != null)
                {
                    if (member != null)
                    {
                        Tank script = member.GetComponent<Tank>();

                        if (script != null)
                        {
                            if(mission == "Movement")
                            {
                                script.needReturn();
                            }
                            script.setTarget(goal.transform.GetChild(i++).gameObject);
                        }
                    }

                }
            });
        }
    }

    void addMember(GameObject newMember)
    {
        if (!this.members.Contains(newMember))
        {
            this.members.Add(newMember);
        }
    }

    void removeMember(GameObject newMember)
    {
        if (this.members.Contains(newMember))
        {
            this.members.Remove(newMember);
        }
    }

    public void lessMemberMovement()
    {
        this.onMoveMember--;
        if(this.onMoveMember < 0)
        {
            this.onMoveMember = 0;
        }
    }

    public GameObject getFirstMember()
    {
        GameObject member = null;
        if (this.members.Count > 0)
        {
            member = this.members[0];
        }
        return member;
    }

    public void setRole(string role)
    {
        this.role = role;
        this.setFormation(role);
        this.inPosition = false;
    }

    public void setDestination(GameObject destination)
    {
        this.newPosition = destination;
    }

    public bool isDestination()
    {
        
        return this.onMoveMember == 0;
    }

    public void setLeader(GameObject newLeader)
    {
        this.leader = newLeader;
    }
}
