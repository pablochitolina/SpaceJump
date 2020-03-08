using UnityEngine;

public class TileBean
{
    private GameObject obj;
    private string type;

    public const string BASE_INIT = "BASE_INIT";
    public const string BASE_END = "BASE_END";
    public const string BASE_MOVEL = "BASE_MOVEL";
    public const string TILE_ACTIVE = "TILE_ACTIVE";
    public const string EDGE = "EDGE";
    public const string AMPLIFIC = "AMPLIFIC";

    public TileBean(GameObject newObj, string newType)
    {
        obj = newObj;
        type = newType;
    }

    public Vector2 getPos()
    {
        return obj.transform.position;
    }

    public GameObject getGameObject()
    {
        return obj;
    }

    public string getType()
    {
        return type;
    }
}