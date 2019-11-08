using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace doorApp
{
    public interface Isqlite
    {
        SQLiteConnection GetConnection();
    }
}