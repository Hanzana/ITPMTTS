﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;

namespace TimeManagementSystem
{
    public partial class ManageSessionRooms : Form
    {
        SQLiteConnection connection;
        private int id = 0;
        List<int> sessionID = new List<int>();

        public ManageSessionRooms()
        {
            InitializeComponent();

            connection = new Classes.SqliteHelper().GetSQLiteConnection();


        }

        private void ManageSessionRooms_Load(object sender, EventArgs e)
        {
            try
            {
                loadSessions();

                loadRooms();

                lstBox.Items.Clear();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void loadSessions()
        {
            try
            {
                SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter("SELECT ID FROM Session", connection);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                cmbSelectSession.DataSource = dataTable;
                cmbSelectSession.DisplayMember = "ID";
                cmbSelectSession.ValueMember = "ID";

                cmbSelectSession.Text = "< - - - - Select Session - - - - >";
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void loadRooms()
        {
            try
            {
                SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter("SELECT Room FROM Location", connection);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                cmbSelectRoom.DataSource = dataTable;
                cmbSelectRoom.DisplayMember = "Room";
                cmbSelectRoom.ValueMember = "Room";
                cmbSelectRoom.Text = "< - - - - Select Room - - - - >";
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void clear()
        {
            try
            {
                cmbSelectSession.ResetText();
                cmbSelectRoom.ResetText();
                lstBox.Items.Clear();
                sessionID.Clear();
                cmbSelectSession.Text = "< - - - - Select Session - - - - >";
                cmbSelectRoom.Text = "< - - - - Select Room - - - - >";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cmbSelectSession_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (id==0)
                {
                    lstBox.Items.Clear();
                    id++;
                }
                else
                {
                    id = int.Parse(this.cmbSelectSession.Text);

                    sessionID.Add(id);

                    SQLiteCommand command1 = new SQLiteCommand("SELECT * FROM Session WHERE ID='" + id + "';", connection);
                    connection.Open();

                    SQLiteDataReader read = command1.ExecuteReader();

                    while (read.Read())
                    {
                        
                        string lecture1 = read["Lecture1"].ToString();
                        string lecture2 = read["Lecture2"].ToString();
                        string subCode = read["SubjectCode"].ToString();
                        string subName = read["SubjectName"].ToString();
                        string grpID = read["GroupID"].ToString();
                        string tag = read["Tag"].ToString();
                        int stCount = Convert.ToInt32(read["StudentCount"].ToString());
                        int duration = Convert.ToInt32(read["Duration"].ToString());
                        string room = read["Room"].ToString();

                        lstBox.Items.Add("Lecturer 1 - " + lecture1);
                        lstBox.Items.Add("Lecturer 2 - " + lecture2);
                        lstBox.Items.Add("Subject Code - " + subCode);
                        lstBox.Items.Add("Subject Name - " + subName);
                        lstBox.Items.Add("Group ID - " + grpID);
                        lstBox.Items.Add("Tag - " + tag);
                        lstBox.Items.Add("Student Count - " + stCount);
                        lstBox.Items.Add("Duration - " + duration);
                        lstBox.Items.Add("Room - " + room);
                        lstBox.Items.Add("\n");
                    }
                    read.Close();
                    connection.Close();

                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if(lstBox.Items.Count == 0)
                {
                    MessageBox.Show("Please Select a Session(s)!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    connection.Open();

                    for (int x = 1; x < sessionID.Count; x++)
                    {
                        SQLiteCommand command = new SQLiteCommand("UPDATE Session SET Room='" + cmbSelectRoom.Text + "' WHERE ID='" + sessionID[x] + "';", connection);

                        int i = command.ExecuteNonQuery();
                        if( x == sessionID.Count - 1)
                        {
                            if (i != 0)
                            {
                                clear();
                                connection.Close();
                                sessionID.Clear();
                                MessageBox.Show("Data Updated", "Done!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                AddRoom frm1 = new AddRoom();
                                frm1.Show();
                                this.Hide();
                            }
                            else
                            {
                                connection.Close();
                                sessionID.Clear();
                                MessageBox.Show("Data Not Updated", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                       
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void lstBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cmbSelectRoom_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
