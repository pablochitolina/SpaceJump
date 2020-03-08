using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeController : MonoBehaviour
{
    public int count = 0;
    public List<GameObject> listSonars = new List<GameObject>();
    public List<GameObject> listBases = new List<GameObject>();
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "ampli")
        {
            count++;
        }
        if (collision.gameObject.tag == "baseInner")
        {
            listBases.Add(collision.gameObject);
        }
        if (collision.gameObject.tag == "baseActive")
        {
            listSonars.Add(collision.gameObject);
        }
    }
  
}
