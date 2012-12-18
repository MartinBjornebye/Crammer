using System;
using Windows.Storage;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    public sealed partial class ManageEntries : Crammer.Common.LayoutAwarePage
    {
        #region Attributes
        private DictionaryEntry mCurrentEntry = null;
        private IEnumerable<DictionaryEntry> mMatchingEntries = null;
        #endregion

        public ManageEntries()
        {
            this.InitializeComponent();
            setDictStats();
        }

        /// <summary>
        /// Trigger auto lookup based on the string being entered. Lookup starts to be triggered once entered string
        /// is greater than 1.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtA_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtA.Text) )
                {
                    save.IsEnabled = true;
                    cancel.IsEnabled = true;
                    if (txtA.Text.Length > 1)
                    {
                        waitRing.IsActive = true;
                        matchEntriesForInput();
                        //if (mMatchingEntries == null || mMatchingEntries.Count() == 0)
                        //{
                        //save.IsEnabled = true;
                        //cancel.IsEnabled = true;
                        //}
                        sub.IsEnabled = false;
                    }
                }
                else
                {
                    listEntries.DataContext = null;
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


        /// <summary>
        /// Searcher function which kicks in when a user starts to type in the Key field to find similar existing
        /// sub-string matches in the dictionary. This will allow a user to easily determine if a new entry is unique.
        /// </summary>
        private void matchEntriesForInput()
        {
            if ((Application.Current as App).CurrentDictionary == null)
                return;

            mMatchingEntries = (Application.Current as App).CurrentDictionary.Entries.Where((c) => c.AEntry.ToLower().Contains(txtA.Text.ToLower()));
            if (mMatchingEntries == null || mMatchingEntries.Count() == 0)
            {
                listEntries.DataContext = null;
                return;
            }

            listEntries.DataContext = mMatchingEntries;
            return;
        }

        private async void save_Click_1(object sender, RoutedEventArgs e)
        {
            // Do not allow a save if either of the entry values are empty
            try
            {
                if (string.IsNullOrEmpty(txtA.Text) ||
                     string.IsNullOrEmpty(txtB.Text))
                {
                    var dlg = new Windows.UI.Popups.MessageDialog("An entry must contain two valid values. Cannot save");
                    await dlg.ShowAsync();
                    return;
                }

                waitRing.IsActive = true;
                if (mCurrentEntry == null)
                {
                    DictionaryEntry entry = new DictionaryEntry();
                    entry.AEntry = txtA.Text;
                    entry.BEntry = txtB.Text;

                    if ((Application.Current as App).CurrentDictionary.entryExists(entry))
                    {
                        var dlg = new Windows.UI.Popups.MessageDialog("Entry already exists and will not be added");
                        await dlg.ShowAsync();
                        txtA.Text = "";
                        txtB.Text = "";
                        return;
                    }

                    (Application.Current as App).CurrentDictionary.addEntry(entry);
                    await (Application.Current as App).CurrentDictionary.save(true);
                }
                else
                {
                    mCurrentEntry.AEntry = txtA.Text;
                    mCurrentEntry.BEntry = txtB.Text;

                    await (Application.Current as App).CurrentDictionary.save(true);

                    mCurrentEntry = null;
                    save.IsEnabled = false;
                }
                clearAfterChange();
                enableButtons(false);
                setDictStats();
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

        /// <summary>
        /// Remove entry as selected in the grid
        /// </summary>
        async private void sub_Click_1(object sender, RoutedEventArgs e)
        {
            if (listEntries.SelectedIndex < 0 )
                return;

            DictionaryEntry entry = listEntries.SelectedItem as DictionaryEntry;
            if (entry == null)
                return;

            (Application.Current as App).CurrentDictionary.removeEntry(entry);
            await (Application.Current as App).CurrentDictionary.save(true);

            clearAfterChange();
            setDictStats();
            sub.IsEnabled = false;
        }

        private void cancel_Click_1(object sender, RoutedEventArgs e)
        {
            clearAfterChange();
        }



        private void showAll_Click_1(object sender, RoutedEventArgs e)
        {
            if (sub.IsEnabled == true)
                sub.IsEnabled = false;

            listEntries.DataContext = (Application.Current as App).CurrentDictionary.Entries;
        }

        private void asc_Click_1(object sender, RoutedEventArgs e)
        {
            if (sub.IsEnabled == true)
                sub.IsEnabled = false;

            listEntries.DataContext = from data in (Application.Current as App).CurrentDictionary.Entries
                                      orderby data.AEntry ascending
                                      select data;
        }

        private void desc_Click_1(object sender, RoutedEventArgs e)
        {
            if (sub.IsEnabled == true)
                sub.IsEnabled = false;

            listEntries.DataContext = from data in (Application.Current as App).CurrentDictionary.Entries
                                      orderby data.AEntry descending
                                      select data;
        }



        private void clearAfterChange()
        {
            txtA.Text = "";
            txtB.Text = "";

            enableButtons(false);
            listEntries.DataContext = null;
            mCurrentEntry = null;
            if ((Application.Current as App).PageVisited != CrammerConstants.EDITED_ENTRIES)
                (Application.Current as App).PageVisited = CrammerConstants.EDITED_ENTRIES;
        }

        private void enableButtons(bool enabled)
        {
            save.IsEnabled = enabled;
            sub.IsEnabled = enabled;
            cancel.IsEnabled = enabled;
        }

        private void setDictStats()
        {
            txtDictionaryTitle.Text = (Application.Current as App).CurrentDictionary.DictionaryTitle;
            txtTotal.Text = (Application.Current as App).CurrentDictionary.Entries.Count.ToString();
        }


        private void listEntries_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            try
            {
                if (listEntries.SelectedItem == null)
                    return;

                mCurrentEntry = listEntries.SelectedItem as DictionaryEntry;
                if (mCurrentEntry == null)
                    return;

                txtA.TextChanged -= this.txtA_TextChanged;
                txtA.Text = mCurrentEntry.AEntry;
                txtB.Text = mCurrentEntry.BEntry;
                txtA.TextChanged += this.txtA_TextChanged;

                enableButtons(true);

            }
            catch (Exception ex)
            {
                if (this.Frame != null)
                {
                    this.Frame.Navigate(typeof(MessagePopup), ex.Message);
                }
            }
        }

        private void listEntries_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (listEntries.SelectedIndex < 0)
                return;

            sub.IsEnabled = true;
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            this.GoHome(this, null);
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


    }
}
