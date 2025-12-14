class Program
{
    static void Main()
    {
        Console.WriteLine("Тестирование подключения к базе данных...");

        try
        {
            // 1. Тест подключения
            using (var connection = DatabaseConnection.GetConnection())
            {
                connection.Open();
                Console.WriteLine("✓ Подключение к базе данных успешно");
                connection.Close();
            }

            // 2. Тест получения складов
            Console.WriteLine("\nПолучение списка складов:");
            var warehouses = DrugService.GetAllWarehouses();

            if (warehouses.Count == 0)
            {
                Console.WriteLine("Склады не найдены. Проверьте подключение к БД.");
            }
            else
            {
                Console.WriteLine($"Найдено складов: {warehouses.Count}");
                foreach (var wh in warehouses)
                {
                    Console.WriteLine($"  Склад #{wh.WarehouseId}: {wh.Address}");
                }

                // 3. Тест препаратов на первом складе
                if (warehouses.Count > 0)
                {
                    Console.WriteLine($"\nПрепараты на складе #{warehouses[0].WarehouseId}:");
                    var drugs = DrugService.GetAllDrugsInWarehouse(warehouses[0].WarehouseId);
                    Console.WriteLine($"  Найдено: {drugs.Count} позиций");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"✗ Ошибка: {ex.Message}");

            // Дополнительная информация для отладки
            Console.WriteLine("\nСоветы по устранению:");
            Console.WriteLine("1. Убедитесь, что файл main_db.mdb существует");
            Console.WriteLine("2. Проверьте права доступа к файлу");
            Console.WriteLine("3. Установите драйверы Access если используете ODBC");
        }

        Console.WriteLine("\nНажмите Enter для выхода...");
        Console.ReadLine();
    }
}