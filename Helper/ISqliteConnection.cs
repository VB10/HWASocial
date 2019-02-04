using SQLite;

namespace BoshokuDemo1.Helper
{
    public interface ISqliteConnection
    {
        SQLiteConnection GetConnection();
    }
}
