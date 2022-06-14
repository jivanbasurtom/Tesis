using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objeto_astronomico_renderizado : MonoBehaviour
{
    string nombre_objeto;
    private Escenario Escenario;
    private Vector3 T, T_RA;
    private float[][] RA;
    private Quaternion RA_q;
    private GameObject Escenario_objeto;
    private GameObject Objeto_astronomico;

    void Start()
    {
        Escenario_objeto = GameObject.Find("/Modulo_RA/Escenario");
        this.Escenario = Escenario_objeto.GetComponent<Escenario>();
    }

    void Update()
    {
        T = Objeto_astronomico.GetComponent<Transform>().position;
        T_RA = this.Escenario.TA;
        RA = this.Escenario.RA;

        this.gameObject.GetComponent<Transform>().position = Transformaciones.Matrix_x_vec(RA, T) + T_RA;
        this.gameObject.GetComponent<Transform>().rotation = Escenario_objeto.GetComponent<Transform>().rotation;
    }

    public void agregar_enlace(GameObject objeto_astronomico)
    {
        this.Objeto_astronomico = objeto_astronomico;
    }

    public void destruir()
    {
        GameObject.Destroy(this.gameObject);
    }
}