using Godot;
using System;
using System.Collections.Generic;
using TowerDefense.Scenes.Components.Map.TowerPlacement;

public partial class TowerPlacement : Node2D
{
	[Export] private PackedScene _placementSmokeScene;
	
	[ExportGroup("Internal")]
	[Export] private Node2D _placedTowers;
	[Export] public Occupied Occupied;
	[Export] public Paths Paths;

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

	public override void _ExitTree()
	{
		Tower.TowerSold -= _onTowerSold;
	}

	public override void _Process(double delta)
	{
		if (_currentlyPlacing != null) _handleMouseMovement();
	}
	
	public void StartPlacingTower(PackedScene tower)
	{
		Occupied.ShowTileMap();
		
		_currentlyPlacing?.QueueFree();
		_currentlyPlacing = tower.Instantiate<Tower>();
		_currentlyPlacing.Modulate = HiddenTint;
		_currentlyPlacing.OnStartPlacing(this);
		_placedTowers.AddChild(_currentlyPlacing);
	}

	public void CancelPlacingTower()
	{
		Occupied.HideTileMap();
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
	
	public bool PlaceTower()
	{
		Occupied.HideTileMap();
		if (_canPlace == CanPlace.No || _currentlyPlacing == null)
		{
			CancelPlacingTower();
			return false;
		}
		
		if (_canPlace == CanPlace.Enhancement) _enhanceTower();
		else if (_canPlace == CanPlace.Yes) _placeNewTower();

		return true;
	}

	private void _placeNewTower()
	{
		_currentlyPlacing.OnPlaceTower();
		
		_showSmoke();
		
		var mouseTile = GetMouseTile();
		Occupied.AddTile(mouseTile);
		_currentlyPlacing.PlacedAt = mouseTile;
		_currentlyPlacing.Modulate = NoTint;
		
		_towers.Add(mouseTile, _currentlyPlacing);
		_currentlyPlacing = null;
	}

	private void _enhanceTower()
	{
		var mouseTile = GetMouseTile();
		var givenEnhancement = _currentlyPlacing.TowerInfo.GivenEnhancement;
		var tower = _towers[mouseTile];
		tower.TowerUi.TowerActions.IncreaseCurrentCost(Mathf.FloorToInt(50 * 0.5));
		tower.TowerEnhancements.Add(givenEnhancement);
		tower.ApplyEnhancements();

		_showSmoke();

		_currentlyPlacing.QueueFree();
		_currentlyPlacing = null;
	}

	public Vector2I GetMouseTile()
	{
		return (Vector2I)(GetGlobalMousePosition() / 32).Floor();
	}

	private void _handleMouseMovement()
	{
		var mouseTile = GetMouseTile();
		_currentlyPlacing.Position = mouseTile * 32;
		
		_canPlace = _towers.ContainsKey(mouseTile) ? CanPlace.Enhancement 
			: Occupied.IsTileOccupied(mouseTile) ? CanPlace.No 
			: CanPlace.Yes;
		if (_canPlace == CanPlace.Yes)
			_canPlace = _currentlyPlacing.CheckPlacementRequirements(this, mouseTile);
		
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
		Occupied.RemoveTile(tile);
		_towers.Remove(tile);
	}

	public bool IsPlacing()
	{
		return _currentlyPlacing != null;
	}
}
