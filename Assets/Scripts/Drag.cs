using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CircleCollider2D))]

public class Drag : MonoBehaviour
{

    private TilesController scriptTiles;
    private BaseMovelController scriptBaseMovelController;
    private RouteController routeController;
    private int numReturnedCallback = 0;
    private int numAddCallback = 0;
    public GameObject baseMovel;
    private Animator animBaseMovel;
    private GameObject baseMovelCopy;
    private bool isDraged = false;
    private bool showingCopy = false;
    private int contActions = 0;
    public GameObject parent;
    private const float DELAY_ROTATE = 0.1f;
    public Vector3 posMouse;
    public const float DELAY_ANIMA = 0.3f;
    private float timeoutAnima = DELAY_ANIMA;
    public string key = "";
    Vector3 posMouseClick;
    private ColliderController scriptCollController;
    public bool isMouseUp = false;
    private bool isMostrandoAnima = false;
    private bool chamaAnima = false;

    public int getHash()
    {
        return gameObject.GetHashCode();
    }

    void Start()
    {
        
        scriptTiles = Camera.main.GetComponent<TilesController>();
        routeController = Camera.main.GetComponent<RouteController>();
        scriptTiles.bases.Add(StageCreator.getKey(transform), new TileBean(gameObject, TileBean.BASE_MOVEL));
        scriptBaseMovelController = gameObject.GetComponent<BaseMovelController>();
        scriptBaseMovelController.posStart = transform.position;
        animBaseMovel = baseMovel.GetComponent<Animator>();
        scriptCollController = baseMovel.GetComponent<ColliderController>();
        copy();
    }

    void OnMouseDown()
    {
        if (routeController.connectou) return;
        //if (Current.isSwipeControll && Current.isControll) return;
        if (!Current.isOnlyDrag && Current.isControll && scriptTiles.hashMovendo == gameObject.GetHashCode())
        {
            scriptTiles.hashMovendo = 0;
            if (Current.isSwipeControll)
            {
                scriptTiles.objMove.GetComponent<SwipeController>().someOut();
            }
            else
            {
                scriptTiles.objMove.GetComponent<MoveController>().someOut();
            }

        }
        else if (!Current.isOnlyDrag && Current.isControll && scriptTiles.hashMovendo != gameObject.GetHashCode())
        {
            Drag d;
            BaseMovelController bmc;
            if (Current.isSwipeControll)
            {
                d = scriptTiles.objMove.GetComponent<SwipeController>().getBaseMovel().GetComponent<Drag>();
                bmc = scriptTiles.objMove.GetComponent<SwipeController>().getBaseMovel().GetComponent<BaseMovelController>();
            }
            else
            {
                d = scriptTiles.objMove.GetComponent<MoveController>().getBaseMovel().GetComponent<Drag>();
                bmc = scriptTiles.objMove.GetComponent<MoveController>().getBaseMovel().GetComponent<BaseMovelController>();
            }
            if(d == null || bmc == null) return;
            Animator a = d.baseMovel.GetComponent<Animator>();
            if (Vector2.Distance(scriptTiles.objMove.transform.position, bmc.posStart) < 0.1f)
            {
                a.Play("zoom_out_no_a");
            }
            else
            {
                a.Play("zoom_out");
            }
            scriptTiles.hashMovendo = 0;
            if (Current.isSwipeControll)
            {
                scriptTiles.objMove.GetComponent<SwipeController>().someOut();
            }
            else
            {
                scriptTiles.objMove.GetComponent<MoveController>().someOut();
            }
            
        }

        if(contActions == 2)  contActions = 0;
        
        if (contActions == 0)
        {
            chamaAnima = false;
            scriptTiles.hashMovendo = gameObject.GetHashCode();
            destroyClones();
            scriptCollController.resetColl();
            isMouseUp = false;
            posMouseClick = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
            key = StageCreator.getKey(transform);
            routeController.resetConnections();
            animBaseMovel.Play("zoom_in");
            contActions = 1;
            scriptBaseMovelController.posInit = transform.position;
            isDraged = false;
            showingCopy = false;
        }
    }

    void setPos()
    {
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
        posMouse = new Vector3(curPosition.x, curPosition.y, -4);
    }

    void OnMouseDrag()
    {
        if (contActions == 1)
        {
            setPos();
            if(Vector2.Distance(posMouseClick, posMouse) >= Current.minDist/4 && !isDraged)
            {
                isDraged = true;
            }
            if (isDraged && !showingCopy)
            {
                showingCopy = true;
                baseMovelCopy.transform.position = new Vector3(scriptBaseMovelController.posInit.x, scriptBaseMovelController.posInit.y, 0.5f);
                baseMovelCopy.SetActive(true);
            }
            if (isDraged)
            {
                transform.position = posMouse;
            }
            else if(isMostrandoAnima && !Current.isOnlyDrag)
            {
                OnMouseUp();
            }
        }
    }
    
    void OnMouseUp()
    {
        if (contActions == 1)
        {
            contActions = 2;
            bool isStart = Vector2.Distance(transform.position, scriptBaseMovelController.posStart) < 0.1f;
            if (!Current.isOnlyDrag && ((!isDraged && isMostrandoAnima) || isStart))
            {
                RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.zero);
                foreach (RaycastHit2D hit in hits)
                {
                    if (hit.collider.gameObject.tag == "back")
                    {
                        gameObject.transform.position = hit.collider.gameObject.transform.position;
                        break;
                    }
                }
                scriptTiles.ChangePosition(key, gameObject, scriptBaseMovelController.posInit, scriptBaseMovelController.posStart, DELAY_ANIMA);
                contActions = 0;
                scriptTiles.hashMovendo = gameObject.GetHashCode();
                scriptTiles.objMove.SetActive(true);
                showingCopy = false;
                baseMovelCopy.SetActive(false);
                if (Current.isSwipeControll)
                {
                    scriptTiles.objMove.GetComponent<SwipeController>().setBaseMovel(gameObject);
                }
                else
                {
                    scriptTiles.objMove.GetComponent<MoveController>().setBaseMovel(gameObject);
                }
                
            }
            else if (!isDraged && !isMostrandoAnima)
            {
                mouseUpUtil();
                rotate();
                
            }
            else
            {
                mouseUpUtil();
                isDraged = false;
                RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.zero);
                foreach (RaycastHit2D hit in hits)
                {
                    if (hit.collider.gameObject.tag == "back")
                    {
                        gameObject.transform.position = hit.collider.gameObject.transform.position;
                        break;
                    }
                }
                scriptTiles.ChangePosition( key, gameObject, scriptBaseMovelController.posInit, scriptBaseMovelController.posStart, DELAY_ANIMA);
                chamaAnima = true;
            }
        }
    }
    
    public void rotate()
    {
        destroyClones();
        scriptBaseMovelController.Rotate(callbackRotate, baseMovelCopy, DELAY_ROTATE);
    }

    private void mouseUpUtil()
    {
        isMouseUp = true;
        destroyClones();
        contActions = 2;
        baseMovelCopy.SetActive(false);
        showingCopy = false;
    }
    
    void Update()
    {
        if (chamaAnima)
        {
            timeoutAnima -= Time.deltaTime;
            if(timeoutAnima < 0)
            {
                chamaAnima = false;
                zoonOutSonar();
            }
        }
        else
        {
            timeoutAnima = DELAY_ANIMA;
        }

    }

    private void zoonOutSonar()
    {
        if (Vector2.Distance(transform.position,scriptBaseMovelController.posStart) < 0.1f)
        {
            animBaseMovel.Play("zoom_out_no_a");
        }
        else
        {
            animBaseMovel.Play("zoom_out");
        }
        AnimaSonar();
    }

    private void AnimaSonar()
    {
        contActions = 2;
        numReturnedCallback = 0;
        numAddCallback = 0;

        foreach (var tile in scriptTiles.bases)
        {
            if (tile.Value.getType() == TileBean.BASE_MOVEL)
            {
                numAddCallback++;
                //int dest = tile.Value.getGameObject().GetComponent<Drag>().getHash();
                //int ori = getHash();
                tile.Value.getGameObject().GetComponent<BaseMovelController>().MouseUp(callbackPosition, false);//(ori == dest && anima)
            }
        }
    }

    public void callbackRotate()
    {
        if (!Current.isControll)
        {
            chamaAnima = true;
        }
        else
        {
            createCopy(transform.position);
        }
    }

    public void callbackPosition()
    {
        if (contActions == 2)
        {
            numReturnedCallback++;
            if (numAddCallback == numReturnedCallback)
            {
                StartCoroutine(delayCheck());
            }
        }
        else
        {
            numReturnedCallback = 0;
            numAddCallback = 0;
        }
    }

    IEnumerator delayCheck()
    {
        yield return new WaitForSeconds(0f);
        if (contActions == 2)
        {
            contActions = 0;
            routeController.checkRoute();
        }
    }

    public void createCopy(Vector2 pos)
    {
        if (isMouseUp) return;
        destroyClones();
        isMostrandoAnima = true;
        GameObject instance = Instantiate(gameObject);
        instance.transform.position = pos;
        instance.transform.SetParent(transform.parent);
        instance.transform.localScale = new Vector3(1f, 1f, 1f);
        instance.tag = "tagDestroyBase";
        instance.GetComponent<Drag>().baseMovel.GetComponent<Animator>().Play("some");// ALGO BRILHANTE
        Destroy(instance.transform.GetChild(1).gameObject);
        Destroy(instance.GetComponent<Drag>());
        Destroy(instance.transform.GetChild(1).gameObject);
        Destroy(instance.GetComponent<CircleCollider2D>());
        instance.SetActive(true);
        instance.GetComponent<BaseMovelController>().MouseUpMove(gameObject);
    }

    public void some()
    {
        chamaAnima = false;
        destroyClones();
        isMouseUp = true;
        contActions = 0;
        isMostrandoAnima = false;
        zoonOutSonar();
    }

    public void destroyClones()
    {
 
        isMostrandoAnima = false;
        if (scriptTiles.hashMovendo != gameObject.GetHashCode()) return;
        GameObject[] sonarsDestroy = GameObject.FindGameObjectsWithTag("sonarDestroy");
        foreach (GameObject sonarDestroy in sonarsDestroy)
        {
            //sonarDestroy.GetComponent<Animator>().Play("base_sonar_some");
            Destroy(sonarDestroy);
        }
        GameObject[] movelsDestroy = GameObject.FindGameObjectsWithTag("movelDestroy");
        foreach (GameObject movelDestroy in movelsDestroy)
        {
            Destroy(movelDestroy);
        }
        GameObject[] tagDestroyBase = GameObject.FindGameObjectsWithTag("tagDestroyBase");
        foreach (GameObject destroyBase in tagDestroyBase)
        {
            Destroy(destroyBase);
        }
    }

    private void copy()
    {
        //copy
        Color tempColor = baseMovel.GetComponent<SpriteRenderer>().color;
        tempColor.a = 0.2f;
        baseMovelCopy = Instantiate(baseMovel);
        baseMovelCopy.SetActive(false);
        //baseMovelCopy.layer = 0;
        baseMovelCopy.tag = "baseCopyTemp";
        baseMovelCopy.transform.position = transform.position;
        baseMovelCopy.transform.SetParent(parent.transform);
        baseMovelCopy.transform.localScale = new Vector3(0.8660256f, 1f, 1f);
        baseMovelCopy.GetComponent<SpriteRenderer>().color = tempColor;
        Destroy(baseMovelCopy.GetComponent<CircleCollider2D>());
        Destroy(baseMovelCopy.GetComponent<ColliderController>());
    }
}