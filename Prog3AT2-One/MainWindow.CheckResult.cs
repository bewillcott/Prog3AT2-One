/*
 *  File Name:   MainWindow.CheckResult.cs
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
    public partial class MainWindow
    {
        /// <summary>
        /// Result returned from the <see cref="CheckPlayListDirty"/> method.
        /// </summary>
        public enum CheckResult
        {
            /// <summary>
            /// PlayListDirty == <c>false</c>
            /// </summary>
            False,

            /// <summary>
            /// PlayListDirty == <c>true</c>
            /// </summary>
            True,

            /// <summary>
            /// PlayListDirty == <c>true</c> AND user selected [Cancel] button
            /// </summary>
            Cancel
        }
    }
}