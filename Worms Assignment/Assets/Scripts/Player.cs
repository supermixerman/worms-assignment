using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Player : MonoBehaviour
{
    [SerializeField] int maxHealth = 100; //Players max health
    private int health; //Players current health.
    [SerializeField] Weapon weaponEquipped; //Which weapon the player should use
    [SerializeField] HealthBar healthBar;
    [SerializeField] CinemachineVirtualCamera aimCam;
    private bool grounded, isDead;
    //PlayerInput playerInput;
    void Awake()
    {
        health = maxHealth;
        //playerInput = GetComponent<PlayerInput>();
        //playerInput.camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        healthBar.SetMaxValue(maxHealth);
    }

    public void SetName(string newName){
        this.name = newName;
    }
    
    public int GetHealth(){ //Returns how much health the player currently has
        return health;
    }
    
    public void TakeDamage(int damage){ //Lowers health depending on the damage value
        this.health -= damage;
        if (this.health <= 0) {
            this.health = 0;
            healthBar.SetValue(health);
            Dead();
        }
        healthBar.SetValue(health);
    }
    
    public void Heal(int amount){ //Heals the player by the amount.
        this.health += amount;
        
        if (health > maxHealth){ //Makes sure health won't go over the max limit
            health = maxHealth;
        }
        healthBar.SetValue(health);
    }

    public bool IsGrounded(){
        return grounded;
    }
    public void EquipWeapon(Weapon newWeapon) {
        weaponEquipped = newWeapon;
    }

    public void Dead(){

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
