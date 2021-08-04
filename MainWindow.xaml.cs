/*
 *  File Name:   MainWindow.xaml.cs
 *
 *  Copyright (c) 2021 Bradley Willcott
 *
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
 *
 * ****************************************************************
 * Name: Bradley Willcott
 * ID:   M198449
 * Date: 4/08/2021
 * ****************************************************************
 */

namespace Prog3AT2_One
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Windows;
    using System.Windows.Input;
    using System.Xml.Serialization;

    using Microsoft.Win32;

    /// <summary>
    /// Interaction logic for MainWindow.xaml.
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// The default file extension for PlayLists.
        /// </summary>
        private const string PLAYLIST_EXTN = @".pld";

        /// <summary>
        /// The program's title.
        /// </summary>
        private const string PROGRAM_TITLE = @"Java3 AT2.1";

        /// <summary>
        /// The audio file format currently supported.
        /// </summary>
        private const string SUPPORTED_FORMAT = @"*.mp3";

        /// <summary>
        /// The default name for a new play list.
        /// </summary>
        private static string DEFAULT_PLAYLIST_NAME = $"PlayList{PLAYLIST_EXTN}";

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            // Setup default empty play list ...
            PlayList = new();
            PlayListName = DEFAULT_PLAYLIST_NAME;
            PlayListIsNew = true;

            UpdateProgramTitle();
        }

        /// <summary>
        /// Gets or sets the PlayList.
        /// </summary>
        private LinkedList<FileData> PlayList { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the PlayList needs to be saved.
        /// </summary>
        private bool PlayListDirty { get; set; }

        /// <summary>
        /// Is the current Play List a New one?
        /// <para/>
        /// It is if it has not been loaded from, or saved to a file?
        /// </summary>
        private bool PlayListIsNew { get; set; }

        /// <summary>
        /// Gets or sets the full path name of the playlist file.
        /// </summary>
        private string PlayListName { get; set; }

        /// <summary>
        /// Add this file to the Play List.
        /// </summary>
        /// <param name="openFileDialog">The object used to select the filename</param>
        /// <returns>Newly created object</returns>
        private FileData AddAudioFilename(OpenFileDialog openFileDialog)
        {
            FileData fileData = new FileData(openFileDialog);

            PlayList.AddLast(fileData);

            return fileData;
        }

        /// <summary>
        /// Checks to see if the play list needs to be saved.
        /// If it is, it gives the user a chance to save it.
        /// </summary>
        /// <param name="title">for the dialog box.</param>
        /// <returns></returns>
        private CheckResult CheckPlayListDirty(string title)
        {
            CheckResult rtn = CheckResult.False;

            if (PlayListDirty)
            {
                rtn = CheckResult.True;

                MessageBoxResult result = MessageBox.Show(wdwMain, "You have modified your play list.\n" +
                    "Do you want to save it?", title, MessageBoxButton.YesNoCancel,
                    MessageBoxImage.Question, MessageBoxResult.Yes);

                switch (result)
                {
                    case MessageBoxResult.Yes:
                    {
                        SavePlayList();
                        break;
                    }

                    case MessageBoxResult.Cancel:
                    {
                        rtn = CheckResult.Cancel;
                        break;
                    }

                    default:
                    {
                        break;
                    }
                }
            }

            return rtn;
        }

        /// <summary>
        /// The Close_CanExecute.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e <see cref="CanExecuteRoutedEventArgs"/>.</param>
        private void Close_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
        }

        /// <summary>
        /// The Close_Executed.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e <see cref="ExecutedRoutedEventArgs"/>.</param>
        private void Close_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            CheckPlayListDirty(@"File closing...");

            PlayList = null;
            PlayListName = null;
            PlayListDirty = false;

            UpdateProgramTitle();
        }

        /// <summary>
        /// The Exit_Click.
        /// </summary>
        /// <param name="sender">The sender <see cref="object"/>.</param>
        /// <param name="e">The e <see cref="RoutedEventArgs"/>.</param>
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Load the play list from the file.
        /// </summary>
        /// <param name="filename">The play list file to load.</param>
        private void LoadPlayList(string filename)
        {
            PlayListName = filename;
            PlayListDirty = false;

            XmlSerializer mySerializer = new XmlSerializer(typeof(LinkedList<FileData>));
            TextReader myReader = new StreamReader(PlayListName);
            PlayList = (LinkedList<FileData>)mySerializer.Deserialize(myReader);
            myReader.Close();

            UpdateProgramTitle();

            MessageBox.Show($"Load not yet implemented!\n{filename}", @"LoadPlayList");
        }

        /// <summary>
        /// The New_CanExecute.
        /// </summary>
        /// <param name="sender">The sender <see cref="object"/>.</param>
        /// <param name="e">The e <see cref="CanExecuteRoutedEventArgs"/>.</param>
        private void New_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        /// <summary>
        /// The New_Executed.
        /// </summary>
        /// <param name="sender">The sender <see cref="object"/>.</param>
        /// <param name="e">The e <see cref="ExecutedRoutedEventArgs"/>.</param>
        private void New_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            CheckResult result = CheckPlayListDirty(@"New Play List...");

            if (result != CheckResult.Cancel)
            {
                // Setup default empty play list ...
                PlayList = new();
                PlayListName = DEFAULT_PLAYLIST_NAME;
                PlayListDirty = false;
                PlayListIsNew = true;

                UpdateProgramTitle();
            }
        }

        /// <summary>
        /// The Open_CanExecute.
        /// </summary>
        /// <param name="sender">The sender <see cref="object"/>.</param>
        /// <param name="e">The e <see cref="CanExecuteRoutedEventArgs"/>.</param>
        private void Open_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        /// <summary>
        /// The Open_Executed.
        /// </summary>
        /// <param name="sender">The sender <see cref="object"/>.</param>
        /// <param name="e">The e <see cref="ExecutedRoutedEventArgs"/>.</param>
        private void Open_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            string filter = $"Audio files ({SUPPORTED_FORMAT})|{SUPPORTED_FORMAT}|" +
                            $"Play List files ({PLAYLIST_EXTN})|{PLAYLIST_EXTN}|" +
                            @"All files (*.*)|*.*";

            OpenFileDialog openFileDialog = new();

            openFileDialog.CheckFileExists = true;
            openFileDialog.CheckPathExists = true;
            openFileDialog.Filter = filter;
            openFileDialog.Title = @"Select file to Open";

            if ((bool)openFileDialog.ShowDialog(wdwMain))
            {
                string filename = openFileDialog.SafeFileName;
                int index = filename.LastIndexOf('.');
                string extn = filename[index..];

                switch (extn)
                {
                    case PLAYLIST_EXTN:
                    {
                        CheckResult result = CheckPlayListDirty(@"File opening...");

                        if (result != CheckResult.Cancel)
                        {
                            LoadPlayList(filename);
                        }

                        break;
                    }

                    case SUPPORTED_FORMAT:
                    {
                        AddAudioFilename(openFileDialog);
                        OpenAudioFile(openFileDialog.FileName);
                        break;
                    }

                    default:
                    {
                        // Unsupported file type
                        break;
                    }
                }

                UpdateProgramTitle();
            }
        }

        /// <summary>
        /// Open the audio file.
        /// </summary>
        /// <param name="fileName">the full path name of the audio file</param>
        private void OpenAudioFile(string fileName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The Save_CanExecute.
        /// </summary>
        /// <param name="sender">The sender <see cref="object"/>.</param>
        /// <param name="e">The e <see cref="CanExecuteRoutedEventArgs"/>.</param>
        private void Save_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = PlayListDirty;
        }

        /// <summary>
        /// The Save_Executed.
        /// </summary>
        /// <param name="sender">The sender <see cref="object"/>.</param>
        /// <param name="e">The e <see cref="ExecutedRoutedEventArgs"/>.</param>
        private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SavePlayList();
        }

        /// <summary>
        /// The SavePlayList.
        /// </summary>
        private void SavePlayList()
        {
            if (PlayListIsNew)
            {
                string filter = $"Play List files ({PLAYLIST_EXTN})|{PLAYLIST_EXTN}|" +
                @"All files (*.*)|*.*";

                SaveFileDialog saveFileDialog = new();

                saveFileDialog.CheckPathExists = true;
                saveFileDialog.Filter = filter;
                saveFileDialog.FileName = PlayListName;
                saveFileDialog.DefaultExt = PLAYLIST_EXTN;
                saveFileDialog.Title = @"Select file to Save to";

                if ((bool)saveFileDialog.ShowDialog(wdwMain))
                {
                    PlayListName = saveFileDialog.FileName;
                }
                else
                {
                    // User canceled action
                    return;
                }
            }

            // Store records in XML file
            XmlSerializer mySerializer = new XmlSerializer(typeof(LinkedList<FileData>));
            TextWriter myWriter = new StreamWriter(PlayListName);
            mySerializer.Serialize(myWriter, PlayList);
            myWriter.Close();

            PlayListDirty = false;
            PlayListIsNew = false;

            UpdateProgramTitle();
        }

        /// <summary>
        /// Update the MainWindow title based on current settings.
        /// </summary>
        private void UpdateProgramTitle()
        {
            wdwMain.Title = PlayListName +
                (PlayListDirty ? "*" : "")
                + @" | " + PROGRAM_TITLE;
        }

        /// <summary>
        /// The Window_Closing.
        /// </summary>
        /// <param name="sender">The sender <see cref="object"/>.</param>
        /// <param name="e">The e <see cref="System.ComponentModel.CancelEventArgs"/>.</param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            CheckResult result = CheckPlayListDirty(@"Program exiting.");

            switch (result)
            {
                case CheckResult.False:
                {
                    if (MessageBox.Show(wdwMain, @"Are you sure?", @"Program exiting.", MessageBoxButton.YesNo,
                               MessageBoxImage.Warning, MessageBoxResult.No) == MessageBoxResult.No)
                    {
                        e.Cancel = true;
                    }

                    break;
                }

                case CheckResult.True:
                {
                    // Let it close.
                    break;
                }

                case CheckResult.Cancel:
                {
                    e.Cancel = true;
                    break;
                }

                default:
                {
                    break;
                }
            }
        }
    }
}