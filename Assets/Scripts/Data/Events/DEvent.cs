public abstract class DEvent {

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
