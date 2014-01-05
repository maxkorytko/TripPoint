using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Collections;
using System.Collections.ObjectModel;

using TripPoint.Model.Utils;

namespace TripPoint.WindowsPhone.View.Controls
{
    /// <summary>
    /// A simple control for navigating through pictures by sliding horizontally
    /// </summary>
    public partial class PictureSlider : UserControl
    {
        private static readonly double IMAGE_BORDER_THICKNESS = 20.0;

        private PictureSliderHelper _sliderHelper;

        public PictureSlider()
        {
            InitializeComponent();

            // data binding won't work if this.DataContext = this
            // we set the DataContext of the layout root instead
            // all child elements will inherit the DataContext from their parent
            // see: http://blog.jerrynixon.com/2013/07/solved-two-way-binding-inside-user.html
            //
            PictureSliderLayoutRoot.DataContext = this;

            Loaded += (sender, args) => OnLoaded();
            Unloaded += (sender, args) => OnUnloaded();
        }

        #region CurrentPicture Dependency Property

        public TripPoint.Model.Domain.Picture CurrentPicture
        {
            get { return GetValue(CurrentPictureProperty) as TripPoint.Model.Domain.Picture; }
            set { SetValue(CurrentPictureProperty, value); }
        }

        public static readonly DependencyProperty CurrentPictureProperty =
            DependencyProperty.Register(
                "CurrentPicture",
                typeof(TripPoint.Model.Domain.Picture),
                typeof(PictureSlider),
                new PropertyMetadata(OnCurrentPictureChanged));

        public static void OnCurrentPictureChanged(DependencyObject owner,
            DependencyPropertyChangedEventArgs args)
        {
            (owner as PictureSlider).SlideToCurrentPicture();
        }

        #endregion

        #region Pictures Dependency Property

        public Collection<TripPoint.Model.Domain.Picture> Pictures
        {
            get { return (Collection<TripPoint.Model.Domain.Picture>)GetValue(PicturesProperty); }
            set { SetValue(PicturesProperty, value); }
        }

        public static readonly DependencyProperty PicturesProperty =
            DependencyProperty.Register(
                "Pictures",
                typeof(Collection<TripPoint.Model.Domain.Picture>),
                typeof(PictureSlider),
                new PropertyMetadata(OnPicturesChanged));

        public static void OnPicturesChanged(DependencyObject owner,
            DependencyPropertyChangedEventArgs args)
        {
            // nothing
        }

        #endregion

        #region Event Handlers

        private void OnLoaded()
        {
            InitializeSliderHelper();
            SlideToCurrentPicture(0.0);
        }

        private void OnUnloaded()
        {
            ClearValue(CurrentPictureProperty);
            ClearValue(PicturesProperty);
        }

        #endregion

        private void InitializeSliderHelper()
        {
            if (_sliderHelper != null) return;

            _sliderHelper = new PictureSliderHelper(this);
            _sliderHelper.Initialize();
        }

        private void SlideToCurrentPicture()
        {
            SlideToCurrentPicture(0.5);
        }

        private void SlideToCurrentPicture(double duration)
        {
            if (_sliderHelper != null) _sliderHelper.SlideToCurrentPicture(duration);
        }

        #region PictureSliderHelper Class

        /// <summary>
        /// Helps automatically slide a picture into the visible area to ensure it's fully displayed
        /// </summary>
        class PictureSliderHelper
        {
            private bool _initialized;
            private PictureSlider _slider;
            private PictureScrollHelper _scrollHelper;

            public PictureSliderHelper(PictureSlider slider)
            {
                if (slider == null) throw new ArgumentException("slider");

                _slider = slider;
            }
            
            #region Initialization
            
            public void Initialize()
            {
                if (_initialized) return;

                InitializeScrollHelper();
                RegisterScrollStateChangeHandler();

                _initialized = true;
            }

            private void InitializeScrollHelper()
            {
                if (_scrollHelper != null) return;

                _scrollHelper = new PictureScrollHelper(_slider.picturesScrollViewer);
            }

            private void RegisterScrollStateChangeHandler()
            {
                var group = FindVisualStateGroup(_scrollHelper.ScrollViewer, "ScrollStates");

                RegisterHandlerForVisualStateGroupChangingEvent(group, (sender, args) =>
                {
                    if (args.NewState.Name == "NotScrolling")
                    {
                        OnStopScrolling();
                    }
                });
            }

            /// <summary>
            /// Looks for a visual state group with the given name in the given UI element
            /// </summary>
            /// <param name="element">Any UI control, such as Button, ScrollViewer, etc</param>
            /// <param name="groupName">Visual state group to look for</param>
            /// <returns></returns>
            private static VisualStateGroup FindVisualStateGroup(FrameworkElement element, string groupName)
            {
                if (element == null) throw new ArgumentException("element");

                // Visual States are always on the first child of the control template
                var visualStates = VisualTreeHelper.GetChild(element, 0) as FrameworkElement;

                if (visualStates != null)
                {
                    IList groups = VisualStateManager.GetVisualStateGroups(visualStates);

                    foreach (VisualStateGroup group in groups)
                    {
                        if (group.Name == groupName) return group;
                    }
                }

                return null;
            }

            private void RegisterHandlerForVisualStateGroupChangingEvent(VisualStateGroup group,
                EventHandler<VisualStateChangedEventArgs> handler)
            {
                if (group == null || handler == null) return;

                group.CurrentStateChanging += handler;
            }

            #endregion

            #region Sliding Handling

            private void OnStopScrolling()
            {
                SlideToPictureAtIndex(_scrollHelper.GetIndexOfItemInViewport());
            }

            private void SlideToPictureAtIndex(int index)
            {
                if (index < 0 || index >= _slider.Pictures.Count) return;

                var picture = _slider.Pictures[index];

                if (_slider.CurrentPicture == picture) SlideToCurrentPicture(0.25);
                else _slider.CurrentPicture = picture;
            }

            /// <summary>
            /// Slides the current picture into the view making it fully visible
            /// </summary>
            /// <param name="duration">Duration of the slide in seconds</param>
            public void SlideToCurrentPicture(double duration)
            {
                if (_slider.Pictures == null || _slider.CurrentPicture == null) return;

                var currentPictureIndex = _slider.Pictures.IndexOf(_slider.CurrentPicture);

                _scrollHelper.ScrollToItemAtIndex(currentPictureIndex, duration);
            }

            #endregion
        }

        #endregion

        #region PictureScrollHelper Class

        /// <summary>
        /// Encapsulates scrolling behavior of items inside of a ScrollViewer instance
        /// The behavior is based on the fact that only one item can be visible at a time
        /// </summary>
        class PictureScrollHelper : FrameworkElement
        {
            private static readonly string HORIZONTAL_OFFSET_PROPERTY = "HorizontalOffset";

            private ScrollViewer _scrollViewer;

            public PictureScrollHelper(ScrollViewer scrollViewer)
            {
                if (scrollViewer == null) throw new ArgumentException("scrollViewer");

                _scrollViewer = scrollViewer;
            }

            public ScrollViewer ScrollViewer
            {
                get { return _scrollViewer; }
            }

            #region HorizontalOffset Dependency Property

            public double HorizontalOffset
            {
                get { return (double)GetValue(HorizontalOffsetProperty); }
                set { SetValue(HorizontalOffsetProperty, value); }
            }

            public static readonly DependencyProperty HorizontalOffsetProperty =
                DependencyProperty.Register(
                    HORIZONTAL_OFFSET_PROPERTY,
                    typeof(double),
                    typeof(PictureScrollHelper),
                    new PropertyMetadata(OnHorizontalOffsetChanged));

            public static void OnHorizontalOffsetChanged(DependencyObject owner,
                DependencyPropertyChangedEventArgs args)
            {
                var scrollHelper = owner as PictureScrollHelper;

                if (scrollHelper != null)
                {
                    scrollHelper._scrollViewer.ScrollToHorizontalOffset((double)args.NewValue);
                }
            }

            #endregion

            /// <summary>
            /// Determines the 0-based index of an item currenlty displayed in the view port
            /// If scrolling stopped in the middle (2 items are partially visible), the index of the most
            /// visible item is returned
            /// </summary>
            /// <returns>0-based index of the most visible item</returns>
            public int GetIndexOfItemInViewport()
            {
                var itemIndex = 0;
                var viewportWidth = _scrollViewer.ViewportWidth;

                if (viewportWidth > 0.0)
                {
                    viewportWidth += IMAGE_BORDER_THICKNESS; 
                    itemIndex = Convert.ToInt32(Math.Round(_scrollViewer.HorizontalOffset / viewportWidth));
                }

                return itemIndex;
            }

            /// <summary>
            /// Programmatically scrolls to an item at the given index
            /// Scrolling is animated
            /// </summary>
            /// <param name="itemIndex">Must not be negative.</param>
            /// <param name="duration">Duration of the animation in seconds</param>
            public void ScrollToItemAtIndex(int itemIndex, double duration)
            {
                if (itemIndex < 0) return;

                var horizontalOffset = itemIndex * _scrollViewer.ViewportWidth;
                horizontalOffset += itemIndex * PictureSlider.IMAGE_BORDER_THICKNESS;

                ScrollToHorizontalOffset(horizontalOffset, duration);
            }

            private void ScrollToHorizontalOffset(double offset, double duration)
            {
                CreateHorizontalOffsetAnimation(_scrollViewer.HorizontalOffset, offset, duration).Begin();
            }

            private Storyboard CreateHorizontalOffsetAnimation(double from, double to, double seconds)
            {
                var animation = new DoubleAnimation();

                animation.Duration = new Duration(TimeSpan.FromSeconds(seconds));
                animation.From = from;
                animation.To = to;

                Storyboard.SetTarget(animation, this);
                Storyboard.SetTargetProperty(animation, new PropertyPath(HORIZONTAL_OFFSET_PROPERTY));

                var storyboard = new Storyboard();

                storyboard.Duration = animation.Duration;
                storyboard.Children.Add(animation);

                return storyboard;
            }
        }

        #endregion
    }
}
