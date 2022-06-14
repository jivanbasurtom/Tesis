using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mallas_Colision : MonoBehaviour
{
    private Dispositivo_pantalla Dispositivo_virtual;
    public bool interaccion;
    private GameObject Mallas_Sistema_Planetario;
    private GameObject Malla_actual;
    private Vector3 Pa, Pb;
    private Rayo_Vector_Escenario Rayo_Vector_Escenario;
    public string nombre_objeto_final;
    public bool objeto_seleccionado;
    private Objeto_seleccionado Objeto_Seleccionado;

    private Maquina_estado.Sistemas sistema_actual;

    private float x, y, z, hx_malla, hy_malla, hz_malla;
    private float posicion_interseccion_x, posicion_interseccion_y;

    void Start()
    {
        GameObject Dispositivo = GameObject.Find("/Modulo_Control/Dispositivo_inalambrico");
        Dispositivo_virtual = Dispositivo.GetComponent<Dispositivo_pantalla>();

        Mallas_Sistema_Planetario = GameObject.Find("/Modulo_Integracion/Mallas_Sistema_Planetario");
        GameObject Rayo_Vector_Escenario_objeto = GameObject.Find("/Modulo_Control/Rayo_Vector_Escenario");
        this.Rayo_Vector_Escenario = Rayo_Vector_Escenario_objeto.GetComponent<Rayo_Vector_Escenario>();

        GameObject Objeto_Seleccionado_objeto = GameObject.Find("/Modulo_Integracion/Objeto_Seleccionado");
        Objeto_Seleccionado = Objeto_Seleccionado_objeto.GetComponent<Objeto_seleccionado>();
        objeto_seleccionado = false;
    }

    void Update()
    {
        Malla_actual = Mallas_Sistema_Planetario;
        this.interaccion = Dispositivo_virtual.interaccion;
        sistema_actual = Maquina_estado.Sistemas.S_planetario;     

        if (interaccion == true && Objeto_Seleccionado.desactivar_mallas == false)
        {
            float hx;
            hx = Malla_actual.GetComponent<Mallas_Sistema_Planetario>().hx;
            float hy;
            hy = Malla_actual.GetComponent<Mallas_Sistema_Planetario>().hy;
            this.Pa = this.Rayo_Vector_Escenario.Pa;
            this.Pb = this.Rayo_Vector_Escenario.Pb;
            Vector3 Pab = Pb - Pa;

            float distancia_objeto_anterior = 10000;
            nombre_objeto_final = "";

            for (int i = 0; i < Malla_actual.GetComponent<Mallas_Sistema_Planetario>().numero_planos; i++)
            {
                float hz = Malla_actual.GetComponent<Mallas_Sistema_Planetario>().hz; //cambiar para tomar el hz según plano   
                Vector3 Pa0 = new Vector3(0, 0, hz) - Pa;
                (float u, float v) = calcular_u_v_escenario(Pab, Pa0, hx, hy);
                this.posicion_interseccion_x = u * hx;
                this.posicion_interseccion_y = v * hy;
                if ((-1 <= u) && (u <= 1) && (-1 <= v) && (v <= 1))
                {
                    //Dentro
                    Vector3 punto_interseccion = new Vector3(u*hx, v*hy, hz);
                    (string nombre_objeto, float distancia_objeto) = Calcular_objeto_cercano(Malla_actual.GetComponent<Mallas_Sistema_Planetario>().Objetos, i, punto_interseccion);

                    if (distancia_objeto < distancia_objeto_anterior)
                    {
                        nombre_objeto_final = nombre_objeto;
                        distancia_objeto_anterior = distancia_objeto;
                    }

                    obtener_valores_objeto();
                    objeto_seleccionado = interseccion_objeto(x, y, z, hx_malla, hy_malla, hz_malla, this.Pa, Pab);

                    if(objeto_seleccionado == true)
                    {
                        this.Objeto_Seleccionado.guardar_objeto(sistema_actual, nombre_objeto_final);
                    }
                    else
                    {
                        this.Objeto_Seleccionado.guardar_posicion(sistema_actual, this.posicion_interseccion_x, this.posicion_interseccion_y, z);
                    }
                }
                else
                {
                    //fuera
                    Objeto_Seleccionado.deseleccionar_objeto();
                }
            }
        }
    }

    (float u, float v) calcular_u_v_escenario(Vector3 Pab, Vector3 Pa0, float hx, float hy)
    {
        float u = (Pab.x*Pa0.z - Pab.z*Pa0.x) / (hx * Pab.z);
        float v = (Pab.y*Pa0.z - Pab.z*Pa0.y) / (hy * Pab.z);
        return (u, v);
    }

    (string nombre_objeto, float distancia_objeto) Calcular_objeto_cercano(List<Objeto_astronomico> Objetos, int i, Vector3 punto_interseccion)
    {
        int plano = i + 1;
        float inter_x = punto_interseccion.x;
        float inter_y = punto_interseccion.y;
        float distancia_objeto = 0;
        float distancia_objeto_anterior = 10000;
        string nombre_objeto = "";
        foreach (Objeto_astronomico objeto in Objetos)
        {
            if (objeto.plano == plano)
            {
                distancia_objeto = Mathf.Sqrt(Mathf.Pow(objeto.x - inter_x, 2) + Mathf.Pow(objeto.y - inter_y, 2));
                if (distancia_objeto < distancia_objeto_anterior)
                {
                    nombre_objeto = objeto.name;
                    distancia_objeto_anterior = distancia_objeto;
                }
            }
        }
        return (nombre_objeto, distancia_objeto);
    }

    bool interseccion_objeto(float x, float y, float z, float hx, float hy, float hz, Vector3 Pa, Vector3 Pab)
    {
        Vector3 Pa0 = new Vector3(0, 0, 0);
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                Pa0 = Get_Pa0(3*i + j, x, y, z, hx, hy, hz, Pa);
                (float u, float v) = calcular_u_v_objeto(j, Pa0, Pab, hx, hy, hz);
                if ((-1 <= u) && (u <= 1) && (-1 <= v) && (v <= 1))
                {
                    return true;
                }
            }
        }
        return false;
    }

    Vector3 Get_Pa0(int caso, float x, float y, float z, float hx, float hy, float hz, Vector3 Pa)
    {   
        switch (caso)
        {
            case 0:
                z += hz; 
                break;
            case 1:
                x += hx; 
                break;
            case 2:
                y += hy; 
                break;
            case 3:
                z -= hz; 
                break;
            case 4:
                x += hx; 
                break;
            case 5:
                y -= hy; 
                break;
            default:
                break;
        };
        Vector3 Pa0 = new Vector3(x, y, z) - Pa; 
        return Pa0;
    }

    (float u, float v) calcular_u_v_objeto(int ecuacion, Vector3 Pa0, Vector3 Pab, float hx, float hy, float hz)
    {
        float u = 0, v = 0;
        switch (ecuacion)
        {
            case 0:
                u = (Pab.x*Pa0.z - Pab.z*Pa0.x) / (hx*Pab.z);
                v = (Pab.y*Pa0.z - Pab.z*Pa0.y) / (hy*Pab.z);
                break;
            case 1:
                u = (Pab.x*Pa0.z - Pab.z*Pa0.x) / (hz*Pab.x);
                v = (Pab.x*Pa0.y - Pab.y*Pa0.x) / (-hy*Pab.x);
                break;
            case 2:
                u = (Pab.y*Pa0.z - Pab.z*Pa0.y) / (-hz*Pab.y);
                v = (Pab.x*Pa0.y - Pab.y*Pa0.x) / (-hx*Pab.y);
                break;  
            default:
                break;
        }
        return (u, v);
    }

    void obtener_valores_objeto()
    {
        GameObject objeto_objeto = GameObject.Find(nombre_objeto_final);
        Debug.Log(nombre_objeto_final);

        if(sistema_actual == Maquina_estado.Sistemas.S_planetario)
        {
            Objeto_astronomico Objeto;
            Objeto = objeto_objeto.GetComponent<Objeto_astronomico>();
            this.x = Objeto.x;
            this.y = Objeto.y;
            this.z = 0; //cambiar por una funcion para obtener el valor de z segun plano
            this.hx_malla = Objeto.hx;
            this.hy_malla = Objeto.hy;
            this.hz_malla = Objeto.hz;
        }
    }
}