using Godot;

namespace WyrmspireStudios.Events;

public static class EventHandlers
{
    public delegate void BaseHandler();
    public delegate void ValueChangeHandler(int oldValue, int newValue);
    public delegate void CardSelectedHandler(Node node, bool inDeck);

    public delegate void UpgradesChangesHandler(string upgradeId, int level, float modifier);
}