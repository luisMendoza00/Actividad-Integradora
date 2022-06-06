using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shelf : AgentElement
{
    // Indicamos las cajas a crear
    public int howManyBoxes;

    
    public GameObject Boxes;
    private GameObject box;
    
    // Creamos una lista de las cajas que se encuentren el estante
    List<GameObject> boxElemets = new List<GameObject>();

    // Colocamos las ubicaciones para crear las cajas sin colisionar
    private Vector3[] placesBoxes = new[] { new Vector3(-0.4f, 1.6f, 0.6f), new Vector3(-1.2f, 1.6f, 0.6f), new Vector3(-2f, 1.6f, 0.6f), new Vector3(-0.4f, 3.6f, 0.6f), new Vector3(-1.2f, 3.6f, 0.6f), new Vector3(-2f, 3.6f, 0.6f)};


    // Generamos las cajas
    public void addBox()
    {
        for (int indexBox = 0; indexBox < howManyBoxes; indexBox = indexBox + 1)
        {
            box = Instantiate(Boxes);
            box.transform.position = new Vector3(placesBoxes[indexBox].x + this.transform.position.x , placesBoxes[indexBox].y + this.transform.position.y, (placesBoxes[indexBox].z + this.transform.position.z));
            box.transform.Rotate(0f, 180f, 0f);
            boxElemets.Add(box);
        }
    }

    // Eliminamos todas las cajas que se crearon en el estante
    public void autoDestroy()
    {
        foreach(GameObject box in boxElemets)
        {
            Destroy(box);
        }

    }

}
