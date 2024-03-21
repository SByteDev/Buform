using ObjCRuntime;

namespace Buform;

[Preserve(AllMembers = true)]
public abstract class MultiValuePickerFormCellBase<TMultiValuePickerItem> : PickerFormCellBase<TMultiValuePickerItem> where TMultiValuePickerItem : class, IMultiValuePickerFormItem
{
    protected virtual PickerPresenterBase<TMultiValuePickerItem>? PickerPresenter
    {
        get;
        private set;
    }

    public override bool IsSelectable => !Item?.IsReadOnly ?? false;

    protected MultiValuePickerFormCellBase()
    {
        /* Required constructor */
    }

    protected MultiValuePickerFormCellBase(NativeHandle handle)
        : base(handle)
    {
        /* Required constructor */
    }

    protected virtual UIViewController CreateViewController(TMultiValuePickerItem item)
    {
        return new PickerViewController<IMultiValuePickerFormItem>(
            UITableViewStyle.InsetGrouped,
            item
        );
    }

    private void UpdateInputType()
    {
        if (Item == null)
        {
            return;
        }

        PickerPresenter?.Dispose();

        PickerPresenter = Item.InputType switch
        {
            PickerInputType.Default
                => new DefaultPickerPresenter<TMultiValuePickerItem>(CreateViewController),
            PickerInputType.Dialog
                => new DialogPickerPresenter<TMultiValuePickerItem>(CreateViewController),
            PickerInputType.PopUp
                => new DefaultPickerPresenter<TMultiValuePickerItem>(CreateViewController),
            _ => throw new ArgumentOutOfRangeException(nameof(Item.InputType), Item.InputType, null)
        };
    }

    protected override void OnItemSet()
    {
        UpdateReadOnlyState();
        UpdateLabel(Item?.Label);
        UpdateInputType();
        UpdateValue(Item?.FormattedValue);
        UpdateValidationErrorMessage(Item?.ValidationErrorMessage);
    }

    protected override void OnItemPropertyChanged(string? propertyName)
    {
        switch (propertyName)
        {
            case nameof(Item.IsReadOnly):
                UpdateReadOnlyState();
                break;
            case nameof(Item.Label):
                UpdateLabel(Item?.Label);
                break;
            case nameof(Item.InputType):
                UpdateInputType();
                break;
            case nameof(Item.FormattedValue):
                UpdateValue(Item?.FormattedValue);
                break;
            case nameof(Item.ValidationErrorMessage):
                UpdateValidationErrorMessage(Item?.ValidationErrorMessage);
                break;
        }
    }

    public override async void OnSelected()
    {
        base.OnSelected();

        if (Item == null)
        {
            return;
        }

        if (PickerPresenter == null)
        {
            return;
        }

        await PickerPresenter.PickAsync(this, Item).ConfigureAwait(true);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            PickerPresenter?.Dispose();
            PickerPresenter = null;
        }

        base.Dispose(disposing);
    }
}
