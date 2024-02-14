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

namespace DLPlatform.WPF.Controls.Labeling
{
    /// <summary>
    /// Segmentation.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Segmentation : UserControl
    {
        public static readonly DependencyProperty BrushColorsProperty = DependencyProperty.Register(
                       "BrushColors", typeof(IEnumerable<BrushColor>), typeof(Segmentation), new PropertyMetadata(null));
        public IEnumerable<BrushColor> BrushColors
        {
            get=>(IEnumerable<BrushColor>)GetValue(BrushColorsProperty);
            set=>SetValue(BrushColorsProperty,value);
        }

        public static readonly DependencyProperty BrushColorProperty = DependencyProperty.Register(
               "BrushColor", typeof(BrushColor), typeof(Segmentation), new PropertyMetadata(null));
        public BrushColor BrushColor
        {
            get => (BrushColor)GetValue(BrushColorProperty);
            set => SetValue(BrushColorsProperty, value);
        }

        public static readonly DependencyProperty BrushSizeProperty =
    DependencyProperty.Register(nameof(BrushSize), typeof(int), typeof(Segmentation), new PropertyMetadata(30));
        public int BrushSize
        {
            get => (int)GetValue(BrushSizeProperty);
            set => SetValue(BrushSizeProperty, value);
        }

        public static readonly DependencyProperty ImageUriProperty =
            DependencyProperty.Register(
                "ImageUri",
                typeof(Uri),
                typeof(Segmentation),
                new PropertyMetadata(null));
        public Uri ImageUri
        {
            get { return (Uri)GetValue(ImageUriProperty); }
            set { SetValue(ImageUriProperty, value); }
        }
        public Segmentation()
        {
            InitializeComponent();
        }

    }
}
