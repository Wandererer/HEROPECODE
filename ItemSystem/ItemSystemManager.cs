using UnityEngine;

public class ItemSystemManager : NEMonoBehaviour {

    private static ItemSystemManager instance;//싱 글 톤 선 언 

    public static ItemSystemManager Instance
    {
        get { return instance; }
        set { instance=value; }
    }

    private static readonly float itemSpawnRate=0.2f;


    public ItemDatabase itemDB=new ItemDatabase();
    private int itemNumber;

    public override void Awake()
    {
        base.Awake();
        instance=this;
    }


    // Use this for initialization
	void Start () {
		itemDB.InitItemDatabase();
		//Instantiate (itemDB.items [0], new Vector3 (-2f, 0, -1), Quaternion.identity);
	}
	
	public void SpawnItem(Vector3 pos)
    {
        //아 이 템 생 
		bool result =Random.Range (0f, 1.0f) <= itemSpawnRate;
        if(result)
        {
			itemNumber = GenerateItemRandomInt();
            pos.z=-1f;
            Instantiate(itemDB.items[itemNumber],pos,Quaternion.identity);
        }

    }

    public int GenerateItemRandomInt()
    {
        //아이템 번호 가져 옴
        int temp=Random.Range(0,3);
        return temp;
    }

    public void ItemEffectStart(int itemNum)
    {
		//아 이 템 효 과 마 다 맞 는 거 시 작ㅁ
        switch(itemNum)
        {
            case 0://hp 증가
                if(Earth.Instance.currHP<3)
                {
                    Earth.Instance.currHP++;
                }
            break;

            case 1://몬스터 터치 시간 증가
                Game.Instance.gameScene.monsterTimeIncrease=1f;
            break;

            case 2: //부스터
                Game.Instance.gameScene.ActiveBoostState();
            break;

            default:

            break;
        }
    }

    public void DestroyItemObject(GameObject obj)
    {
        Destroy(obj);
    }



/*
		#if UNITY_IOS
			yield return new WaitForSeconds(6.0f);
		#else
			yield return new WaitForSeconds(2.0f);
		#endif*/
		
}
