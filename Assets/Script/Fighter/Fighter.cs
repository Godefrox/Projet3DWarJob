using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    public GameObject target;
    GameObject formation;
    //Les différentes positions de départs des obus.
    List<GameObject> cannons;
    List<GameObject> targets;
    float speed = 50;
    float maxSpeed = 50;
    float maxAngle = 53.13f; //arccos[(11.18034² + 11.18034² − 10²) ÷ (2 × 11.18034 × 11.18034)]
    float lowerDrive = 1.2f;
    float angleSpeed = 2f;
    Vector3 movementDirection = new Vector3();
    bool fired = false;
    int onFire = 0;
    int minDistance = 100;
    int minAltitude = 100;
    public int hp = 0;
    Rigidbody preFabShell;
    bool readyFire = true;
    int enemyOnFire = 0;
    bool priority = false;

    
    void Start()
    {
        this.targets = new List<GameObject>();
        this.preFabShell = ((GameObject)Resources.Load("Models/Shell")).GetComponent<Rigidbody>();
        this.cannons = new List<GameObject>();
        Transform cannon = transform.Find("Canons");
        if(cannon != null)
        {
            int numbCanon = cannon.childCount;
            if (numbCanon > 0)
            {
                for(int i = 0; i < numbCanon; i++)
                {
                    Transform childTransform = cannon.GetChild(i);
                    if(childTransform != null)
                    {
                        GameObject childGameObject = childTransform.gameObject;
                        if(childGameObject != null)
                        {
                            this.cannons.Add(childGameObject);
                        }
                    }
                }
            }
            else
            {
                Debug.Log("Missing gameObject canonPlace in " + gameObject.name);
            }
        }
        else
        {
            Debug.Log("Missing canonns on " + gameObject.name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (this.hp <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            if (target != null || priority)
            {
                if (fired)
                {
                    avoidanceManeuver();
                }
                else
                {
                    if (transform.position.y < minAltitude)
                    {
                        moreAltitude();
                    }
                    else
                    {
                        if (!priority)
                        {
                            minAltitude = 100;
                            Vector3 targetDir = target.transform.position - transform.position;
                            float distance = Mathf.Abs(Vector3.Distance(target.transform.position, transform.position));
                            float angle = Vector3.Angle(targetDir, Vector3.forward);
                            if (distance < minDistance && (angle > maxAngle))
                            {
                                if (LayerMask.LayerToName(target.layer) == "Air")
                                {
                                    minDistance = 300;
                                    speed = speed * 0.9f;
                                    avoidanceManeuver();
                                }
                                else
                                {
                                    moveToPosition(this.target);
                                }
                            }
                            else
                            {
                                if (this.readyFire && this.enemyOnFire > 0)
                                {
                                    fireToEnemy();
                                }
                                moveToPosition(this.target);
                            }
                        }
                        else
                        {
                            if (this.formation != null)
                            {
                                moveToPosition(this.formation);
                                float distance = Mathf.Abs(Vector3.Distance(formation.transform.position, transform.position));
                                if (distance < minDistance)
                                {
                                    this.priority = false;
                                }
                            }
                        }

                    }
                }

                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movementDirection), Time.deltaTime * angleSpeed);
                transform.position += transform.forward * (speed * Time.deltaTime);

            }
            else
            {
                this.targets.RemoveAll(target => target == null);
                if (this.targets.Count > 0 && !this.priority)
                {
                    float distance = Vector3.Distance(this.targets[0].transform.position, transform.position);
                    int targetPos = 0;
                    for(int i = 1;i < this.targets.Count; i++)
                    {
                        float anotherDistance = Vector3.Distance(this.targets[i].transform.position, transform.position);
                        if(anotherDistance < distance)
                        {
                            targetPos = i;
                            distance = anotherDistance;
                        }
                    }
                    this.target = this.targets[targetPos];
                }
                else
                {
                    if(this.formation != null)
                    {

                    }
                }
            }
        }
    }

    /*
     * Augmente l'altitude de l'avion pour évités un potentiel crash. Actuellement basés sur la valeur 0, il est prévu qu'un raycast soit utilisés pour détecter le niveau du sol. 
     */
    void moreAltitude()
    {
        movementDirection.x = 0;
        movementDirection.y = 10;
        movementDirection.z = 0;
        angleSpeed = 5f;
        speed = maxSpeed * lowerDrive;
        minAltitude = 250;
    }

    /*
     * L'avion entre en manoeuvre d'évitement pour éviter soit un tir, soit pour s'éloigner de sa cible.
     */
    void avoidanceManeuver()
    {
        movementDirection.x = Mathf.Clamp((movementDirection.x + Random.Range(-2.0f, 2.0f)), -20.0f, 20.0f);
        movementDirection.y = Mathf.Clamp((movementDirection.y + Random.Range(-2.0f, 2.0f)), -20.0f, 20.0f);
        movementDirection.z = Mathf.Clamp((movementDirection.z + Random.Range(-2.0f, 2.0f)), -20.0f, 20.0f);
        angleSpeed = 5f;
        speed = maxSpeed / lowerDrive;
    }

    /*
     * Indique la position dont l'avion doit se mettre en position.
     */
    public void moveToPosition(GameObject target)
    {
        minDistance = 100;
        movementDirection = target.transform.position - transform.position;
        angleSpeed = 2f;
        speed = 50f;
    }

    public void addFire()
    {
        this.onFire += 1;
        this.fired = true;
    }

    public void removeFire()
    {
        this.onFire -= 1;
        if(this.onFire == 0)
        {
            this.fired = false;

        }
        else if(this.onFire < 0)
        {
            this.onFire = 0;
            this.fired = false;
        }
    }

    public void addEnemyOnFire()
    {
        this.enemyOnFire += 1;
    }

    public void removeEnemyOnFire()
    {
        this.enemyOnFire -= 1;
        if (this.enemyOnFire <= 0)
        {
            this.enemyOnFire = 0;
        }
    }

    public void loseHp(int hp)
    {
        this.hp -= hp;
    }

    /*
     * Enclenche des tirs à l'avant des mitrailleuse pour éléminer une cible potentiels, ne tire que l'orsq'un enemis est présent. Ne prends pas en compte le mouvement de celui-ci.
     */
    void fireToEnemy()
    {
        this.cannons.ForEach(canon =>
        {
            Rigidbody p = Instantiate(this.preFabShell, canon.transform.position, canon.transform.rotation);
            p.velocity = (transform.forward * ((4*maxSpeed * lowerDrive) + 10.0f));
            p.useGravity = false;
            p.gameObject.SetActive(true);
            Shell script = p.gameObject.GetComponent<Shell>();
            if (script)
            {
                script.setAutoDestruction(2);
            }
        });
        
        this.readyFire = false;
        StartCoroutine(WaitFire());

    }

   
    private IEnumerator WaitFire()
    {
        yield return new WaitForSeconds(0.3f);
        this.readyFire = true;
    }

    public void addTarget(GameObject target)
    {
        if (this.targets != null)
        {
            if (!this.targets.Contains(target))
            {
                this.targets.Add(target);
            }
        }
    }

    public void removeTarget(GameObject target)
    {
        if (this.targets != null)
        {
            if (this.targets.Contains(target))
            {
                this.targets.Remove(target);
            }
        }

        if(target == this.target)
        {
            this.target = null;
        }
    }

    public void setFormation(GameObject formation)
    {
        this.formation = formation;
    }

    public void onPriority()
    {
        this.priority = true;
    }
}
