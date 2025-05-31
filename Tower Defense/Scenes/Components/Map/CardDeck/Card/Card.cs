using Godot;
using WyrmspireStudios.Events;

namespace WyrmspireStudios;

public partial class Card : Button
{
    public static event EventHandlers.CardSelectedHandler CardSelected;
    public static event EventHandlers.CardSelectedHandler CardDeselected;

    [Export] public bool Locked;
    [Export] public CardDeck CardDeck;
    [Export] public PackedScene TowerToPlace;
    [Export] public TowerPlacement TowerPlacement;
    [Export] public Vector2 SelectedPositionOffset = new(0, -10);
    [Export] public Vector2 DraggingPositionOffset = new(-24, -72);

    private bool _inDeck;
    private bool _dragging;
    private bool _highlighting;

    private Vector2 _position;
    private Vector2 _defaultPosition;
    private Vector2 _selectedPosition;

    private NinePatchRect _background;
    private ColorRect _highlight;
    private bool _animating;
    private bool _ready = false;

    private Tween _positionTween;

    public override void _Ready()
    {
        _background = GetNode<NinePatchRect>("Background");
        _highlight = GetNode<ColorRect>("Highlight");

        // var towerInstance = TowerToPlace.Instantiate<Tower>();
        // towerInstance.Position = new(8, 16);
        // towerInstance.OnStartPlacing();
        // _background.AddChild(towerInstance);

        ButtonDown += OnButtonDown;
        ButtonUp += OnButtonUp;
        MouseEntered += OnMouseEntered;
        MouseExited += OnMouseExited;
    }

    public override void _ExitTree()
    {
        ButtonDown -= OnButtonDown;
        ButtonUp -= OnButtonUp;
        MouseEntered -= OnMouseEntered;
        MouseExited -= OnMouseExited;
    }

    public void UpdatePositions()
    {
        _position = GetPosition();
        _defaultPosition = GetPosition();
        _selectedPosition = GetPosition() + SelectedPositionOffset;
        _ready = true;
    }

    public override void _Process(double delta)
    {
        if (Locked || !_ready)
        {
            if (!CardDeck.IsInDeck() && !_dragging && _highlighting)
            {
                _background.Position = Vector2.Zero;
                _highlight.Position = new(-1, -1);
                _highlighting = false;
                _highlight.Visible = false;
                ZIndex = 0;
            };
            return;
        };
        
        if (!CardDeck.IsInDeck())
        {
            if (_dragging)
            {
                _background.Visible = false;
            }
            _highlight.Visible = false;
            _highlighting = false;
        }
        else
        {
            _background.Visible = true;
            _highlight.Visible = _highlighting;
        }

        if (!_dragging)
        {
            _background.Visible = true;
        }
        
        var newPosition = _dragging
            ? GetGlobalMousePosition() - CardDeck.GetPosition() + CardDeck.GetNode<NinePatchRect>("NinePatchRect").Size / 2 +
              DraggingPositionOffset
            : _position;
        _positionTween?.Kill();
        _positionTween = CreateTween()
            .SetEase(Tween.EaseType.Out)
            .SetTrans(Tween.TransitionType.Quad);
        _positionTween.TweenProperty(this, "position", newPosition, 0.1f);
        _positionTween.Play();

        ZIndex = _dragging || _highlighting ? 10 : 0;
    }

    private void OnButtonDown()
    {
        if (Locked) return;
        _dragging = true;
        _highlighting = true;
        ZIndex = 10;

        CardSelected?.Invoke(this, CardDeck.IsInDeck());
        _position = _defaultPosition;
        TowerPlacement?.ChangePlacedScene(TowerToPlace);
        TowerPlacement?.StartPlacingTower();
    }

    private void OnButtonUp()
    {
        if (Locked) return;
        _dragging = false;
        _highlighting = false;
        ZIndex = 0;

        _position = _defaultPosition;
        _background.Position = Vector2.Zero;
        _highlight.Position = new(-1, -1);

        if (TowerPlacement.IsPlacing() && !CardDeck.IsInDeck())
        {
            bool success = TowerPlacement.PlaceTower();
            if (success)
            {
                CardDeselected?.Invoke(this, CardDeck.IsInDeck());
                return;
            }
        }

        TowerPlacement.CancelPlacingTower();
        CardDeselected?.Invoke(this, true);
    }

    private void OnMouseEntered()
    {
        if (Locked || _dragging) return;

        _background.Position = SelectedPositionOffset;
        _highlight.Position = SelectedPositionOffset + new Vector2(-1, -1);
        _highlighting = true;
        ZIndex = 10;
    }

    private void OnMouseExited()
    {
        if (Locked || _dragging) return;

        _background.Position = Vector2.Zero;
        _highlight.Position = new(-1, -1);
        _highlighting = false;
        ZIndex = 0;
    }
}