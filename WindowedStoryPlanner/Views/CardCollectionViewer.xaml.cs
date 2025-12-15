using System;
using System.Collections;
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

namespace WindowedStoryPlanner.Views
{
    /// <summary>
    /// Interaction logic for CardCollectionViewer.xaml
    /// </summary>
    public partial class CardCollectionViewer : UserControl
    {
        public CardCollectionViewer()
        {
            InitializeComponent();
        }

        // --- The ItemsSource Dependency Property ---
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(
                nameof(ItemsSource),            // Property Name
                typeof(IEnumerable),            // Property Type (covers all lists)
                typeof(CardCollectionViewer),   // Owner Type
                new PropertyMetadata(null));    // Default Value

        public IEnumerable ItemsSource
        {
            get => (IEnumerable)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        // --- Dependency Properties for Injected Commands ---

        // 1. The "Global Delete" (Library View)
        public static readonly DependencyProperty DeleteItemCommandProperty =
            DependencyProperty.Register(nameof(DeleteItemCommand), typeof(ICommand), typeof(CardCollectionViewer));

        public ICommand DeleteItemCommand
        {
            get => (ICommand)GetValue(DeleteItemCommandProperty);
            set => SetValue(DeleteItemCommandProperty, value);
        }

        // 2. The "Unlink/Remove" (Plot Point View)
        public static readonly DependencyProperty UnlinkItemCommandProperty =
            DependencyProperty.Register(nameof(UnlinkItemCommand), typeof(ICommand), typeof(CardCollectionViewer));

        public ICommand UnlinkItemCommand
        {
            get => (ICommand)GetValue(UnlinkItemCommandProperty);
            set => SetValue(UnlinkItemCommandProperty, value);
        }

        // 3. The "View Payload" (Plot Point View)
        public static readonly DependencyProperty ViewPayloadCommandProperty =
            DependencyProperty.Register(nameof(ViewPayloadCommand), typeof(ICommand), typeof(CardCollectionViewer));

        public ICommand ViewPayloadCommand
        {
            get => (ICommand)GetValue(ViewPayloadCommandProperty);
            set => SetValue(ViewPayloadCommandProperty, value);
        }
    }
}
