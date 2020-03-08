using UnityEngine;
using System.Collections;

public class SonarHexController : MonoBehaviour
{

    private TilesController scriptTiles;
    private TilesController cscriptTiles;
    private RouteController scriptRouteController;
    private string setorCor = "";
    
    void Start()
    {
        scriptTiles = Camera.main.GetComponent<TilesController>();
        scriptRouteController = Camera.main.GetComponent<RouteController>();
    }

    public bool AnimaMove( GameObject objOrigem)
    {
        
        cscriptTiles = Camera.main.GetComponent<TilesController>();
        bool anima = false;
        if (cscriptTiles.mapaAtivo.TryGetValue(StageCreator.getKey(transform), out TileBean mapa))
        {
            anima = true;
            if (cscriptTiles.bases.TryGetValue(StageCreator.getKey(transform), out TileBean ba) && ba.getType() != TileBean.AMPLIFIC && ba.getType() != TileBean.BASE_END && ba.getType() != TileBean.BASE_INIT)
            {
                int dest = ba.getGameObject().GetComponent<Drag>().getHash();
                int ori = objOrigem.GetComponent<Drag>().getHash();
                if (ba.getType() == TileBean.BASE_MOVEL && ori != dest) return false; 
            }
        }

        if(anima) mostrarAnima(true, cscriptTiles, objOrigem);

        return anima;
    }

    public void Anima(System.Action<SonarHexBean> callback, GameObject objOrigem, bool mostraSonar)
    {
        
        bool isTile = false, isBase = false, isHit = false;

        isTile = scriptTiles.mapaAtivo.ContainsKey(StageCreator.getKey(transform));
        if (scriptTiles.bases.TryGetValue(StageCreator.getKey(transform), out TileBean ba) && ba.getType() != TileBean.AMPLIFIC)
        {
            isBase = true;
            if (isValidHit(objOrigem, ba.getGameObject(), ba.getType()))
            {
                isHit = true;
                scriptRouteController.addConnection(objOrigem, ba.getGameObject(), gameObject.tag, setorCor);
                //Debug.Log("origem " + objOrigem.name + " conectou com " + ba.getGameObject().name + " na cor " + gameObject.tag);
            }
        }

        //if (isTile && !isBase && mostraSonar) gameObject.GetComponent<Animator>().Play("base_sonar");
        if (isTile && !isBase && mostraSonar && objOrigem.GetComponent<Drag>().isMouseUp)
        {
            mostrarAnima(false, scriptTiles, objOrigem);
        }

        ActiveSonarBean sonar = null;

        if (isTile && !isBase && !isHit) sonar = new ActiveSonarBean(gameObject.transform.position, gameObject.tag, gameObject.transform.rotation);

        StartCoroutine(delayCallback(callback, new SonarHexBean(isTile && !isBase, isHit, sonar)));
    }

    IEnumerator delayCallback(System.Action<SonarHexBean> callback, SonarHexBean sonar)
    {
        yield return new WaitForSeconds(0f);
        callback(sonar);
    }

    void mostrarAnima(bool isMoving, TilesController script, GameObject objOrigem)
    {
        GameObject sonarCopy = Instantiate(gameObject);

        if (sonarCopy != null)
        {
            Color color = sonarCopy.GetComponent<SpriteRenderer>().color;
            color.a = 0.5f;
            sonarCopy.GetComponent<SpriteRenderer>().color = color;
            sonarCopy.SetActive(true);
            sonarCopy.transform.SetParent(objOrigem.GetComponent<BaseMovelController>().parent.transform);
            sonarCopy.transform.position = transform.position;
            sonarCopy.transform.rotation = gameObject.transform.rotation;
            sonarCopy.transform.localScale = new Vector3(1f,1f,1f);
            sonarCopy.gameObject.tag = "sonarDestroy";
            if (isMoving)
            {
                sonarCopy.GetComponent<Animator>().Play("base_sonar_move");
            }
            else
            {
                sonarCopy.GetComponent<Animator>().Play("base_sonar_some");
                Destroy(sonarCopy, 0.2f);

            }
        }
    }

    private bool isValidHit(GameObject objOrigem, GameObject objHit, string type)
    {
        setorCor = "";
        Vector2 posOrigem = objOrigem.transform.position;
        string ru = "", r = "", rd = "", ld = "", l = "", lu = "";
        if (type == TileBean.BASE_MOVEL)
        {
            BaseMovelController script = objHit.GetComponent<BaseMovelController>();
            ru = script.ru;
            r = script.r;
            rd = script.rd;
            ld = script.ld;
            l = script.l;
            lu = script.lu;
        }
        if (type == TileBean.BASE_END || type == TileBean.BASE_INIT)
        {
            BaseFixaController script = objHit.GetComponent<BaseFixaController>();
            ru = script.ru;
            r = script.r;
            rd = script.rd;
            ld = script.ld;
            l = script.l;
            lu = script.lu;
        }
        if (isBaseMovelRU(posOrigem) && isSameLabel(ru))
        {
            setorCor = "ld" + ru;
            return true;
        }
        if (isBaseMovelR(posOrigem) && isSameLabel(r))
        {
            setorCor = "l" + r;
            return true;
        }
        if (isBaseMovelRD(posOrigem) && isSameLabel(rd))
        {
            setorCor = "lu" + rd;
            return true;
        }
        if (isBaseMovelLD(posOrigem) && isSameLabel(ld))
        {
            setorCor = "ru" + ld;
            return true;
        }
        if (isBaseMovelL(posOrigem) && isSameLabel(l))
        {
            setorCor = "r" + l;
            return true;
        }
        if (isBaseMovelLU(posOrigem) && isSameLabel(lu))
        {
            setorCor = "rd" + lu;
            return true;
        }
        return false;
    }

    private bool isSameLabel(string label){
        return (gameObject.tag == "redSonar" && label == "r") || (gameObject.tag == "greenSonar" && label == "g") || (gameObject.tag == "blueSonar" && label == "b");
    }

    private bool isBaseMovelRU(Vector2 posBaseMovel)
    {
        return (posBaseMovel.x > transform.position.x && posBaseMovel.y > transform.position.y) && !sameRowX(posBaseMovel.y);
    }

    private bool isBaseMovelR(Vector2 posBaseMovel)
    {
        return posBaseMovel.x > transform.position.x && sameRowX(posBaseMovel.y);
    }

    private bool isBaseMovelRD(Vector2 posBaseMovel)
    {
        return (posBaseMovel.x > transform.position.x && posBaseMovel.y < transform.position.y) && !sameRowX(posBaseMovel.y);
    }

    private bool isBaseMovelLD(Vector2 posBaseMovel)
    {
        return (posBaseMovel.x < transform.position.x && posBaseMovel.y < transform.position.y) && !sameRowX(posBaseMovel.y);
    }

    private bool isBaseMovelL(Vector2 posBaseMovel)
    {
        return (posBaseMovel.x < transform.position.x && sameRowX(posBaseMovel.y));
    }

    private bool isBaseMovelLU(Vector2 posBaseMovel)
    {
        return (posBaseMovel.x < transform.position.x && posBaseMovel.y > transform.position.y) && !sameRowX(posBaseMovel.y);
    }

    private bool sameRowX(float y)
    {
        return (y < transform.position.y + 0.1f && y > transform.position.y - 0.1f);
    }
}
