using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Xamarin.Forms;
using SloperMobile.Droid.DependancyObjects;
using SloperMobile.Common.Interfaces;
using SloperMobile.Common.Constants;
using SQLite.Net.Interop;

[assembly: Dependency(typeof(SQLite_Android))]
namespace SloperMobile.Droid.DependancyObjects
{
    class SQLite_Android : ISQLite
    {
        public SQLite_Android() { }
        #region ISQLite implementation
        public SQLite.Net.SQLiteConnection GetConnection()
        {
            var sqliteFilename = AppSetting.APP_DBNAME;
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); // Documents folder
            var path = Path.Combine(documentsPath, sqliteFilename);
            Console.WriteLine(path);
            if (!File.Exists(path)) File.Create(path);
            var plat = new SQLite.Net.Platform.XamarinAndroid.SQLitePlatformAndroidN();
            var conn = new SQLite.Net.SQLiteConnection(plat, path, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.FullMutex,true);
            // Return the database connection 
            return conn;
        }

        #endregion
    }
}