using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Escenario : MonoBehaviour
{
    public GameObject RA_obj;
    private Transform RA_transform;
    
    Vector3 PA_1_vec;
    Vector3 PB_1_vec;
    private Vector3 PA_vec = new Vector3(0, 0, 0);
    private Vector3 PB_vec;

    public float[][] RA;
    public Vector3 TA; 
    public float[][] RA_1 =
        {
            new float[3] {0, 0, 0},
            new float[3] {0, 0, 0},
            new float[3] {0, 0, 0}
        };

    void Start()
    {
        RA_transform = RA_obj.GetComponent<Transform>();
        RA = Transformaciones.Quaternion_to_matrix(RA_transform.rotation);
    }

    void Update()
    {
        RA = Transformaciones.Quaternion_to_matrix(RA_transform.rotation);
        TA = RA_transform.position;
        RA_1 = Transformaciones.Inverse(RA);       
    }
}