using System;
using Buform.Example.Core;
using CoreGraphics;
using Foundation;
using SByteDev.Common.Extensions;
using UIKit;

namespace Buform.Example.MvvmCross.iOS
{
    [Preserve(AllMembers = true)]
    [Register(nameof(DigitGenerationCell))]
    public sealed class DigitGenerationCell : FormCell<DigitGenerationItem>
    {
        private const string GoForwardIconName = "goforward";

        private UILabel? _titleLabel;
        private UILabel? _valueLabel;
        private UIButton? _button;

        public DigitGenerationCell()
        {
            /* Required constructor */
        }

        public DigitGenerationCell(IntPtr handle) : base(handle)
        {
            /* Required constructor */
        }

        private void UpdateReadOnlyState()
        {
            if (Item == null)
            {
                return;
            }

            if (_button == null)
            {
                return;
            }

            _button.Enabled = Item.IsReadOnly;
        }

        private void UpdateTitle()
        {
            if (_titleLabel == null)
            {
                return;
            }

            _titleLabel.Text = Item?.Title;
        }

        private void UpdateValue()
        {
            if (_valueLabel == null)
            {
                return;
            }

            _valueLabel.Text = Item?.Value.ToString();
        }

        private void ExecuteCommand(object _, EventArgs __)
        {
            Item?.RegenerateCommand.SafeExecute();
        }

        protected override void Initialize()
        {
            SelectionStyle = UITableViewCellSelectionStyle.None;
            ContentView.UserInteractionEnabled = true;

            _titleLabel = new UILabel
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                Font = UIFont.PreferredBody,
                TextColor = UIColor.Label
            };

            _valueLabel = new UILabel
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                Font = UIFont.PreferredBody,
                TextColor = UIColor.SecondaryLabel
            };

            _button = UIButton.FromType(UIButtonType.System);
            _button.Frame = new CGRect(0, 0, 70, 40);
            _button.TouchUpInside += ExecuteCommand;
            _button.SetImage(
                UIImage.GetSystemImage(GoForwardIconName)!,
                UIControlState.Normal
            );

            AccessoryView = _button;

            ContentView.AddSubviews(_titleLabel, _valueLabel);

            ContentView.AddConstraints(new[]
            {
                _titleLabel.TopAnchor.ConstraintEqualTo(ContentView.LayoutMarginsGuide.TopAnchor),
                _titleLabel.BottomAnchor.ConstraintEqualTo(ContentView.LayoutMarginsGuide.BottomAnchor),
                _titleLabel.LeadingAnchor.ConstraintEqualTo(ContentView.LayoutMarginsGuide.LeadingAnchor),
                _valueLabel.TopAnchor.ConstraintEqualTo(ContentView.LayoutMarginsGuide.TopAnchor),
                _valueLabel.BottomAnchor.ConstraintEqualTo(ContentView.LayoutMarginsGuide.BottomAnchor),
                _valueLabel.LeadingAnchor.ConstraintEqualTo(_titleLabel.LayoutMarginsGuide.TrailingAnchor, 16),
            });
        }

        protected override void OnItemSet()
        {
            UpdateReadOnlyState();
            UpdateTitle();
            UpdateValue();
        }

        protected override void OnItemPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(Item.IsReadOnly):
                    UpdateReadOnlyState();
                    break;
                case nameof(Item.Title):
                    UpdateTitle();
                    break;
                case nameof(Item.Value):
                    UpdateValue();
                    break;
            }
        }
    }
}