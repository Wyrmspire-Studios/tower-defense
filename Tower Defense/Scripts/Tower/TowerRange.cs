using Godot;

namespace WyrmspireStudios;
public partial class TowerRange : Node2D
{
	[Export] public Vector2 TowerRangeOffset;
	[Export] public float TowerRangeRadius;
	
	[ExportGroup("Internal")]
	[Export] private Area2D _towerRangeArea;
	[Export] private CollisionShape2D _towerRangeShape;

	[Signal]
	public delegate void EnemyEnteredRangeEventHandler(TowerPlaceholderEnemy enemy);
	
	[Signal]
	public delegate void EnemyExitedRangeEventHandler(TowerPlaceholderEnemy enemy);

	private Tower _tower;
	private CircleShape2D _towerRangeCircle;
	private bool _towerRangeVisible;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_tower = GetParent<Tower>();
		_tower.TowerRange = this;
		
		_towerRangeCircle = new CircleShape2D();
		_towerRangeCircle.Radius = TowerRangeRadius;
		_towerRangeShape.Shape = _towerRangeCircle;

		_towerRangeArea.BodyEntered += OnBodyEntered;
		_towerRangeArea.BodyExited += OnBodyExited;
		
		ToggleRangeVisibility();
	}

	public override void _Draw()
	{
		if (!_towerRangeVisible) return;
		
		DrawCircle(TowerRangeOffset, TowerRangeRadius, Color.Color8(100, 149, 237, 64));
		DrawCircle(TowerRangeOffset, TowerRangeRadius, Color.Color8(100, 149, 237, 64), false, 0.5f, true);
	}

	private void OnBodyEntered(Node2D body)
	{
		if (body is TowerPlaceholderEnemy enemy) EmitSignal(SignalName.EnemyEnteredRange, enemy);
	}

	private void OnBodyExited(Node2D body)
	{
		if (body is TowerPlaceholderEnemy enemy) EmitSignal(SignalName.EnemyExitedRange, enemy);
	}

	public void ToggleRangeVisibility()
	{
		_towerRangeVisible = !_towerRangeVisible;
		QueueRedraw();
	}
}
