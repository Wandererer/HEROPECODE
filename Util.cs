using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public class Util {
	public static void SetGlobalManagerByAwake()
	{
		//게임 시 작 시 안없어 지는 것들 생 성 관 리 용 들ㄴ  
		GameObject noRemoveManager = GameObject.Find ("noRemoveManager");

		if(null==noRemoveManager)
		{
			noRemoveManager = new GameObject ("noRemoveManager");
			MonoBehaviour.DontDestroyOnLoad (noRemoveManager);
		}
		if( null==noRemoveManager.GetComponent<NEMonoBehaviour>())
		{
			noRemoveManager.AddComponent<NEMonoBehaviourManager> ();
		}
			
		if(null==noRemoveManager.GetComponent<ResourceManager>())
		{
			noRemoveManager.AddComponent<ResourceManager> ();
		}



	}

	public static void SaveBinaryFormat(string key, object data)
	{
		// 저 장 용 
		BinaryFormatter b= new BinaryFormatter(); // 바이 너 리 포 맷 을 이 용 객 체 그 래 프 를 스 트 림 으 로 직 렬 
		MemoryStream m = new MemoryStream ();
		b.Serialize (m, data);
		PlayerPrefs.SetString (key, Convert.ToBase64String (m.GetBuffer ()));
		/*
		 게임 세션(session)사이에 플레이어 preference를 저장하고, preference에 접근합니다.

Mac OS X 에서 PlayerPrefs는 ~/Library/Preferences 폴더에 unity.[company name].[product name].plist의 파일이름으로 저장되며,

On Mac OS X PlayerPrefs are stored in ~/Library/Preferences folder, in a file named unity.[company name].[product name].plist, where company and product names are the names set up in Project Settings. Windows 독립 플레이어에서 PlayerPrefs는 @@HKCU
Software
 
		 
		 */ 
		return;
	}

	public static object LoadBinaryFormat(string key)
	{
		// 로 드 용 
		string data = PlayerPrefs.GetString(key);
		if(data==null || data=="")
		{
			return null;
		}

		BinaryFormatter b= new BinaryFormatter();
		MemoryStream m = new MemoryStream (Convert.FromBase64String (data));
		return b.Deserialize (m);
	}

	public static void DeleteBinaryFormat(string key)
	{
		// 삭 제 용 
		PlayerPrefs.DeleteKey (key);
	}

}
