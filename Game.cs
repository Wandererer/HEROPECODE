/// <summary>
/// 어플리케이션의 모든 클래스를 관리하는 슈퍼클래스
/// NEMonoBehavior에서 game으로 접근할 수 있으며
/// 일반 클래스에서 접근시 public Game game{ get { return Game.Instance; }} 와 같이 프로퍼티를 만들어서 접근하면 유용함.
/// </summary>
public class Game
{
    private static Game instance;

    public static Game Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new Game();
            }

            return instance;
        }
    }

    public GameInfo gameInfo { get; private set; }
    public GameInputManager inputManager { get; private set; }
    public ObstacleSpawnManager obstacleSpawnManager { get; private set; }
	public MonsterPatternDB monsterPatternDB{ get; private set;}
    public GameScene gameScene { get; private set; }
	public SoundManager soundManager{ get; private set;}


    public Game()
    {
        gameInfo = new GameInfo();
        inputManager = new GameInputManager();
        obstacleSpawnManager = new ObstacleSpawnManager();
		obstacleSpawnManager.Init ();
    }

    public void SetGameScene(GameScene scene)
    {
        gameScene = scene;
    }
}