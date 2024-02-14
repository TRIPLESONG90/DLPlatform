using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DLPlatform.WPF.Controls.Labeling
{
    public record class BrushColor
    {
        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }

        public Color ToColor() => Color.FromRgb(R, G, B);
    }
}
