using UnityEngine;
using UnityEngine.Tilemaps;

public class StageCreator : MonoBehaviour
{
    
    private TilesController tilesController;
    private RouteController routeController;
    private ActiveSonarController activeSonarController;
    public GameObject base_init;
    public GameObject base_end;
    public Transform basesMoveis;
    public Transform amplificadores;
    public GameObject backBase;
    public Tilemap tilemapActive;
    public GameObject parent;
    public GameObject backColl;
    public GameObject edge;

    void OnEnable()
    {

        tilesController = Camera.main.GetComponent<TilesController>();
        routeController = Camera.main.GetComponent<RouteController>();
        activeSonarController = Camera.main.GetComponent<ActiveSonarController>();
        routeController.edge = edge;
        routeController.baseEnd = base_end;

        routeController.baseInit = base_init;

        tilesController.bases.Add(getKey(base_init.transform), new TileBean(base_init, TileBean.BASE_INIT));
        tilesController.bases.Add(getKey(base_end.transform), new TileBean(base_end, TileBean.BASE_END));

        parent.transform.localScale = transform.localScale;

        activeSonarController.parent = parent;

        foreach (Transform go in basesMoveis)
        {
            go.gameObject.GetComponent<Drag>().parent = parent;
            go.gameObject.GetComponent<BaseMovelController>().parent = parent;
            
            GameObject bbase = Instantiate(backBase);
            bbase.transform.SetParent(backBase.transform.parent);
            bbase.transform.localScale = new Vector3(0.8660256f, 1f, 1f);
            bbase.transform.position = new Vector3(go.position.x, go.position.y, 1f);
            bbase.SetActive(true);
        }
        foreach (Transform a in amplificadores)
        {
            GameObject ampli = a.GetChild(0).gameObject;
            tilesController.bases.Add(getKey(a.gameObject.transform), new TileBean(ampli, TileBean.AMPLIFIC));
            activeSonarController.posAmplifics.Add(ampli);
        }

        Vector2 fisrt = new Vector2();
        bool achouFirst = false;
        foreach (Vector3Int localPlace in tilemapActive.cellBounds.allPositionsWithin)
        {
            if (tilemapActive.HasTile(localPlace))
            {
                Vector3 pos = tilemapActive.CellToWorld(localPlace);
                GameObject back = Instantiate(backColl);
                back.transform.SetParent(parent.transform);
                back.transform.position = pos;
                back.transform.localScale = new Vector3(0.8660256f, 1, 1);
                back.GetComponent<BackColliderController>().posBack = pos;
                back.layer = 10;
                back.tag = "back";
                GameObject obj = new GameObject();
                obj.tag = "baseActive";
                obj.layer = 8;
                obj.transform.SetParent(parent.transform);
                obj.transform.position = pos;
                CircleCollider2D cc = obj.AddComponent<CircleCollider2D>();
                cc.radius = 0.35f;
                cc.isTrigger = true;
                if (!achouFirst)
                {
                    achouFirst = true;
                    fisrt = obj.transform.position;
                    Current.firstPos = obj.transform.position;
                }
                string key = getKey(obj.transform);
                //if (!tilesController.bases.ContainsKey(key))
                //{
                    tilesController.mapaAtivo.Add(key, new TileBean(obj, TileBean.TILE_ACTIVE));
                    float tempDist = Vector2.Distance(fisrt, obj.transform.position);
                    if (tempDist < Current.minDist && tempDist > 0.1f)
                    {
                        Current.minDist = tempDist;
                    }
                //}
            }
        }
    }

    public static string getKey(Transform trans)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(trans.position, Vector2.zero);
        Vector2 posHit = new Vector2(0,0);
        bool temHit = false;
        foreach(RaycastHit2D hit in hits)
        {
            if(hit.collider.gameObject.tag == "baseActive" || hit.collider.gameObject.tag == "baseInit" || hit.collider.gameObject.tag == "baseEnd" || hit.collider.gameObject.tag == "baseMovel" || hit.collider.gameObject.tag == "baseFixa")
            {
                posHit = hit.collider.gameObject.transform.position;
                temHit = true;
                break;
            }
            
        }
        if (temHit)
        {
            return Round(Camera.main.WorldToScreenPoint(posHit));
        }
        else
        {
            return Round(Camera.main.WorldToScreenPoint(trans.position));
        }
        
    }

    private static string Round(Vector2 vector)
    {
        float dist1 = Mathf.Round(vector.x/10);
        float dist2 = Mathf.Round(vector.y/10);
        return dist1 + "/" + dist2;
    }
}
