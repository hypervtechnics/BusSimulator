using System.Windows;
using System.Windows.Controls;

namespace BusSimulator.Ui.Components
{
    public partial class RunningTextComponent : UserControl
    {
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof(Text), typeof(string), typeof(RunningTextComponent), new PropertyMetadata(string.Empty));

        public RunningTextComponent()
        {
            this.InitializeComponent();
        }

        public string Text
        {
            get
            {
                return (string) this.GetValue(TextProperty);
            }
            set
            {
                this.SetValue(TextProperty, value);
            }
        }
    }
}
