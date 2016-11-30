using System.Collections.Generic;
using UnityEngine;

public struct ObstacleData
{
    public string prefabName;
    public int hp;
    public float spawnTime;
    public float sectorX;
    public float sectorY;

    public ObstacleData(string prefabName, int hp, float spawnTime, float sectorX, float sectorY)
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
	private int bossHp=0;

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

		//Debug.Log (monsterDB.GetSpawnNumber() + " normal");

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
					//Debug.Log(monsterDB.GetSpawnNumber() +" dd");
					//obstacle.Init(obstacleSpawnLevel);
				
					if(bossHp<4 && obstacle.isBoss==true)
					{
						//Debug.Log(monsterDB.GetSpawnNumber() +" test");
						bossHp++;
						obstacle.hp=bossHp;
						SetHeartSpriteLineUp (bossHp);
					}

					else if(bossHp>=4)
					{
						obstacle.hp = 4;
						SetHeartSpriteLineUp (4);
					}
			
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

	public void BossHpReset()
	{
		bossHp = 0;
	}

	private void SetHeartSpriteLineUp(int bossHP)
	{
		GameObject Heart = GameObject.FindGameObjectWithTag ("HeartSprite");


		switch(bossHp)
		{
		case 1:
			SpriteRendererOn (bossHP, Heart);
			Heart.transform.localPosition = new Vector3 (2.45f, 0, 0);
			break;

		case 2:
			SpriteRendererOn (bossHP, Heart);
			Heart.transform.localPosition = new Vector3 (1.7f, 0, 0);
			break;

		case 3:
			SpriteRendererOn (bossHP, Heart);
			Heart.transform.localPosition = new Vector3 (0.9f, 0, 0);
			break;

		case 4:
			SpriteRendererOn (bossHP, Heart);
			Heart.transform.localPosition = new Vector3 (0.03f, 0, 0);
			break;
		}
	}

	private void SpriteRendererOn(int hp, GameObject Heart)
	{
		SpriteRenderer[] heartsSprite = Heart.GetComponentsInChildren<SpriteRenderer> ();

		for(int i=0;i<hp;i++)
		{
			heartsSprite [i].enabled = true;
		}
	}

	public int GetMonsterNormalPatternNumber()
	{
		return monsterDB.GetSpawnNumber ();
	}

	public int GetDragMonsterPatternNumber()
	{
		return monsterDB.GetDragSpawnNumber ();
	}


	public void ResetSpawnList()
	{
		monsterDB.ResetSpawnData ();
	}

	public void SpawnDragObstacles()
	{
		//몬 스 터 생 성 
		currentSpawnObstacleDatas.AddRange(monsterDB.GetDragMonsterPattern());//DB 에 몬 스 터 해 당 패 턴 리 스 트 가 져 옴

		//Debug.Log (monsterDB.GetDragSpawnNumber () + " drag");

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

		Game.Instance.gameScene.SetIsDragTrue ();
	}




}