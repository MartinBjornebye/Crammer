////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Source:      CrammerDictionary.cs
// Author:      Martin Bjornebye
// Description: This class wraps management of the Crammer dictionaries, such as adding, editing and 
//              removing entries.
//
// Created:     2011.01.08 14:01:11
////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.IO;
using Windows.Storage;
using Windows.Foundation;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MB.Crammer
{
    public class CrammerDictionary
    {

        #region Constants
        private const string STATE_MARKER = " - STATE";
        public const string DICT_EXTENSION = ".crammerdict";
        private const string DICT_ROOT = "Root";
        private const string DICT_HEADER = "Header";
        private const string DICT_TITLE = "Title";
        private const string DICT_ENTRIES = "Entries";
        private const string DICT_FONTS = "Fonts";
        public const string DICT_ROW = "r";
        public const string DICT_A = "a";
        public const string DICT_B = "b";
        public const string DICT_STAMP = "s";
        public const string DICT_ACTIVE = "on";
        public const string ACTIVE = "1";
        public const string INACTIVE = "0";
        private const string DEFAULT_FONT = "Times New Roman";
        public const float DEFAULT_FONT_SIZE = 20;
        private const int DEFAULT_A_FONT_COLOR = -16777216;
        private const int DEFAULT_B_FONT_COLOR = -16744448;

        private const string FONT_A = "a";
        private const string FONT_B = "b";
        private const string FONT_NAME = "Name";
        private const string FONT_SIZE = "Size";
        private const string FONT_COLOR = "Color";
        private const string FONT_BOLD = "Bold";

        private const string STATE_FILE_EXT = ".statefile";
        private const int OUTPUT_ALLOC_SIZE = 5000000;

        #endregion

        #region Attributes
        private XElement mDictionary = null;
        private List<DictionaryEntry> mEntries = new List<DictionaryEntry>();
        private List<DictionaryEntry> mInactiveEntries = new List<DictionaryEntry>();
        private string mDictionaryTitle = "";
        private string mDictionaryFile = "";
        private string mStateFile = "";
        #endregion

        #region Properties

        public bool Empty
        {
            get { return (mEntries.Count == 0); }
        }

        public string DictionaryFile
        {
            get { return mDictionaryFile; }
            set { mDictionaryFile = value; }
        }

        public string DictionaryTitle
        {
            get { return mDictionaryTitle; }
            set { mDictionaryTitle = value; }
        }

        public List<DictionaryEntry> Entries
        {
            get { return (mEntries); }
            set { mEntries = value; }
        }

        public List<DictionaryEntry> InactiveEntries
        {
            get { return (mInactiveEntries); }
        }

        public string StateFile
        {
            get { return mStateFile; }
            set { mStateFile = value; }
        }
        #endregion

        #region Public Functions

        /// <summary>
        /// Load a Crammer dictionary
        /// </summary>
        /// <param name="dictionary"></param>
        public void load(string dictionary, bool activeOnly)
        {
            if (string.IsNullOrEmpty(dictionary))
                throw new Exception("No dictionary name given");

            mDictionaryFile = dictionary;
            createStateFile();

            mDictionary = XElement.Load(dictionary);
            getDictionaryTitle();

            IEnumerable<XElement> rows = mDictionary.Descendants(DICT_ROW);
            mEntries.Clear();
            mInactiveEntries.Clear();
            foreach (XElement row in rows)
            {
                DictionaryEntry entry = new DictionaryEntry(row);
                if (activeOnly )
                {
                    if (entry.Active)
                        mEntries.Add(entry);
                    else
                        mInactiveEntries.Add(entry);
                }
                else
                {
                    mEntries.Add(entry);
                }
            }
        }


        public void reLoad()
        {
            load(mDictionaryFile, true);
        }

        /// <summary>
        /// Saves the current contents by overwriting the existing dictionary file with updated entries.
        /// By default, the statefile is truncated so user has to start over.
        /// </summary>
        async public Task save(bool truncateStateFile)
        {
            if (string.IsNullOrEmpty(mDictionaryFile))
                throw new Exception("This dictionary does not have a file name and is not valid");

            await removeDictionaryFile();

            if (truncateStateFile)
            {
                removeStateFile();
            }

            await create(mDictionaryFile);
        }

        /// <summary>
        /// Creates a new dictionary based on a list of entries and a file name
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="entries"></param>
        async public Task create(string dictionaryPath)
        {
            XElement nodeEntries = null;
            XDocument dictionaryFileXML = new XDocument(
                            new XDeclaration("1.0", "UTF-16", null),
                            new XElement(DICT_ROOT,
                            new XElement(DICT_HEADER,
                                new XElement(DICT_TITLE, mDictionaryTitle),
                            nodeEntries = new XElement(DICT_ENTRIES))));

            foreach (DictionaryEntry entry in mEntries)
            {
                XElement entryElem = new XElement(DICT_ROW,
                                        new XAttribute(DICT_A, entry.AEntry),
                                        new XAttribute(DICT_B, entry.BEntry),
                                        new XAttribute(DICT_STAMP, entry.Stamp.Ticks.ToString()),
                                        new XAttribute(DICT_ACTIVE, entry.Active ? ACTIVE : INACTIVE));

                nodeEntries.Add(entryElem);
            }

            StorageFile dictionaryStorage = await ApplicationData.Current.LocalFolder.CreateFileAsync(Path.GetFileName(dictionaryPath), CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(dictionaryStorage, dictionaryFileXML.ToString());
        }

        public void createStateFile()
        {
            if (string.IsNullOrEmpty(mDictionaryFile))
                throw new Exception("Dictionary file-path not provided. Cannot create a state-file");

            // Derive a state file from dictionary
            string tmpDir = Path.GetDirectoryName(mDictionaryFile);
            string bareFileName = Path.GetFileNameWithoutExtension(mDictionaryFile) + STATE_MARKER;
            mStateFile = Path.Combine(tmpDir, bareFileName) + STATE_FILE_EXT;
        }


        async public Task removeDictionaryFile()
        {
            StorageFile dictionaryStorage = null;
            try
            {
                dictionaryStorage = await ApplicationData.Current.LocalFolder.GetFileAsync(Path.GetFileName(mDictionaryFile));
            }
            catch (FileNotFoundException)
            {
            }
            if (dictionaryStorage != null)
                await dictionaryStorage.DeleteAsync();
        }

        public void removeStateFile()
        {
            setStateFile("");
        }

        public string getStateString()
        {
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey(Path.GetFileName(mStateFile)))
                return (ApplicationData.Current.LocalSettings.Values[Path.GetFileName(mStateFile)].ToString());
            return ("");
        }

        /// <summary>
        /// Sets the state string in the LocalSettings cache
        /// </summary>
        /// <param name="stateString"></param>
        public void setStateFile(string stateString)
        {
            ApplicationData.Current.LocalSettings.Values[Path.GetFileName(mStateFile)] = stateString;
        }


        /// <summary>
        /// Adds a specific entry to the dictionary
        /// </summary>
        /// <param name="entry"></param>
        public void addEntry(DictionaryEntry entry)
        {
            // Check if entry already exists
            if (entryExists(entry))
                throw new Exception("An entry with values: " + entry.AEntry + " / " + entry.BEntry + " already exists");

            mEntries.Add(entry);
        }

        /// <summary>
        /// Removes an entry from the dictionary
        /// </summary>
        /// <param name="entry"></param>
        async public void removeEntry(DictionaryEntry entry)
        {
            if (!entryExists(entry))
                throw new Exception("An entry with values: " + entry.AEntry + " / " + entry.BEntry + " does not exist in the dictionary");

            mEntries.Remove(entry);
            await save(true);
        }

        public bool entryExists(DictionaryEntry entry)
        {
            IEnumerable<DictionaryEntry> matches = from c in mEntries 
                                                   where c.AEntry == entry.AEntry
                                                   select c;
            return ( matches.Count() > 0 );
        }

        public async Task updateTimeStamp(int wordIndex)
        {
            if (wordIndex < 0 || wordIndex >= mEntries.Count)
                throw new Exception("Word index out of bounds");

            DictionaryEntry currentEntry = mEntries[wordIndex];
            currentEntry.Stamp = DateTime.Now;
            await save(false);
        }


        public static void removeStateFiles()
        {
            //foreach (string stateFile in Properties.Settings.Default.DictionaryHistory)
            //{
            //    if (File.Exists(stateFile))
            //        File.Delete(stateFile);
            //}
        }

        #region Sorting Functions

        public async Task randomShuffle()
        {
            mEntries = mEntries.OrderBy(emp => Guid.NewGuid()).ToList();
            await save(true);
        }

        public async Task sortAscending()
        {
            mEntries = mEntries.OrderBy(c => c.AEntry).ToList();
            await save(true);
        }

        public async Task sortDescending()
        {
            mEntries = mEntries.OrderByDescending(c => c.AEntry).ToList();
            await save(true);
        }

        public async Task sortByTimestampAscending()
        {
            mEntries = mEntries.OrderBy(c => c.Stamp).ToList();
            await save(true);
        }

        public async Task sortByTimestampDescending()
        {
            mEntries = mEntries.OrderByDescending(c => c.Stamp).ToList();
            await save(true);
        }
        #endregion

        #endregion // Public

        #region Private Functions

        /// <summary>
        /// Picks up the Dictionary title
        /// </summary>
        private void getDictionaryTitle()
        {
            XElement header = mDictionary.Element(DICT_HEADER);
            if (header == null)
                throw new Exception("Failed to find a Header entry in the dictionary");

            XElement title = header.Element(DICT_TITLE);
            if (title == null)
                throw new Exception("Failed to find a Title entry in the dictionary");

            mDictionaryTitle = title.Value;
        }


        #endregion

    }
}
