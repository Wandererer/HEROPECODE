using UnityEngine;

public class Item  : NEMonoBehaviour{
	public int itemID; 
	public string itemDesc; // 아 이 템 설 명 용
	//아 이 템 효 과 .... 나 중 에 생 성 
	public ItemType itemType; // 아 이 템 타 입 용
    public Sprite pressedSprite;
	public bool isUse=true;

	private float destroyTime=0.7f;

	void Update()
	{
		if(!isUse)
		{
			destroyTime -= Time.deltaTime;
			if (destroyTime <= 0f)
				Destroy (gameObject);
		}

		DestroyByYPosition ();
	}

	public void SetIsUseFalse()
	{
		isUse = false;
	}

	void DestroyByYPosition()
	{
		if(transform.position.y<=-10)
		{
			Destroy (gameObject);
		}
	}
}
