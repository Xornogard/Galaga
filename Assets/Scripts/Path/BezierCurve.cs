using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct BezierCurve
{
    #region PROPERTIES

    private Vector3 Point0 { get; set; }
    private Vector3 Point1 { get; set; }
    private Vector3 Point2 { get; set; }
    private Vector3 Point3 { get; set; }
    private int SamplesCount { get; set; }

    #endregion

    #region FUNCTIONS

    public BezierCurve(Vector3 point0, Vector3 point1, Vector3 point2, Vector3 point3, int defaultSamplesCount)
    {
        Point0 = point0;
        Point1 = point1;
        Point2 = point2;
        Point3 = point3;

        SamplesCount = defaultSamplesCount;
    }

    public IEnumerable<Vector3> GetCurve()
    {
        return GetCurve(SamplesCount);
    }

    public IEnumerable<Vector3> GetCurve(int samplesCount)
    {
        for (int i = 0; i < samplesCount && samplesCount > 0; i++)
        {
            float t = (float)i / samplesCount;
            yield return CalculateCubicBezierPoint(t);
        }
    }

    private Vector3 CalculateCubicBezierPoint(float t)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 p = uuu * Point0;
        p += 3 * uu * t * Point1;
        p += 3 * u * tt * Point2;
        p += ttt * Point3;

        return p;
    }

    #endregion
}
