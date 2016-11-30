using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BackgroundManager : NEMonoBehaviour {

	private static BackgroundManager instance; //싱 글 톤 선 언 

	public static BackgroundManager Instance
	{
		get { return instance;}
		set {instance = value;}
	}
	RopeBackground[] ropeBackgrounds; // 로 프 배 경 
	public List<Background> backGroundList = new List<Background> (); //뒷 배 경 컨 트 롤 위 한 list 
	string[] spriteNameList={"bg_01","bg_02","bg_03","bg_04","bg_05","bg_06","bg_07","bg_08","bg_09","bg_10","bg_11","bg_12","bg_13","bg_14","bg_15","bg_16"}; //CurrIndex에 따 다 뒷 배 경 변 경 

	public override void Awake ()
	{
		base.Awake ();
		instance = this;
	}


	void Start () {
		Background[] tempBackGround = transform.GetComponentsInChildren<Background> ();
		ropeBackgrounds=GameObject.Find("RopeManager").GetComponentsInChildren<RopeBackground>();
		for (int i = 0; i < tempBackGround.Length; i++) {
			backGroundList.Add (tempBackGround [i]);
		}
		backGroundList [0].CurrIndex = 0;
		backGroundList [1].CurrIndex = 1;
		backGroundList [2].CurrIndex = 2;

		//	Debug.Log ("background po " + backGroundList [0].GetComponent<Transform>().position);
	}

	void ChangeBakcGroundPositionByYvalue()
	{
		//관 리 를 쉽 게 하 기 위 해 리 스 트 위 치 계 속 교 체 
		Background bg = backGroundList [0];
		//Debug.Log ("name " + bg.gameObject.name);
		if(bg.isUp==true)
		{
			bg.isUp = false;
			ChangeSpriteByMeter (bg);
			backGroundList.RemoveAt (0);
			backGroundList.Add (bg);
		}
	}

	public void MoveDown(float speed)
	{
		//speed 만 큼 내 려 감 
		Game.Instance.gameScene.meter += speed;
		for (int i = 0; i < backGroundList.Count; i++) {
			backGroundList [i].MoveDown ((int)speed * 30);

		}

		//checkposition
		ChangeFirstListPositionToLast(); //첫 리 스 트 마 지 막 으 로 이 동

		ChangeBakcGroundPositionByYvalue (); //y 에 따 라 위 치 변 

		for(int i=0;i<ropeBackgrounds.Length;i++)
		{
			ropeBackgrounds[i].MoveDown((int)speed);
		}
	}


	private void ChangeFirstListPositionToLast()
	{
		//리 스 트 위 치 변 경 하 여 관 리 함 
		if(backGroundList[0].transform.localPosition.y<= -1450f)
		{
			Vector3 pos = backGroundList [0].transform.localPosition;
			pos.y = backGroundList [2].transform.localPosition.y + 1000f;
			backGroundList [0].transform.localPosition = pos;
			backGroundList [0].isUp = true;
		}
	}

	public void ChangeSpriteByMeter(Background bg)
	{
		//meter마다 sprite 변경하여 배경이 바뀌는 것처럼 함
		float meter = Game.Instance.gameScene.meter;

		bg.CurrIndex = backGroundList [backGroundList.Count - 1].CurrIndex + 1;

		if(meter<3000 && bg.CurrIndex<11)
		{
			bg.ChangeSprite (spriteNameList [bg.CurrIndex]);
		}

		else if (meter < 3000 && bg.CurrIndex == 11) {
			bg.CurrIndex = 9;
			bg.ChangeSprite (spriteNameList [bg.CurrIndex]);
		}
		else if(meter<99999999 && bg.CurrIndex<16)
		{
			bg.ChangeSprite (spriteNameList [bg.CurrIndex]);
		}

		else if(meter<99999999 && bg.CurrIndex==16)
		{
			bg.CurrIndex = 14;
			bg.ChangeSprite (spriteNameList [bg.CurrIndex]);
		}

	}

	public void SettingBackgroundForContinue(float meter)
	{

		if (meter < 45) 
		{
			SetBackgroundByNumber (0);
		} 
		else if (meter < 80 && meter > 45) 
		{
			SetBackgroundByNumber (1);
		} 
		else if (meter < 115 && meter > 80) 
		{
			SetBackgroundByNumber (2);
		} 
		else if (meter < 140 && meter > 115) 
		{
			SetBackgroundByNumber (3);
		} 
		else if (meter < 175 && meter > 140) 
		{
			SetBackgroundByNumber (4);
		} 
		else if (meter < 210 && meter > 175) 
		{
			SetBackgroundByNumber (5);
		} 
		else if (meter < 245  && meter > 210) 
		{
			SetBackgroundByNumber (6);
		} 
		else if (meter < 280 && meter > 245) 
		{
			SetBackgroundByNumber (7);
		} 
		else if (meter < 3000  && meter > 280) 

		{
			SetBackgroundByNumber (8);
		} 
		else if (meter < 3080&& meter > 3000) 
		{
			SetBackgroundByNumber (11);
		} 
		else if (meter < 3125 && meter > 3080)
		{
			SetBackgroundByNumber (12);
		}
		else if (meter < 99999999 && meter > 3125)
		{
			SetBackgroundByNumber (13);
		}
	}



	void SetBackgroundByNumber(int number)
	{

		/*		Debug.Log(temp +"   cvs;scvsa;lkfjaw");*/


		backGroundList [0].CurrIndex = number;
		backGroundList [0].ChangeSprite (spriteNameList [number++]);

		backGroundList [1].CurrIndex = number;
		backGroundList [1].ChangeSprite (spriteNameList [number++]);

		backGroundList [2].CurrIndex = number;
		backGroundList [2].ChangeSprite (spriteNameList [number++]);
	}

}