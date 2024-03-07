using Android.Content;
using Android.Runtime;
using Android.Util;
using AndroidX.RecyclerView.Widget;

namespace Buform;

[Preserve(AllMembers = true)]
[Register("Buform.FormRecyclerView")]
public class FormRecyclerView : RecyclerView
{
    public Form? Form
    {
        get => (GetAdapter() as FormRecyclerViewAdapter)!.Form;
        set => (GetAdapter() as FormRecyclerViewAdapter)!.Form = value;
    }

    public override bool HasFixedSize => false;

    protected FormRecyclerView(IntPtr javaReference, JniHandleOwnership transfer)
        : base(javaReference, transfer)
    {
        /* Required constructor */
    }

    public FormRecyclerView(Context context)
        : base(context)
    {
        Initialize();
    }

    public FormRecyclerView(Context context, IAttributeSet? attrs)
        : base(context, attrs)
    {
        Initialize();
    }

    public FormRecyclerView(Context context, IAttributeSet? attrs, int defStyleAttr)
        : base(context, attrs, defStyleAttr)
    {
        Initialize();
    }

    private void Initialize()
    {
        SetLayoutManager(CreateLayoutManager());
        SetAdapter(CreateAdapter());
    }

    protected virtual LayoutManager CreateLayoutManager()
    {
        // TODO: Add support for multiple columns.
        var layoutManager = new GridLayoutManager(Context, 1);

        layoutManager.SetSpanSizeLookup(new GridLayoutManager.DefaultSpanSizeLookup());

        return layoutManager;
    }

    protected virtual Adapter CreateAdapter()
    {
        return new FormRecyclerViewAdapter();
    }
}
