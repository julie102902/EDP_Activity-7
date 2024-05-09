using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bookstore
{
    public partial class Resetpass : Form
    {
        public Resetpass()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text;
            string password = textBox2.Text;

            // Validate input
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter both username and new password.");
                return;
            }

            // Hash the password
            string hashedPassword = HashPassword(password);

            // Validate password strength (e.g., length, complexity)

            string myConnectionString = "server=127.0.0.1;uid=root;pwd=erick;database=book_shop";
            MySqlConnection conn = null;
            try
            {
                conn = new MySqlConnection(myConnectionString);
                conn.Open();

                // Check if the username exists
                string sql = "SELECT COUNT(*) FROM users WHERE username = @username";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    if (count == 0)
                    {
                        MessageBox.Show("Username not found.");
                        return;
                    }
                }

                // Update the password
                sql = "UPDATE users SET password = @password WHERE username = @username";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@password", hashedPassword);
                    cmd.Parameters.AddWithValue("@username", username);
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Password reset successful.");
                        // Clear textboxes
                        textBox1.Clear();
                        textBox2.Clear();
                    }
                    else
                    {
                        MessageBox.Show("Password reset failed.");
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close(); // Close the connection
                    conn.Dispose(); // Dispose of the connection object
                }
            }

            // Return to login form
            var login = new LogIn();
            login.Show();
            this.Hide();
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array, which we can convert to a string
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Convert byte array to a string
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

    }
}

