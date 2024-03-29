﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Reflection;
using Windows.UI.ApplicationSettings;
using System.Threading.Tasks;

using MB.Crammer;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234227

namespace Crammer
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        #region Constants
        private const string CRAMMER_PRIVACY_POLICY = "http://home.online.no/~martinb/Policy/index.htm";
        #endregion

        #region Attributes
        public MainPage rootPage = null;
        private SettingsFile mSettings = new SettingsFile();
        private bool mApplicationInitialized = false;
        private CrammerDictionary mCurrentDictionary = new CrammerDictionary();
        private PickEngine mEntryEngine = null;
        private string mErrorMessage = "";
        private string mPageVisited = "";
        private Assembly mExecutingAssembly = typeof(App).GetTypeInfo().Assembly;
        #endregion

        #region Properties
        public CrammerDictionary CurrentDictionary
        {
            get { return mCurrentDictionary; }
            set { mCurrentDictionary = value; }
        }

        public PickEngine EntryEngine
        {
            get { return mEntryEngine; }
            set { mEntryEngine = value; }
        }

        public bool ApplicationInitialized
        {
            get { return mApplicationInitialized; }
            set { mApplicationInitialized = value; }
        }

        public string ErrorMessage
        {
            get { return mErrorMessage; }
            set { mErrorMessage = value; }
        }

        public Assembly ExecutingAssembly
        {
            get { return mExecutingAssembly; }
            set { mExecutingAssembly = value; }
        }
        public string PageVisited
        {
            get { return mPageVisited; }
            set { mPageVisited = value; }
        }
        public SettingsFile Settings
        {
            get { return mSettings; }
            set { mSettings = value; }
        }
        #endregion

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                Crammer.Common.SuspensionManager.RegisterFrame(rootFrame, "appFrame");

                if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                if (!rootFrame.Navigate(typeof(MainPage), args.Arguments))
                {
                    throw new Exception("Failed to create initial page");
                }
            }
            // Ensure the current window is active
            Window.Current.Activate();
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();

            if (mEntryEngine != null)
                mEntryEngine.saveState();

            deferral.Complete();

            await Crammer.Common.SuspensionManager.SaveAsync();
        }


        protected override void OnWindowCreated(WindowCreatedEventArgs args)
        {
            // Place your CommandsRequested handler here to ensure your settings are available at all times in your app
            SettingsPane.GetForCurrentView().CommandsRequested += onCommandsRequested;
        }


        private void onCommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            args.Request.ApplicationCommands.Add(
                    new SettingsCommand("privpolicy", "Crammer Privacy Policy",
                                        async a =>
                    {
                        await Windows.System.Launcher.LaunchUriAsync(new Uri(CRAMMER_PRIVACY_POLICY));
                    }));
        }
    }
}
