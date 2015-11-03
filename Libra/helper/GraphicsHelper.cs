using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Libra.helper
{
    public struct LinePoint
    {
        public Point StartPoint { get; set; }
        public Point EndPoint { get; set; }
    }

    public class GraphicsHelper
    {

        public static void Draw(Canvas canvas, List<LinePoint> points, Brush stroke)
        {
            canvas.Children.Clear();

            PathFigureCollection myPathFigureCollection = new PathFigureCollection();
            PathGeometry myPathGeometry = new PathGeometry();

            foreach (LinePoint p in points)
            {
                PathFigure myPathFigure = new PathFigure();
                myPathFigure.StartPoint = p.StartPoint;

                LineSegment myLineSegment = new LineSegment();
                myLineSegment.Point = p.EndPoint;

                PathSegmentCollection myPathSegmentCollection = new PathSegmentCollection();
                myPathSegmentCollection.Add(myLineSegment);

                myPathFigure.Segments = myPathSegmentCollection;

                myPathFigureCollection.Add(myPathFigure);
            }

            myPathGeometry.Figures = myPathFigureCollection;
            Path myPath = new Path();
            myPath.Stroke = stroke == null ? Brushes.Black : stroke;
            myPath.StrokeThickness = 1;
            myPath.Data = myPathGeometry;

            canvas.Children.Add(myPath);
        }

    }
}
