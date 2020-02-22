using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path
{
    #region MEMBERS

    #endregion

    #region PROPERTIES

    private List<BezierCurve> Curves { get; set; } = new List<BezierCurve>();

    #endregion

    #region MONOBEHAVIOUR_CALLBACKS

    #endregion

    #region FUNCTIONS

    public void AddCurve(BezierCurve curve)
    {
        Curves.Add(curve);
    }

    public List<Vector3> SamplePath()
    {
        List<Vector3> path = new List<Vector3>();

        for (int i = 0; i < Curves.Count; i++)
        {
            path.AddRange(Curves[i].GetCurve());
        }

        return path;
    }

    #endregion

    #region CLASS_ENUMS

    #endregion
}
