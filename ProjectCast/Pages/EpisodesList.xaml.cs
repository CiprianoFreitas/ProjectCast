using ProjectCast.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ProjectCast
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EpisodesList : Page
    {
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            // Register a handler for BackRequested events and set the
            // visibility of the Back button
            SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;

            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                rootFrame.CanGoBack ?
                AppViewBackButtonVisibility.Visible :
                AppViewBackButtonVisibility.Collapsed;

            IEnumerable<Episode> passedParameter = (IEnumerable<Episode>)e.Parameter;
            ItemListContainer.ItemsSource = passedParameter;
        }

        public EpisodesList()
        {
            this.InitializeComponent();
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
            //Border.Visibility = MySplitView.IsPaneOpen ? Windows.UI.Xaml.Visibility.Visible :
            //    Windows.UI.Xaml.Visibility.Collapsed;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame.CanGoBack)
            {
                e.Handled = true;
                rootFrame.GoBack();
            }
        }

        private void ItemListContainer_ItemClick(object sender, ItemClickEventArgs e)
        {
            Debug.WriteLine("Clicked Episode");
            var selectedEpisode = (Episode)e.ClickedItem;
            PodcastPlayer.Source = new Uri(selectedEpisode.EpisodeUrl);
            PodcastPlayer.Play();
        }
    }
}