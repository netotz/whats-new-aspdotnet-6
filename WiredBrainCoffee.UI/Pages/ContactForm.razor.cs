using WiredBrainCoffee.UI.Components;

namespace WiredBrainCoffee.UI.Pages;

public partial class ContactForm
{
    public record ContactType(Type Type, string Title);

    public ContactType[] ContactTypes { get; } = new[]
    {
        new ContactType(typeof(CustomerContact), CustomerContact.Title),
        new ContactType(typeof(PartnerContact), PartnerContact.Title)
    };
    public int SelectedIndex { get; private set; } = 0;
    public ContactType SelectedContactType => ContactTypes[SelectedIndex];
}
