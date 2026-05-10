using System;
using System.Collections.Generic;
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
using WindowedStoryPlanner.ViewModels;

namespace WindowedStoryPlanner.Views
{
    /// <summary>
    /// Interaction logic for SubjectLibrary.xaml
    /// </summary>
    public partial class SubjectLibrary : UserControl
    {
        public SubjectLibrary()
        {
            InitializeComponent();
        }

        private void SubjectCard_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListBoxItem { DataContext: SubjectViewModel subject }
                && DataContext is SubjectLibraryViewModel vm)
            {
                vm.OpenSubjectCommand.Execute(subject);
            }
        }
    }
}
