﻿using System;
using System.ComponentModel;
using Android.Views;
using iFactr.UI;
using iFactr.UI.Controls;
using Android.Runtime;
using Android.Content;
using Android.Util;
using Size = iFactr.UI.Size;

namespace iFactr.Droid
{
    public class Switch : Android.Widget.Switch, ISwitch, INotifyPropertyChanged
    {
        #region Constructors

        [Preserve]
        public Switch()
            : base(DroidFactory.MainActivity)
        {
            Initialize();
        }

        public Switch(Context context)
            : base(context)
        {
            Initialize();
        }

        [Preserve]
        public Switch(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
            Initialize(attrs);
        }

        [Preserve]
        public Switch(Context context, IAttributeSet attrs, int defStyle)
            : base(context, attrs, defStyle)
        {
            Initialize(attrs);
        }

        protected Switch(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        private void Initialize(IAttributeSet attrs = null)
        {
            this.InitializeAttributes(attrs);
            Focusable = false;
            CheckedChange += Switch_CheckedChange;
        }

        #endregion

        public void NullifyEvents()
        {
            Validating = null;
            ValueChanged = null;
        }

        #region Value

        public event PropertyChangedEventHandler PropertyChanged;

        private void Switch_CheckedChange(object sender, CheckedChangeEventArgs e)
        {
            (Parent as GridBase)?.SetSubmission(SubmitKey, StringValue);
            this.RaiseEvent("ValueChanged", new ValueChangedEventArgs<bool>(!Value, Value));
            this.OnPropertyChanged("StringValue");
            this.OnPropertyChanged("Value");
            this.OnPropertyChanged("Checked");
        }

        public event ValueChangedEventHandler<bool> ValueChanged;

        public bool Value
        {
            get { return Checked; }
            set { Checked = value; }
        }

        #endregion

        #region Style

        public bool IsEnabled
        {
            get { return Handle != IntPtr.Zero && Enabled; }
            set
            {
                if (Handle == IntPtr.Zero || Enabled == value) return;
                Enabled = value;
                this.OnPropertyChanged();
                this.OnPropertyChanged("Enabled");
            }
        }

        public Color FalseColor
        {
            get { return _falseColor; }
            set
            {
                if (_falseColor == value) return;
                _falseColor = value;
                this.OnPropertyChanged();
            }
        }
        private Color _falseColor;

        public Color ForegroundColor
        {
            get { return _foregroundColor; }
            set
            {
                if (_foregroundColor == value || Handle == IntPtr.Zero) return;
                SetTextColor(value.IsDefaultColor ? Android.Graphics.Color.White : value.ToColor());
                _foregroundColor = value;
                this.OnPropertyChanged();
            }
        }
        private Color _foregroundColor;

        public Color TrueColor
        {
            get { return _trueColor; }
            set
            {
                if (_trueColor == value) return;
                _trueColor = value;
                this.OnPropertyChanged();
            }
        }
        private Color _trueColor;

        #endregion

        #region Submission

        public string StringValue => Checked.ToString().ToLower();

        public string SubmitKey
        {
            get { return _submitKey; }
            set
            {
                if (_submitKey == value) return;
                _submitKey = value;
                this.OnPropertyChanged();
            }
        }
        private string _submitKey;

        public bool Validate(out string[] errors)
        {
            var handler = Validating;
            if (handler != null)
            {
                var args = new ValidationEventArgs(SubmitKey, Value, StringValue);
                handler(Pair ?? this, args);

                if (args.Errors.Count > 0)
                {
                    errors = new string[args.Errors.Count];
                    args.Errors.CopyTo(errors, 0);
                    return false;
                }
            }

            errors = null;
            return true;
        }

        public event ValidationEventHandler Validating;

        #endregion

        #region Layout

        public new Visibility Visibility
        {
            get { return _visibility; }
            set
            {
                if (value == _visibility || Handle == IntPtr.Zero) return;
                var oldVisibility = _visibility;
                _visibility = value;
                switch (value)
                {
                    case Visibility.Visible:
                        base.Visibility = ViewStates.Visible;
                        break;
                    case Visibility.Hidden:
                        base.Visibility = ViewStates.Invisible;
                        break;
                    case Visibility.Collapsed:
                        base.Visibility = ViewStates.Gone;
                        break;
                }
                this.OnPropertyChanged();
                this.RequestResize(oldVisibility == Visibility.Collapsed || _visibility == Visibility.Collapsed);
            }
        }
        private Visibility _visibility;

        public Thickness Margin
        {
            get { return _margin; }
            set
            {
                if (_margin == value) return;
                _margin = value;
                this.OnPropertyChanged();
            }
        }
        private Thickness _margin;

        public int ColumnIndex
        {
            get { return _columnIndex; }
            set
            {
                if (value == _columnIndex) return;
                _columnIndex = value;
                this.OnPropertyChanged();
            }
        }
        private int _columnIndex = Element.AutoLayoutIndex;

        public int ColumnSpan
        {
            get { return _columnSpan; }
            set
            {
                if (value == _columnSpan) return;
                _columnSpan = value;
                this.OnPropertyChanged();
            }
        }
        private int _columnSpan = 1;

        public int RowIndex
        {
            get { return _rowIndex; }
            set
            {
                if (value == _rowIndex) return;
                _rowIndex = value;
                this.OnPropertyChanged();
            }
        }
        private int _rowIndex = Element.AutoLayoutIndex;

        public int RowSpan
        {
            get { return _rowSpan; }
            set
            {
                if (_rowSpan == value) return;
                _rowSpan = value;
                this.OnPropertyChanged();
            }
        }
        private int _rowSpan = 1;

        public HorizontalAlignment HorizontalAlignment
        {
            get { return _horizontalAlignment; }
            set
            {
                if (value == _horizontalAlignment) return;
                _horizontalAlignment = value;
                this.OnPropertyChanged();
            }
        }
        private HorizontalAlignment _horizontalAlignment;

        public VerticalAlignment VerticalAlignment
        {
            get { return _verticalAlignment; }
            set
            {
                if (value == _verticalAlignment) return;
                _verticalAlignment = value;
                this.OnPropertyChanged();
            }
        }
        private VerticalAlignment _verticalAlignment;

        public Size Measure(Size constraints)
        {
            return this.MeasureView(constraints);
        }

        /// <summary>
        /// Sets the location and size of the control within its parent grid.
        /// This is called by the underlying grid layout system and should not be used in application logic.
        /// </summary>
        /// <param name="location">The X and Y coordinates of the upper left corner of the control.</param>
        /// <param name="size">The width and height of the control.</param>
        public void SetLocation(Point location, Size size)
        {
            var left = location.X;
            var right = location.X + MeasuredWidth;
            var top = location.Y;
            var bottom = location.Y + MeasuredHeight;

            Layout((int)left, (int)top, (int)right, (int)bottom);
        }

        #endregion

        #region Identity

        public string ID
        {
            get { return _id; }
            set
            {
                if (_id == value) return;
                _id = value;
                this.OnPropertyChanged();
            }
        }
        private string _id;

        object IElement.Parent => Parent;

        public IPairable Pair
        {
            get { return _pair; }
            set
            {
                if (_pair != null || value == null) return;
                _pair = value;
                _pair.Pair = this;
                this.OnPropertyChanged();
            }
        }
        private IPairable _pair;

        public MetadataCollection Metadata => _metadata ?? (_metadata = new MetadataCollection());
        private MetadataCollection _metadata;

        public bool Equals(IElement other)
        {
            var control = other as Element;
            return control?.Equals(this) ?? ReferenceEquals(this, other);
        }
    }

    #endregion
}