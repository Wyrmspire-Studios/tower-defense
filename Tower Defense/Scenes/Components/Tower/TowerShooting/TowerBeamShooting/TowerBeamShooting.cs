using Godot;
using System;

public partial class TowerBeamShooting : Node2D
{
	private Timer _shootDelayTimer;
	private Timer _shootDurationTimer;
	private Timer _damageTickTimer;

	public RangedBeamTower Tower;

	private bool _canShoot;
	private Enemy _shootingAt;

	public void Initialize(RangedBeamTower tower)
	{
		Tower = tower;

		Tower.TowerTargeting.TargetChange += _onTargetChange;

		_shootDelayTimer = new Timer
		{
			WaitTime = Tower.RangedBeamTowerInfo.FireDelay,
			Autostart = false,
			OneShot = true
		};

		_shootDurationTimer = new Timer
		{
			WaitTime = Tower.RangedBeamTowerInfo.FireDuration,
			Autostart = false,
			OneShot = true
		};

		_damageTickTimer = new Timer
		{
			WaitTime = 0.1f,
			Autostart = true
		};
		
		AddChild(_shootDelayTimer);
		AddChild(_shootDurationTimer);
		AddChild(_damageTickTimer);
		
		_shootDelayTimer.Timeout += _onShootDelay;
		_shootDurationTimer.Timeout += _onShootDuration;
		_damageTickTimer.Timeout += _onDamageTick;
		
		_onShootDelay();
	}

	public override void _Draw()
	{
		if (!Tower.RangedBeamTowerInfo.Active) return;
		if (!_canShoot || _shootingAt == null) return;

		var shootFrom = Tower.RangedBeamTowerInfo.BeamSpawnOffset;
		var shootAt = _shootingAt.GlobalPosition - GlobalPosition;
		
		DrawLine(shootFrom, shootAt, Tower.RangedBeamTowerInfo.BackgroundBeamColor, 4);
		DrawLine(shootFrom, shootAt, Tower.RangedBeamTowerInfo.ForegroundBeamColor, 1);

		DrawCircle(shootFrom, 4, Tower.RangedBeamTowerInfo.BackgroundBeamColor);
		DrawCircle(shootFrom, 2.5f, Tower.RangedBeamTowerInfo.ForegroundBeamColor);
		
		DrawCircle(shootAt, 4, Tower.RangedBeamTowerInfo.BackgroundBeamColor);
		DrawCircle(shootAt, 2.5f, Tower.RangedBeamTowerInfo.ForegroundBeamColor);
	}

	public override void _Process(double delta)
	{
		QueueRedraw();
	}

	public void UpdateTimerDurations()
	{
		_shootDelayTimer.WaitTime = Tower.RangedBeamTowerInfo.FireDelay;
		_shootDelayTimer.Start();
		
		_shootDurationTimer.WaitTime = Tower.RangedBeamTowerInfo.FireDuration;
		_shootDurationTimer.Stop();
	}

	private void _onShootDelay()
	{
		if (!Tower.RangedBeamTowerInfo.Active) return;
		_canShoot = true;
		_shootDurationTimer.Start(Tower.RangedBeamTowerInfo.FireDuration);
	}

	private void _onShootDuration()
	{
		if (!Tower.RangedBeamTowerInfo.Active) return;
		_canShoot = false;
		_shootDelayTimer.Start(Tower.RangedBeamTowerInfo.FireDelay);
	}

	private void _onDamageTick()
	{
		if (!Tower.RangedBeamTowerInfo.Active) return;
		if (!_canShoot || _shootingAt == null) return;
		Tower.OnEnemyShot(_shootingAt);
	}

	private void _onTargetChange(Enemy newTarget)
	{
		_shootingAt = newTarget;
	}
}
