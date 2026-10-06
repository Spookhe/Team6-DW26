using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [System.Serializable]
    public struct SpawnableObject
    {
        public GameObject prefab;                 // Cacti assigned to the prefab
        [Range(0f, 1f)] public float chanceToSpawn; // Spawn chances for each obstacle
    }

    // Minimum and maximum time between each obstacle spawn
    public SpawnableObject[] objects;
    public float minSpawnRate = 1f;
    public float maxSpawnRate = 2f;

    private void OnEnable()
    {
        ScheduleNextSpawn(); // Spawns obstacles when enabled
    }

    private void OnDisable()
    {
        CancelInvoke(); // Stops spawning obstacles
    }

    private void Spawn()
    {
        // Randomly pick from range to spawn obstacles
        int index = Random.Range(0, objects.Length);
        Instantiate(objects[index].prefab, transform.position, Quaternion.identity);

        ScheduleNextSpawn();
    }

    // Repeats spawning from range
    private void ScheduleNextSpawn()
    {
        float delay = Random.Range(minSpawnRate, maxSpawnRate);
        Invoke(nameof(Spawn), delay);
    }
}
