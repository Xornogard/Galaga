using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformExtensionMethods
{
	#region MEMBERS

	#endregion

	#region PROPERTIES

	#endregion

	#region FUNCTIONS

	public static void MoveAlongPath(this Transform transform, List<Vector3> path, float speed, bool rotateDuringMovement = true)
	{
		Vector2 currentPosition = transform.position;
		Vector2 nextPosition = Vector2.zero;

		float distanceToMove = speed * Time.deltaTime;
		float currentDistance, previousDistanceToMove;

		do
		{
			previousDistanceToMove = distanceToMove;

			nextPosition = path[0];
			currentDistance = Vector2.Distance(currentPosition, nextPosition);

			if (distanceToMove > currentDistance)
			{
				distanceToMove -= currentDistance;
				currentPosition = path[0];
				path.RemoveAt(0);
			}

		} while (previousDistanceToMove > currentDistance && path.Count > 0);

		float t = currentDistance == 0 ? 1.0f : distanceToMove / currentDistance;

		transform.position = Vector3.Lerp(currentPosition, nextPosition, t);

		if(rotateDuringMovement == true)
		{
			transform.LookAt2D(nextPosition);
		}
	}

	public static void LookAt2D(this Transform transform, Vector2 targetPosition)
	{
		Vector2 currentPosition = transform.position;
		Vector2 lookDirection = (targetPosition - currentPosition).normalized;

		transform.LookAtDirection2D(lookDirection);
	}

	public static void LookAtDirection2D(this Transform transform, Vector2 direction)
	{
		float rotationZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ - 90f);
	}

	#endregion

	#region CLASS_ENUMS

	#endregion
}
