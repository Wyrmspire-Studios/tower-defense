using Godot;
using System;
using WyrmspireStudios;
using WyrmspireStudios.Data;

public partial class BuyCardButton : Button
{
	[Export] private CardDeck _cardDeck;
	[Export] private Label _cardCost;
	
	private static readonly Color AffordableTint = Color.Color8(128, 255, 128);
	private static readonly Color NotAffordableTint = Color.Color8(255, 128, 128);

	private bool _canAfford;

	public override void _Ready()
	{
		LevelData.GoldChanged += _onGoldChanged;
		LevelData.CardCostChanged += _onCardCostChanged;
		ButtonDown += _onButtonDown;
		
		_onCardCostChanged(LevelData.GetCardCost(), LevelData.GetCardCost());
	}

	public override void _ExitTree()
	{
		LevelData.GoldChanged -= _onGoldChanged;
		LevelData.CardCostChanged -= _onCardCostChanged;
		ButtonDown -= _onButtonDown;
	}

	private void _onGoldChanged(int oldValue, int newValue)
	{
		_canAfford = newValue >= LevelData.GetCardCost();
		_refreshLabel();
	}

	private void _onCardCostChanged(int oldValue, int newValue)
	{
		_canAfford = newValue <= LevelData.GetGold();
		_refreshLabel();
	}

	private void _onButtonDown()
	{
		if (!_canAfford) return;
		
		_cardDeck.RollCard();
		LevelData.RemoveGold(LevelData.GetCardCost());
		LevelData.AddCardCost(10);
	}

	private void _refreshLabel()
	{
		_cardCost.Text = $"${LevelData.GetCardCost()}";
		_cardCost.Modulate = _canAfford ? AffordableTint : NotAffordableTint; 
	}
}
