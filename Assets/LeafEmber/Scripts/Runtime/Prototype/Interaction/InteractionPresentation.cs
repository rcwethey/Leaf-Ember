namespace LeafEmber.Prototype.Interaction
{

public sealed class InteractionPresentation
{
    public string Location { get; }

    public string Category { get; }

    public string Action { get; }

    public string Explanation { get; }

    public string Cost { get; }

    public InteractionPresentation(
        string location,
        string category,
        string action,
        string explanation,
        string cost)
    {
        Location = location;
        Category = category;
        Action = action;
        Explanation = explanation;
        Cost = cost;
    }
}
}
