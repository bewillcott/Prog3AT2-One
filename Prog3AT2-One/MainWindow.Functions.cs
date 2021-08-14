/*
 *  File Name:   MainWindow.Functions.cs
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
 * Date: 5/08/2021
 * ****************************************************************
 */

namespace Prog3AT2_One
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Resources;
    using System.Xml.Serialization;

    using Microsoft.Win32;

    using Prog3AT2_One.Classes;

    /// <summary>
    /// Defines the <see cref="MainWindow" />.
    /// </summary>
    public partial class MainWindow
    {
        /// <summary>
        /// Provides a delay of <paramref name="seconds"/> seconds.
        /// </summary>
        /// <param name="seconds">the delay required</param>
        /// <returns></returns>
        private static async Task DelayAsync(int seconds)
        {
            await Task.Delay(seconds * 1000).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the image.
        /// <para/>
        /// <b>Note:</b> The original code was copied from: <br/>
        /// https://stackoverflow.com/questions/13592150/load-image-from-resources. <br/> I have
        /// made some minor cosmetic changes.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>.</returns>
        private static ImageSource GetImage(string name)
        {
            StreamResourceInfo sri = Application.GetResourceStream(new Uri($"/Prog3AT2-One;component/Images/{name}", UriKind.Relative));
            BitmapImage bmp = new();
            bmp.BeginInit();
            bmp.StreamSource = sri.Stream;
            bmp.EndInit();

            return bmp;
        }

        private FileData AddAudioFilename(FileData fileData)
        {
            MediaPlayer temp = new();
            temp.Open(new Uri(fileData.FullPathName));

            PlayList.Add(fileData);

            while (!temp.NaturalDuration.HasTimeSpan)
            {
                DelayAsync(1).Wait();
            }

            fileData.Duration = (int)temp.NaturalDuration.TimeSpan.TotalSeconds;
            PlayListIsDirty = true;

            if (AutoplayIsOn)
            {
                lvPlayList.SelectedIndex = lvPlayList.Items.Count - 1;
            }
            else
            {
                AutoplayIsEnabled = true;
            }

            return fileData;
        }

        /// <summary>
        /// Checks to see if the play list needs to be saved. If it is, it gives the user a chance
        /// to save it.
        /// </summary>
        /// <param name="title">for the dialog box.</param>
        /// <returns>.</returns>
        private CheckResult CheckPlayListDirty(string title)
        {
            CheckResult rtn = CheckResult.False;

            if (PlayListIsDirty)
            {
                rtn = CheckResult.True;

                MessageBoxResult result = MessageBox.Show(wdwMain, "You have modified your play list.\n" +
                    "Do you want to save it?", title, MessageBoxButton.YesNoCancel,
                    MessageBoxImage.Question, MessageBoxResult.Yes);

                switch (result)
                {
                    case MessageBoxResult.Yes:
                    {
                        rtn = SavePlayList();
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
        /// Clears the media controls.
        /// </summary>
        /// <returns></returns>
        private void ClearMediaControls()
        {
            PreviousIsEnabled = false;
            NextIsEnabled = false;

            if (MediaIsPlaying)
            {
                ToolBarImageClick(imgStop, false);
            }

            if (TheMediaPlayer.Source != null)
            {
                PlayIsEnabled = false;
                TheMediaPlayer.Close();
                tbPlaying.Text = "";
                lblTotal.Content = "00:00";
            }

            AutoplayIsEnabled = false;
            UpdatePlayListHasMultipleItems();
        }

        /// <summary>
        /// Format the text for the tbPlaying field.
        /// </summary>
        /// <param name="fileData">The source of the info to display</param>
        /// <returns></returns>
        private string FormatPlayingText(FileData fileData) =>
            tbPlaying.Text = $"{(!string.IsNullOrWhiteSpace(fileData.Artist) ? $"{fileData.Artist}\n" : "")}{fileData.Title}";

        /// <summary>
        /// Load the play list from the file.
        /// </summary>
        /// <param name="filename">The play list file to load.</param>
        private void LoadPlayList(string filename)
        {
            PlayListName = filename;

            XmlSerializer mySerializer = new(typeof(ObservableLinkedList<FileData>));
            TextReader myReader = new StreamReader(PlayListName);
            PlayList = (ObservableLinkedList<FileData>)mySerializer.Deserialize(myReader);
            myReader.Close();

            AutoplayIsEnabled = true;
        }

        /// <summary>
        /// Open the audio file for playing.
        /// </summary>
        /// <param name="fileData">The data for the file to load</param>
        private void OpenAudioFile(FileData fileData)
        {
            if (MediaIsPlaying)
            {
                ToolBarImageClick(imgStop, false);
            }

            if (TheMediaPlayer.Source != null)
            {
                TheMediaPlayer.Close();
            }

            TheMediaPlayer.Open(new Uri(fileData.FullPathName));

            slPosition.Maximum = fileData.Duration;
            lblTotal.Content = TimeSpan.FromSeconds(fileData.Duration).ToString(@"mm\:ss");
            tbPlaying.Text = FormatPlayingText(fileData);

            // Enable controls ...
            SliderIsEnabled = true;
            PlayIsEnabled = true;

            NextIsEnabled = true;
            PreviousIsEnabled = true;

            if (AutoplayIsOn)
            {
                ToolBarImageClick(imgPlay, false);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        private CheckResult SavePlayList()
        {
            CheckResult rtn = CheckResult.False;

            if (PlayListIsNew)
            {
                SaveFileDialog saveFileDialog = new();

                saveFileDialog.CheckPathExists = true;
                saveFileDialog.Filter = SAVE_PLAYLIST_FILTER;
                saveFileDialog.FileName = PlayListName;
                saveFileDialog.DefaultExt = PLAYLIST_EXTN;
                saveFileDialog.Title = @"Select file to Save to";

                if ((bool)saveFileDialog.ShowDialog(wdwMain))
                {
                    PlayListName = saveFileDialog.FileName;
                    PlayListIsNew = false;
                    rtn = CheckResult.True;
                }
                else
                {
                    // User canceled action
                    return CheckResult.Cancel;
                }
            }

            // Store records in XML file
            XmlSerializer mySerializer = new(typeof(ObservableLinkedList<FileData>));
            TextWriter myWriter = new StreamWriter(PlayListName);
            mySerializer.Serialize(myWriter, PlayList);
            myWriter.Close();
            PlayListIsDirty = false;

            return rtn;
        }

        /// <summary>
        /// Processes the Click event of the ToolBar Image controls.
        /// </summary>
        /// <remarks>
        /// This method is called as a result of the <c>MouseUp</c> events of the media image controls.
        /// </remarks>
        /// <seealso cref="ToolBarImage_MouseUp(object, System.Windows.Input.MouseButtonEventArgs)"/>
        /// <param name="image">The image.</param>
        /// <param name="mouseover">The mouse is over the button</param>
        private void ToolBarImageClick(Image image, bool mouseover = true)
        {
            switch (image.Tag)
            {
                case AUTOPLAY_OFF:
                {
                    // Turn on autoplay
                    image.Tag = AUTOPLAY_ON;

                    image.Source = GetImage($"{image.Tag as string}{(mouseover ? MOUSEOVER : "")}.png");

                    AutoplayIsOn = true;
                    break;
                }

                case AUTOPLAY_ON:
                {
                    // Turn off autoplay
                    image.Tag = AUTOPLAY_OFF;

                    image.Source = GetImage($"{image.Tag as string}{(mouseover ? MOUSEOVER : "")}.png");

                    AutoplayIsOn = false;
                    break;
                }

                case BUTTON_FIRST:
                {
                    // Select and load first audio file
                    image.Source = GetImage($"{image.Tag as string}{(mouseover ? MOUSEOVER : "")}.png");

                    lvPlayList.SelectedItem = PlayList.First.Value;

                    break;
                }

                case BUTTON_LAST:
                {
                    // Select and load last audio file
                    image.Source = GetImage($"{image.Tag as string}{(mouseover ? MOUSEOVER : "")}.png");

                    lvPlayList.SelectedItem = PlayList.Last.Value;

                    break;
                }

                case BUTTON_NEXT:
                {
                    // Select and load next audio file
                    image.Source = GetImage($"{image.Tag as string}{(mouseover ? MOUSEOVER : "")}.png");

                    if (PlayList.Count > 1)
                    {
                        int nextIndex = 0;

                        if ((lvPlayList.SelectedIndex >= 0) && (lvPlayList.SelectedIndex < (lvPlayList.Items.Count - 1)))
                        {
                            nextIndex = lvPlayList.SelectedIndex + 1;
                        }

                        lvPlayList.SelectedItem = PlayList[nextIndex];
                    }
                    else
                    {
                        MediaIsPlaying = false;
                        MediaIsPlaying = true;
                    }

                    break;
                }

                case BUTTON_PAUSE:
                {
                    // Pausing audio file
                    image.Tag = BUTTON_PLAY;
                    image.ToolTip = PLAY_TOOLTIP;

                    image.Source = GetImage($"{image.Tag as string}{(mouseover ? MOUSEOVER : "")}.png");

                    MediaIsPaused = true;
                    break;
                }

                case BUTTON_PLAY:
                {
                    // Play audio file
                    image.Tag = BUTTON_PAUSE;
                    image.ToolTip = PAUSE_TOOLTIP;

                    image.Source = GetImage($"{image.Tag as string}{(mouseover ? MOUSEOVER : "")}.png");

                    StopIsEnabled = true;

                    if (MediaIsPlaying)
                    {
                        MediaIsPaused = false;
                    }
                    else
                    {
                        MediaIsPlaying = true;
                    }

                    break;
                }

                case BUTTON_PREVIOUS:
                {
                    // Select and load previous audio file
                    image.Source = GetImage($"{image.Tag as string}{(mouseover ? MOUSEOVER : "")}.png");

                    int prevIndex = lvPlayList.Items.Count - 1;

                    if ((lvPlayList.SelectedIndex > 0) && (lvPlayList.SelectedIndex < lvPlayList.Items.Count))
                    {
                        prevIndex = lvPlayList.SelectedIndex - 1;
                    }

                    lvPlayList.SelectedItem = PlayList[prevIndex];

                    break;
                }

                case BUTTON_STOP:
                {
                    // Stop audio file
                    image.Source = GetImage($"{image.Tag as string}{(mouseover ? MOUSEOVER : "")}.png");

                    MediaIsPlaying = false;
                    imgPlay.Tag = BUTTON_PLAY;
                    imgPlay.Source = GetImage($"{imgPlay.Tag as string}.png");
                    StopIsEnabled = false;

                    break;
                }

                case EDIT_LIST_ITEM:
                {
                    image.Source = GetImage($"{image.Tag as string}{(mouseover ? MOUSEOVER : "")}.png");

                    if (AutoplayIsOn)
                    {
                        ToolBarImageClick(imgAutoplay, false);
                    }

                    FileData fileData = PlayList[lvPlayList.SelectedIndex];
                    FileDataDialog fileDataDialog = new(fileData);

                    if (fileDataDialog.ShowDialog() == true)
                    {
                        tbPlaying.Text = FormatPlayingText(fileData);
                        PlayListIsDirty = true;
                    }

                    break;
                }

                case MOVE_DOWN:
                {
                    image.Source = GetImage($"{image.Tag as string}{(mouseover ? MOUSEOVER : "")}.png");

                    if (AutoplayIsOn)
                    {
                        ToolBarImageClick(imgAutoplay, false);
                    }

                    int index = lvPlayList.SelectedIndex;

                    if (index >= 0 && index < PlayList.Count - 1)
                    {
                        // Get the node to move
                        LinkedListNode<FileData> node1 = PlayList.GetNodeAt(index);
                        PlayList.RemoveAt(index);

                        // Get the node we want to move below
                        LinkedListNode<FileData> node2 = PlayList.GetNodeAt(index);
                        PlayList.AddAfter(node2, node1);

                        lvPlayList.SelectedItem = node1.Value;
                        PlayListIsDirty = true;
                    }

                    break;
                }

                case MOVE_UP:
                {
                    image.Source = GetImage($"{image.Tag as string}{(mouseover ? MOUSEOVER : "")}.png");

                    if (AutoplayIsOn)
                    {
                        ToolBarImageClick(imgAutoplay, false);
                    }

                    int index = lvPlayList.SelectedIndex;

                    if (index > 0)
                    {
                        // Get the node to move
                        LinkedListNode<FileData> node1 = PlayList.GetNodeAt(index);
                        PlayList.RemoveAt(index);

                        // Get the node we want to move above
                        LinkedListNode<FileData> node2 = PlayList.GetNodeAt(index - 1);
                        PlayList.AddBefore(node2, node1);

                        lvPlayList.SelectedItem = node1.Value;
                        PlayListIsDirty = true;
                    }

                    break;
                }

                case PLAY_LIST_ONCE:
                {
                    // Turn on repeating
                    image.Tag = REPEAT;
                    image.Source = GetImage($"{image.Tag as string}{(mouseover ? MOUSEOVER : "")}.png");
                    RepeatIsOn = true;

                    break;
                }

                case RECYCLE:
                {
                    image.Source = GetImage($"{image.Tag as string}{(mouseover ? MOUSEOVER : "")}.png");

                    if (AutoplayIsOn)
                    {
                        ToolBarImageClick(imgAutoplay, false);
                    }

                    if (MediaIsPlaying)
                    {
                        ToolBarImageClick(imgStop, false);
                    }

                    if (MessageBox.Show(wdwMain, $"{(lvPlayList.SelectedItem as FileData)}\n" +
                        $"Are you sure you want to remove this?", RECYCLE, MessageBoxButton.YesNo,
                        MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
                    {
                        TheMediaPlayer.Close();
                        int index = lvPlayList.SelectedIndex;

                        PlayList.RemoveAt(index);

                        lvPlayList.SelectedItem = (lvPlayList.Items.Count > 0)
                            ? (index < PlayList.Count) ? PlayList[index] : PlayList[PlayList.Count - 1]
                            : null;

                        PlayListIsDirty = true;
                        UpdatePlayListHasMultipleItems();
                    }

                    break;
                }

                case REPEAT:
                {
                    // Turn off repeating
                    image.Tag = PLAY_LIST_ONCE;
                    image.Source = GetImage($"{image.Tag as string}{(mouseover ? MOUSEOVER : "")}.png");
                    RepeatIsOn = false;

                    break;
                }

                default:
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Sets the update play list has multiple items.
        /// </summary>
        /// <value>
        /// The update play list has multiple items.
        /// </value>
        private void UpdatePlayListHasMultipleItems() => PlayListHasMultipleItems = PlayList.Count > 1;

        /// <summary>
        /// Update the MainWindow title based on current settings.
        /// </summary>
        private void UpdateProgramTitle() =>
            wdwMain.Title = $"{PlayListName}{(PlayListIsDirty ? @"*" : @"")} | {PROGRAM_TITLE}";
    }
}