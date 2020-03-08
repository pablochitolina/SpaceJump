using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class TilesController : MonoBehaviour
{ 
    public float distance = 1;
    public Dictionary<string, TileBean> mapaAtivo = new Dictionary<string, TileBean>();
    public Dictionary<string, TileBean> bases = new Dictionary<string, TileBean>();
    private RouteController routeController;
    public GameObject redSonar;
    public GameObject greenSonar;
    public GameObject blueSonar;
    public int hashMovendo;
    public GameObject buttons;
    public GameObject swipe;
    public GameObject objMove;
    
    void Start()
    {
        if (Current.isSwipeControll)
        {
            objMove = swipe;
        }
        else
        {
            objMove = buttons;
        }
        routeController = gameObject.GetComponent<RouteController>();
    }

    public bool ChangePosition( string keyOrigem, GameObject baseMovel, Vector2 posInit, Vector2 posStart, float delayAnima)
    {
        bool change = true;
        
        if (mapaAtivo.TryGetValue(StageCreator.getKey(baseMovel.transform), out TileBean tileDestino))//se tem alguma pos naquela posição
        {
            //tile destino é uma base movel
            if (bases.TryGetValue(StageCreator.getKey(baseMovel.transform), out TileBean baseDestino))
            {
                if (baseDestino.getType() == TileBean.AMPLIFIC ||
                    baseDestino.getType() == TileBean.BASE_END || baseDestino.getType() == TileBean.BASE_INIT)
                {
                    change = false;
                    baseMovel.transform.position = new Vector3(posInit.x, posInit.y, -3);
                }
                else
                {
                    int dest = baseDestino.getGameObject().GetComponent<Drag>().getHash();
                    int bm = baseMovel.GetComponent<Drag>().getHash();
                    if (dest != bm && baseDestino.getType() == TileBean.BASE_MOVEL)
                    {
                        TileBean auxOrigem = bases[keyOrigem];
                        bases.Remove(keyOrigem);
                        bases[StageCreator.getKey(baseMovel.transform)] = auxOrigem;
                        Vector3 start = new Vector3(baseDestino.getGameObject().GetComponent<BaseMovelController>().posStart.x, baseDestino.getGameObject().GetComponent<BaseMovelController>().posStart.y, -3);
                        baseDestino.getGameObject().transform.position = start;
                        bases[StageCreator.getKey(baseDestino.getGameObject().transform)] = baseDestino;
                        Animator anim = baseDestino.getGameObject().transform.GetChild(1).GetComponent<Animator>();
                        anim.Play("zoom_in");
                        StartCoroutine(delayAnimaStart(delayAnima, anim));
                    }
                    if (dest == bm)
                    {
                        change = false;
                        baseMovel.transform.position = new Vector3(posInit.x, posInit.y, -3); ;
                    }
                }
            }
            else
            {
                TileBean auxOrigem = bases[keyOrigem];
                bases.Remove(keyOrigem);
                bases.Add(StageCreator.getKey(baseMovel.transform), auxOrigem);
            }
        }
        else
        {
            baseMovel.transform.position = new Vector3(posStart.x, posStart.y, -3); ;
            TileBean aux = bases[keyOrigem];
            bases.Remove(keyOrigem);
            bases.Add(StageCreator.getKey(baseMovel.transform), aux);
            change = false;
        }
        baseMovel.transform.position = new Vector3(baseMovel.transform.position.x, baseMovel.transform.position.y, -3);
        return change;
    }


    IEnumerator delayAnimaStart(float time, Animator anim)
    {
        yield return new WaitForSeconds(time);
        anim.Play("zoom_out_no_a");
    }
}
