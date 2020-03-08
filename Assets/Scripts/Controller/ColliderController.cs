using UnityEngine;

public class ColliderController : MonoBehaviour
{
    private bool isTriggered = false;
    private const float DELAY_ANIMA = 0.5f;
    private float timeAnima = DELAY_ANIMA;
    private bool isAnimated = false;
    private TilesController scriptTiles;
    private bool isBaseInit = false;
    private Vector2 posColl;
    public GameObject baseParent;
    private Drag scriptDrag;

    private void Start()
    {
        scriptTiles = Camera.main.GetComponent<TilesController>();
        scriptDrag = baseParent.GetComponent<Drag>();
    }

    private void Update()
    {
        if (isTriggered && !isAnimated && !isBaseInit)
        {
            timeAnima -= Time.deltaTime;
            if(timeAnima < 0 && !scriptDrag.isMouseUp)
            {
                isAnimated = true;
                scriptDrag.createCopy(posColl);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if((collision.gameObject.tag == "baseActive" || collision.gameObject.tag == "baseInit" || collision.gameObject.tag == "baseEnd") && collision.gameObject.GetHashCode() != gameObject.GetHashCode() && !isTriggered && !isBaseInit)
        {
            if (scriptTiles.bases.TryGetValue(StageCreator.getKey(collision.transform), out TileBean baseDestino))//se tem alguma base naquela posição
            {
                if (baseDestino.getType() == TileBean.AMPLIFIC)
                {
                    scriptDrag.destroyClones();
                    isBaseInit = true;
                    return;
                }
                //isBaseInit = baseDestino.getGameObject().GetComponent<Drag>().getHash() == transform.parent.GetComponent<Drag>().getHash(); //não deixa mostrar sonar se estiver na base init
            }
            posColl = collision.transform.position;
            timeAnima = DELAY_ANIMA;
            isTriggered = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.GetHashCode() != gameObject.GetHashCode())
        {
            scriptDrag.destroyClones();
            isTriggered = false;
            isAnimated = false;
            isBaseInit = false;
        }   
    }
    public void resetColl()
    {
        isTriggered = false;
        isAnimated = false;
        isBaseInit = false;
    }
}
