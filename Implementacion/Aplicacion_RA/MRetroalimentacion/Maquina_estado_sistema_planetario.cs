using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Maquina_estado_sistema_planetario : MonoBehaviour
{
    public int numero;
    private Maquina_estado.transiciones transicion;
    public Objeto_astronomico Objeto;
    public string nombre_objeto;

    private enum SP
    {
        agregar,
        estrella,
        planeta,
        modificar,
        posicion,
        direccion_velocidad,
        magnitud_velocidad,
        masa,
        eliminar,
        seguro,
        mostrar,
        informacion,
        ninguno,
        reiniciar,
        inicial
    }

    private Dictionary<SP, string> SP_estados = new Dictionary<SP, string>()
    {
        {SP.agregar, "Agregar"},
        {SP.estrella, "Estrella"},
        {SP.planeta, "Planeta"},
        {SP.modificar, "Modificar"},
        {SP.posicion, "Posicion"},
        {SP.direccion_velocidad, "Direccion de velocidad"},
        {SP.magnitud_velocidad, "Magnitud de velocidad"},
        {SP.masa, "Masa"},
        {SP.eliminar, "Eliminar"},
        {SP.seguro, "¿Seguro?"},
        {SP.mostrar, "Mostrar"},
        {SP.informacion, "Informacion"},
        {SP.ninguno, ""},
        {SP.reiniciar, ""},
        {SP.inicial, ""}
    };

    SP estado;
    private Maquina_estado maquina_estado;
    private Objeto_seleccionado objeto_seleccionado;
    private UI_texto UI;
    private float posicion_x, posicion_y;

    private Sistema Sistema_clase;
    private Sistema_Planetario Sistema_Planetario_clase;

    private BT BT_clase;
    void Start()
    {
        numero = 0;
        this.estado = SP.inicial;
        GameObject Maquina_estado_objeto = GameObject.Find("/Modulo_Retroalimentacion/Maquina_estado");
        maquina_estado = Maquina_estado_objeto.GetComponent<Maquina_estado>();

        GameObject Objeto_Seleccionado_objeto = GameObject.Find("/Modulo_Integracion/Objeto_Seleccionado");
        objeto_seleccionado = Objeto_Seleccionado_objeto.GetComponent<Objeto_seleccionado>();

        GameObject UI_texto_objeto = GameObject.Find("/UI");
        UI = UI_texto_objeto.GetComponent<UI_texto>();

        GameObject Sistema_objeto = GameObject.Find("Sistema");
        Sistema_clase = Sistema_objeto.GetComponent<Sistema>();

        GameObject Sistema_Planetario_objeto = GameObject.Find("Sistema_Planetario");
        Sistema_Planetario_clase = Sistema_Planetario_objeto.GetComponent<Sistema_Planetario>();

        GameObject BT_objeto = GameObject.Find("BT");
        BT_clase = BT_objeto.GetComponent<BT>();
    }

    void Update()
    {
        
    }

    public void ME_SP_segunda(Maquina_estado.transiciones transicion_recibida)
    {
        transicion = transicion_recibida;
        switch (this.estado)
        {
            case SP.inicial:
                this.estado = SP.agregar;
                
            break;

            case SP.agregar:
                if(transicion == Maquina_estado.transiciones.mantener_derecho){
                    reiniciar(); }
                
                if(transicion == Maquina_estado.transiciones.mantener_izquierdo){
                    this.estado = SP.planeta;   }
            break;

            case SP.planeta:
                if(transicion == Maquina_estado.transiciones.mantener_derecho){
                    this.estado = SP.agregar;   }

                if(transicion == Maquina_estado.transiciones.mantener_izquierdo){
                    Sistema_clase.agregar_al_sistema_planetario_guardar(false, maquina_estado.x, maquina_estado.y);
                    UI.borrar();
                    reiniciar();   }

                if(transicion == Maquina_estado.transiciones.presionar_izquierdo){
                    this.estado = SP.estrella;  }
            break;

            case SP.estrella:
                if(transicion == Maquina_estado.transiciones.mantener_derecho){
                    this.estado = SP.agregar;   }

                 if(transicion == Maquina_estado.transiciones.mantener_izquierdo){
                    Sistema_clase.agregar_al_sistema_planetario_guardar(true, maquina_estado.x, maquina_estado.y);
                    UI.borrar();
                    reiniciar();   }

                if(transicion == Maquina_estado.transiciones.presionar_izquierdo){
                    this.estado = SP.planeta;   }
            break;

            default:

            break;
        };
        
        Debug.Log(this.estado.ToString());
        UI.cambiar_estado(SP_estados[this.estado]);
        this.maquina_estado.transicion = Maquina_estado.transiciones.nada;
    }

    public void ME_SP_primera(Maquina_estado.transiciones transicion_recibida)
    {
        transicion = transicion_recibida;
        switch (this.estado)
        {
            case SP.inicial:
                this.estado = SP.modificar;
                this.Objeto.desactivar_modelo();
                this.Objeto.calcular_direccion();
                this.Objeto.calcular_magnitud();
                this.Objeto.calcular_magnitud_unitaria();
                this.Objeto.calcular_masa_unitaria();
                BT_clase.habilitar_home = false;
                break;

            case SP.modificar:
                if(transicion == Maquina_estado.transiciones.mantener_derecho){
                    this.Objeto.activar_modelo();
                    UI.borrar();
                    UI.sin_movimiento();
                    reiniciar();    }

                if(transicion == Maquina_estado.transiciones.mantener_izquierdo){
                    this.estado = SP.posicion;
                    entrar_posicion();
                    UI.borrar();    }

                if(transicion == Maquina_estado.transiciones.presionar_izquierdo){
                    this.estado = SP.eliminar;  }
                
                if(transicion == Maquina_estado.transiciones.presionar_derecho){
                    this.estado = SP.eliminar;  }
                break;

            case SP.posicion:
                if(transicion == Maquina_estado.transiciones.mantener_derecho){
                    this.estado = SP.modificar;
                    salir_posicion(false);
                    entrar_modificar();
                    UI.borrar();    }
                
                if(transicion == Maquina_estado.transiciones.mantener_izquierdo){
                    this.estado = SP.direccion_velocidad;
                    salir_posicion(true);
                    entrar_direccion_velocidad();
                    UI.borrar();  }

                guardar_reiniciar_x_y();

                if(transicion == Maquina_estado.transiciones.nada){
                    (this.posicion_x, this.posicion_y) = UI.user_x_y();
                    this.Objeto.actualizar_posicion(this.posicion_x, this.posicion_y);  }
                break;

            case SP.direccion_velocidad:
                if(transicion == Maquina_estado.transiciones.mantener_derecho){
                    this.estado = SP.posicion;
                    salir_direccion_velocidad(false);
                    entrar_posicion();
                    UI.borrar();    }
                
                if(transicion == Maquina_estado.transiciones.mantener_izquierdo){
                    this.estado = SP.magnitud_velocidad;
                    salir_direccion_velocidad(true);
                    entrar_magnitud_velocidad();
                    UI.borrar();    }

                guardar_reiniciar_z();

                if(transicion == Maquina_estado.transiciones.nada){
                    this.Objeto.actualizar_direccion(UI.user_z());  }
                break;

            case SP.magnitud_velocidad:
                if(transicion == Maquina_estado.transiciones.mantener_derecho){
                    this.estado = SP.direccion_velocidad;
                    salir_magnitud_velocidad(false);
                    entrar_direccion_velocidad();
                    UI.borrar();    }
                
                if(transicion == Maquina_estado.transiciones.mantener_izquierdo){
                    this.estado = SP.masa;
                    salir_magnitud_velocidad(true);
                    entrar_masa();
                    UI.borrar();    }

                guardar_reiniciar_z();

                if(transicion == Maquina_estado.transiciones.nada)
                {
                    this.Objeto.actualizar_magnitud(UI.user_z());
                }
                break;
                
            case SP.masa:
                if(transicion == Maquina_estado.transiciones.mantener_derecho){
                    this.estado = SP.magnitud_velocidad;
                    salir_masa(false);
                    entrar_magnitud_velocidad();
                    UI.borrar();    }
                
                if(transicion == Maquina_estado.transiciones.mantener_izquierdo){
                    salir_masa(true);
                    this.Objeto.activar_modelo();
                    UI.borrar();
                    UI.sin_movimiento();
                    reiniciar();    }

                guardar_reiniciar_z();

                if(transicion == Maquina_estado.transiciones.nada && this.estado != SP.inicial){
                    this.Objeto.actualizar_masa_unitaria(this.UI.user_z()); }
                break;
            
            case SP.eliminar:
                if(transicion == Maquina_estado.transiciones.mantener_derecho){
                    this.Objeto.activar_modelo();
                    UI.borrar();
                    UI.sin_movimiento();
                    reiniciar();    }

                if(transicion == Maquina_estado.transiciones.mantener_izquierdo){
                    this.estado = SP.seguro;    }

                if(transicion == Maquina_estado.transiciones.presionar_izquierdo){
                    this.estado = SP.modificar;  }

                if(transicion == Maquina_estado.transiciones.presionar_derecho){
                    this.estado = SP.modificar;  }
                break;

            case SP.seguro:
                if(transicion == Maquina_estado.transiciones.mantener_derecho){
                    this.estado = SP.eliminar;  }

                if(transicion == Maquina_estado.transiciones.mantener_izquierdo){
                    reiniciar();
                    eliminar_estrella_planeta();    }

                if(transicion == Maquina_estado.transiciones.presionar_izquierdo){
                    this.estado = SP.modificar;  }

                if(transicion == Maquina_estado.transiciones.presionar_derecho){
                    this.estado = SP.modificar;  }
                break;
                
            default:
                break;
        };
        Debug.Log(this.estado.ToString());
        UI.cambiar_estado(SP_estados[this.estado]);
        this.maquina_estado.transicion = Maquina_estado.transiciones.nada;
    }

    private void reiniciar()
    {
        this.maquina_estado.maquina_estado_actual = Maquina_estado.ME.ninguna;
        this.objeto_seleccionado.deseleccionar_objeto();
        this.estado = SP.inicial;
        BT_clase.habilitar_home = true;
    }

    public void guardar_objeto_seleccionado(string nombre_objeto)
    {
        this.nombre_objeto = nombre_objeto;
        GameObject Objeto = GameObject.Find(nombre_objeto);
        this.Objeto = Objeto.GetComponent<Objeto_astronomico>();
    }

    void entrar_modificar()
    {
        UI.movimiento_ninguno();
    }

    void entrar_posicion()
    {
        (this.posicion_x, this.posicion_y) = this.Objeto.posicion();  
        UI.movimiento_x_y(this.posicion_x,  this.posicion_y);   
    }

    void entrar_direccion_velocidad()
    {
        this.Objeto.calcular_direccion();
        UI.movimiento_z(Objeto.direccion);
    }

    void entrar_magnitud_velocidad()
    {
        this.Objeto.calcular_magnitud();    
        this.Objeto.calcular_magnitud_unitaria();
        UI.movimiento_z(this.Objeto.magnitud_unitaria);
    }

    void entrar_masa()
    {
        this.Objeto.calcular_masa_unitaria();
        UI.movimiento_z(this.Objeto.masa_unitaria);
    }

    void salir_posicion(bool avanzar)
    {
        (this.posicion_x, this.posicion_y) = (avanzar == true) ? UI.avanzar_x_y() : UI.regresar_x_y();
        this.Objeto.actualizar_posicion(this.posicion_x, this.posicion_y);
    }

    void salir_direccion_velocidad(bool avanzar)
    {
        this.Objeto.actualizar_direccion((avanzar == true) ? UI.avanzar_z() : UI.regresar_z());
        this.Objeto.actualizar_velocidad();
    }

    void salir_magnitud_velocidad(bool avanzar)
    {
        this.Objeto.actualizar_magnitud((avanzar == true) ? UI.avanzar_z() : UI.regresar_z());
        this.Objeto.actualizar_velocidad();
    }

    void salir_masa(bool avanzar)
    {
        this.Objeto.actualizar_masa_unitaria((avanzar == true) ? UI.avanzar_z() : UI.regresar_z());
    }

    void guardar_reiniciar_x_y()
    {
        if(transicion == Maquina_estado.transiciones.presionar_derecho){
            UI.reiniciar_x_y(); }

        if(transicion == Maquina_estado.transiciones.presionar_izquierdo){
            UI.guardar_x_y();   }
    }

    void guardar_reiniciar_z()
    {
        if(transicion == Maquina_estado.transiciones.presionar_derecho){
            UI.reiniciar_z(); }

        if(transicion == Maquina_estado.transiciones.presionar_izquierdo){
            UI.guardar_z();}
    }

    void eliminar_estrella_planeta()
    {
        if(this.Objeto.estrella == true && this.Sistema_Planetario_clase.numero_estrellas > 1)
        {
            Sistema_clase.eliminar_del_sistema_planetario_guardar(nombre_objeto);
            return;
        }
        if(this.Objeto.estrella == false && this.Sistema_Planetario_clase.numero_planetas > 1)
        {
            Sistema_clase.eliminar_del_sistema_planetario_guardar(nombre_objeto);
            return;
        }
        this.Objeto.activar_modelo();
    }
}