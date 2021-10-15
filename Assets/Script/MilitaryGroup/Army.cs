using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Army : MonoBehaviour
{
    /*
     * Listes des bataillons sous les ordres de l'arm�e et ceux d�finis comme enemis.
     * Attention, une arm�e est d�fini par un gameObject enfant  "Bataillons".
     */
    List<GameObject> bataillons;
    List<GameObject> bataillonsEnemy;

    /*
     * La faction de l'arm�e d�finis par le Tag.
     */
    string faction;

    
    void Start()
    {
        /*
         * D�finition des valeurs par d�faut. 
         */
        this.bataillons = new List<GameObject>();
        this.bataillonsEnemy = new List<GameObject>();

        GameObject bataillons = transform.Find("Bataillons").gameObject;

        
        if (bataillons != null)
        {
            for (int i = 0; i < bataillons.transform.childCount; i++)
            {
                GameObject newBataillon = bataillons.transform.GetChild(i).gameObject;
                this.bataillons.Add(newBataillon);
            }
        }

        this.faction = gameObject.tag;
        gameObject.name = this.faction;
        this.findBataillonsEnemy();
    }

    void Update()
    {
        /*
         * Netoyage des listes avant leurs utilisations, indispensable car on ne d�sire pas d'�l�ments null lors de nos appels. 
         */
        this.bataillons.RemoveAll(bataillon => bataillon == null);
        this.bataillonsEnemy.RemoveAll(bataillon => bataillon == null);
        /*
         * Une arm�e est d�truite lors de la perte de tous ces bataillons. 
         */
        if (this.bataillons.Count <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            int nbAlly = this.bataillons.Count;
            int nbEnemy = this.bataillonsEnemy.Count;
            int pos = nbEnemy-1;
            if (nbEnemy > 0)
            {
                /*
                 * D�finis une cible pour chaque bataillons 
                 */
                for (int i = 0; i < nbAlly; i++)
                {
                    LandBataillon script = this.bataillons[i].GetComponent<LandBataillon>();
                    if (script != null)
                    {
                        Transform squads = this.bataillonsEnemy[pos--].transform.Find("Squads");
                        if (squads != null)
                        {
                            if (squads.childCount > 0)
                            {
                                Transform squadron = squads.GetChild(0);
                                if (squadron != null)
                                {
                                    Transform members = squadron.Find("Members");
                                    if (members != null)
                                    {
                                        if (members.childCount > 1)
                                        {
                                            Transform child = members.GetChild(0);
                                            if (child != null)
                                            {
                                                GameObject target = child.gameObject;
                                                if (target != null)
                                                {
                                                    script.setPosition(target);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                    }
                    if (pos < 0)
                    {
                        pos = nbEnemy - 1;
                    }
                }
            }
        }
    }

    /*
     * Supprime le bataillon de la liste dans le cas ou celui-ci est r�af�ct� ou �l�min�. 
     */
    public void removeMember(GameObject exBataillon)
    {
        if (this.bataillons.Contains(exBataillon))
        {
            this.bataillons.Remove(exBataillon);
        }
    }

    /*
     * Recherche parmis toutes les arm�es sur la sc�ne pour identifier les bataillons enemis.
     */
    void findBataillonsEnemy()
    {
        List<GameObject> factions = new List<GameObject>();
        Transform[] objs = Resources.FindObjectsOfTypeAll<Transform>() as Transform[];
        for (int i = 0; i < objs.Length; i++)
        {
            if (objs[i].hideFlags == HideFlags.None)
            {
                if (objs[i].gameObject.layer == gameObject.layer && objs[i].gameObject != gameObject)
                {
                    factions.Add( objs[i].gameObject);
                }
            }
        }
        factions.ForEach(faction =>
        {
            GameObject bataillons = faction.transform.Find("Bataillons").gameObject;


            if (bataillons != null)
            {
                for (int i = 0; i < bataillons.transform.childCount; i++)
                {
                    GameObject newBataillon = bataillons.transform.GetChild(i).gameObject;
                    this.bataillonsEnemy.Add(newBataillon);
                }
            }

        });

    }
}
