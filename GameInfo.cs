using UnityEngine;

public class GameInfo {

	GameData m_gameData=new GameData();

    float meter = 0; //미 터 
    int highScore; //최 고 점 수 
	int characterNumber; //캐 릭 터 숫 자
    int money; // 돈 
	bool bgm; //배 경 음 악 
	int selectCharacterNumber;//선 택 한 캐 릭 터 숫 자
	bool effect;//효 과 
	//캐 릭 터 샀 나 안 샀 나 
	bool second; // 두 번 째 캐 릭 터 샀 나 안 샀 나

    public GameInfo()
    {
        FirstSetting(); //정 보 가 져 옴 
		//ResetGameData ();
    }

    public void ResetGameData()
	{
		//정 보 초 기 화 
		PlayerPrefs.DeleteAll ();
		FirstSetting ();
	}

	public void FirstSetting()
	{
		//각 게 임 데 이 터 불 러 옴 
		m_gameData.FirstSetting ();

        highScore = gameData.saveData.highScore;
        money = gameData.saveData.money;
		bgm = gameData.saveData.Bgm;
		effect = gameData.saveData.Effect;
		second = gameData.saveData.SECOND;
		selectCharacterNumber = gameData.saveData.SelectNumber;
		characterNumber = gameData.saveData.CharacterNumber;
    }

	public GameData gameData
	{
		get{return m_gameData;}
	}

    public void Reset()
    {
        meter = 0;
    }

    public float Meter
    {
        get { return meter; }
        set { meter = value; }
    }

    public int HighSocre
    {
        get { return highScore; }
        set { highScore = value; }
    }

    public int Money
    {
        get { return money; }
        set { money = value; }
    }

	public bool BGM
	{
		get{ return bgm;}
		set{ bgm = value;}
	}

	public bool Effect
	{
		get{ return effect; }
		set{ effect = value;}
	}

	public bool SECOND
	{
		get { return second; }
		set { second = value; }
	}

	public int SelectNumber{
		get { return selectCharacterNumber; }
		set { selectCharacterNumber = value;}
	}

	public int CharacterNumber {
		get {return characterNumber; }
		set { characterNumber = value; }
	}
}
