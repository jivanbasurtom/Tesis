using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RA_Obj : MonoBehaviour
{
    private Transform Transform_actual;
    public float X = 0, Y = 0, Z = 0, Rx = 0, Ry = 0, Rz = 0, Rw = 0;

    private float X_ant = 0, Y_ant = 0, Z_ant = 0, Rx_ant = 0, Ry_ant = 0, Rz_ant = 0;
    private float X_sal_ant = 0, Y_sal_ant = 0, Z_sal_ant = 0, Rx_sal_ant = 0, Ry_sal_ant = 0, Rz_sal_ant = 0;
    private float X_sal, Y_sal, Z_sal, Rx_sal, Ry_sal, Rz_sal;
    private float num_1 = 0.07412406f, num_2 = 0.85175187f;

    private Gestor_RA Gestor_RA_clase;
    public Maquina_estado.Sistemas nombre;
    private bool permiso = false;

    private MeshRenderer render;

    void Start()
    {
        GameObject Gestor_RA_objeto = GameObject.Find("Gestor_RA");
        Gestor_RA_clase = Gestor_RA_objeto.GetComponent<Gestor_RA>();
        render = this.gameObject.GetComponent<MeshRenderer>();
    }

    void Update()
    {
        if(Gestor_RA_clase.hay_cambios() == true)
        {
            permiso = Gestor_RA_clase.Obtener_permiso(nombre);
        }
        if(permiso == true)
        {
            obtener_filtrar_valores();
        }
    }

    public void OnInvoke()
    {
        Gestor_RA_clase.Iniciar(nombre);
        Gestor_RA_clase.Actualizar_sistema();
        render.enabled = false;
    }

    public void OnDelete()
    {
        GameObject Gestor_RA_objeto = GameObject.Find("Gestor_RA");
        Gestor_RA_clase = Gestor_RA_objeto.GetComponent<Gestor_RA>();
        Gestor_RA_clase.Borrar(nombre);
        Gestor_RA_clase.Actualizar_sistema();
    }
    
    private void obtener_filtrar_valores()
    {
        X = gameObject.transform.position.x;
        Y = gameObject.transform.position.y;
        Z = gameObject.transform.position.z;
        Rx = gameObject.transform.eulerAngles.x;
        Ry = gameObject.transform.eulerAngles.y;
        Rz = gameObject.transform.eulerAngles.z;

        X_sal = (X_sal_ant * num_2) + ((X + X_ant) * num_1);
        Y_sal = (Y_sal_ant * num_2) + ((Y + Y_ant) * num_1);
        Z_sal = (Z_sal_ant * num_2) + ((Z + Z_ant) * num_1);
        Rx_sal = (Rx_sal_ant * num_2) + ((Rx + Rx_ant) * num_1);
        Ry_sal = (Ry_sal_ant * num_2) + ((Ry + Ry_ant) * num_1);
        Rz_sal = (Rz_sal_ant * num_2) + ((Rz + Rz_ant) * num_1);

        X_sal_ant = X_sal;
        X_ant = X;
        Y_sal_ant = Y_sal;
        Y_ant = Y;
        Z_sal_ant = Z_sal;
        Z_ant = Z;
        Rx_sal_ant = Rx_sal;
        Rx_ant = Rx;
        Ry_sal_ant = Ry_sal;
        Ry_ant = Ry;
        Rz_sal_ant = Rz_sal;
        Rz_ant = Rz;

        float limite = 0.4f;
        if((Mathf.Abs(X_sal - X)) > limite )
        {
            X_sal = X;
        }
        if((Mathf.Abs(Y_sal - Y)) > limite )
        {
            Y_sal = Y;
        }
        if((Mathf.Abs(Z_sal - Z)) > limite )
        {
            Z_sal = Z;
        }
        if((Mathf.Abs(Rx_sal - Rx)) > limite )
        {
            Rx_sal = Rx;
        }
        if((Mathf.Abs(Ry_sal - Ry)) > limite )
        {
            Ry_sal = Ry;
        }
        if((Mathf.Abs(Rz_sal - Rz)) > limite )
        {
            Rz_sal = Rz;
        }
        Gestor_RA_clase.Actualizar(ref X_sal, ref Y_sal, ref Z_sal, ref Rx_sal, ref Ry_sal, ref Rz_sal);
    }
}