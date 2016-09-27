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

		for (int i = 0; i < 2000; i++) {
			//클 래 스 아 이 디 0 ~ 1999 생 성
			m_classIdGenerator.Add (i);
		}
	}

	public void FixedUpdate()
	{
		m_elapsedTime += Time.fixedDeltaTime / Time.timeScale;

		if (m_elapsedTime < 1)
			return;

		m_elapsedTime -= 1;

		if(m_classList.Count!=0)
		{
			Dictionary<int,NEMonoBehaviour>.Enumerator rator = m_classList.GetEnumerator ();

			while(rator.MoveNext())
			{
				NEMonoBehaviour beHaviour = rator.Current.Value;
				if(beHaviour==null)
				{
					continue;
				}

				beHaviour.UpdateOneSecond ();
			}
		}
	}

	void LateUpdate()
	{
		m_occurEvent = true;
		if(m_classList.Count!=0)
		{
			while(true)
			{
				if (m_occurMessageList.Count == 0)
					break;

				EventActionMessageInfo info = m_occurMessageList [0];
				Dictionary<int, NEMonoBehaviour>.Enumerator rator = m_classList.GetEnumerator ();

				while(rator.MoveNext())
				{
					NEMonoBehaviour behaviour = rator.Current.Value;
					if (behaviour == null) {
						continue;
					}
					behaviour.OccurEventActionMessage (info._msg, info._param);
				}
				m_occurMessageList.RemoveAt (0);
			}	
		}
		m_occurEvent = false;

		if(m_removeClass.Count!=0)
		{
			Dictionary<int, NEMonoBehaviour>.Enumerator rator = m_classList.GetEnumerator ();

			while(rator.MoveNext())
			{
				if(ReturnClassIndex(rator.Current.Key)==false)
				{
					NEMonoBehaviour beHaviour = rator.Current.Value;
					if(beHaviour)
					{
						//Debug.Log (string.Format ("{0}class Id Error!1", beHaviour.name));
					}

				}
				m_classList.Remove (rator.Current.Key);

			}
			m_removeClass.Clear ();
		}

		if(m_prepareAwakeList.Count!=0)
		{
			for (int i = 0; i < m_prepareAwakeList.Count; i++) {
				NEMonoBehaviour beHaviour = m_prepareAwakeList [i];
				beHaviour.classIndex = GenerateClassIndex ();
				if(beHaviour.classIndex==0)
				{
					//Debug.Log (beHaviour.name);
				}
				m_classList.Add (beHaviour.classIndex, beHaviour);
			}
			m_prepareAwakeList.Clear ();
		}
	}

	public void CreateClass(NEMonoBehaviour haviour)
	{
		if (m_occurEvent == false) 
		{
			haviour.classIndex = GenerateClassIndex ();
			if (haviour.classIndex == 0) {
				//Debug.Log (haviour.name);
			}
			m_classList.Add (haviour.classIndex, haviour);
		} 
		else
			m_prepareAwakeList.Add (haviour);
	}

	public void RemoveClass(int index)
	{
		if(m_removeClass.ContainsKey(index))
		{
			NEMonoBehaviour haviour = (NEMonoBehaviour)m_removeClass [index];
			if(haviour!=null)
			{
				Debug.Log (haviour.name);
			}
			else
			{
				Debug.Log ("RemoveClass Duplicate Add 0!!!!!");
				return;
			}
		}

		if(m_classList.ContainsKey(index)==false)
		{
//			Debug.Log (string.Format ("RemoveClass noClass 0!!!!!", index));
			return;
		}

		m_removeClass.Add (index, m_classList [index]);
	}

	int GenerateClassIndex()
	{
		int index = m_classIdGenerator [0];
		m_classIdGenerator.RemoveAt (0);
		if(index==0)
		{
			//Debug.Log (string.Format ("GenerateClassIndex index=0 classidNum{0}", m_classIdGenerator.Count));
		}
		return index;
	}

	bool ReturnClassIndex(int classIndex)
	{
		if(m_classIdGenerator.Contains(classIndex))
		{
			return false;
		}
		m_classIdGenerator.Add (classIndex);
		return true;
	}

	public void SendActionMessage(EventMessage msg,params object[] paramList)
	{
		if(m_addOccurEvent==true)
		{
			Debug.Log (msg);
			Debug.Break ();
		}
		m_addOccurEvent = true;
		EventActionMessageInfo info = new EventActionMessageInfo ();
		info._msg = msg;
		info._param = paramList;
		m_occurMessageList.Add (info);

		m_addOccurEvent = false;
	}



}
