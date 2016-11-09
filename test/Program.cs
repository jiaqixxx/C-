using System;
using System.Data;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using System.IO;

namespace test
{
	class MainClass
	{
		private MySqlConnection conn;
		public MainClass() {
			Initialize();
		}	

		public void Initialize() {
			string connection = "Server=localhost; User ID=root; Password=; Database=C#";
			conn = new MySqlConnection(connection);
		}

		private bool OpenConnection() {
			conn.Open();
			return true;
		}

		private bool CloseConnection() {
			conn.Close();
			return true;

		}

		public void Create() {
			string query = "CREATE TABLE test (" +
				"testID int NOT NULL AUTO_INCREMENT PRIMARY KEY," +
				"FirstName varchar(255) NOT NULL)";
			if (this.OpenConnection() == true) { 
				MySqlCommand cmd = new MySqlCommand(query, conn);
				cmd.ExecuteNonQuery();
				this.CloseConnection();
			}
		}

		public void Insert() {
			string query = "INSERT INTO Persons (PersonID, information) VALUES ('3', 'xxxxxxx')";
			if (this.OpenConnection() == true) { 
				MySqlCommand cmd = new MySqlCommand(query, conn);
				cmd.ExecuteNonQuery();
				this.CloseConnection();
			}
		}

		public void Update() {
			string query = "UPDATE Persons SET information='yyyyy' WHERE PersonID='3'";
			if (this.OpenConnection() == true) {
				MySqlCommand cmd = new MySqlCommand(query,conn);
				cmd.ExecuteNonQuery();
				this.CloseConnection();
			}
		}

		public void Delete() {
			string query = "DELETE FROM Persons WHERE PersonID='501'";
			if (this.OpenConnection() == true) {
				MySqlCommand cmd = new MySqlCommand(query, conn);
				cmd.ExecuteNonQuery();
				this.CloseConnection();
			}
		}

		public List<string>[] Select() {
			string query = "SELECT * FROM Persons";
			List<string>[] list = new List<string>[2];
			list[0] = new List<string>();
			list[1] = new List<string>();

			if (this.OpenConnection() == true)
			{
				MySqlCommand cmd = new MySqlCommand(query, conn);
				MySqlDataReader reader = cmd.ExecuteReader();

				while (reader.Read())
				{
					list[0].Add(reader["PersonID"] + "");
					list[1].Add(reader["information"] + "");
				}
				reader.Close();
				this.CloseConnection();
				return list;
			}else{
				return list;
			}

		}

		public int Count() {
			string query = "SELECT Count(*) FROM Persons";
			int count = -1;
			if (this.OpenConnection() == true)
			{
				MySqlCommand cmd = new MySqlCommand(query, conn);
				count = int.Parse(cmd.ExecuteScalar() + "");
				this.CloseConnection();
				return count;
			}
			else {
				return count;
			}
		}

		public void Join() {
			string query = "SELECT user_details.user_id, user_details.first_name, Persons.information FROM user_details INNER JOIN Persons " +
				"ON user_details.user_id=Persons.PersonID";

			if (this.OpenConnection() == true) {
				MySqlCommand cmd = new MySqlCommand(query, conn);
				MySqlDataReader reader = cmd.ExecuteReader();
				while (reader.Read()) {
					Console.WriteLine("{0}, {1}, {2}", reader.GetString(0), reader.GetString(1), reader.GetString(2));
				}
				reader.Close();
				this.CloseConnection();
			}
		}

		public void Backup() {
			string path = "/Users/jessy/Desktop/temp1.sql";
			StreamWriter file = new StreamWriter(path);

			ProcessStartInfo psi = new ProcessStartInfo();
			psi.FileName = "mysqldump";
			psi.RedirectStandardInput = false;
			psi.RedirectStandardOutput = true;
			psi.Arguments = string.Format(@"-u{0} -p{1} -h{2} {3}", "root", "", "localhost", "C#");
			psi.UseShellExecute = false;

			Process process = Process.Start(psi);
			string output = process.StandardOutput.ReadToEnd();
			file.WriteLine(output);
			process.WaitForExit();
			file.Close();
			process.Close();
		}

		public void Restore() {
			string path = "/Users/jessy/Desktop/temp1.sql";
			StreamReader file = new StreamReader(path);
			string input = file.ReadToEnd();
			file.Close();

			ProcessStartInfo psi = new ProcessStartInfo();
			psi.FileName = "mysql";
			psi.RedirectStandardInput = true;
			psi.RedirectStandardOutput = false;
			psi.Arguments = string.Format(@"-u{0} -p{1} -h{2} {3}", "root", "", "localhost", "C#");
			psi.UseShellExecute = false;

			Process process = Process.Start(psi);
			process.StandardInput.WriteLine(input);
			process.StandardInput.Close();
			process.WaitForExit();
			process.Close();
		}

		public static void Main(string[] args)
		{
			MainClass db = new MainClass();
			//db.Insert();
			//db.Update();
			//db.Delete();
			/*List<string>[] list = new List<string>[2];
			list = db.Select();
			foreach (string temp in list[0]) {
				Console.WriteLine(temp);
			}*/
			//Console.WriteLine(db.Count());
			//db.Join();
			//db.Backup();
			//db.Restore();
			//db.Create();
		}
	}
}
