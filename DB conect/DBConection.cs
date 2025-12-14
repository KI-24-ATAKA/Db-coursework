using System;
using System.Data;
using System.Data.OleDb;

public static class DatabaseConnection
{
    private static readonly string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=main_db.mdb;";

    public static OleDbConnection GetConnection()
    {
        return new OleDbConnection(connectionString);
    }
}