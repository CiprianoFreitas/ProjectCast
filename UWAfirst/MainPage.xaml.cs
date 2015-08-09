using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using UWAfirst.Model;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UWAfirst
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            var applicationView = ApplicationView.GetForCurrentView();
            var titleBar = applicationView.TitleBar;

            Podcasts.Source = GetPodcasts("Planet");

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

        private static ObservableCollection<Podcast> GetPodcasts(string searchTerm)
        {
            ObservableCollection<Podcast> podcastList = new ObservableCollection<Podcast>();
            string sURL;
            sURL = string.Format("http://itunes.apple.com/search?term={0}&media=podcast", searchTerm);

            WebRequest wrGETURL;
            wrGETURL = WebRequest.Create(sURL);

            wrGETURL.ContentType = "application/json";
            Stream objStream;
            objStream = wrGETURL.GetResponseAsync().GetAwaiter().GetResult().GetResponseStream();

            StreamReader objReader = new StreamReader(objStream);

            string response = "";
            string line;
            while ((line = objReader.ReadLine()) != null)
            {
                response += line;
            }

            JToken token = JObject.Parse(response);

            int resultsCount = (int)token.SelectToken("resultCount");

            var results = (IEnumerable<dynamic>)token.SelectToken("results");

            foreach (var result in results)
            {
                podcastList.Add(new Podcast
                {
                    ImageURL = (string)result.SelectToken("artworkUrl600"),
                    FeedUrl = (string) result.SelectToken("feedUrl")
                });
            }

            return podcastList;
        }

        private void SubmitSearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchTerm = SearchText.Text;
            Podcasts.Source = GetPodcasts(searchTerm);
            Bindings.Update();
        }

        private void SearchText_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key.Equals(VirtualKey.Enter)){
                string searchTerm = SearchText.Text;
                Podcasts.Source = GetPodcasts(searchTerm);
                Bindings.Update();
            }
        }


        private void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var selectedPodcast = (Podcast)e.ClickedItem;
            var mp3Url = selectedPodcast.GetLatestEpisode();
            PodcastPlayer.Source = new Uri(mp3Url);
            PodcastPlayer.Play();
        }
    }
}
