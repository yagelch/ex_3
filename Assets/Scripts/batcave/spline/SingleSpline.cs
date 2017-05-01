

using UnityEngine;

namespace BatCave.Spline {
public class SingleSpline {
    public float[] k_s;
    public float[] d_s;
    public float[] a_s;
    public float[] b_s;
    public float[] c_s;
    public float[] d_tags;
    public float[] c_tags;
    public float[] aCalc;
    public float[] bCalc;
    public int num_of_points;
    public Vector2[] controlPointscopy;
    public SingleSpline(Vector2[] controlPoints) {
        // TODO: Implement: initialize the spline to match the given control points.
        num_of_points = controlPoints.Length;
        k_s = new float[num_of_points];
        d_s = new float[num_of_points];
        a_s = new float[num_of_points];
        b_s = new float[num_of_points];
        c_s = new float[num_of_points];
        d_tags= new float[num_of_points];
        c_tags= new float[num_of_points];
        controlPointscopy = controlPoints;
        /*
        for (int k = 0; k < num_of_points; k++) {
            controlPointscopy [k] = controlPoints [k];
        }
        */
        //System.Array.Copy(controlPoints,controlPointscopy,num_of_points);

        for (int i = 0; i < num_of_points ; i++) {
            if (i == 0 ) {
                a_s [0] = 0;
                b_s [0] = 2 / (controlPoints [i + 1].x - controlPoints [i].x );
                c_s [0] = 1 / (controlPoints [i + 1] .x - controlPoints [i].x );
                d_s [0] = 3*(controlPoints [i + 1].y - controlPoints [i].y) /
                    Mathf.Pow((controlPoints [i + 1].x - controlPoints [i].x),2);
                c_tags [i] = c_s [i] / (b_s [i] - a_s [i]);
                d_tags [0] = d_s [0] / b_s [0];

            }
            else if (i == num_of_points -1) {
                a_s [i] = 1 / (controlPoints [i].x - controlPoints [i-1].x );
                c_s[i] = 0;
                b_s [i] = 2 / (controlPoints [i] .x - controlPoints [i-1].x );
                d_s [i] = 3*((controlPoints [i].y - controlPoints [i-1].y) /
                    Mathf.Pow((controlPoints [i].x - controlPoints [i-1].x),2));
                c_tags [i] = 0;
                d_tags [i] = (d_s [i] - a_s [i] * d_tags [i - 1])/
                    (b_s [i] - a_s [i]*c_tags [i-1]) ;


            }
            else{
                a_s [i] = 1 / (controlPoints [i].x  - controlPoints [i-1].x );
                b_s [i] = 2*((1 / (controlPoints [i].x  - controlPoints [i-1].x ))+
                    (1 / (controlPoints [i + 1].x  - controlPoints [i].x )));

                d_s [i] = 3*(((controlPoints [i ].y - controlPoints [i-1].y) /
                    Mathf.Pow((controlPoints [i].x  - controlPoints [i-1].x ),2))+
                    ((controlPoints [i+1 ].y - controlPoints [i].y) /
                        Mathf.Pow((controlPoints [i+1].x  - controlPoints [i].x ),2)));
                c_s [i] = 1 / (controlPoints [i + 1].x  - controlPoints [i].x );
                c_tags [i] = c_s [i] / (b_s [i] - a_s [i]);
                d_tags [i] = (d_s [i] - a_s [i] * d_tags [i - 1])/
                    (b_s [i] - a_s [i]*c_tags [i-1]);

            }
        }
        for (int j = num_of_points-1; j >= 0; j--) {
            if (j == num_of_points-1) {
                k_s [j] = d_tags [j];
            } 
            else {
                k_s [j] = d_tags [j] - c_tags [j] * k_s [j + 1];
            }
        }
        aCalc = new float[num_of_points];
        bCalc = new float[num_of_points];
        for (int i = 0; i < num_of_points; i++) {
        
            if (i == 0){
                aCalc[i]=0;
                bCalc[i]=0;
            }
            else{
                aCalc[i] = k_s[i-1]*(controlPointscopy[i].x  - controlPointscopy[i-1].x ) - (controlPointscopy[i].y - controlPointscopy[i-1].y);
                bCalc[i] = -k_s[i]*(controlPointscopy[i].x  - controlPointscopy[i-1].x ) + (controlPointscopy[i].y - controlPointscopy[i-1].y);
            }
       }
    }

    /// <summary>
    /// Returns the value of the spline at point X.
    /// </summary>
    public float Value(float X) {
        int i;
        if (X == controlPointscopy [num_of_points-1].x) {
            return controlPointscopy [num_of_points-1].y;
        }
        for (i= 1; i < num_of_points; i++ ) {
            if (controlPointscopy[i].x >= X) {
                break;
            } else if (i == num_of_points - 1) {
                i = -1;
                break;
            }
        }
        if (i == -1) {
            return 0;
        }
        float a, b,t, y;
        a = aCalc[i];
        b = bCalc[i];
       
        t = (X - controlPointscopy [i-1].x ) / (controlPointscopy [i].x  - controlPointscopy [i-1].x );
        y = (1 - t) * controlPointscopy [i-1].y + t * controlPointscopy [i].y + t * (1 - t) * (a * (1 - t) + b * t);
        return y;

    }
}
}
