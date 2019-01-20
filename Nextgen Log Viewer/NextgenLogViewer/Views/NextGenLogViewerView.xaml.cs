using System;
using System.Windows;
using System.Windows.Forms;
using WinForms = System.Windows.Forms;

namespace NextgenLogViewer.Views
{
    /// <summary>
    /// Interaction logic for NextGenLogViewerView.xaml
    /// </summary>
    public partial class NextGenLogViewerView 
    {
        private string strFilePath = "";
        public NextGenLogViewerView()
        {
            InitializeComponent();
        }
     
        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new WinForms.OpenFileDialog { FileName = strFilePath };
            dialog.Filter = "Text file|*.txt";
            dialog.InitialDirectory = Environment.CurrentDirectory;
            dialog.Title = "Select Log File.";
            var result = dialog.ShowDialog();
            if (result == WinForms.DialogResult.OK)
            {
                txtFilePath.Text = dialog.FileName;
            }
        }

        private void txtSearch_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            TextBox textBoxName = (TextBox)sender;

        }

        private void btnTimeElapsedSearch_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
