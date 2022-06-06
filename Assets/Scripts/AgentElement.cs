using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AgentElement : MonoBehaviour
{
    // Para generar animacion
    public Animator ani;


    [HideInInspector]
    public bool isAnimationDone;
    AnimationManager animationManager;
    public int id;


    public float gradeX;
    public float gradeZ;

    public Quaternion angle;


    // Asignamos un id
    public void Build(AnimationManager animationManager, int id)
    {
        this.animationManager = animationManager;
        this.id = id;
    }


    // Iniciamos la corrutina para animacion
    public void Run(Vector3 from, Vector3 to, float duration)
    {
        isAnimationDone = false;
        StartCoroutine(AnimateStep(from, to, duration));
    }


    // Regresa indicando que llego a su objetivo
    void ArrivedPoint()
    {
        animationManager.DoneAgentStep(this);
    }

    // Genera la animaci�n
    IEnumerator AnimateStep(Vector3 from, Vector3 to, float duration)
    {
        // La animacion acabara con la duracion recibida
        float dt = 0;
        while (true)
        {

            // Validamos que tenga movimiento para activar animacion
            if (from == to)
            {
                ani.SetBool("Walk", false);
            }
            else
            {
                ani.SetBool("Walk", true);
            }
            dt += Time.deltaTime;
            float scale = dt / duration;
            
            // Movemos el robot
            transform.position = Vector3.Lerp(from, to, scale);

            if (scale >= 1)
                break;
            // Esperamos que termina la animacion para seguir
            yield return new WaitForEndOfFrame();
        }

        
        isAnimationDone = true;
        ArrivedPoint();
    }
}
