using System;
using System.Runtime.InteropServices.JavaScript;
using System.Threading.Tasks;
using Godot;
using WyrmspireStudios.Data;
using WyrmspireStudios.Events;

namespace WyrmspireStudios;

public partial class CardDeck : Control
{
	public static event EventHandlers.BaseHandler EnteredDeck;
	public static event EventHandlers.BaseHandler ExitedDeck;

	[Export] public TowerPlacement TowerPlacement;
	[Export] public PackedScene CardScene;
	[Export] public PackedScene[] PossibleTowers;
	[Export] public HBoxContainer CardsContainer;
	[Export] public AnimationPlayer AnimationPlayer;
	[Export] public ReferenceRect CollisionRect;

	private bool _placing;
	private NinePatchRect _background;
	private bool _hoveringBackground;
	private bool _animating;
	private bool _hidden;

	public override void _Ready()
	{
		foreach (var node in CardsContainer.GetChildren())
		{
			var card = (Card)node;
			card.TowerPlacement = TowerPlacement;
		}

		_background = GetNode<NinePatchRect>("NinePatchRect");
		
		Card.CardSelected += LockOtherCards;
		Card.CardDeselected += OnCardDeselected;
		
		var timer = new Timer
		{
			WaitTime = 0.5,
			Autostart = false,
			OneShot = true
		};
		
		AddChild(timer);
		timer.Timeout += UpdateCardsPositions;
		timer.Start();
		
		// GetTree().CreateTimer(0.5).Timeout += HideDeckAnimation;
		
		_background.Position = new Vector2(-132, 64);
		_hidden = true;
		
		var startingCards = Mathf.FloorToInt(GameData.GetModifier("starting_cards"));
		for (var i = 0; i < startingCards; i++)
		{
			RollCard();
		}
	}

	private void OnCardDeselected(Node node, bool inDeck)
	{
		RemoveCard(node, inDeck);
		UnlockAllCards();
	}
	
	public override void _ExitTree()
	{
		Card.CardSelected -= LockOtherCards;
		Card.CardDeselected -= OnCardDeselected;
	}

	public override void _Process(double delta)
	{
		var mousePos = GetGlobalMousePosition();
		
		if (CollisionRect.GetGlobalRect().HasPoint(mousePos))
		{
			if (!_animating && _hidden)
			{
				ShowDeckAnimation();
			}
			if (_hoveringBackground) return;
			_hoveringBackground = true;
			MouseEnteredDeck();
		}
		else
		{
			if (!_animating && !_hidden)
			{
				HideDeckAnimation();
			}
			if (!_hoveringBackground) return;
			_hoveringBackground = false;
			MouseExitedDeck();
		}
	}

	private void MouseEnteredDeck()
	{
		if (_placing)
		{
			EnteredDeck?.Invoke();
		}
		ShowDeckAnimation();
	}

	private void MouseExitedDeck()
	{
		if (_placing)
		{
			ExitedDeck?.Invoke();
		}
		if (_animating) _hidden = false;
		HideDeckAnimation();
	}
	
	public bool IsInDeck() => _hoveringBackground;

	private void LockOtherCards(Node node, bool inDeck)
	{
		_placing = true;
		foreach (var cardNode in CardsContainer.GetChildren())
		{
			var card = (Card)cardNode;
			if (card != node)
			{
				card.Locked = true;
			}
		}
	}

	private void UnlockAllCards(Node scene, bool inDeck)
	{
		_placing = false;
		_ = UpdateCards();
	}
	
	private void UnlockAllCards()
	{
		foreach (var node in CardsContainer.GetChildren())
		{
			var card = (Card)node;
			card.Locked = false;
		}
	}
	
	private void LockAllCards()
	{
		foreach (var node in CardsContainer.GetChildren())
		{
			var card = (Card)node;
			card.Locked = true;
		}
	}

	private void UpdateCardsPositions()
	{
		foreach (var node in CardsContainer.GetChildren())
		{
			var card = (Card)node;
			card.UpdatePositions();
		}
	}

	private void ShowDeckAnimation()
	{
		if (_animating || !_hidden) return;
		
		_animating = true;
		AnimationPlayer.Play("ShowDeck");
		if (!_placing) UnlockAllCards();
		GetTree().CreateTimer(0.6).Timeout += () =>
		{
			_hidden = false;
			_animating = false;
		};
	}

	private void HideDeckAnimation()
	{
		if (_animating || _hidden) return;
		
		_animating = true;
		if (!_placing) LockAllCards();
		AnimationPlayer.Play("HideDeck");
		GetTree().CreateTimer(0.6).Timeout += () =>
		{
			_hidden = true;
			_animating = false;
		};
	}

	private void UpdateDeckSeparation()
	{
		int containerWidth = (int)Math.Ceiling(GetNode<NinePatchRect>("NinePatchRect").Size.X);
		
		int cardCount = CardsContainer.GetChildCount();
		int cardWidth = 48;
		int totalCardWidth = cardCount * cardWidth;
		
		int availableSpace = containerWidth - totalCardWidth;
		int newSeparation = 0;
		if (availableSpace > 0)
		{
			newSeparation = 4;
		}
		else
		{
			newSeparation = availableSpace / Math.Max(cardCount - 2, 1);
		}
		
		CardsContainer.AddThemeConstantOverride("separation", newSeparation);
	}

	public void AddCard(PackedScene tower)
	{
		var card = CardScene.Instantiate<Card>();
		card.CardDeck = this;
		card.TowerPlacement = TowerPlacement;
		card.TowerToPlace = tower;
		CardsContainer.AddChild(card);
		
		_ = UpdateCards();
	}
	
	private async Task DelaySeconds(double seconds)
	{
		var timer = GetTree().CreateTimer(seconds);
		await ToSignal(timer, "timeout");
	}
	
	private async Task UpdateCards()
	{
		LockAllCards();
		await DelaySeconds(0.1);

		UpdateDeckSeparation();
		CardsContainer.QueueSort();
		CardsContainer.QueueRedraw();

		await ToSignal(GetTree(), "process_frame");

		UpdateCardsPositions();
		UnlockAllCards();
	}

	public void RemoveCard(Node node, bool inDeck)
	{
		if (!inDeck)
		{
			node.QueueFree();
			_ = UpdateCards();
		}
	}
	
	public void RemoveCard(int index)
	{
		CardsContainer.GetChildren()[index].QueueFree();
	}
	
	public bool IsHidden => _hidden;
	
	private PackedScene _pickTower()
	{
		return PossibleTowers[Random.Shared.Next(PossibleTowers.Length)];
	}

	public void RollCard()
	{
		AddCard(_pickTower());
	}
	
	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventKey keyEvent && keyEvent.Pressed && keyEvent.Keycode == Key.A)
		{
			RollCard();
		}
	}
}
