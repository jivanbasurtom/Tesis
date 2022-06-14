using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objeto_seleccionado : MonoBehaviour
{
    private Mallas_Colision Mallas_Colision;
    string nombre_objeto;
    float x, y, z;

    private Dispositivo_pantalla Dispositivo_virtual;
    private bool clic_derecho;
    private bool clic_izquierdo;
    private bool cambio_estado;
    private int contador;
    private bool bloquear_izquierdo;
    private bool bloquear_derecho;

    public bool objeto_seleccionado;
    public Maquina_estado.transiciones transicion; 
    public Maquina_estado.transiciones transicion_anterior;
    public bool desactivar_mallas;

    private Maquina_estado maquina_estado;
    private UI_texto UI;

    void Start()
    {
        GameObject Mallas_Colision_objeto = GameObject.Find("/Modulo_Integracion/Mallas_Colision");
        this.Mallas_Colision = Mallas_Colision_objeto.GetComponent<Mallas_Colision>(); 

        GameObject Dispositivo = GameObject.Find("/Modulo_Control/Dispositivo_inalambrico");
        Dispositivo_virtual = Dispositivo.GetComponent<Dispositivo_pantalla>();
        clic_derecho = false;
        clic_izquierdo = false;
        cambio_estado = true; //no cambiar
        contador = 0;
        bloquear_derecho = false;
        bloquear_izquierdo = false;

        desactivar_mallas = false;

        GameObject Modulo_Retroalimentacion_objeto = GameObject.Find("/Modulo_Retroalimentacion/Maquina_estado");
        maquina_estado = Modulo_Retroalimentacion_objeto.GetComponent<Maquina_estado>();

        GameObject UI_objeto = GameObject.Find("/UI");
        UI = UI_objeto.GetComponent<UI_texto>();
    }

    void Update()
    {
        if(objeto_seleccionado == true)
        {
            desactivar_mallas = true;

            clic_derecho = Dispositivo_virtual.Clic_derecho && !bloquear_derecho; //tiene prioridad el clic derecho
            clic_izquierdo = Dispositivo_virtual.Clic_izquierdo && !bloquear_izquierdo;

            if (clic_derecho == false && clic_izquierdo == false)
            {
                cambio_estado = false;
                bloquear_derecho = false;
                bloquear_izquierdo = false;
            }
            
            bloquear_derecho = (clic_izquierdo == true && clic_derecho == false) ? true : false;
            bloquear_izquierdo = (clic_izquierdo == false && clic_derecho == true) ? true : false;

            if(clic_derecho == true && clic_izquierdo == true)
            {
                clic_izquierdo = false;
                clic_derecho = false;
            }

            clic_izquierdo = !cambio_estado && clic_izquierdo;
            clic_derecho = !cambio_estado && clic_derecho;
            
            contador = ((clic_derecho == true || clic_izquierdo == true) && cambio_estado == false) ? contador + 1 : 0;
            
            if (contador > 3 && contador <= 50)
            {
                if (clic_izquierdo == true)
                {
                    transicion = Maquina_estado.transiciones.presionar_izquierdo;
                }
                if(clic_derecho == true)
                {
                    transicion = Maquina_estado.transiciones.presionar_derecho;
                }
            }

            if (contador > 50)
            {
                if (clic_izquierdo == true)
                {
                    transicion = Maquina_estado.transiciones.mantener_izquierdo;
                }
                if(clic_derecho == true)
                {
                    transicion = Maquina_estado.transiciones.mantener_derecho;
                }
            }

            if (contador <= 3)
            {
                transicion = Maquina_estado.transiciones.nada;
            }

            if( ( 
                    (transicion == Maquina_estado.transiciones.presionar_izquierdo || transicion == Maquina_estado.transiciones.mantener_izquierdo) &&
                    (transicion_anterior == Maquina_estado.transiciones.presionar_derecho || transicion_anterior == Maquina_estado.transiciones.mantener_derecho) 
                ) || 
                (
                    (transicion_anterior == Maquina_estado.transiciones.presionar_izquierdo || transicion_anterior == Maquina_estado.transiciones.mantener_izquierdo) &&
                    (transicion == Maquina_estado.transiciones.presionar_derecho || transicion == Maquina_estado.transiciones.mantener_derecho) 
                ) 
               )
            {
                contador = 0;
                transicion = Maquina_estado.transiciones.nada;
                transicion_anterior = Maquina_estado.transiciones.nada; 
            }

            if (transicion_anterior != Maquina_estado.transiciones.nada && transicion == Maquina_estado.transiciones.nada)
            {
                maquina_estado.enviar_transicion(transicion_anterior);
            }
            transicion_anterior = transicion;
        }
        else
        {
            desactivar_mallas = false;
            cambio_estado = true;
            transicion_anterior = Maquina_estado.transiciones.nada;
        }
    }

    public void guardar_objeto(Maquina_estado.Sistemas sistema_actual, string nombre)
    {
        this.nombre_objeto = nombre;
        this.objeto_seleccionado = true;
        maquina_estado.guardar_objeto_seleccionado(sistema_actual, nombre);
        UI.guardar_objeto_seleccionado(nombre);
    }

    public void guardar_posicion(Maquina_estado.Sistemas sistema_actual, float x, float y, float z)
    {
        maquina_estado.guardar_posicion_objeto_seleccionado(sistema_actual, x, y, z);
        this.objeto_seleccionado = true;
    }

    public void deseleccionar_objeto()
    {
        this.objeto_seleccionado = false;
        this.desactivar_mallas = false;
    }
}