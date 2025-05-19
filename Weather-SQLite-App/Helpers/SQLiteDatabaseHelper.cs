using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Weather_SQLite_App.Models;

namespace Weather_SQLite_App.Helpers
{
    public class SQLiteDatabaseHelper
    {
        readonly SQLiteAsyncConnection _connection;

        // Constructor
        public SQLiteDatabaseHelper(string path)
        {
            _connection = new SQLiteAsyncConnection(path);
            _connection.CreateTableAsync<Weather>().Wait();
        }

        // Create method
        public Task<int> Create(Weather product)
        {
            return _connection.InsertAsync(product);
        }

        // Delete method
        public Task<int> Delete(int id)
        {
            return _connection.Table<Weather>().DeleteAsync(i => i.Id == id);
        }

        // Getting all products
        public Task<List<Weather>> GetAll()
        {
            return _connection.Table<Weather>().ToListAsync();
        }

        // Search products
        public Task<List<Weather>> Search(string query)
        {
            string SQL = $"SELECT * FROM Weather WHERE Description LIKE '%{query}%' ";

            return _connection.QueryAsync<Weather>(SQL);
        }
    }
}
