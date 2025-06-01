using Godot;
using System;

public partial class MapPaths : Control
{
	public MapPicker MapPicker;
	
	private static readonly Color ShadowColor = Color.Color8(0, 0, 0, 128);
	private static readonly Color GrassColor = Color.Color8(101, 108, 23);
	private static readonly Color PathColor = Color.Color8(215, 192, 115);

	public override void _Process(double delta)
	{
		QueueRedraw();
	}

	public override void _Draw()
	{
		if (MapPicker == null) return;
		
		var mapLayers = MapPicker.MapLayers;

		for (var i = 0; i < mapLayers.Count - 1; i++)
		{
			var mapLayer = mapLayers[i];
			for (var j = 0; j < mapLayer.MapButtons.Count; j++)
			{
				var mapLayerMapButton = mapLayer.MapButtons[j];
				foreach (var connectedTo in mapLayerMapButton.ConnectedTo)
				{
					var from = mapLayerMapButton.GlobalPosition - GlobalPosition + new Vector2(20, 20);
					var to = connectedTo.GlobalPosition - GlobalPosition + new Vector2(20, 20);

					var middle = (from + to) / 2 + mapLayer.MapButtonOffsets[j];
					var shadowOffset = new Vector2(1f, 1f);

					DrawPolyline([from + shadowOffset, middle + shadowOffset, middle + shadowOffset, to + shadowOffset],
						ShadowColor, 16);
					DrawPolyline([from, middle, middle, to], GrassColor, 16);
					DrawPolyline([from, middle, middle, to], PathColor, 12);
				}
			}
		}
	}
}
