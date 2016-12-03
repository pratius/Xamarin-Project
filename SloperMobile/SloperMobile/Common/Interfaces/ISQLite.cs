using SQLite.Net;

namespace SloperMobile.Common.Interfaces
{
    public interface ISQLite
    {
        SQLiteConnection GetConnection();
        //void DeleteDataFile();
    }
}
