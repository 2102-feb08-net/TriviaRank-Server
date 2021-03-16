using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriviaServer.DAL;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;


namespace TriviaServer.Tests.Library
{
    public class TestTriviaGameContextFactory : IDisposable
    {
        private DbConnection _connection;
        private bool _disposedValue;

        private DbContextOptions<TriviaRankContext> CreateOptions()
        {
            return new DbContextOptionsBuilder<TriviaRankContext>()
                .UseSqlite(_connection)
                .Options;
        }

        public TriviaRankContext CreateContext()
        {
            if (_connection == null)
            {
                _connection = new SqliteConnection("DataSource=:memory:");
                _connection.Open();

                DbContextOptions<TriviaRankContext> options = CreateOptions();
                using var context = new TriviaRankContext(options);
                context.Database.EnsureCreated();

                // add extra test seed data here (or, in each test method)
            }

            return new TriviaRankContext(CreateOptions());
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _connection.Dispose();
                }

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
