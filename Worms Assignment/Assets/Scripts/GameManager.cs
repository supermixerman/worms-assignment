using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab, continueScreen, gameCanvas, menuCanvas;
    [SerializeField] CinemachineVirtualCamera playerCamera;
    [SerializeField] int playerAmount, turn;
    [SerializeField] CharacterController characterController;
    [SerializeField] AudioManager audioManager;
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] RoundTimer roundTimer;
    [SerializeField] Text turnText;
    //[SerializeField] VictoryUI victoryUI;
    [SerializeField] List<GameObject> playerList;
    [SerializeField] List<Transform> spawnLocations;
    [SerializeField] List<Color> playerColorsList;
    private string winner;
    bool routineRunning;
    public static GameManager gameManager;

    // Start is called before the first frame update
    void Awake()
    {
        if (gameManager == null){
            gameManager = this;
        }
        else {
            Destroy(this);
        }
        menuCanvas.SetActive(true);
        //SpawnPlayers(playerAmount);
        //TurnStart();
    }

    void LateUpdate()
    {
        if(menuCanvas.activeInHierarchy) return;
        if(roundTimer.GetTimer()<=0&&!routineRunning&&roundTimer.runTimer || playerList[turn].GetComponent<Player>().IsDead()&&!routineRunning)
        {
            characterController.turnOver = true; //Just to make sure to disable player controls if the turn ends by timer.
            routineRunning = true;
            StartCoroutine(WaitForNextTurn(1f));
        }
    }

    public void StartGame(int players){
        playerAmount = players;
        SpawnPlayers(playerAmount);
        TurnStart();
        menuCanvas.SetActive(false);
        gameCanvas.SetActive(true);

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
            if (playerColorsList.Count >= playerList.Count){
                playerList[i].GetComponent<Player>().SetPlayerColor(playerColorsList[i]);
            }
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
        turnText.text = "Turn: "+playerList[0].name;
        characterController.turnOver = true;
        continueScreen.SetActive(true);
    }

    public void NewTurn(){
        Debug.Log("New Turn");
        turn++;
        if (turn >= playerList.Count){
            Debug.Log("Reset turn");
            turn = 0;
        }
        while(playerList[turn].gameObject.GetComponent<Player>().IsDead()){
            Debug.Log("Player "+ playerList[turn] + " inactive.");
            turn++;
            if (turn >= playerList.Count){
                Debug.Log("Reset turn");
                turn = 0;
            }
        }
        /*if (playerList[turn].gameObject.GetComponent<Player>().IsDead()){
           Debug.Log("Player "+ playerList[turn] + " inactive.");
        }*/
        Debug.Log("Turn num = "+turn);
        characterController.SetActivePlayer(playerList[turn]);
        CameraFollow(playerList[turn].transform);
        turnText.text = "Turn: "+playerList[turn].name;
        continueScreen.SetActive(true);
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
            roundTimer.StopTimer();
            gameOverScreen.SetActive(true);
            turnText.gameObject.SetActive(false);
            roundTimer.gameObject.SetActive(false);
            gameOverScreen.GetComponent<VictoryUI>().SetWinText(winner);
            Debug.Log("The winner is " + winner);
        }
    }

    public IEnumerator WaitForNextTurn(float timer){
        roundTimer.StopTimer();
        Debug.Log("Corutine Started");
        yield return new WaitForSeconds(timer);
        WinCheck();
        if (winner == null){
            NewTurn();
            routineRunning = false;
        }
        Debug.Log("Corutine Ended");
        yield return null;
    }

    public void ReloadScene(){
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    public void ResumeTurn(){
        continueScreen.SetActive(false);
        roundTimer.StartTimer();
    }

    public void QuitGame(){
        Application.Quit();
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