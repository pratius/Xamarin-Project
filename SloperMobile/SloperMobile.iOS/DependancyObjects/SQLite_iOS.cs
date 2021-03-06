﻿using SloperMobile.Common.Constants;
using SloperMobile.Common.Interfaces;
using SloperMobile.iOS.DependancyObjects;
using SQLite.Net.Interop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;

[assembly: Dependency(typeof(SQLite_iOS))]
namespace SloperMobile.iOS.DependancyObjects
{
    public class SQLite_iOS : ISQLite
    {
        public SQLite_iOS()
        {
        }
        #region ISQLite implementation
        public SQLite.Net.SQLiteConnection GetConnection()
        {
            var sqliteFilename = AppSetting.APP_DBNAME;
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); // Documents folder
            string libraryPath = Path.Combine(documentsPath, "..", "Library"); // Library folder
            var path = Path.Combine(libraryPath, sqliteFilename);

            // This is where we copy in the prepopulated database
            Console.WriteLine(path);
            if (!File.Exists(path))
            {
                File.Create(path);
            }

            var plat = new SQLite.Net.Platform.XamarinIOS.SQLitePlatformIOS();
            var conn = new SQLite.Net.SQLiteConnection(plat, path, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.FullMutex, true);

            // Return the database connection 
            return conn;
        }
        #endregion
    }
}
