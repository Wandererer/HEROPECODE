using UnityEngine;
using System.Collections;

public class BackgroundStar : MonoBehaviour
{
    public float speed = 1;

    private UISprite sprite;
    private float alpha;
    private bool isForwardAlpha;
	// Use this for initialization
	void Awake ()
	{
	    sprite = GetComponent<UISprite>();
	    alpha = 0.0f;
	    isForwardAlpha = true;
	}
	
	// Update is called once per frame
	void Update () {
	    sprite.color = new Color(1, 1, 1, alpha);

	    if (isForwardAlpha)
	    {
            alpha += Time.deltaTime * speed;

	        if (alpha > 1)
	        {
	            isForwardAlpha = false;
	        }
	    }
        else if (isForwardAlpha == false)
        {
            alpha -= Time.deltaTime * speed;

            if (alpha < 0)
            {
                isForwardAlpha = true;
            }
        }
	}
}
