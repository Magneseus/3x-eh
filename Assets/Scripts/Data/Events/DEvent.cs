public abstract class DEvent {

    public string promptText;
    public delegate bool activationCondition(DEvent e);
    public activationCondition actCondition;
    public DCity city;

    public abstract void Activate();
    public abstract void Resolve(string selection = Constants.NO_INPUT);
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
}
