using UnityEngine;
using System.Collections;

public class Background : MonoBehaviour {

	private UISprite sprite;
	private int currIndex;
	public bool isUp = false;

	public int CurrIndex{
		get { return currIndex; }
		set { currIndex = value; }
	}

	// Use this for initialization
	void Start () {
		sprite = this.GetComponent<UISprite> ();
	}
	
	// Update is called once per frame
	void Update () {
		//ngui 이면 localposition

	}

	public void  MoveDown(float speed)
	{
		//speed 만 큼 내 려 감 
		this.transform.localPosition = new Vector3 (transform.localPosition.x, transform.localPosition.y-speed, 0);
	}
		

	public void ChangeSprite(string spriteName)
	{
		//sprite name에 따 라 현 재 SPRITE 변 경 
		sprite.spriteName = spriteName;
	}
}
