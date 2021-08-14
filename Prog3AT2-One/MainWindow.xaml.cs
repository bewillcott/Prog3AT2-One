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
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using Microsoft.Win32;

    using Prog3AT2_One.Classes;

    /// <summary>
    /// Interaction logic for MainWindow.xaml.
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            ListView lv;

            // Initialize mediaplayer
            TheMediaPlayer = new();
            TheMediaPlayer.MediaEnded += MediaPlayer_MediaEnded;
            TheMediaPlayer.MediaFailed += MediaPlayer_MediaFailed;
            TheMediaPlayer.MediaOpened += MediaPlayer_MediaOpened;
            timer = new();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;

            PropertyChanged += MainWindow_PropertyChanged;
        }

        /// <summary>
        /// Handles the CanExecute event of the Close control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="CanExecuteRoutedEventArgs"/> instance containing the event data.</param>
        private void Close_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        /// <summary>
        /// Handles the Execute event of the Close control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ExecutedRoutedEventArgs"/> instance containing the event data.</param>
        private void Close_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Handles the Click event of the Help control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Help_Click(object sender, RoutedEventArgs e)
        {
            _ = new AboutDialog().ShowDialog();
        }

        /// <summary>
        /// Handles the IsEnabledChanged event of the Image control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private void Image_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Image image = sender as Image;

            switch (image.Tag as string)
            {
                case BUTTON_PAUSE:
                {
                    image.Tag = BUTTON_PLAY;
                    break;
                }

                case AUTOPLAY_ON:
                {
                    image.Tag = AUTOPLAY_OFF;
                    break;
                }

                case REPEAT:
                {
                    image.Tag = PLAY_LIST_ONCE;
                    break;
                }

                default:
                {
                    break;
                }
            }

            image.Source = (bool)e.NewValue
                ? GetImage($"{image.Tag as string}.png")
                : GetImage($"{image.Tag as string}{DISABLED}.png");
        }

        /// <summary>
        /// Handles the PropertyChanged event of the MainWindow control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The e<see cref="PropertyChangedEventArgs"/>.</param>
        private void MainWindow_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(AutoplayIsEnabled):
                {
                    if (!AutoplayIsEnabled)
                    {
                        AutoplayIsOn = false;
                        ListItemSelected = false;
                    }

                    break;
                }

                case nameof(AutoplayIsOn):
                {
                    if (AutoplayIsOn)
                    {
                        RepeatIsEnabled = true;

                        if (lvPlayList.SelectedIndex < 0)
                        {
                            lvPlayList.SelectedIndex = 0;
                        }
                        else if (!MediaIsPlaying)
                        {
                            ToolBarImageClick(imgPlay, false);
                        }
                    }
                    else
                    {
                        RepeatIsOn = false;
                        RepeatIsEnabled = false;
                    }

                    break;
                }

                case nameof(ListItemSelected):
                {
                    break;
                }

                case nameof(MediaIsPaused):
                {
                    if (MediaIsPaused)
                    {
                        TheMediaPlayer.Pause();
                    }
                    else
                    {
                        TheMediaPlayer.Play();
                    }

                    break;
                }

                case nameof(MediaIsPlaying):
                {
                    if (MediaIsPlaying)
                    {
                        timer.Start();
                        MediaIsPaused = false;
                    }
                    else
                    {
                        MediaIsPaused = true;
                        TheMediaPlayer.Stop();
                        timer.Stop();
                        slPosition.Value = 0.0;
                    }
                    break;
                }

                case nameof(NextIsEnabled):
                {
                    break;
                }

                case nameof(MoveDownIsEnabled):
                {
                    break;
                }

                case nameof(MoveUpIsEnabled):
                {
                    break;
                }

                case nameof(PlayIsEnabled):
                {
                    SliderIsEnabled = PlayIsEnabled;

                    break;
                }

                case nameof(PlayList):
                {
                    lvPlayList.ItemsSource = PlayList;

                    PlayListIsNew = PlayList.Count == 0;

                    break;
                }

                case nameof(PlayListIsDirty):
                {
                    UpdateProgramTitle();
                    break;
                }

                case nameof(PlayListIsNew):
                {
                    if (PlayListIsNew)
                    {
                        PlayListName = DEFAULT_PLAYLIST_NAME;
                    }

                    break;
                }

                case nameof(PlayListName):
                {
                    if (PlayListIsDirty)
                    {
                        PlayListIsDirty = false;
                    }
                    else
                    {
                        UpdateProgramTitle();
                    }

                    break;
                }

                case nameof(PreviousIsEnabled):
                {
                    break;
                }

                case nameof(RepeatIsEnabled):
                {
                    if (!RepeatIsEnabled)
                    {
                        RepeatIsOn = false;
                    }

                    break;
                }

                case nameof(RepeatIsOn):
                {
                    break;
                }

                case nameof(SliderIsDragging):
                {
                    if (SliderIsDragging)
                    {
                        if (MediaIsPlaying)
                        {
                            MediaIsPaused = true;
                        }
                    }
                    else
                    {
                        TheMediaPlayer.Position = TimeSpan.FromSeconds(slPosition.Value);

                        if (MediaIsPlaying)
                        {
                            MediaIsPaused = false;
                        }
                        else
                        {
                            ToolBarImageClick(imgPlay, false);
                        }
                    }

                    break;
                }

                case nameof(SliderIsEnabled):
                {
                    break;
                }

                case nameof(StopIsEnabled):
                {
                    break;
                }

                case nameof(TheMediaPlayer):
                {
                    TheMediaPlayer.MediaEnded += MediaPlayer_MediaEnded;
                    TheMediaPlayer.MediaFailed += MediaPlayer_MediaFailed;
                    TheMediaPlayer.MediaOpened += MediaPlayer_MediaOpened;

                    break;
                }

                default:
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Handles the MediaEnded event of the MediaPlayer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void MediaPlayer_MediaEnded(object sender, EventArgs e)
        {
            if (AutoplayIsOn)
            {
                // Not repeating, and at the end of the PlayList
                if (!RepeatIsOn && lvPlayList.SelectedIndex == lvPlayList.Items.Count - 1)
                {
                    ToolBarImageClick(imgStop, false);
                    return;
                }

                DelayAsync(1).Wait();
                ToolBarImageClick(imgNext, false);
            }
            else
            {
                ToolBarImageClick(imgStop, false);
            }
        }

        /// <summary>
        /// The MediaPlayer_MediaFailed.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="System.Windows.Media.ExceptionEventArgs"/>.</param>
        private void MediaPlayer_MediaFailed(object sender, System.Windows.Media.ExceptionEventArgs e)
        {
            MessageBox.Show(wdwMain, $"{e.ErrorException}", "Media Failed");
            TheMediaPlayer = new();
        }

        /// <summary>
        /// The MediaPlayer_MediaOpened.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="EventArgs"/>.</param>
        private void MediaPlayer_MediaOpened(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Handles the CanExecute event of the New menu control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="CanExecuteRoutedEventArgs"/> instance containing the event data.</param>
        private void New_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        /// <summary>
        /// Handles the Executed event of the New menu control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ExecutedRoutedEventArgs"/> instance containing the event data.</param>
        private void New_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            CheckResult result = CheckPlayListDirty(@"New Play List...");

            if (result != CheckResult.Cancel)
            {
                // Setup default empty play list ...
                PlayList = null;
                ClearMediaControls();
            }
        }

        /// <summary>
        /// Handles the CanExecute event of the Open menu control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="CanExecuteRoutedEventArgs"/> instance containing the event data.</param>
        private void Open_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        /// <summary>
        /// Handles the Executed event of the Open menu control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ExecutedRoutedEventArgs"/> instance containing the event data.</param>
        private void Open_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new();
            openFileDialog.Multiselect = false;
            openFileDialog.CheckFileExists = true;
            openFileDialog.CheckPathExists = true;
            openFileDialog.Filter = OPEN_FILE_FILTER;
            openFileDialog.Title = @"Select file to Open";

            if ((bool)openFileDialog.ShowDialog(wdwMain))
            {
                string filename = openFileDialog.FileName;
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

                    case SUPPORTED_EXTN:
                    {
                        AddAudioFilename(new(filename));
                        break;
                    }

                    default:
                    {
                        // Unsupported file type
                        break;
                    }
                }

                UpdatePlayListHasMultipleItems();
            }
        }

        /// <summary>
        /// Handles the DoubleClick event of the PlayList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        private void PlayList_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                ToolBarImageClick(imgEdit, false);
            }
            catch (ArgumentOutOfRangeException)
            {
                // Ignore: No item selected
            }
        }

        /// <summary>
        /// Handles the SelectionChanged event of the PlayList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SelectionChangedEventArgs"/> instance containing the event data.</param>
        private void PlayList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView listView = sender as ListView;
            int index = listView.SelectedIndex;
            int count = listView.Items.Count;
            FileData fileData = listView.SelectedItem as FileData;

            ListItemSelected = index > -1;
            MoveUpIsEnabled = index > 0;
            MoveDownIsEnabled = index >= 0 && index < count - 1;

            if (fileData != null)
            {
                OpenAudioFile(fileData);
            }
        }

        /// <summary>
        /// Handles the CanExecute event of the Save menu control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="CanExecuteRoutedEventArgs"/> instance containing the event data.</param>
        private void Save_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = PlayListIsDirty;
        }

        /// <summary>
        /// Handles the Executed event of the Save menu control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ExecutedRoutedEventArgs"/> instance containing the event data.</param>
        private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _ = SavePlayList();
        }

        /// <summary>
        /// Handles the DragCompleted event of the SlPosition slider control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.Primitives.DragCompletedEventArgs"/> instance containing the event data.</param>
        private void SlPosition_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            SliderIsDragging = false;
        }

        /// <summary>
        /// Handles the DragStarted event of the SlPosition slider control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.Primitives.DragStartedEventArgs"/> instance containing the event data.</param>
        private void SlPosition_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            SliderIsDragging = true;
        }

        /// <summary>
        /// Handles the ValueChanged event of the SlPosition slider control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedPropertyChangedEventArgs{System.Double}"/> instance containing the event data.</param>
        private void SlPosition_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            lblPosition.Content = TimeSpan.FromSeconds(slPosition.Value).ToString(@"mm\:ss");
        }

        /// <summary>
        /// Handles the Tick event of the timer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void Timer_Tick(object sender, EventArgs e)
        {
            if ((TheMediaPlayer.Source != null) && TheMediaPlayer.NaturalDuration.HasTimeSpan && (!SliderIsDragging))
            {
                slPosition.Value = TheMediaPlayer.Position.TotalSeconds;
            }
        }

        /// <summary>
        /// Handles the MouseDown event of the ToolBar Image controls.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        private void ToolBarImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Image image = sender as Image;
            image.Source = GetImage($"{image.Tag as string}{DOWN}.png");
        }

        /// <summary>
        /// Handles the MouseEnter event of the ToolBar Image controls.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private void ToolBarImage_MouseEnter(object sender, MouseEventArgs e)
        {
            Image image = sender as Image;
            image.Source = GetImage($"{image.Tag as string}{MOUSEOVER}.png");
        }

        /// <summary>
        /// Handles the MouseLeave event of the ToolBar Image controls.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private void ToolBarImage_MouseLeave(object sender, MouseEventArgs e)
        {
            Image image = sender as Image;
            string tag = image.Tag as string;
            bool disabled = false;

            switch (tag)
            {
                case BUTTON_STOP:
                {
                    disabled = !StopIsEnabled;
                    break;
                }

                case MOVE_DOWN:
                {
                    disabled = !MoveDownIsEnabled;
                    break;
                }

                case MOVE_UP:
                {
                    disabled = !MoveUpIsEnabled;
                    break;
                }

                default:
                {
                    break;
                }
            }

            image.Source = GetImage($"{tag}{(disabled ? DISABLED : "")}.png");
        }

        /// <summary>
        /// Handles the MouseUp event of the ToolBar Image controls.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        private void ToolBarImage_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ToolBarImageClick(sender as Image);
        }

        /// <summary>
        /// Handles the Closing event of the Window control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.CancelEventArgs"/> instance containing the event data.</param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //CheckResult result = CheckPlayListDirty(@"Program exiting.");
            e.Cancel = CheckPlayListDirty(@"Program exiting.") == CheckResult.Cancel;

            //switch (result)
            //{
            //    case CheckResult.False:
            //    {
            //        // TODO Reactivate this for production
            //        //if (MessageBox.Show(wdwMain, @"Are you sure?", @"Program exiting.", MessageBoxButton.YesNo,
            //        //           MessageBoxImage.Warning, MessageBoxResult.No) == MessageBoxResult.No)
            //        //{
            //        //    e.Cancel = true;
            //        //}

            //        break;
            //    }

            //    case CheckResult.True:
            //    {
            //        // Let it close.
            //        break;
            //    }

            //    case CheckResult.Cancel:
            //    {
            //        e.Cancel = true;
            //        break;
            //    }

            //    default:
            //    {
            //        break;
            //    }
            //}
        }
    }
}