using Godot;
using System;

public partial class Paths : TileMapLayer
{
	public void AddTile(Vector2I tile)
	{
		SetCellsTerrainConnect([tile], 0, 0);
	}

	public void RemoveTile(Vector2I tile)
	{
		SetCellsTerrainConnect([tile], 0, -1);
	}
	
	public bool IsTileOccupied(Vector2I tile)
	{
		return GetCellSourceId(tile) != -1;
	}
}
