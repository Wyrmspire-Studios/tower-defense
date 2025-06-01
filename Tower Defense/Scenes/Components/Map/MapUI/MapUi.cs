using System;
using Godot;
using WyrmspireStudios;
using WyrmspireStudios.Data;

public partial class MapUi : Control
{
	[Export] private AnimationPlayer _animationPlayer;
	[Export] private AnimationPlayer _mapPickerAnimationPlayer;
	[Export] private AnimationPlayer _shopMenuAnimationPlayer;

	[Export] private MapPicker _mapPicker;
	[Export] private TextureButton _closeMapMenuButton;

	private bool _finishedAfterWave;
	private bool _died;

	public override void _Ready()
	{
		_animationPlayer.Play("ShowUI");

		LevelData.HealthChanged += _onHealthChange;
		EnemySpawner.EnemiesDead += _onEnemiesDead;
		MapPicker.MapPicked += _onMapPicked;
		_closeMapMenuButton.ButtonDown += _onCloseMapMenu;
	}

	public override void _ExitTree()
	{
		_animationPlayer.Play("HideUI");
		
		LevelData.HealthChanged -= _onHealthChange;
		EnemySpawner.EnemiesDead -= _onEnemiesDead;
		MapPicker.MapPicked -= _onMapPicked;
		_closeMapMenuButton.ButtonDown -= _onCloseMapMenu;
	}

	public void _onHealthChange(int oldValue, int newValue)
	{
		if (!_died && newValue <= 0)
		{
			_animationPlayer.Play("HideUI");
			_died = true;
			GD.Print("You Lose!");
		}
	}

	public void _onEnemiesDead()
	{
		_animationPlayer.Play("HideUI");
		
		if (_finishedAfterWave)
		{
			GD.Print("You Win!");
		}
		else
		{
			_mapPickerAnimationPlayer.Play("ShowMapPicker");
		}
	}

	public void _onCloseMapMenu()
	{
		_mapPicker.EnableButtons();
		_mapPickerAnimationPlayer.Play("ShowMapPicker");
		_shopMenuAnimationPlayer.Play("HideShopMenu");
	}

	public void _onMapPicked(MapType mapType)
	{
		if (mapType == MapType.Shop)
		{
			_mapPickerAnimationPlayer.Play("HideMapPicker");
			_shopMenuAnimationPlayer.Play("ShowShopMenu");
			return;
		}
		
		_mapPickerAnimationPlayer.Play("ShowBlack");
		var mapIndex = 8; // Random.Shared.Next(1, 7);
		var pickedMap = GD.Load<PackedScene>($"res://Scenes/Maps/Map{mapIndex}.tscn");

		var mapInstance = pickedMap.Instantiate();
		var root = GetTree().GetRoot();
		GetTree().CreateTimer(1).Timeout += () =>
		{
			LevelData.ResetLevelData();
			
			mapInstance.GetNode<AnimationPlayer>("MapUi/MapPickerAnimationPlayer").Play("HideBlack");
			if (mapType == MapType.Boss) mapInstance.GetNode<MapUi>("MapUi/MapUI")._finishedAfterWave = true;
			root.AddChild(mapInstance);
			GetTree().CurrentScene = mapInstance;
			root.RemoveChild(GetParent().GetParent());
		};
	}
}
