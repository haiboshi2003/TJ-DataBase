using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor.Infrastructure;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;
using System.Numerics;
using System.Data.Common;
using System.Reflection.PortableExecutable;
using System.Text;

[Route("api/caseInvestigation/queryCase")]
[ApiController]
public class caseQueryControllerZYH : ControllerBase
{
    private readonly OracleConnection _connection;
    public caseQueryControllerZYH(OracleConnection connection)
    {
        _connection = connection;
    }

    [HttpGet]
    public IActionResult HandleEndpoint([FromQuery] string? caseID)
    {
        var cases = new List<object>();
        try
        {
            _connection.Open();
            // 执行 SQL 查询
            using (var command = _connection.CreateCommand())
            {
                command.Connection = _connection;
                //先写SQL语句的主体
                command.CommandText = "SELECT * FROM CASES WHERE STATUS = '立案'";
                //下面的whereClause可以接到SQL语句的后面，实现SQL语句的动态变化
                StringBuilder whereClause = new StringBuilder();

                if (!string.IsNullOrEmpty(caseID))
                {
                    //下面的SQL表示筛选出包含inputInfo.caseID的结果
                    whereClause.Append(" AND CASE_ID LIKE '%' || :caseID || '%'");
                    command.Parameters.Add(":caseID", OracleDbType.Varchar2).Value = caseID;
                }

                if (whereClause.Length > 0)
                {
                    command.CommandText += whereClause.ToString();
                }

                using (OracleDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var acase = new
                        {
                             caseID = reader.GetString(reader.GetOrdinal("CASE_ID")),
                             caseType = reader.GetString(reader.GetOrdinal("CASE_TYPE")),
                             status = reader.GetString(reader.GetOrdinal("STATUS")),
                             registerTime = reader.GetDateTime(reader.GetOrdinal("REGISTER_TIME")),
                             address = reader.GetString(reader.GetOrdinal("ADDRESS")),
                             ranking = reader.GetString(reader.GetOrdinal("RANKING")),
                        };

                        cases.Add(acase);
                    }

                    _connection.Close();
                    return Ok(cases);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"queryCase查询数据时发生错误:{ex.Message}");
            return StatusCode(499, "查询数据时发生错误！");
        }
        finally
        {
            _connection.Close();
        }
    }
}

[Route("api/caseInvestigation/queryPolicemen")]
[ApiController]
public class policemenQueryControllerZYH : ControllerBase
{
    private readonly OracleConnection _connection;
    public policemenQueryControllerZYH(OracleConnection connection)
    {
        _connection = connection;
    }

    [HttpGet]
    public IActionResult HandleEndpoint([FromQuery] string? policemenID)
    {
        var men = new List<object>();
        try
        {
            _connection.Open();
            // 执行 SQL 查询
            using (var command = _connection.CreateCommand())
            {
                command.Connection = _connection;
                //先写SQL语句的主体
                command.CommandText = "SELECT POLICE_NUMBER,POLICE_NAME,GENDER,NATION,STATUS,POSITION FROM POLICEMEN WHERE STATUS = '在职'";
                //下面的whereClause可以接到SQL语句的后面，实现SQL语句的动态变化
                StringBuilder whereClause = new StringBuilder();

                if (!string.IsNullOrEmpty(policemenID))
                {
                    //下面的SQL表示筛选出包含inputInfo.caseID的结果
                    whereClause.Append(" AND POLICE_NUMBER LIKE '%' || :policemenID || '%'");
                    command.Parameters.Add(":policemenID", OracleDbType.Varchar2).Value = policemenID;
                }

                if (whereClause.Length > 0)
                {
                    command.CommandText += whereClause.ToString();
                }

                using (OracleDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var man = new
                        {
                            policeNum = reader.GetString(reader.GetOrdinal("POLICE_NUMBER")),
                            policeName = reader.GetString(reader.GetOrdinal("POLICE_NAME")),
                            gender = reader.GetString(reader.GetOrdinal("GENDER")) == "M" ? "男" : "女",
                            nation = reader.GetString(reader.GetOrdinal("NATION")),
                            status = reader.GetString(reader.GetOrdinal("STATUS")),
                            position = reader.GetString(reader.GetOrdinal("POSITION")),
                        };
                        
                        men.Add(man);
                    }

                    _connection.Close();
                    return Ok(men);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"queryPolicemen查询数据时发生错误:{ex.Message}");
            return StatusCode(499, "查询数据时发生错误！");
        }
        finally
        {
            _connection.Close();
        }
    }
}

[Route("api/caseInvestigation/modifyCaseStatus")]
[ApiController]
public class ModifyControllerZYH : ControllerBase
{
    private readonly OracleConnection _connection;
    public ModifyControllerZYH(OracleConnection connection)
    {
        _connection = connection;
    }

    [HttpPut]
    public IActionResult HandleEndpoint([FromQuery] string? caseID)
    {        
        try
        {
            _connection.Open();

            string sql = "update cases " +
                "set status='调查' " +
                "where case_ID ='" + caseID + "'";
            OracleCommand command = new OracleCommand(sql, _connection);
            command.ExecuteNonQuery();
        }
        catch (Exception ex)
        {

            Console.WriteLine($"modifyCaseStatus查询数据时发生错误:{ex.Message}");
            return StatusCode(499, "修改数据时发生错误！");
        }
        finally
        {
            _connection.Close();
        }
        return Ok("");
    }
}