using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace SampleSolution
{
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
            if (IsServer)
            {
                SpawnWitch();
            }
        }

        private void SpawnWitch()
        {
            GameObject witch = Instantiate(witchPrefab, witchSpawnPoint.position, Quaternion.identity);
            witch.GetComponent<NetworkObject>().Spawn();

            // keep this 
            witch.GetComponent<WitchBehaviour>().flightTargets = flightTargets;
        }
    }
}
