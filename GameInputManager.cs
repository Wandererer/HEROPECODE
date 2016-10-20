using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/*게 임 터 치 또 는 클 릭 관 리 */

public class GameInputManager : MonoBehaviour {

    public Game game
    {
        get { return Game.Instance; }
    }

    private static readonly int OBSTACLE_LAYER_MASK = 1 << LayerMask.NameToLayer("Obstacle");

    private Vector2 lastTouchPosition;
    private Vector2 newTouchPosition;
    private Vector2 firstDragTouchPosition;
	private Vector3 offset;
	private Vector3 screentPosition;

	private bool isMouseDown=false;
    public bool isMousePressed=true;
	GameObject dragInterraction;

    private List<Obstacle> dragObstacles = new List<Obstacle>();

    public void UpdateInput()
	{
#if UNITY_EDITOR
        UpdateMouseInput();
#else
        UpdateTouchInput();
#endif
	}

    private void UpdateMouseInput()
	{
		lastTouchPosition = newTouchPosition;
		newTouchPosition = Input.mousePosition;

			if (Input.GetMouseButtonDown (0)) {

				OnTouchDown ();
			}

			if (Input.GetMouseButtonUp (0)) {

				isMousePressed = false;
		
				if (Vector2.Distance (firstDragTouchPosition, newTouchPosition) > 10f) {
					OnDragTouchUp ();
				} else {
					OnTouchUp ();
				}
			}

			if (isMousePressed && lastTouchPosition != newTouchPosition) {
				OnDrag ();
			}

	}

    private void UpdateTouchInput()
    {
        if (Input.touchCount == 0)
        {
            isMousePressed = false;
            return;
        }
        Touch touch = Input.GetTouch(0);

        lastTouchPosition = newTouchPosition;
        newTouchPosition = touch.position;

        switch (touch.phase)
        {
            case TouchPhase.Began:
                OnTouchDown();
                break;
            case TouchPhase.Moved:
                if (isMousePressed == false)
                {
                    break;
                }

                OnDrag();
                break;
            case TouchPhase.Ended:

                if (isMousePressed == false)
                {
                    break;
                }

                isMousePressed = false;

                if (Vector2.Distance(firstDragTouchPosition, newTouchPosition) > 10f)
                {
                    OnDragTouchUp();
                }
                else
                {
                    OnTouchUp();
                }
                break;
            case TouchPhase.Stationary:
                break;
            case TouchPhase.Canceled:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void OnTouchDown()
    {
        if (UICamera.isOverUI)
        {
            return;
        }

		isMouseDown = true;

        firstDragTouchPosition = newTouchPosition;

        isMousePressed = true;
    }

    private void OnTouchUp()
    {
        List<Obstacle> tempObstacleList=RaycastObstaclesFromScreenPos();
		DeleteLineRendererFunction ();
		isMouseDown = false;

        foreach (Obstacle obstacle in RaycastObstaclesFromScreenPos())
        {
            if (obstacle.isDragType)
            {
				//Debug.Log ("Drag");
                continue;
            }

            game.gameScene.ShotObstacle(obstacle);

            SpawnEffect("Prefabs/HitEffect");
            return;
        }

		isMousePressed = false;

		game.gameScene.StartTapInput();

        SpawnEffect("Prefabs/TouchEffect");
    }

    private void SpawnEffect(string path)
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(newTouchPosition);
        pos.z = -5;
        GameObject.Instantiate(Resources.Load(path), pos, Quaternion.identity);
    }
		
    private void OnDrag()
    {
        foreach (Obstacle obstacle in RaycastObstaclesFromScreenPos())
        {
			
            if (obstacle.isDragType == false)
            {
                continue;
            }

            if (dragObstacles.Find(x => x.Equals(obstacle)) == null)
			{		
				dragInterraction = GameObject.Find ("DragInterraction");
	
				if (dragInterraction == null && isMouseDown) {
					dragInterraction = (GameObject)Instantiate (Resources.Load ("prefabs/DragInterraction"), obstacle.gameObject.transform.position, Quaternion.identity);

					obstacle.gameObject.AddComponent<Lightning> ();
					Lightning light = obstacle.gameObject.GetComponent<Lightning> ();
					light.Target = dragInterraction.transform;
					light.Steps = 5;
					light.Width = 1;
					light.Delay = 0.1f;
					isMouseDown = false;
					dragInterraction.GetComponent<DragIntteration> ().light = light;
					dragInterraction.name = "DragInterraction";

					Game.Instance.gameScene.AddLightAndRendererList (obstacle.gameObject, light);
				}


                dragObstacles.Add(obstacle);
            }
			else
			{
				//TODO : linerederer target 설 정 
		
			}
        }

		if(dragInterraction!=null && !isMouseDown)
		{
			Vector3 pos = Camera.main.ScreenToWorldPoint(newTouchPosition);
			pos.z = -1;
			dragInterraction.transform.position = pos;
		}
    }

	private void DeleteLineRendererFunction()
	{
		game.gameScene.DestroyLightningAndResetRenderer ();
		DestroyImmediate (dragInterraction);
		game.gameScene.AddLineRenerer ();
	}

    private void OnDragTouchUp()
    {
		DeleteLineRendererFunction ();
		isMouseDown = false;
        if (game.gameScene.IsExistObstacle() == false)
        {

            OnTouchUp();
            return;
        }
	

        game.gameScene.ShotDragObstacles(dragObstacles);
        dragObstacles.Clear();
    }

    public List<Obstacle> RaycastObstaclesFromScreenPos()
    {
        Ray ray = Camera.main.ScreenPointToRay(newTouchPosition);
        RaycastHit[] raycastHits = Physics.RaycastAll(ray, Mathf.Infinity, OBSTACLE_LAYER_MASK);
        List<Obstacle> collisionObjects = new List<Obstacle>();
        foreach (RaycastHit hit in raycastHits)
        {
            Obstacle obstacle = hit.collider.gameObject.GetComponent<Obstacle>();

            if (obstacle != null && obstacle.isAlive)
            {
                collisionObjects.Add(obstacle);
            }

            CheckHitItem(hit);//아이템 클릭시
        }

        return collisionObjects;
    }

   private void CheckHitItem(RaycastHit hit)
   {
       if(hit.collider!=null && hit.collider.tag=="Item")
       { //아이템 효과 추가
         //  Debug.Log("Hit");
           Item item=hit.collider.GetComponent<Item>();
           int itemNumber=item.itemID;
           hit.collider.GetComponent<SpriteRenderer>().sprite=item.pressedSprite;
           ItemSystemManager.Instance.ItemEffectStart(itemNumber);
		   hit.collider.GetComponent<Item> ().SetIsUseFalse ();
       }
   }

}
