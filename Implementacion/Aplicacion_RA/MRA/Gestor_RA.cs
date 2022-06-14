using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gestor_RA : MonoBehaviour
{
    private Dictionary<Maquina_estado.Sistemas, int> sistemas = new Dictionary<Maquina_estado.Sistemas, int>()
    {
        {Maquina_estado.Sistemas.ninguno, 1},
        {Maquina_estado.Sistemas.S_planetario, 1}
    };
    private int numero_objetos = 2;
    private Maquina_estado.Sistemas Sistema_actual, Sistema_actual_anterior;
    public Vector3 posicion = new Vector3(0, 0, 0);
    public Quaternion rotacion = Quaternion.Euler(0, 0, 0);

    private Sistema sistema;
    private bool cambios = false;

    int contador = 0;
    private Transform Filtro;

    private bool pruebas; 

    void Start()
    {
        Sistema_actual = Maquina_estado.Sistemas.ninguno;
        Sistema_actual_anterior = Maquina_estado.Sistemas.ninguno;

        GameObject Sistema_objeto = GameObject.Find("Sistema");
        sistema = Sistema_objeto.GetComponent<Sistema>();
        Sistema_actual = Maquina_estado.Sistemas.ninguno;

        pruebas = false;
        Filtro = this.gameObject.GetComponent<Transform>();
    }

    void Update()
    {
        if(pruebas == true)
        {
            simular();
        }
        else
        {
            if(Sistema_actual != Sistema_actual_anterior)
            {
                sistema.sistema_eliminar(Sistema_actual_anterior);
                sistema.sistema_crear(Sistema_actual);
                Sistema_actual_anterior = Sistema_actual;
            }
            Filtro.transform.rotation = this.rotacion;
            Filtro.transform.position = this.posicion;
        }

    }

    public void Iniciar(Maquina_estado.Sistemas nombre_sistema)
    {
        sistemas[nombre_sistema] += 1;
        numero_objetos++;
        cambios = true;
    }

    public void Borrar(Maquina_estado.Sistemas nombre_sistema)
    {
        sistemas[nombre_sistema] -= 1;
        numero_objetos--;
        cambios = true;
    }
    
    public bool Obtener_permiso(Maquina_estado.Sistemas nombre_sistema)
    {
        contador++;
        if(contador == numero_objetos)
        {
            cambios = false;
            contador = 0;
        }

        return (nombre_sistema == this.Sistema_actual) ? true : false;
    }

    public void Actualizar(ref float X, ref float Y, ref float Z, ref float Gx, ref float Gy, ref float Gz)
    {
        this.posicion = new Vector3(X, Y, Z);
        this.rotacion = Quaternion.Euler(Gx, Gy, Gz);
    }

    public bool hay_cambios()
    {
        return cambios;
    }

    public void Actualizar_sistema()
    {
        if(sistemas[Maquina_estado.Sistemas.ninguno] == 1)
        {
            Sistema_actual = Maquina_estado.Sistemas.ninguno;
            return;
        }
        if(sistemas[Maquina_estado.Sistemas.S_planetario] == 1)
        {
            Sistema_actual = Maquina_estado.Sistemas.S_planetario;
            return;
        }
        if((sistemas[Maquina_estado.Sistemas.ninguno] == 0) && (sistemas[Maquina_estado.Sistemas.S_planetario] == 0))
        {
             Sistema_actual = Maquina_estado.Sistemas.ninguno;
            return;
        }
    }

    private void simular()
    {
        if(Sistema_actual != Sistema_actual_anterior)
        {
            sistema.sistema_eliminar(Sistema_actual_anterior);
            sistema.sistema_crear(Sistema_actual);
            Sistema_actual_anterior = Sistema_actual;
        }

        if(contador == 100)
        {
            Sistema_actual = Maquina_estado.Sistemas.ninguno;
        }

        if(contador == 200)
        {
            Sistema_actual = Maquina_estado.Sistemas.S_planetario;
        }
        contador++;
    }
}
