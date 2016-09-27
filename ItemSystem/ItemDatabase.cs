using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ItemDatabase  {

	//미 리 아 이 템 정 보 들 추 가 해 놈
    public object[] objects;
    public List<GameObject> items;


    public void InitItemDatabase()
    {
		//아 이 템 정 보 불 러 옴 
        items=new List<GameObject>();
        objects= Resources.LoadAll(string.Format("Item/"));

        for(int i=0;i<objects.Length;i++)
        {
            items.Add((GameObject)objects[i]);
        }

    }


}
