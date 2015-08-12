using ProjectCast.Helpers;
using ProjectCast.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.System;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ProjectCast
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private IPodcastService PodcastService = new ItunesService();

        public MainPage()
        {
            this.InitializeComponent();
            var applicationView = ApplicationView.GetForCurrentView();
            var titleBar = applicationView.TitleBar;

            Podcasts.Source = PodcastService.GetPodcasts("Planet");

            var mainBrand = Application.Current.Resources["MainBrandColor"]; ;

            titleBar.ButtonBackgroundColor = Colors.Black;
            titleBar.BackgroundColor = (Color)mainBrand;
            titleBar.ForegroundColor = Colors.White;
            titleBar.ButtonForegroundColor = Colors.White;
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
            //Border.Visibility = MySplitView.IsPaneOpen ? Windows.UI.Xaml.Visibility.Visible :
            //    Windows.UI.Xaml.Visibility.Collapsed;
        }

        private void SubmitSearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchTerm = SearchText.Text;
            Podcasts.Source = PodcastService.GetPodcasts(searchTerm);
            Bindings.Update();
        }

        private void SearchText_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key.Equals(VirtualKey.Enter))
            {
                string searchTerm = SearchText.Text;
                Podcasts.Source = PodcastService.GetPodcasts(searchTerm);
                Bindings.Update();
            }
        }

        private async void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var selectedPodcast = (Podcast)e.ClickedItem;
            IEnumerable<Episode> episodesList = await selectedPodcast.GetEpisodeList();

            Frame.Navigate(typeof(EpisodesList), episodesList);
        }
    }
}