using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Player : MonoBehaviour
{
    [SerializeField] int maxHealth = 100; //Players max health
    private int health; //Players current health.
    [SerializeField] GameObject weaponEquipped; //Which weapon the player should use
    [SerializeField] HealthBar healthBar;
    [SerializeField] CinemachineVirtualCamera aimCam;
    [SerializeField] private bool grounded, isDead = false;
    private Animator anim;
    //PlayerInput playerInput;
    void Awake()
    {
        health = maxHealth;
        //playerInput = GetComponent<PlayerInput>();
        //playerInput.camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        healthBar.SetMaxValue(maxHealth);
        anim = this.GetComponent<Animator>();
    }

    private void Update() {
        Debug.DrawRay(transform.position, transform.forward*100, Color.red);
    }

    public void SetName(string newName){
        this.name = newName;
    }
    
    public int GetHealth(){ //Returns how much health the player currently has
        return health;
    }
    
    public void TakeDamage(int damage){ //Lowers health depending on the damage value
        if(isDead){return;}

        this.health -= damage;
        if (this.health <= 0) {
            isDead = true;
            this.health = 0;
            healthBar.SetValue(health);
            StartCoroutine("Dead");
            return;
        }
        anim.SetTrigger("Damage");
        anim.SetInteger("DamageType", 1);
        healthBar.SetValue(health);
        Debug.Log("Player health: "+health);
    }
    
    public void Heal(int amount){ //Heals the player by the amount.
        this.health += amount;
        
        if (health > maxHealth){ //Makes sure health won't go over the max limit
            health = maxHealth;
        }
        healthBar.SetValue(health);
    }

    public GameObject GetWeapon(){
        return weaponEquipped;
    }
    public void GetWeaponFire(){
        weaponEquipped.GetComponent<Weapon>().Fire();
    }

    public bool IsGrounded(){
        return grounded;
    }

    public bool IsDead(){
        return isDead;
    }
    public void EquipWeapon(GameObject newWeapon) {
        weaponEquipped = newWeapon;
    }

    IEnumerator Dead(){
        Debug.Log("Player Dies");
        anim.SetInteger("DamageType", 2);
        anim.SetTrigger("Damage");
        yield return new WaitForSeconds(2f);
        this.gameObject.SetActive(false);
        yield return null;
    }

    public void ActivateAimCamera() {
        aimCam.Priority = 2;
    }

    public void DeactivateAimCamera() {
        aimCam.Priority = 0;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("floor") || other.gameObject.CompareTag("Player")){
                grounded = true;
                Debug.Log("Touch Grass " + other.gameObject.name);
        }
        /*
        if (other.gameObject.CompareTag("wall")){
            Vector3 direction = other.contacts[0].point - transform.position;
            direction = -direction.normalized;
            gameObject.GetComponent<Rigidbody>().AddForce(direction*200f);
        }*/
    }

    private void OnTriggerStay(Collider other) {
        if (other.gameObject.CompareTag("floor") || other.gameObject.CompareTag("Player")){
                grounded = true;
                //Debug.Log("Touch Grass " + other.gameObject.name);
        }
    }

    private void OnTriggerExit(Collider other) {
        Debug.Log("Collider Exited: " + other.gameObject.name);
        if (other.gameObject.tag == "floor" || other.gameObject.tag == "Player"){
            grounded = false;
        }
    }
}
