using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeController : MonoBehaviour
{
    private GameObject baseMovel;

    private TilesController scriptTiles;
    private const float DELAY_SOME = 6;
    private float tempoSome = DELAY_SOME;
    private const float DELAY_CLICK = 0.1f;
    private bool isFirstClick = false;

    public GameObject ok;

    public GameObject arrows;

    void OnSwipeDetected(Swipe direction, Vector2 swipeVelocity)
    {
        if (Current.isOnlyDrag) return;
        if (!Current.isControll || baseMovel == null) return;
        if (direction == Swipe.LongClick)
        {
            some();
            return;
        }
        if (isFirstClick)
        {
            isFirstClick = false;
            return;
        }
        tempoSome = DELAY_SOME;
        string tag = getTag(direction);
        string key = StageCreator.getKey(baseMovel.transform);
        baseMovel.GetComponent<BaseMovelController>().posInit = baseMovel.transform.position;
        bool moveu = true;
        bool isStart = false;
        if (Vector2.Distance(baseMovel.GetComponent<BaseMovelController>().posStart, baseMovel.transform.position) < 0.1 
            && (tag == "upRight" || tag == "upLeft" || tag == "left" || tag == "right" || tag == "downRight" || tag == "downLeft"))
        {
            isStart = true;
            baseMovel.transform.position = Current.firstPos;
        }else if (tag == "rot")
        {
            baseMovel.GetComponent<Drag>().rotate();
        } else if (tag == "ok")
        {
            moveu = false;
            some();
        }
        if (moveu)
        {
            bool keep = false;
            int tries = 5;
            do {
                tries--;
                keep = false;
                if ((tag == "upRight" || tag == "upLeft" || tag == "left" || tag == "right" || tag == "downRight" || tag == "downLeft") && !isStart) baseMovel.transform.position = getProx(tag);
                RaycastHit2D[] hits = Physics2D.RaycastAll(baseMovel.transform.position, Vector2.zero);
                foreach (RaycastHit2D hit in hits)
                {
                    if (hit.collider.gameObject.tag == "back")
                    {
                        baseMovel.transform.position = hit.collider.gameObject.transform.position;
                        break;
                    }
                }
                if (scriptTiles.bases.TryGetValue(StageCreator.getKey(baseMovel.transform), out TileBean baseDestino))
                {
                    if(baseDestino.getType() == TileBean.BASE_MOVEL)
                    {
                        int dest = baseDestino.getGameObject().GetComponent<Drag>().getHash();
                        int bm = baseMovel.GetComponent<Drag>().getHash();
                        if(dest != bm)
                        {
                            keep = true;
                            isStart = false;
                        }
                    } else  if (baseDestino.getType() == TileBean.AMPLIFIC)
                    {
                        keep = true;
                        isStart = false;
                    }
                }
  
            } while (keep && tries > 0);

            if (scriptTiles.mapaAtivo.ContainsKey(StageCreator.getKey(baseMovel.transform)))
            {
                scriptTiles.ChangePosition(key, baseMovel, baseMovel.GetComponent<BaseMovelController>().posInit, baseMovel.GetComponent<BaseMovelController>().posStart, Drag.DELAY_ANIMA);
            }
            else
            {
                baseMovel.transform.position = new Vector3(baseMovel.GetComponent<BaseMovelController>().posInit.x, baseMovel.GetComponent<BaseMovelController>().posInit.y, -4);
            }
            
        }
        arrows.transform.position = new Vector3(baseMovel.transform.position.x, baseMovel.transform.position.y, -9);
    }

    private Vector2 getProx(string tag)
    {
        Vector2 dir = baseMovel.transform.position;

        if (tag == "upRight") dir = new Vector2(baseMovel.transform.position.x + 0.1f, baseMovel.transform.position.y + Current.minDist);
        if (tag == "upLeft") dir = new Vector2(baseMovel.transform.position.x - 0.1f, baseMovel.transform.position.y + Current.minDist);
        if (tag == "left") dir = new Vector2(baseMovel.transform.position.x - Current.minDist, baseMovel.transform.position.y);
        if (tag == "right") dir = new Vector2(baseMovel.transform.position.x + Current.minDist, baseMovel.transform.position.y);
        if (tag == "downRight") dir = new Vector2(baseMovel.transform.position.x + 0.1f, baseMovel.transform.position.y - Current.minDist);
        if (tag == "downLeft") dir = new Vector2(baseMovel.transform.position.x - 0.1f, baseMovel.transform.position.y - Current.minDist);

        return dir;
    }

    void Start()
    {
        SwipeManager.OnSwipeDetected += OnSwipeDetected;
        scriptTiles = Camera.main.GetComponent<TilesController>();
    }

    public void setBaseMovel(GameObject b)
    {
        Current.isControll = true;
        isFirstClick = true;
        tempoSome = DELAY_SOME;
        baseMovel = b;
        arrows.transform.position = new Vector3(baseMovel.transform.position.x, baseMovel.transform.position.y, -7);
        StartCoroutine(delayFirst());
        Camera.main.GetComponent<RouteController>().footer.SetActive(false);
    }

    IEnumerator delayFirst()
    {
        yield return new WaitForSeconds(0.1f);
        isFirstClick = false;

    }

    public GameObject getBaseMovel()
    {
        return baseMovel;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null)
            {
                if (hit.collider.gameObject.tag == "ok")
                {
                    Current.isControll = false;
                    ok.GetComponent<Animator>().Play("click");
                    clickUp();
                }
            }
        }

        if (Current.isControll)
        {
            tempoSome -= Time.deltaTime;
            if (tempoSome < 0) some();
        }

    }


    private void clickUp()
    {
       
        ok.GetComponent<Animator>().Play("clickUp");
        StartCoroutine(delayClick());
        
    }

    IEnumerator delayClick()
    {
        yield return new WaitForSeconds(DELAY_CLICK);
        some();
    }

        private void some()
    {
        Camera.main.GetComponent<RouteController>().footer.SetActive(true);
        baseMovel.GetComponent<Drag>().some();
        someOut();
    }

    public void someOut()
    {
        Current.isControll = false;
        gameObject.SetActive(false);
    }

    private string getTag(Swipe dir)
    {
        string tag = "ok";

        switch (dir) {
            case Swipe.Right:
                tag = "right";
                break;
            case Swipe.UpRight:
                tag = "upRight";
                break;
            case Swipe.UpLeft:
                tag = "upLeft";
                break;
            case Swipe.Left:
                tag = "left";
                break;
            case Swipe.DownLeft:
                tag = "downLeft";
                break;
            case Swipe.DownRight:
                tag = "downRight";
                break;
            case Swipe.None:
                tag = "rot";
                break;
            default:
                tag = "ok";
                break;
        }
        return tag;
    }
}
