using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sistema_Planetario : MonoBehaviour
{
    public List<Objeto_astronomico> objetos = new List<Objeto_astronomico>();
    private int numero_objetos = 0;
    public bool REE, REP, MCeE;
    private Objeto_astronomico origen, destino;
    private float dt = 7380; //4380
    private Valores Vals;
    public int numero_planetas = 0, numero_estrellas = 0;
    private Valores.Tarea tarea;
    private bool bloquear_algoritmo_bool;
    private Sistema sistema;

    void Start()
    {
        GameObject sistema_objeto = GameObject.Find("Sistema");
        sistema = sistema_objeto.GetComponent<Sistema>();

        this.REE = false;
        this.REP = false;
        this.MCeE = false;
    }

    void Update()
    {
        if(bloquear_algoritmo_bool == true)
        {
            if(tarea == Valores.Tarea.crear)
            {
                sistema.agregar_al_sistema_planetario(false, 0, 0, true);
            }

            if(tarea == Valores.Tarea.eliminar)
            {
                sistema.eliminar_del_sistema_planetario("", true);
            }
            bloquear_algoritmo_bool = false;
        }
        else
        {
            Algoritmo();
        }
    }

    private void Algoritmo()
    {
        for (int i = 0; i < numero_objetos; i++)
        {
            origen = objetos[i];
            if(origen.estrella == true) //si es estrella
            {
                if(this.MCeE == true)
                {
                    for (int j = i + 1; j < numero_objetos; j++)
                    {
                        destino = objetos[j];
                        if(!(!REE && destino.estrella))
                        {
                            Calcular_aceleracion(origen, destino);
                        }
                    }
                }
                else
                {
                    for (int j = i + 1; j < numero_objetos; j++)
                    {
                        destino = objetos[j];
                        Calcular_aceleracion(origen, destino);
                    }
                }
            }
            else //si es planeta
            {
                for (int j = i + 1; j < numero_objetos; j++)
                {
                    destino = objetos[j];
                    if(REP || destino.estrella)
                    {
                        Calcular_aceleracion(origen, destino);
                    }
                }
            }
        }

        for(int i = 0; i < this.numero_objetos; i++)
        {
            if(!MCeE && objetos[i].estrella)
            {
                continue;
            }
            else
            {
                Aplicar_modelo_computacional(objetos[i]);
            }
        }
        Reiniciar_aceleracion();
    }

    private void Reiniciar_aceleracion()
    {
        foreach (Objeto_astronomico objeto in objetos)
        {
            objeto.Reiniciar_aceleracion();
        }
    }

    private void Calcular_aceleracion(Objeto_astronomico ori, Objeto_astronomico dest)
    {
        float xi, yi, xj, yj, x, y;
        float distancia_cuadrado, distancia, distancia_cubo;
        float aceleracion, aceleracion_x, aceleracion_y;

        xi = ori.posicion_x;
        yi = ori.posicion_y;
        xj = dest.posicion_x;
        yj = dest.posicion_y;
        
        x = xj - xi;
        y = yj - yi;

        distancia_cuadrado = x*x + y*y;
        distancia = Mathf.Sqrt(distancia_cuadrado);
        distancia_cubo = distancia_cuadrado * distancia;

        aceleracion = dest.Gm / distancia_cubo;
        aceleracion_x = aceleracion * x;
        aceleracion_y = aceleracion * y;

        ori.aceleracion_x += aceleracion_x;
        ori.aceleracion_y += aceleracion_y;

        if(!ori.estrella && dest.estrella && !MCeE)
        {
            return; 
        }
        else
        {
            dest.aceleracion_x -= x * ori.Gm / distancia_cubo;
            dest.aceleracion_y -= y * ori.Gm / distancia_cubo;
        }
    }

    private void Aplicar_modelo_computacional(Objeto_astronomico ori)
    {
        if (ori.aplicar_modelo == true)
        {
            ori.posicion_x += ori.velocidad_x * dt;
            ori.velocidad_x += ori.aceleracion_x * dt;
            ori.posicion_y += ori.velocidad_y * dt;
            ori.velocidad_y += ori.aceleracion_y * dt;
        }
        else
        {
            //el objeto esta siendo modificado
        }
    }

    public void limpiar_lista_objetos()
    {
        objetos.Clear();
    }

    public void actualizar_numero_objetos()
    {
        numero_objetos = objetos.Count;
    }

    public void add_numero_estrellas()
    {
        numero_estrellas++;
    }

    public void add_numero_planetas()
    {
        numero_planetas++;
    }

    public void quitar_numero_estrellas()
    {
        numero_estrellas--;
    }

    public void quitar_numero_planetas()
    {
        numero_planetas--;
    }

    public void borrar_numeros()
    {
        numero_objetos = 0;
        numero_estrellas = 0;
        numero_planetas = 0;
    }

    public void bloquear_algoritmo(Valores.Tarea tarea)
    {
        this.tarea = tarea;
        this.bloquear_algoritmo_bool = true;
    }
}