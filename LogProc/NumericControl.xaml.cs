using System.Windows;
using System.Windows.Controls;

namespace LogProc {
	public partial class NumericUpDown : UserControl {

		public static readonly DependencyProperty ValueProperty =
	DependencyProperty.Register("Value", typeof(int), typeof(NumericUpDown), new FrameworkPropertyMetadata(0, (FrameworkPropertyMetadataOptions.BindsTwoWayByDefault), new PropertyChangedCallback(OnValuablePropertyChanged), new CoerceValueCallback(CoerceValue)));

		public static readonly DependencyProperty MaximumProperty =
   DependencyProperty.Register("Maximum", typeof(double), typeof(NumericUpDown), new FrameworkPropertyMetadata(100.0, (FrameworkPropertyMetadataOptions.BindsTwoWayByDefault), new PropertyChangedCallback(OnMaximumPropertyChanged), new CoerceValueCallback(CoerceMaximum)));

		public static readonly DependencyProperty MininumProperty =
   DependencyProperty.Register("Mininum", typeof(double), typeof(NumericUpDown), new FrameworkPropertyMetadata(0.0, (FrameworkPropertyMetadataOptions.BindsTwoWayByDefault), new PropertyChangedCallback(OnMininumPropertyChanged), new CoerceValueCallback(CoerceMininum)));

		public double Maximum {
			set {
				SetValue(MaximumProperty, value);
			}
			get {
				return (double)GetValue(MaximumProperty);
			}
		}
		public double Mininum {
			set {
				SetValue(MininumProperty, value);
			}
			get {
				return (double)GetValue(MininumProperty);
			}
		}
		public int Value {
			set {
				SetValue(ValueProperty, value);
				this.tbValue.Text = value.ToString();
			}
			get {
				return (int)GetValue(ValueProperty); ;
			}
		}
		public int TextFontSize {
			set {
				SetValue(FontSizeProperty, value);
			}
			get {
				return (int)GetValue(FontSizeProperty);
			}
		}

		public NumericUpDown() {
			InitializeComponent();
		}

		private void ValuePlus(object sender, RoutedEventArgs e) {
			if(Value < Maximum)
				Value++;
		}
		private void ValueMinus(object sender, RoutedEventArgs e) {
			if(Value > Mininum)
				Value--;
		}

		private void ValueChange(object sender, RoutedEventArgs e) {
			this.Value = (int)((System.Windows.Controls.Primitives.ScrollBar)sender).Value;
		}

		public void SetText(string text) {
			this.tbValue.Text = text;
		}

		private static void OnValuablePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
			NumericUpDown n = d as NumericUpDown;

			n.SetText(e.NewValue.ToString());
		}

		private static void OnMaximumPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
		}

		private static void OnMininumPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
		}

		private static object CoerceValue(DependencyObject d, object baseValue) {

			NumericUpDown n = d as NumericUpDown;

			int value = (int)baseValue;

			if(value >= n.Maximum)
				value = (int)n.Maximum;
			if(value <= n.Mininum)
				value = (int)n.Mininum;

			return value;
		}

		private static object CoerceMaximum(DependencyObject d, object baseValue) {
			NumericUpDown n = d as NumericUpDown;

			double value = (double)baseValue;

			return value;
		}

		private static object CoerceMininum(DependencyObject d, object baseValue) {
			NumericUpDown n = d as NumericUpDown;

			double value = (double)baseValue;

			return value;
		}
	}
}
