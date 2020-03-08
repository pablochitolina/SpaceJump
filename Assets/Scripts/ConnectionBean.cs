using UnityEngine;

public class ConnectionBean
{

    private GameObject destino;
    private GameObject origem;
    private string color;
    private string setorCorOrigem;

    public ConnectionBean(GameObject newOrigem, GameObject newDestino, string newColor, string setorCor)
    {
        this.origem = newOrigem;
        this.destino = newDestino;
        this.color = newColor;
        this.setorCorOrigem = setorCor;
    }

    public int getHashDestino()
    {
        return this.destino.GetHashCode();
    }
    public int getHashOrigem()
    {
        return this.origem.GetHashCode();
    }

    public GameObject getDestino()
    {
        return this.destino;
    }

    public GameObject getOrigem()
    {
        return this.origem;
    }

    public string getColor()
    {
        return this.color;
    }

    public string getSetorCorOrigem()
    {
        return this.setorCorOrigem;
    }
   
}
