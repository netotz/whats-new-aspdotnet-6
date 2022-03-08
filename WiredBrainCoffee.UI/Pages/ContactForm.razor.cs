using WiredBrainCoffee.UI.Components;

namespace WiredBrainCoffee.UI.Pages;

public partial class ContactForm
{
    public Type ContactType { get; private set; }

    protected override void OnInitialized()
    {
        ContactType = typeof(CustomerContact);
        base.OnInitialized();
    }

    private void OnSelectedContactTypeChanged(string newContactType)
    {
        ContactType = newContactType switch
        {
            nameof(CustomerContact) => typeof(CustomerContact),
            nameof(PartnerContact) => typeof(PartnerContact),
            _ => throw new ArgumentException($"The type {newContactType} is not valid."),
        };
    }
}
