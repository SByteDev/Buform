using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Buform;

public abstract class FormItem<TValue> : IFormItem
{
    private PropertyInfo? _propertyInfo;
    private Expression<Func<TValue>>? _property;
    private TValue? _value;
    private bool _isReadOnly;
    private bool _isVisible;
    private bool _shouldSkipValueChangedCallback;

    protected Form? Form { get; private set; }

    public object? Target { get; private set; }

    public string? PropertyName { get; }

    public virtual bool IsReadOnly
    {
        get => _isReadOnly;
        set
        {
            _isReadOnly = value;

            NotifyPropertyChanged();
        }
    }

    public bool IsVisible
    {
        get => _isVisible;
        set
        {
            if (_isVisible == value)
            {
                return;
            }

            _isVisible = value;

            NotifyPropertyChanged();
            OnIsVisibleChanged();
        }
    }

    object? IFormItem.Value
    {
        get => Value;
        set => Value = (TValue?)value;
    }

    public TValue? Value
    {
        get => GetValue();
        set => SetValue(value);
    }

    protected virtual bool IsValueChanged { get; set; }

    public virtual Action<Form, TValue?>? ValueChangedCallback { get; set; }

    public event EventHandler<FormValueChangedEventArgs>? ValueChanged;
    public event EventHandler? VisibilityChanged;

    public event PropertyChangedEventHandler? PropertyChanged;

    protected FormItem(Expression<Func<TValue>> property)
    {
        ArgumentNullException.ThrowIfNull(property);

        _property = property;

        PropertyName = _property.GetMemberName();

        _isVisible = true;
    }

    protected FormItem(TValue value)
    {
        ArgumentNullException.ThrowIfNull(value);

        _value = value;

        _isVisible = true;
    }

    private TValue? GetValue()
    {
        if (_propertyInfo == null)
        {
            return _value;
        }

        return Target == null ? default : (TValue?)_propertyInfo.GetValue(Target);
    }

    private void SetValue(TValue? value)
    {
        if (Target == null)
        {
            return;
        }

        if (!Equals(value, Value))
        {
            IsValueChanged = true;
        }

        _shouldSkipValueChangedCallback = true;

        _propertyInfo?.SetValue(Target, value);

        _shouldSkipValueChangedCallback = false;

        OnValueChanged();
    }

    private void OnTargetPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName != PropertyName && !string.IsNullOrWhiteSpace(e.PropertyName))
        {
            return;
        }

        OnValueChanged();
    }

    protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected virtual void OnValueChanged()
    {
        if (Form == null)
        {
            return;
        }

        NotifyPropertyChanged(nameof(Value));

        ValueChanged?.Invoke(this, new FormValueChangedEventArgs(PropertyName ?? string.Empty));

        if (_shouldSkipValueChangedCallback)
        {
            return;
        }

        ValueChangedCallback?.Invoke(Form, Value);
    }

    protected virtual void OnIsVisibleChanged()
    {
        VisibilityChanged?.Invoke(this, EventArgs.Empty);
    }

    public void Initialize(Form form, object target)
    {
        ArgumentNullException.ThrowIfNull(form);
        ArgumentNullException.ThrowIfNull(target);

        Form = form;

        if (Target is INotifyPropertyChanged oldNotifyPropertyChanged)
        {
            oldNotifyPropertyChanged.PropertyChanged -= OnTargetPropertyChanged;
        }

        Target = target;

        if (Target is INotifyPropertyChanged newNotifyPropertyChanged)
        {
            newNotifyPropertyChanged.PropertyChanged += OnTargetPropertyChanged;
        }

        _propertyInfo = Target?.GetType().GetProperty(PropertyName ?? string.Empty);

        _shouldSkipValueChangedCallback = true;

        OnValueChanged();

        _shouldSkipValueChangedCallback = false;
    }

    public void Dispose()
    {
        Dispose(true);
    }

    protected virtual void Dispose(bool isDisposing)
    {
        if (!isDisposing)
        {
            return;
        }

        _propertyInfo = null;
        _property = null;

        if (Target is INotifyPropertyChanged notifyPropertyChanged)
        {
            notifyPropertyChanged.PropertyChanged -= OnTargetPropertyChanged;
        }

        Target = null;

        _value = default;
    }
}
