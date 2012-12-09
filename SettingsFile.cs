// Write settings
// ApplicationData.Current.LocalSettings.Values["test"] = "Setting Value"; 
 
// Read test setting
// string t = (string)ApplicationData.Current.LocalSettings.Values["test"]; 


using System;
using System.IO;
using Windows.Storage;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace MB.Crammer
{
    public class SettingsFile
    {
        #region Constants
        private const string DEFAULT_DICTIONARY = "Worlds Capitals.crammerdict";
        private const string APP_SETTINGS_FILE = "AppSettings.xml";

        private const string ROOT_NODE = "Root";
        public const string CURRENT_DICT_NODE = "CurrentDictionary";
        public const string CHAMBER_NODE_NODE = "LargeChambers";
        #endregion

        #region Attributes
        public bool LargeChambers = true;
        public string CurrentDictionary = ""; // First valid dictionary if nothing else specified
        #endregion

        async public Task init()
        {
            //var dlg = new Windows.UI.Popups.MessageDialog("Inside SettingsFile.init");
            //await dlg.ShowAsync();

            if (ApplicationData.Current.LocalSettings.Values.ContainsKey(CURRENT_DICT_NODE))
            {
                CurrentDictionary = (string)ApplicationData.Current.LocalSettings.Values[CURRENT_DICT_NODE];

                // A check to verify that the dictionary file physically exists
                StorageFile dictFile = null;
                try
                {
                    dictFile = await ApplicationData.Current.LocalFolder.GetFileAsync(Path.GetFileName(CurrentDictionary));

                }
                catch (System.IO.FileNotFoundException)
                {
                    dictFile = null;
                }

                // In case for some reason the dictionary file is not available or accessible, start from scratch with the country dict.
                if (dictFile == null)
                {
                    await saveInitial();
                }
                else
                {
                    LargeChambers = true;
                    if (ApplicationData.Current.LocalSettings.Values.ContainsKey(CHAMBER_NODE_NODE))
                        LargeChambers = bool.Parse((string)ApplicationData.Current.LocalSettings.Values[CHAMBER_NODE_NODE]);

                    WordChamber.LargeChambers = LargeChambers; // Also make this value available in the WordChamber class
                }
            }
            else
            {
                await saveInitial();
            }

        }

        async private Task saveInitial()
        {
            // This must be the initial launch, so use the default dictionary and create a settings file
            //Windows.Storage.StorageFile.getFileFromApplicationUriAsync(new Windows.Foundation.Uri("ms-appx:///images/logo.png"))
            string countryDictString = DefaultDictClass.DictionaryDefault;
            XElement countryXml = XElement.Parse(countryDictString.Trim());
            StorageFile dictStorage = await ApplicationData.Current.LocalFolder.CreateFileAsync(DEFAULT_DICTIONARY, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(dictStorage, countryXml.ToString());
            CurrentDictionary = dictStorage.Path;
            ApplicationData.Current.LocalSettings.Values[CURRENT_DICT_NODE] = dictStorage.Path;
            ApplicationData.Current.LocalSettings.Values[CHAMBER_NODE_NODE] = "True";
        }

        public void save()
        {
            ApplicationData.Current.LocalSettings.Values[CURRENT_DICT_NODE] = CurrentDictionary;
            ApplicationData.Current.LocalSettings.Values[CHAMBER_NODE_NODE] = LargeChambers.ToString();
        }

    }
}
