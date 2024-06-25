using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor.Infrastructure;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;
using System.Numerics;
using System.Data.Common;
using System.Reflection.PortableExecutable;
using System.Text;


public class CaseRequestData
{
    public string inputTextCase { get; set; }
}

public class PoliceRequestData
{
    public string inputTextPolice { get; set; }
}

public class ModifyRequestData
{
    public string inputCase { get; set; }
}

// 不再使用
[Route("api/queryCase")]
[ApiController]
public class CaseController : ControllerBase
{
    private readonly OracleConnection _connection;
    public CaseController(OracleConnection connection)
    {
        _connection = connection;
    }

    [HttpPost]
    public ActionResult<IEnumerable<string>> CaseQuery(CaseRequestData requestData)
    {
        string input = requestData.inputTextCase;
        string sql = "select case_ID,case_type,status,ranking " +
            "from cases " +
            "where case_ID='" + input + "'";
        string[] tmp = new string[4];
        try
        {
            _connection.Open();
            OracleCommand command = new OracleCommand(sql, _connection);
            OracleDataReader reader = command.ExecuteReader();
            if (reader.HasRows==false)
            {
                return Ok(tmp);
            }
            else
            {
                while (reader.Read())
                {
                    tmp[0] = reader.GetString("case_ID");
                    tmp[1] = reader.GetString("case_type");
                    tmp[2] = reader.GetString("status");
                    tmp[3] = reader.GetString("ranking");
                }
            }
        }
        catch(Exception ex)
        {
            return StatusCode(500, $"查询数据时发生错误: {ex.Message}");
        }
        finally
        {
            _connection.Close();
        }
        return Ok(tmp);
    }
}

// 不再使用
[Route("api/queryPolice")]
[ApiController]
public class PoliceController : ControllerBase
{
    private readonly OracleConnection _connection;
    public PoliceController(OracleConnection connection)
    {
        _connection = connection;
    }

    [HttpPost]
    public ActionResult<IEnumerable<string>> PoliceQuery(PoliceRequestData requestData)
    {
        string input = requestData.inputTextPolice;
        string sql = "select police_number,police_name,phone_number,status,position " +
            "from policemen " +
            "where police_number='" + input + "'";
        string[] tmp = new string[5];
        try
        {
            _connection.Open();
            OracleCommand command = new OracleCommand(sql, _connection);
            OracleDataReader reader = command.ExecuteReader();
            if (reader.HasRows==false)
            {
                return Ok(tmp);
            }
            else
            {
                while (reader.Read())
                {
                    tmp[0] = reader.GetString("police_number");
                    tmp[1] = reader.GetString("police_name");
                    tmp[2] = reader.GetString("phone_number");
                    tmp[3] = reader.GetString("status");
                    tmp[4] = reader.GetString("position");
                }
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"查询数据时发生错误: {ex.Message}");
        }
        finally
        {
            _connection.Close();
        }
        return Ok(tmp);
    }
}

[Route("api/modifyCaseStatus")]
[ApiController]
public class ModifyController : ControllerBase
{
    private readonly OracleConnection _connection;
    public ModifyController(OracleConnection connection)
    {
        _connection = connection;
    }

    [HttpPut]
    public ActionResult<IEnumerable<string>> StatusModify(ModifyRequestData requestData)
    {
        string input = requestData.inputCase;
        string sql = "update cases " +
            "set status='调查' " +
            "where case_ID='" + input + "'";
        try
        {
            _connection.Open();
            OracleCommand command = new OracleCommand(sql, _connection);
            command.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"查询数据时发生错误: {ex.Message}");
        }
        finally
        {
            _connection.Close();
        }
        return Ok("");
    }
}

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
        List <Dictionary<string, string>> cases = new List<Dictionary<string, string>>();

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

                // Console.WriteLine($"查询数据SQL为:{command.CommandText}");
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
                            { "登记时间", registerTime.ToString() },
                            { "案发地点", address },
                            { "案件等级", ranking }
                        };

                        cases.Add(result);
                    }

                    _connection.Close();
                    return Ok(cases);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"查询数据时发生错误:{ex.Message}");
            return StatusCode(499, $"查询数据时发生错误: {ex.Message}");
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
        List<Dictionary<string, string>> men = new List<Dictionary<string, string>>();

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

                // Console.WriteLine($"查询数据SQL为:{command.CommandText}");
                using (OracleDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string policeNum = reader.GetString(reader.GetOrdinal("POLICE_NUMBER"));
                        string policeName = reader.GetString(reader.GetOrdinal("POLICE_NAME"));
                        string gender = reader.GetString(reader.GetOrdinal("GENDER"));
                        string nation = reader.GetString(reader.GetOrdinal("NATION"));
                        string status = reader.GetString(reader.GetOrdinal("STATUS"));
                        string position = reader.GetString(reader.GetOrdinal("POSITION"));

                        if (gender == "F")
                            gender = "女";
                        else if (gender == "M")
                            gender = "男";

                        Dictionary<string, string> result = new Dictionary<string, string>
                        {
                            { "警员编号", policeNum },
                            { "姓名", policeName },
                            { "性别", status },
                            { "民族", gender },
                            { "状态", status },
                            { "职位", position }
                        };

                        men.Add(result);
                    }

                    _connection.Close();
                    return Ok(men);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"查询数据时发生错误:{ex.Message}");
            return StatusCode(499, $"查询数据时发生错误: {ex.Message}");
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
        string sql = "update cases " +
            "set status='调查' " +
            "where case_ID ='" + caseID + "'";
        
        try
        {
            _connection.Open();
            OracleCommand command = new OracleCommand(sql, _connection);
            command.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"查询数据时发生错误: {ex.Message}");
        }
        finally
        {
            _connection.Close();
        }
        return Ok("");
    }
}