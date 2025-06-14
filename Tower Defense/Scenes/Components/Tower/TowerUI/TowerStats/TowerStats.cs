using Godot;
using System;

public partial class TowerStats : NinePatchRect
{
	private Tower _tower;
	[Export] private VBoxContainer _statContainer;
	[Export] private Label _nameLabel;

	[Export] private PackedScene _damageStat;
	public TowerStat DamageStatContainer;
	
	[Export] private PackedScene _projectileSpeedStat;
	public TowerStat ProjectileSpeedStatContainer;
	
	[Export] private PackedScene _fireDelayStat;
	public TowerStat FireDelayStatContainer;
	
	[Export] private PackedScene _fireDurationStat;
	public TowerStat FireDurationStatContainer;

	[Export] private PackedScene _rangeStat;
	public TowerStat RangeStatContainer;

	[Export] private PackedScene _healthStat;
	public TowerStat HealthStatContainer;
	
	[Export] private PackedScene _stunStat;
	public TowerStat StunStatContainer;

	public void Initialize(Tower tower)
	{
		_tower = tower;
		_nameLabel.Text = _tower.TowerInfo.Name;
	}

	private void _increaseSize()
	{
		Size += new Vector2(0, 22);
		Position -= new Vector2(0, 22);
	}

	public void ResetToDefaultSize()
	{
		var children = _statContainer.GetChildren();
		Position += new Vector2(0, 22) * children.Count;
		
		foreach (var child in children)
		{
			child.QueueFree();
		}
		
		Size = new Vector2(84, 32);
	}
	
	public void EnableDamageStat()
	{
		DamageStatContainer = _damageStat.Instantiate<TowerStat>();
		_statContainer.AddChild(DamageStatContainer);
		_increaseSize();
	}
	
	public void EnableProjectileSpeedStat()
	{
		ProjectileSpeedStatContainer = _projectileSpeedStat.Instantiate<TowerStat>();
		_statContainer.AddChild(ProjectileSpeedStatContainer);
		_increaseSize();
	}
	
	public void EnableFireDelayStat()
	{
		FireDelayStatContainer = _fireDelayStat.Instantiate<TowerStat>();
		_statContainer.AddChild(FireDelayStatContainer);
		_increaseSize();
	}
	
	public void EnableFireDurationStat()
	{
		FireDurationStatContainer = _fireDurationStat.Instantiate<TowerStat>();
		_statContainer.AddChild(FireDurationStatContainer);
		_increaseSize();
	}

	public void EnableRangeStat()
	{
		RangeStatContainer = _rangeStat.Instantiate<TowerStat>();
		_statContainer.AddChild(RangeStatContainer);
		_increaseSize();
	}

	public void EnableHealthStat()
	{
		HealthStatContainer = _healthStat.Instantiate<TowerStat>();
		_statContainer.AddChild(HealthStatContainer);
		_increaseSize();
	}
	
	public void EnableStunStat()
	{
		StunStatContainer = _stunStat.Instantiate<TowerStat>();
		_statContainer.AddChild(StunStatContainer);
		_increaseSize();
	}
}
