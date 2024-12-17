using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class WitchSpawner : NetworkBehaviour
{
    public GameObject witchPrefab;
    public Transform witchSpawnPoint;

    public List<Transform> flightTargets;

    private void Awake()
    {
        
    }

    private void Start()
    {
        
    }

    public override void OnNetworkSpawn()
    {
        if(IsServer){ //check for spawning responsible instance
            SpawnWitch();
        }
    }

    private void SpawnWitch()
    {
        // Implement the spawning behaviour here
         

        // Spawn the witchPrefab at the position and with the rotation of whitchSpawnPoint in SpawnWitch()
        GameObject witchInstance = Instantiate(witchPrefab, witchSpawnPoint.transform.position, witchSpawnPoint.transform.rotation);
        var witchInstanceNetworkObject = witchInstance.GetComponent<NetworkObject>();
        witchInstanceNetworkObject.Spawn();
            
        // keep this line in code, it sets the flight targets for task 2.9
        witchInstanceNetworkObject.GetComponent<WitchBehaviour>().flightTargets = flightTargets;
    }
}
