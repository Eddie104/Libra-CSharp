using System;
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

        public static void Draw(Canvas canvas, List<LinePoint> points, Brush stroke, bool clear = true)
        {
            if (clear)
            {
                canvas.Children.Clear();
            }

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

        /// <summary>
        /// 绘制网格
        /// </summary>
        public static void DrawNet(int startRow, int startCol, int rows, int cols, double cellWidth, double cellHeight, int canvasWidth, int canvasHeight, Canvas canvas, bool isISO, out ICoordinateHelper coordinateHelper)
        {
            List<LinePoint> points = new List<LinePoint>();
            //Point topPoint;
            if (isISO)
            {
                double totalWidth = (rows + cols) * cellWidth / 2;
                double totalHeight = totalWidth / 2;
                coordinateHelper = new ISOHelper(cellWidth, cellHeight);
                coordinateHelper.TopPoint = new Point(canvasWidth / 2 - (totalWidth - cellWidth * rows) / 2,
                    (int)Math.Floor((canvasHeight - totalHeight) * 0.5));

                double endX = cols * cellWidth / 2;
                double endY = endX / 2;
                Point p;
                for (int row = 0; row <= rows; row++)
                {
                    p = new Point(coordinateHelper.TopPoint.X - cellWidth / 2 * row,
                            coordinateHelper.TopPoint.Y + cellHeight / 2 * row);
                    points.Add(new LinePoint()
                    {
                        StartPoint = p,
                        EndPoint = p + new Vector(endX, endY)
                    });
                }

                endX = rows * cellWidth / 2;
                endY = endX / 2;
                for (int col = 0; col <= cols; col++)
                {
                    p = new Point(coordinateHelper.TopPoint.X + cellWidth / 2 * col,
                            coordinateHelper.TopPoint.Y + cellHeight / 2 * col);
                    points.Add(new LinePoint()
                    {
                        StartPoint = p,
                        EndPoint = new Point(p.X - endX, p.Y + endY)
                    });
                }
            }
            else
            {
                double totalWidth = cellWidth * cols;
                double totalHeight = cellHeight * rows;
                double startX = 0; double startY = 0;
                startX = (canvasWidth - totalWidth) / 2;
                startY = (canvasHeight - totalHeight) / 2;
                coordinateHelper = new RectangularHelper() { Width = cellWidth, Height = cellHeight };
                coordinateHelper.TopPoint = new Point(startX, startY);
                
                Point index = coordinateHelper.GetItemIndex(new Point(startX, startY));
                index = coordinateHelper.GetItemPos((int)index.X, (int)index.Y);
                startX = (int)index.X; startY = (int)index.Y;
                for (int row = 0; row <= rows; row++)
                {
                    points.Add(new LinePoint()
                    {
                        StartPoint = new Point(startX, startY + row * cellHeight),
                        EndPoint = new Point(startX + cols * cellWidth, startY + row * cellHeight)
                    });
                }
                for (int col = 0; col <= cols; col++)
                {
                    points.Add(new LinePoint()
                    {
                        StartPoint = new Point(startX + col * cellWidth, startY),
                        EndPoint = new Point(startX + col * cellWidth, startY + rows * cellHeight)
                    });
                }
            }
            Draw(canvas, points, Brushes.Black);
        }

    }
}
