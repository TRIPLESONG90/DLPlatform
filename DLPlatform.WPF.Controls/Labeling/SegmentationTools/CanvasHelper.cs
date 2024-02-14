using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Path = System.Windows.Shapes.Path;

namespace DLPlatform.WPF.Controls.Labeling.SegmentationTools
{


    internal class Drawer
    {
        Grid _drawingGrid;
        public Drawer(Grid drawingGrid)
        {
            _drawingGrid = drawingGrid;
        }

        internal void Draw(bool isErase, Color color, Geometry geometry)
        {
            var strColor = color.ToString();
            var tmp = _drawingGrid.Children.Cast<Path>()
                                             .Where(x => x.Tag.ToString() == strColor)
                                             .FirstOrDefault();
            if(tmp == null)
            {
                _drawingGrid.Children.Add(new Path()
                {
                    StrokeThickness = 1,
                    IsHitTestVisible = false,
                    Stroke = new SolidColorBrush(Color.FromArgb(255, color.R, color.G, color.B)),
                    Fill = new SolidColorBrush(Color.FromArgb(128, color.R, color.G, color.B)),
                    Tag = color.ToString(),
                    Data = Geometry.Parse(""),
                });
            }
            var layer = _drawingGrid.Children.Cast<Path>()
                                 .Where(x => x.Tag.ToString() == strColor)
                                 .FirstOrDefault();
            if (isErase)
            {
                layer.Data = Geometry.Combine(layer.Data, geometry, GeometryCombineMode.Exclude, null);
            }
            else
            {
                layer.Data = Geometry.Combine(layer.Data, geometry, GeometryCombineMode.Union, null);
            }

        }
    }
    internal class CursorGenerator
    {
        Grid _cursorGrid;
        public CursorGenerator(Grid cursorGrid)
        {
            _cursorGrid = cursorGrid;
        }
        private Geometry GetGeometry(BrushShapes shape, Point pt, int size)
        {
            double halfsize = size / 2.0;
            switch (shape)
            {
                case BrushShapes.Ellipse:
                    return new EllipseGeometry()
                    {
                        Center = new Point(pt.X, pt.Y),
                        RadiusX = halfsize,
                        RadiusY = halfsize,
                    };
                case BrushShapes.Rectangle:
                    return new RectangleGeometry(new Rect(pt.X - halfsize, pt.Y - halfsize, size, size));
                default:
                    return null;
            }
        }
        private bool CursorExist()
        {
            return _cursorGrid.Children.Count > 0 ? true : false;
        }

        private void CreateCursor(Geometry cursorGeometry, SolidColorBrush color)
        {
            _cursorGrid.Children.Add(new Path()
            {
                StrokeThickness = 1,
                IsHitTestVisible = false,
                Stroke = new SolidColorBrush(Color.FromArgb(128, color.Color.R, color.Color.G, color.Color.B)),
                Tag = color.ToString(),
                Data = cursorGeometry
            });
        }

        private void UpdateCursor(Geometry cursorGeometry, SolidColorBrush color)
        {
            (_cursorGrid.Children[0] as Path).Data = cursorGeometry;
            (_cursorGrid.Children[0] as Path).Stroke = color;
            (_cursorGrid.Children[0] as Path).Fill = new SolidColorBrush(Color.FromArgb(128, color.Color.R, color.Color.G, color.Color.B));
        }

        public Geometry Create(bool isErase, BrushColor brushColor, BrushShapes brushShape, int brushSize, Point pt)
        {
            var color = Color.FromRgb(brushColor.R, brushColor.G, brushColor.B);
            //var shape = brushShape;
            //var size = brushSize; 

            var cursorGeomtry = GetGeometry(brushShape, pt, brushSize);

            SolidColorBrush brushSColor = !isErase ? new(color) : new(Colors.Gray);
            if (!CursorExist())
                CreateCursor(cursorGeomtry, brushSColor);
            else
                UpdateCursor(cursorGeomtry, brushSColor);

            _cursorGrid.Cursor = Cursors.None;

            return cursorGeomtry;
        }

        public void Remove()
        {
            _cursorGrid.Children.Clear();
        }
    }
}
