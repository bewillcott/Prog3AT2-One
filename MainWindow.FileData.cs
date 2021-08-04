/*
 *  File Name:   MainWindow.FileData.cs
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

    using Microsoft.Win32;

    /// <summary>
    /// Defines the <see cref="MainWindow" />.
    /// </summary>
    public partial class MainWindow
    {
        /// <summary>
        /// Defines the <see cref="FileData" />.
        /// </summary>
        [System.Runtime.InteropServices.Guid("CEE20963-878A-4934-90D9-A120B326116C")]
        public sealed class FileData : IComparable<FileData>, IEquatable<FileData>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="FileData"/> class.
            /// </summary>
            /// <param name="openFileDialog">The openFileDialog <see cref="OpenFileDialog"/>.</param>
            public FileData(OpenFileDialog openFileDialog)
            {
                FileName = openFileDialog.SafeFileName;
                FullPathName = openFileDialog.FileName;
                Title = FileName;
            }

            /// <summary>
            /// Prevents a default instance of the <see cref="FileData"/> class from being created.
            /// </summary>
            private FileData()
            {
            }

            /// <summary>
            /// Gets or sets the Artist.
            /// </summary>
            public string Artist { get; set; }

            /// <summary>
            /// Gets the Extension.
            /// </summary>
            public string Extension { get; private set; }

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

            /// <summary>
            /// Gets or sets the Title.
            /// </summary>
            public string Title { get; set; }

            /// <summary>
            /// Implements the operator !=.
            /// </summary>
            /// <param name="left">The left.</param>
            /// <param name="right">The right.</param>
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
            /// <param name="left">The left.</param>
            /// <param name="right">The right.</param>
            /// <returns>
            /// The result of the operator.
            /// </returns>
            public static bool operator <(FileData left, FileData right)
            {
                return ReferenceEquals(left, null) ? !ReferenceEquals(right, null) : left.CompareTo(right) < 0;
            }

            /// <summary>
            /// Implements the operator &lt;=.
            /// </summary>
            /// <param name="left">The left.</param>
            /// <param name="right">The right.</param>
            /// <returns>
            /// The result of the operator.
            /// </returns>
            public static bool operator <=(FileData left, FileData right)
            {
                return ReferenceEquals(left, null) || left.CompareTo(right) <= 0;
            }

            /// <summary>
            /// Implements the operator ==.
            /// </summary>
            /// <param name="left">The left.</param>
            /// <param name="right">The right.</param>
            /// <returns>
            /// The result of the operator.
            /// </returns>
            public static bool operator ==(FileData left, FileData right)
            {
                if (ReferenceEquals(left, null))
                {
                    return ReferenceEquals(right, null);
                }

                return left.Equals(right);
            }

            /// <summary>
            /// Implements the operator &gt;.
            /// </summary>
            /// <param name="left">The left.</param>
            /// <param name="right">The right.</param>
            /// <returns>
            /// The result of the operator.
            /// </returns>
            public static bool operator >(FileData left, FileData right)
            {
                return !ReferenceEquals(left, null) && left.CompareTo(right) > 0;
            }

            /// <summary>
            /// Implements the operator &gt;=.
            /// </summary>
            /// <param name="left">The left.</param>
            /// <param name="right">The right.</param>
            /// <returns>
            /// The result of the operator.
            /// </returns>
            public static bool operator >=(FileData left, FileData right)
            {
                return ReferenceEquals(left, null) ? ReferenceEquals(right, null) : left.CompareTo(right) >= 0;
            }

            /// <summary>
            /// Compares the current instance with another object of the same type and returns an integer
            /// that indicates whether the current instance precedes, follows, or occurs in the same position
            /// in the sort order as the other object.
            /// <para/>
            /// <b>Note:</b><br/>
            /// This implementation of 'CompareTo(FileData)' is <u>NOT</u> compatible with 'Equals(FileData)'.
            /// They are working with different properties, for different uses.
            /// </summary>
            /// <param name="other">An object to compare with this instance.</param>
            /// <returns>The <see cref="int"/>.</returns>
            public int CompareTo(FileData other)
            {
                return Title.CompareTo(other.Title);
            }

            /// <summary>
            /// Indicates whether the current object is equal to another object of the same type.
            /// </summary>
            /// <param name="other">An object to compare with this object.</param>
            /// <returns>The <see cref="bool"/>.</returns>
            public bool Equals(FileData other)
            {
                return FullPathName.Equals(other.FullPathName);
            }

            /// <summary>
            /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
            /// </summary>
            /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
            /// <returns>The <see cref="bool"/>.</returns>
            public override bool Equals(object obj)
            {
                if (ReferenceEquals(this, obj))
                {
                    return true;
                }

                if (ReferenceEquals(obj, null))
                {
                    return false;
                }

                return (obj is FileData fd) && Equals(fd);
            }

            /// <summary>
            /// Returns a hash code for this instance.
            /// </summary>
            /// <returns>The <see cref="int"/>.</returns>
            public override int GetHashCode()
            {
                return FullPathName.GetHashCode();
                ;
            }

            /// <summary>
            /// Converts to string.
            /// </summary>
            /// <returns>The <see cref="string"/>.</returns>
            public override string ToString()
            {
                return "FileData[\n" +
                      $"  Artist       = {Artist}\n" +
                      $"  Title        = {Title}\n" +
                      $"  Filename     = {FileName}\n" +
                      $"  FullPathName = {FullPathName}\n" +
                      ']';
            }
        }
    }
}