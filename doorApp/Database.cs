//using SQLite;
//using SQLiteDB.Resources.Model;
//using System.Collections.Generic;
//using System.Linq;
//namespace SQLiteDB.Resources.Helper
//{
//    public class Database
//    {
//        string folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
//        public bool createDatabase()
//        {
//            try
//            {
//                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "tDevices.db")))
//                {
//                    connection.CreateTable<tDevice>();
//                    return true;
//                }
//            }
//            catch (SQLiteException ex)
//            {
//                //Log.Info("SQLiteEx", ex.Message);
//                return false;
//            }
//        }
//        //Add or Insert Operation  

//        public bool insertIntoTable(tDevice device)
//        {
//            try
//            {
//                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "tDevices.db")))
//                {
//                    connection.Insert(device);
//                    return true;
//                }
//            }
//            catch (SQLiteException ex)
//            {
//                //Log.Info("SQLiteEx", ex.Message);
//                return false;
//            }
//        }
//        public List<tDevice> selectTable()
//        {
//            try
//            {
//                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "tDevices.db")))
//                {
//                    return connection.Table<tDevice>().ToList();
//                }
//            }
//            catch (SQLiteException ex)
//            {
//                //Log.Info("SQLiteEx", ex.Message);
//                return null;
//            }
//        }
//        //Edit Operation  

//        public bool updateTable(tDevice device)
//        {
//            try
//            {
//                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "tDevicess.db")))
//                {
//                    connection.Query<tDevice>("UPDATE tDevice set Name=?, Department=?, Email=? Where Id=?", device.ipAddr, device.macAddr, device.nickname, device.Id);
//                    return true;
//                }
//            }
//            catch (SQLiteException ex)
//            {
//               // Log.Info("SQLiteEx", ex.Message);
//                return false;
//            }
//        }
//        //Delete Data Operation  

//        public bool removeTable(tDevice device)
//        {
//            try
//            {
//                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "tDevices.db")))
//                {
//                    connection.Delete(device);
//                    return true;
//                }
//            }
//            catch (SQLiteException ex)
//            {
//                //Log.Info("SQLiteEx", ex.Message);
//                return false;
//            }
//        }
//        //Select Operation  

//        public bool selectTable(int Id)
//        {
//            try
//            {
//                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "tDevices.db")))
//                {
//                    connection.Query<tDevice>("SELECT * FROM Person Where Id=?", Id);
//                    return true;
//                }
//            }
//            catch (SQLiteException ex)
//            {
//                //Log.Info("SQLiteEx", ex.Message);
//                return false;
//            }
//        }
//    }
//}