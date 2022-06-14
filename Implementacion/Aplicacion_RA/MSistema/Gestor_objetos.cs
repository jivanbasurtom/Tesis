using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gestor_objetos : MonoBehaviour
{
    private GameObject Sistema;

    void Start()
    {
        Sistema = GameObject.Find("/Modulo_Sistema/Sistema");
    }

    void Update()
    {
        
        
    }

    public void eliminar_objetos_renderizados()
    {
        foreach(Transform hijo in GetComponent<Transform>())
        {
            GameObject.Destroy(hijo.gameObject);
        }
    }
}