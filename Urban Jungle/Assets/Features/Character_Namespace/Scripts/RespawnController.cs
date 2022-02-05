using UnityEngine;

public class RespawnController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private CharacterController charControl;
    [SerializeField] private GameObject[] spawnPoint;

    private int _currentSpawnPoint;

    private void Awake()
    {
        _currentSpawnPoint = 0;
    }

    public void Update()
    {
        if (_currentSpawnPoint == 0 && player.position.z <= 54)
            _currentSpawnPoint = 1;
        if (_currentSpawnPoint == 1 && player.position.z <= -15)
            _currentSpawnPoint = 2;
        if (_currentSpawnPoint == 2 && player.position.z <= -102 && player.position.y >= 55)
            _currentSpawnPoint = 3;
        if (_currentSpawnPoint == 3 && player.position.x <= -22 && player.position.z <= -160)
            _currentSpawnPoint = 4;
        if (_currentSpawnPoint == 4 && (player.position.x <= -64 || player.position.z <= -195))
            _currentSpawnPoint = 5;
        if (_currentSpawnPoint == 5 && player.position.z >= -110)
            _currentSpawnPoint = 6;
        
        if (player.position.y >= 0) return;
        
        Respawn();
    }

    public void RespawnTo(int repawnPosition)
    {
        charControl.enabled = false;
        charControl.transform.position = spawnPoint[repawnPosition].transform.position;
        charControl.enabled = true;
    }

    public void Respawn()
    {
        GetComponent<AudioSource>().Play();
        RespawnTo(_currentSpawnPoint);
    }
}
