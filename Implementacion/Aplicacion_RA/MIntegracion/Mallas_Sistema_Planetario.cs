using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mallas_Sistema_Planetario : MonoBehaviour
{
    public List<Objeto_astronomico> Objetos;
    private Mallas_Colision Malla;
    private bool interaccion;
    public int numero_planos;
    public float hx;
    public float hy;
    public float hz;

    void Start()
    {
        this.Objetos = GameObject.Find("Modulo_Sistema/Sistema_Planetario").GetComponent<Sistema_Planetario>().objetos;
        hx = 3.0f;
        hy = 3.0f;
        hz = 0;
        numero_planos = 1;
    }

    void Update()
    {

    }

    public void mostrar_nombres(int estado_dispositivo)
    {
        foreach (Objeto_astronomico objeto in this.Objetos)
            {
                Debug.Log(objeto.name.ToString());
            }
    }
}