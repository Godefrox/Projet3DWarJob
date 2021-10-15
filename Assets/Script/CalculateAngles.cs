using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculateAngles : MonoBehaviour
{
    public Transform test1;
    public Transform test2;
    public Transform test3;
    public Transform test4;

    
    void Start()
    {
        StartCoroutine(wait());
    }

    void Update()
    {
       
    }

    IEnumerator wait()
    {
        

        while (true) {
            Vector3 lookPos = test1.position - transform.position;
            lookPos.y = 0;
            Quaternion rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Mathf.Infinity);
            yield return new WaitForSeconds(1);
            lookPos = test2.position - transform.position;
            lookPos.y = 0;
            rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Mathf.Infinity);
            yield return new WaitForSeconds(1);
            lookPos = test3.position - transform.position;
            lookPos.y = 0;
            rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Mathf.Infinity);
            yield return new WaitForSeconds(1);
            lookPos = test4.position - transform.position;
            lookPos.y = 0;
            rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Mathf.Infinity);
            yield return new WaitForSeconds(1);
        }
    }
}
