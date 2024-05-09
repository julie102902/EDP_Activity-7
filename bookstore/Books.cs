using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.IO; //needed packages
using OfficeOpenXml; //needed packages
using LicenseContext = OfficeOpenXml.LicenseContext; //EPP packages to manipulate the excel file
using OfficeOpenXml.Drawing.Chart; // for charts and graphs
using OfficeOpenXml.Style; // changes fonts, style, etc

namespace bookstore
{
    public partial class Books : Form
    {
        //Update
        //Merging
        // Declare private fields for database connection and command
        private MySqlConnection connection;
        private MySqlCommand command;
        // Database connection parameters
        private string server;
        private string database;
        private string uid;
        private string pass;
        private MySqlDataAdapter adapter;
        private DataTable dataTable;

        public Books()
        {
            InitializeComponent();
            InitializeDatabase(); // Initialize database connection
            LoadDataIntoDataGridView(); // Load data into DataGridView 
        }

        private void InitializeDatabase()
        {
            // Database connection parameters
            server = "localhost";
            database = "book_shop";
            uid = "root";
            pass = "erick";
            // Construct connection string
            string connectionString = $"SERVER={server};DATABASE={database};UID={uid};PASSWORD={pass};";
            connection = new MySqlConnection(connectionString); // Create MySqlConnection object named "connection"
        }

        private void LoadDataIntoDataGridView()
        {
            try
            {
                connection.Open();

                string query = "SELECT * FROM books";
                command = new MySqlCommand(query, connection);

                adapter = new MySqlDataAdapter(command);
                dataTable = new DataTable();
                adapter.Fill(dataTable);

                dataGridView1.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        // Redirect to Author form
        private void label2_Click(object sender, EventArgs e)
        {
            var author = new Author();
            author.Show();
            this.Close();
        }

        // Refresh current Books form
        private void label1_Click(object sender, EventArgs e)
        {
            var books = new Books();
            books.Show();
            this.Close();
        }

        // Event handler for DataGridView cell click (not implemented)
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        // Event handler for printing button click
        private void printbtn_Click(object sender, EventArgs e)
        {
            string filePath = @"C:\Users\avell\OneDrive\Desktop\Book Record.xlsx";

            try
            {
                connection.Open();

                string query = "SELECT * FROM books ";
                string graphquery = "SELECT Year, COUNT(ID) AS BookCount FROM books GROUP BY Year";

                using (MySqlCommand Command = new MySqlCommand(query, connection)) // command for books
                {
                    using (MySqlDataReader Reader = Command.ExecuteReader()) // reader for books
                    {
                        ExcelPackage excelPackage = new ExcelPackage(); // using the excel package

                        // Add a worksheet to the Excel package for Books data
                        ExcelWorksheet worksheet_1 = excelPackage.Workbook.Worksheets.Add("Books"); // creating first sheet

                        // Add a worksheet to the Excel package for Graphs data
                        ExcelWorksheet worksheet_2 = excelPackage.Workbook.Worksheets.Add("Graphs"); // second sheet

                        // Add logo to the first worksheet
                        var picture = worksheet_1.Drawings.AddPicture("Logo", new FileInfo("F:\\VisualStudio2022 Repository\\bookstore\\images\\Screenshot 2024-04-17 003346.png"));
                        picture.SetSize(70, 70); // Set the picture size in pixels

                        // Set the width of column 1 and height of row 1 for the first worksheet
                        worksheet_1.Column(1).Width = 13.71;
                        worksheet_1.Row(1).Height = 52.50;
                        worksheet_1.Cells[1, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet_1.Cells[1, 1].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#efe58b"));

                        // Calculate the offsets to center the picture in cell A1 for the first worksheet
                        double cellWidth = worksheet_1.Column(1).Width;
                        double cellHeight = worksheet_1.Row(1).Height;
                        double xOffset = cellWidth / 13.71; // Calculate horizontal offset
                        double yOffset = cellHeight / 4; // Calculate vertical offset
                        picture.SetPosition(0, (int)xOffset, 0, (int)yOffset); // Set the position of the picture to center it in cell A1

                        // Merging and adding the company name for the first worksheet
                        ExcelRange cellsToMerge = worksheet_1.Cells["B1:F1"];
                        cellsToMerge.Merge = true;
                        cellsToMerge.Value = "Books Information System";
                        cellsToMerge.Style.Font.Size = 16;
                        cellsToMerge.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        cellsToMerge.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#efe58b"));
                        cellsToMerge.Style.Font.Name = "Britannic Bold";
                        cellsToMerge.Style.Font.Color.SetColor(Color.Black);
                        cellsToMerge.Style.Font.Bold = true;
                        cellsToMerge.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cellsToMerge.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        // Merging and adding the "Available Books" label for the first worksheet
                        ExcelRange cellsToMerge1 = worksheet_1.Cells["B3:E3"];
                        cellsToMerge1.Merge = true;
                        cellsToMerge1.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        cellsToMerge1.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#efe58b"));
                        cellsToMerge1.Style.Font.Name = "Britannic Bold";
                        cellsToMerge1.Style.Font.Color.SetColor(Color.Black);
                        cellsToMerge1.Value = "Available Books";
                        cellsToMerge1.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cellsToMerge1.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        // Add logo to the second worksheet
                        var picture1 = worksheet_2.Drawings.AddPicture("Logo", new FileInfo("F:\\VisualStudio2022 Repository\\bookstore\\images\\Screenshot 2024-04-17 003346.png"));
                        picture1.SetSize(70, 70); // Set the picture size in pixels

                        // Set the width of column 1 and height of row 1 for the second worksheet
                        worksheet_2.Column(1).Width = 13.71;
                        worksheet_2.Row(1).Height = 52.50;
                        worksheet_2.Cells[1, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet_2.Cells[1, 1].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#efe58b"));

                        // Calculate the offsets to center the picture in cell A1 for the second worksheet
                        double cellWidths = worksheet_2.Column(1).Width;
                        double cellHeights = worksheet_2.Row(1).Height;
                        double xOffsets = cellWidths / 13.71;
                        double yOffsets = cellHeights / 4; // Calculate vertical offset
                        picture1.SetPosition(0, (int)xOffsets, 0, (int)yOffsets); // Set the position of the picture to center it in cell A1

                        // Merging and adding the company name for the second worksheet
                        ExcelRange cellsToMerge2 = worksheet_2.Cells["B1:F1"];
                        cellsToMerge2.Merge = true;
                        cellsToMerge2.Value = "Books Information System";
                        cellsToMerge2.Style.Font.Size = 16;
                        cellsToMerge2.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        cellsToMerge2.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#efe58b"));
                        cellsToMerge2.Style.Font.Name = "Britannic Bold";
                        cellsToMerge2.Style.Font.Color.SetColor(Color.Black);
                        cellsToMerge2.Style.Font.Bold = true;
                        cellsToMerge2.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cellsToMerge2.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        // Set headers for Book data in the first worksheet
                        worksheet_1.Cells["B4"].Value = "BookID";
                        worksheet_1.Cells["C4"].Value = "Title";
                        worksheet_1.Cells["D4"].Value = "Year";
                        worksheet_1.Cells["E4"].Value = "Price";

                        int row = 5;

                        // Populate book data into the first worksheet
                        while (Reader.Read())
                        {
                            worksheet_1.Cells[row, 2].Value = Reader.GetString(0);
                            worksheet_1.Cells[row, 3].Value = Reader.GetString(1);
                            worksheet_1.Cells[row, 4].Value = Reader.GetString(2);
                            worksheet_1.Cells[row, 5].Value = Reader.GetString(3);
                            row++;
                        }

                        // Add Librarian signature line in the first worksheet
                        worksheet_1.Cells[17, 4].Value = "_________________________";
                        worksheet_1.Cells[18, 4].Value = "Librarian";

                        worksheet_1.Cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet_1.Cells.AutoFitColumns();

                        Reader.Close();

                        using (MySqlCommand graphCommand = new MySqlCommand(graphquery, connection)) // command for fetching graph data
                        {
                            using (MySqlDataReader graphReader = graphCommand.ExecuteReader()) // reader to get the counts
                            {
                                int row1 = 5;

                                // Populate graph data into the second worksheet
                                while (graphReader.Read())
                                {
                                    worksheet_2.Cells[row1, 1].Value = graphReader.GetString("Year");
                                    worksheet_2.Cells[row1, 2].Value = graphReader.GetInt32("BookCount");
                                    row1++;
                                }

                                // Add a bar chart to visualize the book count per year in the second worksheet
                                var chart = worksheet_2.Drawings.AddChart("BookCountChart", eChartType.BarClustered);
                                chart.Title.Text = "Number of Books Stored per Year"; // Set the title of the chart
                                chart.Series.Add(worksheet_2.Cells["B1:B" + (row - 1)], worksheet_2.Cells["A1:A" + (row - 1)]); // Set the data range for the chart
                                chart.XAxis.Title.Text = "Year"; // Set axis titles
                                chart.YAxis.Title.Text = "Number of Books";
                                chart.SetPosition(2, 0, 0, 0); // Set the position of the chart
                                chart.SetSize(415, 300); // Set the size of the chart

                                // Add Librarian signature line in the second worksheet
                                worksheet_2.Cells[20, 5].Value = "_________________________";
                                worksheet_2.Cells[21, 5].Value = "Librarian";

                                worksheet_2.Cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                worksheet_2.Cells.AutoFitColumns();

                                // Save the Excel file
                                excelPackage.SaveAs(new FileInfo(filePath));
                                MessageBox.Show("Excel file saved successfully!");
                                excelPackage.Dispose();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
