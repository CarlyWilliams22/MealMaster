public static class GameEvent
{
    public const string PAUSE = "PAUSE"; // bool
    public const string CHANGED_MOUSE_SENSITIVITY = "CHANGED_MOUSE_SENSITIVITY"; // float
    public const string CHANGED_CASH = "CHANGED_CASH"; // float
    public const string LEVEL_COMPLETE = "LEVEL_COMPLETE"; // bool win/lose
    public const string GRAB_HOLDABLE = "GRAB_HOLDABLE"; // HoldableScript, bool grab/release, GameObject
    public const string CLICK_INTERACTABLE = "CLICK_INTERACTABLE"; // InteractableScript
    public const string CUSTOMER_CHANGE_ACTIVE = "CUSTOMER_CHANGE_ACTIVE"; // customer script. bool
    public const string CHANGED_TIME_OF_DAY = "CHANGED_TIME_OF_DAY"; //int
    public const string AREA_TRACKER_ENTER_AREA = "AREA_TRACKER_ENTER_AREA"; // AreaManagerScript, AreaTrackerScript
    public const string AREA_TRACKER_EXIT_AREA = "AREA_TRACKER_EXIT_AREA"; // AreaManagerScript, AreaTrackerScript
    public const string CUSTOMER_LEAVE_SPOT = "CUSTOMER_LEAVE_SPOT"; // CustomerScript
    public const string FOOD_ITEM_COOKED = "FOOD_ITEM_COOKED"; // FoodItemScript
    public const string ON_FIRE = "ON_FIRE"; //BurgerScript, BGAMScript
    public const string OFF_FIRE = "OFF_FIRE"; //BurgerScript, BGAMScript
}
