using Godot;
using System;
using System.Collections.Generic;

public partial class MapLayer : VBoxContainer
{
	[Export] private PackedScene _mapButtonScene;
	public List<MapButton> MapButtons = [];
	public List<Vector2> MapButtonOffsets = [];

	public MapPicker MapPicker;

	public void GenerateMaps()
	{
		var mapCount = Random.Shared.Next(1, 4);
		
		for (var i = 0; i < mapCount; i++)
		{
			var mapType = _pickMapType();
			AddMapButton(mapType);
		}
	}

	public void GenerateTutorialMaps()
	{
		AddMapButton(MapType.Enemy);
	}

	public void GenerateBossMaps()
	{
		AddMapButton(MapType.Boss);
	}
	
	public void AddMapButton(MapType mapType, bool generateOffset = true)
	{
		var mapButton = _mapButtonScene.Instantiate<MapButton>();
		mapButton.MapType = mapType;
		mapButton.ButtonDown += () => OnMapButtonClicked(mapButton);
		if (generateOffset) MapButtonOffsets.Add(new Vector2(0, Random.Shared.Next(-20, 21)));
		MapButtons.Add(mapButton);
		AddChild(mapButton);
	}

	private MapType _pickMapType()
	{
		var number = Random.Shared.Next(0, 150);
		return number < 10 ? MapType.VeryHardEnemy : number < 25 ? MapType.HardEnemy : MapType.Enemy;
	}

	public void OnMapButtonClicked(MapButton mapButton)
	{
		var index = MapButtons.IndexOf(mapButton);
		MapPicker.OnLayerMapPicked(this, mapButton.MapType, index);
	}
}
