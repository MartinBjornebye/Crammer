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
using System.Threading.Tasks;

using MB.Crammer;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace Crammer
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class MainPage : Crammer.Common.LayoutAwarePage
    {
        #region Attributes
        //private PickEngine mEntryEngine = null;
        //private SettingsFile mSettings = new SettingsFile();

        private string mCurrentWord = "";
        private bool mCheckModeOn = false;
        private string mInSystem = "";
        private string mDoneWords = "";

        private double mNormalFontSize = 32;
        #endregion

        #region Properties
         public CrammerDictionary CurrentDictionary
        {
            get 
            {
                return (Application.Current as App).CurrentDictionary;
            }
        }

         //public PickEngine PickerEngine
         //{
         //    get
         //    {
         //        return (Application.Current as App).EntryEngine;
         //    }
         //}

        #endregion

        public MainPage()
        {
            this.InitializeComponent();

            Application.Current.Suspending += new SuspendingEventHandler(appSuspending);
        }

        // Ensure state is saved when and if program is closed or suspended
        void appSuspending(Object sender,Windows.ApplicationModel.SuspendingEventArgs e)
        {
            if ((Application.Current as App).EntryEngine != null)
                (Application.Current as App).EntryEngine.close();
        }

        private async void pageRoot_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                // If already initialized, do not restore
                if ((Application.Current as App).ApplicationInitialized == false)
                {
                    (Application.Current as App).ApplicationInitialized = true;

                    // Load Crammer settings
                    await (Application.Current as App).Settings.init();

                    //mCurrentDictionary = (Application.Current as App).CurrentDictionary;
                    loadDictionaryAndPickFirstWord(false);
                }
            }
            catch (Exception ex)
            {
                (Application.Current as App).ErrorMessage = ex.Message;
                if (this.Frame != null)
                {
                    this.Frame.Navigate(typeof(MessagePopup), ex.Message);
                }
            }
        }

        private void pageRoot_Unloaded(object sender, RoutedEventArgs e)
        {
            (Application.Current as App).Settings.save();
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
        protected async override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            // Restore values stored in session state.
            if (pageState != null)
            {
                await (Application.Current as App).Settings.init();

                //(Application.Current as App).EntryEngine.init(false); Commented out Dec. 9
            }
        }


        async protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                if ((Application.Current as App).ApplicationInitialized == true)
                {
                    if (!string.IsNullOrEmpty((Application.Current as App).PageVisited))
                    {
                        await (Application.Current as App).Settings.init();
                        string optionChosen = (Application.Current as App).PageVisited;
                        (Application.Current as App).PageVisited = "";
                        if (string.IsNullOrEmpty(optionChosen) == false)
                        {
                            switch (optionChosen)
                            {
                                case "OpenedDictionary":
                                    loadDictionaryAndPickFirstWord(false);
                                    break;

                                case "NewDictionary":
                                    loadDictionaryAndPickFirstWord(true);
                                    break;

                                case "StartAgain":
                                    //cleanState();
                                    //advanceWord(true);
                                    break;

                                case "EditedEntries":
                                    await CurrentDictionary.save(true);
                                    cleanState();
                                    loadDictionaryAndPickFirstWord(true);
                                    break;
                            }
                        }
                    }
                    else
                    {
                        await (Application.Current as App).Settings.init();
                        loadDictionaryAndPickFirstWord(false);
                    }
                }

                base.OnNavigatedTo(e);

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
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
            (Application.Current as App).Settings.save();

            if ((Application.Current as App).EntryEngine != null)
                (Application.Current as App).EntryEngine.saveState();
        }

        private void cmdNo_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                noOption();
            }
            catch (Exception ex)
            {
                if (this.Frame != null)
                {
                    this.Frame.Navigate(typeof(MessagePopup), ex.Message);
                }
            }
        }

        private void cmdVerify_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                checkOption();
            }
            catch (Exception ex)
            {
                if (this.Frame != null)
                {
                    this.Frame.Navigate(typeof(MessagePopup), ex.Message);
                }
            }
        }

        private void cmdYes_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                yesOption();
            }
            catch (Exception ex)
            {
                if (this.Frame != null)
                {
                    this.Frame.Navigate(typeof(MessagePopup), ex.Message);
                }
            }
        }

        private void menuNewDict_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            if (this.Frame != null)
            {
                this.Frame.Navigate(typeof(NewDictTitle));
            }
        }

        private void openDictionary()
        {
            try
            {
                if (this.Frame != null)
                {
                    this.Frame.Navigate(typeof(OpenDictionary));
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

        private void menuOpenDict_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                openDictionary();
            }
            catch (Exception ex)
            {
                if (this.Frame != null)
                {
                    this.Frame.Navigate(typeof(MessagePopup), ex.Message);
                }
            }
        }

        private void menuEditDict_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            if (this.Frame != null)
            {
                this.Frame.Navigate(typeof(ManageEntries));
            }
        }

        private void noOption()
        {
            if (CurrentDictionary.Entries.Count == 0)
                return;

            if ((Application.Current as App).EntryEngine == null)
                return;

            (Application.Current as App).EntryEngine.answer(false);
            advanceWord(true);
        }

        /// <summary>
        /// User wants to check the entry
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkOption()
        {
            try
            {
                if (CurrentDictionary.Entries.Count == 0)
                    return;

                if ((Application.Current as App).EntryEngine == null)
                    return;

                if (!mCheckModeOn)
                {
                    mCurrentWord = (Application.Current as App).EntryEngine.getNative();
                    setCheck();
                }
                else
                {
                    mCurrentWord = (Application.Current as App).EntryEngine.getWord();
                    resetCheck();
                }

                txtOut.Text = mCurrentWord;
                showNewWords();

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
        /// User knows the entry
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void yesOption()
        {
            try
            {
                if (CurrentDictionary.Entries.Count == 0)
                    return;

                if ((Application.Current as App).EntryEngine == null)
                    return;

                if ((Application.Current as App).EntryEngine.done())
                {
                    cleanState();
                    var dlg = new Windows.UI.Popups.MessageDialog("Congratulations! You have completed the dictionary.");
                    await dlg.ShowAsync();

                    //if (this.Frame != null)
                    //{
                    //    this.Frame.Navigate(typeof(CompletedMessage));
                    //}
                }
                else
                {
                    (Application.Current as App).EntryEngine.answer(true);
                    advanceWord(true);
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


        public async void advanceWord(bool newSession)
        {
            try
            {

                if (CurrentDictionary.Empty == false)
                {
                    if ((Application.Current as App).EntryEngine.done())
                    {
                        cleanState();
                        var dlg = new Windows.UI.Popups.MessageDialog("Congratulations! You have completed the dictionary.");
                        await dlg.ShowAsync();
                    }
                    else
                    {
                        if (newSession)
                            (Application.Current as App).EntryEngine.execute();
                    }
                }
            }
            catch (FinishedException)
            {
                cleanState();

                //(Application.Current as App).EntryEngine.reset();

                //if (this.Frame != null)
                //{
                //    this.Frame.Navigate(typeof(CompletedMessage));
                //}

                //mEntryEngine.reset();
                //advanceWord(true);
                return;
            }
            catch (Exception ex)
            {
                txtOut.Text = "Error detected!\n" + ex.Message;
                (Application.Current as App).EntryEngine.reset();
                advanceWord(true);
                return;
            }

            resetCheck();
            setRightFont();

            mInSystem = (Application.Current as App).EntryEngine.getInSystem();
            mDoneWords = (Application.Current as App).EntryEngine.getDoneWords();
            mCurrentWord = (Application.Current as App).EntryEngine.getWord();
            txtTotal.Text = CurrentDictionary.Entries.Count.ToString();
            txtActive.Text = mInSystem;
            //labelInactive.Text = CurrentDictionary.InactiveEntries.Count.ToString();
            //labelCompleted.Text = mDoneWords;
            txtOut.Text = mCurrentWord;
            showNewWords();
        }

        private void showNewWords()
        {
            if ((Application.Current as App).EntryEngine.newWordsActive())
            {
                Grid gridOut = txtOut.Parent as Grid;
                if (gridOut != null)
                    gridOut.Background = new SolidColorBrush(Windows.UI.Colors.White);

                txtOut.Foreground = new SolidColorBrush(Windows.UI.Colors.Black);
                //Windows.UI.Color.FromArgb(FF#FF063C93
                txtOut.FontSize = mNormalFontSize + 4;
                txtOut.FontWeight = Windows.UI.Text.FontWeights.Bold;
            }
            else
            {
                Grid gridOut = txtOut.Parent as Grid;
                if (gridOut != null)
                    gridOut.Background = new SolidColorBrush(Windows.UI.Colors.LightGray);

                //txtOut.Foreground = new SolidColorBrush(Windows.UI.Colors.White);
                txtOut.Foreground = new SolidColorBrush(Windows.UI.Colors.Black);
                txtOut.FontSize = mNormalFontSize;
                txtOut.FontWeight = Windows.UI.Text.FontWeights.Normal;
                //txtOut.Foreground = new SolidColorBrush(Colors.White);
            }
        }

        private void setRightFont()
        {
            if (mCheckModeOn)
            {
                txtOut.FontStyle = Windows.UI.Text.FontStyle.Italic;
            }
            else
            {
                txtOut.FontStyle = Windows.UI.Text.FontStyle.Normal;
            }
        }

        void resetCheck()
        {
            if (mCheckModeOn)
            {
                mCheckModeOn = false;
                setRightFont();
            }
        }

        void setCheck()
        {
            if (!mCheckModeOn)
            {
                mCheckModeOn = true;
                setRightFont();
            }
        }

        private void loadDictionaryAndPickFirstWord(bool cleanState)
        {
            (Application.Current as App).Settings.save();
            CurrentDictionary.load((Application.Current as App).Settings.CurrentDictionary, true);
            txtDictionaryTitle.Text = CurrentDictionary.DictionaryTitle;

            if (CurrentDictionary.Empty)
                clearUIFields();

            setRightFont();
            (Application.Current as App).EntryEngine = new PickEngine(CurrentDictionary, cleanState);

            mCurrentWord = "";
            mDoneWords = "0";
            mInSystem = "0";

            if (!(Application.Current as App).EntryEngine.restored())
            {
                //(Application.Current as App).EntryEngine.reset();
                advanceWord(true);
            }
            else
            {
                advanceWord(false);
            }
        }


        private void cleanState()
        {
            if ((Application.Current as App).EntryEngine != null)
            {
                (Application.Current as App).EntryEngine.removeStateFile();
                (Application.Current as App).EntryEngine.reset();
            }

            loadDictionaryAndPickFirstWord(true);
        }

        private void clearUIFields()
        {
            mInSystem = "";
            mDoneWords = "";
            mCurrentWord = "";
            txtTotal.Text = "0";
            txtActive.Text = "0";
            //labelCompleted.Text = "0";
            txtOut.Text = "";
        }

        private async Task shuffleIt()
        {
            await CurrentDictionary.randomShuffle();
            loadDictionaryAndPickFirstWord(true);
        }

        async private void menuShuffle_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                await shuffleIt();
            }
            catch (Exception ex)
            {
                (Application.Current as App).ErrorMessage = ex.Message;
            }
        }

        private async Task sortAsc()
        {
            await CurrentDictionary.sortAscending();
            loadDictionaryAndPickFirstWord(true);
        }

        async private void menuSortAsc_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                await sortAsc();
            }
            catch (Exception ex)
            {
                (Application.Current as App).ErrorMessage = ex.Message;
            }
        }

        private async Task sortDesc()
        {
            await CurrentDictionary.sortDescending();
            loadDictionaryAndPickFirstWord(true);
        }

        async private void menuSortDesc_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                await sortDesc();
            }
            catch (Exception ex)
            {
                (Application.Current as App).ErrorMessage = ex.Message;
            }
        }

        private async Task sortTimeAsc()
        {
            await CurrentDictionary.sortByTimestampAscending();
            loadDictionaryAndPickFirstWord(true);
        }

        async private void menuTimeAscending_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                await sortTimeAsc();
            }
            catch (Exception ex)
            {
                (Application.Current as App).ErrorMessage = ex.Message;
            }
        }

        private async Task sortTimeDesc()
        {
            await CurrentDictionary.sortByTimestampDescending();
            loadDictionaryAndPickFirstWord(true);
        }

        async private void menuTimeDescending_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                await sortTimeDesc();
            }
            catch (Exception ex)
            {
                (Application.Current as App).ErrorMessage = ex.Message;
            }
        }


        private void menuFlipSequence_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                (Application.Current as App).EntryEngine.toggleSequence();
                advanceWord(false);
            }
            catch (Exception ex)
            {
                (Application.Current as App).ErrorMessage = ex.Message;
            }
        }

        private void menuStartOver_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                cleanState();
            }
            catch (Exception ex)
            {
                (Application.Current as App).ErrorMessage = ex.Message;
            }
        }


    }
}
