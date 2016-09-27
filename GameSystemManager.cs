using UnityEngine;
using System.Collections;

public class GameSystemManager : NEMonoBehaviour {

	private static GameSystemManager instance; //싱 글 톤 선 언 

	int gold;


	public static GameSystemManager Instance{
		get {return instance; }
	}

	public override void Awake ()
	{
		base.Awake ();
		instance = this;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
