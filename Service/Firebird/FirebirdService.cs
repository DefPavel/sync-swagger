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
        private static int num;

        #region Список должностей на отдел
        public static async Task<IEnumerable<Position>> GetPositionsAsync(int idDep)
        {
            List<Position> List = new();
            string sql = " select " +
                "D.NAME, " +
                "D.IS_PED," +
                "D.OKLAD_B, " +
                "D.OKLAD_NB," +
                "D.OTPUSK," +
                "D.KOLVO_B," +
                "D.KOLVO_NB," +
                "D.FREE_B," +
                "D.FREE_NB," +
                "D.PRIORITI_DOLJ," +
                "D.NAME_ROD " +

                " from DOLJNOST D where D.OTDEL_ID = " + idDep;

            await using FbConnection connection = new(StringConnection);
            connection.Open();
            await using FbTransaction transaction = await connection.BeginTransactionAsync();
            await using FbCommand command = new(sql, connection, transaction);
            FbDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                List.Add(new Position
                {
                    Name = reader.GetString(0),
                    IsPed = (string)reader["IS_PED"] == "T",
                    oklad_B = reader["OKLAD_B"] != DBNull.Value ? reader.GetDecimal(2) : 0,
                    oklad_N = reader["OKLAD_NB"] != DBNull.Value ? reader.GetDecimal(3) : 0,
                    holidayLimit = (short)(reader["OTPUSK"] != DBNull.Value ? reader.GetInt16(4) : 0),
                    count_B = reader["KOLVO_B"] != DBNull.Value ? reader.GetDecimal(5) : 0,
                    count_NB = reader["KOLVO_NB"] != DBNull.Value ? reader.GetDecimal(6) : 0,
                    free_b = reader["FREE_B"] != DBNull.Value ? reader.GetDecimal(7) : 0,
                    free_nb = reader["FREE_NB"] != DBNull.Value ? reader.GetDecimal(8) : 0,
                    priority = (short)(reader["PRIORITI_DOLJ"] != DBNull.Value ? reader.GetInt16(9) : 0),
                    padeg = reader["NAME_ROD"] != DBNull.Value ? reader.GetString(10) : "Не указано"

                });
            }
            await reader.CloseAsync();

            return List.AsReadOnly();
        }
        #endregion

        #region Список отделов
        public static async Task<List<Department>> GetDepartment()
        {
            List<Department> List = new();
            string sql = " select * from get_tree_root;";

            await using FbConnection connection = new(StringConnection);
            connection.Open();
            await using FbTransaction transaction = await connection.BeginTransactionAsync();
            await using FbCommand command = new(sql, connection, transaction);
            FbDataReader reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                string TypeDep = reader.GetString(3).ToLower().Contains("институт") ? "Институт" :
              reader.GetString(3).ToLower().Contains("кафедра") ? "Кафедра" :
              reader.GetString(3).ToLower().Contains("отдел") ? "Отдел" :
              reader.GetString(3).ToLower().Contains("факультет") ? "Факультет" :
              reader.GetString(3).ToLower().Contains("ректорат") ? "Ректорат" :
              reader.GetString(3).ToLower().Contains("центр") ? "Центр" :
              reader.GetString(3).ToLower().Contains("сектор") ? "Сектор" :
              reader.GetString(3).ToLower().Contains("колледж") ? "Колледж" :
              reader.GetString(3).ToLower().Contains("центр") ? "Центр" :
              "Не указано";


                List.Add(new Department
                {
                    Id = reader.GetInt32(1),
                    Phone = reader["phone"] != DBNull.Value ? reader.GetString(2) : "Не указано",
                    Name = reader.GetString(3),
                    Short = reader.GetString(4),
                    Padeg = reader.GetString(5),
                    Type = TypeDep,
                    Root = reader.GetString(7),
                    Positions = await GetPositionsAsync(reader.GetInt32(1))

                });
            }
            await reader.CloseAsync();

            return List;
        }

        #endregion

        #region Основная информация по сотрудникам

        public static async Task<IEnumerable<Persons>> GetPersonsAsync()
        {
            List<Persons> List = new();
            string sql = "select" +
                        " distinct " +
                        "s.famil ," + //0
                        "s.name ," +//1
                        "s.otch ," +//2
                        "s.sex ," +//3
                        "s.date_birth ," +//4
                        "s.IDENT_CODE," +//5
                        "s.phone," + // 6
                        "s.phone_lug," +// 7
                        "s.address," + //8
                        "s.pasp_typ," +//9
                        "s.pasp_ser," +//10
                        "s.pasp_n ," +//11
                        "s.pasp_date," +//12
                        "s.pasp_organ ," +//13
                        "s.pasp_rus," + //14
                        "s.ROD_STAN," +//15
                        "s.is_stud," +
                        "s.is_aspirant," +
                        "s.is_outher," +
                        "s.is_doktor," +
                        "s.is_rus_snils," +
                        "s.mat_otv," +
                        "s.MAT_ODINOCH," +
                        "s.MAT_DVOIH," +
                        "s.PREVIOUS_CONVICTION, " +
                        "s.date_start_job," +
                        "s.FIO_DATEL  ," +
                        "s.id ," +
                        "s.photo " +
                        "from sotr s " +
                        "inner join sotr_doljn sd on s.id = sd.sotr_id " +
                        " where sd.dolj_id <> 0 " +
                        "order by s.id desc";

            await using FbConnection connection = new(StringConnection);
            connection.Open();
            await using FbTransaction transaction = await connection.BeginTransactionAsync();
            await using FbCommand command = new(sql, connection, transaction);
            FbDataReader reader = await command.ExecuteReaderAsync();
            string TypePassport = string.Empty;
            if (reader.HasRows)
            {

                while (await reader.ReadAsync())
                {
                    //byte[] temp_backToBytes = Convert.FromBase64String((byte[])reader["photo"]);

                    bool isNum = int.TryParse(reader.GetString(10).Replace(" ", ""), out num);
                    if (isNum)
                    {
                        TypePassport = "Паспорт(России)";
                    }
                    else
                    {
                        TypePassport = reader.GetString(10).Replace(" ", "") == "ТН" ? "Паспорт(ЛНР)" :
                                         reader.GetString(10).Replace(" ", "") == "ТМ" ? "Паспорт(ЛНР)" :
                                         reader.GetString(10).Replace(" ", "") == "ТТ" ? "Паспорт(ЛНР)" :
                                         reader.GetString(10).Replace(" ", "") == "нет" ? "Не указано" :
                                         "Паспорт(Украины)";
                    }

                    //Внешний совместитель
                    bool isOuther = reader.GetString(18) == "T";

                    List.Add(new Persons
                    {
                        persId = reader.GetInt32(27),
                        FirstName = reader.GetString(0),
                        Name = reader.GetString(1),
                        LastName = reader.GetString(2),
                        Gender = reader.GetString(3) == "Жен" ? "female" : "male",
                        DateBirthay = reader.GetDateTime(4).ToString("yyyy-MM-dd"),
                        Code = reader["IDENT_CODE"] != DBNull.Value ? reader.GetString(5) : "Не указано",
                        //photo = reader["photo"] as byte[] != null ? Convert.ToBase64String(reader["photo"] as byte[]) : null,
                        PhoneUA = reader.GetString(6),
                        PhoneLG = reader.GetString(7),
                        Adress = reader.GetString(8),
                        TypePassport = TypePassport,
                        SerialPassport = reader["pasp_ser"] != DBNull.Value ? reader.GetString(10) : "Не указано",
                        NumberPassport = reader["pasp_n"] != DBNull.Value ? reader.GetString(11) : "Не указано",
                        DatePassport = reader["pasp_date"] != DBNull.Value ? reader.GetDateTime(12).ToString("yyyy-MM-dd") : "1970-01-01",
                        OrganiztionPassport = reader.GetString(13),
                        IsRusPassport = reader.GetString(14) == "T", // Имеет ли российский паспорт
                        IsMarriage = reader.GetString(15) == "T", // Состоит ли в браке
                        IsStudent = reader.GetString(16) == "T",
                        IsAspirant = reader.GetString(17) == "T",
                        IsDoctor = reader.GetString(19) == "T",
                        IsSnils = reader.GetString(20) == "T", // Снилс
                        IsResponsible = reader.GetString(21) == "T", // Матереально отвест.
                        IsSingleMother = reader.GetString(22) == "T",
                        IsTwoChildMother = reader.GetString(23) == "T",
                        IsPreviosConvition = reader.GetString(24) == "T",// Справка о не судимости
                        DateToWorking = reader["date_start_job"] != DBNull.Value ? reader.GetDateTime(25).ToString("yyyy-MM-dd") : "1970-01-01",
                        Padeg = reader["FIO_DATEL"] != DBNull.Value ? reader.GetString(26) : "Не указано",
                        ArrayPositions = await GetPersonPosition(reader.GetInt32(27)), // Должности
                        ArraySurname = await GetChangeSurname(reader.GetInt32(27)),// Смена фамилии
                        ArrayFamily = await GetFamiliesAsync(reader.GetInt32(27)), // Родственники

                        ArrayPensioners = await GetPensionerAsync(reader.GetInt32(27)), // Пенсонеры
                        ArrayInvalids = await GetInvalids(reader.GetInt32(27)), //Инвалиды

                        ArraEducation = await GetEducationsAsync(reader.GetInt32(27)), // Образование
                        ArrayHistoryBook = await GetHistoryBooksAsync(reader.GetInt32(27)),// Трудовая книга
                        ArrayMedical = await GetMedicals(reader.GetInt32(27)), // Медицинское обзраование
                                                                               // ArrayMove = await GetMovesAsync(reader.GetInt32(27)),
                        ArrayAcademics = await ChlenAcademicsAsync(reader.GetInt32(27)),
                    });
                }
            }
            await reader.CloseAsync();
            return List.AsReadOnly();
        }

        private static async Task<IEnumerable<ChlenAcademic>> ChlenAcademicsAsync(int id_pers)
        {
            List<ChlenAcademic> List = new();

            string sql = " select distinct chl.name , chl.academ_name, chl.num_diploma , chl.obr_date " +
                "from sotr_chlen_academ chl where chl.sotr_id =" + id_pers;

            await using FbConnection connection = new(StringConnection);
            connection.Open();
            await using FbTransaction transaction = await connection.BeginTransactionAsync();
            await using FbCommand command = new(sql, connection, transaction);
            FbDataReader reader = await command.ExecuteReaderAsync();

            if (reader.HasRows)
            {
                while (await reader.ReadAsync())
                {
                    List.Add(new ChlenAcademic
                    {
                        Type = reader.GetString(0),
                        AcademicalName = reader["name"] != DBNull.Value ? reader.GetString(1) : "Не указано",
                        NumberDiplom = reader["num_diploma"] != DBNull.Value ? reader.GetString(2) : "Не указано",
                        Date = reader["obr_date"] != DBNull.Value ? reader.GetDateTime(3).ToString("yyyy-MM-dd") : null,
                    });
                }

            }
            await reader.CloseAsync();
            return List;
        }

        private static async Task<IEnumerable<Medical>> GetMedicals(int id_pers)
        {
            List<Medical> List = new();

            string sql = " select distinct pm.text , mk.name , pm.date_start , pm.date_finish " +
                "from pers_med pm inner join med_kategor mk on mk.id = pm.kat_id where pm.sotr_id =" + id_pers;

            await using FbConnection connection = new(StringConnection);
            connection.Open();
            await using FbTransaction transaction = await connection.BeginTransactionAsync();
            await using FbCommand command = new(sql, connection, transaction);
            FbDataReader reader = await command.ExecuteReaderAsync();

            if (reader.HasRows)
            {
                while (await reader.ReadAsync())
                {
                    List.Add(new Medical
                    {
                        Name = reader.GetString(0),
                        Type = reader.GetString(1),
                        DateBegin = reader["date_start"] != DBNull.Value ? reader.GetDateTime(2).ToString("yyyy-MM-dd") : null,
                        DateEnd = reader["date_finish"] != DBNull.Value ? reader.GetDateTime(3).ToString("yyyy-MM-dd") : null,
                    });
                }

            }
            await reader.CloseAsync();
            return List;
        }

        private static async Task<IEnumerable<HistoryBook>> GetHistoryBooksAsync(int IdPerson)
        {
            List<HistoryBook> List = new();
            string sql = "select tk.n_zap , tk.date_zap , tk.info , tk.staj_ob ,tk.staj_ped, tk.staj_nauch , tk.staj_lgpu , tk.staj_bibl, tk.staj_muzei, tk.staj_med, tk.prikaz_name "
                         + " from trud_knizka tk"
                         + " where tk.date_zap is not null and tk.sotr_id = " + IdPerson;

            await using FbConnection connection = new(StringConnection);
            connection.Open();
            await using FbTransaction transaction = await connection.BeginTransactionAsync();
            await using FbCommand command = new(sql, connection, transaction);
            FbDataReader reader = await command.ExecuteReaderAsync();
            int record = 1;
            if (reader.HasRows)
            {
                while (await reader.ReadAsync())
                {
                    List.Add(new HistoryBook
                    {
                        numberRecord = record,
                        dateInsert = reader.GetDateTime(1).ToString("yyyy-MM-dd"),
                        information = reader["info"] != DBNull.Value ? reader.GetString(2) : "Не указано",
                        isOver = reader.GetString(3) == "T",
                        isPedagogical = reader.GetString(4) == "T",
                        isScience = reader.GetString(5) == "T",
                        isUniver = reader.GetString(6) == "T",
                        isLibrary = reader.GetString(7) == "T",
                        isMuseum = reader.GetString(8) == "T",
                        isMedical = reader.GetString(9) == "T",
                        orderName = reader["prikaz_name"] != DBNull.Value ? reader.GetString(10) : "Не указано"

                    });
                    record++;
                }

            }
            await reader.CloseAsync();
            return List.AsReadOnly();
        }

        private static async Task<IEnumerable<Education>> GetEducationsAsync(int IdPerson)
        {
            List<Education> List = new();
            string sql = "select distinct ed.uch_zav , ed.typ_obr , ed.spec , ed.kvalification , ed.date_vidachy ,ed.n_diploma , ed.is_osn" +
                " from education ed " +
                "where ed.date_vidachy is not null and ed.sotr_id = " + IdPerson;

            await using FbConnection connection = new(StringConnection);
            connection.Open();
            await using FbTransaction transaction = await connection.BeginTransactionAsync();
            await using FbCommand command = new(sql, connection, transaction);
            FbDataReader reader = await command.ExecuteReaderAsync();
            if (reader.HasRows)
            {
                while (await reader.ReadAsync())
                {
                    List.Add(new Education
                    {
                        institution = reader.GetString(0),
                        type = reader.GetString(1),
                        specialty = reader.GetString(2),
                        qualification = reader.GetString(3),
                        date_issue = reader["date_vidachy"] != DBNull.Value ? reader.GetDateTime(4).ToString("yyyy-MM-dd") : null,
                        name_diplom = reader.GetString(5),
                        is_actual = reader.GetString(6) == "T"
                    });
                }
            }
            await reader.CloseAsync();
            return List.AsReadOnly();
        }

        private static async Task<IEnumerable<Invalids>> GetInvalids(int IdPerson)
        {
            List<Invalids> List = new();
            string sql = "select NUM_DOC , DATE_BEGIN , DATE_END , FOR_DEATH , GROUPE " +
                " from SOTR_INV " +
                "where SOTR_INV.sotr_id = " + IdPerson;

            await using FbConnection connection = new(StringConnection);
            connection.Open();
            await using FbTransaction transaction = await connection.BeginTransactionAsync();
            await using FbCommand command = new(sql, connection, transaction);
            FbDataReader reader = await command.ExecuteReaderAsync();
            if (reader.HasRows)
            {
                while (await reader.ReadAsync())
                {
                    Console.WriteLine(IdPerson);
                    List.Add(new Invalids
                    {
                        NameDocument = reader["num_doc"] != DBNull.Value ? reader.GetString(0) : "Не указано",
                        DateBegin = reader["DATE_BEGIN"] != DBNull.Value ? reader.GetDateTime(1).ToString("yyyy-MM-dd") : "1900-01-01",
                        DateEnd = reader["DATE_END"] != DBNull.Value ? reader.GetDateTime(2).ToString("yyyy-MM-dd") : null,
                        ForLife = reader.GetString(3) == "T",
                        Group = reader["GROUPE"] != DBNull.Value ? reader.GetInt16(4) : 0
                    });
                }
            }

            await reader.CloseAsync();
            return List.AsReadOnly();
        }

        private static async Task<IEnumerable<Pensioners>> GetPensionerAsync(int IdPerson)
        {
            List<Pensioners> List = new();
            string sql = "select num_doc , date_doc , PRICINA_VYHODA " +
                " from SOTR_PENS " +
                "where SOTR_PENS.sotr_id = " + IdPerson;

            await using FbConnection connection = new(StringConnection);
            connection.Open();
            await using FbTransaction transaction = await connection.BeginTransactionAsync();
            await using FbCommand command = new(sql, connection, transaction);
            FbDataReader reader = await command.ExecuteReaderAsync();
            if (reader.HasRows)
            {
                while (await reader.ReadAsync())
                {
                    List.Add(new Pensioners
                    {
                        Document = reader["num_doc"] != DBNull.Value ? reader.GetString(0) : "Не указано",
                        DateDocument = reader["date_doc"] != DBNull.Value ? reader.GetDateTime(1).ToString("yyyy-MM-dd") : "1900-01-01",
                        TypeDocument = reader["PRICINA_VYHODA"] != DBNull.Value ? reader.GetString(2).ToLower() : "вид не указан"
                    });
                }
            }

            await reader.CloseAsync();
            return List.AsReadOnly();
        }

        private static async Task<IEnumerable<Family>> GetFamiliesAsync(int idPers)
        {
            List<Family> List = new();
            string sql = "select f.typ , f.FIO , f.prim from FAMILY f where f.sotr_id = " + idPers;
            await using FbConnection connection = new(StringConnection);
            connection.Open();
            await using FbTransaction transaction = await connection.BeginTransactionAsync();
            await using FbCommand command = new(sql, connection, transaction);
            FbDataReader reader = await command.ExecuteReaderAsync();
            if (reader.HasRows)
            {
                while (await reader.ReadAsync())
                {
                    List.Add(new Family
                    {
                        Type = reader.GetString(0),
                        FullName = reader.GetString(1),
                        Description = reader["prim"] != DBNull.Value ? reader.GetString(2) : "Не указано",
                    });
                }
            }
            await reader.CloseAsync();

            return List.AsReadOnly();
        }

        private static async Task<IEnumerable<ChangeSurname>> GetChangeSurname(int idPers)
        {
            List<ChangeSurname> List = new();
            string sql = " select CF.ex_famil , P.name , P.date_crt , P.typ " +
                "from CHANGED_FAMILS CF " +
                "inner join prikaz p on p.id = CF.prikaz_id " +
                "where CF.sotr_id = " + idPers;


            await using FbConnection connection = new(StringConnection);
            connection.Open();
            await using FbTransaction transaction = await connection.BeginTransactionAsync();
            await using FbCommand command = new(sql, connection, transaction);
            FbDataReader reader = await command.ExecuteReaderAsync();
            if (reader.HasRows)
            {
                while (await reader.ReadAsync())
                {
                    string typeOrder =
                        reader.GetString(3) == "famil" ? "Смена фамилии" : "";

                    List.Add(new ChangeSurname
                    {
                        OldSurname = reader["ex_famil"] != DBNull.Value ? reader.GetString(0) : "Не указано",
                        Order = $"{reader.GetString(1)}(от {reader.GetDateTime(2).ToShortDateString()})",
                        TypeOrder = typeOrder,
                        DateOrder = reader.GetDateTime(2).ToString("yyyy-MM-dd"),

                    });
                }
            }
            await reader.CloseAsync();

            return List.AsReadOnly();
        }

        private static async Task<IEnumerable<PersonPosition>> GetPersonPosition(int IdPerson)
        {
            List<PersonPosition> List = new();
            string sql =
                  "select "
                + "d.name as DNAME,"
                + "o.name as ONAME,"
                + "pr.name as ORDERS,"
                + "pr.date_crt as DATEORDER,"
                + "pr.typ as TYPEORDER,"
                + "td.name as CONTRACT,"
                + "mj.name as PLACE,"
                + "sd.is_osn as ISMAIN,"
                + "sd.kolvo_b,"
                + "sd.kolvo_nb,"
                + "s.is_outher ,"
                + "d.is_ped ,"
                + "sd.date_kontr_nach,"
                + "sd.date_kontr_kon,"
                + "sd.dat_drop,"
                + "sd.dolj_uvoln"
                + " from sotr s"
                + " inner join sotr_doljn sd on s.id = sd.sotr_id"
                + " left join MESTO_JOB mj on mj.id = sd.MESTO_JOB"
                + " left join TYP_DOGOVOR td on td.id = sd.typ_dog"
                + " inner join prikaz pr on pr.id = sd.prikaz_id"
                + " inner join doljnost d on d.id = sd.dolj_id"
                + " inner join otdel o on o.id = d.otdel_id"
                + " where s.id = " + IdPerson;

            await using FbConnection connection = new(StringConnection);
            connection.Open();
            await using FbTransaction transaction = await connection.BeginTransactionAsync();
            await using FbCommand command = new(sql, connection, transaction);
            FbDataReader reader = await command.ExecuteReaderAsync();
            if (reader.HasRows)
            {
                while (await reader.ReadAsync())
                {
                    string typeOrder =
                        reader.GetString(4) == "priyom" ? "Приём" :
                        reader.GetString(4) == "uvolnenie" ? "Увольнение" :
                        reader.GetString(4) == "perev" ? "Перевод" :
                        reader.GetString(4) == "otpusk" ? "Отпуск" :
                        reader.GetString(4) == "voensbor" ? "Военные сборы" :
                        reader.GetString(4) == "prodstd" ? "Продление СТД" :
                        reader.GetString(4) == "den" ? "Денежная компенсация за неиспользованный отпуск" :
                        "";
                    List.Add(new PersonPosition
                    {
                        Position = reader.GetString(0),
                        Department = reader.GetString(1),
                        Order = $"{reader.GetString(2)}(от {reader.GetDateTime(3).ToShortDateString()})",
                        DateOrder = reader.GetDateTime(3).ToString("yyyy-MM-dd"),
                        TypeOrder = typeOrder,
                        Contract = reader["CONTRACT"] != DBNull.Value ? reader.GetString(5) : "Не указано",
                        Place = reader.GetString(6),
                        IsMain = reader.GetString(7) == "T",
                        CountBudget = reader.GetDecimal(8),
                        CountNoBudget = reader.GetDecimal(9),
                        IsPluralismOter = reader.GetString(10) == "T", //Внешний совместитель
                        IsPluralismInner = reader.GetString(7) == "F", // Совместитель
                        IsPed = reader.GetString(11) == "T",
                        DateStartContract = reader["date_kontr_nach"] != DBNull.Value ? reader.GetDateTime(12).ToString("yyyy-MM-dd") : null,
                        DateEndContract = reader["date_kontr_kon"] != DBNull.Value ? reader.GetDateTime(13).ToString("yyyy-MM-dd") : null,
                        DateDrop = reader["dat_drop"] != DBNull.Value ? reader.GetDateTime(14).ToString("yyyy-MM-dd") : null,
                        PositionDrop = reader.GetString(15),

                    });
                }
            }

            await reader.CloseAsync();
            return List.AsReadOnly();
        }

        #endregion
    }
}
