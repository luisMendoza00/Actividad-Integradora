using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Warehouse : MonoBehaviour
{
    SimulationResult simulationResult;

    // World
    public GameObject Floor;
    public GameObject Wall;

    // Size of the warehouse
    private int SizeFloor;

    // For instantiates GameObjects
    private GameObject floorTitle;
    private GameObject wall;

    public void Build(SimulationResult pythonSimulation)
    {
        
        this.simulationResult = pythonSimulation;
        SizeFloor = simulationResult.grid.height;
        wareHouseStart();

    }
        
        // Start is called before the first frame update
    public void wareHouseStart()
        {
            for (float a = 0; a < SizeFloor; a = a + 1)
            {

                floorTitle = Instantiate(Floor);
                floorTitle.transform.position = new Vector3(0f, 0f, a * 6f);

                wall = Instantiate(Wall);
                wall.transform.position = new Vector3(0f, 0f, a * 6f);

                wall = Instantiate(Wall);
                wall.transform.position = new Vector3(SizeFloor * 6f, 0f, (a * 6f) + 6f);
                wall.transform.Rotate(0f, 180f, 0f);

                wall = Instantiate(Wall);
                wall.transform.position = new Vector3((a * 6f) + 6f, 0f, 0f);
                wall.transform.Rotate(0f, 270f, 0f);

                wall = Instantiate(Wall);
                wall.transform.position = new Vector3(a * 6f, 0f, SizeFloor * 6f);
                wall.transform.Rotate(0f, 90f, 0f);

                for (float i = 1; i < SizeFloor; i = i + 1)
                {
                    floorTitle = Instantiate(Floor);
                    floorTitle.transform.position = new Vector3(i * 6f, 0f, a * 6f);
                }
            }
    }
}
