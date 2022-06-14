using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maquina_estado : MonoBehaviour
{
    public string nombre_objeto;
    public float x, y, z;
    private int ME_seleccionada;
    private int Sistema_seleccionado;

    public enum ME
    {
        primera,
        segunda,
        ninguna
    }

    public Dictionary<ME, int> ME_disponibles = new Dictionary<ME, int>
    {
        {ME.primera, 1},
        {ME.segunda, 2},
        {ME.ninguna, -1}
    };

    public enum Sistemas
    {
        S_planetario,
        S_engranes,
        S_colisiones,
        ninguno
    }

    private Dictionary<Sistemas, int> Sistemas_disponibles = new Dictionary<Sistemas, int>
    {
        {Sistemas.S_planetario, 1},
        {Sistemas.S_engranes, 2},
        {Sistemas.S_colisiones, 3},
        {Sistemas.ninguno, -1}
    };

    public enum SPlanetario
    {
        agregar_estrella,
        agregar_planeta,
        eliminar_objeto,
        modificar_posicion,
        modificar_direccion,
        modificar_magnitud,
        modificar_masa,
        nada
    }

    private Maquina_estado.Sistemas sistema_actual;
    public Maquina_estado.ME maquina_estado_actual;
    private Maquina_estado_sistema_planetario ME_SP;
    
    public enum transiciones
    {
        presionar_izquierdo,
        presionar_derecho,
        mantener_izquierdo,
        mantener_derecho,
        nada
    }

    public transiciones transicion;
    private Objeto_seleccionado objeto_seleccionado;
    private Maquina_estado_sistema_planetario Sistema_Planetario;

    void Start()
    {
        this.sistema_actual = Maquina_estado.Sistemas.ninguno;
        this.maquina_estado_actual = Maquina_estado.ME.ninguna;

        GameObject Objeto_seleccionado_objeto = GameObject.Find("/Modulo_Integracion/Objeto_Seleccionado");
        objeto_seleccionado = Objeto_seleccionado_objeto.GetComponent<Objeto_seleccionado>();

        GameObject Maquinas_estado_sistemas = GameObject.Find("/Modulo_Retroalimentacion/Maquina_estado_sistema_planetario");
        Sistema_Planetario = Maquinas_estado_sistemas.GetComponent<Maquina_estado_sistema_planetario>();

        transicion = transiciones.nada;
    }

    void Update()
    {
         switch (sistema_actual)
        {
            case Maquina_estado.Sistemas.S_planetario:
                if(maquina_estado_actual == Maquina_estado.ME.primera)
                {
                    Sistema_Planetario.ME_SP_primera(transicion);
                }

                if(maquina_estado_actual == Maquina_estado.ME.segunda)
                {
                    Sistema_Planetario.ME_SP_segunda(transicion);
                }
            break;
            
            case Maquina_estado.Sistemas.S_engranes:
            
            break;
            
            case Maquina_estado.Sistemas.S_colisiones:
            
            break;
        };
    }

    public void guardar_objeto_seleccionado(Maquina_estado.Sistemas sistema_actual, string nombre)
    {
        this.sistema_actual = sistema_actual;
        this.nombre_objeto = nombre;
        this.maquina_estado_actual = Maquina_estado.ME.primera;
        if (this.sistema_actual == Maquina_estado.Sistemas.S_planetario)
        {
            Sistema_Planetario.guardar_objeto_seleccionado(nombre);
        }
    }

    public void guardar_posicion_objeto_seleccionado(Maquina_estado.Sistemas sistema_actual, float x, float y, float z)
    {
        this.sistema_actual = sistema_actual;
        this.x = x;
        this.y = y;
        this.z = z;
        if (this.sistema_actual == Maquina_estado.Sistemas.S_planetario)
        {
            this.maquina_estado_actual = Maquina_estado.ME.segunda;
        }
        else
        {
            this.maquina_estado_actual = Maquina_estado.ME.ninguna;
        }
    }

    public void deseleccionar_objeto()
    {
        objeto_seleccionado.objeto_seleccionado = false;
    }

    public void enviar_transicion(transiciones transicion)
    {
        this.transicion = transicion;
    }
}