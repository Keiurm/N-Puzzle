using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManeger : MonoBehaviour
{
    [SerializeField] StageManager stageManager;
    PlayerManager playerManager;
    // Start is called before the first frame update
    enum DIRECTION
    {
        UP,
        DOWN,
        RIGHT,
        LEFT
    }
    void Start()
    {
        stageManager.LoadStageData();
        stageManager.CreateStage();
        playerManager = stageManager.GetPlayerManager();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Debug.Log("上矢印が押されました。");
            Vector2Int currentPlayerPosition = stageManager.GetObjPosition(playerManager.gameObject);
            Vector2Int nextPlayerPosition = currentPlayerPosition - Vector2Int.up;
            playerManager.Move(stageManager.GetScreenPositionFromTileTable(nextPlayerPosition));
            stageManager.SetObjectOnStage(playerManager.gameObject, nextPlayerPosition);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Debug.Log("下矢印が押されました。");
            Vector2Int currentPlayerPosition = stageManager.GetObjPosition(playerManager.gameObject);
            Vector2Int nextPlayerPosition = currentPlayerPosition - Vector2Int.down;
            playerManager.Move(stageManager.GetScreenPositionFromTileTable(nextPlayerPosition));
            stageManager.SetObjectOnStage(playerManager.gameObject, nextPlayerPosition);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Debug.Log("左矢印が押されました。");
            Vector2Int currentPlayerPosition = stageManager.GetObjPosition(playerManager.gameObject);
            Vector2Int nextPlayerPosition = currentPlayerPosition + Vector2Int.left;
            playerManager.Move(stageManager.GetScreenPositionFromTileTable(nextPlayerPosition));
            stageManager.SetObjectOnStage(playerManager.gameObject, nextPlayerPosition);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Debug.Log("右矢印が押されました。");
            Vector2Int currentPlayerPosition = stageManager.GetObjPosition(playerManager.gameObject);
            Vector2Int nextPlayerPosition = currentPlayerPosition + Vector2Int.right;
            playerManager.Move(stageManager.GetScreenPositionFromTileTable(nextPlayerPosition));
            stageManager.SetObjectOnStage(playerManager.gameObject, nextPlayerPosition);
        }
    }
    void MoveToNextPosition(DIRECTION direction)
    {
        Vector2Int currentPlayerPosition = stageManager.GetObjPosition(playerManager.gameObject);
        Vector2Int nextPlayerPosition = GetNextPosition(currentPlayerPosition, direction);
        if (stageManager.IsWall(nextPlayerPosition))
        {
            return;
        }
        else if (stageManager.IsBlock(nextPlayerPosition))
        {
            Vector2Int nextBlockPosition = GetNextPosition(nextPlayerPosition, direction);
            stageManager.UpdateObjectPosition(nextPlayerPosition, nextBlockPosition,
            StageManager.STAGE_TYPE.BLOCK);
        }
        stageManager.UpdateStageTableForPlayer(currentPlayerPosition, nextPlayerPosition);
        playerManager.Move(stageManager.GetScreenPositionFromTileTable(nextPlayerPosition));
        stageManager.SetObjPositionOnStage(playerManager.gameObject, nextPlayerPosition);
    }  
    Vector2Int GetNextPosition(Vector2Int currentPosition, DIRECTION direction)
    {
        switch (direction)
        {
            case DIRECTION.UP:
                return currentPosition - Vector2Int.up;
                break;
            case DIRECTION.DOWN:
                return currentPosition + Vector2Int.down;
                break;
            case DIRECTION.LEFT:
                return currentPosition + Vector2Int.left;
                break;
            case DIRECTION.RIGHT:
                return currentPosition + Vector2Int.right;
                break;
            default:
                return currentPosition;
        }
    }
}
