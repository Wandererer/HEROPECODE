using System.Collections;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private static readonly float Fall_Down_Speed = 10;
    public Sprite[] alive;
    public Sprite[] die;

    public bool isDragType;

    public bool isAlive
    {
        get { return isFallDown == false && isDie == false; }
    }

    private bool isFallDown;
    private bool isDie;
    private float dieDeltaTime = 0.7f;

    private int currentIndex;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Init(int level)
    {
        if(alive.Length <= level)
        {
            level = alive.Length - 1;
        }

        currentIndex = level;
        Alive();
    }

    public void StartFallDown()
    {
        isFallDown = true;
    }

    void Update()
    {
        if(isFallDown)
        {
            transform.position += Vector3.down * Time.deltaTime * Fall_Down_Speed;

            if (transform.position.y < -5)
            {
                Destroy(gameObject);
            }
        }
        else if (isDie)
        {
            dieDeltaTime -= Time.deltaTime;

            if (dieDeltaTime < 0)
            {
                Destroy(gameObject);
            }
        }
    }

    public void Alive()
    {
        spriteRenderer.sprite = alive[currentIndex];
    }

    public void StartDie()
    {
        isDie = true;
        spriteRenderer.sprite = die[currentIndex];

    }
}