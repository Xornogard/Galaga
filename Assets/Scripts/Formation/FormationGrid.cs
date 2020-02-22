using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormationGrid : MonoBehaviour
{
	#region MEMBERS
	#pragma warning disable 0649

	[SerializeField]
	private Vector2 cellSize;

	[SerializeField]
	private Vector2 spacing;

	#pragma warning restore 0649
	#endregion

	#region PROPERTIES

	private Vector2 CellSize => cellSize;
	private Vector2 Spacing => spacing;

	public int CurrentRowIndex {
		get;
		private set;
	}

	#endregion

	#region MONOBEHAVIOUR_CALLBACKS

	private void OnDrawGizmosSelected()
	{
		System.Action<Color, int, int, int> drawEnemyInFormation = (Color gizmosColor, int alienCount, int startRowIndex, int rowsCount) =>
		{
			Gizmos.color = gizmosColor;
			for (int i = 0; i < alienCount; i++)
			{
				Gizmos.DrawCube(EstimatePosition(i, startRowIndex, rowsCount), CellSize);
			}
		};

		//blueWings
		drawEnemyInFormation(Color.blue, 4, 0, 1);

		//redWings
		drawEnemyInFormation(Color.red, 16, 1, 2);

		//yellowWing
		drawEnemyInFormation(Color.yellow, 24, 3, 2);
	}

	#endregion

	#region FUNCTIONS

	public void ResetFormationGrid()
	{
		CurrentRowIndex = 0;
	}

	public void ReserveRows(int rowsCount)
	{
		CurrentRowIndex += rowsCount;
	}

	public Vector2 EstimatePosition(int indexPosition, int firstRowIndex, int rowsCount)
	{
		Vector2 gridPosition = Vector2.zero;

		gridPosition.x = EstimateColumnPosition(indexPosition, rowsCount);
		gridPosition.y = EstimateRowPosition(indexPosition, firstRowIndex, rowsCount);

		return gridPosition;
	}

	private float EstimateColumnPosition(int indexPosition, int rowsCount)
	{
		float columnPosition = 0.0f;

		int side = Mathf.CeilToInt(Mathf.Pow(-1, indexPosition % 2 + 1));
		int columnIndex = indexPosition / (2 * rowsCount);

		columnPosition = transform.position.x + (CellSize.x + 2 * Spacing.x * columnIndex) * side;

		return columnPosition;
	}

	private float EstimateRowPosition(int indexPosition, int startingRowIndex, int rowsCount)
	{
		float rowPosition = 0.0f;

		int rowIndex = startingRowIndex + (indexPosition / 2) % rowsCount;

		rowPosition = transform.position.y + CellSize.y - 2 * Spacing.y * rowIndex;

		return rowPosition;
	}

	#endregion

	#region CLASS_ENUMS

	#endregion
}
