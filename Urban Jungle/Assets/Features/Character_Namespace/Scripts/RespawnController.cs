using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.UIElements;

public class RespawnController : MonoBehaviour
{
    [SerializeField] float playerHeight;

    [SerializeField] private Transform player;

    [SerializeField] private CharacterController charControl;

    [SerializeField] private GameObject[] spawnPoint;

    private int currentSpawnPoint = 0;
    
    void Update()
    {
        if (currentSpawnPoint == 0 && player.position.z <= -15)
            currentSpawnPoint = 1;
        if (currentSpawnPoint == 1 && player.position.z <= -102 && player.position.y >= 55)
            currentSpawnPoint = 2;
        if (currentSpawnPoint == 2 && player.position.x <= -22 && player.position.z <= -160)
            currentSpawnPoint = 3;
        if (currentSpawnPoint == 3 && (player.position.x <= -64 || player.position.z <= -195))
            currentSpawnPoint = 4;
        if (currentSpawnPoint == 4 && player.position.z >= -110)
            currentSpawnPoint = 5;
        if (player.position.y >= 0) return;
        charControl.Move(new Vector3(0, 1000,0));
        charControl.Move(spawnPoint[currentSpawnPoint].transform.position - player.position);
        
        //charControl.detectCollisions = false;
        /*
        charControl.Move(new Vector3(0, spawnPoint[0].transform.position.y - (playerPos.position.y+300f), 0));
        charControl.Move(new Vector3(spawnPoint[0].transform.position.x - playerPos.position.x, 0, spawnPoint[0].transform.position.z-playerPos.position.z));
        charControl.Move(new Vector3(0, 300, 0));
        */
        //if (playerPos.position == spawnPoint[0].transform.position)charControl.detectCollisions = true;
    }
    }
