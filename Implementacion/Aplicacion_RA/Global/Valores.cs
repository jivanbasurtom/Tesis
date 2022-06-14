using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Valores : MonoBehaviour
{
    public enum Parametros
    {
        posicion_x,
        posicion_y,
        Masa,
        Velocidad_x,
        velocidad_y, 
        direccion_velocidad,
        numero_dientes,
        modulo,
        fuerza,
        masa_plataforma_A,
        longitud_plataforma_A,
        masa_plataforma_B,
        longitud_plataforma_B,
        longitud_martillo,
        masa_martillo,
        objeto_lanzado,
        masa_tierra,
        masa_sol,
        velocidad_tierra,
        distancia_tierra,
        Constante_Gravitacion
    }

    public enum Tarea
    {
        crear, 
        eliminar,
        nada
    }

    public Dictionary<Parametros, float> Constantes = new Dictionary<Parametros, float>()
    {
        {Parametros.masa_tierra, 5.97e+10f},
        {Parametros.masa_sol, 1988500e+24f},
        {Parametros.velocidad_tierra, 29800},
        {Parametros.distancia_tierra, 149.6e+9f},
        {Parametros.Constante_Gravitacion, 6.672e-11f}
    };

    void Start()
    {   
        
    }
}