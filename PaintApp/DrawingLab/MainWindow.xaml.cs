using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DrawingLab
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {

        /* Variables. */

        private enum Shapes { Rectangle, Circle, Line, Draw }
        private Shapes _shape;
        private bool _mouseDown;
        private Rectangle _rect;
        private Ellipse _circle;
        private Line _line;
        private Point _point;
        private Polyline _polyline;
        private SolidColorBrush _stroke = new SolidColorBrush(Colors.Red);
        private SolidColorBrush _fill = new SolidColorBrush(Colors.Red);

        public MainWindow()
        {
            InitializeComponent();

            ComboStroke.ItemsSource = typeof(Colors).GetProperties();
            ComboFill.ItemsSource = typeof(Colors).GetProperties();
        }

        /* Select stroke & fill color. */

        private void ComboStroke_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedColor = (Color)(ComboStroke.SelectedItem as PropertyInfo).GetValue(null, null);
            _stroke = new SolidColorBrush(selectedColor);
        }

        private void ComboFill_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedColor = (Color)(ComboFill.SelectedItem as PropertyInfo).GetValue(null, null);
            _fill = new SolidColorBrush(selectedColor);
        }

        private void ComboThickness_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        /* Select shape to draw. */

        private void DrawRectangle_Click(object sender, RoutedEventArgs e)
        {
            _shape = Shapes.Rectangle;
        }

        private void DrawCircle_Click(object sender, RoutedEventArgs e)
        {
            _shape = Shapes.Circle;
        }

        private void DrawLine_Click(object sender, RoutedEventArgs e)
        {
            _shape = Shapes.Line;
        }

        private void Draw_Click(object sender, RoutedEventArgs e)
        {
            _shape = Shapes.Draw;
        }

        /* Drawing events, change depending on selected shape. */

        private void myCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _mouseDown = (e.ButtonState == MouseButtonState.Pressed) && (e.ChangedButton == MouseButton.Left);

            if (!_mouseDown) return;
            switch (_shape)
            {
                case Shapes.Rectangle:
                {
                    _point = e.GetPosition(myCanvas);
                    _rect = new Rectangle
                    {
                        StrokeThickness = 6,
                        Fill = _fill,
                        Stroke = _stroke
                    };
                    myCanvas.Children.Add(_rect);
                        break;
                }

                case Shapes.Circle:
                {
                    _point = e.GetPosition(myCanvas);
                    _circle = new Ellipse
                    {
                        StrokeThickness = 6,
                        Fill = _fill,
                        Stroke = _stroke
                    };
                    myCanvas.Children.Add(_circle);
                        break;
                }

                case Shapes.Line:
                {
                    _point = e.GetPosition(myCanvas);
                    _line = new Line
                    {
                        X1 = _point.X,
                        Y1 = _point.Y,
                        StrokeThickness = 6,
                        Stroke = _stroke
                    };
                    myCanvas.Children.Add(_line);
                        break;
                }

                case Shapes.Draw:
                {
                    _polyline = new Polyline
                    {
                        StrokeDashCap = PenLineCap.Round,
                        StrokeLineJoin = PenLineJoin.Round,
                        StrokeThickness = 6,
                        Stroke = _stroke,
                    };
                    myCanvas.Children.Add(_polyline);
                        break;
                }
                default:
                    return;
            }
        }

        private void myCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_mouseDown) return;
            switch (_shape)
            {
                case Shapes.Rectangle:
                {
                    var pos = e.GetPosition(myCanvas);
                    _rect.SetValue(Canvas.LeftProperty, Math.Min(pos.X, _point.X));
                    _rect.SetValue(Canvas.TopProperty, Math.Min(pos.Y, _point.Y));
                    _rect.Width = Math.Abs(pos.X - _point.X);
                    _rect.Height = Math.Abs(pos.Y - _point.Y);
                        break;
                }

                case Shapes.Circle:
                {
                    var pos = e.GetPosition(myCanvas);
                    _circle.SetValue(Canvas.LeftProperty, Math.Min(pos.X, _point.X));
                    _circle.SetValue(Canvas.TopProperty, Math.Min(pos.Y, _point.Y));
                    _circle.Width = Math.Abs(pos.X - _point.X);
                    _circle.Height = Math.Abs(pos.X - _point.X);
                        break;
                }

                case Shapes.Line:
                {
                    var pos = e.GetPosition(myCanvas);
                    _line.X2 = Math.Abs(pos.X);
                    _line.Y2 = Math.Abs(pos.Y);
                        break;
                }

                case Shapes.Draw:
                {
                    if (!_mouseDown)
                    {
                        _polyline.Points.Clear();
                    }
                    _polyline.Points.Add(e.GetPosition(myCanvas));
                        break;
                }
                default:
                    return;
            }
        }

        private void myCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left) return;
            _mouseDown = false;
        }

        private void ClearCanvas(object sender, RoutedEventArgs e)
        {
            myCanvas.Children.Clear();
        }
    }
}
