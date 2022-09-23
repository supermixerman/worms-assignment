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
    private bool grounded, isDead = false;
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

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "floor"){
            grounded = true;
        }
    }

    private void OnCollisionExit(Collision other) {
        if (other.gameObject.tag == "floor"){
            grounded = false;
        }
    }
}
