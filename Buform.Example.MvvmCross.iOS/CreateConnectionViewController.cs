using Buform.Example.Core;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;

namespace Buform.Example.MvvmCross.iOS;

[Foundation.Preserve(AllMembers = true)]
[MvxModalPresentation(
    WrapInNavigationController = true,
    ModalPresentationStyle = UIModalPresentationStyle.FormSheet
)]
public sealed class CreateConnectionViewController
    : MvxTableViewController<CreateConnectionViewModel>
{
    private FormTableViewSource? _source;

    public CreateConnectionViewController()
        : base(UITableViewStyle.InsetGrouped)
    {
        /* Required constructor */
    }

    public override void ViewDidLoad()
    {
        base.ViewDidLoad();

        NavigationItem.LeftBarButtonItem = new UIBarButtonItem(UIBarButtonSystemItem.Cancel);

        TableView.KeyboardDismissMode = UIScrollViewKeyboardDismissMode.Interactive;

        _source = new FormTableViewSource(TableView);

        var set = CreateBindingSet();
        set.Bind(this).For(v => v.Title).To(vm => vm.Title);
        set.Bind(NavigationItem.LeftBarButtonItem).To(vm => vm.CancelCommand);
        set.Bind(_source).For(v => v.Form).To(vm => vm.Form);
        set.Apply();
    }
}
