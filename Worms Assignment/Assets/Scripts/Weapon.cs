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
        //string fire = Fire();
        //Debug.Log("You hit: " + fire); 
    }

    public int DealDamage(){
        return damage;
    }

    public void Fire(){
        RaycastHit hit;
        if (Physics.Raycast(startPoint.transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
            Debug.DrawRay(startPoint.transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            //Debug.Log("Hit "+ hit.collider.name);
            if (hit.collider.CompareTag("Player")){
                Rigidbody targetRB = hit.transform.GetComponent<Rigidbody>();
                hit.transform.GetComponent<Player>().TakeDamage(this.damage);
                //Vector3 pos = startPoint.transform.position;
                //pos.y = targetRB.transform.position.y;
                //targetRB.rotation = Quaternion.LookRotation(pos);
                hit.transform.LookAt(startPoint.transform.position);
                targetRB.AddForce(-targetRB.transform.forward*20, ForceMode.Impulse);
                hit.transform.Rotate(0 ,hit.transform.rotation.y, 0);
                //targetRB.rotation = Quaternion.identity;
            }
        }
        else {
            Debug.DrawRay(startPoint.transform.position, transform.TransformDirection(Vector3.forward)*100, Color.red);
            Debug.Log("Hit nothing");
        }
    }
}
