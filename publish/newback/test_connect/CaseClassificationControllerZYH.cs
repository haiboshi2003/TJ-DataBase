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
public class caseInfoControllerZYH : ControllerBase
{
    private OracleConnection _connection;

    public caseInfoControllerZYH(OracleConnection connection)
    {
        _connection = connection;
    }

    [HttpGet("api/caseInfo")]
    public IActionResult HandleEndpoint([FromQuery] string? caseID, [FromQuery] string? caseType, [FromQuery] string? caseStatus, [FromQuery] string? caseAddress, [FromQuery] string? caseRanking)
    {
        List<caseInfoZYH> cases = new List<caseInfoZYH>();
        try
        {
            _connection.Open();
            // 执行 SQL 查询
            using (var command = _connection.CreateCommand())
            {
                command.Connection = _connection;
                command.CommandText = "SELECT * FROM CASES WHERE 1 = 1";
                StringBuilder whereClause = new StringBuilder();

                if (!string.IsNullOrEmpty(caseID))
                {
                    whereClause.Append(" AND CASE_ID  LIKE '%' ||  :caseID || '%'");
                    command.Parameters.Add(":caseID", OracleDbType.Varchar2).Value = caseID;
                }
                if (!string.IsNullOrEmpty(caseType))
                {
                    whereClause.Append(" AND CASE_TYPE = :caseType");
                    command.Parameters.Add(":caseType", OracleDbType.Varchar2).Value = caseType;
                }
                if (!string.IsNullOrEmpty(caseStatus))
                {
                    whereClause.Append(" AND STATUS = :caseStatus");
                    command.Parameters.Add(":caseStatus", OracleDbType.Varchar2).Value = caseStatus;
                }
                if (!string.IsNullOrEmpty(caseAddress))
                {
                    whereClause.Append(" AND ADDRESS LIKE '%' || :address || '%'");
                    command.Parameters.Add(":address", OracleDbType.Varchar2).Value = caseAddress;
                }
                if (!string.IsNullOrEmpty(caseRanking))
                {
                    whereClause.Append(" AND RANKING = :caseRanking");
                    command.Parameters.Add(":caseRanking", OracleDbType.Varchar2).Value = caseRanking;
                }

                if (whereClause.Length > 0)
                {
                    command.CommandText += whereClause.ToString();
                }

                using (OracleDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        caseInfoZYH acase = new caseInfoZYH
                        {
                            caseID = reader.GetString(reader.GetOrdinal("CASE_ID")),
                            caseType = reader.GetString(reader.GetOrdinal("CASE_TYPE")),
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

    [HttpGet("api/caseDetail")]
    public IActionResult DetHandleEndpoint([FromQuery] string? caseID)
    {
        List<object> men = new List<object>();
        try
        {
            _connection.Open();
            // 执行 SQL 查询
            using (var command = _connection.CreateCommand())
            {
                command.Connection = _connection;
                command.CommandText = @"SELECT CITIZEN.CITIZEN_NAME, CITIZEN.ID_NUM, 
                                    CASE 
                                        WHEN CITIZEN.GENDER = 'F' THEN '女' 
                                        WHEN CITIZEN.GENDER = 'M' THEN '男' 
                                        ELSE '未知' 
                                    END AS GENDER,
                                    RELATED.RELATED_TYPE 
                                    FROM CASES
                                    INNER JOIN RELATED ON CASES.CASE_ID = RELATED.CASE_ID
                                    INNER JOIN CITIZEN ON RELATED.ID_NUM = CITIZEN.ID_NUM
                                    WHERE CASES.CASE_ID = :caseID";
                command.Parameters.Add(":caseID", OracleDbType.Varchar2).Value = caseID;

                using (OracleDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var man = new
                        {
                            name = reader.GetString(reader.GetOrdinal("CITIZEN_NAME")),
                            ID = reader.GetString(reader.GetOrdinal("ID_NUM")),
                            gender = reader.GetString(reader.GetOrdinal("GENDER")),
                            caseT = reader.GetString(reader.GetOrdinal("RELATED_TYPE"))
                        };
                        men.Add(man);
                    }
                }

                _connection.Close();
                return Ok(men);
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

    [HttpPut("api/closeCase")]
    public IActionResult UpdateHandleEndpoint([FromQuery] string? caseID, [FromQuery] string? caseType)
    {
        try
        {
            _connection.Open();

            if (string.IsNullOrEmpty(caseID) || string.IsNullOrEmpty(caseType))
                return Ok("修改案件信息失败！信息不全！");

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


            // 查询案件编号是否存在
            using (var command1 = _connection.CreateCommand())
            {
                command1.Connection = _connection;
                command1.CommandText = "SELECT * FROM CASES WHERE CASE_ID = :caseID";
                command1.Parameters.Add(":caseID", OracleDbType.Varchar2).Value = caseID;
                using (OracleDataReader reader = command1.ExecuteReader())
                    if (!reader.HasRows)
                        return Ok("没有该编号的案件！请重新输入！");
            }

            using (var command1 = _connection.CreateCommand())
            {
                command1.Connection = _connection;
                command1.CommandText = "SELECT * FROM CASES WHERE CASE_ID = :caseID AND STATUS = '立案'";
                command1.Parameters.Add(":caseID", OracleDbType.Varchar2).Value = caseID;
                using (OracleDataReader reader = command1.ExecuteReader())
                    if (reader.HasRows)
                        return Ok("该案件还未开始调查！");
            }

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "UPDATE CASES SET STATUS = '结案' WHERE CASE_ID = :caseID";
                command.Parameters.Add(":caseID", OracleDbType.Varchar2).Value = caseID;
                command.ExecuteNonQuery();

                // 插入成功，返回 JSON 数据
                var response = new { data = "成功" };
                return Ok(JsonSerializer.Serialize(response));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"更新数据时发生错误:{ex.Message}");
            return StatusCode(499, $"更新数据时发生错误: {ex.Message}");
        }
        finally
        {
            _connection.Close();
        }
    }

    [HttpPost("api/addCase")]
    public IActionResult AddHandleEndpoint([FromQuery] string? caseID, [FromQuery] string? caseType, [FromQuery] string? caseAddress, [FromQuery] string? caseRanking)
    {
        try
        {
            _connection.Open();

            if (string.IsNullOrEmpty(caseID) || string.IsNullOrEmpty(caseType) || string.IsNullOrEmpty(caseAddress) || string.IsNullOrEmpty(caseRanking))
                return Ok("新增案件失败！信息不全！");

            if (!Regex.IsMatch(caseID, @"^\d+$"))
                return Ok("无效的案件编号！");

            if (caseID.Length < 7)
                return Ok("案件编号过短！请完善编号！");

            string pattern = @"^.*\p{Lo}+市.*$";
            if (!Regex.IsMatch(caseAddress, pattern))
            {
                return Ok("地址不符规范！（XX市XXX）");
            }

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

            // 查询案件编号是否存在
            using (var command1 = _connection.CreateCommand())
            {
                command1.Connection = _connection;
                command1.CommandText = "SELECT * FROM CASES WHERE CASE_ID = :caseID";
                command1.Parameters.Add(":caseID", OracleDbType.Varchar2).Value = caseID;
                using (OracleDataReader reader = command1.ExecuteReader())
                    if (reader.HasRows)
                        return Ok("该编号已存在！请重新输入！");
            }

            string insertQuery = "INSERT INTO CASES (CASE_ID, CASE_TYPE, STATUS, REGISTER_TIME, ADDRESS, RANKING) " +
                        "VALUES (:caseID, :caseType, :caseStatus, :registerTime, :caseAddress, :caseRanking)";
            DateTime registerTime = DateTime.Now;
            using (OracleCommand command = new OracleCommand(insertQuery, _connection))
            {
                command.Connection = _connection;
                command.Parameters.Add(":caseID", OracleDbType.Varchar2).Value = caseID;
                command.Parameters.Add(":caseType", OracleDbType.Varchar2).Value = caseType;
                command.Parameters.Add(":caseStatus", OracleDbType.Varchar2).Value = "立案";
                command.Parameters.Add(":registerTime", OracleDbType.Date).Value = registerTime;
                command.Parameters.Add(":caseAddress", OracleDbType.Varchar2).Value = caseAddress;
                command.Parameters.Add(":caseRanking", OracleDbType.Varchar2).Value = caseRanking;

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

    [HttpDelete("api/delCase")]
    public IActionResult DeleteHandleEndpoint([FromQuery] string? caseID, [FromQuery] string? caseType)
    {
        try
        {
            _connection.Open();

            if (string.IsNullOrEmpty(caseID) || string.IsNullOrEmpty(caseType))
                return Ok("案件信息不全！");
            // 检查案件ID是否规范
            if (caseID.Length < 7 || !Regex.IsMatch(caseID, @"^\d+$"))
                return Ok("案件编号无效！请重新输入！");


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

            // 查询该案件ID对应的行是否存在
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM CASES WHERE CASE_ID = :caseID";
                command.Parameters.Add(":caseID", OracleDbType.Varchar2).Value = caseID;
                using (OracleDataReader reader = command.ExecuteReader())
                {
                    if (!reader.HasRows)
                        return Ok("未找到该编号的案件！");
                }
            }

            // 执行删除相关记录的操作
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM CALLS WHERE CASE_ID = :caseID";
                command.Parameters.Add(":caseID", OracleDbType.Varchar2).Value = caseID;
                command.ExecuteNonQuery();
            }
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM CASE_VIDEO WHERE CASE_ID = :caseID";
                command.Parameters.Add(":caseID", OracleDbType.Varchar2).Value = caseID;
                command.ExecuteNonQuery();
            }
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM RELATED WHERE CASE_ID = :caseID";
                command.Parameters.Add(":caseID", OracleDbType.Varchar2).Value = caseID;
                command.ExecuteNonQuery();
            }
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM RESPONSE WHERE CASE_ID = :caseID";
                command.Parameters.Add(":caseID", OracleDbType.Varchar2).Value = caseID;
                command.ExecuteNonQuery();
            }
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM INVESTIGATION WHERE CASE_ID = :caseID";
                command.Parameters.Add(":caseID", OracleDbType.Varchar2).Value = caseID;
                command.ExecuteNonQuery();
            }

            // 执行删除案件操作
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM CASES WHERE CASE_ID = :caseID";
                command.Parameters.Add(":caseID", OracleDbType.Varchar2).Value = caseID;
                command.ExecuteNonQuery();
            }

            // 返回成功的JSON响应
            var response = new { data = "成功" };
            return Ok(JsonSerializer.Serialize(response));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"删除数据时发生错误: {ex.Message}");
            return StatusCode(499, $"删除数据时发生错误: {ex.Message}");
        }
        finally
        {
            _connection.Close();
        }
    }


    [HttpPut("api/updCase")]
    public IActionResult PutHandleEndpoint([FromQuery] string? oriCaseID, [FromQuery] string? oriCaseType, [FromQuery] string? updCaseID, [FromQuery] string? updCaseType, [FromQuery] string? updCaseAddress, [FromQuery] string? updCaseRanking)
    {
        try
        {
            _connection.Open();

            if (string.IsNullOrEmpty(oriCaseID) || string.IsNullOrEmpty(oriCaseType))
                return Ok("原案件信息不全！");

            if (string.IsNullOrEmpty(updCaseID) || string.IsNullOrEmpty(updCaseType) || string.IsNullOrEmpty(updCaseAddress) || string.IsNullOrEmpty(updCaseRanking))
                return Ok("新案件信息不全！");

            // 检查案件ID是否规范
            if (oriCaseID.Length < 7 || !Regex.IsMatch(oriCaseID, @"^\d+$"))
                return Ok("原案件的编号无效！请重新输入！");
            if (updCaseID.Length < 7 || !Regex.IsMatch(updCaseID, @"^\d+$"))
                return Ok("原案件的编号无效！请重新输入！");

            if (oriCaseType == "强奸")
                oriCaseID = "A" + oriCaseID;
            else if (oriCaseType == "抢劫")
                oriCaseID = "R" + oriCaseID;
            else if (oriCaseType == "故意伤害")
                oriCaseID = "H" + oriCaseID;
            else if (oriCaseType == "盗窃")
                oriCaseID = "T" + oriCaseID;
            else if (oriCaseType == "诈骗")
                oriCaseID = "C" + oriCaseID;
            else if (oriCaseType == "谋杀")
                oriCaseID = "M" + oriCaseID;
            else
                return Ok("未知的案件类型！");

            if (updCaseType == "强奸")
                updCaseID = "A" + updCaseID;
            else if (updCaseType == "抢劫")
                updCaseID = "R" + updCaseID;
            else if (updCaseType == "故意伤害")
                updCaseID = "H" + updCaseID;
            else if (updCaseType == "盗窃")
                updCaseID = "T" + updCaseID;
            else if (updCaseType == "诈骗")
                updCaseID = "C" + updCaseID;
            else if (updCaseType == "谋杀")
                updCaseID = "M" + updCaseID;
            else
                return Ok("未知的案件类型！");


            string pattern = @"^.*\p{Lo}+市.*$";
            if (!Regex.IsMatch(updCaseAddress, pattern))
            {
                return Ok("地址不符规范！（XX市XXX）");
            }

            // 查询该案件ID对应的行是否存在
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM CASES WHERE CASE_ID = :caseID";
                command.Parameters.Add(":caseID", OracleDbType.Varchar2).Value = oriCaseID;
                using (OracleDataReader reader = command.ExecuteReader())
                {
                    if (!reader.HasRows)
                        return Ok("未找到该原编号的案件！");
                }
            }
            // 查询该案件ID对应的行是否存在
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM CASES WHERE CASE_ID = :caseID";
                command.Parameters.Add(":caseID", OracleDbType.Varchar2).Value = updCaseID;
                using (OracleDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                        return Ok("已存在新编号的案件！");
                }
            }

            if (oriCaseID == updCaseID)
            {
                using (var command = _connection.CreateCommand())
                {
                    command.CommandText = "UPDATE CASES SET ADDRESS = :address WHERE CASE_ID = :caseID";
                    command.Parameters.Add(":caseID", OracleDbType.Varchar2).Value = oriCaseID;
                    command.ExecuteNonQuery();
                }
                using (var command = _connection.CreateCommand())
                {
                    command.CommandText = "UPDATE CASES SET RANKING = :ranking WHERE CASE_ID = :caseID";
                    command.Parameters.Add(":caseID", OracleDbType.Varchar2).Value = oriCaseID;
                    command.ExecuteNonQuery();
                }

                // 返回成功的JSON响应
                var res = new { data = "成功" };
                return Ok(JsonSerializer.Serialize(res));
            }

            // 创建新记录
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO CASES (CASE_ID, CASE_TYPE, ADDRESS, RANKING, STATUS, REGISTER_TIME) VALUES (:caseID, :caseType, :caseAddress, :caseRanking, (SELECT STATUS FROM CASES WHERE CASE_ID = :oriCaseID), (SELECT REGISTER_TIME FROM CASES WHERE CASE_ID = :oriCaseID))";
                command.Parameters.Add(":caseID", OracleDbType.Varchar2).Value = updCaseID;
                command.Parameters.Add(":caseType", OracleDbType.Varchar2).Value = updCaseType;
                command.Parameters.Add(":caseAddress", OracleDbType.Varchar2).Value = updCaseAddress;
                command.Parameters.Add(":caseRanking", OracleDbType.Varchar2).Value = updCaseRanking;
                command.Parameters.Add(":oriCaseID", OracleDbType.Varchar2).Value = oriCaseID;
                command.ExecuteNonQuery();
            }

            // 执行修改相关记录的操作
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "UPDATE CALLS SET CASE_ID = :updCaseID WHERE CASE_ID = :oriCaseID";
                command.Parameters.Add(":updCaseID", OracleDbType.Varchar2).Value = updCaseID;
                command.Parameters.Add(":oriCaseID", OracleDbType.Varchar2).Value = oriCaseID;
                command.ExecuteNonQuery();
            }
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "UPDATE CASE_VIDEO SET CASE_ID = :updCaseID WHERE CASE_ID = :oriCaseID";
                command.Parameters.Add(":updCaseID", OracleDbType.Varchar2).Value = updCaseID;
                command.Parameters.Add(":oriCaseID", OracleDbType.Varchar2).Value = oriCaseID;
                command.ExecuteNonQuery();
            }
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "UPDATE RELATED SET CASE_ID = :updCaseID WHERE CASE_ID = :oriCaseID";
                command.Parameters.Add(":updCaseID", OracleDbType.Varchar2).Value = updCaseID;
                command.Parameters.Add(":oriCaseID", OracleDbType.Varchar2).Value = oriCaseID;
                command.ExecuteNonQuery();
            }
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "UPDATE RESPONSE SET CASE_ID = :updCaseID WHERE CASE_ID = :oriCaseID";
                command.Parameters.Add(":updCaseID", OracleDbType.Varchar2).Value = updCaseID;
                command.Parameters.Add(":oriCaseID", OracleDbType.Varchar2).Value = oriCaseID;
                command.ExecuteNonQuery();
            }
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "UPDATE INVESTIGATION SET CASE_ID = :updCaseID WHERE CASE_ID = :oriCaseID";
                command.Parameters.Add(":updCaseID", OracleDbType.Varchar2).Value = updCaseID;
                command.Parameters.Add(":oriCaseID", OracleDbType.Varchar2).Value = oriCaseID;
                command.ExecuteNonQuery();
            }

            // 执行删除案件操作
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM CASES WHERE CASE_ID = :caseID";
                command.Parameters.Add(":caseID", OracleDbType.Varchar2).Value = oriCaseID;
                command.ExecuteNonQuery();
            }

            // 返回成功的JSON响应
            var response = new { data = "成功" };
            return Ok(JsonSerializer.Serialize(response));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"删除数据时发生错误: {ex.Message}");
            return StatusCode(499, $"删除数据时发生错误: {ex.Message}");
        }
        finally
        {
            _connection.Close();
        }
    }
}