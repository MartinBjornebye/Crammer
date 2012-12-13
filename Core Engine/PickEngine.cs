using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace MB.Crammer
{
    public class PickEngine
    {
        #region Constants
        private const string PROGRESS_STATE = "ProgressState";
        #endregion

        #region Attributes
        private NewWords mNewWordsChamber = new NewWords();
        private WordChamber mFirstChamber = new WordChamber();
        private WordChamber mSecondChamber = new WordChamber();
        private WordChamber mThirdChamber = new WordChamber();
        private WordChamber mFourthChamber = new WordChamber();
        private WordChamber mFifthChamber = new WordChamber();
        private WordChamber mCompletedChamber = new WordChamber();

        private CrammerDictionary mCrammerDict;	// Reference to Crammer Database
        private int mTotalWords = 0;			// Count of words in the database
        private int mKnownWords = 0;			// Count of completed words
        private int mStart = 0;				    // Index used for fetching new words
        private int mCurrentWord = 0;		    // Crammer database cursor (index)

        private int mInSystem = 0;			    // Count of words currently in the system
        private bool mRestoredState = false;	// Session restored from a previous session

        private bool mNewWordsInUse = false;	// Flag indicating if we're working on new words or not
        private bool mSwapSequence = false;		// Indicates which language is first in the sequence
        private bool mReachedEnd = false;
        #endregion

        #region Properties
        public int CurrentEntry
        {
            get { return mCurrentWord; }
            set { mCurrentWord = value; }
        }
        
        #endregion
        public PickEngine(CrammerDictionary dict, bool cleanState)
        {
            mCrammerDict = dict;

            // Are we able to restore from a previous session?
            // If not, initialize for a fresh one.
            init(cleanState);
        }

        public void close()
        {
            saveState();
            cleanUp();
        }

        public void reset()
        {
            cleanUp();

            mTotalWords = 0;
            mKnownWords = 0;
            mStart = 0;
            mInSystem = 0;
            mRestoredState = false;
            mNewWordsInUse = true;
            mReachedEnd = false;
        }

        public void execute()
        {
            while (true)
            {
                if (chamberCheck(mFirstChamber))
                    break;
                else if (chamberCheck(mSecondChamber))
                    break;
                else if (chamberCheck(mThirdChamber))
                    break;
                else if (chamberCheck(mFourthChamber))
                    break;
                else if (chamberCheck(mFifthChamber))
                    break;
                else
                {
                    if (newWords())
                        break;
                }
            }
        }

        /// <summary>
        /// Does the chamber we're checking have more space for additional entries?
        /// </summary>
        /// <param name="chamber"></param>
        /// <returns></returns>
        private bool chamberCheck(WordChamber chamber)
        {
            if (chamber.InChamber < WordChamber.chamber(chamber.ID))
                return (false);

            mCurrentWord = chamber.First;
            mNewWordsInUse = false;
            return (true);
        }

        /// <summary>
        /// Determines which chambers to work on based on the number of entries in them.
        /// The knowsEntry variable indicates if the user has answered that he knows it.
        /// </summary>
        /// <param name="yes"></param>
        public void answer(bool knowsEntry)
        {
            if (mFirstChamber.InChamber >= WordChamber.chamber(WordChamber.ChamberType.C1))
                handleAnswer(knowsEntry, mFirstChamber, mSecondChamber);
            else if (mSecondChamber.InChamber >= WordChamber.chamber(WordChamber.ChamberType.C2))
                handleAnswer(knowsEntry, mSecondChamber, mThirdChamber);
            else if (mThirdChamber.InChamber >= WordChamber.chamber(WordChamber.ChamberType.C3))
                handleAnswer(knowsEntry, mThirdChamber, mFourthChamber);
            else if (mFourthChamber.InChamber >= WordChamber.chamber(WordChamber.ChamberType.C4))
                handleAnswer(knowsEntry, mFourthChamber, mFifthChamber);
            else if (mFifthChamber.InChamber >= WordChamber.chamber(WordChamber.ChamberType.C5))
                handleAnswer(knowsEntry, mFifthChamber, mCompletedChamber);
            else
            {
                if (!knowsEntry)
                    mNewWordsChamber.skip();
            }
        }

        /// <summary>
        /// Picks up new fresh words
        /// </summary>
        /// <returns></returns>
        private bool newWords()
        {
            if (mInSystem < mTotalWords)
                mNewWordsInUse = true;

            if (mStart <= mTotalWords)
            {
                // Should we fill up with new words first?
                if (mNewWordsChamber.ChamberDone)
                {
                    mFirstChamber.append(mNewWordsChamber);
                    int nMax = WordChamber.chamber(WordChamber.ChamberType.C0);
                    int nNewWords = (mTotalWords - mStart) < nMax ? mTotalWords - mStart : nMax;

                    // Special case which indicates that we're done, i.e. 
                    // there are no more words in the database.
                    if (nNewWords == 0 && !mReachedEnd)
                    {
                        var dlg = new Windows.UI.Popups.MessageDialog("No more words available.\n" +
                                            "It seems you have successfully completed the current dictionary!");

                        dlg.ShowAsync();
                        throw new FinishedException("No more words available.\n" +
                                            "It seems you have successfully completed the current dictionary!");
                    }

                    // Have we reached the end of new words and have to squeeze out
                    // the remainder by using the last chamber? Is mInSystem has
                    // the same value as total words, that is the case.
                    if (mInSystem == mTotalWords)
                    {
                        mNewWordsChamber.initIndices(mCompletedChamber.getIndices(), mStart, nNewWords);
                    }
                    else
                    {
                        mNewWordsChamber.initIndices(mStart, nNewWords);
                    }

                    mStart += nNewWords;

                    // Increase in-system counter if new words are not done
                    if (mInSystem < mTotalWords)
                        mInSystem += nNewWords;

                    return (false);
                }
                else
                {
                    mCurrentWord = mNewWordsChamber.pickNext();
                }
            }
            else
            {
                // Start all over to squeeze out the last words
                mStart = 0;
                mReachedEnd = true;

                // Make sure the following is only done once
                if (mInSystem < mTotalWords)
                    mInSystem = mTotalWords;
            }
            return (true);
        }

        /// <summary>
        /// If user knows the entry, it is moved from one chamber to the next.
        /// If not, it is moved at the back of the first chamber to make it re-occur again within 
        /// a reasonable time of iterating through the dictionary entries.
        /// </summary>
        /// <param name="knowsEntry"></param>
        /// <param name="w"></param>
        /// <param name="next"></param>
        private void handleAnswer(bool knowsEntry, WordChamber w, WordChamber next)
        {
            if (mCurrentWord < 0)
                throw new Exception("Current word index is not valid");

            if (knowsEntry)
            {
                mCurrentWord = w.removeFirst();
                next.putLast(mCurrentWord);

                // If next is the sixth chamber, increase counter of known words.
                if (next.ID > WordChamber.ChamberType.C5)
                    mKnownWords++;
            }
            else
            {
                mFirstChamber.putLast(w.removeFirst());
            }
        }

        public bool done()
        {
            // This test will return true if the number of total words is less than the required 1500+
            // which it takes to run through the entire algorithm
            if (mCurrentWord >= mTotalWords)
                return (true);

            return (mTotalWords == mKnownWords);
        }

        public string getWord()
        {
            if (mCurrentWord < 0 || mCrammerDict.Empty )
                //throw new Exception("Current word index is not valid");
                return ("");

            if (mCurrentWord >= mCrammerDict.Entries.Count ||
                mCurrentWord < 0)
                throw new Exception("Index out of bounds in getWord");

            if (!mSwapSequence)
                return mCrammerDict.Entries[mCurrentWord].AEntry;
            else
                return mCrammerDict.Entries[mCurrentWord].BEntry;
        }

        /// <summary>
        /// Picks up a so called "native" entry which is basically equivalent to the answer
        /// </summary>
        /// <returns></returns>
        public string getNative()
        {
            if (mCurrentWord < 0)
                throw new Exception("Current word index is not valid");

            if (!mSwapSequence)
                return mCrammerDict.Entries[mCurrentWord].BEntry;
            else
                return mCrammerDict.Entries[mCurrentWord].AEntry;
        }

        public string getInSystem()
        {
            return (mInSystem.ToString());
        }

        public string getDoneWords()
        {
            return (mKnownWords.ToString());
        }


        public bool newWordsActive()
        {
            return (mNewWordsInUse);
        }


        #region Save/Restore

        public bool restored()
        {
            return (mRestoredState);
        }

        /// <summary>
        /// Restores the saved progress state of the current dictionary
        /// </summary>
        private bool restoreState()
        {
            mRestoredState = false;
            if (string.IsNullOrEmpty(mCrammerDict.StateFile))
                return (false);

            try
            {
                string stateString = mCrammerDict.getStateString();
                if (string.IsNullOrEmpty(stateString))
                    return (false);

                IBuffer stateBuffer = BinaryBufferConverter.getBufferFromString(stateString);
                using (DataReader dataReader = DataReader.FromBuffer(stateBuffer))
                {
                    mTotalWords = dataReader.ReadInt32();
                    mKnownWords = dataReader.ReadInt32();
                    mStart = dataReader.ReadInt32();
                    mCurrentWord = dataReader.ReadInt32();
                    mNewWordsInUse = dataReader.ReadBoolean();
                    mInSystem = dataReader.ReadInt32();
                    mSwapSequence = dataReader.ReadBoolean();
                    mReachedEnd = dataReader.ReadBoolean();

                    mNewWordsChamber.restoreState(dataReader);
                    mFirstChamber.restoreState(dataReader);
                    mSecondChamber.restoreState(dataReader);
                    mThirdChamber.restoreState(dataReader);
                    mFourthChamber.restoreState(dataReader);
                    mFifthChamber.restoreState(dataReader);
                    mCompletedChamber.restoreState(dataReader);
                }
            }
            catch (Exception)
            {
                // Assume mismatch with chamber-size config and contents of .STA file.
                // Delete it so that the dictionary will load successfully next time.
                mCrammerDict.removeStateFile();
                return (false);
            }

            mRestoredState = true;
            return (true);
        }

        /// <summary>
        /// Saves the current progress state for the current dictionary
        /// </summary>
        public void saveState()
        {
            if (string.IsNullOrEmpty(mCrammerDict.StateFile))
                throw new Exception("No state file available");

            try
            {
                using (InMemoryRandomAccessStream memoryStream = new InMemoryRandomAccessStream())
                {
                    using (DataWriter dataWriter = new DataWriter(memoryStream))
                    {
                        dataWriter.WriteInt32(mTotalWords);
                        dataWriter.WriteInt32(mKnownWords);
                        dataWriter.WriteInt32(mStart);
                        dataWriter.WriteInt32(mCurrentWord);
                        dataWriter.WriteBoolean(mNewWordsInUse);
                        dataWriter.WriteInt32(mInSystem);
                        dataWriter.WriteBoolean(mSwapSequence);
                        dataWriter.WriteBoolean(mReachedEnd);

                        mNewWordsChamber.saveState(dataWriter);
                        mFirstChamber.saveState(dataWriter);
                        mSecondChamber.saveState(dataWriter);
                        mThirdChamber.saveState(dataWriter);
                        mFourthChamber.saveState(dataWriter);
                        mFifthChamber.saveState(dataWriter);
                        mCompletedChamber.saveState(dataWriter);

                        IBuffer stateBuffer = dataWriter.DetachBuffer();
                        string stateString = BinaryBufferConverter.getStringFromBuffer(stateBuffer);

                        mCrammerDict.setStateFile(stateString);
                    }
                }
            }
            catch (Exception)
            {
                // Assume mismatch with chamber-size config and contents of .STA file.
                // Delete it so that the dictionary will load successfully next time.
                mCrammerDict.removeStateFile();
            }

            mRestoredState = true;
        }

        #endregion

        public void removeStateFile()
        {
            mCrammerDict.removeStateFile();
        }

        public bool swappedSequence()
        {
            return (mSwapSequence);
        }

        public void toggleSequence()
        {
            mSwapSequence = !mSwapSequence;
        }

        private void setTotalWords()
        {
            mTotalWords = mCrammerDict.Entries.Count;
        }

        public async Task updateTimeStamp()
        {
            await mCrammerDict.updateTimeStamp(mCurrentWord);
        }

        private void cleanUp()
        {
            mNewWordsChamber.reset();
            mFirstChamber.reset();
            mSecondChamber.reset();
            mThirdChamber.reset();
            mFourthChamber.reset();
            mFifthChamber.reset();
            mCompletedChamber.reset();
        }

        public void init(bool cleanState)
        {
            mNewWordsChamber.init();
            mFirstChamber.init(WordChamber.ChamberType.C1);
            mSecondChamber.init(WordChamber.ChamberType.C2);
            mThirdChamber.init(WordChamber.ChamberType.C3);
            mFourthChamber.init(WordChamber.ChamberType.C4);
            mFifthChamber.init(WordChamber.ChamberType.C5);

            setTotalWords();
            mCompletedChamber.init(WordChamber.ChamberType.C6, mTotalWords);

            int C0ChamberSize = WordChamber.chamber(WordChamber.ChamberType.C0);

            // Should we ignore already saved state?
            if (!cleanState)
            {
                if ( restoreState() == false )
                {
                    mNewWordsInUse = true;
                    mNewWordsChamber.initIndices(mStart, mTotalWords > C0ChamberSize ? C0ChamberSize : mTotalWords); // Fill up with starter words
                    mStart += (mTotalWords > C0ChamberSize ? C0ChamberSize : mTotalWords);
                }
            }
            else
            {
                mNewWordsInUse = true;
                mNewWordsChamber.initIndices(mStart, mTotalWords > C0ChamberSize ? C0ChamberSize : mTotalWords); // Fill up with starter words
                mStart += mTotalWords > C0ChamberSize ? C0ChamberSize : mTotalWords;
            }
        }

    }
}
