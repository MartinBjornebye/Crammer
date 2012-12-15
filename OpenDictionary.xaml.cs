using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Storage;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using MB.Crammer;
// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace Crammer
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class OpenDictionary : Crammer.Common.LayoutAwarePage
    {

        public OpenDictionary()
        {
            this.InitializeComponent();
        }

        async protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //string message = e.Parameter as string;
            IReadOnlyList<StorageFile> files = await ApplicationData.Current.LocalFolder.GetFilesAsync();
            if (files == null || files.Count == 0)
            {
                var dlg = new Windows.UI.Popups.MessageDialog("No previously saved dictionaries available");
                await dlg.ShowAsync();
            }

            foreach (StorageFile file in files)
            {
                if (file.Path.EndsWith(MB.Crammer.CrammerDictionary.DICT_EXTENSION))
                {
                    listDictionaries.Items.Add(Path.GetFileName(file.Path));
                }
            }

            if (listDictionaries.Items.Count == 0)
                OpenSelected.IsEnabled = false;
        }


        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
        }

        async private void OpenSelected_Click_1(object sender, RoutedEventArgs e)
        {
            string dictionary = listDictionaries.SelectedItem as string;
            if (string.IsNullOrEmpty(dictionary))
            {
                var dlg = new Windows.UI.Popups.MessageDialog("Please select one of the existing dictionaries");
                await dlg.ShowAsync();
            }

            CrammerDictionary crammerDict = (Application.Current as App).CurrentDictionary;
            crammerDict.DictionaryFile = Path.Combine(ApplicationData.Current.LocalFolder.Path, dictionary);
            (Application.Current as App).Settings.CurrentDictionary = Path.Combine(ApplicationData.Current.LocalFolder.Path, dictionary);
            (Application.Current as App).Settings.save();

            this.GoHome(this, null);
        }

        private async void OpenExternal_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Windows.UI.ViewManagement.ApplicationView.Value != Windows.UI.ViewManagement.ApplicationViewState.Snapped ||
                     Windows.UI.ViewManagement.ApplicationView.TryUnsnap() == true)
                {
                    Windows.Storage.Pickers.FileOpenPicker openPicker = new Windows.Storage.Pickers.FileOpenPicker();
                    openPicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
                    //openPicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.
                    openPicker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;

                    // Filter to include a sample subset of file types.
                    openPicker.FileTypeFilter.Clear();
                    openPicker.FileTypeFilter.Add(CrammerDictionary.DICT_EXTENSION);

                    // Open the file picker.
                    Windows.Storage.StorageFile file = await openPicker.PickSingleFileAsync();

                    waitRing.IsActive = true;

                    // file is null if user cancels the file picker.
                    if (file == null)
                        return;

                    StorageFile dictFile = await file.CopyAsync(ApplicationData.Current.LocalFolder, Path.GetFileName(file.Path), NameCollisionOption.ReplaceExisting);

                    CrammerDictionary crammerDict = (Application.Current as App).CurrentDictionary;
                    crammerDict.DictionaryFile = dictFile.Path;
                    (Application.Current as App).Settings.CurrentDictionary = dictFile.Path;
                    (Application.Current as App).Settings.save();

                    this.GoHome(this, null);
                }
            }
            catch (Exception ex)
            {
                waitRing.IsActive = false;
                if (this.Frame != null)
                {
                    this.Frame.Navigate(typeof(MessagePopup), ex.Message);
                }
            }
            finally
            {
                waitRing.IsActive = false;
            }
        }

        private void listDictionaries_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            OpenSelected_Click_1(sender, e);
        }

        private async void Remove_Click_1(object sender, RoutedEventArgs e)
        {
            if ( listDictionaries.SelectedItem == null)
                return;

            string dictionaryFile = listDictionaries.SelectedItem as string;
            if (dictionaryFile == null)
                return;

            if (ApplicationData.Current.LocalSettings.Values.ContainsKey(dictionaryFile))
                ApplicationData.Current.LocalSettings.Values.Remove(dictionaryFile);

            string dictPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, dictionaryFile);
            try
            {
                StorageFile dictFile = await ApplicationData.Current.LocalFolder.GetFileAsync(dictionaryFile);
                await dictFile.DeleteAsync();
            }
            catch (FileNotFoundException)
            {
            }

            listDictionaries.Items.Remove(dictionaryFile);
            Remove.IsEnabled = false;
        }

        private void listDictionaries_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (listDictionaries.SelectedItem == null)
                return;

            Remove.IsEnabled = true;
        }
    }
}
