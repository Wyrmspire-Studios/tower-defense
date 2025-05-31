using Godot;
using System;

public partial class TowerBeamShooting : Node2D
{
	[Export] private Timer _shootDelayTimer;
	[Export] private Timer _shootDurationTimer;
	[Export] private Timer _damageTickTimer;

	public RangedBeamTower Tower;

	private bool _canShoot;
	private Enemy _shootingAt;

	public void Initialize(RangedBeamTower tower)
	{
		Tower = tower;

		_shootDelayTimer.Timeout += _onShootDelay;
		_shootDurationTimer.Timeout += _onShootDuration;
		_damageTickTimer.Timeout += _onDamageTick;

		Tower.TowerTargeting.TargetChange += _onTargetChange;

		_shootDelayTimer.WaitTime = Tower.RangedBeamTowerInfo.FireDelay;
		_shootDelayTimer.Autostart = true;
	}

	public override void _Draw()
	{
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
		if (_shootingAt == null) return;
		QueueRedraw();
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
		if (_shootingAt != null) _shootDelayTimer.Start(Tower.RangedBeamTowerInfo.FireDelay);
	}

	private void _onDamageTick()
	{
		if (!Tower.RangedBeamTowerInfo.Active) return;
		if (!_canShoot || _shootingAt == null) return;
		Tower.OnEnemyShot(_shootingAt);
	}

	private void _onTargetChange(Enemy newTarget)
	{
		var reset = _shootingAt == null;
		_shootingAt = newTarget;
		
		if (reset && _shootingAt != null)
		{
			_onShootDelay();
		}
	}
}
