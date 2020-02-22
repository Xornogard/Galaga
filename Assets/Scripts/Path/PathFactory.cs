using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PathFactory
{
	#region MEMBERS

	#endregion

	#region PROPERTIES

	#endregion

	#region FUNCTIONS

	public static Path CreateAppearPath(Vector3 startingPosition, Vector3 formationPosition, int loopsCount = 0)
	{
		Path appearPath = new Path();

		float screenSideSign = Mathf.Sign(startingPosition.x);
		appearPath.AddCurve(new BezierCurve(startingPosition, startingPosition, new Vector2(screenSideSign*3, 2), new Vector2(screenSideSign*2, 0), 30));
		appearPath.AddCurve(new BezierCurve(new Vector2(screenSideSign*2, 0), new Vector2(screenSideSign*1, -3), new Vector2(screenSideSign*5, -1f), new Vector2(screenSideSign*3, 0), 30));

		for (int i = 0; i < loopsCount; i++)
		{
			appearPath.AddCurve(new BezierCurve(new Vector2(screenSideSign * 3, 0), new Vector2(screenSideSign * 2, .5f), new Vector2(screenSideSign * 1, -1.5f), new Vector2(screenSideSign * 3, -1.5f), 30));
			appearPath.AddCurve(new BezierCurve(new Vector2(screenSideSign * 3, -1.5f), new Vector2(screenSideSign * 3.5f, -1.5f), new Vector2(screenSideSign * 4f, -0.3f), new Vector2(screenSideSign * 3, 0), 30));
		}

		appearPath.AddCurve(new BezierCurve(new Vector2(screenSideSign * 3, 0), new Vector2(screenSideSign * 2, .5f), formationPosition + Vector3.up*2f, formationPosition, 30));

		return appearPath;
	}

	public static Path CreateDiveInPath(Vector3 startingPosition, float amplitude = 0.5f)
	{
		amplitude *= Mathf.Sign(startingPosition.x);

		Path diveInPath = new Path();
		float screenSideSign = Mathf.Sign(startingPosition.x);

		(Vector2 startPoint, Vector2 controlPoint) curveStartPoints = (startingPosition, startingPosition );
		(Vector2 endPoint, Vector2 controlPoint) curveEndPoints = (new Vector2(startingPosition.x + amplitude, startingPosition.y - 1), new Vector2(startingPosition.x, startingPosition.y - 1));
		diveInPath.AddCurve(curveStartPoints, curveEndPoints, 30);

		for (int i = 0; i < 10; i++)
		{
			float sign = Mathf.Pow(-1, i);
			curveStartPoints = (new Vector2(startingPosition.x + sign* amplitude, startingPosition.y - (i+1)), new Vector2(startingPosition.x + sign* amplitude*3, startingPosition.y - (i+1.2f)));
			curveEndPoints = (new Vector2(startingPosition.x - sign* amplitude, startingPosition.y - (i+2)), new Vector2(startingPosition.x, startingPosition.y - (i+2)));
			diveInPath.AddCurve(curveStartPoints, curveEndPoints, 30);
		}


		return diveInPath;
	}

	private static void AddCurve(this Path path, (Vector2 startPoint, Vector2 controlPoint) curveStartPoints, (Vector2 endPoint, Vector2 controlPoint) curveEndPoints, int samplesCount)
	{
		BezierCurve newCurve = new BezierCurve(curveStartPoints.startPoint, curveStartPoints.controlPoint, curveEndPoints.controlPoint, curveEndPoints.endPoint, samplesCount);
		path.AddCurve(newCurve);
	}

	#endregion

	#region CLASS_ENUMS

	#endregion
}
