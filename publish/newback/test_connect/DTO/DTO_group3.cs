using Oracle.ManagedDataAccess.Client;

// 赵毅辉编写使用
// crimeDataStatisticsControllerZYH.cs使用了本类
// 用于统计各项犯罪数据
public class CaseStatisticsZYH
{
    // 立案数
    public int numFiling { get; set; }
    // 调查数
    public int numInvestigating { get; set; }
    // 结案数
    public int numClose { get; set; }
    // 强奸案数
    public int numRape { get; set; }
    // 抢劫案数
    public int numRobbery { get; set; }
    // 故意伤害案数
    public int numInjury { get; set; }
    // 盗窃案数
    public int numTheft { get; set; }
    // 诈骗案数
    public int numFraud { get; set; }
    // 谋杀案数
    public int numMurder { get; set; }
    // 案件记录的最小年份
    public int minYear { get; set; }
    // 案件记录的最大年份
    public int maxYear { get; set; }
    // 每年每月案件数量
    public Dictionary<int, Dictionary<int, int>> numYearMonth { get; set; }
    // 存储城市名和对应次数的字典
    public Dictionary<string, int> cityCount { get; set; }
    public Dictionary<string, List<int>> cityType;
    // 每年每月案件数量
    public Dictionary<int, Dictionary<int, int>> numCityYearMonth { get; set; }

    /* 下面是方法 */
    // 构造函数
    public CaseStatisticsZYH()
    {
        numYearMonth = new Dictionary<int, Dictionary<int, int>>();
        cityCount = new Dictionary<string, int>();
        numCityYearMonth = new Dictionary<int, Dictionary<int, int>>();
        cityType = new Dictionary<string, List<int>>();
    }
    public void getStatusCityDateStatistics(string city, string year, string month)
    {
        try
        {
            string connectionString = "User ID=POLICE;Password=police;Data Source=(DESCRIPTION = (ADDRESS_LIST= (ADDRESS = (PROTOCOL = TCP)(HOST = 118.25.23.3)(PORT =1521))) (CONNECT_DATA = (SERVICE_NAME = xe)))";
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();

                string addiCondition = "1=1";

                if (year != "全部")
                    addiCondition += " AND EXTRACT(YEAR FROM REGISTER_TIME) = :year";

                if (month != "全部")
                    addiCondition += " AND EXTRACT(MONTH FROM REGISTER_TIME) = :month";

                if (city != "全部")
                    addiCondition += " AND ADDRESS LIKE '%' || :city || '%'";

                using (OracleCommand command = new OracleCommand("SELECT COUNT(*) FROM CASES WHERE STATUS = '立案' AND " + addiCondition, connection))
                {
                    if (year != "全部")
                        command.Parameters.Add(new OracleParameter("year", OracleDbType.Varchar2) { Value = year });
                    if (month != "全部")
                        command.Parameters.Add(new OracleParameter("month", OracleDbType.Varchar2) { Value = month });
                    if (city != "全部")
                        command.Parameters.Add(new OracleParameter("city", OracleDbType.Varchar2) { Value = city });

                    numFiling = Convert.ToInt32(command.ExecuteScalar());
                }

                // 类似地生成查询“调查”和“结案”的代码
                using (OracleCommand command = new OracleCommand("SELECT COUNT(*) FROM CASES WHERE STATUS = '调查' AND " + addiCondition, connection))
                {
                    if (year != "全部")
                        command.Parameters.Add(new OracleParameter("year", OracleDbType.Varchar2) { Value = year });
                    if (month != "全部")
                        command.Parameters.Add(new OracleParameter("month", OracleDbType.Varchar2) { Value = month });
                    if (city != "全部")
                        command.Parameters.Add(new OracleParameter("city", OracleDbType.Varchar2) { Value = city });

                    numInvestigating = Convert.ToInt32(command.ExecuteScalar());
                }

                using (OracleCommand command = new OracleCommand("SELECT COUNT(*) FROM CASES WHERE STATUS = '结案' AND " + addiCondition, connection))
                {
                    if (year != "全部")
                        command.Parameters.Add(new OracleParameter("year", OracleDbType.Varchar2) { Value = year });
                    if (month != "全部")
                        command.Parameters.Add(new OracleParameter("month", OracleDbType.Varchar2) { Value = month });
                    if (city != "全部")
                        command.Parameters.Add(new OracleParameter("city", OracleDbType.Varchar2) { Value = city });

                    numClose = Convert.ToInt32(command.ExecuteScalar());
                }

                connection.Close();
            }
        }
        catch (Exception ex)
        {
            // 处理异常
            Console.WriteLine("CaseStatisticsZYH类的getStatusCityDateStatistics函数发生异常：" + ex.Message);
            throw;
        }
    }
    public void getTypeCityDateStatistics(string city, string year, string month)
    {
        try
        {
            string connectionString = "User ID=POLICE;Password=police;Data Source=(DESCRIPTION = (ADDRESS_LIST= (ADDRESS = (PROTOCOL = TCP)(HOST = 118.25.23.3)(PORT =1521))) (CONNECT_DATA = (SERVICE_NAME = xe)))";
            {
                connection.Open();

                string addiCondition = "1=1";

                if (year != "全部")
                    addiCondition += " AND EXTRACT(YEAR FROM REGISTER_TIME) = :year";

                if (month != "全部")
                    addiCondition += " AND EXTRACT(MONTH FROM REGISTER_TIME) = :month";

                if (city != "全部")
                    addiCondition += " AND ADDRESS LIKE '%' || :city || '%'";

                using (OracleCommand command = new OracleCommand("SELECT COUNT(*) FROM CASES WHERE CASE_TYPE = '强奸' AND " + addiCondition, connection))
                {
                    if (year != "全部")
                        command.Parameters.Add(new OracleParameter("year", OracleDbType.Varchar2) { Value = year });
                    if (month != "全部")
                        command.Parameters.Add(new OracleParameter("month", OracleDbType.Varchar2) { Value = month });
                    if (city != "全部")
                        command.Parameters.Add(new OracleParameter("city", OracleDbType.Varchar2) { Value = city });

                    numRape = Convert.ToInt32(command.ExecuteScalar());
                }

                using (OracleCommand command = new OracleCommand("SELECT COUNT(*) FROM CASES WHERE CASE_TYPE = '抢劫' AND " + addiCondition, connection))
                {
                    if (year != "全部")
                        command.Parameters.Add(new OracleParameter("year", OracleDbType.Varchar2) { Value = year });
                    if (month != "全部")
                        command.Parameters.Add(new OracleParameter("month", OracleDbType.Varchar2) { Value = month });
                    if (city != "全部")
                        command.Parameters.Add(new OracleParameter("city", OracleDbType.Varchar2) { Value = city });

                    numRobbery = Convert.ToInt32(command.ExecuteScalar());
                }

                using (OracleCommand command = new OracleCommand("SELECT COUNT(*) FROM CASES WHERE CASE_TYPE = '故意伤害' AND " + addiCondition, connection))
                {
                    if (year != "全部")
                        command.Parameters.Add(new OracleParameter("year", OracleDbType.Varchar2) { Value = year });
                    if (month != "全部")
                        command.Parameters.Add(new OracleParameter("month", OracleDbType.Varchar2) { Value = month });
                    if (city != "全部")
                        command.Parameters.Add(new OracleParameter("city", OracleDbType.Varchar2) { Value = city });

                    numInjury = Convert.ToInt32(command.ExecuteScalar());
                }

                using (OracleCommand command = new OracleCommand("SELECT COUNT(*) FROM CASES WHERE CASE_TYPE = '盗窃' AND " + addiCondition, connection))
                {
                    if (year != "全部")
                        command.Parameters.Add(new OracleParameter("year", OracleDbType.Varchar2) { Value = year });
                    if (month != "全部")
                        command.Parameters.Add(new OracleParameter("month", OracleDbType.Varchar2) { Value = month });
                    if (city != "全部")
                        command.Parameters.Add(new OracleParameter("city", OracleDbType.Varchar2) { Value = city });

                    numTheft = Convert.ToInt32(command.ExecuteScalar());
                }

                using (OracleCommand command = new OracleCommand("SELECT COUNT(*) FROM CASES WHERE CASE_TYPE = '诈骗' AND " + addiCondition, connection))
                {
                    if (year != "全部")
                        command.Parameters.Add(new OracleParameter("year", OracleDbType.Varchar2) { Value = year });
                    if (month != "全部")
                        command.Parameters.Add(new OracleParameter("month", OracleDbType.Varchar2) { Value = month });
                    if (city != "全部")
                        command.Parameters.Add(new OracleParameter("city", OracleDbType.Varchar2) { Value = city });

                    numFraud = Convert.ToInt32(command.ExecuteScalar());
                }

                using (OracleCommand command = new OracleCommand("SELECT COUNT(*) FROM CASES WHERE CASE_TYPE = '谋杀' AND " + addiCondition, connection))
                {
                    if (year != "全部")
                        command.Parameters.Add(new OracleParameter("year", OracleDbType.Varchar2) { Value = year });
                    if (month != "全部")
                        command.Parameters.Add(new OracleParameter("month", OracleDbType.Varchar2) { Value = month });
                    if (city != "全部")
                        command.Parameters.Add(new OracleParameter("city", OracleDbType.Varchar2) { Value = city });

                    numMurder = Convert.ToInt32(command.ExecuteScalar());
                }

                connection.Close();
            }
        }
        catch (Exception ex)
        {
            // 处理异常
            Console.WriteLine("CaseStatisticsZYH类的getTypeCityDateStatistics函数发生异常：" + ex.Message);
            throw;
        }
    }
    public void getYearRange()
    {
        try
        {
            string connectionString = "User ID=POLICE;Password=police;Data Source=(DESCRIPTION = (ADDRESS_LIST= (ADDRESS = (PROTOCOL = TCP)(HOST = 118.25.23.3)(PORT =1521))) (CONNECT_DATA = (SERVICE_NAME = xe)))";
            {
                connection.Open();

                string sqlQuery = "SELECT MIN(EXTRACT(YEAR FROM REGISTER_TIME)) AS min_year, MAX(EXTRACT(YEAR FROM REGISTER_TIME)) AS max_year FROM CASES";
                using (OracleCommand command = new OracleCommand(sqlQuery, connection))
                {
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            minYear = Convert.ToInt32(reader["min_year"]);
                            maxYear = Convert.ToInt32(reader["max_year"]);
                        }
                    }
                }

                connection.Close();
            }
        }
        catch (Exception ex)
        {
            // 处理异常
            Console.WriteLine("CaseStatisticsZYH类的getYearRange函数发生异常：" + ex.Message);
            throw;
        }
    }
    public void getYearMonthStatistics()
    {
        try
        {
            string connectionString = "User ID=POLICE;Password=police;Data Source=(DESCRIPTION = (ADDRESS_LIST= (ADDRESS = (PROTOCOL = TCP)(HOST = 118.25.23.3)(PORT =1521))) (CONNECT_DATA = (SERVICE_NAME = xe)))";
            {
                getYearRange();

                connection.Open();

                for (int year = minYear; year <= maxYear; year++)
                {
                    Dictionary<int, int> numMonth = new Dictionary<int, int>();

                    for (int month = 1; month <= 12; month++)
                    {
                        string sql = $"SELECT COUNT(*) FROM CASES WHERE EXTRACT(YEAR FROM REGISTER_TIME) = {year} AND EXTRACT(MONTH FROM REGISTER_TIME) = {month}";
                        using (OracleCommand command = new OracleCommand(sql, connection))
                        {
                            int count = Convert.ToInt32(command.ExecuteScalar());
                            numMonth.Add(month, count);
                        }
                    }

                    numYearMonth.Add(year, numMonth);
                }

                connection.Close();
            }
        }
        catch (Exception ex)
        {
            // 处理异常
            Console.WriteLine("CaseStatisticsZYH类的getYearMonthStatistics函数发生异常：" + ex.Message);
            throw;
        }
    }
    public void getCityStatistics()
    {
        try
        {
            string connectionString = "User ID=POLICE;Password=police;Data Source=(DESCRIPTION = (ADDRESS_LIST= (ADDRESS = (PROTOCOL = TCP)(HOST = 118.25.23.3)(PORT =1521))) (CONNECT_DATA = (SERVICE_NAME = xe)))";
            {
                connection.Open();

                using (OracleCommand command = new OracleCommand("SELECT SUBSTR(ADDRESS, 1, INSTR(ADDRESS, '市')-1) AS CITY, COUNT(*) AS ROW_COUNT FROM CASES GROUP BY SUBSTR(ADDRESS, 1, INSTR(ADDRESS, '市')-1)", connection))
                {
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string city = reader["CITY"].ToString();
                            int count = Convert.ToInt32(reader["ROW_COUNT"]);

                            cityCount.Add(city, count);
                        }
                    }
                }

                connection.Close();
            }
        }
        catch (Exception ex)
        {
            // 处理异常
            Console.WriteLine("CaseStatisticsZYH类的getCityStatistics函数发生异常：" + ex.Message);
            throw;
        }
    }
    public void getCityTypeStatistics()
    {
        try
        {
            cityType = new Dictionary<string, List<int>>();
            // 配置数据库连接字符串
            string connectionString = "User ID=POLICE;Password=police;Data Source=(DESCRIPTION = (ADDRESS_LIST= (ADDRESS = (PROTOCOL = TCP)(HOST = 118.25.23.3)(PORT =1521))) (CONNECT_DATA = (SERVICE_NAME = xe)))";
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();

                // 查询所有城市
                using (OracleCommand command = new OracleCommand("SELECT DISTINCT SUBSTR(ADDRESS, 1, INSTR(ADDRESS,'市')-1) AS City FROM CASES", connection))
                {
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string city = reader["City"].ToString();
                            cityType.Add(city, new List<int>());
                        }
                    }
                }

                // 查询每个城市的每种案件数量
                foreach (string city in cityType.Keys)
                {
                    using (OracleCommand command = new OracleCommand("SELECT COUNT(*) FROM CASES WHERE ADDRESS LIKE '%' || :city || '%' AND CASE_TYPE = :caseType", connection))
                    {
                        command.Parameters.Add(new OracleParameter("city", OracleDbType.Varchar2) { Value = city });

                        command.Parameters.Add(new OracleParameter("caseType", OracleDbType.Varchar2));

                        // 统计每种案件的数量
                        foreach (string caseType in new List<string> { "强奸", "抢劫", "故意伤害", "盗窃", "诈骗", "谋杀" })
                        {
                            command.Parameters["caseType"].Value = caseType;

                            int count = Convert.ToInt32(command.ExecuteScalar());

                            cityType[city].Add(count);
                        }
                    }
                }

                connection.Close();
            }
        }
        catch (Exception ex)
        {
            // 处理异常
            Console.WriteLine("CaseStatisticsZYH类的getCityTypeStatistics函数发生异常：" + ex.Message);
            throw;
        }
    }
    public void getCityYearMonthStatistics(string cityName)
    {
        try
        {
            string connectionString = "User ID=POLICE;Password=police;Data Source=(DESCRIPTION = (ADDRESS_LIST= (ADDRESS = (PROTOCOL = TCP)(HOST = 118.25.23.3)(PORT =1521))) (CONNECT_DATA = (SERVICE_NAME = xe)))";
            {
                getYearRange();

                connection.Open();

                for (int year = minYear; year <= maxYear; year++)
                {
                    Dictionary<int, int> numMonth = new Dictionary<int, int>();

                    for (int month = 1; month <= 12; month++)
                    {
                        string sql = $"SELECT COUNT(*) FROM CASES WHERE EXTRACT(YEAR FROM REGISTER_TIME) = {year} AND EXTRACT(MONTH FROM REGISTER_TIME) = {month} AND ADDRESS LIKE '%{cityName}%'";
                        using (OracleCommand command = new OracleCommand(sql, connection))
                        {
                            int count = Convert.ToInt32(command.ExecuteScalar());
                            numMonth.Add(month, count);
                        }
                    }

                    numCityYearMonth.Add(year, numMonth);
                }

                connection.Close();
            }
        }
        catch (Exception ex)
        {
            // 处理异常
            Console.WriteLine("CaseStatisticsZYH类的getCityYearMonthStatistics函数发生异常：" + ex.Message);
            throw;
        }
    }
}
// 赵毅辉编写使用
// caseControllerZYHZBW.cs使用了本类
// 用于储存返回给前端的CASES表的查询数据
public class caseInfoZYH
{
    public string caseID { get; set; }
    public string caseType { get; set; }
    public string status { get; set; }
    public DateTime registerTime { get; set; }
    public string address { get; set; }
    public string ranking { get; set; }
}

// 赵毅辉编写使用
// caseControllerZYHZBW.cs使用了本类
// 用于储存前端传来的数据
public class inputCaseInfoZYH
{
    public string caseID { get; set; }
    public string caseType { get; set; }
    public string status { get; set; }
    public string address { get; set; }
    public string ranking { get; set; }
}


// 赵毅辉编写使用
// citizenControllerZYHZBW.cs使用了本类
// 用于储存返回给前端的CITIZEN表的查询数据
public class citizenInfoZYH
{
    public string IDNum { get; set; }
    public string citizenName { get; set; }
    public string gender { get; set; }
    public string fatherID { get; set; }
    public string motherID { get; set; }
}

// 赵毅辉编写使用
// citizenControllerZYHZBW.cs使用了本类
// 用于储存前端传来的数据
public class inputCitizenInfoZYH
{
    public string IDNum { get; set; }
    public string citizenName { get; set; }
    public string gender { get; set; }
}

// 赵毅辉编写使用
// citizenInCaseControllerZYHZBW.cs使用了本类
// 用于储存返回给前端的CITIZEN表、CASES表的查询数据
public class citizenInCaseInfoZYH
{
    public string caseID { get; set; }
    public string caseType { get; set; }
    public string status { get; set; }
    public DateTime registerTime { get; set; }
    public string address { get; set; }
    public string ranking { get; set; }
    public string IDNum { get; set; }
    public string relatedType { get; set; }
    public string citizenName { get; set; }
    public string gender { get; set; }
}

// 赵毅辉编写使用
// citizenInCaseControllerZYHZBW.cs使用了本类
// 用于储存前端传来的数据
public class inputCitizenInCaseInfoZYH
{
    public string caseID { get; set; }
    public string caseType { get; set; }
    public string status { get; set; }
    public string address { get; set; }
    public string ranking { get; set; }
    public string IDNum { get; set; }
    public string relatedType { get; set; }
}

// 赵毅辉编写使用
// PolicemenControllerZYH.cs使用了本类
// 用于储存返回给前端的POLICE表的查询数据
public class PolicemenInfoZYH
{
    public string policemenNumber { get; set; }
    public string policemenName { get; set; }
    public string IDNumber { get; set; }
    public string birthday { get; set; }
    public string gender { get; set; }
    public string nation { get; set; }
    public string phoneNumber { get; set; }
    public string policemenStatus { get; set; }
    public string policemenPosition { get; set; }
}

// 赵毅辉编写使用
// PolicemenControllerZYH.cs使用了本类
// 用于储存前端传来的数据
public class inputPolicemenInfoZYH
{
    public string policemenNumber { get; set; }
    public string policemenName { get; set; }
    public string policemenStatus { get; set; }
    public string policemenPosition { get; set; }
}

// 赵毅辉编写使用
// PolicemenControllerZYHZBW.cs使用了本类
// 用于储存前端传来的数据 & 返回给前端的STATION表的查询数据
public class StationInfoZYH
{
    public string stationID { get; set; }
    public string stationName { get; set; }
    public string city { get; set; }
    public string address { get; set; }
    public int? budget { get; set; }
}

// 赵毅辉编写使用
// videoControllerZYHZBW.cs使用了本类
// 用于储存返回给前端的VIDEO表的查询数据
public class videoInfoZYH
{
    public string videoID { get; set; }
    public string videoType { get; set; }
    public DateTime recordTime { get; set; }
    public DateTime uploadTime { get; set; }
    public string principleID { get; set; }
}

// 赵毅辉编写使用
// videoControllerZYHZBW.cs使用了本类
// 用于储存前端传来的数据
public class inputVideoInfoZYH
{
    public string videoID { get; set; }
    public string videoType { get; set; }
    public string principleID { get; set; }
}
// 赵勃维编写使用
// equipControllerZBW.cs使用了本类
// 用于储存后端返回的数据
public class EquipInfo
{
    public string EquipID { get; set; }
    public string EquipType { get; set; }
    public string EquipStatus { get; set; }
    public string EquipStation { get; set; }

}
// 赵勃维编写使用
// equipControllerZBW.cs使用了本类
// 用于储存前端传来的数据
public class inputEquipInfo
{
    public string EquipID { get; set; }
    public string EquipType { get; set; }
    public string Equipstaion { get; set; }
}
// 赵勃维编写使用
// equipControllerZBW.cs使用了本类
// 用于储存后端使用部分的数据
public class EquipUseInfo
{
    public string equipID { get; set; }
    public string policemenID { get; set; }
    public DateTime borrowtime { get; set; }
    public DateTime? returntime { get; set; }
}
// 赵勃维编写使用
// attendControllerZBW.cs使用了本类
// 用于储存前端传来的数据
public class attendinfo
{
    public string policemanNumber { get; set; }
    public string area { get; set; }
    public string time { get; set; }
}
// 赵勃维编写使用
// callControllerZBW.cs使用了本类
// 用于储存前端传来的数据
public class callinfo
{
    public string audioid { get; set; }
    public string calltime { get; set; }
    public string caseid { get; set; }
    public string phonenumber { get; set; }
    public string answernumber { get; set; }
}


// 赵勃维编写使用
// vehicleControllerZBW.cs使用了本类
// 用于储存后端使用部分的数据
public class inputVehicleinfo
{
    public string VID { get; set; }
    public string VTYPE { get; set; }
    public string VST { get; set; }
}
// 赵勃维编写使用
// vehicleControllerZBW.cs使用了本类
// 用于储存后端使用部分的数据
public class Vehicleinfo
{
    public string Vehicle_ID { get; set; }
    public string Vehicle_Type { get; set; }
    public string Status { get; set; }
}
public class Vehicleuseinfo
{
    public string VehicleID { get; set; }
    public string StationID { get; set; }
    public DateTime borrowtime { get; set; }
    public DateTime? returntime { get; set; }
}


// 赵毅辉编写使用
// keyIndividualsControllerZYH.cs使用了本类
// 用于实现查询重点人员的方法

// 赵毅辉编写使用
// keyIndividualsControllerZYH.cs使用了本类
// 用于实现查询重点人员的方法
public class keyIndividualsZYH
{
    public List<string> repeatOffendersName;
    public List<Dictionary<string, string>> repeatOffendersInfo;
    public List<Dictionary<string, string>> repeatOffendersCase;
    public keyIndividualsZYH()
    {
        repeatOffendersName = new List<string>();
        repeatOffendersInfo = new List<Dictionary<string, string>>();
        repeatOffendersCase = new List<Dictionary<string, string>>();
    }
    public void getRepeatOffenderNameStatistics()
    {
        repeatOffendersInfo = new List<Dictionary<string, string>>();
        string connectionString = "User ID=POLICE;Password=police;Data Source=(DESCRIPTION = (ADDRESS_LIST= (ADDRESS = (PROTOCOL = TCP)(HOST = 118.25.23.3)(PORT =1521))) (CONNECT_DATA = (SERVICE_NAME = xe)))";
        using (OracleConnection connection = new OracleConnection(connectionString))
        {
            connection.Open();
            string query = "SELECT ID_NUM FROM RELATED WHERE RELATED_TYPE = '犯人' GROUP BY ID_NUM HAVING COUNT(*) > 1";

            using (OracleCommand command = new OracleCommand(query, connection))
            {
                using (OracleDataReader reader = command.ExecuteReader())
                {
                    repeatOffendersName = new List<string>();

                    while (reader.Read())
                    {
                        string idNum = reader.GetString(reader.GetOrdinal("ID_NUM"));
                        repeatOffendersName.Add(idNum);
                    }
                }
            }
            connection.Close();
        }
    }
    public void getRepeatOffenderInfoStatistics()
    {
        getRepeatOffenderNameStatistics();

        repeatOffendersInfo = new List<Dictionary<string, string>>();
        string connectionString = "User ID=POLICE;Password=police;Data Source=(DESCRIPTION = (ADDRESS_LIST= (ADDRESS = (PROTOCOL = TCP)(HOST = 118.25.23.3)(PORT =1521))) (CONNECT_DATA = (SERVICE_NAME = xe)))";
        using (OracleConnection connection = new OracleConnection(connectionString))
        {
            connection.Open();
            string query = "SELECT ID_NUM, CITIZEN_NAME, GENDER, ADDRESS FROM CITIZEN WHERE ID_NUM = :IDNum";

            using (OracleCommand command = new OracleCommand(query, connection))
            {
                OracleParameter parameter = new OracleParameter(":IDNum", OracleDbType.Varchar2);

                foreach (string idNum in repeatOffendersName)
                {
                    parameter.Value = idNum;
                    command.Parameters.Add(parameter);

                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string idNumValue = reader.GetString(reader.GetOrdinal("ID_NUM"));
                            string citizenName = reader.GetString(reader.GetOrdinal("CITIZEN_NAME"));
                            string gender = reader.GetString(reader.GetOrdinal("GENDER"));
                            string Caddress = reader.GetString(reader.GetOrdinal("ADDRESS"));

                            // 转换GENDER字段的值为"女"和"男"
                            if (gender == "F")
                            {
                                gender = "女";
                            }
                            else if (gender == "M")
                            {
                                gender = "男";
                            }

                            Dictionary<string, string> result = new Dictionary<string, string>
                            {
                                { "身份证号", idNumValue },
                                { "姓名", citizenName },
                                { "性别", gender },
                                { "住址", Caddress },
                            };

                            repeatOffendersInfo.Add(result);
                        }
                    }

                    // 清除参数，以便下一次循环使用相同的 command 对象
                    command.Parameters.Clear();
                }
            }

            connection.Close();
        }
    }
    public void getRepeatOffenderFilterStatistics(string ID, string name, string sex, string address)
    {
        getRepeatOffenderNameStatistics();
        repeatOffendersInfo = new List<Dictionary<string, string>>();
        string connectionString = "User ID=POLICE;Password=police;Data Source=(DESCRIPTION = (ADDRESS_LIST= (ADDRESS = (PROTOCOL = TCP)(HOST = 118.25.23.3)(PORT =1521))) (CONNECT_DATA = (SERVICE_NAME = xe)))";
        using (OracleConnection connection = new OracleConnection(connectionString))
        {
            connection.Open();

            // 第一轮筛选
            string firstQuery = "SELECT ID_NUM, CITIZEN_NAME, GENDER, ADDRESS FROM CITIZEN WHERE 1 = 1";

            if (!string.IsNullOrEmpty(ID))
            {
                firstQuery += " AND ID_NUM LIKE '%' || :CID || '%'";
            }

            if (!string.IsNullOrEmpty(name))
            {
                firstQuery += " AND CITIZEN_NAME LIKE '%' || :CName || '%'";
            }

            if (!string.IsNullOrEmpty(sex) && sex != "全部")
            {
                firstQuery += " AND GENDER = :CGender";
            }

            if (!string.IsNullOrEmpty(address))
            {
                firstQuery += " AND ADDRESS LIKE '%' || :CAddress || '%'";
            }

            using (OracleCommand firstCommand = new OracleCommand(firstQuery, connection))
            {
                if (!string.IsNullOrEmpty(ID))
                {
                    OracleParameter parameterID = new OracleParameter(":CID", OracleDbType.Varchar2);
                    parameterID.Value = ID;
                    firstCommand.Parameters.Add(parameterID);
                }

                if (!string.IsNullOrEmpty(name))
                {
                    OracleParameter parameterName = new OracleParameter(":CName", OracleDbType.Varchar2);
                    parameterName.Value = name;
                    firstCommand.Parameters.Add(parameterName);
                }

                if (!string.IsNullOrEmpty(sex) && sex != "全部")
                {
                    OracleParameter parameterSex = new OracleParameter(":CGender", OracleDbType.Varchar2);
                    parameterSex.Value = sex;
                    firstCommand.Parameters.Add(parameterSex);
                }

                if (!string.IsNullOrEmpty(address))
                {
                    OracleParameter parameterAddress = new OracleParameter(":CAddress", OracleDbType.Varchar2);
                    parameterAddress.Value = address;
                    firstCommand.Parameters.Add(parameterAddress);
                }
                // 存储第一轮筛选结果的列表
                List<string> filteredIdNums = new List<string>();

                using (OracleDataReader firstReader = firstCommand.ExecuteReader())
                {
                    while (firstReader.Read())
                    {
                        string idNumValue = firstReader.GetString(firstReader.GetOrdinal("ID_NUM"));
                        filteredIdNums.Add(idNumValue);
                    }
                }


                // 第二轮筛选
                string secondQuery = "SELECT ID_NUM, CITIZEN_NAME, GENDER, ADDRESS FROM CITIZEN WHERE ID_NUM IN (:IDList)";

                using (OracleCommand secondCommand = new OracleCommand(secondQuery, connection))
                {
                    OracleParameter parameter = new OracleParameter(":IDList", OracleDbType.Varchar2);
                    foreach (string idNum in repeatOffendersName)
                    {
                        if (filteredIdNums.Contains(idNum))
                        {
                            parameter.Value = idNum;
                            secondCommand.Parameters.Add(parameter);

                            using (OracleDataReader secondReader = secondCommand.ExecuteReader())
                            {
                                while (secondReader.Read())
                                {
                                    string idNumValue = secondReader.GetString(secondReader.GetOrdinal("ID_NUM"));
                                    string citizenName = secondReader.GetString(secondReader.GetOrdinal("CITIZEN_NAME"));
                                    string gender = secondReader.GetString(secondReader.GetOrdinal("GENDER"));
                                    string Caddress = secondReader.GetString(secondReader.GetOrdinal("ADDRESS"));

                                    // 转换 GENDER 字段的值为 "女" 和 "男"
                                    if (gender == "F")
                                    {
                                        gender = "女";
                                    }
                                    else if (gender == "M")
                                    {
                                        gender = "男";
                                    }

                                    Dictionary<string, string> result = new Dictionary<string, string>
                                    {
                                        { "身份证号", idNumValue },
                                        { "姓名", citizenName },
                                        { "性别", gender },
                                        { "住址", Caddress },
                                    };

                                    repeatOffendersInfo.Add(result);
                                }
                            }

                            // 清除参数，以便下一次循环使用相同的 command 对象
                            secondCommand.Parameters.Clear();
                        }
                    }
                    // foreach (Dictionary<string, string> dict in repeatOffendersInfo)
                    // {
                    // foreach (KeyValuePair<string, string> pair in dict)
                    // {
                    // Console.WriteLine($"{pair.Key}: {pair.Value}");
                    // }
                    // Console.WriteLine();
                    // }
                }
            }

            connection.Close();
        }
    }
    public void getRepeatOffenderCaseStatistics(string id)
    {
        repeatOffendersCase = new List<Dictionary<string, string>>();

        string connectionString = "User ID=POLICE;Password=police;Data Source=(DESCRIPTION = (ADDRESS_LIST= (ADDRESS = (PROTOCOL = TCP)(HOST = 118.25.23.3)(PORT =1521))) (CONNECT_DATA = (SERVICE_NAME = xe)))";
        using (OracleConnection connection = new OracleConnection(connectionString))
        {
            connection.Open();

            string query = "SELECT CASES.CASE_ID, CASE_TYPE, STATUS, REGISTER_TIME, ADDRESS, RANKING FROM CASES, RELATED WHERE CASES.CASE_ID = RELATED.CASE_ID AND ID_NUM = :CID";

            using (OracleCommand command = new OracleCommand(query, connection))
            {
                OracleParameter parameter = new OracleParameter(":CID", OracleDbType.Varchar2);
                parameter.Value = id;
                command.Parameters.Add(parameter);

                using (OracleDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string caseId = reader.GetString(reader.GetOrdinal("CASE_ID"));
                        string caseType = reader.GetString(reader.GetOrdinal("CASE_TYPE"));
                        string status = reader.GetString(reader.GetOrdinal("STATUS"));
                        DateTime registerTime = reader.GetDateTime(reader.GetOrdinal("REGISTER_TIME"));
                        string address = reader.GetString(reader.GetOrdinal("ADDRESS"));
                        string ranking = reader.GetString(reader.GetOrdinal("RANKING"));

                        Dictionary<string, string> result = new Dictionary<string, string>
                        {
                            { "案件编号", caseId },
                            { "案件类型", caseType },
                            { "案件状态", status },
                            { "登记时间", registerTime.ToString("yyyy-MM-dd HH:mm:ss") },
                            { "案发地点", address },
                            { "案件等级", ranking }
                        };

                        repeatOffendersCase.Add(result);
                    }
                }
            }

            connection.Close();
        }
    }

}


