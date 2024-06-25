using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data.Common;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;


[ApiController]
[Route("/")]
public class prisonerInfoControllerZYH : ControllerBase
{
    private OracleConnection _connection;

    public prisonerInfoControllerZYH(OracleConnection connection)
    {
        _connection = connection;
    }

    [HttpGet("api/prisonerInfo")]
    public IActionResult HandleEndpoint([FromQuery] string? prisonerID, [FromQuery] string? prisonerName, [FromQuery] string? prisonerSex, [FromQuery] string? prisonerAddress)
    {
        List<object> men = new List<object>();
        try
        {
            _connection.Open();
            // 执行 SQL 查询
            using (var command = _connection.CreateCommand())
            {
                command.Connection = _connection;
                command.CommandText = "SELECT DISTINCT CITIZEN.ID_NUM, CITIZEN_NAME, GENDER, CITIZEN.ADDRESS FROM CITIZEN,RELATED WHERE CITIZEN.ID_NUM = RELATED.ID_NUM AND RELATED_TYPE = '犯人'";
                StringBuilder whereClause = new StringBuilder();

                if (!string.IsNullOrEmpty(prisonerID))
                {
                    whereClause.Append(" AND ID_NUM LIKE '%' ||  :prisonerID || '%'");
                    command.Parameters.Add(":prisonerID", OracleDbType.Varchar2).Value = prisonerID;
                }
                if (!string.IsNullOrEmpty(prisonerName))
                {
                    whereClause.Append(" AND CITIZEN_NAME LIKE '%' || :prisonerType || '%'");
                    command.Parameters.Add(":prisonerName", OracleDbType.Varchar2).Value = prisonerName;
                }
                if (!string.IsNullOrEmpty(prisonerSex))
                {
                    whereClause.Append(" AND GENDER = :prisonerSex");
                    command.Parameters.Add(":prisonerSex", OracleDbType.Varchar2).Value = prisonerSex;
                }
                if (!string.IsNullOrEmpty(prisonerAddress))
                {
                    whereClause.Append(" AND ADDRESS LIKE '%' || :address || '%'");
                    command.Parameters.Add(":address", OracleDbType.Varchar2).Value = prisonerAddress;
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
                            id = reader.GetString(reader.GetOrdinal("ID_NUM")),
                            name = reader.GetString(reader.GetOrdinal("CITIZEN_NAME")),
                            sex = reader.GetString(reader.GetOrdinal("GENDER")),
                            address = reader.GetString(reader.GetOrdinal("ADDRESS"))
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
            Console.WriteLine($"查询数据时发生错误:{ex.Message}");
            return StatusCode(499, $"查询数据时发生错误: {ex.Message}");
        }
        finally
        {
            _connection.Close();
        }
    }

    [HttpGet("api/getCaseP")]
    public IActionResult caseHandleEndpoint([FromQuery] string? prisonerID)
    {
        List<object> cases = new List<object>();
        try
        {
            _connection.Open();
            // 执行 SQL 查询
            using (var command = _connection.CreateCommand())
            {
                command.Connection = _connection;
                command.CommandText = "SELECT DISTINCT * FROM CASES,RELATED WHERE CASES.CASE_ID = RELATED.CASE_ID AND RELATED.ID_NUM = :prisonerID AND RELATED_TYPE = '犯人'";
                command.Parameters.Add(":prisonerID", OracleDbType.Varchar2).Value = prisonerID;

                using (OracleDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        var acase = new
                        {
                            id = reader.GetString(reader.GetOrdinal("CASE_ID")),
                            type = reader.GetString(reader.GetOrdinal("CASE_TYPE")),
                            status = reader.GetString(reader.GetOrdinal("STATUS")),
                            registerTime = reader.GetDateTime(reader.GetOrdinal("REGISTER_TIME")),
                            address = reader.GetString(reader.GetOrdinal("ADDRESS")),
                            ranking = reader.GetString(reader.GetOrdinal("RANKING"))
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
            Console.WriteLine($"查询数据时发生错误:{ex.Message}");
            return StatusCode(499, $"查询数据时发生错误: {ex.Message}");
        }
        finally
        {
            _connection.Close();
        }
    }

    [HttpPost("api/addPrisoner")]
    public IActionResult AddHandleEndpoint([FromQuery] string? prisonerID, [FromQuery] string? caseID, [FromQuery] string? caseType)
    {
        try
        {
            _connection.Open();

            if (string.IsNullOrEmpty(prisonerID) || string.IsNullOrEmpty(caseID) || string.IsNullOrEmpty(caseType))
                return Ok("新增犯人失败！信息不全！");

            if (!Regex.IsMatch(prisonerID, @"^\d+$"))
                return Ok("无效的身份证号码！");

            if (prisonerID.Length < 18)
                return Ok("犯人编号过短！请完善编号！");

            if (!Regex.IsMatch(caseID, @"^\d+$"))
                return Ok("无效的案件编号！");

            if (caseID.Length < 7)
                return Ok("案件编号过短！请完善编号！");

            if (caseType == "强奸")
                caseID = "A" + caseID;
            else if (caseType == "抢劫")
                caseID = "R" + caseID;
            else if (caseType == "故意伤害")
                caseID = "H" + caseID;
            else if (caseType == "盗窃")
                caseID = "T" + caseID;
            else if (caseType == "诈骗")
                caseID = "C" + caseID;
            else if (caseType == "谋杀")
                caseID = "M" + caseID;
            else
                return Ok("未知的案件类型！");

            // 查询犯人编号是否存在
            using (var command1 = _connection.CreateCommand())
            {
                command1.Connection = _connection;
                command1.CommandText = "SELECT * FROM CITIZEN WHERE ID_NUM = :prisonerID";
                command1.Parameters.Add(":prisonerID", OracleDbType.Varchar2).Value = prisonerID;
                using (OracleDataReader reader = command1.ExecuteReader())
                    if (!reader.HasRows)
                        return Ok("没有此身份证号码的居民！请重新输入！");
            }
            // 查询案件编号是否存在
            using (var command1 = _connection.CreateCommand())
            {
                command1.Connection = _connection;
                command1.CommandText = "SELECT * FROM CASES WHERE CASE_ID = :caseID";
                command1.Parameters.Add(":caseID", OracleDbType.Varchar2).Value = caseID;
                using (OracleDataReader reader = command1.ExecuteReader())
                    if (!reader.HasRows)
                        return Ok("没有此编号的案件！请重新输入！");
            }
            // 查询案件编号是否存在
            using (var command1 = _connection.CreateCommand())
            {
                command1.Connection = _connection;
                command1.CommandText = "SELECT * FROM RELATED WHERE CASE_ID = :caseID AND ID_NUM = :prisonerID";
                command1.Parameters.Add(":caseID", OracleDbType.Varchar2).Value = caseID;
                command1.Parameters.Add(":prisonerID", OracleDbType.Varchar2).Value = prisonerID;
                using (OracleDataReader reader = command1.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string? relatedType = reader["RELATED_TYPE"].ToString();

                        // 根据 RELATED_TYPE 做相应的操作
                        if (relatedType == "受害人")
                        {
                            return Ok("该人已登记为此案的受害人，请先删除其受害人身份！");
                        }
                        else if (relatedType == "嫌疑人") 
                        {
                            return Ok("该人已登记为此案的嫌疑人，请先删除其受害人身份！");
                        }
                        else if (relatedType == "犯人")
                        {
                            return Ok("该人已是该案件的犯人");
                        }
                    }
                }
            }


            string insertQuery = "INSERT INTO RELATED (CASE_ID, ID_NUM, RELATED_TYPE) " +
                        "VALUES (:caseID, :prisonerID, '犯人')";
            using (OracleCommand command = new OracleCommand(insertQuery, _connection))
            {
                command.Connection = _connection;
                command.Parameters.Add(":caseID", OracleDbType.Varchar2).Value = caseID;
                command.Parameters.Add(":prisonerID", OracleDbType.Varchar2).Value = prisonerID;

                command.ExecuteNonQuery();
                // 插入成功，返回 JSON 数据
                var response = new { data = "成功" };
                return Ok(JsonSerializer.Serialize(response));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"插入数据时发生错误:{ex.Message}");
            return StatusCode(499, $"插入数据时发生错误: {ex.Message}");
        }
        finally
        {
            _connection.Close();
        }
    }

    [HttpDelete("api/delPrisoner")]
    public IActionResult DeleteHandleEndpoint([FromQuery] string? prisonerID, [FromQuery] string? caseID, [FromQuery] string? caseType)
    {
        try
        {
            _connection.Open();

            if (string.IsNullOrEmpty(prisonerID) || string.IsNullOrEmpty(caseID) || string.IsNullOrEmpty(caseType))
                return Ok("删除犯人失败！信息不全！");

            if (!Regex.IsMatch(prisonerID, @"^\d+$"))
                return Ok("无效的身份证号码！");

            if (prisonerID.Length < 18)
                return Ok("犯人编号过短！请完善编号！");

            if (!Regex.IsMatch(caseID, @"^\d+$"))
                return Ok("无效的案件编号！");

            if (caseID.Length < 7)
                return Ok("案件编号过短！请完善编号！");

            if (caseType == "强奸")
                caseID = "A" + caseID;
            else if (caseType == "抢劫")
                caseID = "R" + caseID;
            else if (caseType == "故意伤害")
                caseID = "H" + caseID;
            else if (caseType == "盗窃")
                caseID = "T" + caseID;
            else if (caseType == "诈骗")
                caseID = "C" + caseID;
            else if (caseType == "谋杀")
                caseID = "M" + caseID;
            else
                return Ok("未知的案件类型！");

            // 查询犯人编号是否存在
            using (var command1 = _connection.CreateCommand())
            {
                command1.Connection = _connection;
                command1.CommandText = "SELECT * FROM CITIZEN WHERE ID_NUM = :prisonerID";
                command1.Parameters.Add(":prisonerID", OracleDbType.Varchar2).Value = prisonerID;
                using (OracleDataReader reader = command1.ExecuteReader())
                    if (!reader.HasRows)
                        return Ok("没有此身份证号码的居民！请重新输入！");
            }
            // 查询案件编号是否存在
            using (var command1 = _connection.CreateCommand())
            {
                command1.Connection = _connection;
                command1.CommandText = "SELECT * FROM CASES WHERE CASE_ID = :caseID";
                command1.Parameters.Add(":caseID", OracleDbType.Varchar2).Value = caseID;
                using (OracleDataReader reader = command1.ExecuteReader())
                    if (!reader.HasRows)
                        return Ok("没有此编号的案件！请重新输入！");
            }
            // 查询案件编号是否存在
            using (var command1 = _connection.CreateCommand())
            {
                command1.Connection = _connection;
                command1.CommandText = "SELECT * FROM RELATED WHERE CASE_ID = :caseID AND ID_NUM = :prisonerID AND RELATED_TYPE = '犯人'";
                command1.Parameters.Add(":caseID", OracleDbType.Varchar2).Value = caseID;
                command1.Parameters.Add(":prisonerID", OracleDbType.Varchar2).Value = prisonerID;
                using (OracleDataReader reader = command1.ExecuteReader())
                {
                    if (!reader.HasRows)
                        return Ok("此案件没有该犯人！");
                }
            }

            string sql = "DELETE FROM RELATED WHERE CASE_ID = :caseID AND ID_NUM = :prisonerID AND RELATED_TYPE = '犯人'";
            using (OracleCommand command = new OracleCommand(sql, _connection))
            {
                command.Connection = _connection;
                command.Parameters.Add(":caseID", OracleDbType.Varchar2).Value = caseID;
                command.Parameters.Add(":prisonerID", OracleDbType.Varchar2).Value = prisonerID;

                command.ExecuteNonQuery();
                // 插入成功，返回 JSON 数据
                var response = new { data = "成功" };
                return Ok(JsonSerializer.Serialize(response));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"插入数据时发生错误:{ex.Message}");
            return StatusCode(499, $"插入数据时发生错误: {ex.Message}");
        }
        finally
        {
            _connection.Close();
        }
    }
}