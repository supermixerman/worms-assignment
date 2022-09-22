using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] int damage = 10;
    [SerializeField] GameObject startPoint;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate() {
        string fire = Fire();
        Debug.Log("You hit: " + fire); 
    }

    public int DealDamage(){
        return damage;
    }

    public string Fire(){
        RaycastHit hit;
        if (Physics.Raycast(startPoint.transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
            Debug.DrawRay(startPoint.transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            //Debug.Log("Hit "+ hit.collider.name);
            return hit.collider.name;
        }
        else {
            Debug.DrawRay(startPoint.transform.position, transform.TransformDirection(Vector3.forward)*100, Color.red);
            return "None";
        }
    }
}
