using System.Xml;
using System.Text;
using System.IO;

public class GameData {

	//이 곳 에 서 x m l 관 련 도 불 러 올 수 있 도 록 함  

	GameSaveData m_saveData=new GameSaveData(); //세 이 브 데 이 터 선 언

	public void FirstSetting()
	{
		//로 드 해 옴 
		m_saveData = LoadData ();
	}

	public GameSaveData saveData
	{
		get {return m_saveData;}
	}

	public void SaveData(GameSaveData saveData)
	{
		//게 임 실 제 저 장 
		Util.SaveBinaryFormat ("GameSaveData", SaveXml (saveData));
		m_saveData = saveData;
	}

	public GameSaveData LoadData()
	{
		//저 장 된 데 이 터 가 져 옴 
		byte[] loadObject = (byte[])Util.LoadBinaryFormat ("GameSaveData");
		if(loadObject==null) //없 으 면 초 기 화 해 서 다 0 으 로 
		{
			GameSaveData saveData = new GameSaveData ();
			saveData.highScore = 0;
			saveData.money = 0;
			saveData.Bgm = true;
			saveData.Effect = true;
			saveData.SECOND = false;
			saveData.SelectNumber = 0;
			saveData.CharacterNumber = 0;
			return saveData;
		}
		return LoadXml (loadObject); //있으면 로 드 
	}

	byte[] SaveXml(GameSaveData saveData)
	{
		// xml 파 일로 저 장 
		XmlDocument loadXmlDoc=new XmlDocument(); //새 xmldocument 클래스 선언 
		XmlElement rootElement = loadXmlDoc.CreateElement ("ROOT");//루 트 생 

		rootElement.SetAttribute ("HighScore", saveData.highScore.ToString ()); //안에 text로 value 지정
		rootElement.SetAttribute ("Money", saveData.money.ToString ());
		rootElement.SetAttribute ("Bgm", saveData.Bgm.ToString ());
		rootElement.SetAttribute ("Effect", saveData.Effect.ToString ());
		rootElement.SetAttribute ("SECOND", saveData.SECOND.ToString ());
		rootElement.SetAttribute ("Select", saveData.SelectNumber.ToString ());
		rootElement.SetAttribute ("Character", saveData.CharacterNumber.ToString ());

		loadXmlDoc.AppendChild (rootElement);//루 트 하 위 로 데 이 터 추 가 

		//File.WriteAllText (Application.dataPath+"/save.xml", loadXmlDoc.OuterXml,System.Text.Encoding.UTF8);
		//  직 접 생 성 가 능  확 인 용 

		return Encoding.Default.GetBytes (loadXmlDoc.OuterXml);
	}

	GameSaveData LoadXml(byte[] loadData)
	{
		//데 이 터 로 드 
		GameSaveData saveData = new GameSaveData ();

		MemoryStream m = new MemoryStream (loadData);
		XmlDocument loadXmlDoc = new XmlDocument ();
		loadXmlDoc.Load (m);

		XmlNode rootNode = loadXmlDoc.SelectSingleNode ("ROOT");

		for (int i = 0; i < rootNode.Attributes.Count; i++) {
			int temp = 0;
			bool isBool;
			XmlAttribute attr = rootNode.Attributes [i];

			switch(attr.Name)
			{
			case "HighScore":
				//점수 일 경우 불러옴 
				if(int.TryParse(attr.Value,out temp))
				{
					saveData.highScore = temp;
				}
				break;
			case "Select":
				//점수 일 경우 불러옴 
				if(int.TryParse(attr.Value,out temp))
				{
					saveData.SelectNumber = temp;
				}
				break;
			case "Money":
				//돈 일 경 우 불 러 옴 
				if(int.TryParse(attr.Value,out temp))
				{
					saveData.money = temp;
				}
				break;

			case "Bgm":
				if(bool.TryParse(attr.Value,out isBool))
				{
					saveData.Bgm = isBool;
				}
				break;

			case "Effect":
				if(bool.TryParse(attr.Value,out isBool))
				{
					saveData.Effect = isBool;
				}
				break;

			case "SECOND":
				if (bool.TryParse (attr.Value, out isBool)) {
					saveData.SECOND = isBool;
				}
				break;

			case "Character":
				if(int.TryParse(attr.Value,out temp))
				{
					saveData.CharacterNumber = temp;
				}

				break;

			default :

				break;
			}
		}

		return saveData;
	}

	public void UpdateHighScore(int score)
	{
		//high score 저 장 
		GameSaveData saveData = LoadData ();
		saveData.highScore = score;
		SaveData (saveData);
	}

	public void AddMoney(int money)
	{
		//돈 얻 은 만 큼 저 장 
		GameSaveData saveData = LoadData ();
		saveData.money += money;
		SaveData (saveData);
	}

	public void MinusMoney(int money)
	{
		//돈 쓴 만 큼 저 
	
		GameSaveData saveData = LoadData ();
		saveData.money -= money;
		SaveData (saveData);
	}

	public void SaveBgmToggleInfo(bool current)
	{
		//bgm 상 태 저 
		GameSaveData saveData = LoadData ();
		saveData.Bgm = current;
		SaveData (saveData);
	}

	public void SaveEffectToggleInfo(bool current)
	{
		//effect 상 태 저 
		GameSaveData saveData = LoadData ();
		saveData.Effect = current;
		SaveData (saveData);
	}

	public void SaveMyCharacterNumber(int number)
	{
		//현 재 캐 릭 터 저 장 
		GameSaveData saveData = LoadData ();
		saveData.CharacterNumber = number;
		SaveData (saveData);
	}

	public void ChangeTitleImageForSelectCharacter(int index)
	{
		//선 택 한 캐 릭 터 정 보 저 
		GameSaveData saveData = LoadData ();
		saveData.SelectNumber = index;
		SaveData (saveData);
	}

	public void SaveCharacterBuyInfo(int current)
	{
		//캐 릭 턴 산 정 보 저 장
		switch(current)
		{
		case 1:
			GameSaveData saveData = LoadData ();
			saveData.SECOND = true;
			SaveData (saveData);
			break;

		default:

			break;
		}
	}
}
