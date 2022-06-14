using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_texto : MonoBehaviour
{
    public Text texto_funcion;
    public Text texto_valor;
    private float posicion_x_original;
    private float posicion_y_original;
    private float valor_parametro_x;
    private float valor_parametro_y;
    private float valor_parametro_z;
    private float valor_movimiento_x;
    private float valor_movimiento_y;
    private float valor_movimiento_z;
    private float valor_movimiento_x_anterior;
    private float valor_movimiento_y_anterior;
    private float valor_movimiento_z_anterior;

    public enum movimientos
    {
        ninguno,
        traslacion_x_y,
        rotacion_z
    };

    private movimientos movimiento_Actual;
    private Dispositivo_pantalla dispositivo_inalambrico;
    private Maquina_estado.Sistemas Sistema;
    private Maquina_estado maquina_estado;
    private GameObject objeto;
    private string objeto_seleccionado;
    private Objeto_astronomico Objeto_astro;

    private float sensibilidad = 10;

    void Start()
    {
        GameObject Texto_objeto = GameObject.Find("/UI/Texto_funcion");
        texto_funcion = Texto_objeto.GetComponent<Text>();

        Texto_objeto = GameObject.Find("/UI/Texto_valor");
        texto_valor = Texto_objeto.GetComponent<Text>();

        texto_funcion.text = "";
        texto_valor.text = "";
        posicion_x_original = 0;
        posicion_y_original = 0;
        valor_parametro_x = 0;
        valor_parametro_y = 0;
        valor_parametro_z = 0;
        valor_movimiento_x = 0;
        valor_movimiento_y = 0;
        valor_movimiento_z = 0;
        valor_movimiento_x_anterior = 0;
        valor_movimiento_y_anterior = 0;
        valor_movimiento_z_anterior = 0;

        GameObject Dispositivo_virtual_objeto = GameObject.Find("/Modulo_Control/Dispositivo_inalambrico");
        dispositivo_inalambrico = Dispositivo_virtual_objeto.GetComponent<Dispositivo_pantalla>();

        movimiento_Actual = movimientos.ninguno;
        Sistema = Maquina_estado.Sistemas.S_planetario;

        GameObject Maquina_Estado_objeto = GameObject.Find("/Modulo_Retroalimentacion/Maquina_estado");
        maquina_estado = Maquina_Estado_objeto.GetComponent<Maquina_estado>();
        objeto_seleccionado = maquina_estado.nombre_objeto;
    }

    void Update()
    {
        switch (movimiento_Actual)
        {
            case movimientos.ninguno:
                this.texto_valor.text = "";
                break;
            case movimientos.traslacion_x_y:
                if(Sistema == Maquina_estado.Sistemas.S_planetario)
                {
                    this.valor_movimiento_x = dispositivo_inalambrico.ang_x;
                    this.valor_movimiento_y = dispositivo_inalambrico.ang_y;
                    this.texto_valor.text = (this.valor_parametro_x + (this.valor_movimiento_x - this.valor_movimiento_x_anterior) / sensibilidad).ToString() +", "+
                                        (this.valor_parametro_y + (this.valor_movimiento_y - this.valor_movimiento_y_anterior) / sensibilidad).ToString();                    
                }
                if(Sistema == Maquina_estado.Sistemas.S_engranes)
                {

                }
                if(Sistema == Maquina_estado.Sistemas.S_colisiones)
                {

                }
                break;
            case movimientos.rotacion_z:
                if(Sistema == Maquina_estado.Sistemas.S_planetario)
                {
                    this.valor_movimiento_z = dispositivo_inalambrico.ang_z;
                    this.texto_valor.text = (this.valor_parametro_z + (this.valor_movimiento_z - this.valor_movimiento_z_anterior) / sensibilidad).ToString();
                }
                if(Sistema == Maquina_estado.Sistemas.S_engranes)
                {

                }
                if(Sistema == Maquina_estado.Sistemas.S_colisiones)
                {

                }
            break;

            default:

            break;
        }
    }

    public void borrar()
    {
        texto_funcion.text = "";
        texto_valor.text = "";
    }

    public void sin_movimiento()
    {
        this.movimiento_Actual = movimientos.ninguno;
    }

    public void cambiar_estado(string estado)
    {
        texto_funcion.text = estado;
    }

    public void movimiento_z(float valor_inicial)
    {
        this.movimiento_Actual = movimientos.rotacion_z;
        this.valor_parametro_z = valor_inicial;
        this.valor_movimiento_z_anterior = dispositivo_inalambrico.ang_z; //cambiar por el eje z
    }

    public void movimiento_x_y(float valor_inicial_x, float valor_inicial_y)
    {
        this.movimiento_Actual = movimientos.traslacion_x_y;
        this.valor_parametro_x = valor_inicial_x;
        this.valor_parametro_y = valor_inicial_y;
        this.valor_movimiento_x_anterior = dispositivo_inalambrico.ang_x;
        this.valor_movimiento_y_anterior = dispositivo_inalambrico.ang_y;
    }

    public void inicial_x_y(float parametro_x, float parametro_y)
    {
        this.valor_movimiento_x_anterior = dispositivo_inalambrico.ang_x;
        this.valor_movimiento_y_anterior = dispositivo_inalambrico.ang_y;
        this.valor_parametro_x = parametro_x;
        this.valor_parametro_y = parametro_y;
    }

    public void inicial_z(float parametro_z)
    {
        this.valor_movimiento_z_anterior = dispositivo_inalambrico.ang_x;
        this.valor_parametro_z = parametro_z;
    }

    public float regresar_z()
    {
        return valor_parametro_z;
    }

    public (float, float) regresar_x_y()
    {
        return (this.valor_parametro_x, this.valor_parametro_y);
    }

    public void reiniciar_z()
    {
        this.valor_movimiento_z_anterior = this.valor_movimiento_z;
    }

    public void reiniciar_x_y()
    {
        this.valor_movimiento_x_anterior = this.valor_movimiento_x;
        this.valor_movimiento_y_anterior = this.valor_movimiento_y;
    }

    public float avanzar_z()
    {
        guardar_z();
        return this.valor_parametro_z;
    }

    public (float, float) avanzar_x_y()
    {
        guardar_x_y();
        return (this.valor_parametro_x, this.valor_parametro_y);
    }

    public void guardar_z()
    {
        this.valor_parametro_z += (this.valor_movimiento_z - this.valor_movimiento_z_anterior) / sensibilidad;
        this.valor_movimiento_z_anterior = this.valor_movimiento_z;
    }

    public void guardar_x_y()
    {
        this.valor_parametro_x += (this.valor_movimiento_x - this.valor_movimiento_x_anterior) / sensibilidad;
        this.valor_parametro_y += (this.valor_movimiento_y - this.valor_movimiento_y_anterior) / sensibilidad;
        this.valor_movimiento_x_anterior = this.valor_movimiento_x;
        this.valor_movimiento_y_anterior = this.valor_movimiento_y;
    }

    public void guardar_objeto_seleccionado(string nombre_objeto)
    {
        this.objeto_seleccionado = nombre_objeto;
        this.objeto = GameObject.Find(objeto_seleccionado);
    }

    public (float, float) user_x_y()
    {
        return (this.valor_parametro_x + (this.valor_movimiento_x - this.valor_movimiento_x_anterior) / sensibilidad, 
                                        this.valor_parametro_y + (this.valor_movimiento_y - this.valor_movimiento_y_anterior) / sensibilidad);
    }

    public float user_z()
    {
        return this.valor_parametro_z + (this.valor_movimiento_z - this.valor_movimiento_z_anterior) / sensibilidad;
    }

    public void capturar_movimientos()
    {
        this.valor_movimiento_x = dispositivo_inalambrico.ang_x;
        this.valor_movimiento_y = dispositivo_inalambrico.ang_y;
        this.valor_movimiento_z = dispositivo_inalambrico.ang_z;
    }

    void cambios(int contador)
    {
        texto_funcion.text = "cambio en funcion, " + contador.ToString();
        texto_valor.text = "cambio de valor, " + contador.ToString();
        return;
    }

    public void movimiento_ninguno()
    {
        this.movimiento_Actual = movimientos.ninguno;
    }
}