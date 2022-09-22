using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;
    [SerializeField] CinemachineVirtualCamera playerCamera;
    [SerializeField] List<Transform> spawnLocations;
    [SerializeField] int playerAmount;
    [SerializeField] List<GameObject> playerList;
    // Start is called before the first frame update
    void Awake()
    {
        SpawnPlayers(playerAmount);
        CameraFollow(playerList[0].transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnPlayers(int amount){
        if (amount > spawnLocations.Count)
        {
            Debug.LogError("Player Amount is too large. Add more Spawn Locations or lower the value.");
            return;
        }
        for (int i = 0; i < amount; i++)
        {
            //Instantiate(playerPrefab, spawnLocations[i].position, Quaternion.identity);
            playerList.Add(Instantiate(playerPrefab, spawnLocations[i].position, Quaternion.identity));
        }
    }

    public void CameraFollow(Transform target){
        playerCamera.Follow = target;
        playerCamera.LookAt = target;
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