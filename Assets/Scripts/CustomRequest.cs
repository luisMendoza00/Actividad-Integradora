using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class CustomRequest : MonoBehaviour
{

    // Colocamos la url donde se encuetre el JSON
	[SerializeField] private string uri;

    // Designamos las animaciones y la bodega
    [SerializeField] private AnimationManager animationManager;
    [SerializeField] private Warehouse warehouse;


    void Start()
    {
        // Creamos una corrutina de la peticion al servidor
        StartCoroutine(GetRequest(uri));
    }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Hacemos la peticin y esperamos la respuesta
            yield return webRequest.SendWebRequest();

            // Separamos los tipos de información recibida
            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            // Validamos los resultados
            switch (webRequest.result)
            {
                // En caso de errores imprime en consola
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;

                // Si la peticion fue exitosa generamos la simulacion
                case UnityWebRequest.Result.Success:

                    // Convertimos el JSON en un formato compatible con Unity
                    string jsonText = webRequest.downloadHandler.text;
                    SimulationResult pSimulation = JsonUtility.FromJson<SimulationResult>(jsonText);
                    
                    // Generamos la simulacion
                    animationManager.Build(pSimulation);

                    // Creamos la bodega
                    warehouse.Build(pSimulation);
                    break;
            }
        }
    }
}