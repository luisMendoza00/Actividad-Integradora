using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    // Guardamos los datos del JSON
    SimulationResult simulationResult;

    // Cantidades
    int stepIndex;
    int agentDoneAnimation;

    private int agentQuantity;

    // Prefabs
    [SerializeField] private AgentElement robotPrefab;
    [SerializeField] private AgentElement boxesPrefab;
    [SerializeField] private Shelf shelvesPrefab;

    // Duracion de cada paso
    [SerializeField] private float durationStep;

    // Listas para guardar
    List<AgentElement> robotElemets = new List<AgentElement>();
    List<AgentElement> boxElemets = new List<AgentElement>();
    List<Shelf> shelvesElements = new List<Shelf>();

    // Tag para eliminar Shelves;
    // [SerializeField]
    // private string tagOfShelves;
    // [SerializeField]
    // private GameObject[] shelvesLost;

    public void Build(SimulationResult simulationResult)
    {
        stepIndex = 0;
        agentDoneAnimation = 0;
        this.simulationResult = simulationResult;

        CreateRobots();
        CreateBoxes();
        CreateShelves();

        NextStep();
    }

    void CreateBoxes()
    {
        // Crea las cajas tenemos en la simulacion
        int boxQuantity = simulationResult.steps[stepIndex].boxes.Length;
        for (int i = 0; i < boxQuantity; i++)
        {
            // Instanciamos una caja
            AgentElement agentElement = Instantiate(boxesPrefab);
            agentElement.Build(this, simulationResult.steps[stepIndex].boxes[i].id);

            // Agregamos a la lista
            boxElemets.Add(agentElement);


            // Cambiamos a su lugar
            int x = (simulationResult.steps[stepIndex].boxes[i].pos.x * 6) + 3;
            int y = simulationResult.steps[stepIndex].boxes[i].pos.y;
            int z = (simulationResult.steps[stepIndex].boxes[i].pos.z * 6) + 3;
            Vector3 position = new Vector3(x, y, z);
            agentElement.transform.position = position;
        }
    }

    void CreateRobots()
    {
        // Cuantos robots tenemos en la simulacion
        int robotQuantity = simulationResult.steps[stepIndex].robots.Length;
        for (int i = 0; i < robotQuantity; i++)
        {
            //Instanciamos el robot
            AgentElement agentElement = Instantiate(robotPrefab);
            agentElement.Build(this, simulationResult.steps[stepIndex].robots[i].id);
            robotElemets.Add(agentElement);


            //Cambiamos a su lugar
            int x = (simulationResult.steps[stepIndex].robots[i].pos.x * 6) + 3;
            int y = simulationResult.steps[stepIndex].robots[i].pos.y;
            int z = (simulationResult.steps[stepIndex].robots[i].pos.z * 6) + 3;
            Vector3 position = new Vector3(x, y, z);
            agentElement.transform.position = position;
        }
    }

    void CreateShelves()
    {
        // Crea los estantes tenemos en la simulaci�n
        int shelvesQuantity = simulationResult.steps[stepIndex].shelves.Length;
        for (int i = 0; i < shelvesQuantity; i++)
        {
            // Instanciamos el objeto
            Shelf shelf = Instantiate(shelvesPrefab);
            shelf.Build(this, simulationResult.steps[stepIndex].shelves[i].id);

            // Agregamos a la lista
            shelvesElements.Add(shelf);

            // Cambiamos de posicion
            float x = (simulationResult.steps[stepIndex].shelves[i].pos.x * 6f) + 4.5f;
            float y = simulationResult.steps[stepIndex].shelves[i].pos.y;
            float z = (simulationResult.steps[stepIndex].shelves[i].pos.z * 6f) + 1f;
            Vector3 position = new Vector3(x, y, z);
            shelf.transform.position = position;

            // Indicamos cuantas cajas queremos en el estante
            shelf.howManyBoxes = simulationResult.steps[stepIndex].shelves[i].stored;
            shelf.addBox();

            
        }
    }

    // Se ejecuta una vez terminando el paso actual
    void NextStep()
    {
        agentDoneAnimation = 0;
        MoveRobots();
    }
    // Elimina todas las cajas de la lista
    void DeleteBoxes()
    {
        List<AgentElement> toDelete = new List<AgentElement>();
        foreach(AgentElement box in boxElemets)
        {
            bool found = false;
            foreach(Agent agent in simulationResult.steps[stepIndex].boxes)
            {
                if(box.id == agent.id)
                {
                    found = true;
                    break;
                }
            }

            if(!found)
            {
                toDelete.Add(box);
            }
        }
        foreach(AgentElement box in toDelete)
        {
            boxElemets.Remove(box);
            Destroy(box.gameObject);
        }
    }

    void CreateNewShelves()
    {
        int shelvesQuantity = simulationResult.steps[stepIndex].shelves.Length;
        bool found;
        for (int i = 0; i < shelvesQuantity; i++)
        {
            found = false;
            foreach(Shelf shelf in shelvesElements)
            {
                if(shelf.id == simulationResult.steps[stepIndex].shelves[i].id)
                {
                    found = true;
                }
            }
            if(!found)
            {
                // Instanciamos el objeto
                Shelf shelf = Instantiate(shelvesPrefab);
                shelf.Build(this, simulationResult.steps[stepIndex].shelves[i].id);
                // Agregamos a la lista
                shelvesElements.Add(shelf);

                // Cambiamos de posicion
                float x = (simulationResult.steps[stepIndex].shelves[i].pos.x * 6f) + 4.5f;
                float y = simulationResult.steps[stepIndex].shelves[i].pos.y;
                float z = (simulationResult.steps[stepIndex].shelves[i].pos.z * 6f) + 1f;
                Vector3 position = new Vector3(x, y, z);
                shelf.transform.position = position;
            }
        }
    }
    

    void DeleteShelves()
    {
        List<Shelf> toDelete = new List<Shelf>();
        foreach(Shelf shelf in shelvesElements)
        {   
            shelf.autoDestroy();
            Destroy(shelf.gameObject);
        }
        shelvesElements.Clear();
    }

    // Activamos las animaciones para cada robot
    void MoveRobots()
    {
        agentQuantity = simulationResult.steps[stepIndex].robots.Length;
        for (int i = 0; i < agentQuantity; i++)
        {

            int x = (simulationResult.steps[stepIndex].robots[i].pos.x * 6) + 3;
            int y = simulationResult.steps[stepIndex].robots[i].pos.y * 6;
            int z = (simulationResult.steps[stepIndex].robots[i].pos.z * 6) + 3;
            Vector3 to = new Vector3(x, y, z);
            
            robotElemets[i].Run(robotElemets[i].transform.position, to, durationStep);
        }
    }

    void FillShelves()
    {
        int shelvesQuantity = simulationResult.steps[stepIndex].shelves.Length;
        for (int i = 0; i < shelvesQuantity; i++)
        {
            foreach(Shelf shelf in shelvesElements)
            {
                if(shelf.id == simulationResult.steps[stepIndex].shelves[i].id)
                {
                    shelf.howManyBoxes = simulationResult.steps[stepIndex].shelves[i].stored;
                    shelf.autoDestroy();
                    shelf.addBox();
                }
            }
        }
    }

    // Una vez que termina el paso manda a llamar el siguiente
    public void DoneAgentStep(AgentElement agentElement)
    {
        agentDoneAnimation++;
        if (agentDoneAnimation >= agentQuantity)
        {
            if (stepIndex >= simulationResult.stepCount)
            {
                stepIndex = 0;
                CreateBoxes();
                DeleteShelves();
            }
            DeleteBoxes();
            CreateNewShelves();
            FillShelves();
            stepIndex++;
            NextStep();
        }
    }
}


