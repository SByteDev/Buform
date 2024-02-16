using System.Text.RegularExpressions;
using ObjCRuntime;

namespace Buform;

[Preserve(AllMembers = true)]
public sealed class TextFormGroupFooter : FormHeaderFooter<TextFormGroup>
{
    public TextFormGroupFooter()
    {
        /* Required constructor */
    }

    public TextFormGroupFooter(NativeHandle handle) : base(handle)
    {
        /* Required constructor */
    }

    private void UpdateLabel()
    {
        TextLabel.Text = Group?.FooterLabel;
    }

    protected override void OnGroupSet()
    {
        UpdateLabel();
    }

    protected override void OnGroupPropertyChanged(string propertyName)
    {
        switch (propertyName)
        {
            case nameof(Group.FooterLabel):
                UpdateLabel();
                break;
        }
    }
}