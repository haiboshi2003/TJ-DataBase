using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data.Common;
using System.Drawing;
using System.Security.Claims;
using web.DTO_group2;




[ApiController]
[Route("/")]
public class CityCrimeData: ControllerBase
{
    private OracleConnection _connection;

    public CityCrimeData(OracleConnection connection)
    {
        _connection = connection;
    }
    public List<string> GetDistrictName(string CityName)
    {
        List<string> DistrictName = new List<string>();
        try
        {
            _connection.Open();
            string query = "SELECT  distinct SUBSTR(address, 1, INSTR(address, '区')) as districtName" +
                " FROM  (    SELECT address    FROM citizen     WHERE address LIKE '%' || :temp || '%'  ) ";

            Console.WriteLine($"建立sql");
            OracleCommand command = new OracleCommand(query, _connection);
            Console.WriteLine($"建立cmd");
            command.Parameters.Add(new OracleParameter("temp", CityName));
            Console.WriteLine($"添加参数:{CityName}");

            using (OracleDataReader reader = command.ExecuteReader())
            {
                Console.WriteLine($"查询数据SQL为:{command.CommandText}");
                while (reader.Read())
                {
                    DistrictName.Add(reader["districtName"].ToString());
                }

            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
        finally
        {
            _connection.Close();
        }
        return DistrictName;
    }

    public List<int> GetDistrictPopulations(List<string> districtNames)
    {
        List<int> districtPopulations = new List<int>();
        try
        {
            _connection.Open();

            foreach (string districtName in districtNames)
            {
                string query1 = "SELECT COUNT(*) FROM citizen WHERE address LIKE  '%' || :temp1 || '%' ";
                OracleCommand command1 = new OracleCommand(query1, _connection);
                command1.Parameters.Add(new OracleParameter("temp1", districtName));
                decimal populationDecimal = (decimal)command1.ExecuteScalar();
                int population = Convert.ToInt32(populationDecimal);
                districtPopulations.Add(population);
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine("Error GetDistrictNums: " + ex.Message);
        }
        finally
        {
            _connection.Close();
        }
        return districtPopulations;
    }

    public List<int> GetDistrictCrimeNums(List<string> districtNames)
    {
        List<int> districtCrimes = new List<int>();
        try
        {
            _connection.Open();
            decimal crimeNumDecimal = 0;
            int crimeNum = 0;
            foreach (string districtName in districtNames)
            {
                string query2 = "SELECT COUNT(*) FROM citizen JOIN related ON citizen.ID_NUM = related.ID_NUM " +
                    "WHERE citizen.address LIKE  '%' || :temp2 || '%'  AND related.RELATED_TYPE = '嫌疑人'";
                OracleCommand command2 = new OracleCommand(query2, _connection);
                command2.Parameters.Add(new OracleParameter("temp2", districtName));
                crimeNumDecimal = (decimal)command2.ExecuteScalar();
                crimeNum = Convert.ToInt32(crimeNumDecimal);
                districtCrimes.Add(crimeNum);
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine("Error GetDistrictNums: " + ex.Message);
        }
        finally
        {
            _connection.Close();
        }
        return districtCrimes;
    }
    public List<List<int>> GetDistrictCrimeType(List<string> districtNames)
    {
        List<List<int>> DistrictCrimeTypes = new List<List<int>>();

        try
        {
            _connection.Open();

            foreach (string districtName in districtNames)
            {
                List<int> crimeTypes = new List<int>();

                string sql = "SELECT " +
                             "SUM(CASE WHEN CASE_TYPE='强奸' THEN 1 ELSE 0 END) AS count1, " +
                             "SUM(CASE WHEN CASE_TYPE='抢劫' THEN 1 ELSE 0 END) AS count2, " +
                             "SUM(CASE WHEN CASE_TYPE='故意伤害' THEN 1 ELSE 0 END) AS count3, " +
                             "SUM(CASE WHEN CASE_TYPE='盗窃' THEN 1 ELSE 0 END) AS count4, " +
                             "SUM(CASE WHEN CASE_TYPE='诈骗' THEN 1 ELSE 0 END) AS count5, " +
                             "SUM(CASE WHEN CASE_TYPE='谋杀' THEN 1 ELSE 0 END) AS count6 " +
                             "FROM CASES " +
                             "WHERE ADDRESS LIKE  '%' || :temp || '%' ";
                OracleCommand command = new OracleCommand(sql, _connection);
                command.Parameters.Add(new OracleParameter("temp", districtName));
                int index = 0;
                decimal crimeTypeNumDecimal =0;
                OracleDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    for (int i = 0; i < 6; i++)
                    {
                        crimeTypeNumDecimal = (decimal)reader[i];
                        crimeTypes.Add(Convert.ToInt32(crimeTypeNumDecimal));
                    }
                }
                Console.WriteLine($"查询数据开始:{crimeTypes.Count},{index},{crimeTypes}");
                DistrictCrimeTypes.Add(crimeTypes);
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine("Error GetDistrictCrimeType: " + ex.Message);
        }
        finally
        {
            _connection.Close();
        }

        return DistrictCrimeTypes;
    }

    public List<Dictionary<string, int>> GetDistrictCrimeTime(List<string> districtNames)
    {
        List<Dictionary<string, int>> DistrictCrimeTime = new List<Dictionary<string, int>>();

        try
        {
            _connection.Open();

            foreach (string districtName in districtNames)
            {
                Dictionary<string, int> crimeTimeDict = new Dictionary<string, int>();

                string sql = "SELECT  COUNT(*),  EXTRACT(YEAR FROM REGISTER_TIME) || '-' ||  CASE WHEN EXTRACT(MONTH FROM REGISTER_TIME) < 6 THEN '上半年' ELSE '下半年' END AS TIME_PERIOD FROM cases WHERE ADDRESS LIKE  '%' || :temp || '%'  GROUP BY EXTRACT(YEAR FROM REGISTER_TIME) || '-' ||   CASE WHEN EXTRACT(MONTH FROM REGISTER_TIME) < 6 THEN '上半年' ELSE '下半年' END";
                OracleCommand command = new OracleCommand(sql, _connection);
                command.Parameters.Add(new OracleParameter("temp", districtName));
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string timePeriod = reader.GetString(1);
                        int count = reader.GetInt32(0);

                        crimeTimeDict[timePeriod] = count;
                    }
                    DistrictCrimeTime.Add(crimeTimeDict);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error GetDistrictCrimeType: " + ex.Message);
        }
        finally
        {
            _connection.Close();
        }

        return DistrictCrimeTime;
    }

    [HttpGet("api/CityCrimeInfo")]
    public IActionResult GetCityCrimeInfo([FromQuery] string? CityName)
    {
        List<CityCrimeInfo> cityCrimeInfoList = new List<CityCrimeInfo>();
        try
        {
            Console.WriteLine("查询数据开始");
            List<string> districtNames = GetDistrictName(CityName);
            List<int> districtPopulations = GetDistrictPopulations(districtNames);
            List<int> districtCrimes = GetDistrictCrimeNums(districtNames);
            List<List<int>> crimeTypes = GetDistrictCrimeType(districtNames);
            List<Dictionary<string, int>> crimeTimes = GetDistrictCrimeTime(districtNames);
            //Console.WriteLine($"1:{crimeTypes[0].Count},{crimeTimes.Count}");
            // 赋值到对象列表
            for (int i = 0; i < districtNames.Count; i++)
            {
                CityCrimeInfo info = new CityCrimeInfo();
                Console.WriteLine($"2");
                info.DistrictName = districtNames[i];
                Console.WriteLine($"3");
                info.Population = districtPopulations[i];
                Console.WriteLine($"4");
                info.CrimeNum = districtCrimes[i];
                // 转换犯罪类型List
                List<int> crimeTypeStat = new List<int>();
                crimeTypeStat.Add(crimeTypes[i][0]); // 强奸
                crimeTypeStat.Add(crimeTypes[i][1]); // 抢劫
                crimeTypeStat.Add(crimeTypes[i][2]); //故意伤害                            
                crimeTypeStat.Add(crimeTypes[i][3]); //盗窃
                crimeTypeStat.Add(crimeTypes[i][4]); //盗窃
                crimeTypeStat.Add(crimeTypes[i][5]); //谋杀
                Console.WriteLine($"1");
                info.CrimeTypeStatistic = crimeTypeStat;

                info.DistrictCrimeTimeStatistic = crimeTimes[i];

                cityCrimeInfoList.Add(info);
               
            }
            //Console.WriteLine($"赋值结束:{cityCrimeInfoList[0].DistrictName},{cityCrimeInfoList[0].CrimeNum},{cityCrimeInfoList[0].Population}");
            return Ok(cityCrimeInfoList);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
            return StatusCode(499, $"查询数据时发生错误: {ex.Message}");
        }
        finally
        {

        }
    }
}

