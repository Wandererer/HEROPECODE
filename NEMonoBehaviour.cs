using UnityEngine;
using System.Collections;

public class NEMonoBehaviour : MonoBehaviour {

    public Game game
    {
        get { return Game.Instance; }
    }

	int m_classIndex; //클래스 개수들

	public NEMonoBehaviour(){}

	public virtual void Awake()
	{
		#if UNITY_EDITOR
		if(Application.isPlaying==false)
		{
			return;
		}
		#endif

		if(NEMonoBehaviourManager.Instance==null)
		{
			//각 종 매 니 저 용 생 성
			Util.SetGlobalManagerByAwake ();
		}
		//NEMonoBehaviourManager.Instance.CreateClass (this);
	}

	public virtual void OnDestroy()
	{
		#if UNITY_EDITOR
		if(Application.isPlaying==false)
			return;
		#endif
		NEMonoBehaviourManager.Instance.RemoveClass (m_classIndex);

	}

	public virtual void OccurEventActionMessage(EventMessage msg, params object[] paramList){
	}

	public virtual void SetActive(bool active)
	{
		gameObject.SetActive (active);
	}

	public virtual void UpdateOneSecond() {
	}

	public bool IsActive()
	{
		return gameObject.activeSelf;
	}

	public int classIndex
	{
		get { return classIndex; }
		set { classIndex = value; }
	}
}
