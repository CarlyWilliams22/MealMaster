public static class GameEvent
{
    public const string PAUSE = "PAUSE"; // bool
    public const string CHANGED_MOUSE_SENSITIVITY = "CHANGED_MOUSE_SENSITIVITY"; // float
    public const string CHANGED_CASH = "CHANGED_CASH"; // float
    public const string LEVEL_COMPLETE = "LEVEL_COMPLETE"; // bool win/lose
    public const string GRAB_HOLDABLE = "GRAB_HOLDABLE"; // HoldableScript, bool grab/release
    public const string CLICK_INTERACTABLE = "CLICK_INTERACTABLE"; // InteractableScript
    public const string CUSTOMER_ENABLE = "CUSTOMER_ENABLE"; // customer script
}
