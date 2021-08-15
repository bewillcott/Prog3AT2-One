/*
 *  File Name:   FileData.cs
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

namespace Prog3AT2_One.Classes

{
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Defines the <see cref="FileData" />.
    /// </summary>
    [Serializable]
    [System.Runtime.InteropServices.Guid("CEE20963-878A-4934-90D9-A120B326116C")]
    public sealed class FileData : IComparable<FileData>, IEquatable<FileData>, INotifyPropertyChanged
    {
        /// <summary>
        /// The artist
        /// </summary>
        private string _artist;

        /// <summary>
        /// The duration
        /// </summary>
        private int _duration;

        /// <summary>
        /// The file name
        /// </summary>
        private string _fileName;

        /// <summary>
        /// The full path name
        /// </summary>
        private string _fullPathName;

        /// <summary>
        /// The title
        /// </summary>
        private string _title;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileData"/> class.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        public FileData(string filePath)
        {
            FullPathName = filePath;
            FileName = Path.GetFileName(FullPathName);
            Title = Path.GetFileNameWithoutExtension(FileName);
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="FileData"/> class from being created.
        /// </summary>
        private FileData()
        {
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the Artist.
        /// </summary>
        public string Artist
        {
            get => _artist;
            set
            {
                if (_artist != value)
                {
                    _artist = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the duration.
        /// </summary>
        public int Duration
        {
            get => _duration;
            set
            {
                if (_duration != value)
                {
                    _duration = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets the FileName.
        /// This is just the file name with extension.
        /// </summary>
        public string FileName
        {
            get => _fileName;
            set
            {
                if (_fileName == null)
                {
                    _fileName = value;
                }
            }
        }

        /// <summary>
        /// Gets the FullPathName.
        /// This is the full path to and including the file name and extension.
        /// </summary>
        public string FullPathName
        {
            get => _fullPathName;
            set
            {
                if (_fullPathName == null)
                {
                    _fullPathName = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the Title.
        /// </summary>
        public string Title
        {
            get => _title;
            set
            {
                if (_title != value)
                {
                    _title = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left <see cref="FileData"/> object.</param>
        /// <param name="right">The right <see cref="FileData"/> object.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(FileData left, FileData right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Implements the operator &lt;.
        /// </summary>
        /// <param name="left">The left <see cref="FileData"/> object.</param>
        /// <param name="right">The right <see cref="FileData"/> object.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator <(FileData left, FileData right) =>
            left is null ? right is not null : left.CompareTo(right) < 0;

        /// <summary>
        /// Implements the operator &lt;=.
        /// </summary>
        /// <param name="left">The left <see cref="FileData"/> object.</param>
        /// <param name="right">The right <see cref="FileData"/> object.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator <=(FileData left, FileData right) =>
            left is null || left.CompareTo(right) <= 0;

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left <see cref="FileData"/> object.</param>
        /// <param name="right">The right <see cref="FileData"/> object.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(FileData left, FileData right) =>
            left is null ? right is null : left.Equals(right);

        /// <summary>
        /// Implements the operator &gt;.
        /// </summary>
        /// <param name="left">The left <see cref="FileData"/> object.</param>
        /// <param name="right">The right <see cref="FileData"/> object.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator >(FileData left, FileData right) =>
            left is not null && left.CompareTo(right) > 0;

        /// <summary>
        /// Implements the operator &gt;=.
        /// </summary>
        /// <param name="left">The left <see cref="FileData"/> object.</param>
        /// <param name="right">The right <see cref="FileData"/> object.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator >=(FileData left, FileData right) =>
            left is null ? right is null : left.CompareTo(right) >= 0;

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer
        /// that indicates whether the current instance precedes, follows, or occurs in the same position
        /// in the sort order as the other object.
        /// <para/>
        /// <b>Note:</b><br/>
        /// This implementation of '<c>CompareTo(FileData)</c>' is <u>NOT</u> compatible with '<c>Equals(FileData)</c>'.
        /// They are working with different properties, for different uses.
        /// </summary>
        /// <param name="other">A <see cref="FileData"/> object to compare with this instance.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public int CompareTo(FileData other) =>
            Title.CompareTo(other.Title);

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">A <see cref="FileData"/> object to compare with this object.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool Equals(FileData other) =>
            other != null && FullPathName.Equals(other.FullPathName);

        /// <summary>
        /// Determines whether the specified <see cref="object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="object" /> to compare with this instance.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj is null)
            {
                return false;
            }

            return (obj is FileData fd) && Equals(fd);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>The <see cref="int"/>.</returns>
        public override int GetHashCode() => FullPathName.GetHashCode();

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        public override string ToString()
        {
            return "FileData[\n" +
                  $"  Artist       = {Artist}\n" +
                  $"  Title        = {Title}\n" +
                  $"  Duration     = {Duration}\n" +
                  $"  Filename     = {FileName}\n" +
                  $"  FullPathName = {FullPathName}\n" +
                  ']';
        }

        /// <summary>
        /// Called when [property changed].
        /// </summary>
        /// <param name="name">The name of the property that changed.</param>
        private void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}