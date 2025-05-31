using Godot;
using System;

public partial class TowerStat : MarginContainer
{
	[Export] private Label _statText;

	public void SetStatText(string statText)
	{
		_statText.Text = statText;
	}
}
