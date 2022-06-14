using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rayo_Vector_Escenario : MonoBehaviour
{
    public GameObject RA;
    private GameObject Rayo_vector;
    private Transform RA_valores;
    private Vector3 PA_vec = new Vector3(0.0f, 0.0f, 0.0f);
    private Vector3 PB_vec;
    private Vector3 PB_1_vec, PA_1_vec;

    public GameObject PA;
    public GameObject PB;
    public Vector3 Pa;
    public Vector3 Pb;
    
    void Start()
    {
        Rayo_vector = GameObject.Find("/Modulo_Control/Rayo_Vector");
    }

    void Update()
    {
        PB_vec = Rayo_vector.GetComponent<Rayo_Vector>().PB;
        PA_vec = Rayo_vector.GetComponent<Rayo_Vector>().PA;

        PB_1_vec = Transformaciones.Matrix_x_vec(RA.GetComponent<Escenario>().RA_1, PB_vec - RA.GetComponent<Escenario>().TA);
        PA_1_vec = Transformaciones.Matrix_x_vec(RA.GetComponent<Escenario>().RA_1, PA_vec - RA.GetComponent<Escenario>().TA);

        Pa = PA_1_vec;
        Pb = PB_1_vec;
    }
}