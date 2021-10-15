using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    /*
     * La classe turret permet au tank de se protéger et d'attaquer les enemis à distance qui sont visibles. 
     */

    List<GameObject> targets;
    List<GameObject> shootables;

    GameObject shorter;

    Transform canon;

    float range = 0.0f;
    float speedTurret = 1.5f;
    float distance;

    int damageShot = 100;

    bool readyFire = true;
    
    Rigidbody preFabShell;
    Rigidbody shell;

    public bool cons = false;
    public bool printCons = false;

    //SCRIPT
    Tank tank;

    void Start()
    {
        this.targets = new List<GameObject>();
        this.shootables = new List<GameObject>();

        this.preFabShell = ((GameObject)Resources.Load("Models/Shell")).GetComponent<Rigidbody>();
        this.tank = transform.parent.gameObject.GetComponent<Tank>();
        this.canon = transform.Find("Canon");
        
        Transform sensor = transform.Find("Sensor");
        SphereCollider sph = null;
        if (sensor != null) {
            sph = sensor.gameObject.GetComponent<SphereCollider>();
        }
        else
        {
            Debug.Log("Sensor are missing in " + gameObject.name);
        }

        if(sph != null)
        {
            this.range = sph.radius;
            this.damageShot = (int) ((sph.radius / 80) * 100);
        }
        else
        {
            Debug.Log("Sphere Collider are missing in " + gameObject.name);
        }
        
    }

    void Update()
    {
        
        this.targets.RemoveAll(target => target == null);
        this.shootables.RemoveAll(target => target == null);
        
        GameObject nextTarget = null;
        float nextDistance = 81f;
        this.targets.ForEach(target =>
        {
            if (target != null)
            {
                Vector3 lookPos = target.transform.position - transform.position;
                Ray ray = new Ray(transform.position, lookPos);
                RaycastHit hit;
               
                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    if (this.printCons)
                    {
                       
                        Vector3 ano = (hit.collider.gameObject.transform.position - transform.position);
                    }
                    if (hit.collider.tag != "Untagged" && hit.collider.tag != tag && hit.distance <= this.range) {
                       
                        if (hit.distance < nextDistance)
                        {
                            nextDistance = hit.distance;
                            nextTarget = target;
                        }
                        if (!this.shootables.Contains(target))
                        {
                            this.shootables.Add(target);
                        }
                    }
                    else
                    {
                            if (this.shootables.Contains(target))
                            {
                                this.shootables.Remove(target);
                            }
                    }
                }
                else
                {
                   
                    if (this.shootables.Contains(target))
                    {
                        this.shootables.Remove(target);
                    }
                }
            }
        });
        if(nextTarget != null) { 
        this.shorter = nextTarget;
        this.distance = nextDistance;
        }

        if (this.shorter != null)
        {
            Vector3 target = this.shorter.transform.position - transform.position;
            Ray ray = new Ray(transform.position, target);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (this.printCons)
                {

                    Vector3 ano = (hit.collider.gameObject.transform.position - transform.position);
                }
                if (hit.collider.tag != "Untagged" && hit.collider.tag != tag && hit.distance <= this.range)
                {

                    Vector3 lookPos = this.shorter.transform.position - transform.position;
                    lookPos.y = 0;
                    Quaternion rotation = Quaternion.LookRotation(lookPos);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * this.speedTurret);
                    if (this.readyFire == true)
                    {
                        fireToEnemy();
                    }
                }
                else
                {
                    this.shootables.Remove(this.shorter);
                    if (this.shootables.Count > 0)
                    {
                        this.shorter = this.shootables[0];
                    }
                }
            }
            else
            {
                this.shootables.Remove(this.shorter);
                if (this.shootables.Count > 0)
                {
                    this.shorter = this.shootables[0];
                }
            }
        }
    }

    void fireToEnemy()
    {
        if (this.shorter != null)
        {
            if (this.preFabShell != null && this.canon != null)
            {
                Rigidbody p = Instantiate(this.preFabShell, this.canon.position, this.canon.rotation);
                p.velocity = transform.forward * ((Vector3.Distance(transform.position, this.shorter.transform.position) + 10.0f));
                p.gameObject.SetActive(true);
                Shell script = p.gameObject.GetComponent<Shell>();
                if (script)
                {
                    script.setDamage(this.damageShot);
                }
                this.shell = p;
                this.readyFire = false;
                StartCoroutine(WaitFire());
            }
            else
            {
                Debug.Log("preFabShell or canon in Turret.cs are null in " + gameObject.name);
            }

        }
        
    }

    private IEnumerator WaitFire()
    {
        
        yield return new WaitForSeconds(4);
        this.readyFire = true;
        if(this.shell != null)
        {
            Destroy(this.shell.gameObject);
        }
    }

    public void addTarget(GameObject enemy)
    {
        if(enemy.tag != "Untagged" && enemy.layer != LayerMask.NameToLayer("IgnoreCollider") && tag != enemy.tag) {
            if (!this.targets.Contains(enemy)) { 
                this.targets.Add(enemy);
            }
        }
    }

    public void removeTarget(GameObject enemy)
    {
        if (enemy.tag != "Untagged" && enemy.layer != LayerMask.NameToLayer("IgnoreCollider") && tag != enemy.tag)
        {
            if (this.targets.Contains(enemy))
            {
                this.targets.Remove(enemy);
                if (this.shootables.Contains(enemy))
                {
                    this.shootables.Remove(enemy);
                }
            }
        }
    }

    public void loseHp(int hp)
    {
        if(this.tank != null) {
            this.tank.loseHp(hp);
        }
        else
        {
            Debug.Log("Tank in Turret.cs has null in " + gameObject.name);
        }
    }

    public void death()
    {
        if(this.shell != null) { 
        Destroy(this.shell.gameObject);
        }
        Destroy(this);
    }
}
