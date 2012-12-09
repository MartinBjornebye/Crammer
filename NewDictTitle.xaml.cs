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
    public sealed partial class NewDictTitle : Crammer.Common.LayoutAwarePage
    {
        public NewDictTitle()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
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

        /// <summary>
        /// Check for name clash with existing dictionary files
        /// </summary>
        private async Task<bool> similarDictionaryFileNameExists()
        {
            IReadOnlyList<StorageFile> filesInLocalFolder = await ApplicationData.Current.LocalFolder.GetFilesAsync();
            foreach (StorageFile file in filesInLocalFolder)
            {
                if ( !file.Path.Trim().EndsWith(CrammerDictionary.DICT_EXTENSION))
                    continue;

                string nameOfFile = Path.GetFileNameWithoutExtension(file.Path.Trim());
                if (nameOfFile == txtDictTitle.Text)
                    return (true);
            }
            return (false);
        }

        private async void Next_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtDictTitle.Text))
                    throw new Exception("Please provide a dictionary title");

                if (await similarDictionaryFileNameExists())
                {
                    var dlg = new Windows.UI.Popups.MessageDialog("A dictionary titled '" + txtDictTitle.Text + "' already exists.\nPlease choose a different name");
                    await dlg.ShowAsync();
                    return;
                }

                // Go to the next stage where you can opt to import from text files
                if (this.Frame != null)
                {
                    this.Frame.Navigate(typeof(TextFileImport), txtDictTitle.Text);
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
    }
}
