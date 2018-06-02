using Days.Model;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Days.Helper
{
    public class CheckVisibility
    {
        public static void CheckButtonGridVisibility(ObservableCollection<Events> eventList, Grid ButtonGrid)
        {
            if (eventList != null && eventList.Count != 0)
            {
                ButtonGrid.Visibility = Visibility.Visible;
            }
            else
            {
                ButtonGrid.Visibility = Visibility.Collapsed;
            }
        }
    }
}
