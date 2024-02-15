using System.Linq.Expressions;

namespace Buform;

public class SegmentsFormItem<TValue> : ValidatableFormItem<TValue>, ISegmentsFormItem
{
    private string? _label;
    private Func<TValue?, string?>? _formatter;
    private IEnumerable<TValue>? _source;

    IEnumerable<ISegmentsOptionFormItem> ISegmentsFormItem.Items => Items;

    public virtual string? Label
    {
        get => _label;
        set
        {
            _label = value;

            NotifyPropertyChanged();
        }
    }

    public virtual Func<TValue?, string?>? Formatter
    {
        get => _formatter;
        set
        {
            _formatter = value;

            foreach (var item in Items)
            {
                item.Formatter = Formatter;
            }

            NotifyPropertyChanged(nameof(Formatter));
        }
    }

    public virtual IEnumerable<TValue>? Source
    {
        get => _source;
        set
        {
            _source = value;

            Items = _source?.Select(CreateItem) ?? Array.Empty<SegmentsOptionFormItem<TValue>>();

            NotifyPropertyChanged(nameof(Source));
            NotifyPropertyChanged(nameof(Items));
        }
    }

    public virtual IEnumerable<SegmentsOptionFormItem<TValue>> Items { get; private set; }

    public SegmentsFormItem(Expression<Func<TValue>> targetProperty) : base(targetProperty)
    {
        Items = Array.Empty<SegmentsOptionFormItem<TValue>>();
    }

    protected virtual SegmentsOptionFormItem<TValue> CreateItem(TValue value)
    {
        return new SegmentsOptionFormItem<TValue>(value)
        {
            Formatter = Formatter
        };
    }

    protected override void Dispose(bool isDisposing)
    {
        if (isDisposing)
        {
            _formatter = null;
            _source = null;
        }

        base.Dispose(isDisposing);
    }
}

public class SegmentsFormItem : SegmentsFormItem<string?>
{
    public SegmentsFormItem(Expression<Func<string?>> targetProperty) : base(targetProperty)
    {
        /* Required constructor */
    }
}