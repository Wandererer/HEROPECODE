using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonsterPatternDB  
{

	private List<List<ObstacleData>> spawnList = new List<List<ObstacleData>>();
	private List<List<ObstacleData>> dragSpawnList = new List<List<ObstacleData>>();

	private int spawnNumber = 0;
	private int dragSpawnNumber = 0;

	private string[] prefabNames = new []{"Squirrel", "Bird", "Dove","Bat","Dragon", "Sun", "Moon", "Raindrop", "Planet", "Jesus",
		"Dung", "Ailien", "Angel",   "Buddha", "Rocket", "Thunder"};

	public void Init()
	{
		//패턴 1  다 람 , 새 

		List<ObstacleData> list = new List<ObstacleData>();
		//위 치 정 보 와 함 께 프 리 팹 이 름 까 지 같 이 큐 에 집 어 넣 
		SafelyAddList(list, new ObstacleData(prefabNames[0], 1, 0,  Random.Range(-4, 4f),  Random.Range(-1f, 6f)),prefabNames[0]);
		SafelyAddList(list, new ObstacleData(prefabNames[1], 1, 0,  Random.Range(-4, 4f),  Random.Range(-1f, 6f)),prefabNames[1]);

		spawnList.Add(list);

		//패 턴 2 다 람 , 비 둘 
		list = new List<ObstacleData>();
		//위 치 정 보 와 함 께 프 리 팹 이 름 까 지 같 이 큐 에 집 어 넣 
		SafelyAddList(list, new ObstacleData(prefabNames[0], 1, 0,  Random.Range(-4, 4f),  Random.Range(-1f, 6f)),prefabNames[0]);
		SafelyAddList(list, new ObstacleData(prefabNames[2], 1, 0,  Random.Range(-4, 4f),  Random.Range(-1f, 6f)),prefabNames[2]);

		spawnList.Add(list);

		//패 턴 3 새 , 박 쥐 
		list = new List<ObstacleData>();
		//위 치 정 보 와 함 께 프 리 팹 이 름 까 지 같 이 큐 에 집 어 넣 
		SafelyAddList(list, new ObstacleData(prefabNames[3], 1, 0,  Random.Range(-4, 4f),  Random.Range(-1f, 6f)),prefabNames[3]);
		SafelyAddList(list, new ObstacleData(prefabNames[1], 1, 0,  Random.Range(-4, 4f),  Random.Range(-1f, 6f)),prefabNames[1]);

		spawnList.Add(list);

		//패 턴 4 새 , 비 둘 , 박 쥐 
		list = new List<ObstacleData>();
		//위 치 정 보 와 함 께 프 리 팹 이 름 까 지 같 이 큐 에 집 어 넣 
		SafelyAddList(list, new ObstacleData(prefabNames[1], 1, 0,  Random.Range(-4, 4f),  Random.Range(-1f, 6f)),prefabNames[1]);
		SafelyAddList(list, new ObstacleData(prefabNames[2], 1, 0,  Random.Range(-4, 4f),  Random.Range(-1f, 6f)),prefabNames[2]);
		SafelyAddList(list, new ObstacleData(prefabNames[3], 1, 0,  Random.Range(-4, 4f),  Random.Range(-1f, 6f)),prefabNames[3]);

		spawnList.Add(list);

		//패 턴 5 다 람 , 새 , 박 쥐 
		list = new List<ObstacleData>();
		//위 치 정 보 와 함 께 프 리 팹 이 름 까 지 같 이 큐 에 집 어 넣 
		SafelyAddList(list, new ObstacleData(prefabNames[0], 1, 0,  Random.Range(-4, 4f),  Random.Range(-1f, 6f)),prefabNames[0]);
		SafelyAddList(list, new ObstacleData(prefabNames[1], 1, 0,  Random.Range(-4, 4f),  Random.Range(-1f, 6f)),prefabNames[1]);
		SafelyAddList(list, new ObstacleData(prefabNames[3], 1, 0,  Random.Range(-4, 4f),  Random.Range(-1f, 6f)),prefabNames[3]);

		spawnList.Add(list);

		//패 턴 6 // 다 람 , 새 , 
		list = new List<ObstacleData>();
		//위 치 정 보 와 함 께 프 리 팹 이 름 까 지 같 이 큐 에 집 어 넣 
		SafelyAddList(list, new ObstacleData(prefabNames[0], 1, 0,  Random.Range(-4, 4f),  Random.Range(-1f, 6f)),prefabNames[0]);
		SafelyAddList(list, new ObstacleData(prefabNames[1], 1, 0,  Random.Range(-4, 4f),  Random.Range(-1f, 6f)),prefabNames[1]);
		SafelyAddList(list, new ObstacleData(prefabNames[1], 1, 0,  Random.Range(-4, 4f),  Random.Range(-1f, 6f)),prefabNames[1]);

		spawnList.Add(list);

		//패 턴 7 박 쥐 , 박 쥐 ,박 쥐 
		list = new List<ObstacleData>();
		//위 치 정 보 와 함 께 프 리 팹 이 름 까 지 같 이 큐 에 집 어 넣 
		SafelyAddList(list, new ObstacleData(prefabNames[3], 1, 0,  Random.Range(-4, 4f),  Random.Range(-1f, 6f)),prefabNames[3]);
		SafelyAddList(list, new ObstacleData(prefabNames[3], 1, 0,  Random.Range(-4, 4f),  Random.Range(-1f, 6f)),prefabNames[3]);
		SafelyAddList(list, new ObstacleData(prefabNames[3], 1, 0,  Random.Range(-4, 4f),  Random.Range(-1f, 6f)),prefabNames[3]);

		spawnList.Add(list);

		//패 턴 8 새 , 새 , 비 둘 
		list = new List<ObstacleData>();
		//위 치 정 보 와 함 께 프 리 팹 이 름 까 지 같 이 큐 에 집 어 넣 
		SafelyAddList(list, new ObstacleData(prefabNames[1], 1, 0,  Random.Range(-4, 4f),  Random.Range(-1f, 6f)),prefabNames[0]);
		SafelyAddList(list, new ObstacleData(prefabNames[2], 1, 0,  Random.Range(-4, 4f),  Random.Range(-1f, 6f)),prefabNames[0]);
		SafelyAddList(list, new ObstacleData(prefabNames[1], 1, 0,  Random.Range(-4, 4f),  Random.Range(-1f, 6f)),prefabNames[0]);

		spawnList.Add(list);

		//패 턴 9 다 람 , 비 둘 , 비 둘 , 박 쥐 
		list = new List<ObstacleData>();
		//위 치 정 보 와 함 께 프 리 팹 이 름 까 지 같 이 큐 에 집 어 넣 
		SafelyAddList(list, new ObstacleData(prefabNames[0], 1, 0,  Random.Range(-4, 4f),  Random.Range(-1f, 6f)),prefabNames[0]);
		SafelyAddList(list, new ObstacleData(prefabNames[2], 1, 0,  Random.Range(-4, 4f),  Random.Range(-1f, 6f)),prefabNames[2]);
		SafelyAddList(list, new ObstacleData(prefabNames[2], 1, 0,  Random.Range(-4, 4f),  Random.Range(-1f, 6f)),prefabNames[2]);
		SafelyAddList(list, new ObstacleData(prefabNames[3], 1, 0,  Random.Range(-4, 4f),  Random.Range(-1f, 6f)),prefabNames[3]);

		spawnList.Add(list);

		//패 턴 10 비 둘 , 박 쥐 , 박 쥐 , 박 쥐 
		list = new List<ObstacleData>();
		//위 치 정 보 와 함 께 프 리 팹 이 름 까 지 같 이 큐 에 집 어 넣 
		SafelyAddList(list, new ObstacleData(prefabNames[2], 1, 0,  Random.Range(-4, 4f),  Random.Range(-1f, 6f)),prefabNames[2]);
		SafelyAddList(list, new ObstacleData(prefabNames[3], 1, 0,  Random.Range(-4, 4f),  Random.Range(-1f, 6f)),prefabNames[3]);
		SafelyAddList(list, new ObstacleData(prefabNames[3], 1, 0,  Random.Range(-4, 4f),  Random.Range(-1f, 6f)),prefabNames[3]);
		SafelyAddList(list, new ObstacleData(prefabNames[3], 1, 0,  Random.Range(-4, 4f),  Random.Range(-1f, 6f)),prefabNames[3]);

		spawnList.Add(list);

		//패 턴 11 다 람 , 비 둘 , 비 둘 , 비 
		list = new List<ObstacleData>();
		//위 치 정 보 와 함 께 프 리 팹 이 름 까 지 같 이 큐 에 집 어 넣 
		SafelyAddList(list, new ObstacleData(prefabNames[0], 1, 0,  Random.Range(-4, 4f),  Random.Range(-1f, 6f)),prefabNames[0]);
		SafelyAddList(list, new ObstacleData(prefabNames[2], 1, 0,  Random.Range(-4, 4f),  Random.Range(-1f, 6f)),prefabNames[2]);
		SafelyAddList(list, new ObstacleData(prefabNames[2], 1, 0,  Random.Range(-4, 4f),  Random.Range(-1f, 6f)),prefabNames[2]);
		SafelyAddList(list, new ObstacleData(prefabNames[2], 1, 0,  Random.Range(-4, 4f),  Random.Range(-1f, 6f)),prefabNames[2]);

		spawnList.Add(list);

		//패 턴 12 // 비 둘 , 비 둘 , 비 둘 , 박 쥐 
		list = new List<ObstacleData>();
		//위 치 정 보 와 함 께 프 리 팹 이 름 까 지 같 이 큐 에 집 어 넣 
		SafelyAddList(list, new ObstacleData(prefabNames[2], 1, 0,  Random.Range(-4, 4f),  Random.Range(-1f, 6f)),prefabNames[2]);
		SafelyAddList(list, new ObstacleData(prefabNames[2], 1, 0,  Random.Range(-4, 4f),  Random.Range(-1f, 6f)),prefabNames[2]);
		SafelyAddList(list, new ObstacleData(prefabNames[2], 1, 0,  Random.Range(-4, 4f),  Random.Range(-1f, 6f)),prefabNames[2]);
		SafelyAddList(list, new ObstacleData(prefabNames[3], 1, 0,  Random.Range(-4, 4f),  Random.Range(-1f, 6f)),prefabNames[3]);

		spawnList.Add(list);

		//패 턴 13 다 람 , 다 람 , 다 람 , 박 쥐 
		list = new List<ObstacleData>();
		//위 치 정 보 와 함 께 프 리 팹 이 름 까 지 같 이 큐 에 집 어 넣 
		SafelyAddList(list, new ObstacleData(prefabNames[0], 1, 0,  Random.Range(-4, 4f),  Random.Range(-1f, 6f)),prefabNames[0]);
		SafelyAddList(list, new ObstacleData(prefabNames[0], 1, 0,  Random.Range(-4, 4f),  Random.Range(-1f, 6f)),prefabNames[0]);
		SafelyAddList(list, new ObstacleData(prefabNames[0], 1, 0,  Random.Range(-4, 4f),  Random.Range(-1f, 6f)),prefabNames[0]);
		SafelyAddList(list, new ObstacleData(prefabNames[3], 1, 0,  Random.Range(-4, 4f),  Random.Range(-1f, 6f)),prefabNames[3]);

		spawnList.Add(list);

		//패 턴 14 다 람 쥐 , 새, 새, 박 
		list = new List<ObstacleData>();
		//위 치 정 보 와 함 께 프 리 팹 이 름 까 지 같 이 큐 에 집 어 넣 
		SafelyAddList(list, new ObstacleData(prefabNames[0], 1, 0,  Random.Range(-4, 4f),  Random.Range(-1f, 6f)),prefabNames[0]);
		SafelyAddList(list, new ObstacleData(prefabNames[1], 1, 0,  Random.Range(-4, 4f),  Random.Range(-1f, 6f)),prefabNames[1]);
		SafelyAddList(list, new ObstacleData(prefabNames[1], 1, 0,  Random.Range(-4, 4f),  Random.Range(-1f, 6f)),prefabNames[1]);
		SafelyAddList(list, new ObstacleData(prefabNames[3], 1, 0,  Random.Range(-4, 4f),  Random.Range(-1f, 6f)),prefabNames[3]);

		spawnList.Add(list);

		//패 턴 15  드 래 곤 
		list = new List<ObstacleData>();
		//위 치 정 보 와 함 께 프 리 팹 이 름 까 지 같 이 큐 에 집 어 넣 
		SafelyAddList(list, new ObstacleData(prefabNames[4], 5, 0, 0,6),prefabNames[4]);

		spawnList.Add(list);


		//Debug.Log (spawnList.Count + "sfasdfds");

		//--------------------------------------------------
		//드 래 그 패   sun 5 moon 6 planet 8

		//위 치 정 보 와 함 께 프 리 팹 이 름 까 지 같 이 큐 에 집 어 넣 
		//패 턴 1 태양 달 
		list = new List<ObstacleData>();

		SafelyAddListDrag(list, new ObstacleData(prefabNames[5], 1, 0,  Random.Range(-4, 4f),  Random.Range(-1f, 6f)),prefabNames[5]);
		SafelyAddListDrag(list, new ObstacleData(prefabNames[6], 1, 0,  Random.Range(-4, 4f),  Random.Range(-1f, 6f)),prefabNames[6]);

		dragSpawnList.Add(list);

		//pattern 2 planet moon
		list = new List<ObstacleData>();

		SafelyAddListDrag(list, new ObstacleData(prefabNames[6], 1, 0,  Random.Range(-4, 4f),  Random.Range(-1f, 6f)),prefabNames[6]);
		SafelyAddListDrag(list, new ObstacleData(prefabNames[8], 1, 0,  Random.Range(-4, 4f),  Random.Range(-1f, 6f)),prefabNames[8]);

		dragSpawnList.Add(list);

		//pattern 3 sum planet
		list = new List<ObstacleData>();

		SafelyAddListDrag(list, new ObstacleData(prefabNames[5], 1, 0,  Random.Range(-4, 4f),  Random.Range(-1f, 6f)),prefabNames[5]);
		SafelyAddListDrag(list, new ObstacleData(prefabNames[8], 1, 0,  Random.Range(-4, 4f),  Random.Range(-1f, 6f)),prefabNames[8]);

		dragSpawnList.Add(list);

		//pattern 4 sun moon moon
		list = new List<ObstacleData>();

		SafelyAddListDrag(list, new ObstacleData(prefabNames[5], 1, 0,  Random.Range(-4, 4f),  Random.Range(-1f, 6f)),prefabNames[5]);
		SafelyAddListDrag(list, new ObstacleData(prefabNames[6], 1, 0,  Random.Range(-4, 4f),  Random.Range(-1f, 6f)),prefabNames[6]);
		SafelyAddListDrag(list, new ObstacleData(prefabNames[6], 1, 0,  Random.Range(-4, 4f),  Random.Range(-1f, 6f)),prefabNames[6]);

		dragSpawnList.Add(list);

		//pattern 3 sun sun sun
		list.Clear();

		SafelyAddListDrag(list, new ObstacleData(prefabNames[5], 1, 0,  Random.Range(-4, 4f),  Random.Range(-1f, 6f)),prefabNames[5]);
		SafelyAddListDrag(list, new ObstacleData(prefabNames[5], 1, 0,  Random.Range(-4, 4f),  Random.Range(-1f, 6f)),prefabNames[5]);
		SafelyAddListDrag(list, new ObstacleData(prefabNames[5], 1, 0,  Random.Range(-4, 4f),  Random.Range(-1f, 6f)),prefabNames[5]);

		dragSpawnList.Add(list);

//		Debug.Log (dragSpawnList.Count + " count");

		//list.Clear ();
	}

	public void ResetSpawnData()
	{
		spawnList.Clear ();
		dragSpawnList.Clear ();
		spawnNumber = 0;
		dragSpawnNumber = 0;
		Init ();
	} 

	public List<ObstacleData> GetNormalMonsterPattern()
	{
		List<ObstacleData> obstacleDatas = spawnList[spawnNumber]; //패 턴 리 턴 을 위 해 꺼 냄
		Game.Instance.obstacleSpawnManager.spawnState = SpawnState.Start; //스 폰 시 작 상 태

		spawnNumber++;
		spawnNumber %= (spawnList.Count);
		return obstacleDatas;
	}

	public List<ObstacleData> GetDragMonsterPattern()
	{
		List<ObstacleData> obstacleDatas = dragSpawnList [dragSpawnNumber]; //패 턴 리 턴 을 위 해 꺼 냄
		Game.Instance.obstacleSpawnManager.spawnState = SpawnState.Start; //스 폰 시 작 상 태 
		//Debug.Log(obstacleDatas[0].prefabName +" prefab");
		dragSpawnNumber++;
		dragSpawnNumber %= (dragSpawnList.Count-1);
		return obstacleDatas;
	}

	public int GetSpawnNumber()
	{
		return spawnNumber;
	}

	public int GetDragSpawnNumber()
	{
		return dragSpawnNumber;
	}

	private void SafelyAddList(List<ObstacleData> list, ObstacleData data,string name)
	{
		//겹 치 지 않 으 면 데 이 터 에 집 어 넣 음 
		while(list.FindIndex(x=>((x.sectorX<=(data.sectorX+1.4f) && x.sectorY<=(data.sectorY+1.4f)) && (x.sectorX>=(data.sectorX-1.4f) && x.sectorY>=(data.sectorY-1.4f)) ))!=-1)
		{
			data = new ObstacleData (name, 1, 0, Random.Range (-4f, 4f), Random.Range (-1f, 6f));
		}
		list.Add(data);

	}

	private void SafelyAddListDrag(List<ObstacleData> list, ObstacleData data,string name)
	{
		//겹 치 지 않 으 면 데 이 터 에 집 어 넣 음 
		while(list.FindIndex(x=>((x.sectorX<=(data.sectorX+2.0f) && x.sectorY<=(data.sectorY+2.0f)) && (x.sectorX>=(data.sectorX-2.0f) && x.sectorY>=(data.sectorY-2.0f)) ))!=-1)
		{
			data = new ObstacleData (name, 1, 0, Random.Range (-4f, 4f), Random.Range (-1f, 6f));
		}
		list.Add(data);

	}



		
}