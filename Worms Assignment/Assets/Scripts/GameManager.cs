using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;
    [SerializeField] CinemachineVirtualCamera playerCamera;
    [SerializeField] List<Transform> spawnLocations;
    [SerializeField] int playerAmount, turn;
    [SerializeField] List<GameObject> playerList;
    [SerializeField] CharacterController characterController;
    [SerializeField] GameObject gameOverScreen;
    private string winner;
    // Start is called before the first frame update
    void Awake()
    {
        SpawnPlayers(playerAmount);
        TurnStart();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Spawns the amount of players in spawn locations set out in the scene.
    public void SpawnPlayers(int amount){
        if (amount > spawnLocations.Count) //There can not be more players than the amount of spawn locations.
        {
            Debug.LogError("Player Amount is too large. Add more Spawn Locations or lower the value. Changed player amount to the amount of spawn locations.");
            amount = spawnLocations.Count;
            playerAmount = spawnLocations.Count;
        }
        for (int i = 0; i < amount; i++)
        {
            playerList.Add(Instantiate(playerPrefab, spawnLocations[i].position, Quaternion.identity));
        }
    }

    //Sets which player the camera should follow
    public void CameraFollow(Transform target){
        playerCamera.Follow = target;
        playerCamera.LookAt = target;
    }

    //Initiates the first turn.
    public void TurnStart(){
        turn = 0;
        characterController.SetActivePlayer(playerList[0]);
        CameraFollow(playerList[0].transform);
    }

    public void NewTurn(){
        turn++;
        if (turn > playerList.Count){
            turn = 0;
        }
        if (!playerList[turn].activeInHierarchy){
           WinCheck();
        }
        //playerList[turn].GetComponent<Player>().IsDead();
        characterController.SetActivePlayer(playerList[turn]);
        CameraFollow(playerList[turn].transform);
    }

    public void WinCheck(){
        List<GameObject> playersAlive = new List<GameObject>();
        for (int i = 0; i < playerList.Count; i++)
        {
            if (playerList[i].activeInHierarchy){
                playersAlive.Add(playerList[i]);
            }            
        }
        if (playersAlive.Count == 1){
            winner = playersAlive[0].gameObject.name;
            gameOverScreen.SetActive(true);
            Debug.Log("The winner is " + winner);
        }
    }
}

/*public class Multiplayer
{
    [SerializeField] GameObject player;
    [SerializeField] Transform spawnLocation;

    public void SetPlayer(GameObject setObject){
        player = setObject;
    }

    public void SetSpawnLocation(Transform newLocation){
        spawnLocation = newLocation;
    }
}*/