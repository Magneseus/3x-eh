public abstract class DEvent {

    public static DGame dGame { get; set; }

    public static activationCondition ParseActivationCondition(string condition)
    {
        switch (condition)
        {
            case "exploring":
                activationCondition actExplore = e => e.City.HasPeopleInTask(typeof(DTask_Explore));
                return actExplore;
            case Constants.IQALUIT_CITY_NAME:
                activationCondition actIqaluit = e => e.City.Name == Constants.IQALUIT_CITY_NAME;
                return actIqaluit;
            case Constants.OTTAWA_CITY_NAME:
                activationCondition actOttawa = e => e.City.Name == Constants.OTTAWA_CITY_NAME;
                return actOttawa;
            case Constants.VANCOUVER_CITY_NAME:
                activationCondition actVancouver = e => e.City.Name == Constants.VANCOUVER_CITY_NAME;
                return actVancouver;
            case "true":
            default:
                activationCondition actTrue = e => true;
                return actTrue;
        }
    }

    public string promptText;
    public delegate bool activationCondition(DEvent e);
    public activationCondition actCondition;
    public DCity city;
    public int priority = Constants.EVENT_PRIORITY_DEFAULT;
    public int turnsToActivation;

    public abstract void Activate();
    public abstract void Resolve(int selection = Constants.NO_INPUT);
    public bool ActivationCondition()
    {
        return actCondition(this);
    }

    public string PromptText
    {
        get { return promptText; }
        set { promptText = value; }
    }

    public DCity City
    {
        get { return city; }
        set { city = value; }
    }

    public int Priority
    {
        get { return priority; }
        set { priority = value; }
    }

    public int TurnsToActivation
    {
        get { return turnsToActivation; }
        set { turnsToActivation = value; }
    }
}
