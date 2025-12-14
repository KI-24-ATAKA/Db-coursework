using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;

public static class DrugService
{
    //Получить все препараты на всех складах
    public static List<DrugStock> GetAllDrugsInAllWarehouses()
    {
        var drugs = new List<DrugStock>();

        using (var connection = DatabaseConnection.GetConnection())
        {
            connection.Open();
            string query = @"
                SELECT ds.* 
                FROM DrugStock ds 
                INNER JOIN Batch b ON ds.BatchId = b.BatchId
                WHERE b.ReceiptDate IS NOT NULL AND ds.ExpiryDate > ?";

            using (var command = new OleDbCommand(query, connection))
            {
                command.Parameters.AddWithValue("@expiry", DateTime.Now);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        drugs.Add(MapDrugStockFromReader(reader));
                    }
                }
            }
        }

        return drugs;
    }

    //Получить все препараты в конкретном складе
    public static List<DrugStock> GetAllDrugsInWarehouse(int warehouseId)
    {
        var drugs = new List<DrugStock>();

        using (var connection = DatabaseConnection.GetConnection())
        {
            connection.Open();
            string query = @"
                SELECT ds.* 
                FROM DrugStock ds 
                INNER JOIN Batch b ON ds.BatchId = b.BatchId
                WHERE ds.WarehouseId = ? 
                AND b.ReceiptDate IS NOT NULL 
                AND ds.ExpiryDate > ?";

            using (var command = new OleDbCommand(query, connection))
            {
                command.Parameters.AddWithValue("@warehouseId", warehouseId);
                command.Parameters.AddWithValue("@expiry", DateTime.Now);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        drugs.Add(MapDrugStockFromReader(reader));
                    }
                }
            }
        }

        return drugs;
    }

    //Получить конкретный препарат на всех складах
    public static List<DrugStock> GetSpecificDrugInAllWarehouses(int drugId)
    {
        var drugs = new List<DrugStock>();

        using (var connection = DatabaseConnection.GetConnection())
        {
            connection.Open();
            string query = @"
                SELECT ds.* 
                FROM DrugStock ds 
                INNER JOIN Batch b ON ds.BatchId = b.BatchId
                WHERE ds.DrugId = ? 
                AND b.ReceiptDate IS NOT NULL 
                AND ds.ExpiryDate > ?";

            using (var command = new OleDbCommand(query, connection))
            {
                command.Parameters.AddWithValue("@drugId", drugId);
                command.Parameters.AddWithValue("@expiry", DateTime.Now);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        drugs.Add(MapDrugStockFromReader(reader));
                    }
                }
            }
        }

        return drugs;
    }

    //Получить конкретный препарат в конкретном складе
    public static List<DrugStock> GetSpecificDrugInWarehouse(int drugId, int warehouseId)
    {
        var drugs = new List<DrugStock>();

        using (var connection = DatabaseConnection.GetConnection())
        {
            connection.Open();
            string query = @"
                SELECT ds.* 
                FROM DrugStock ds 
                INNER JOIN Batch b ON ds.BatchId = b.BatchId
                WHERE ds.DrugId = ? 
                AND ds.WarehouseId = ? 
                AND b.ReceiptDate IS NOT NULL 
                AND ds.ExpiryDate > ?";

            using (var command = new OleDbCommand(query, connection))
            {
                command.Parameters.AddWithValue("@drugId", drugId);
                command.Parameters.AddWithValue("@warehouseId", warehouseId);
                command.Parameters.AddWithValue("@expiry", DateTime.Now);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        drugs.Add(MapDrugStockFromReader(reader));
                    }
                }
            }
        }

        return drugs;
    }

    //Получить информацию о препарате по ID
    public static Drug GetDrugInfo(int drugId)
    {
        using (var connection = DatabaseConnection.GetConnection())
        {
            connection.Open();
            string query = "SELECT * FROM Drug WHERE DrugId = ?";

            using (var command = new OleDbCommand(query, connection))
            {
                command.Parameters.AddWithValue("@drugId", drugId);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Drug
                        {
                            DrugId = Convert.ToInt32(reader["DrugId"]),
                            DrugName = reader["DrugName"].ToString(),
                            Dosage = Convert.ToDecimal(reader["Dosage"]),
                            Form = reader["Form"].ToString(),
                            Manufacturer = reader["Manufacturer"].ToString(),
                            PrescriptionRequired = Convert.ToBoolean(reader["PrescriptionRequired"])
                        };
                    }
                }
            }
        }

        return null;
    }

    //Получить список всех складов
    public static List<Warehouse> GetAllWarehouses()
    {
        var warehouses = new List<Warehouse>();

        using (var connection = DatabaseConnection.GetConnection())
        {
            connection.Open();
            string query = "SELECT * FROM Warehouse";

            using (var command = new OleDbCommand(query, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        warehouses.Add(new Warehouse
                        {
                            WarehouseId = Convert.ToInt32(reader["WarehouseId"]),
                            Address = reader["Address"].ToString(),
                            Phone = reader["Phone"].ToString(),
                            ResponsiblePerson = reader["ResponsiblePerson"].ToString()
                        });
                    }
                }
            }
        }

        return warehouses;
    }

    private static DrugStock MapDrugStockFromReader(OleDbDataReader reader)
    {
        return new DrugStock
        {
            BatchId = reader["BatchId"].ToString(),
            WarehouseId = Convert.ToInt32(reader["WarehouseId"]),
            DrugId = Convert.ToInt32(reader["DrugId"]),
            ExpiryDate = Convert.ToDateTime(reader["ExpiryDate"]),
            Price = Convert.ToDecimal(reader["Price"]),
            PackageCount = Convert.ToInt32(reader["PackageCount"]),
            UnitCount = Convert.ToInt32(reader["UnitCount"])
        };
    }
}