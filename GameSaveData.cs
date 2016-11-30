using UnityEngine;
using System.Collections;

public class GameSaveData {
	int m_highScore; //최 고 점 수 
	int m_money; // 돈 
	int characterNumber; //선 택 한 플 레 이 용 캐 릭 터 번 호
	int selectCharacterNumber; //메 인 화 면 용 캐 릭 터 번 
	bool effectPlay; //effect on off
	bool bgmPlay; //bgm on off
	//여 기 서 부 터 는 캐 릭 터 샀 나 안 샀 나 
	bool second;
	bool third;

	public int highScore
	{
		get {return m_highScore; }
		set {m_highScore = value;}
	}

	public int SelectNumber{
		get { return selectCharacterNumber; }
		set { selectCharacterNumber = value;}
	}

	public int money{
		get {return m_money; }
		set {m_money = value; }
	}

	public int CharacterNumber {
		get { return characterNumber; }
		set { characterNumber = value; }
	}

	public bool Effect
	{
		get {return effectPlay;}
		set {effectPlay = value;}
	}

	public bool Bgm{
		get {return bgmPlay;}
		set {bgmPlay = value;}
	}

	public bool SECOND
	{
		get { return second; }
		set { second = value; }
	}

	public bool THIRD
	{
		get { return third;}
		set { third = value;}
	}

}
