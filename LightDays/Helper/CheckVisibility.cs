using Days.Model;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Days.Helper
{
    public class CheckVisibility
    {
        public static void CheckButtonGridVisibility(int eventsNum, Grid ButtonGrid)
        {
            ButtonGrid.Visibility = (eventsNum != 0) ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
