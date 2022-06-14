using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objeto_astronomico : MonoBehaviour
{
    public float posicion_x;
    public float posicion_y;
    public float velocidad_x;
    public float velocidad_y;
    public float aceleracion_x;
    public float aceleracion_y;
    public float masa;
    public float masa_unitaria;
    public float direccion;
    public float magnitud;
    public float magnitud_unitaria;
    public bool estrella;
    public float Gm;
    public float x, y;
    public int plano;
    public float hx, hy, hz;
    private Valores Valores;
    public bool aplicar_modelo;

    public  GameObject Flecha;
    private Transform Esta_Flecha;
    private Vector3 posicion_flecha;
    private Vector3 rotacion_flecha;
    public int nombre;
    private string objeto_render = "objeto_astronomico_rederizado";
    private string objeto_enlazado;

    public GameObject estrella_render, planeta_render;
    private Objeto_astronomico_renderizado objeto_astronomico_clase; 

    void Start()
    {
        this.Valores = GameObject.Find("Valores").GetComponent<Valores>();

        this.masa = estrella ? this.Valores.Constantes[Valores.Parametros.masa_sol] : this.Valores.Constantes[Valores.Parametros.masa_tierra];
        this.x = this.gameObject.transform.position.x;
        this.y = this.gameObject.transform.position.y;
        this.Gm = this.masa * Valores.Constantes[Valores.Parametros.Constante_Gravitacion];
        if (this.estrella == true)
        {
            this.posicion_x = this.x * Valores.Constantes[Valores.Parametros.distancia_tierra];
            this.posicion_y = this.y * Valores.Constantes[Valores.Parametros.distancia_tierra];
            this.hx = 0.5f/2;
            this.hy = 0.5f/2;
            this.hz = 0.5f/2;
        }
        else
        {
            this.velocidad_y = Valores.Constantes[Valores.Parametros.velocidad_tierra];
            this.posicion_x = this.x * Valores.Constantes[Valores.Parametros.distancia_tierra];
            this.posicion_y = this.y * Valores.Constantes[Valores.Parametros.distancia_tierra];
            this.hx = 0.1f/2;
            this.hy = 0.1f/2;
            this.hz = 0.1f/2;
        }
        this.plano = 1;
        aplicar_modelo = true;
        posicion_flecha = new Vector3(0, 0, 0);
        rotacion_flecha = new Vector3(0, 0, 0);
    }

    void Update()
    {
        x = this.posicion_x / Valores.Constantes[Valores.Parametros.distancia_tierra];
        y = this.posicion_y / Valores.Constantes[Valores.Parametros.distancia_tierra];
        this.gameObject.transform.position = new Vector3(x, y, this.gameObject.transform.position.z);    
    }

    public void Reiniciar_aceleracion()
    {
        this.aceleracion_x = 0;
        this.aceleracion_y = 0;
    }

    public void actualizar_posicion(float posicion_x, float posicion_y)
    {
        this.posicion_x = posicion_x * Valores.Constantes[Valores.Parametros.distancia_tierra];
        this.posicion_y = posicion_y * Valores.Constantes[Valores.Parametros.distancia_tierra];
    }

    public (float, float) posicion()
    {
        return (this.posicion_x / Valores.Constantes[Valores.Parametros.distancia_tierra], this.posicion_y / Valores.Constantes[Valores.Parametros.distancia_tierra]);
    }

    public void calcular_magnitud()
    {
        this.magnitud = Mathf.Sqrt(Mathf.Pow(this.velocidad_x, 2) + Mathf.Pow(this.velocidad_y, 2));
    }

    public void calcular_magnitud_unitaria()
    {
        this.magnitud_unitaria = this.magnitud / Valores.Constantes[Valores.Parametros.velocidad_tierra];
    }

    public void calcular_direccion()
    {
        this.direccion = Mathf.Atan2(this.velocidad_y, this.velocidad_x);
    }

    public void calcular_masa_unitaria()
    {
        if(this.estrella == true)
        {
            this.masa_unitaria = this.masa / Valores.Constantes[Valores.Parametros.masa_sol];
        }
        else
        {
            this.masa_unitaria = this.masa / Valores.Constantes[Valores.Parametros.masa_tierra];
        }
    }
 
    public void actualizar_magnitud(float magnitud_unitaria_nueva)
    {
        this.magnitud = magnitud_unitaria_nueva * Valores.Constantes[Valores.Parametros.velocidad_tierra];
    }

    public void actualizar_direccion(float direccion)
    {
        this.direccion = direccion;
    }

    public void actualizar_velocidad()
    {
        this.velocidad_x = this.magnitud * Mathf.Cos(this.direccion);
        this.velocidad_y = this.magnitud * Mathf.Sin(this.direccion);
    }

    public void actualizar_masa_unitaria(float masa)
    {
        if(masa < 0.000001)
        {
            masa = 0.000001f;
        }

        this.masa_unitaria = masa;

        if(this.estrella == true)
        {
            this.masa = this.masa_unitaria * Valores.Constantes[Valores.Parametros.masa_sol];
        }
        else
        {
            this.masa = this.masa_unitaria * Valores.Constantes[Valores.Parametros.masa_tierra];
        }
        this.Gm = this.masa * Valores.Constantes[Valores.Parametros.Constante_Gravitacion]; 
    }

    public void activar_modelo()
    {
        this.aplicar_modelo = true;
    }

    public void desactivar_modelo()
    {
        this.aplicar_modelo = false;
    }

    public void mostrar_flecha()
    {
        Instantiate(Flecha, new Vector3(this.x, this.y, this.gameObject.transform.position.z), this.gameObject.transform.rotation);
        GameObject Esta_Flecha_Objeto = GameObject.Find("Flecha");
        Esta_Flecha = Esta_Flecha_Objeto.GetComponent<Transform>();
    }

    public void crear_objeto_renderizado(Transform gestor_objetos_transform)
    {
        Instantiate((estrella == true) ? estrella_render : planeta_render);
        GameObject objeto_astronomico_render = (estrella == true) ? GameObject.Find("estrella_render(Clone)") : GameObject.Find("planeta_render(Clone)");
        objeto_enlazado = objeto_render + "_0" + nombre.ToString();
        objeto_astronomico_render.name = objeto_enlazado;
        objeto_astronomico_render.transform.SetParent(gestor_objetos_transform);
        objeto_astronomico_clase = objeto_astronomico_render.GetComponent<Objeto_astronomico_renderizado>();
        objeto_astronomico_clase.agregar_enlace(this.gameObject);
    }

    public void eliminar_objeto_renderizado()
    {
        objeto_astronomico_clase.destruir();
    }
}