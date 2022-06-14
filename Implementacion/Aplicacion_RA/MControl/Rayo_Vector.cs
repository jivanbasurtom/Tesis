using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rayo_Vector : MonoBehaviour
{
    private GameObject Dispositivo_inalambrico;
    private Dispositivo_pantalla Dispositivo_Pantalla;
    public GameObject puntero;
    private Transform puntero_proyeccion;
    private Quaternion Q;
    private float ang_x;
    private float ang_y;
    private float pos_x;
    private float pos_y;
    public Vector3 PA = new Vector3(0, 0, 0);
    public Vector3 PB;

    private Transform puntero_proyeccion_visible;

    void Start()
    {
        Dispositivo_inalambrico = GameObject.Find("/Modulo_Control/Dispositivo_inalambrico");
        Dispositivo_Pantalla = Dispositivo_inalambrico.GetComponent<Dispositivo_pantalla>();
        puntero.SetActive(true);
        puntero_proyeccion = puntero.GetComponent<Transform>().GetChild(0);
        ang_x = 0;
        ang_y = 0;
        puntero_proyeccion_visible = puntero.GetComponent<Transform>();
    }

    void Update()
    {
        ang_x = Dispositivo_Pantalla.ang_x;
        ang_y = Dispositivo_Pantalla.ang_y;
        pos_x = 2 * Mathf.Tan(ang_x * Mathf.Deg2Rad);
        pos_y = 2 * Mathf.Tan(ang_y * Mathf.Deg2Rad);

        puntero.transform.rotation = Camera.main.transform.rotation; //cambiar por la camara de RA
        puntero.SetActive(true);
        puntero_proyeccion.localPosition = new Vector3(pos_x, pos_y, 1);

        Q = puntero.GetComponent<Transform>().rotation * Quaternion.Euler(- ang_y, ang_x, 0);
        PB = puntero_proyeccion.position;// + Camera.main.transform.position;
        
        PA = Camera.main.transform.position;
        puntero_proyeccion_visible.position = Camera.main.transform.position;
    }
}