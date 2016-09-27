using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResourceManager : NEMonoBehaviour {

	private static ResourceManager instance; //싱 글 톤 변 수 선 언
	private Dictionary<string, GameObject> m_prefab=new  Dictionary<string, GameObject>();//실 제 로 드 된 거 넣 는 
	private Dictionary<string, GameObject> m_commonPrefab=new  Dictionary<string, GameObject>(); //전 부 넣 는 용
	public static ResourceManager Instance
	{
		get { return instance; }
	}

	public override void Awake ()
	{
		base.Awake ();
		instance = this;

		LoadAllPrefab ();
	}

	public override void OnDestroy ()
	{
		base.OnDestroy ();
	}

	public void LoadAllPrefab()
	{
		//처음 시 작 시 프 리 팹 폴 더 에 서 가 져 오 위 
		object[] temp0=Resources.LoadAll("Prefabs");

		for (int i = 0; i < temp0.Length; i++) {
			GameObject temp1 = (GameObject)(temp0 [i]);
			if (m_commonPrefab.ContainsKey (temp1.name))
				continue;
			m_commonPrefab [temp1.name] = temp1;
		}
	}

	public void LoadPrefab(string objectPath)
	{
		//원 하 는 거 가 져 와 서 집 어 넣 음 
		GameObject gameObject = (GameObject)Resources.Load (objectPath, typeof(GameObject));
		m_prefab.Add (gameObject.name, gameObject);
	}

	public GameObject ClonePrefab(string key)
	{
		// 프 리 팹 생 성 시 킴 
		GameObject temp = null;

		if(m_commonPrefab.ContainsKey(key))
		{
			temp = (GameObject)(GameObject.Instantiate (m_commonPrefab [key]));
		}
		else if(m_prefab.ContainsKey(key))
		{
			temp = (GameObject)(GameObject.Instantiate (m_prefab [key]));
		}

		if(temp==null)
		{
			Debug.Log (string.Format ("Resources Manager Clone Failed key = {0}", key));
			return null;
		}

		temp.name= key+"_Clone";
		return temp;

	}

	public void RemoveAllPrefab(string key)
	{
		//모 든 프 리 팹 제 거 
		if (m_prefab.Count == 0)
			return;

		m_prefab.Clear ();
	}
		
}
