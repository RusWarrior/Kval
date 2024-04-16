using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MySql.Data.MySqlClient;

namespace YogaCenter_by_Ovchinnikov;

public partial class bottom : Window
{
    
    private MySqlConnection _connection;
    private string connectionString = "server=localhost;port=3301;database=abd;user id=root;password=0000)";
    private List<users_pon> _pons;
    

    private List<filters> _filters;
    public bottom()
    {
        InitializeComponent();
        string sql = "SELECT * FROM Clients";
        ShowTable(sql);
        filter_user();
    }

    public class users_pon
    {
        public int id { get; set; }
        public string name { get; set; }
        public string last_name { get; set; }
        public int phone { get; set; }
        public DateTime year { get; set; }
        
        public int stage_level { get; set; }
        
        public int sale { get; set; }
    }

    public class filters
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    private void ShowTable(string sql)
    {
        _pons = new List<users_pon>();
        _connection = new MySqlConnection(connectionString);
        _connection.Open();
        MySqlCommand command = new MySqlCommand(sql, _connection);
        MySqlDataReader reader = command.ExecuteReader();
        
        while (reader.Read() && reader.HasRows)
        {
            var current = new users_pon()
            {
                id = reader.GetInt32("id"),
                name = reader.GetString("name"),
                last_name = reader.GetString("last_name"),
                phone = reader.GetInt32("phone"),
                year = reader.GetDateTime("year")
            };
            _pons.Add(current);
        }

        Grid.ItemsSource = _pons;
    }

    private void Add_OnClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            
            _connection = new MySqlConnection(connectionString);
            _connection.Open();
            string insert = "INSERT INTO Clients (name, last_name, phone, year, stage_level, sale) VALUES ('"+text2.Text+"', '"+text3.Text+"', '"+text4.Text+"', '"+text5.Text+"', '"+text6.Text+"', '"+text7.Text+"')";
            MySqlCommand command = new MySqlCommand(insert, _connection);
            command.ExecuteNonQuery();
            _connection.Close();
            text1.Text = "Succesfully data";
        }
        catch (Exception exception)
        {
            text1.Text = "Incorrect data";
        }
    }

    private void Update_OnClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            _connection = new MySqlConnection(connectionString);
            _connection.Open();
            string insert = "INSERT INTO Clients (name, last_name, phone, year, stage_level, sale) VALUES ('"+text2.Text+"', '"+text3.Text+"', '"+text4.Text+"', '"+text5.Text+"', '"+text6.Text+"', '"+text7.Text+"')";
            MySqlCommand command = new MySqlCommand(update, _connection);
            command.ExecuteNonQuery();
            _connection.Close();
            text1.Text = "Succesfully data";
        }
        catch (Exception exception)
        {
            text1.Text = "Incorrect data";
        }
    }

    private void Delete_OnClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            _connection = new MySqlConnection(connectionString);
            _connection.Open();
            string update = "DELETE FROM Clients WHERE id = '"+text1.Text+"'";
            MySqlCommand command = new MySqlCommand(update, _connection);
            command.ExecuteNonQuery();
            _connection.Close();
            text1.Text = "Succesfully data";
        }
        catch (Exception exception)
        {
            text1.Text = "Incorrect data";
        }
    }

    private void Save_OnClick(object? sender, RoutedEventArgs e)
    {
        _pons = new List<users_pon>();
        _connection = new MySqlConnection(connectionString);
        _connection.Open();
        string sql = "SELECT * FROM Clients";
        MySqlCommand command = new MySqlCommand(sql, _connection);
        MySqlDataReader reader = command.ExecuteReader();

        while (reader.Read() && reader.HasRows)
        {
            var current = new users_pon()
            {
                id = reader.GetInt32("id"),
                name = reader.GetString("name"),
                last_name = reader.GetString("last_name"),
                phone = reader.GetInt32("phone"),
                year = reader.GetDateTime("year"),
                stage_level = reader.GetInt32("stage_level"),
                sale = reader.GetInt32("sale"),
            };
            _pons.Add(current);
        }

        Grid.ItemsSource = _pons;
    }

    private void filter_user()
    {
        _filters = new List<filters>();
        _connection = new MySqlConnection(connectionString);
        _connection.Open();
        string sql = "SELECT id, name FROM Clients";
        MySqlCommand command = new MySqlCommand(sql, _connection);
        MySqlDataReader reader = command.ExecuteReader();

        while (reader.Read() && reader.HasRows)
        {
            var current = new filters()
            {
                stage_level = reader.GetInt32("stage_level"),
                name = reader.GetString("name"),
            };
            _filters.Add(current);
        }
        var combobox = this.Find<ComboBox>("Box");
        combobox.ItemsSource = _filters;
        _connection.Close();
        

    }

    private void Box_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        var combobox = (ComboBox)sender;
        var current = combobox.SelectedItem as filters;
        var filter = _pons.Where(x => x.name == current.name).ToList();
        Grid.ItemsSource = filter;
    }

    private void Search_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        string sql1 = "SELECT id, name, last_name, phone, year, stage_level, sale FROM Clients WHERE stage_level LIKE '%"+search.Text+"%'";
        ShowTable(sql1);
    }

    private void A_Z_OnClick(object? sender, RoutedEventArgs e)
    {
        string sql = "SELECT id, name, last_name, phone, year, stage_level, sale FROM Clients ORDER BY last_name desc";
        ShowTable(sql);
    }

    private void Back_OnClick(object? sender, RoutedEventArgs e)
    {
        MainWindow _mainWindow = new MainWindow();
        this.Hide();
        _mainWindow.Show();
    }
}