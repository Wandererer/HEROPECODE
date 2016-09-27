using System.Collections.Generic;
using UnityEngine;

public struct ObstacleData
{
    public string prefabName;
    public int hp;
    public float spawnTime;
    public int sectorX;
    public int sectorY;

    public ObstacleData(string prefabName, int hp, float spawnTime, int sectorX, int sectorY)
    {
        this.prefabName = prefabName;
        this.hp = hp;
        this.spawnTime = spawnTime;
        this.sectorX = sectorX;
        this.sectorY = sectorY;
    }
}

public enum SpawnState
{
    Start,
    Stop,
}

public class ObstacleSpawnManager
{
    private static readonly float Obstacle_Spawn_Min_Rate = 0.3f;
    private static readonly float Obstacle_Spawn_Max_Rate = 0.6f;
    private static readonly float Obstacle_Spawn_Increase_Rate = 0.08f;
    private static readonly float Obstacle_Spawn_Interval = 4.0f;

    public Game game
    {
        get { return Game.Instance; }
    }

    private Queue<List<ObstacleData>> spawnQueue = new Queue<List<ObstacleData>>();
    private List<ObstacleData> currentSpawnObstacleDatas = new List<ObstacleData>();
    private float spawnUpdateTime;
    public float obstacleSpawnDeltaTime;
    private int obstacleSpawnLevel;

    private SpawnState spawnState;

    public void AddObstacleData()
    {

    }

	private string[] prefabNames = new []{"Squirrel", "Sun", "Moon", "Raindrop", "Planet", "Jesus",
		"Dung", /*"Dragon",*/ "Ailien", "Angel", "Bat", "Bird", "Buddha", "Dove", "Rocket", "Thunder"};

    public string GetRandomPrefabName()
    {
        int index = Random.Range(0, prefabNames.Length);
        return prefabNames[index];
    }

    public void Init()
    {
        List<ObstacleData> list = new List<ObstacleData>();

        SafelyAddList(list, new ObstacleData(GetRandomPrefabName(), 1, 0, Random.Range(-2, 3), Random.Range(0, 2)));
        SafelyAddList(list, new ObstacleData(GetRandomPrefabName(), 1, 0, Random.Range(-2, 3), Random.Range(0, 2)));

        spawnQueue.Enqueue(list);

        list = new List<ObstacleData>();

        SafelyAddList(list, new ObstacleData(GetRandomPrefabName(), 1, 0, Random.Range(-2, 3), Random.Range(0, 2)));
        SafelyAddList(list, new ObstacleData(GetRandomPrefabName(), 1, 0, Random.Range(-2, 3), Random.Range(0, 2)));
        SafelyAddList(list, new ObstacleData(GetRandomPrefabName(), 1, 0, Random.Range(-2, 3), Random.Range(0, 2)));

        spawnQueue.Enqueue(list);

        list = new List<ObstacleData>();

        SafelyAddList(list, new ObstacleData(GetRandomPrefabName(), 1, 0, Random.Range(-2, 3), Random.Range(0, 2)));
        SafelyAddList(list, new ObstacleData(GetRandomPrefabName(), 1, 0, Random.Range(-2, 3), Random.Range(0, 2)));
        SafelyAddList(list, new ObstacleData(GetRandomPrefabName(), 1, 0, Random.Range(-2, 3), Random.Range(0, 2)));
        SafelyAddList(list, new ObstacleData(GetRandomPrefabName(), 1, 0, Random.Range(-2, 3), Random.Range(0, 2)));

        spawnQueue.Enqueue(list);
/*
        list = new List<ObstacleData>();

        SafelyAddList(list, new ObstacleData("Dung", 1, 0, Random.Range(-2, 6), Random.Range(0, 4)));
        SafelyAddList(list, new ObstacleData("Dung", 1, 0.1f, Random.Range(-2, 6), Random.Range(0, 4)));
        SafelyAddList(list, new ObstacleData("Dung", 1, 0.2f, Random.Range(-2, 6), Random.Range(0, 4)));

        spawnQueue.Enqueue(list);
*/
    }

    private void SafelyAddList(List<ObstacleData> list, ObstacleData data)
    {
        if (list.FindIndex(x => x.sectorX == data.sectorX && x.sectorY == data.sectorY) == -1)
        {
            list.Add(data);
        }
    }

    public void UpdateSpawnInterval()
    {
        obstacleSpawnDeltaTime += Time.deltaTime;

        if(IsEnableSpawnTime())
        {
            Debug.Log("obstacleSpawnLevel : " + obstacleSpawnLevel);
            obstacleSpawnDeltaTime = 0;
            float rate = Obstacle_Spawn_Min_Rate + Obstacle_Spawn_Increase_Rate * obstacleSpawnLevel;
            rate = Mathf.Min(rate, Obstacle_Spawn_Max_Rate);

            bool result = Random.Range (0, 1.0f) < rate;

            if (result)
            {
                game.obstacleSpawnManager.SpawnObstacles ();
            }

            obstacleSpawnLevel++;
        }
    }

    public void UpdateObstacles()
    {
        spawnUpdateTime += Time.deltaTime;
        //Debug.Log (currentSpawnObstacleDatas.Count + "개수 ");
        for (int i = 0; i < currentSpawnObstacleDatas.Count;)
        {
            ObstacleData data = currentSpawnObstacleDatas[i];

            if (spawnUpdateTime >= data.spawnTime)
            {
                Vector3 position = new Vector3(data.sectorX * 2, data.sectorY * 2, -1);
				GameObject obj= GameObject.Instantiate(Resources.Load("Prefabs/Obstacles/" + data.prefabName), position,
                        Quaternion.identity) as GameObject;

                if (obj != null)
                {
                    Obstacle obstacle = obj.GetComponent<Obstacle>();
                    obstacle.Init(obstacleSpawnLevel);
                    game.gameScene.AddObstacle(obstacle);
                }

                currentSpawnObstacleDatas.RemoveAt(i);
            }
            else
            {
                i++;
            }
        }

        if (currentSpawnObstacleDatas.Count == 0)
        {
            spawnState = SpawnState.Stop;
        }
    }

    bool IsEnableSpawnTime()
    {
        if(obstacleSpawnDeltaTime >= Obstacle_Spawn_Interval)
        {
            return true;
        }

        return false;
    }

    public void SpawnObstacles()
    {
        List<ObstacleData> obstacleDatas = spawnQueue.Dequeue();
        currentSpawnObstacleDatas.AddRange(obstacleDatas);
        spawnQueue.Enqueue(obstacleDatas);

        spawnUpdateTime = 0;
        spawnState = SpawnState.Start;
    }
}