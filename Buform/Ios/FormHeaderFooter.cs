using System.ComponentModel;

namespace Buform;

[Preserve(AllMembers = true)]
public abstract class FormHeaderFooterView : UITableViewHeaderFooterView
{
    protected FormHeaderFooterView()
    {
        /* Required constructor */
    }

    protected FormHeaderFooterView(IntPtr handle) : base(handle)
    {
        /* Required constructor */
    }

    protected UITableView? GetTableView()
    {
        var view = this as UIView;

        while (true)
        {
            switch (view.Superview)
            {
                case null:
                    return null;
                case UITableView tableView:
                    return tableView;
                default:
                    view = view.Superview;
                    break;
            }
        }
    }

    public abstract void Initialize(IFormGroup group);
}

[Preserve(AllMembers = true)]
public abstract class FormHeaderFooter<TGroup> : FormHeaderFooterView where TGroup : class, IFormGroup
{
    protected TGroup? Group { get; private set; }

    protected FormHeaderFooter()
    {
        /* Required constructor */
    }

    protected FormHeaderFooter(IntPtr handle) : base(handle)
    {
        /* Required constructor */
    }

    private void OnGroupPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (Group == null)
        {
            return;
        }

        OnGroupPropertyChanged(e.PropertyName);
    }

    protected virtual void OnGroupSet()
    {
        /* Nothing to do */
    }

    protected virtual void OnGroupPropertyChanged(string? propertyName)
    {
        /* Nothing to do */
    }

    public sealed override void Initialize(IFormGroup group)
    {
        if (ReferenceEquals(Group, group))
        {
            return;
        }

        if (Group != null)
        {
            Group.PropertyChanged -= OnGroupPropertyChanged;
        }

        Group = group as TGroup;

        if (Group == null)
        {
            return;
        }

        Group.PropertyChanged += OnGroupPropertyChanged;

        OnGroupSet();
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            var group = Group;
            if (group != null)
            {
                group.PropertyChanged -= OnGroupPropertyChanged;
            }

            Group = null;
        }

        base.Dispose(disposing);
    }
}