using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RouteController : MonoBehaviour
{
    public GameObject baseInit;
    public GameObject baseEnd;
    private ActiveSonarController scriptActiveSonarController;
    public GameObject menu;
    public GameObject footer;
    public GameObject header;
    private List<ConnectionBean> connections = new List<ConnectionBean>();
    private List<GameObject> anim = new List<GameObject>();
    private string[] colours = { SonarHexBean.RED, SonarHexBean.GREEN, SonarHexBean.BLUE };
    public bool connectou = false;
    private List<Vector2> pontos = new List<Vector2>();
    public GameObject edge;
    private int totalAmpli = 0;

    private void Start()
    {
        scriptActiveSonarController = gameObject.GetComponent<ActiveSonarController>();
    }

    private bool percorreCon(ConnectionBean con, Swipe dir)
    {

        bool conectou = false;
        ConnectionBean c = connections.Find(x => con.getHashDestino() == x.getHashOrigem() && x.getHashDestino() != con.getHashOrigem() && x.getColor() == con.getColor());
        if (c != null)
        {
            pontos.Add(c.getOrigem().transform.position);
            Swipe para = getDir(c.getOrigem(), c.getColor(), getDirInvertido(dir));
            if (c.getHashDestino() == baseEnd.GetHashCode())
            {
                //Debug.Log(c.getOrigem().name + " de " + para + " para " + getDirInvertido(getDir(baseEnd, c.getColor(), Swipe.None)));
                totalAmpli += somaAmplis(c.getOrigem().transform.position, getDirInvertido(getDir(baseEnd, c.getColor(), Swipe.None)));
                return true;
            }
           // Debug.Log(c.getOrigem().name + " de " + para + " para " + getDirInvertido(dir));
            totalAmpli += somaAmplis(c.getOrigem().transform.position, getDirInvertido(dir));
            conectou = percorreCon(c, para);
        }
        return conectou;
    }

    private Swipe getDirInvertido(Swipe dir)
    {
        Swipe c = Swipe.None;
        if (dir == Swipe.UpRight) c = Swipe.DownLeft;
        if (dir == Swipe.Right) c = Swipe.Left;
        if (dir == Swipe.DownRight) c = Swipe.UpLeft;
        if (dir == Swipe.DownLeft) c = Swipe.UpRight;
        if (dir == Swipe.Left) c = Swipe.Right;
        if (dir == Swipe.UpLeft) c = Swipe.DownRight;
        return c;
    }

    public void checkRoute()
    {
        List<ConnectionBean> listCon = new List<ConnectionBean>();
        List<HashColor> hashColorInit = new List<HashColor>();
        List<HashColor> hashColorEnd = new List<HashColor>();
        connectou = false;
        Dictionary<string, GameObject> edges = new Dictionary<string, GameObject>();

        foreach (ConnectionBean con in connections)
        {
            if (con.getHashDestino() == baseInit.GetHashCode()) // verificação para poder adicionar apenas 1 por cor
            {
                hashColorInit.Add(new HashColor(con.getHashOrigem(), con.getColor()));
            }
            if (con.getHashDestino() == baseEnd.GetHashCode())
            {
                hashColorEnd.Add(new HashColor(con.getHashOrigem(), con.getColor()));
            }
        }
        foreach (string color in colours)
        {
            totalAmpli = 0;
            pontos = new List<Vector2>();
            if (!(hashColorInit.Exists(x => x.getColor() == color) && hashColorEnd.Exists(x => x.getColor() == color))) continue;
            List<int> tiles = new List<int>();
            int hashOrigem = hashColorInit.Find(x => x.getColor() == color).getHash();//tenho o hash da base que connectou com a init
            ConnectionBean conOrigem = connections.Find(x => x.getHashOrigem() == hashOrigem && x.getHashDestino() != baseInit.GetHashCode() && x.getColor() == color); // acha base 1
            bool conectouTentativa = false;
            if (conOrigem != null)
            {
                Swipe d = getDir(baseInit, color, Swipe.None);//sai de ini
                totalAmpli += somaAmplis(baseInit.transform.position, d);

                //Debug.Log(conOrigem.getOrigem().name + " de " + getDirInvertido(d) + " para " + getDir(conOrigem.getOrigem(), color, getDirInvertido(d)));
                totalAmpli += somaAmplis(conOrigem.getOrigem().transform.position, getDir(conOrigem.getOrigem(), color, getDirInvertido(d)));

                pontos.Add(conOrigem.getOrigem().transform.position);
                if (conOrigem.getHashDestino() == baseEnd.GetHashCode())
                {
                    conectouTentativa = true;
                }
                else
                {
                    conectouTentativa = percorreCon(conOrigem, d);
                }

            }
            if (conectouTentativa)
            {
                //Debug.Log("total ampli " + totalAmpli);
                connectou = true;
                edges.Add(color, createEdge(color));
                scriptActiveSonarController.AnimSonar(null, 0, true, null);
                foreach (ConnectionBean con in connections)
                {
                    if (con.getColor() == color) con.getOrigem().GetComponent<BaseMovelController>().playAnim(con.getSetorCorOrigem());
                }
            }
        }
        if (connectou)
        {
            footer.SetActive(false);
            header.SetActive(false);
            if (Current.isSwipeControll)
            {
                if (Camera.main.GetComponent<TilesController>().objMove != null) Camera.main.GetComponent<TilesController>().objMove.GetComponent<SwipeController>().someOut();
            }
            else
            {
                if (Camera.main.GetComponent<TilesController>().objMove != null) Camera.main.GetComponent<TilesController>().objMove.GetComponent<MoveController>().someOut();
            }

            StartCoroutine(delayMostraMenu(edges));
        }
        else
        {
            scriptActiveSonarController.AnimSonar(null, 0, false, null);
            foreach (ConnectionBean con in connections)
            {
                con.getOrigem().GetComponent<BaseMovelController>().playAnim(con.getSetorCorOrigem());
            }
        }
    }

    private int somaAmplis(Vector2 posInit, Swipe dir)
    {
        int contAmpli = 0;
        int max = 20;
        Vector2 nextPos = posInit;
        while (max > 0)
        {
            max--;
            if (dir == Swipe.UpRight) nextPos = new Vector2(nextPos.x + (Current.minDist / 2), nextPos.y + Current.minDist);
            if (dir == Swipe.UpLeft) nextPos = new Vector2(nextPos.x - (Current.minDist / 2), nextPos.y + Current.minDist);
            if (dir == Swipe.Left) nextPos = new Vector2(nextPos.x - Current.minDist, nextPos.y);
            if (dir == Swipe.Right) nextPos = new Vector2(nextPos.x + Current.minDist, nextPos.y);
            if (dir == Swipe.DownRight) nextPos = new Vector2(nextPos.x + (Current.minDist / 2), nextPos.y - Current.minDist);
            if (dir == Swipe.DownLeft) nextPos = new Vector2(nextPos.x - (Current.minDist / 2), nextPos.y - Current.minDist);

            RaycastHit2D[] hits = Physics2D.RaycastAll(nextPos, Vector2.zero);
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider.gameObject.tag == "ampli")
                {
                    contAmpli++;
                }
                if (hit.collider.gameObject.tag == "baseInner" || hit.collider.gameObject.tag == "baseInit" || hit.collider.gameObject.tag == "baseEnd")
                {
                    break;
                }

            }

        }
        return contAmpli;

    }

    private Swipe getDir(GameObject b, string color, Swipe de)
    {
        string c = "";
        if (color == "redSonar") c = "r";
        if (color == "greenSonar") c = "g";
        if (color == "blueSonar") c = "b";

        Swipe dir = Swipe.None;
        if (de == Swipe.None)
        {
            if (b.GetComponent<BaseFixaController>().ru == c && de != Swipe.UpRight) dir = Swipe.UpRight;
            if (b.GetComponent<BaseFixaController>().r == c && de != Swipe.Right) dir = Swipe.Right;
            if (b.GetComponent<BaseFixaController>().rd == c && de != Swipe.DownRight) dir = Swipe.DownRight;
            if (b.GetComponent<BaseFixaController>().ld == c && de != Swipe.DownLeft) dir = Swipe.DownLeft;
            if (b.GetComponent<BaseFixaController>().l == c && de != Swipe.Left) dir = Swipe.Left;
            if (b.GetComponent<BaseFixaController>().lu == c && de != Swipe.UpLeft) dir = Swipe.UpLeft;
        }
        else
        {
            if (b.GetComponent<BaseMovelController>().ru == c && de != Swipe.UpRight) dir = Swipe.UpRight;
            if (b.GetComponent<BaseMovelController>().r == c && de != Swipe.Right) dir = Swipe.Right;
            if (b.GetComponent<BaseMovelController>().rd == c && de != Swipe.DownRight) dir = Swipe.DownRight;
            if (b.GetComponent<BaseMovelController>().ld == c && de != Swipe.DownLeft) dir = Swipe.DownLeft;
            if (b.GetComponent<BaseMovelController>().l == c && de != Swipe.Left) dir = Swipe.Left;
            if (b.GetComponent<BaseMovelController>().lu == c && de != Swipe.UpLeft) dir = Swipe.UpLeft;
        }




        return dir;
    }

    private GameObject createEdge(string color)
    {
        List<Vector2> newVerticies = new List<Vector2>();
        newVerticies.Add(baseInit.transform.position);
        newVerticies.AddRange(pontos);
        newVerticies.Add(baseEnd.transform.position);
        GameObject edg = Instantiate(edge);
        edg.layer = 11;
        EdgeCollider2D edgColl = edg.AddComponent(typeof(EdgeCollider2D)) as EdgeCollider2D;
        edgColl.points = newVerticies.ToArray();
        edgColl.isTrigger = true;
        edg.transform.localScale = new Vector3(1, 1, 1);
        edg.transform.position = new Vector3(0, 0, 0);
        return edg;
    }

    public int numBonus = 0;
    IEnumerator delayMostraMenu(Dictionary<string, GameObject> edges)
    {
        string cor = "";
        yield return new WaitForSeconds(1f);
        GameObject[] sonarsDestroy = GameObject.FindGameObjectsWithTag("sonarDestroy");
        foreach (GameObject sonarDestroy in sonarsDestroy)
        {
            sonarDestroy.GetComponent<Animator>().Play("base_sonar_expande");
            Destroy(sonarDestroy, 0.3f);
        }
        GameObject[] movelsDestroy = GameObject.FindGameObjectsWithTag("movelDestroy");
        foreach (GameObject movelDestroy in movelsDestroy)
        {
            Destroy(movelDestroy);
        }
        foreach (KeyValuePair<string, GameObject> entry in edges)
        {
            int count = entry.Value.GetComponent<EdgeController>().count;
            scriptActiveSonarController.AnimSonar(entry.Key, count, false, entry.Value.GetComponent<EdgeController>().listSonars);
            foreach (ConnectionBean con in connections)
            {
                bool achou = false;
                foreach(GameObject bm in entry.Value.GetComponent<EdgeController>().listBases)
                {
                    if(Vector2.Distance(bm.transform.position, con.getOrigem().transform.position) < 0.2f)
                    {
                        achou = true;
                        break;
                    }
                }
                if (con.getColor() == entry.Key && achou) con.getOrigem().GetComponent<BaseMovelController>().playAnim(con.getSetorCorOrigem());
            }
            if (count > numBonus)
            {
                numBonus = count;
                cor = entry.Key;
            }
        }
        GameSaveController.Save(Current.sceneName + "-" + Current.stageNumber, numBonus);
        //if (Current.stageNumber < ConstelattionsController.getMaxSizeByScene(Current.sceneName))
        //{
        // GameSaveController.Save(Current.sceneName + "-" + (Current.stageNumber + 1), 0);
        // }

        yield return new WaitForSeconds(3.5f);

        menu.SetActive(true);
    }

    public void addConnection(GameObject origem, GameObject destino, string cor, string setorCor)
    {
        connections.Add(new ConnectionBean(origem, destino, cor, setorCor));
    }

    public void resetConnections()
    {
        connections = new List<ConnectionBean>();
        scriptActiveSonarController.cleanSonar();
    }

    public class HashColor
    {
        private int hash;
        private string color;

        public HashColor(int hash, string color)
        {
            this.hash = hash;
            this.color = color;
        }

        public int getHash()
        {
            return this.hash;
        }

        public string getColor()
        {
            return this.color;
        }

    }

}
