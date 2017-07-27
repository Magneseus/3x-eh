public abstract class DEvent {

    public string promptText;

    public abstract void Activate();
    public abstract void Resolve(string selection = Constants.NO_INPUT);

    public string PromptText
    {
        get { return promptText; }
        set { promptText = value; }
    }
}
