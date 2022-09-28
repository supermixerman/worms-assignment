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
                targetRB.AddForceAtPosition(transform.TransformDirection(Vector3.forward)*400, hit.point);
                //Vector3 lookDirection = startPoint.transform.position - hit.transform.position;
                //targetRB.rotation = Quaternion.Ro(pos, Vector3.up);
                //pos.y = targetRB.transform.position.y;
                //targetRB.rotation = Quaternion.Euler(0, lookDirection.y, 0);
                //hit.transform.LookAt(hit.point);
                //targetRB.AddForce(-targetRB.transform.forward*500, ForceMode.Force);
                //hit.transform.Rotate(0 ,hit.transform.rotation.y, 0);
                //targetRB.rotation = Quaternion.identity;
                Debug.Log("Hit Player");
            }
            else{
                Debug.Log("Hit: " + hit.transform.tag);
            }
        }
        else {
            Debug.DrawRay(startPoint.transform.position, transform.TransformDirection(Vector3.forward)*100, Color.red);
            Debug.Log("Hit nothing");
        }
    }
}
