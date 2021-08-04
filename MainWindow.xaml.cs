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
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Input;

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
        /// The audio file format currently supported.
        /// </summary>
        private const string SUPPORTED_FORMAT = @"*.mp3";

        /// <summary>
        /// The program's title.
        /// </summary>
        private static string PROGRAM_TITLE = @"Java3 AT2.1";

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            wdwMain.Title = PROGRAM_TITLE;
            PlayListDirty = true;
        }

        /// <summary>
        /// Gets or sets the PlayList.
        /// </summary>
        private LinkedList<string> PlayList { get; set; }

        /// <summary>
        /// Gets or sets the full path name of the playlist file.
        /// </summary>
        private string PlayListName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the PlayList needs to be saved.
        /// </summary>
        private bool PlayListDirty { get; set; }

        /// <summary>
        /// The Close_CanExecute.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="CanExecuteRoutedEventArgs"/>.</param>
        private void Close_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = PlayListName != null;
        }

        /// <summary>
        /// The Close_Executed.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="ExecutedRoutedEventArgs"/>.</param>
        private void Close_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (PlayListDirty)
            {
                if (MessageBox.Show(wdwMain, "You have modified your play list.\n" +
                    "Do you want to save it?", "File closing.", MessageBoxButton.YesNo,
                    MessageBoxImage.Question, MessageBoxResult.Yes) == MessageBoxResult.Yes)
                {
                    SavePlayList();
                }
            }

            PlayList = null;
            PlayListName = null;
            PlayListDirty = false;
            wdwMain.Title = PROGRAM_TITLE;
        }

        /// <summary>
        /// The Exit_Click.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="RoutedEventArgs"/>.</param>
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// The Open_CanExecute.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="CanExecuteRoutedEventArgs"/>.</param>
        private void Open_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        /// <summary>
        /// The Open_Executed.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="ExecutedRoutedEventArgs"/>.</param>
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
                string extn = filename[index.];

                switch (extn)
                {
                    case PLAYLIST_EXTN:
                    {
                        CheckResult result = CheckPlayListDirty(@"File opening.");

                        switch (result)
                        {
                            case CheckResult.False:
                            case CheckResult.True:
                            {
                                LoadPlayList(filename);
                                break;
                            }

                            default:
                            {
                                break;
                            }
                        }

                        break;
                    }

                    case SUPPORTED_FORMAT:
                    {
                        break;
                    }
                    default:
                    {
                        break;
                    }
                }

                UpdateProgramTitle();
            }
        }

        /// <summary>
        /// The AddAudioFilename.
        /// </summary>
        /// <param name="filename">The filename<see cref="string"/>.</param>
        private void AddAudioFilename(string filename)
        {
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
        /// The SavePlayList.
        /// </summary>
        private void SavePlayList()
        {
            MessageBox.Show($"Save not yet implemented!\n{PlayListName}", @"SavePlayList");
        }

        /// <summary>
        /// Load the play list from the file.
        /// </summary>
        /// <param name="filename">The play list file to load.</param>
        private void LoadPlayList(string filename)
        {
            PlayListName = filename;
            PlayList = new();
            PlayListDirty = false;

            MessageBox.Show($"Load not yet implemented!\n{filename}", @"LoadPlayList");
        }

        /// <summary>
        /// The Save_CanExecute.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="CanExecuteRoutedEventArgs"/>.</param>
        private void Save_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = PlayListDirty;
        }

        /// <summary>
        /// The Save_Executed.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="ExecutedRoutedEventArgs"/>.</param>
        private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
        }

        /// <summary>
        /// The Window_Closing.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="System.ComponentModel.CancelEventArgs"/>.</param>
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

        /// <summary>
        /// Result returned from the CheckPlayListDirty method.
        /// </summary>
        private enum CheckResult
        {
            /// <summary>
            /// PlayListDirty == false
            /// </summary>
            False,

            /// <summary>
            /// PlayListDirty == true
            /// </summary>
            True,

            /// <summary>
            /// PlayListDirty == true AND user selected [Cancel] button
            /// </summary>
            Cancel
        }

        /// <summary>
        /// The New_CanExecute.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="CanExecuteRoutedEventArgs"/>.</param>
        private void New_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
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
        /// The New_Executed.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="ExecutedRoutedEventArgs"/>.</param>
        private void New_Executed(object sender, ExecutedRoutedEventArgs e)
        {
        }

        /// <summary>
        /// Defines the <see cref="AudioFile" />.
        /// </summary>
        private class AudioFile
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="AudioFile"/> class.
            /// </summary>
            /// <param name="safeFileName">The safeFileName<see cref="string"/>.</param>
            /// <param name="fileName">The fileName<see cref="string"/>.</param>
            public AudioFile(string safeFileName, string fileName)
            {
                FileName = safeFileName;
                FullPathName = fileName;
            }

            /// <summary>
            /// Gets the FileName
            /// This is just the file name with extension.
            /// </summary>
            public string FileName { get; private set; }

            /// <summary>
            /// Gets the FullPathName
            /// This is the full path to and including the file name and extension.
            /// </summary>
            public string FullPathName { get; private set; }
        }
    }
}