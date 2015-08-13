using ProjectCast.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.Media;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ProjectCast
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EpisodesList : Page
    {
        private SystemMediaTransportControls systemPlayerControls;

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

            InitializeSystemMediaControls();

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

        #region System Media Controls

        private void InitializeSystemMediaControls()
        {
            systemPlayerControls = SystemMediaTransportControls.GetForCurrentView();

            systemPlayerControls.ButtonPressed += SystemControls_ButtonPressed;

            systemPlayerControls.IsPlayEnabled = true;
            systemPlayerControls.IsPauseEnabled = true;
        }

        void SystemControls_ButtonPressed(SystemMediaTransportControls sender,
    SystemMediaTransportControlsButtonPressedEventArgs args)
        {
            switch (args.Button)
            {
                case SystemMediaTransportControlsButton.Play:
                    PlayMedia();
                    break;
                case SystemMediaTransportControlsButton.Pause:
                    PauseMedia();
                    break;
                default:
                    break;
            }
        }

        async void PlayMedia()
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                PodcastPlayer.Play();
            });
        }

        async void PauseMedia()
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                PodcastPlayer.Pause();
            });
        }

        void PodcastPlayer_CurrentStateChanged(object sender, RoutedEventArgs e)
        {
            switch (PodcastPlayer.CurrentState)
            {
                case MediaElementState.Playing:
                    systemPlayerControls.PlaybackStatus = MediaPlaybackStatus.Playing;
                    break;
                case MediaElementState.Paused:
                    systemPlayerControls.PlaybackStatus = MediaPlaybackStatus.Paused;
                    break;
                case MediaElementState.Stopped:
                    systemPlayerControls.PlaybackStatus = MediaPlaybackStatus.Stopped;
                    break;
                case MediaElementState.Closed:
                    systemPlayerControls.PlaybackStatus = MediaPlaybackStatus.Closed;
                    break;
                default:
                    break;
            }
        }

        private void PodcastPlayer_MediaOpened(object sender, RoutedEventArgs e)
        {
            UpdatePodcastInfoInControls();
        }

        void UpdatePodcastInfoInControls()
        {
            // Get the updater.
            SystemMediaTransportControlsDisplayUpdater updater = systemPlayerControls.DisplayUpdater;

            // Music metadata.
            //updater.MusicProperties.Title = "song title";

            //// Set the album art thumbnail.
            //// RandomAccessStreamReference is defined in Windows.Storage.Streams
            //updater.Thumbnail =
            //   RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Music/music1_AlbumArt.jpg"));

            // Update the system media transport controls.
            updater.Update();
        } 
        #endregion
    }
}