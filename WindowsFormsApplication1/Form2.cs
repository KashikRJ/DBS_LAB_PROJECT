using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace WindowsFormsApplication1
{
    public partial class Form2 : Form
    {
        OracleDataAdapter da, da1, da2;
        DataTable dt, dt1, dt2;
        OracleConnection conn;
        public void ConnectDB()
        {
            conn = new OracleConnection("DATA SOURCE= 192.168.56.1:1521/XE;USER ID=SYSTEM;PASSWORD=student");
            try
            {
                conn.Open();
                MessageBox.Show("Connected");
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DataLoad();
        }
        string userId;
        public Form2(string id)
        {
            userId = id;
            InitializeComponent();
            ConnectDB();
            InitializeDataGridView();
            dataGridView1.CellContentClick -= DataGridView1_CellContentClick; // Remove existing handlers to avoid duplicates
            dataGridView1.CellContentClick += DataGridView1_CellContentClick; // Add the event handler
           
            Genres.SelectedIndexChanged += new EventHandler(Genres_SelectedIndexChanged);
            dataGridView1.CellFormatting += DataGridView1_CellFormatting;
            Artists.SelectedIndexChanged += new EventHandler(Artists_SelectedIndexChanged);
            Albums.SelectedIndexChanged += new EventHandler(Albums_SelectedIndexChanged);

        }
        private void LoadThumbnail()
        {
            try
            {   string path= "C:\\songs\\AF.png";
                pictureBox1.Image = Image.FromFile(path);
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;

                //pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading thumbnail: " + ex.Message);
            }
        }
        private void InitializeDataGridView()
        {
            // Add Song ID and Song Name columns
            dataGridView1.Columns.Add("Song ID", "Song ID");
            dataGridView1.Columns["Song ID"].DataPropertyName = "song_id";
            dataGridView1.Columns["Song ID"].Width = 50; // Adjust the width as needed
            dataGridView1.Columns.Add("Song Name", "Song Name");
            dataGridView1.Columns["Song Name"].DataPropertyName = "song_name";
            dataGridView1.Columns["Song Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill; // Make the Song Name column fill the remaining space
            // Add Like button column
            DataGridViewButtonColumn likeButton = new DataGridViewButtonColumn();
            likeButton.Name = "LikeButton";
            likeButton.Text = "Like";
            likeButton.UseColumnTextForButtonValue = true;
            dataGridView1.Columns.Add(likeButton);
            dataGridView1.Columns["LikeButton"].Width = 100;
            // Add Play button column
            DataGridViewButtonColumn playButton = new DataGridViewButtonColumn();
            playButton.Name = "PlayButton";
            playButton.Text = "Play";
            playButton.UseColumnTextForButtonValue = true;
            dataGridView1.Columns.Add(playButton);
            dataGridView1.Columns["PlayButton"].Width = 100;
        }

        private void DataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name == "LikeButton" && e.RowIndex >= 0)
            {
                var cellValue = dataGridView1.Rows[e.RowIndex].Cells["Song ID"].Value;

                if (cellValue != null)
                {
                    string songId = cellValue.ToString();
                    DataGridViewButtonCell buttonCell = (DataGridViewButtonCell)dataGridView1.Rows[e.RowIndex].Cells["LikeButton"];

                    if (IsSongAlreadyLiked(songId, userId))
                    {
                        buttonCell.UseColumnTextForButtonValue = false;
                        buttonCell.Value = "Liked";
                        buttonCell.FlatStyle = FlatStyle.Flat;
                        buttonCell.Style.BackColor = Color.LightGray;
                        buttonCell.ReadOnly = true;
                    }
                    else
                    {
                        buttonCell.UseColumnTextForButtonValue = true;
                        buttonCell.Value = "Like";
                        buttonCell.FlatStyle = FlatStyle.Standard;
                        buttonCell.Style.BackColor = Color.White;
                        buttonCell.ReadOnly = false;
                    }
                }
            }
        }

        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView1.Columns["PlayButton"].Index && e.RowIndex >= 0)
            {
                var cellValue = dataGridView1.Rows[e.RowIndex].Cells["Song ID"].Value;

                if (cellValue != null)
                {
                    string songId = cellValue.ToString();
                    PlaySong(songId);
                }
            }
            else if (e.ColumnIndex == dataGridView1.Columns["LikeButton"].Index && e.RowIndex >= 0)
            {
                var cellValue = dataGridView1.Rows[e.RowIndex].Cells["Song ID"].Value;

                if (cellValue != null)
                {
                    string songId = cellValue.ToString();
                    DataGridViewButtonCell buttonCell = dataGridView1.Rows[e.RowIndex].Cells["LikeButton"] as DataGridViewButtonCell;

                    if (buttonCell != null)
                    {
                        if (buttonCell.Value != null && buttonCell.Value.ToString() == "Like")
                        {
                            InsertIntoFavouritesTable(songId);
                            buttonCell.Value = "Liked";
                            UpdateButtonStyle(buttonCell, true);
                        }
                        else
                        {
                            RemoveFromFavouritesTable(songId);
                            buttonCell.Value = "Like";
                            UpdateButtonStyle(buttonCell, false);
                        }

                        dataGridView1.RefreshEdit();
                        dataGridView1.InvalidateRow(e.RowIndex);
                    }
                }
            }
        }


        private void button3_Click(object sender, EventArgs e)
        {
            Form3 frm = new Form3(userId);
            frm.Show();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void Albums_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Albums.SelectedItem != null)
            {
                string selectedAlbum = Albums.GetItemText(Albums.SelectedItem);
                LoadSongsForAlbum(selectedAlbum);
            }
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            DataLoad();
            LoadThumbnail();
        }
        private void Genres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Genres.SelectedItem != null)
            {
                string selectedGenre = Genres.GetItemText(Genres.SelectedItem);
                LoadSongsForGenre(selectedGenre);
            }
        }
        private void Artists_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Artists.SelectedItem != null)
            {
                string selectedArtists = Artists.GetItemText(Artists.SelectedItem);
                LoadSongsForArtists(selectedArtists);
            }
        }
        /*private void LoadSongsForAlbum(string AlbumName)
        {
            string query3 = @"
        SELECT s.song_id, s.song_name 
        FROM songs s 
        JOIN albums a ON s.album_id = a.album_id
        WHERE LOWER(a.album_name) = LOWER(:AlbumName)";

            using (OracleCommand command = new OracleCommand(query3, conn))
            {
                command.Parameters.Add("AlbumName", OracleDbType.Varchar2).Value = AlbumName;
                using (OracleDataAdapter da = new OracleDataAdapter(command))
                {
                    DataTable newDt = new DataTable();
                    da.Fill(newDt);
                    dataGridView1.DataSource = newDt; // Set the DataGridView's DataSource to the DataTable
                }
            }
        }
        private void LoadSongsForGenre(string genreName)
        {
            string query = @"
        SELECT s.song_id, s.song_name 
        FROM songs s 
        JOIN type t ON s.song_id = t.song_id 
        JOIN genre g ON t.genre_id = g.genre_id 
        WHERE LOWER(g.genre_name) = LOWER(:genreName)";

            using (OracleCommand command = new OracleCommand(query, conn))
            {
                command.Parameters.Add("genreName", OracleDbType.Varchar2).Value = genreName;
                using (OracleDataAdapter da = new OracleDataAdapter(command))
                {
                    DataTable newDt = new DataTable();
                    da.Fill(newDt);
                    dataGridView1.DataSource = newDt; // Set the DataGridView's DataSource to the DataTable
                }
            }
        }
        private void LoadSongsForArtists(string ArtistsName)
        {
            string query2 = @"
        SELECT s.song_id, s.song_name 
        FROM songs s 
        JOIN artists a ON s.artist_id = a.artist_id
        WHERE LOWER(a.artist_name) = LOWER(:ArtistsName)";

            using (OracleCommand command = new OracleCommand(query2, conn))
            {
                command.Parameters.Add("ArtistsName", OracleDbType.Varchar2).Value = ArtistsName;
                using (OracleDataAdapter da = new OracleDataAdapter(command))
                {
                    DataTable newDt = new DataTable();
                    da.Fill(newDt);
                    dataGridView1.DataSource = newDt; // Set the DataGridView's DataSource to the DataTable
                }
            }
        }*/
        private void LoadSongsForAlbum(string AlbumName)
        {
            using (OracleCommand command = new OracleCommand("GetSongsByAlbum", conn))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("p_albumName", OracleDbType.Varchar2).Value = AlbumName;
                command.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                using (OracleDataAdapter da = new OracleDataAdapter(command))
                {
                    DataTable newDt = new DataTable();
                    da.Fill(newDt);
                    dataGridView1.DataSource = newDt;
                }
            }
        }
        private void LoadSongsForGenre(string genreName)
        {
            using (OracleCommand command = new OracleCommand("GetSongsByGenre", conn))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("p_genreName", OracleDbType.Varchar2).Value = genreName;
                command.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                using (OracleDataAdapter da = new OracleDataAdapter(command))
                {
                    DataTable newDt = new DataTable();
                    da.Fill(newDt);
                    dataGridView1.DataSource = newDt;
                }
            }
        }
        private void LoadSongsForArtists(string ArtistsName)
        {
            using (OracleCommand command = new OracleCommand("GetSongsByArtist", conn))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("p_ArtistsName", OracleDbType.Varchar2).Value = ArtistsName;
                command.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                using (OracleDataAdapter da = new OracleDataAdapter(command))
                {
                    DataTable newDt = new DataTable();
                    da.Fill(newDt);
                    dataGridView1.DataSource = newDt;
                }
            }
        }



        private void DataLoad()
        {
            // Load genres liked by the user
            string genreQuery = "SELECT genre_name FROM genre WHERE genre_id IN (SELECT genre_id FROM likes WHERE user_id = " + userId + ")";
            LoadListBoxData(genreQuery, Genres, "genre_name");

            // Load artists listened to by the user
            string artistQuery = "SELECT artist_name FROM artists WHERE artist_id IN (SELECT artist_id FROM listens WHERE user_id = " + userId + ")";
            LoadListBoxData(artistQuery, Artists, "artist_name");

            // Load albums listened to by the user
            string albumQuery = "SELECT album_name FROM albums WHERE artist_id IN (SELECT artist_id FROM listens WHERE user_id = " + userId + ")";
            LoadListBoxData(albumQuery, Albums, "album_name");

            // Load songs based on user's liked genres and artists
            string songQuery = @"
        SELECT s.song_id, s.song_name
        FROM songs s
        JOIN type t ON s.song_id = t.song_id
        WHERE t.genre_id IN (SELECT genre_id FROM likes WHERE user_id = " + userId + @")
        OR s.artist_id IN (SELECT artist_id FROM works_in WHERE genre_id IN (SELECT genre_id FROM likes WHERE user_id = " + userId + "))";
            LoadDataGridViewData(songQuery);

          


        }

        private void LoadListBoxData(string query, ListBox listBox, string displayMember)
        {
            using (OracleDataAdapter adapter = new OracleDataAdapter(query, conn))
            {
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                listBox.DataSource = dataTable;
                listBox.DisplayMember = displayMember;
            }
        }

        private void LoadDataGridViewData(string query)
        {
            using (OracleDataAdapter adapter = new OracleDataAdapter(query, conn))
            {
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                dataGridView1.DataSource = dataTable;
            }
        }
       
        private NAudio.Wave.WaveOutEvent outputDevice;
        private NAudio.Wave.AudioFileReader audioFile;

        private void PlaySong(string songId)
        {

            string audioFilePath = GetAudioFilePath(songId);
            string thumbnailPath = GetThumbnailPath(songId); // Implement this method similar to GetAudioFilePath
            if (!string.IsNullOrEmpty(audioFilePath))
            {
                AudioPlayerForm audioPlayerForm = new AudioPlayerForm(audioFilePath, thumbnailPath);
                audioPlayerForm.ShowDialog(); // This opens the form as a modal dialog
            }
        }
        private void OnPlaybackStopped(object sender, NAudio.Wave.StoppedEventArgs args)
        {
            // Cleanup resources after playback is stopped
            if (outputDevice != null)
            {
                outputDevice.Dispose();
                outputDevice = null;
            }
            if (audioFile != null)
            {
                audioFile.Dispose();
                audioFile = null;
            }
        }

        private string GetAudioFilePath(string songId)
        {
            string query = "SELECT audio_file_path FROM songs WHERE song_id = :songId";
            string audioFilePath = "";

            using (OracleCommand command = new OracleCommand(query, conn))
            {
                command.Parameters.Add(":songId", OracleDbType.Varchar2).Value = songId;

                using (OracleDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        audioFilePath = reader["audio_file_path"].ToString();
                    }
                }
            }

            return audioFilePath;
        }
        private string GetThumbnailPath(string songId)
        {
            string query = "SELECT thumbnail_path FROM songs WHERE song_id = :songId";
            string thumbnailPath = "";

            using (OracleCommand command = new OracleCommand(query, conn))
            {
                command.Parameters.Add(":songId", OracleDbType.Varchar2).Value = songId;

                using (OracleDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        thumbnailPath = reader["thumbnail_path"].ToString();
                    }
                }
            }

            return thumbnailPath;
        }
        private void InsertIntoFavouritesTable(string songId)
        {
            try
            {
                if (!IsSongAlreadyLiked(songId, userId))
                {
                    string insertQuery = "INSERT INTO favourites (user_id, song_id) VALUES (:userId, :songId)";

                    using (OracleCommand command = new OracleCommand(insertQuery, conn))
                    {
                        command.Parameters.Add(":userId", OracleDbType.Varchar2).Value = userId;
                        command.Parameters.Add(":songId", OracleDbType.Varchar2).Value = songId;
                        command.ExecuteNonQuery();
                    }

                    // Update song likes
                    UpdateSongLikes(songId);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error inserting into Favourites table: " + ex.Message);
            }
        }
        private bool IsSongAlreadyLiked(string songId, string userId)
        {
            string query = "SELECT COUNT(*) FROM favourites WHERE user_id = :userId AND song_id = :songId";
            using (OracleCommand command = new OracleCommand(query, conn))
            {
                command.Parameters.Add(":userId", OracleDbType.Varchar2).Value = userId;
                command.Parameters.Add(":songId", OracleDbType.Varchar2).Value = songId;
                int count = Convert.ToInt32(command.ExecuteScalar());
                return count > 0;
            }
        }

        private void UpdateSongLikes(string songId)
        {
            string updateQuery = "UPDATE songs SET song_likes = song_likes + 1 WHERE song_id = :songId";
            using (OracleCommand command = new OracleCommand(updateQuery, conn))
            {
                command.Parameters.Add(":songId", OracleDbType.Varchar2).Value = songId;
                command.ExecuteNonQuery();
            }
        }
        private void RemoveFromFavouritesTable(string songId)
        {
            try
            {
                string deleteQuery = "DELETE FROM favourites WHERE user_id = :userId AND song_id = :songId";
                using (OracleCommand command = new OracleCommand(deleteQuery, conn))
                {
                    command.Parameters.Add(":userId", OracleDbType.Varchar2).Value = userId;
                    command.Parameters.Add(":songId", OracleDbType.Varchar2).Value = songId;
                    command.ExecuteNonQuery();
                }

                // Decrement song likes
                DecrementSongLikes(songId);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error removing from Favourites table: " + ex.Message);
            }
        }
        private void DecrementSongLikes(string songId)
        {
            string updateQuery = "UPDATE songs SET song_likes = song_likes - 1 WHERE song_id = :songId";
            using (OracleCommand command = new OracleCommand(updateQuery, conn))
            {
                command.Parameters.Add(":songId", OracleDbType.Varchar2).Value = songId;
                command.ExecuteNonQuery();
            }
        }
        private void UpdateButtonStyle(DataGridViewButtonCell buttonCell, bool isLiked)
        {
            if (isLiked)
            {
                buttonCell.FlatStyle = FlatStyle.Flat;
                buttonCell.Style.BackColor = Color.LightGray;
                buttonCell.ReadOnly = true;
            }
            else
            {
                buttonCell.FlatStyle = FlatStyle.Standard;
                buttonCell.Style.BackColor = Color.White;
                buttonCell.ReadOnly = false;
            }
        }

        private void Albums_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void Artists_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string searchText = textBox1.Text.ToLower();

            LoadFilteredArtists(searchText);
            LoadFilteredGenres(searchText);
            LoadFilteredSongs(searchText);
            LoadFilteredAlbums(searchText);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Login f2= new Login();
            f2.Show();
            this.Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void LoadFilteredArtists(string searchText)
        {
            string query = "SELECT artist_name FROM artists WHERE LOWER(artist_name) LIKE '%" + searchText + "%'";
            OracleDataAdapter da = new OracleDataAdapter(query, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);

            Artists.DataSource = dt;
            Artists.DisplayMember = "artist_name";
        }
        private void LoadFilteredAlbums(string searchText)
        {
            string query = "SELECT album_name FROM albums WHERE LOWER(album_name) LIKE '%" + searchText + "%'";
            OracleDataAdapter da = new OracleDataAdapter(query, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);

            Albums.DataSource = dt;
            Albums.DisplayMember = "album_name";
        }

        private void LoadFilteredGenres(string searchText)
        {
            string query = "SELECT genre_name FROM genre WHERE LOWER(genre_name) LIKE '%" + searchText + "%'";
            OracleDataAdapter da = new OracleDataAdapter(query, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);

            Genres.DataSource = dt;
            Genres.DisplayMember = "genre_name";
        }

        private void LoadFilteredSongs(string searchText)
        {
            string query = "SELECT song_id, song_name FROM songs WHERE LOWER(song_name) LIKE '%" + searchText + "%'";
            OracleDataAdapter da = new OracleDataAdapter(query, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);

            dataGridView1.DataSource = dt; // Set the DataGridView's DataSource to the DataTable
        }


    }
}
