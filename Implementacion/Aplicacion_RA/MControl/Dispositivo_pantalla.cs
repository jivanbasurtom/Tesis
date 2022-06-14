using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Dispositivo_pantalla : MonoBehaviour
{
    Touch Puntador;
    Touch Boton;
    private float width_p;
    private float height_p;
    private float width_middle;
    private float height_middle;
    private float Touch_to_Puntero;
    private float pos_x;
    private float pos_y;
    public float ang_x;
    public float ang_y;
    private float ang_x_ant;
    private float ang_y_ant;
    public float ang_vel_x;
    public float ang_vel_y;
    private float ang_vel_x_ant;
    private float ang_vel_y_ant;
    public float ang_acel_x;
    public float ang_acel_y;
    private Quaternion Q;
    public bool interaccion;
    public bool Clic_derecho;
    public bool Clic_izquierdo;
    private bool Clic_izquierdo_anterior;
    private int numero_toques;
    private float boton_x;
    private float boton_y;
    private float dt;

    public bool pruebas;
    private BT BT_clase;
    public float ang_z;

    void Start()
    {
        
        width_middle = Camera.main.pixelWidth / 2;
        height_middle = Camera.main.pixelHeight / 2;
        Touch_to_Puntero = 1000;
        ang_x = 0;
        ang_y = 0;
        ang_z = 0;
        ang_x_ant = 0;
        ang_y_ant = 0;
        ang_vel_x_ant = 0;
        ang_vel_y_ant = 0;
        Clic_izquierdo = false;
        Clic_derecho = false;
        Clic_izquierdo_anterior = false;
        GameObject BT_objeto = GameObject.Find("BT");
        BT_clase = BT_objeto.GetComponent<BT>();
    }

    void Update()
    {
        if(pruebas == true)
        {
            pantalla_tactil();
        }
        else
        {
            Clic_derecho = BT_clase.leer_clic_derecho();
            Clic_izquierdo = BT_clase.leer_clic_izquierdo();
            ang_x = BT_clase.obtener_ang_x();
            ang_y = BT_clase.obtener_ang_y();
            ang_z = BT_clase.obtener_ang_z();
        }

        if (Clic_izquierdo_anterior == true && Clic_izquierdo == false) 
        {
            interaccion = true;
        }
        else 
        {
            interaccion = false;
        }
        
        if (interaccion == true)
        {
            Debug.Log("interacción");
        }
        Clic_izquierdo_anterior = Clic_izquierdo;
    }

    private void pantalla_tactil()
    {
        dt = Time.deltaTime;
        numero_toques = Input.touchCount;

        if (numero_toques > 0)
        {
            Puntador = Input.GetTouch(0);
            pos_x = (Puntador.position.x - width_middle) / Touch_to_Puntero;
            pos_y = (Puntador.position.y - height_middle) / Touch_to_Puntero;

            ang_x = Mathf.Atan2(pos_x, 2) * Mathf.Rad2Deg;
            ang_y = Mathf.Atan2(pos_y, 2) * Mathf.Rad2Deg;
            
            ang_vel_x = (ang_x - ang_x_ant) / dt;
            ang_vel_y = (ang_y - ang_y_ant) / dt;
            ang_acel_x = (ang_vel_x - ang_vel_x_ant) / dt;
            ang_acel_y = (ang_vel_y - ang_vel_y_ant) / dt;

            ang_x_ant = ang_x;
            ang_y_ant = ang_y;
            ang_vel_x_ant = ang_vel_x;
            ang_vel_y_ant = ang_vel_y;
        }
        else
        {
            ang_x_ant = 0;
            ang_y_ant = 0;
            ang_vel_x_ant = 0;
            ang_vel_y_ant = 0;
        }

        for(int i = 0; i < numero_toques; i++)
        {
            Boton = Input.GetTouch(i);
            boton_x = Boton.position.x;
            boton_y = Boton.position.y;

            if (boton_x > 1760)
            {
                Clic_derecho = boton_y > 540 ? true : false;
                Clic_izquierdo = boton_y <= 540 ? true : false;
            }
            else
            {
                Clic_derecho = false;
                Clic_izquierdo = false;
            }
        }
    }
}