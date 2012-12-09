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
        public ManageEntries()
        {
            this.InitializeComponent();
            setDictStats();
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

        private void txtA_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtA.Text))
            {
                matchEntriesForInput();
            }
            else
            {
                listEntries.DataContext = null;
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

            IEnumerable<DictionaryEntry> entries = (Application.Current as App).CurrentDictionary.Entries.Where((c) => c.AEntry.ToLower().Contains(txtA.Text.ToLower()));
            if (entries == null || entries.Count() == 0)
            {
                listEntries.DataContext = null;
                return;
            }

            listEntries.DataContext = entries;
        }

        /// <summary>
        /// Add new entry
        /// </summary>
        private void add_Click_1(object sender, RoutedEventArgs e)
        {
            DictionaryEntry entry = new DictionaryEntry();
            entry.AEntry = txtA.Text;
            entry.BEntry = txtB.Text;
            (Application.Current as App).CurrentDictionary.addEntry(entry);
            clearAfterChange();
        }

        /// <summary>
        /// Remove entry as selected in the grid
        /// </summary>
        private void sub_Click_1(object sender, RoutedEventArgs e)
        {
            if (listEntries.SelectedIndex < 0 )
                return;

            DictionaryEntry entry = listEntries.SelectedItem as DictionaryEntry;
            if (entry == null)
                return;

            (Application.Current as App).CurrentDictionary.removeEntry(entry);

            clearAfterChange();
        }

        private void clearAfterChange()
        {
            txtA.Text = "";
            txtB.Text = "";
            listEntries.DataContext = null;
            if ((Application.Current as App).PageVisited != CrammerConstants.EDITED_ENTRIES)
                (Application.Current as App).PageVisited = CrammerConstants.EDITED_ENTRIES;

            setDictStats();
        }

        private void setDictStats()
        {
            txtDictionaryTitle.Text = (Application.Current as App).CurrentDictionary.DictionaryTitle;
            txtTotal.Text = (Application.Current as App).CurrentDictionary.Entries.Count.ToString();
        }
    }
}
