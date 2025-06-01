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

	[Export] private PackedScene _mainMenuScene;
	[Export] private Button _exitButton;

	public static bool Transitioning;

	private MapType _lastMapType = MapType.Enemy;
	private bool _finishedAfterWave;
	private bool _died;

	public override void _Ready()
	{
		_animationPlayer.Play("ShowUI");

		LevelData.HealthChanged += _onHealthChange;
		EnemySpawner.EnemiesDead += _onEnemiesDead;
		MapPicker.MapPicked += _onMapPicked;
		_closeMapMenuButton.ButtonDown += _onCloseMapMenu;
		_exitButton.ButtonDown += _onExitButton;
	}

	public override void _ExitTree()
	{
		_animationPlayer.Play("HideUI");
		
		LevelData.HealthChanged -= _onHealthChange;
		EnemySpawner.EnemiesDead -= _onEnemiesDead;
		MapPicker.MapPicked -= _onMapPicked;
		_closeMapMenuButton.ButtonDown -= _onCloseMapMenu;
		_exitButton.ButtonDown -= _onExitButton;
	}

	private void _onExitButton()
	{
		GetTree().ChangeSceneToPacked(_mainMenuScene);
		MapPicker.Reset();
		LevelData.ResetLevelData();
		GameData.ResetGameData();
	}

	public void _onHealthChange(int oldValue, int newValue)
	{
		if (!_died && newValue <= 0)
		{
			_animationPlayer.Play("HideUI");
			_died = true;
			_mapPickerAnimationPlayer.Play("ShowYouLose");
		}
	}

	public void _onEnemiesDead()
	{
		_animationPlayer.Play("HideUI");
		
		if (_finishedAfterWave)
		{
			_mapPickerAnimationPlayer.Play("ShowYouWin");
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
		var givenShards = _lastMapType switch
		{
			MapType.Enemy => 10,
			MapType.HardEnemy => 20,
			MapType.VeryHardEnemy => 30,
			MapType.Boss => 40,
			_ => 0
		};
		
		GameData.AddShards(givenShards);
		
		if (mapType == MapType.Shop)
		{
			_mapPickerAnimationPlayer.Play("HideMapPicker");
			_shopMenuAnimationPlayer.Play("ShowShopMenu");
			return;
		}
		
		_mapPickerAnimationPlayer.Play("ShowBlack");
		
		var mapIndex = Random.Shared.Next(1, 4);
		var pickedMapPath = mapType switch
		{
			MapType.Enemy => $"res://Scenes/Maps/EasyMaps/EasyMap{mapIndex}.tscn",
			MapType.HardEnemy => $"res://Scenes/Maps/MediumMaps/MediumMap{mapIndex}.tscn",
			MapType.VeryHardEnemy => $"res://Scenes/Maps/HardMaps/HardMap{mapIndex}.tscn",
			MapType.Boss => "res://Scenes/Maps/BossMaps/BossMap1.tscn",
			_ => throw new ArgumentOutOfRangeException(nameof(mapType), mapType, null)
		};
		
		var pickedMap = GD.Load<PackedScene>(pickedMapPath);

		Transitioning = true;

		var mapInstance = pickedMap.Instantiate();
		var root = GetTree().GetRoot();
		GetTree().CreateTimer(1).Timeout += () =>
		{
			Transitioning = false;
			
			LevelData.ResetLevelData();
			TowerSpawning.SpawnedUnits.Clear();
			
			_lastMapType = mapType;
			
			mapInstance.GetNode<AnimationPlayer>("MapUi/MapPickerAnimationPlayer").Play("HideBlack");
			if (mapType == MapType.Boss) mapInstance.GetNode<MapUi>("MapUi/MapUI")._finishedAfterWave = true;
			root.AddChild(mapInstance);
			var current = GetTree().CurrentScene;
			GetTree().CurrentScene = mapInstance;
			current.QueueFree();
		};
	}
}
