using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;
//using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;
    [SerializeField] CinemachineVirtualCamera playerCamera;
    [SerializeField] int playerAmount, turn;
    [SerializeField] CharacterController characterController;
    [SerializeField] GameObject gameOverScreen;
    //[SerializeField] VictoryUI victoryUI;
    [SerializeField] List<GameObject> playerList;
    [SerializeField] List<Transform> spawnLocations;
    private string winner;
    bool routineRunning;
    // Start is called before the first frame update
    void Awake()
    {
        SpawnPlayers(playerAmount);
        TurnStart();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(characterController.turnOver&&!routineRunning)
        {
            routineRunning = true;
            StartCoroutine(WaitForNextTurn(1f));
        }
    }

    //Spawns the amount of players in spawn locations set out in the scene.
    public void SpawnPlayers(int amount){
        //int playersSpawned = 0;
        if (amount > spawnLocations.Count) //There can not be more players than the amount of spawn locations.
        {
            Debug.LogError("Player Amount is too large. Add more Spawn Locations or lower the value. Changed player amount to the amount of spawn locations.");
            amount = spawnLocations.Count;
            playerAmount = spawnLocations.Count;
        }
        for (int i = 0; i < amount; i++)
        {
            playerList.Add(Instantiate(playerPrefab, spawnLocations[i].position, Quaternion.identity));
            playerList[i].gameObject.name = "Player " + (i+1);
            Debug.Log(playerList[i].gameObject.name);
        }
    }

    //Sets which player the camera should follow
    public void CameraFollow(Transform target){
        Debug.Log("Camera Target: "+target);
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
        Debug.Log("New Turn");
        WinCheck();
        turn++;
        if (turn >= playerList.Count){
            Debug.Log("Reset turn");
            turn = 0;
        }
        if (playerList[turn].gameObject.GetComponent<Player>().IsDead()){
           Debug.Log("Player "+ playerList[turn] + " inactive.");
           return;
        }
        Debug.Log("Turn num = "+turn);
        //playerList[turn].GetComponent<Player>().IsDead();
        characterController.SetActivePlayer(playerList[turn]);
        CameraFollow(playerList[turn].transform);
        characterController.turnOver = false;
    }

    public void WinCheck(){
        List<GameObject> playersAlive = new List<GameObject>();
        for (int i = 0; i < playerList.Count; i++)
        {
            if (!playerList[i].gameObject.GetComponent<Player>().IsDead()){
                playersAlive.Add(playerList[i]);
            }            
        }
        Debug.Log("Players Alive: " + playersAlive.Count);
        if (playersAlive.Count == 1){
            winner = playersAlive[0].gameObject.name;
            //victoryUI.SetWinText(winner);
            gameOverScreen.SetActive(true);
            gameOverScreen.GetComponent<VictoryUI>().SetWinText(winner);
            Debug.Log("The winner is " + winner);
        }
    }

    IEnumerator WaitForNextTurn(float timer){
        yield return new WaitForSeconds(timer);
        NewTurn();
        routineRunning = false;
        yield return null;
    }

    public void ReloadScene(){
        SceneManager.LoadScene(0, LoadSceneMode.Single);
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