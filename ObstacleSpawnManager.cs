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
 
	private static readonly float Obstalce_Spawn_Normal_Time=4.0f;
	private static  float Obstalce_Spawn_Normal_Min_Time=0f;
	private static  float Obstalce_Spawn_Normal_Max_Time=3f;
	private static readonly float Obstalce_Spawn_Minus_Time= -0.08f;
	private static readonly float Obstalce_Spawn_Limit_Min_Time = -2f;
	private static readonly float Obstalce_Spawn_Limit_Max_Time = 0.8f;

	public Game game
	{
		get { return Game.Instance; }
	}
   
	private MonsterPatternDB monsterDB;
		
    private List<ObstacleData> currentSpawnObstacleDatas = new List<ObstacleData>();
    private float spawnUpdateTime;
    public float obstacleSpawnDeltaTime; //초 당 시 간 초 증 가 
  //  private int obstacleSpawnLevel;

    public SpawnState spawnState;

    public void AddObstacleData()
    {

    }
		

    public void Init()
    {
		monsterDB=new MonsterPatternDB();
		monsterDB.Init();
    }
/*
	private static readonly float Obstalce_Spawn_Normal_Time=4.0f;
	private static  float Obstalce_Spawn_Normal_Min_Time=0f;
	private static  float Obstalce_Spawn_Normal_Max_Time=3f;
	private static readonly float Obstalce_Spawn_Minus_Time=0.08f;
	private static readonly float Obstalce_Spawn_Limit_Min_Time = -2f;
	private static readonly float Obstalce_Spawn_Limit_Max_Time = 0.8f;
*/
	public float PickNormalTime()
	{
		//몬 스 터 나 오 는 시 간 리 턴 
		//4초 + 공 식 에 의 한 초 
		return Obstalce_Spawn_Normal_Time+Random.Range (Obstalce_Spawn_Normal_Min_Time, Obstalce_Spawn_Normal_Max_Time);
	}

	public void SetNormalTimeForNextSpawn()
	{
		//다 음 normal min,max time 을 깍고 제 한 된 크 기 만 큼 까 지 만 깍 이 도 록 함 
		//최 소 시 간 설 정 
		if ((Obstalce_Spawn_Normal_Min_Time + Obstalce_Spawn_Minus_Time) > Obstalce_Spawn_Limit_Min_Time)
			Obstalce_Spawn_Normal_Min_Time += Obstalce_Spawn_Minus_Time;
		else
			Obstalce_Spawn_Normal_Min_Time = Obstalce_Spawn_Limit_Min_Time;

		//최 대 시 간 설 정 
		if ((Obstalce_Spawn_Normal_Max_Time + Obstalce_Spawn_Minus_Time) > Obstalce_Spawn_Limit_Max_Time)
			Obstalce_Spawn_Normal_Max_Time += Obstalce_Spawn_Minus_Time;
		else
			Obstalce_Spawn_Normal_Max_Time = Obstalce_Spawn_Limit_Max_Time;

	}

	public void SpawnNormalObstacles()
	{
		//몬 스 터 생 성 
		currentSpawnObstacleDatas.AddRange(monsterDB.GetNormalMonsterPattern());//DB 에 몬 스 터 해 당 패 턴 리 스 트 가 져 옴

		spawnUpdateTime += Time.deltaTime; //스 폰 시 간 차 이 나 는 거 
		//Debug.Log (currentSpawnObstacleDatas.Count + "개수 ");
		for (int i = 0; i < currentSpawnObstacleDatas.Count;)
		{
			//리 스 트 만 큼 몬 스 터 생 성 시 
			ObstacleData data = currentSpawnObstacleDatas[i]; 

			if (spawnUpdateTime >= data.spawnTime)
			{
				Vector3 position = new Vector3(data.sectorX, data.sectorY, -1); //생 성 위 치 
				GameObject obj= GameObject.Instantiate(Resources.Load("Prefabs/Obstacles/" + data.prefabName), position,
					Quaternion.identity) as GameObject; //생 성 시 
				 
				if (obj != null)
				{
					Obstacle obstacle = obj.GetComponent<Obstacle>();
					//obstacle.Init(obstacleSpawnLevel);
			
					Game.Instance.gameScene.AddObstacle(obstacle);// 게 임 신 에 몬 스 터 개 수 증 가  
				}

				currentSpawnObstacleDatas.RemoveAt(i);//여 기 서 삭 제 
			}
			else
			{
				i++;
			}
		}

		if (currentSpawnObstacleDatas.Count == 0)
		{
			//생 성 상 태 종 료 
			spawnState = SpawnState.Stop;
		}
	}

	public void ResetSpawnList()
	{
		monsterDB.ResetSpawnData ();
	}

	public void SpawnDragObstacles()
	{
		//몬 스 터 생 성 
		currentSpawnObstacleDatas.AddRange(monsterDB.GetDragMonsterPattern());//DB 에 몬 스 터 해 당 패 턴 리 스 트 가 져 옴

		spawnUpdateTime += Time.deltaTime; //스 폰 시 간 차 이 나 는 거 
		//Debug.Log (currentSpawnObstacleDatas.Count + "개수 ");
		for (int i = 0; i < currentSpawnObstacleDatas.Count;)
		{
			//리 스 트 만 큼 몬 스 터 생 성 시 
			ObstacleData data = currentSpawnObstacleDatas[i]; 

			if (spawnUpdateTime >= data.spawnTime)
			{
				Vector3 position = new Vector3(data.sectorX, data.sectorY, -1); //생 성 위 치 
				GameObject obj= GameObject.Instantiate(Resources.Load("Prefabs/Obstacles/" + data.prefabName), position,
					Quaternion.identity) as GameObject; //생 성 시 

				if (obj != null)
				{
					Obstacle obstacle = obj.GetComponent<Obstacle>();
					//obstacle.Init(obstacleSpawnLevel);
					obstacle.isDragType=true;
					Game.Instance.gameScene.AddObstacle(obstacle);// 게 임 신 에 몬 스 터 개 수 증 가  
				}

				currentSpawnObstacleDatas.RemoveAt(i);//여 기 서 삭 제 
			}
			else
			{
				i++;
			}
		}

		if (currentSpawnObstacleDatas.Count == 0)
		{
			//생 성 상 태 종 료 
			spawnState = SpawnState.Stop;
		}
	}

	/*  public void UpdateSpawnInterval()
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
                game.obstacleSpawnManager.SpawnObstacles (); //생 성 큐 에 삽 입 
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
			//개 수 만 큼 리 스 트 에 집 어 넣 음 
            ObstacleData data = currentSpawnObstacleDatas[i];

            if (spawnUpdateTime >= data.spawnTime)
            {
                Vector3 position = new Vector3(data.sectorX * 2, data.sectorY * 2, -1); //생 성 위 치 
				GameObject obj= GameObject.Instantiate(Resources.Load("Prefabs/Obstacles/" + data.prefabName), position,
                        Quaternion.identity) as GameObject;

                if (obj != null)
                {
                    Obstacle obstacle = obj.GetComponent<Obstacle>();
                    obstacle.Init(obstacleSpawnLevel);
                    game.gameScene.AddObstacle(obstacle);// 게 임 신 에 데 이 터 넘 김 
                }

                currentSpawnObstacleDatas.RemoveAt(i);//여 기 서 삭 제 
            }
            else
            {
                i++;
            }
        }

        if (currentSpawnObstacleDatas.Count == 0)
        {
			//생 성 상 태 종 
            spawnState = SpawnState.Stop;
        }
    }*/

	/* bool IsEnableSpawnTime()
    {
		//obstacleSpawnDeltaTime 시 간 이 4 초 보 다 크 면 몬 스 터 생 성 시 켜 야 함 
        if(obstacleSpawnDeltaTime >= Obstacle_Spawn_Interval)
        {
            return true;
        }

        return false;
    }*/


}