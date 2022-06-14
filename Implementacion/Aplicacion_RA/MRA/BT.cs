using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class BT : MonoBehaviour
{
    private bool isconnected;
    public string datareceived = "";
    private string datain;
    private string Data_Removida_F;
    private string Data_Removida;
    private int index_fin;
    private int indice_cortado;
    private char[] cadena;

    private int contador_muestras = 0;
    private int contador_iteraciones = 0;

    private char[] cadena_valores;
    private char cadena_led;
    private char caracter;

    private char[] encriptar = {'0', '1', '2', '3', '4', '5', '6', '7',
                    '8', '9', ':', ';', 'A', 'B', 'C', 'D',
                    'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L',
                    'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T',
                    'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b',
                    'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j',
                    'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r',
                    's', 't', 'u', 'v', 'w', 'x', 'y', 'z'};

    private float gyro_constante = 500;
    private float accel_constante = 2;
    private float Ax = 0, Ay = 0, Az = 1, Gx = 0, Gy = 0, Gz = 0;
    private float Axp = 0, Ayp = 0, Azp = 0;
    private int posicion;
    private float Alto, Bajo;
    enum constante{gyro, accel};
    private float Rx = 1, Ry = 1, Rz = 1;

    private float T = 1/30f;
    private float innovacion_1 = 0, innovacion_2 = 0;
    private float kalman_1 = 0, kalman_2 = 0;
    private float error_proces_1 = 0.8f, error_proces_2 = 8;
    private float error_obser_1 = 0.8f, error_obser_2 = 8; 
    private float Ax_est = 1, Gx_est = 10, Ax_est_error = 2, Gx_est_error = 20;
    private float Ay_est = 1, Gy_est = 10, Ay_est_error = 2, Gy_est_error = 20;
    private float Az_est = 1, Gz_est = 10, Az_est_error = 2, Gz_est_error = 20;

    private float Ax_home = 0, Ay_home = 0, Az_home; 
 
    //filtro complentario
    private float Ax_ant = 0, Ay_ant = 0, Az_ant = 0;
    private float Ax_act = 0, Ay_act = 0, Az_act = 0; 
    private float Gx_off = .48828f, Gy_off = -.244f, Gz_off = -.535f; //-0.530f -.488f

    private bool clic_izquierdo, clic_derecho;

    float G, Goff;
    public bool habilitar_home = true;

    void Start()
    {
        isconnected = false;
        BluetoothService.CreateBluetoothObject();
        isconnected = BluetoothService.StartBluetoothConnection("HC-05");
        cadena = "U0U0U0U0U0U00".ToCharArray();
        cadena_valores = "U0U0U0U0U0U0".ToCharArray();
        cadena_led = '0';
    }

    void Update()
    {
        if(isconnected)
        {
            BluetoothService.WritetoBluetooth("1");
        }

        if(isconnected)
        {
            try
            {
                datain = BluetoothService.ReadFromBluetooth();
                if(datain.Length > 1)
                {
                    datareceived += datain;
                }

                Data_Removida = "";
                Data_Removida_F = "";
                
                if(datareceived.Length >= 15)
                {
                    index_fin = datareceived.IndexOf("zz", 0, 15);
                }
                else
                {
                    index_fin = datareceived.IndexOf("zz", 0, datareceived.Length);
                }
                    
                if(index_fin == -1)
                {

                }
                else
                {
                    if(index_fin >= 13)
                    {
                        Data_Removida_F = datareceived.Substring(index_fin - 13, 15);
                        datareceived = datareceived.Remove(index_fin - 13, 15);
                        Data_Removida = Data_Removida_F.Substring(0, 13);
                        agregar_cadena(Data_Removida);
                        contador_muestras++;
                    }
                    else if(index_fin < 13)
                    {
                        Data_Removida_F = datareceived.Substring(0, index_fin + 2);
                        datareceived = datareceived.Remove(0, index_fin + 2);

                        if(index_fin > 0)
                        {
                            indice_cortado = index_fin;
                            Data_Removida = Data_Removida_F.Substring(0, index_fin);

                            char[] cadena_incompleta = Data_Removida.ToCharArray();

                            indice_cortado--;
                            int j = 12;
                            for(int i = indice_cortado; indice_cortado >= 0; indice_cortado--)
                            {
                                agregar_cadena_incompleta(j, cadena_incompleta[indice_cortado]);
                                j--;
                            }
                            contador_muestras++;
                        }
                    }
                    desencriptar_valor();
                    acelerometro_a_posicion_angular();
                    Aplicar_Filtro_Kalman();
                    
                    Gx =  (Mathf.Round(Gx * 100)/100) - Gx_off;
                    Gy =  (Mathf.Round(Gy * 100)/100) - Gy_off;
                    Gz =  (Mathf.Round(Gz * 100)/100) - Gz_off;
                    
                    Ax_act = 0.9f * ( Ax_ant + ( Gx * T ) ) + ( 0.1f * Axp );
                    Ay_act = 0.9f * ( Ay_ant + ( Gy * T ) ) + ( 0.1f * Ayp );
                    Az_act = Az_ant + ( Gz * T );
                    
                    Ax_ant = Ax_act;
                    Ay_ant = Ay_act;
                    Az_ant = Az_act;

                    Leer_Pulsadores();

                    volver_home();
                }
            }
            catch(Exception ex)
            {
                
            }
        }
        if(isconnected)
        {
            contador_iteraciones++;
            float periodo = (((float)contador_muestras)/((float)contador_iteraciones)) * (1/Time.deltaTime);
        }
    }

    void agregar_cadena(string cadena_completa)
    {
        this.cadena = cadena_completa.ToCharArray();
    }

    void agregar_cadena_incompleta(int pos, char caracter)
    {
        this.cadena[pos] = caracter;
    }

    void separar_cadena()
    {
        for(int i = 0; i < 12; i++)
        {
            this.cadena_valores[i] = cadena[i];
        }
        this.cadena_led = cadena[12]; 
    }

    float desencriptar_letra()
    {
        caracter = cadena_valores[posicion];
        posicion++;
        for(int i = 0; i < 64; i++)
        {
            if(caracter == encriptar[i])
            {
                return (float) i;
            }
        }
        return 0.0f;
    }

    void desencriptar_Alto_Bajo()
    {
        this.Alto = desencriptar_letra();
        this.Bajo = desencriptar_letra();
    }

    float desnormalizar(constante constante)
    {
        return ( ( ( ( (Alto * 64) + Bajo ) - 2048 ) ) / 2048) * (constante == constante.gyro ? this.gyro_constante : this.accel_constante);
    }

    void desencriptar_valor()
    {
        separar_cadena();
        this.posicion = 0;
        desencriptar_Alto_Bajo();
        this.Ax = desnormalizar(constante.accel); 
        desencriptar_Alto_Bajo();
        this.Ay = desnormalizar(constante.accel);
        desencriptar_Alto_Bajo();
        this.Az = desnormalizar(constante.accel); 
        desencriptar_Alto_Bajo();
        this.Gx = desnormalizar(constante.gyro); 
        desencriptar_Alto_Bajo();
        this.Gy = desnormalizar(constante.gyro); 
        desencriptar_Alto_Bajo();
        this.Gz = desnormalizar(constante.gyro); 
    }

    void acelerometro_a_posicion_angular()
    {
        Rx = Mathf.Sqrt( Mathf.Pow(Ay, 2) + Mathf.Pow(Az, 2) );
        Ry = Mathf.Sqrt( Mathf.Pow(Ax, 2) + Mathf.Pow(Az, 2) );
        //Rz = Mathf.Sqrt( Mathf.Pow(Ax, 2) + Mathf.Pow(Ay, 2) );        
        this.Axp = Mathf.Atan2( Ax , Rx ) * Mathf.Rad2Deg;
        this.Ayp = Mathf.Atan2( Ay , Ry ) * Mathf.Rad2Deg;
        //this.Azp = Mathf.Atan2( Az , Rz ) * Mathf.Rad2Deg;
    }

    void Aplicar_Filtro_Kalman()
    {

        Filtro_Kalman(ref Axp, ref Gx, ref Ax_est, ref Gx_est, ref Ax_est_error, ref Gx_est_error);
        Filtro_Kalman(ref Ayp, ref Gy, ref Ay_est, ref Gy_est, ref Ay_est_error, ref Gy_est_error);
        Filtro_Kalman(ref Az_ant, ref Gz, ref Az_est, ref Gz_est, ref Az_est_error, ref Gz_est_error);
        
    }

    void Filtro_Kalman(ref float sensor_x1, ref float sensor_x2, ref float est_x1, ref float est_x2, ref float est_error_1, ref float est_error_2)
    {
        
        est_x1 = est_x1 + ( est_x2 * T ); //estimacion del estado
        
        est_error_1 = est_error_1 + ( est_error_2 * ( T * T ) ) + error_proces_1; //estimacion del error
        est_error_2 = est_error_2 + error_proces_2;

        innovacion_1 = sensor_x1 - est_x1; //medida de la innovacion
        innovacion_2 = sensor_x2 - est_x2;

        kalman_1 = est_error_1 * (1 / (est_error_1 * error_obser_1 ) ); //ganancia Kalman
        kalman_2 = est_error_2 * (1 / (est_error_2 * error_obser_2 ) );

        est_x1 = est_x1 + ( kalman_1 * innovacion_1 ); //actualizacion de la estimacion del estado
        est_x2 = est_x2 + ( kalman_2 * innovacion_2 );

        est_error_1 = ( 1 - kalman_1 ) * est_error_1; //actualizacion de la estimacion del error
        est_error_2 = ( 1 - kalman_2 ) * est_error_2;
    }

    void Leer_Pulsadores()
    {
        if(cadena_led == '0')
        {
            clic_derecho = false;
            clic_izquierdo = false;
        }
        else if(cadena_led == '1')
        {
            clic_derecho = false;
            clic_izquierdo = true;
        }
        else if(cadena_led == '2')
        {
            clic_derecho = true;
            clic_izquierdo = false;
        }
        else
        {
            clic_derecho = true;
            clic_izquierdo = true;
        }
    }

    public bool leer_clic_izquierdo()
    {
        return this.clic_izquierdo;
    }

    public bool leer_clic_derecho()
    {
        return clic_derecho;
    }

    public float obtener_ang_x()
    {
        return  -((Az_act - Az_home)/1.5f);//angulo_z.text = (Ax_act - Ax_home).ToString();
    }

    public float obtener_ang_y()
    {
        return (Ay_act - Ay_home)/3; //angulo_x.text = (Ay_act - Ay_home).ToString(); 
    }

    public float obtener_ang_z()
    {
        return  (Ax_act - Ax_home);//angulo_y.text = (Az_act - Az_home).ToString();
    }

    public void volver_home()
    {
        if(this.habilitar_home == true)
        {
            if(clic_derecho == true)
            {
                Ay_home = Ay_act;
                Az_home = Az_act;
                Ax_home = Ax_act;
            }
        }
    }
}