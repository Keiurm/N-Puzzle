using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class StageManager : MonoBehaviour
{
    [SerializeField] TextAsset stageAsset;
    [SerializeField] GameObject[] prefabs;

    public enum STAGE_TYPE
    {
        WALL,
        GROUND,
        BLOCK_POINT,
        BLOCK,
        PLAYER
    }
    STAGE_TYPE[,] stageTable;

    private PlayerManager playerManager;

    public PlayerManager GetPlayerManager()
    {
        return playerManager;
    }

    public void LoadStageData()
    {
        string[] lines = stageAsset.text.Split(new[] { '\n', '\r' });
        int rows = lines.Length;
        int colums = lines[0].Split(new[] { ',' }).Length;
        stageTable = new STAGE_TYPE[rows, colums];

        for (int x = 0; x < rows; x++)
        {
            string[] values = lines[x].Split(new[] { ',' });
            for (int y = 0; y < colums; y++)
            {
                stageTable[x, y] = (STAGE_TYPE)int.Parse(values[y]);
                Debug.Log($"{x}:{y} => {stageTable[x, y]}");
            }
        }
    }
    float tileSize;
    Vector2 centerPosition;
    public void CreateStage()
    {
        tileSize = prefabs[0].GetComponent<SpriteRenderer>().bounds.size.x;
        centerPosition.x = (stageTable.GetLength(0) / 2) * tileSize;
        centerPosition.y = (stageTable.GetLength(1) / 2) * tileSize;

        for (int x = 0; x < stageTable.GetLength(0); x++)
        {
            for (int y = 0; y < stageTable.GetLength(1); y++)
            {
                Vector2Int position = new Vector2Int(y, x);

                STAGE_TYPE groundStageType = STAGE_TYPE.GROUND;
                GameObject groundObj = Instantiate(prefabs[(int)groundStageType]);
                groundObj.transform.position = GetScreenPositionFromTileTable(position);

                STAGE_TYPE stageType = stageTable[x, y];
                GameObject obj = Instantiate(prefabs[(int)stageType]);
                obj.transform.position = GetScreenPositionFromTileTable(position);

                if (stageType == STAGE_TYPE.PLAYER)
                {
                    playerManager = obj.GetComponent<PlayerManager>();
                    moveObjPositionOnStage.Add(obj, position);
                }
                if (stageType == STAGE_TYPE.BLOCK)
                {
                    moveObjPositionOnStage.Add(obj, position);
                }
            }
        }
    }
    public Vector2 GetScreenPositionFromTileTable(Vector2Int position)
    {
        return new Vector2(position.x * tileSize - centerPosition.x,
                           -(position.y * tileSize - centerPosition.y));
    }

    public void SetObjectOnStage(GameObject obj, Vector2Int pos)
    {
        moveObjPositionOnStage[obj] = pos;
    }
    private Dictionary<GameObject, Vector2Int> moveObjPositionOnStage = new Dictionary<GameObject, Vector2Int>();

    public void SetObjPositionOnStage(GameObject obj, Vector2Int pos)
    {
        moveObjPositionOnStage[obj] = pos;
    }
    public Vector2Int GetObjPosition(GameObject obj)
    {
        return moveObjPositionOnStage[obj];
    }
    public bool IsWall(Vector2Int position)
    {
        if (stageTable[position.y, position.x] == STAGE_TYPE.WALL)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool IsBlock(Vector2Int position)
    {
        if (stageTable[position.y, position.x] == STAGE_TYPE.BLOCK)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private GameObject GetObjectByPosition(Vector2 position)
    {
        foreach (var objPosition in moveObjPositionOnStage)
        {
            if (objPosition.Value == position)
            {
                return objPosition.Key;
            }
        }
        return null;
    }
    public void UpdateObjectPosition(Vector2Int nowPosition, Vector2Int nextPosition,STAGE_TYPE stageType)
    {
        GameObject targetBlock = GetObjectByPosition(nowPosition);
        targetBlock.transform.position = GetScreenPositionFromTileTable(nextPosition);

        moveObjPositionOnStage[targetBlock] = nextPosition;
        stageTable[nextPosition.y, nextPosition.x] = stageType;
    }
    public void UpdateStageTableForPlayer(Vector2Int nowPosition, Vector2Int nextPosition)
    {
        stageTable[nowPosition.y, nowPosition.x] = STAGE_TYPE.GROUND;
        stageTable[nextPosition.y, nextPosition.x] = STAGE_TYPE.PLAYER;
    }

}
