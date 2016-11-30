using UnityEngine;
using System.Collections;

[System.SerializableAttribute]
public class CharacterSprite
{
    public Sprite[] front;
    public Sprite[] back;
    public bool dualLayer;
    public bool enableMove;
    private bool isPlaying;
    private int currentIndex;
	private int maxCharacter = 3;

    public bool IsEnableMoveAction()
    {
		//이 동 가 능 한 지 확 인 
        if(enableMove == false)
        {
            return false;
        }

        if(front.Length > 1)
        {
			return currentIndex % maxCharacter == 0;
        }

        return true;
    }

    public void PlayNextSprite(SpriteRenderer frontRenderer, SpriteRenderer backRenderer)
    {
		//다 음 스 프 라 이 트 로 이 동ㅁ
        if (isPlaying == false)
        {
            currentIndex = 0;
            isPlaying = true;
        }

        SetSprite(currentIndex, frontRenderer, backRenderer);

        currentIndex++;

        if (currentIndex >= front.Length)
        {
            currentIndex = 0;
        }
    }

    public void SetEndFrame(SpriteRenderer frontRenderer, SpriteRenderer backRenderer)
    {
        SetSprite(front.Length - 1, frontRenderer, backRenderer);
    }

    private void SetSprite(int index, SpriteRenderer frontRenderer, SpriteRenderer backRenderer)
    {
        frontRenderer.sprite = front[index];
        if(back != null)
        {
            backRenderer.sprite = back[index];
        }
    }

    public void Stop()
    {
        isPlaying = false;
    }

    public bool IsEndFrame()
    {
        return isPlaying && currentIndex == 0;
    }
}

public class Character : MonoBehaviour
{
    public CharacterSprite climb;
    public CharacterSprite attack;
    public CharacterSprite fix;
    public CharacterSprite boost;
    public CharacterSprite fall;
    public CharacterSprite stand;

    private SpriteRenderer frontRenderer;
    private SpriteRenderer backRenderer;

    private CharacterSprite currentSprite;
    private bool isClimb;

    public bool IsEnableMoveAction()
    {
        if(currentSprite == null)
        {
            return false;
        }

        return currentSprite.IsEnableMoveAction();
    }

    public bool IsEndFrame()
    {
        if(currentSprite == null)
        {
            return false;
        }

        return currentSprite.IsEndFrame();
    }

    void Awake()
    {
        frontRenderer = transform.FindChild("Front").GetComponent<SpriteRenderer>();
        backRenderer = transform.FindChild("Back").GetComponent<SpriteRenderer>();
    }

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

    private void Play(CharacterSprite sprite)
    {
        if(currentSprite != null && currentSprite != sprite)
        {
            currentSprite.Stop();
        }

        sprite.PlayNextSprite(frontRenderer, backRenderer);
        currentSprite = sprite;
    }

    public void ReadyClimb()
    {
        climb.SetEndFrame(frontRenderer, backRenderer);
        currentSprite = climb;
    }

    public void ReadyStand()
    {
        stand.SetEndFrame(frontRenderer, backRenderer);
        currentSprite = stand;
    }

	public void ReadyFix()
	{
		fix.SetEndFrame (frontRenderer, backRenderer);
		currentSprite = fix;
	}

    public void Climb()
    {
        Play(climb);
    }

    public void Attack()
    {
        Play(attack);
        StartCoroutine(RecoveryClimb());
    }

    private IEnumerator RecoveryClimb()
    {
        yield return new WaitForSeconds(0.3f);
		if(currentSprite==boost)
		{
			
		}
        else if(currentSprite != climb)
        {
            ReadyClimb();
        }
    }
		 
    public void Fix()
    {
        Play(fix);
    }

    public void Boost()
    {
        Play(boost);
    }

    public void Stand()
    {
        Play(stand);
    }

    public void Fall()
    {
        Play(fall);
    }
}
