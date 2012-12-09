using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;

using MB.Crammer;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace Crammer
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class TextFileImport : Crammer.Common.LayoutAwarePage
    {
        #region Attributes
        private List<DictionaryEntry> mDictEntries = new List<DictionaryEntry>();
        private string mDictionaryTitle = "";
        private StorageFile mCopiedImportFile = null;
        #endregion

        public TextFileImport()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            mDictionaryTitle = e.Parameter as string;
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

        private void chkHaveImportFile_Click_1(object sender, RoutedEventArgs e)
        {
            if (chkHaveImportFile.IsChecked == true)
            {
                cmdBrowseForInputFile.IsEnabled = true;
                frameImport.IsEnabled = true;
            }
            else
            {
                cmdBrowseForInputFile.IsEnabled = false;
                frameImport.IsEnabled = false;
            }
        }

        private async void cmdNext_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                if (mCopiedImportFile != null)
                {
                    await importFile();
                    await mCopiedImportFile.DeleteAsync();
                }

                (Application.Current as App).CurrentDictionary = new CrammerDictionary();
                (Application.Current as App).CurrentDictionary.DictionaryTitle = mDictionaryTitle;
                (Application.Current as App).CurrentDictionary.Entries = mDictEntries;

                string newDictPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, mDictionaryTitle + CrammerDictionary.DICT_EXTENSION);

                await (Application.Current as App).CurrentDictionary.create(mDictionaryTitle + CrammerDictionary.DICT_EXTENSION);
                ApplicationData.Current.LocalSettings.Values[SettingsFile.CURRENT_DICT_NODE] = newDictPath;
                (Application.Current as App).PageVisited = "NewDictionary";

                this.GoHome(this, null);

            }
            catch (Exception ex)
            {
                if (this.Frame != null)
                {
                    this.Frame.Navigate(typeof(MessagePopup), ex.Message);
                }
            }
        }

        /// <summary>
        /// Imports an external text file with delimited entries to create a new dictionary.
        /// </summary>
        private async Task<bool> importFile()
        {
            mDictEntries.Clear();

            string[] delimiter = null;
            if (rbCSV.IsChecked == true)
                delimiter = new string[] { ";" };
            else
            {
                if (string.IsNullOrEmpty(txtCustomSeparator.Text))
                {
                    //var dlg = new Windows.UI.Popups.MessageDialog("You must specify a delimiter string");
                    //await dlg.ShowAsync();
                    //return (false);
                    throw new Exception("You must specify a delimiter string");
                }

                delimiter = new string[] { txtCustomSeparator.Text };
            }

            IList<string> fileLines = await FileIO.ReadLinesAsync(mCopiedImportFile);
            if (fileLines.Count == 0)
            {
                var dlg = new Windows.UI.Popups.MessageDialog("File " + mCopiedImportFile.Path + " does not contain any valid lines");
                await dlg.ShowAsync();
                return (false);
            }

            // Use current date ticks and increment by one to distinguish entries which may otherwise 
            // get an exact same tick value depending on the speed of the processor.
            long currentTimeStampTicks = DateTime.Now.Ticks;
            foreach (string line in fileLines)
            {
                string entry = line.Trim();
                if (string.IsNullOrEmpty(entry))
                    continue;

                string[] elems = entry.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
                if (elems.Length < 2)
                    continue;

                DictionaryEntry dictEntry = new DictionaryEntry();

                if (string.IsNullOrEmpty(elems[0]) == false)
                    dictEntry.AEntry = elems[0].Trim();
                else
                    dictEntry.AEntry = elems[0];

                if (string.IsNullOrEmpty(elems[1]) == false)
                    dictEntry.BEntry = elems[1].Trim();
                else
                    dictEntry.BEntry = elems[1];

                dictEntry.Stamp = new DateTime(currentTimeStampTicks++);

                mDictEntries.Add(dictEntry);
            }

            if (mDictEntries.Count == 0)
            {
                var dlg = new Windows.UI.Popups.MessageDialog("File " + Path.GetFileName(mCopiedImportFile.Path) + " did not yield any valid entries");
                await dlg.ShowAsync();
                return (false);
            }

            var successDlg = new Windows.UI.Popups.MessageDialog("Successfully imported " + mDictEntries.Count + " entries from:\r\n" + Path.GetFileName(mCopiedImportFile.Path));
            await successDlg.ShowAsync();
            return (true);
        }

        private async void openDictionary()
        {
            try
            {
                if (Windows.UI.ViewManagement.ApplicationView.Value != Windows.UI.ViewManagement.ApplicationViewState.Snapped ||
                    Windows.UI.ViewManagement.ApplicationView.TryUnsnap() == true)
                {
                    Windows.Storage.Pickers.FileOpenPicker openPicker = new Windows.Storage.Pickers.FileOpenPicker();
                    openPicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
                    openPicker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;

                    // Filter to include a sample subset of file types.
                    openPicker.FileTypeFilter.Clear();
                    openPicker.FileTypeFilter.Add(".txt");

                    // Open the file picker.
                    Windows.Storage.StorageFile file = await openPicker.PickSingleFileAsync();

                    // file is null if user cancels the file picker.
                    if (file == null)
                        return;

                    mCopiedImportFile = await file.CopyAsync(ApplicationData.Current.LocalFolder, Path.GetFileName(file.Path), NameCollisionOption.ReplaceExisting);
                    txtImportFile.Text = Path.GetFileName(file.Path);

                }
            }
            catch (Exception ex)
            {
                if (this.Frame != null)
                {
                    this.Frame.Navigate(typeof(MessagePopup), ex.Message);
                }
            }
        }


        private void cmdBrowseForInputFile_Click_1(object sender, RoutedEventArgs e)
        {
            openDictionary();
        }



    }
}
