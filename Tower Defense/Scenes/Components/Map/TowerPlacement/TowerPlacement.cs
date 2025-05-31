using Godot;
using System;
using System.Collections.Generic;
using TowerDefense.Scenes.Components.Map.TowerPlacement;

public partial class TowerPlacement : Node2D
{
	[Export] private PackedScene _placementSmokeScene;
	[Export] private PackedScene _placedScene;
	
	[ExportGroup("Internal")]
	[Export] private Node2D _placedTowers;
	[Export] private Occupied _occupied;

	private CanPlace _canPlace;
	private Tower _currentlyPlacing;
	private Dictionary<Vector2I, Tower> _towers = new();
	
	private static readonly Color NoTint = Color.Color8(255, 255, 255);
	private static readonly Color HiddenTint = Color.Color8(255, 255, 255, 0);
	private static readonly Color PlaceableTint = Color.Color8(128, 255, 128, 192);
	private static readonly Color EnhancementTint = Color.Color8(128, 128, 255, 192);
	private static readonly Color UnplaceableTint = Color.Color8(255, 128, 128, 192);

	public override void _Ready()
	{
		Tower.TowerSold += _onTowerSold;
	}

	public override void _Process(double delta)
	{
		if (_currentlyPlacing != null) _handleMouseMovement();
		
		if (Input.IsActionJustPressed("Start Placing Tower")) _startPlacingTower();
		if (_currentlyPlacing != null && Input.IsActionJustPressed("Cancel Placing Tower")) _cancelPlacingTower();
		
		if (_canPlace == CanPlace.Enhancement && _currentlyPlacing != null && Input.IsActionJustPressed("Place Tower")) _enhanceTower();
		else if (_canPlace == CanPlace.Yes && _currentlyPlacing != null && Input.IsActionJustPressed("Place Tower")) _placeTower();
	}
	
	private void _startPlacingTower()
	{
		_occupied.ShowTileMap();
		
		_currentlyPlacing?.QueueFree();
		_currentlyPlacing = _placedScene.Instantiate<Tower>();
		_currentlyPlacing.Modulate = HiddenTint;
		_currentlyPlacing.OnStartPlacing();
		_placedTowers.AddChild(_currentlyPlacing);
	}

	private void _cancelPlacingTower()
	{
		_occupied.HideTileMap();
		_currentlyPlacing?.QueueFree();
		_currentlyPlacing = null;
	}

	private void _showSmoke()
	{
		var placementSmoke = _placementSmokeScene.Instantiate<AnimatedSprite2D>();
		placementSmoke.Position = _currentlyPlacing.Position;
		placementSmoke.AnimationFinished += placementSmoke.QueueFree;
		_placedTowers.AddChild(placementSmoke);
	}
	
	private void _placeTower()
	{
		_occupied.HideTileMap();
		
		_currentlyPlacing.OnPlaceTower();

		_showSmoke();
		
		var mouseTile = _getMouseTile();
		_occupied.AddTile(mouseTile);
		_currentlyPlacing.PlacedAt = mouseTile;
		_currentlyPlacing.Modulate = NoTint;
		
		_towers.Add(mouseTile, _currentlyPlacing);
		_currentlyPlacing = null;
	}

	private void _enhanceTower()
	{
		_occupied.HideTileMap();

		var mouseTile = _getMouseTile();
		var givenEnhancement = _currentlyPlacing.TowerInfo.GivenEnhancement;
		var tower = _towers[mouseTile];
		tower.TowerActions.IncreaseCurrentCost(Mathf.FloorToInt(50 * 0.5));
		tower.TowerEnhancements.Add(givenEnhancement);
		tower.ApplyEnhancements();

		_showSmoke();

		_currentlyPlacing.QueueFree();
		_currentlyPlacing = null;
	}

	private Vector2I _getMouseTile()
	{
		return (Vector2I)(GetGlobalMousePosition() / 32).Floor();
	}

	private void _handleMouseMovement()
	{
		var mouseTile = _getMouseTile();
		_currentlyPlacing.Position = mouseTile * 32;
		_canPlace = _towers.ContainsKey(mouseTile) ? CanPlace.Enhancement 
			: _occupied.IsTileOccupied(mouseTile) ? CanPlace.No 
			: CanPlace.Yes;
		_currentlyPlacing.Modulate = _canPlace switch
		{
			CanPlace.No => UnplaceableTint,
			CanPlace.Enhancement => EnhancementTint,
			CanPlace.Yes => PlaceableTint,
			_ => throw new ArgumentOutOfRangeException()
		};
		_currentlyPlacing.QueueRedraw();
	}

	private void _onTowerSold(Vector2I tile)
	{
		_occupied.RemoveTile(tile);
		_towers.Remove(tile);
	}
}
