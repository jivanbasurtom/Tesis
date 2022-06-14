using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sistema : MonoBehaviour
{
    private Maquina_estado.Sistemas Sistema_actual;

    private GameObject gestor_objetos_objeto;
    private Transform gestor_objetos_transform;

    //sistema planetario
    private Transform sistema_planetario_transform; 
    private Sistema_Planetario sistema_planetario;
    private int numero_sistema_planetario = 0;
    private string nombre_sistema_planetario = "Objeto_astronomico";
    public GameObject estrella;
    public GameObject planeta;

    private bool es_estrella;
    private float posicion_x_unitaria;
    private float posicion_y_unitaria;
    private string nombre_objeto;

    void Start()
    {
        Sistema_actual = Maquina_estado.Sistemas.ninguno;
        GameObject sistema_planetario_objeto = GameObject.Find("Sistema_Planetario");
        sistema_planetario_transform = sistema_planetario_objeto.GetComponent<Transform>();
        sistema_planetario = sistema_planetario_objeto.GetComponent<Sistema_Planetario>();

        gestor_objetos_objeto = GameObject.Find("Gestor_Objetos");
        gestor_objetos_transform = gestor_objetos_objeto.GetComponent<Transform>();
    }

    void Update()
    {

    }

    void Iniciar_Sistema_Planetario()
    {
        agregar_al_sistema_planetario(true, 0.0f, 0.0f, false);

        agregar_al_sistema_planetario(false, 1.0f, 0.0f, false);
        
        sistema_planetario.enabled = true;
        sistema_planetario.actualizar_numero_objetos();
        sistema_planetario.numero_estrellas = 1;
        sistema_planetario.numero_planetas = 1;
    }

    void Eliminar_Sistema_Planetario()
    {
        Gestor_objetos gestor_objetos_clase = gestor_objetos_objeto.GetComponent<Gestor_objetos>();
        gestor_objetos_clase.eliminar_objetos_renderizados();
        sistema_planetario.limpiar_lista_objetos();
        sistema_planetario.enabled = false;
        foreach(Transform hijo in sistema_planetario_transform)
        {
            GameObject.Destroy(hijo.gameObject);
        }
        numero_sistema_planetario = 0;
    }

    public void agregar_al_sistema_planetario(bool es_estrella, float posicion_x_unitaria, float posicion_y_unitaria, bool valores_propios)
    {
        if(valores_propios == true)
        {
            es_estrella = this.es_estrella;
            posicion_x_unitaria = this.posicion_x_unitaria;
            posicion_y_unitaria = this.posicion_y_unitaria;
        }
        Instantiate((es_estrella == true) ? estrella : planeta, new Vector3(posicion_x_unitaria, posicion_y_unitaria, 0), Quaternion.identity);
        GameObject objeto_astronomico =  GameObject.Find((es_estrella == true) ? "estrella(Clone)" : "planeta(Clone)");
        numero_sistema_planetario++;
        objeto_astronomico.name = nombre_sistema_planetario + "_0" + numero_sistema_planetario.ToString();
        objeto_astronomico.transform.SetParent(sistema_planetario_transform);
        Objeto_astronomico objeto_astronomico_clase = objeto_astronomico.GetComponent<Objeto_astronomico>();
        objeto_astronomico_clase.nombre = numero_sistema_planetario;
        sistema_planetario.objetos.Add(objeto_astronomico_clase);
        if(es_estrella == true)
        {
            sistema_planetario.add_numero_estrellas();
        }
        else
        {
            sistema_planetario.add_numero_planetas();
        }
        sistema_planetario.actualizar_numero_objetos();
        objeto_astronomico_clase.crear_objeto_renderizado(gestor_objetos_transform);
    }

    public void agregar_al_sistema_planetario_guardar(bool es_estrella, float posicion_x_unitaria, float posicion_y_unitaria)
    {
        this.es_estrella = es_estrella;
        this.posicion_x_unitaria = posicion_x_unitaria;
        this.posicion_y_unitaria = posicion_y_unitaria;
        sistema_planetario.bloquear_algoritmo(Valores.Tarea.crear);
    }

    public void eliminar_del_sistema_planetario(string nombre_objeto, bool valores_propios)
    {
        if(valores_propios == true)
        {
            nombre_objeto = this.nombre_objeto;
        }
        GameObject objeto_astronomico_objeto = GameObject.Find(nombre_objeto);
        Objeto_astronomico objeto_astronomico_clase = objeto_astronomico_objeto.GetComponent<Objeto_astronomico>();
        objeto_astronomico_clase.eliminar_objeto_renderizado();
        if(objeto_astronomico_clase.estrella == estrella) 
        {
            sistema_planetario.quitar_numero_estrellas();
        }
        else
        {
            sistema_planetario.quitar_numero_planetas();
        }
        sistema_planetario.objetos.Remove(objeto_astronomico_clase);
        sistema_planetario.actualizar_numero_objetos();
        
        Destroy(objeto_astronomico_objeto);
    }

    public void eliminar_del_sistema_planetario_guardar(string nombre_objeto)
    {
        this.nombre_objeto = nombre_objeto;
        sistema_planetario.bloquear_algoritmo(Valores.Tarea.eliminar);
    }

    public void sistema_crear(Maquina_estado.Sistemas Sistema_nuevo)
    {
        if(Sistema_nuevo == Maquina_estado.Sistemas.S_planetario)
        {
            Iniciar_Sistema_Planetario();
        }
        if(Sistema_nuevo == Maquina_estado.Sistemas.S_engranes)
        {
        }
        if(Sistema_nuevo == Maquina_estado.Sistemas.S_colisiones)
        {
        }
        if(Sistema_nuevo == Maquina_estado.Sistemas.ninguno)
        {
        }
    }

    public void sistema_eliminar(Maquina_estado.Sistemas Sistema_anterior)
    {
        if(Sistema_anterior == Maquina_estado.Sistemas.S_planetario)
        {
            Eliminar_Sistema_Planetario();
        }
        if(Sistema_anterior == Maquina_estado.Sistemas.S_engranes)
        {
        }
        if(Sistema_anterior == Maquina_estado.Sistemas.S_colisiones)
        {
        }
        if(Sistema_anterior == Maquina_estado.Sistemas.ninguno)
        {
        }
    }
}