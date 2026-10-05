using UnityEngine;
using Unity.Netcode;

public class MultiplayerManager : MonoBehaviour
{
    public GameObject enemyPrefab;

    private bool enemySpawned = false;

    void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
    }

    void OnDestroy()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
        }
    }

    void OnClientConnected(ulong clientId)
    {
        // Only the host/server should spawn the Enemy
        if (!NetworkManager.Singleton.IsServer)
            return;

        // Client 0 is the host (you)
        // The first other client is your friend
        if (clientId == NetworkManager.ServerClientId)
            return;

        if (enemySpawned)
            return;

        GameObject enemy = Instantiate(
            enemyPrefab,
            new Vector3(5f, 1f, 5f),
            Quaternion.identity
        );

        NetworkObject networkObject =
            enemy.GetComponent<NetworkObject>();

        networkObject.SpawnWithOwnership(clientId);

        enemySpawned = true;
    }
}