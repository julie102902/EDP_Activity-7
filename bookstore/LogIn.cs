using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;


namespace bookstore
{
    public partial class LogIn : Form
    {
        public LogIn()
        {
            InitializeComponent();
        }

        private void username_TextChanged(object sender, EventArgs e)
        {

        }

        private void password_TextChanged(object sender, EventArgs e)
        {

        }

        private void loginbtn_Click(object sender, EventArgs e)
        {
            string username = this.username.Text;
            string password = this.password.Text;
            string accType = (loginbtn.Text == "LOGIN") ? "user" : "admin"; // Determine account type based on button text

            string connectionString = "Server=127.0.0.1;Port=3306;Database=book_shop;Uid=root;Pwd=erick;";
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    // Hash the entered password
                    string hashedPassword = HashPassword(password);
                    Console.WriteLine("Hashed password: " + hashedPassword); // Debugging statement

                    // Query to check username and password
                    string sql = "SELECT COUNT(*) FROM users WHERE username = @username AND password = @password AND acc_type = @accType";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", hashedPassword); // Use the hashed password for comparison
                    cmd.Parameters.AddWithValue("@accType", accType);

                    int count = Convert.ToInt32(cmd.ExecuteScalar());

                    if (count > 0)
                    {
                        // Set the status to 1 for the logged-in user
                        string updateSql = "UPDATE users SET status = 1 WHERE username = @username";
                        MySqlCommand updateCmd = new MySqlCommand(updateSql, conn);
                        updateCmd.Parameters.AddWithValue("@username", username);
                        updateCmd.ExecuteNonQuery();

                        // Redirect to appropriate dashboard based on account type
                        var books = new Books();
                        this.Hide();
                        books.Show();
                    }
                    else
                    {
                        MessageBox.Show("Invalid username/password");
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void create_acc_Click(object sender, EventArgs e)
        {

        }

        private void resetPass_Click(object sender, EventArgs e)
        {
            var resetPass  = new Resetpass();

            // Show the resetPass form
            resetPass.Show();

            // Optionally, you can hide the current form
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
    }
}
