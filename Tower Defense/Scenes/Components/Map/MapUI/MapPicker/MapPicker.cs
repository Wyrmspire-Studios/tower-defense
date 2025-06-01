using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class MapPicker : Control
{
	public delegate void MapPickedHandler(MapType mapType);

	public static event MapPickedHandler MapPicked;

	[Export] private PackedScene _mapLayerScene;
	[Export] private HBoxContainer _mapLayerContainer;
	[Export] private MapPaths _mapPaths;
	
	public static int CurrentLayer = 1;
	public static List<int> PickedMaps = [];
	public static List<MapLayerInfo> GeneratedMapLayers = [];

	public List<MapLayer> MapLayers = [];

	public override void _Ready()
	{
		if (GeneratedMapLayers.Count != 0) _applyGeneratedMapLayers();
		else
		{
			_generateMapLayers();
			_connectButtons();
		}
	}

	public static void Reset()
	{
		PickedMaps.Clear();
		CurrentLayer = 1;
		GeneratedMapLayers.Clear();
	}

	public MapLayer CreateLayer()
	{
		var mapLayer = _mapLayerScene.Instantiate<MapLayer>();
		mapLayer.MapPicker = this;
		MapLayers.Add(mapLayer);
		_mapLayerContainer.AddChild(mapLayer);
		return mapLayer;
	}

	private void _generateMapLayers()
	{
		var tutorialLayer = CreateLayer();
		tutorialLayer.GenerateTutorialMaps();

		for (var i = 0; i < 3; i++)
		{
			var mapLayer = CreateLayer();
			mapLayer.GenerateMaps();
		}

		var bossLayer = CreateLayer();
		bossLayer.GenerateBossMaps();

		var withShop = MapLayers[Random.Shared.Next(1, MapLayers.Count - 1)];
		var shopButton = withShop.MapButtons[Random.Shared.Next(0, withShop.MapButtons.Count)];
		shopButton.MapType = MapType.Shop;
		shopButton.UpdateIcon();

		if (PickedMaps.Count == 0)
		{
			PickedMaps = new List<int>(MapLayers.Count);
			for (var i = 0; i < MapLayers.Count; i++)
			{
				PickedMaps.Add(-1);
			}
		}
	}

	private async void _connectButtons()
	{
		await ToSignal(_mapLayerContainer, Container.SignalName.SortChildren);

		foreach (var mapLayer in MapLayers)
		{
			await ToSignal(mapLayer, Container.SignalName.SortChildren);
		}

		for (var i = 0; i < MapLayers.Count - 2; i++)
		{
			var mapLayer = MapLayers[i];
			var nextLayer = MapLayers[i + 1];

			if (mapLayer.MapButtons.Count == 1)
			{
				foreach (var nextLayerMapButton in nextLayer.MapButtons)
				{
					mapLayer.MapButtons[0].ConnectedTo.Add(nextLayerMapButton);
				}
			}
			else
			{
				foreach (var mapLayerMapButton in mapLayer.MapButtons)
				{
					for (var j = 0; j < 2; j++)
					{
						var pickedButton = nextLayer.MapButtons[Random.Shared.Next(0, nextLayer.MapButtons.Count)];
						if (!mapLayerMapButton.ConnectedTo.Contains(pickedButton))
							mapLayerMapButton.ConnectedTo.Add(pickedButton);
					}
				}

				foreach (var nextLayerMapButton in nextLayer.MapButtons)
				{
					bool hasConnection = false;

					foreach (var mapLayerMapButton in mapLayer.MapButtons)
					{
						hasConnection = mapLayerMapButton.ConnectedTo.Contains(nextLayerMapButton);
						if (hasConnection) break;
					}

					if (!hasConnection)
						nextLayerMapButton.ConnectedTo.Add(
							mapLayer.MapButtons[Random.Shared.Next(0, mapLayer.MapButtons.Count)]);
				}
			}
		}

		var preBossLayer = MapLayers[^2];
		var bossLayer = MapLayers[^1];

		var bossButton = bossLayer.MapButtons[0];
		foreach (var mapButton in preBossLayer.MapButtons)
		{
			mapButton.ConnectedTo.Add(bossButton);
		}

		_mapPaths.MapPicker = this;
		_mapPaths.QueueRedraw();

		EnableButtons();
		_saveGeneratedMapLayers();
	}

	public void EnableButtons(bool onlyMark = false)
	{
		foreach (var mapLayer in MapLayers)
		{
			foreach (var mapLayerMapButton in mapLayer.MapButtons)
			{
				mapLayerMapButton.Disable();
			}
		}

		MapLayers[0].MapButtons[0].MarkKilled();

		for (var i = 1; i < MapLayers.Count; i++)
		{
			if (PickedMaps[i] == -1) continue;
			MapLayers[i].MapButtons[PickedMaps[i]].MarkKilled();
		}

		if (onlyMark || CurrentLayer > MapLayers.Count - 1) return;

		foreach (var mapButton in MapLayers[CurrentLayer].MapButtons)
		{
			if (CurrentLayer == 1) mapButton.Enable();
			else if (MapLayers[CurrentLayer - 1].MapButtons[PickedMaps[CurrentLayer - 1]].ConnectedTo
			         .Contains(mapButton)) mapButton.Enable();
		}
	}

	public void OnLayerMapPicked(MapLayer mapLayer, MapType mapType, int buttonIndex)
	{
		var layerIndex = MapLayers.IndexOf(mapLayer);
		CurrentLayer = layerIndex + 1;
		PickedMaps[layerIndex] = buttonIndex;

		EnableButtons(true);

		MapPicked?.Invoke(mapType);
	}

	private void _saveGeneratedMapLayers()
	{
		for (var i = 0; i < MapLayers.Count; i++)
		{
			var mapLayer = MapLayers[i];

			if (i + 1 == MapLayers.Count)
			{
				var bossLayerInfo =
					new MapLayerInfo(
						mapLayer.MapButtons.Select(
							button => new MapButtonInfo(button.MapType,
									[]
							)).ToList(), mapLayer.MapButtonOffsets);
				
				GeneratedMapLayers.Add(bossLayerInfo);
				continue;
			}
			
			var nextLayer = MapLayers[i + 1];

			var mapLayerInfo =
				new MapLayerInfo(mapLayer.MapButtons.Select(button => new MapButtonInfo(button.MapType,
					button.ConnectedTo.Select(connectedButton => nextLayer.MapButtons.IndexOf(connectedButton))
						.ToList())).ToList(), mapLayer.MapButtonOffsets);
			
			GeneratedMapLayers.Add(mapLayerInfo);
		}
	}

	private void _applyGeneratedMapLayers()
	{
		foreach (var generatedMapLayer in GeneratedMapLayers)
		{
			var layerInstance = _mapLayerScene.Instantiate<MapLayer>();
			layerInstance.MapButtonOffsets = generatedMapLayer.MapButtonOffsets;
			layerInstance.MapPicker = this;
			foreach (var mapButtonInfo in generatedMapLayer.MapButtons)
			{
				layerInstance.AddMapButton(mapButtonInfo.MapType, false);
			}
			MapLayers.Add(layerInstance);
			_mapLayerContainer.AddChild(layerInstance);
		}

		for (var i = 0; i < MapLayers.Count - 1; i++)
		{
			var nextLayer = MapLayers[i + 1];
			for (var j = 0; j < MapLayers[i].MapButtons.Count; j++)
			{
				MapLayers[i].MapButtons[j].ConnectedTo =
					GeneratedMapLayers[i].MapButtons[j].ConnectedTo.Select(buttonIndex => nextLayer.MapButtons[buttonIndex]).ToList();
			}
		}
		
		_mapPaths.MapPicker = this;
		_mapPaths.QueueRedraw();
		
		EnableButtons();
	}
}