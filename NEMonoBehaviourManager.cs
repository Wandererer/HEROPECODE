using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NEMonoBehaviourManager : MonoBehaviour {

	public class EventActionMessageInfo
	{
		public EventMessage _msg;
		public object[] _param;
	}

	private static NEMonoBehaviourManager instance; // 싱글톤선언

	List<EventActionMessageInfo> m_occurMessageList = new List<EventActionMessageInfo> ();
	List<int> m_classIdGenerator= new List<int>(); //class id 생
	List<NEMonoBehaviour> m_prepareAwakeList=new List<NEMonoBehaviour>(); // 

	Dictionary<int, NEMonoBehaviour> m_classList = new Dictionary<int, NEMonoBehaviour> (); //NEMonoBehaviour 관리
	Dictionary<int, NEMonoBehaviour>m_removeClass=new Dictionary<int, NEMonoBehaviour>();// NEMonoBehaviour 삭제 관 

	float m_elapsedTime=0;

	bool m_occurEvent=false; //일어난 이 벤 트
	bool m_addOccurEvent=false; // 이벤트 넣었는게 일 어 났 

	public static NEMonoBehaviourManager Instance
	{
		get{ return instance; }
	}

	void Awake()
	{
		instance = this;

	}





}
