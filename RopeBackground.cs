using UnityEngine;
using System.Collections;

public class RopeBackground : MonoBehaviour {

    private float x,y,z; //이 transform position

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		x=transform.position.x;
		y=transform.position.y;
		z=transform.position.z;
	}


    public void  MoveDown(float speed)
    {
        CheckCurrentPosition ();
        //ngui이면 localposition
		this.transform.position = new Vector3 (x, y-speed, z);
    }

    public void CheckCurrentPosition()
    {
		if(y<= -20f)
        {
            //ngui이면 localposition
			this.transform.position = new Vector3 (x, y += 48, z);
            return;
        }

    }
}
