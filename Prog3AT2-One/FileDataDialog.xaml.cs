/*
 *  File Name:   FileDataDialog.xaml.cs
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
 * Date: 12/08/2021
 * ****************************************************************
 */

namespace Prog3AT2_One
{
    using System;
    using System.Windows;

    using Prog3AT2_One.Classes;

    /// <summary>
    /// Interaction logic for FileDataDialog.xaml.
    /// </summary>
    public partial class FileDataDialog : Window
    {
        /// <summary>
        /// The file data.
        /// </summary>
        private readonly FileData fileData;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileDataDialog"/> class.
        /// </summary>
        /// <param name="fileData">The file data.</param>
        public FileDataDialog(FileData fileData)
        {
            this.fileData = fileData;
            Artist = fileData.Artist;
            AudioTitle = fileData.Title;

            InitializeComponent();
            DataContext = this;
        }

        /// <summary>
        /// Gets or sets the Artist.
        /// </summary>
        public string Artist { get; set; }

        /// <summary>
        /// Gets the Duration.
        /// </summary>
        public string Duration { get => TimeSpan.FromSeconds(fileData.Duration).ToString(@"mm\:ss"); }

        /// <summary>
        /// Gets the FileName
        /// Gets or sets the name of the file..
        /// </summary>
        public string FileName { get => fileData.FileName; }

        /// <summary>
        /// Gets the full name of the path..
        /// </summary>
        public string FullPathName { get => fileData.FullPathName; }

        /// <summary>
        /// Gets or sets the Title.
        /// </summary>
        public string AudioTitle { get; set; }

        /// <summary>
        /// Handles the Click event of the BtnDialogOk control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void BtnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            fileData.Artist = txtArtist.Text;
            fileData.Title = txtTitle.Text;
            DialogResult = true;
        }
    }
}