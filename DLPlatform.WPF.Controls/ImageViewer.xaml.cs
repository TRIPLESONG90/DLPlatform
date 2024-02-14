using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DLPlatform.WPF.Controls
{
    /// <summary>
    /// ImageViewer.xaml에 대한 상호 작용 논리
    /// </summary>
    [ContentProperty(nameof(Children))] 
    public partial class ImageViewer : UserControl
    {
        static class ImageZoomer
        {
            static double ScaleStep = 0.05;
            static double CurrentScale = 1;
            public static double Zoom(ImageViewer viewer, MouseWheelEventArgs eventArgs)
            {
                var point = eventArgs.GetPosition(viewer.viewBox);
                var delta = eventArgs.Delta;

                Matrix matrix = viewer.viewBox.RenderTransform.Value;
                double ratio = delta > 0 ? 1 + ScaleStep : 1 - ScaleStep;
                double scale = CurrentScale * matrix.M11 * ratio;
                Point ptTranslate = viewer.viewBox.TranslatePoint(point, viewer.viewBox);

                if ((scale > 80) || (scale < 0.2))
                    ratio = 1.0;

                matrix.ScaleAtPrepend(ratio, ratio, ptTranslate.X, ptTranslate.Y);
                viewer.viewBox.RenderTransform = new MatrixTransform(matrix);

                eventArgs.Handled = true;

                return CalcZoomRatio(viewer, scale);
            }

            private static double CalcZoomRatio(ImageViewer viewer, double scale)
            {
                double ratio = 1.0;
                if (scale == 0)
                {

                    if (viewer.imageGrid.Width > viewer.imageGrid.Height)
                        ratio = (double)viewer.viewBox.ActualWidth / (double)viewer.imageGrid.Width;
                    else
                        ratio = (double)viewer.viewBox.ActualHeight / (double)viewer.imageGrid.Height;
                }
                else
                {
                    double initVal = 0;
                    if (viewer.imageGrid.Width > viewer.imageGrid.Height)
                        initVal = viewer.viewBox.ActualWidth;
                    else
                        initVal = viewer.viewBox.ActualHeight;
                    double currentVal = initVal * scale;
                    ratio = currentVal / initVal;
                }
                return Math.Round(ratio * 100, 2);
            }
        }

        class ImageMover
        {
            ImageViewer _viewer;
            Point? _startPt;
            public ImageMover(ImageViewer viewer)
            {
                _viewer = viewer;
                _viewer.mainGrid.MouseEnter += (s, e) =>
                {
                    _viewer.mainGrid.Focusable = true;
                    _viewer.mainGrid.Focus();
                };
                _viewer.mainGrid.MouseLeave += (s, e) =>
                {
                    _viewer.mainGrid.Cursor = Cursors.Arrow;
                    _viewer.mainGrid.Focusable = false;
                };
                _viewer.mainGrid.KeyDown += (s, e) =>
                {
                    if (e.Key == _viewer.MoveKey)
                        _viewer.mainGrid.Cursor = Cursors.ScrollAll;
                };
                _viewer.mainGrid.KeyUp += (s, e) =>
                {
                    if (e.Key == _viewer.MoveKey)
                        _viewer.mainGrid.Cursor = Cursors.Arrow;
                };
                _viewer.mainGrid.MouseDown += (s, e) =>
                {
                    if (e.ChangedButton == MouseButton.Left && Keyboard.IsKeyDown(_viewer.MoveKey))
                    {
                        _startPt = e.GetPosition(_viewer);
                        Mouse.Capture(_viewer.mainGrid);
                        e.Handled = true;
                    }
                };
                _viewer.mainGrid.MouseUp += (s, e) =>
                {
                    Mouse.Capture(null);
                };
            }
            public void Move(MouseEventArgs e)
            {
                var isMoveKeyDown = Keyboard.IsKeyDown(_viewer.MoveKey);

                var pt = e.GetPosition(_viewer.imageGrid);
                if ((pt.X >= 0 && pt.Y >= 0) && (pt.X <= _viewer.imageGrid.ActualWidth && pt.Y <= _viewer.imageGrid.ActualHeight))
                    _viewer.MousePosition = new Point(Math.Ceiling(pt.X), Math.Ceiling(pt.Y));

                if (_startPt == null || !isMoveKeyDown || Mouse.LeftButton != MouseButtonState.Pressed)
                {
                    return;
                }

                var currentPt = e.GetPosition(_viewer);
                var startPt = _startPt.Value;

                var matrix = _viewer.viewBox.RenderTransform.Value;
                matrix.Translate(currentPt.X - startPt.X, currentPt.Y - startPt.Y);
                _viewer.viewBox.RenderTransform = new MatrixTransform(matrix);
                _startPt = currentPt;
                if (isMoveKeyDown)
                {
                    _viewer.mainGrid.Cursor = Cursors.ScrollAll;
                    e.Handled = true;
                }
            }
        }

        public static readonly DependencyProperty ImageUriProperty = 
            DependencyProperty.Register(
                "ImageUri", 
                typeof(Uri), 
                typeof(ImageViewer), 
                new PropertyMetadata(null, new ((dp, arg) =>
                    {
                        var control = dp as ImageViewer;
                        var uri = arg.NewValue as Uri;
                        if (uri != null)
                            control.LoadImageAsync(uri);
                    }))
                );
        public Uri ImageUri
        {
            get { return (Uri)GetValue(ImageUriProperty); }
            set { SetValue(ImageUriProperty, value); }
        }

        public static readonly DependencyProperty ZoomRatioProperty = 
            DependencyProperty.Register(
                "ZoomRatio", 
                typeof(double),
                typeof(ImageViewer), 
                new PropertyMetadata(100.0)
                );
        public double ZoomRatio
        {
            get => (double)GetValue(ZoomRatioProperty);
            set => SetValue(ZoomRatioProperty, value);
        }

        public static readonly DependencyProperty MoveKeyProperty = DependencyProperty.Register("MoveKey", typeof(Key), typeof(ImageViewer), new PropertyMetadata(Key.LeftCtrl));

        public Key MoveKey
        {
            get => (Key)GetValue(MoveKeyProperty);
            set => SetValue(MoveKeyProperty, value);
        }

        public static readonly DependencyProperty MousePositionProperty = DependencyProperty.Register("MousePosition", typeof(Point), typeof(ImageViewer));
        public Point MousePosition
        {
            get => (Point)GetValue(MousePositionProperty);
            set => SetValue(MousePositionProperty, value);
        }

        ImageMover imageMover;

        public static readonly DependencyPropertyKey ChildrenProperty = DependencyProperty.RegisterReadOnly(
            nameof(Children),  // Prior to C# 6.0, replace nameof(Children) with "Children"
            typeof(UIElementCollection),
            typeof(ImageViewer),
            new PropertyMetadata());

        public UIElementCollection Children
        {
            get { return (UIElementCollection)GetValue(ChildrenProperty.DependencyProperty); }
            private set { SetValue(ChildrenProperty, value); }
        }
        public ImageViewer()
        {
            InitializeComponent();
            Children = PART_Host.Children;
            imageMover = new ImageMover(this);
            MouseWheel += (s, e) => ZoomRatio = ImageZoomer.Zoom(this, e);
            MouseMove +=(s, e) => imageMover.Move(e);
        }


        private async void LoadImageAsync(Uri uri)
        {
            await Task.Factory.StartNew(() =>
            {
                Debug.WriteLineIf(uri != null, uri.ToString());

                this.Dispatcher.Invoke(() =>
                {
                    imageGrid.Children.Clear();
                    Image image = new Image();
                    image.Source = new BitmapImage(uri);
                    imageGrid.Children.Add(image);
                    mainGrid.Width = image.Source.Width;
                    mainGrid.Height = image.Source.Height;
                });
            });
        }
    }
}
