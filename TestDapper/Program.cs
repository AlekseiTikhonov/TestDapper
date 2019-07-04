using Npgsql;
using Dapper;
using System;
using System.Linq;
using System.Collections.Generic;

namespace TestDapper
{
    class Program
    {
        private static string sqlconnect = "Server=127.0.0.1;User Id=postgres; " + "Password=root;Database=Employees;";
        static List<Employee> employees = new List<Employee>();
        static void Main(string[] args)
        {
            ShowStartMenu();
        }

        public static void ShowStartMenu()
        {
            Console.WriteLine("Выберите, что вы хотите сделать");
            Console.WriteLine("1 - Посмотреть информацию в базе данных");
            Console.WriteLine("2 - Добавить сотрудника в бд");
            MakeChoice();
        }

        public static void MakeChoice()
        {
            uint choice = 0;
            bool check = false;
            do
            {
                check = uint.TryParse(Console.ReadLine(), out choice);
                if (!check || choice <= 0)
                {
                    Console.WriteLine("Ввод был неккоретен, пожалуйста введите цифру от 1-3");
                }

            }
            while (!check || choice <= 0);
            ExecuteChoice(choice);
        }

        public static void ExecuteChoice(uint choice)
        {
            switch (choice)
            {
                case 1:
                    Console.Clear();
                    SelectRequest();
                    ShowData();
                    QuestionToMainMenu();
                    break;
                case 2:
                    Console.Clear();
                    AddEmployee();
                    InsertEmployee();
                    InsertTelephons();
                    QuestionToMainMenu();
                    break;
            }
        }


        public static void SelectRequest()
        {

            string sql = " Select p.id, p.first_name, p.last_name, t.telephone " +
                "from people p left join(SELECT owner_id, string_agg(telephone, ',') as telephone FROM telephone" +
                       " GROUP BY owner_id) t on p.id = t.owner_id";

            using (NpgsqlConnection connection = new NpgsqlConnection(sqlconnect))
            {
                SqlMapper.ResetTypeHandlers();
                SqlMapper.AddTypeHandler(new TelephonesTypeHandler());
                employees = connection.Query<Employee>(sql).ToList();
            }

        }

        public static void ShowData()
        {
            foreach (var employee in employees)
            {
                Console.WriteLine("ID: = {0}", employee.Id);
                Console.WriteLine("First name: = {0}", employee.first_name);
                Console.WriteLine("Last name: = {0}", employee.last_name);
                if (employee.telephone != null)
                {
                    foreach (var telephone in employee.telephone)
                    {
                        Console.WriteLine("Telephone : = {0}", telephone.ToString());
                    }
                }
                else
                {
                    Console.WriteLine("Telephone doesn't exist");
                } 
                Console.WriteLine("============================");
            }
        }

        public static void QuestionToMainMenu()
        {
            Console.WriteLine("Для перехода в главное меню нажмите любую клавишу");
            Console.ReadKey();
            Console.Clear();
            ShowStartMenu();
        }

        public static void AddEmployee()
        {
            employees.Add(new Employee());
            Console.WriteLine("Введите имя работника:");
            employees.ElementAt(employees.Count - 1).first_name = Console.ReadLine();
            Console.WriteLine("Введите фамилию работника:");
            employees.ElementAt(employees.Count - 1).last_name = Console.ReadLine();
        }

        public static void InsertEmployee()
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(sqlconnect))
            {
                string insertQuery = @"INSERT INTO people(first_name, last_name) VALUES (@first_name, @last_name)";
                var result = connection.Execute(insertQuery, new { first_name = employees.ElementAt(employees.Count - 1).first_name, last_name = employees.ElementAt(employees.Count - 1).last_name });
                
            }
        }

        public static void InsertTelephons()
        {
            Console.WriteLine("Введите количество телефонов (число)");
            
            uint n = 0;
            bool check = false;
            do
            {
                check = uint.TryParse(Console.ReadLine(), out  n);
                if (!check || n <= 0 || n >3)
                {
                    Console.WriteLine("Ввод был неккоретен, число должно быть от 1 до 3");
                }

            }
            while (!check || n <= 0 || n>3);

            string[] tels = new string[n];
            for (int i = 0; i < n; i++)
            {
                do
                {
                    Console.Write("Телефон {0}: ",i+1);
                    tels[i] = Console.ReadLine();
                    //  employees.ElementAt(employees.Count - 1).telephone.Add(new Telephone());
                    //  employees.ElementAt(employees.Count - 1).telephone.ElementAt(i).TelephoneStr = Console.ReadLine();
                }
                while (tels[i]==""|| tels[i].Length>12);
            }

            using (NpgsqlConnection connection = new NpgsqlConnection(sqlconnect))
            {
                foreach (var telephon in tels) //employees.ElementAt(employees.Count - 1).telephone)
                {
                    string insertQuery2 = $"INSERT INTO public.telephone(telephone, owner_id) Select '{telephon.ToString()}',max(id) from people;";
                    var result2 = connection.Execute(insertQuery2);
                }
            }
        }

        /* var sql = "select cast(1 as decimal) ProductId, 'a' ProductName, 'x' AccountOpened, cast(1 as decimal) CustomerId, 'name' CustomerName";

    var item = connection.Query<ProductItem, Customer, ProductItem>(sql,
        (p, c) => { p.Customer = c; return p; }, splitOn: "CustomerId").First();

    item.Customer.CustomerId.IsEqualTo(1);*/

    }
}
