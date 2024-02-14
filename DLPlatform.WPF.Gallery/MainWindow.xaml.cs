using PLPlatform.Core.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DLPlatform.WPF.Gallery
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        ObservableCollection<ClassInfo> classInfos;
        public ObservableCollection<ClassInfo> ClassInfos
        {
            get
            {
                return classInfos;
            }
            set
            {
                classInfos = value;
                RaisePropertyChanged(nameof(ClassInfos));
            }
        }

        Uri imageUri;
        public Uri ImageUri
        {
            get
            {
                return imageUri;
            }
            set
            {
                imageUri = value;
                RaisePropertyChanged(nameof(ImageUri));
            }
        }
        public MainWindowViewModel()
        {
            ClassInfos = new ObservableCollection<ClassInfo>()
            {
                new()
                {
                    Name = "Class 1",
                    Color = new()
                    {
                        R = 255,
                        G = 0,
                        B = 0
                    }
                },
                new()
                {
                    Name = "Class 2",
                    Color = new()
                    {
                        R = 0,
                        G = 255,
                        B = 0
                    }
                },
                new()
                {
                    Name = "Class 3",
                    Color = new()
                    {
                        R = 0,
                        G = 0,
                        B = 255
                    }
                }
            };
            ImageUri = new("C:\\Users\\DW-PC\\Desktop\\123.jpg");
        }
    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var vm = new MainWindowViewModel();
            this.DataContext = vm;
        }
    }
}