/*
 *  File Name:   MainWindow.Properties.cs
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
 * Date: 7/08/2021
 * ****************************************************************
 */

namespace Prog3AT2_One
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows.Media;
    using System.Windows.Threading;

    using Prog3AT2_One.Classes;

    /// <summary>
    /// Defines the <see cref="MainWindow" />.
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        /// <summary>
        ///The autoplay off button
        /// </summary>
        private const string AUTOPLAY_OFF = @"Autoplay-Off";

        /// <summary>
        ///The autoplay on button
        /// </summary>
        private const string AUTOPLAY_ON = @"Autoplay-On";

        /// <summary>
        /// The first button.
        /// </summary>
        private const string BUTTON_FIRST = @"Controls-First";

        /// <summary>
        /// The last button.
        /// </summary>
        private const string BUTTON_LAST = @"Controls-Last";

        /// <summary>
        /// The next button.
        /// </summary>
        private const string BUTTON_NEXT = @"Controls-Next";

        /// <summary>
        /// The pause button.
        /// </summary>
        private const string BUTTON_PAUSE = @"Controls-Pause";

        /// <summary>
        /// The play button.
        /// </summary>
        private const string BUTTON_PLAY = @"Controls-Play";

        /// <summary>
        /// The previous button.
        /// </summary>
        private const string BUTTON_PREVIOUS = @"Controls-Previous";

        /// <summary>
        /// The stop button.
        /// </summary>
        private const string BUTTON_STOP = @"Controls-Stop";

        /// <summary>
        /// The disabled
        /// </summary>
        private const string DISABLED = @"-disabled";

        /// <summary>
        /// Down
        /// </summary>
        private const string DOWN = @"-down";

        /// <summary>
        /// The edit list item
        /// </summary>
        private const string EDIT_LIST_ITEM = @"Edit";

        /// <summary>
        /// The mouseover text
        /// </summary>
        private const string MOUSEOVER = @"-mouseover";

        /// <summary>
        /// The move down
        /// </summary>
        private const string MOVE_DOWN = @"Down-Arrow";

        /// <summary>
        /// The move up
        /// </summary>
        private const string MOVE_UP = @"Up-Arrow";

        /// <summary>
        /// The pause tooltip
        /// </summary>
        private const string PAUSE_TOOLTIP = @"Pause audio playback";

        /// <summary>
        /// The play list once
        /// </summary>
        private const string PLAY_LIST_ONCE = @"Play-List-Once";

        /// <summary>
        /// The play tooltip
        /// </summary>
        private const string PLAY_TOOLTIP = @"Play audio";

        /// <summary>
        /// The default file extension for PlayLists.
        /// </summary>
        private const string PLAYLIST_EXTN = @".pld";

        /// <summary>
        /// The program's title.
        /// </summary>
        private const string PROGRAM_TITLE = @"Java3 AT2.1";

        /// <summary>
        /// The recycle
        /// </summary>
        private const string RECYCLE = @"Recycle";

        /// <summary>
        /// The repeat
        /// </summary>
        private const string REPEAT = @"Repeat";

        /// <summary>
        /// The audio file format currently supported.
        /// </summary>
        private const string SUPPORTED_EXTN = @".mp3";

        /// <summary>
        /// The default name for a new play list.
        /// </summary>
        private static readonly string DEFAULT_PLAYLIST_NAME = $"PlayList{PLAYLIST_EXTN}";

        /// <summary>
        /// The open file filter
        /// </summary>
        private static readonly string OPEN_FILE_FILTER =
            $"Audio files (*{SUPPORTED_EXTN})|*{SUPPORTED_EXTN}|" +
            $"Play List files (*{PLAYLIST_EXTN})|*{PLAYLIST_EXTN}|" +
            $"All files (*.*)|*.*";

        /// <summary>
        /// The save playlist filter
        /// </summary>
        private static readonly string SAVE_PLAYLIST_FILTER =
            $"Play List files (*{PLAYLIST_EXTN})|*{PLAYLIST_EXTN}|" +
            $"All files (*.*)|*.*";

        /// <summary>
        /// The dispatch timer.
        /// </summary>
        private readonly DispatcherTimer timer;

        /// <summary>
        /// The autoplay image/button is enabled.
        /// </summary>
        private bool _autoplayIsEnabled;

        /// <summary>
        /// Autoplay is on.
        /// </summary>
        private bool _autoplayIsOn;

        /// <summary>
        /// A list item is selected
        /// </summary>
        private bool _listItemSelected;

        /// <summary>
        /// The media is paused
        /// </summary>
        private bool _mediaIsPaused;

        /// <summary>
        /// The media is playing.
        /// </summary>
        private bool _mediaIsPlaying;

        /// <summary>
        /// The move down image/button is enabled
        /// </summary>
        private bool _moveDownIsEnabled;

        /// <summary>
        /// The move up image/button is enabled
        /// </summary>
        private bool _moveUpIsEnabled;

        /// <summary>
        /// The next track image/button is enabled.
        /// </summary>
        private bool _nextIsEnabled;

        /// <summary>
        /// The play track image/button is enabled.
        /// </summary>
        private bool _playIsEnabled;

        /// <summary>
        /// The PlayList.
        /// </summary>
        private ObservableLinkedList<FileData> _playList;

        /// <summary>
        /// The play list has multiple items
        /// </summary>
        private bool _playListHasMultipleItems;

        /// <summary>
        /// A value indicating whether the PlayList needs to be saved.
        /// </summary>
        private bool _playListIsDirty;

        /// <summary>
        /// A value indicating whether PlayList is new.
        /// Is the current Play List a New one?
        /// <para/>
        /// It is if it has not been loaded from, or saved to a file?.
        /// </summary>
        private bool _playListIsNew;

        /// <summary>
        /// The full path name of the playlist file.
        /// </summary>
        private string _playListName;

        /// <summary>
        /// The previous track image/button is enabled.
        /// </summary>
        private bool _previousIsEnabled;

        /// <summary>
        /// The repeat image/button is enabled.
        /// </summary>
        private bool _repeatIsEnabled;

        /// <summary>
        /// The repeat is on.
        /// </summary>
        private bool _repeatIsOn;

        /// <summary>
        /// The slider is dragging.
        /// </summary>
        private bool _sliderIsDragging;

        /// <summary>
        /// The slider is enabled.
        /// </summary>
        private bool _sliderIsEnabled;

        /// <summary>
        /// The stop track image/button is enabled.
        /// </summary>
        private bool _stopIsEnabled;

        /// <summary>
        /// The media player.
        /// </summary>
        private MediaPlayer _theMediaPlayer;

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets a value indicating whether [autoplay is enabled].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [autoplay is enabled]; otherwise, <c>false</c>.
        /// </value>
        public bool AutoplayIsEnabled
        {
            get => _autoplayIsEnabled;
            set
            {
                if (_autoplayIsEnabled != value)
                {
                    _autoplayIsEnabled = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [autoplay is on].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [autoplay is on]; otherwise, <c>false</c>.
        /// </value>
        public bool AutoplayIsOn
        {
            get => _autoplayIsOn;
            set
            {
                if (_autoplayIsOn != value)
                {
                    _autoplayIsOn = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [list item selected].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [list item selected]; otherwise, <c>false</c>.
        /// </value>
        public bool ListItemSelected
        {
            get => _listItemSelected;
            set
            {
                if (_listItemSelected != value)
                {
                    _listItemSelected = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [media is paused].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [media is paused]; otherwise, <c>false</c>.
        /// </value>
        public bool MediaIsPaused
        {
            get => _mediaIsPaused;
            set
            {
                _mediaIsPaused = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [media is playing].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [media is playing]; otherwise, <c>false</c>.
        /// </value>
        public bool MediaIsPlaying
        {
            get => _mediaIsPlaying;
            set
            {
                if (_mediaIsPlaying != value)
                {
                    _mediaIsPlaying = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [move down is enabled].
        /// </summary>
        /// <value><c>true</c> if [move up is enabled]; otherwise, <c>false</c>.</value>
        public bool MoveDownIsEnabled
        {
            get => _moveDownIsEnabled;
            set
            {
                if (_moveDownIsEnabled != value)
                {
                    _moveDownIsEnabled = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [move up is enabled].
        /// </summary>
        /// <value><c>true</c> if [move up is enabled]; otherwise, <c>false</c>.</value>
        public bool MoveUpIsEnabled
        {
            get => _moveUpIsEnabled;
            set
            {
                if (_moveUpIsEnabled != value)
                {
                    _moveUpIsEnabled = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [next is enabled].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [next is enabled]; otherwise, <c>false</c>.
        /// </value>
        public bool NextIsEnabled
        {
            get => _nextIsEnabled;
            set
            {
                if (_nextIsEnabled != value)
                {
                    _nextIsEnabled = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [play is enabled].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [play is enabled]; otherwise, <c>false</c>.
        /// </value>
        public bool PlayIsEnabled
        {
            get => _playIsEnabled;
            set
            {
                if (_playIsEnabled != value)
                {
                    _playIsEnabled = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the play list.
        /// </summary>
        /// <remarks>
        /// If PlayList is set to <c>null</c>, or has not yet been initialized,
        /// it will be set to a new empty <see cref="ObservableLinkedList{T}"/>
        /// the first time it is accessed after either of the aforementioned
        /// situations.
        /// <para/>
        /// Then the <see cref="PlayListIsNew"/> property will be set to <c>true</c>.
        /// </remarks>
        /// <value>
        /// The play list.
        /// </value>
        public ObservableLinkedList<FileData> PlayList
        {
            get
            {
                if (_playList == null)
                {
                    _playList = new();
                    OnPropertyChanged();
                    PlayListIsNew = true;
                }

                return _playList;
            }
            set
            {
                if (_playList != value)
                {
                    _playList = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [play list has multiple items].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [play list has multiple items]; otherwise, <c>false</c>.
        /// </value>
        public bool PlayListHasMultipleItems
        {
            get => _playListHasMultipleItems;
            set
            {
                if (_playListHasMultipleItems != value)
                {
                    _playListHasMultipleItems = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [play list is dirty].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [play list is dirty]; otherwise, <c>false</c>.
        /// </value>
        public bool PlayListIsDirty
        {
            get => _playListIsDirty;
            set
            {
                if (_playListIsDirty != value)
                {
                    _playListIsDirty = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [play list is new].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [play list is new]; otherwise, <c>false</c>.
        /// </value>
        public bool PlayListIsNew
        {
            get => _playListIsNew;
            set
            {
                _playListIsNew = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the name of the play list.
        /// </summary>
        /// <value>
        /// The name of the play list.
        /// </value>
        public string PlayListName
        {
            get => _playListName;
            set
            {
                _playListName = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [previous is enabled].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [previous is enabled]; otherwise, <c>false</c>.
        /// </value>
        public bool PreviousIsEnabled
        {
            get => _previousIsEnabled;

            set
            {
                if (_previousIsEnabled != value)
                {
                    _previousIsEnabled = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [repeat is enabled].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [repeat is enabled]; otherwise, <c>false</c>.
        /// </value>
        public bool RepeatIsEnabled
        {
            get => _repeatIsEnabled;
            set
            {
                if (_repeatIsEnabled != value)
                {
                    _repeatIsEnabled = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [repeat is on].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [repeat is on]; otherwise, <c>false</c>.
        /// </value>
        public bool RepeatIsOn
        {
            get => _repeatIsOn;
            set
            {
                if (_repeatIsOn != value)
                {
                    _repeatIsOn = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [slider is dragging].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [slider is dragging]; otherwise, <c>false</c>.
        /// </value>
        public bool SliderIsDragging
        {
            get => _sliderIsDragging;
            set
            {
                if (_sliderIsDragging != value)
                {
                    _sliderIsDragging = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [slider is enabled].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [slider is enabled]; otherwise, <c>false</c>.
        /// </value>
        public bool SliderIsEnabled
        {
            get => _sliderIsEnabled;
            set
            {
                if (_sliderIsEnabled != value)
                {
                    _sliderIsEnabled = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [stop is enabled].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [stop is enabled]; otherwise, <c>false</c>.
        /// </value>
        public bool StopIsEnabled
        {
            get => _stopIsEnabled;
            set
            {
                if (_stopIsEnabled != value)
                {
                    _stopIsEnabled = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the media player.
        /// </summary>
        /// <value>
        /// The media player.
        /// </value>
        public MediaPlayer TheMediaPlayer
        {
            get => _theMediaPlayer;
            set
            {
                _theMediaPlayer = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Called whenever a [property changed].
        /// </summary>
        /// <param name="name">The name of the property that changed.</param>
        private void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}