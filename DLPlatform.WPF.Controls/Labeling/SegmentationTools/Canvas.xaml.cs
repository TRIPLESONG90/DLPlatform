using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DLPlatform.WPF.Controls.Labeling.SegmentationTools
{
    /// <summary>
    /// Canvas.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Canvas : UserControl
    {
        CursorGenerator cursorGenerator;
        Drawer drawer;

        public static readonly DependencyProperty BrushTypeProperty =
            DependencyProperty.Register(nameof(BrushType), typeof(BrushTypes),typeof(Canvas),new PropertyMetadata(BrushTypes.Draw));
        public BrushTypes BrushType
        {
            get=>(BrushTypes)GetValue(BrushTypeProperty);
            set=>SetValue(BrushTypeProperty,value);
        }
        public static readonly DependencyProperty BrushShapeProperty =
            DependencyProperty.Register(nameof(BrushShape), typeof(BrushShapes), typeof(Canvas), new PropertyMetadata(BrushShapes.Rectangle));
        public BrushShapes BrushShape
        {
            get => (BrushShapes)GetValue(BrushTypeProperty);
            set => SetValue(BrushTypeProperty, value);
        }

        public static readonly DependencyProperty BrushSizeProperty =
            DependencyProperty.Register(nameof(BrushSize), typeof(int),typeof(Canvas),new PropertyMetadata(30));
        public int BrushSize
        {
            get=>(int)GetValue(BrushSizeProperty);
            set=>SetValue(BrushSizeProperty,value);
        }

        public static readonly DependencyProperty BrushColorProperty =
            DependencyProperty.Register(nameof(BrushColor), typeof(BrushColor), typeof(Canvas), new PropertyMetadata(null));
        public BrushColor BrushColor
        {
            get => (BrushColor)GetValue(BrushColorProperty);
            set => SetValue(BrushColorProperty, value);
        }
        public Canvas()
        {
            InitializeComponent();
            cursorGenerator = new CursorGenerator(cursorGrid);
            drawer = new Drawer(drawingGrid);
        }

        private void UserControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (BrushColor == null)
                return;
            var pt = e.GetPosition(this);
            var isErase = BrushType == BrushTypes.Erase || e.RightButton == MouseButtonState.Pressed;

            var geometry = cursorGenerator.Create(isErase, BrushColor, BrushShape, BrushSize, pt);
            if (e.LeftButton == MouseButtonState.Pressed || e.RightButton == MouseButtonState.Pressed)
                drawer.Draw(isErase, BrushColor.ToColor(), geometry);
        }

        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
            if (BrushColor == null)
                return;

            var pt = e.GetPosition(this);
            cursorGenerator.Create(BrushType == BrushTypes.Erase, BrushColor, BrushShape, BrushSize, pt);
        }

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            cursorGenerator.Remove();
        }

        private void UserControl_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
