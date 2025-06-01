using Godot;
using WyrmspireStudios.Data;

public partial class ShardsLabel : Label
{
	public override void _Ready()
	{
		Text = $"Shards: {GameData.GetShards()}";
		GameData.ShardsChanged += OnShardsChanged;
	}

	public override void _ExitTree()
	{
		GameData.ShardsChanged -= OnShardsChanged;
	}

	private void OnShardsChanged(int from, int to)
	{
		Text = $"Shards: {to}";
	}
}
