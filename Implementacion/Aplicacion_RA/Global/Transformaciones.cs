using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transformaciones : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public static float[][] Quaternion_to_matrix(Quaternion Q)
    {
        float w = Q.w;
        float x = Q.x;
        float y = Q.y;
        float z = Q.z;

        float[][] matrix =
        {
            new float[3] {1 - 2 * (y*y + z*z), 2 * (x*y - w*z), 2 * (w*y + x*z)},
            new float[3] {2 * (x*y + w*z), 1 - 2* (x*x + z*z), 2 * (y*z - w*x)},
            new float[3] {2 * (x*z - w*y), 2 * (w*x + y*z), 1 - 2 * (x*x + y*y)}
        };

    return matrix;
    }

    public static  Vector3 Matrix_x_vec(float [][] matrix, Vector3 pos)
    {
        Vector3 Vec = new Vector3(0, 0, 0);
    
        Vec.x = matrix[0][0] * pos.x + matrix[0][1] * pos.y + matrix[0][2] * pos.z;
        Vec.y = matrix[1][0] * pos.x + matrix[1][1] * pos.y + matrix[1][2] * pos.z;
        Vec.z = matrix[2][0] * pos.x + matrix[2][1] * pos.y + matrix[2][2] * pos.z;

        return Vec;
    }

    public static float[][] Inverse(float [][] matrix)
    {
        float[][] Adj =
        {
            new float[3] {0, 0, 0},
            new float[3] {0, 0, 0},
            new float[3] {0, 0, 0}
        };
        float det = 0;

        int k = 0, l = 0, m = 0, n = 0;
        for (int i = 0; i < 3; i++)
        {
            if (i == 1)
            {
                n = -1;
            }
            if (i == 2)
            {
                l = -1;
            }
            for (int j = 0; j < 3; j++)
            {
                if (j == 1)
                {
                    k = -1;
                }
                if(j == 2)
                {
                    m = -1;
                }

                Adj[i][j] = (matrix[1 + n][1 + k] * matrix[2 + l][2 + m] - matrix[2 + l][1 + k] * matrix[1 + n][2 + m]) * Mathf.Pow(-1, i+j);
            }
            k = 0;
            m = 0;
        }

        float aux;
        for (int i = 0; i < 2; i++)
            {
                for (int j = i + 1; j < 3; j++)
                {
                    aux = Adj[i][j];
                    Adj[i][j] = Adj[j][i];
                    Adj[j][i] = aux;
                }
            }

        for (int i = 0; i < 3; i++)
        {
            det += matrix[0][i] * Adj[i][0]; 
        }

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                Adj[i][j] = Adj[i][j] / det;
            }
        }
        return Adj;
    }
}