using FirebirdSql.Data.FirebirdClient;
using sync_swagger.Models.Personnel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace sync_swagger.Service.Firebird
{
    public static class FirebirdService
    {
        private static readonly string StringConnection = "database=192.168.250.6:Pers;user=sysdba;password=Vtlysq~Bcgjkby2020;Charset=win1251;";

        #region Список отделоа
        public static async Task<List<Department>> GetDepartment()
        {
            List<Department> List = new();
            string sql =
                " select" +
                " ROD.FIO," +
                " TYP.NAME," +
                " ROD.ADRES," +
                " ROD.TEL_DOM," +
                " ROD.TEL_RAB " +
                " from RODITELI ROD " +
                " left join TYP_RODSTVO TYP on TYP.ID = ROD.RODSTVO_ID " +
                " where ROD.STUD_ID = ";

            await using FbConnection connection = new(StringConnection);
            connection.Open();
            await using FbTransaction transaction = await connection.BeginTransactionAsync();
            await using FbCommand command = new(sql, connection, transaction);
            FbDataReader reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                List.Add(new Department
                {
                    Name = "NAme"
                    /*FullName = reader["FIO"] != DBNull.Value ? reader.GetString(0) : "Не указано",
                    Type = reader["NAME"] != DBNull.Value ? reader.GetString(1) : "Не указано",
                    Adress = reader["ADRES"] != DBNull.Value ? reader.GetString(2) : "Не указано",
                    Mobbile = reader["TEL_DOM"] != DBNull.Value ? reader.GetString(3) : "Не указано",
                    ExtraMobbile = reader["TEL_RAB"] != DBNull.Value ? reader.GetString(4) : "Не указано",
                    */
                });
            }
            await reader.CloseAsync();

            return List;
        }

        #endregion
    }
}
