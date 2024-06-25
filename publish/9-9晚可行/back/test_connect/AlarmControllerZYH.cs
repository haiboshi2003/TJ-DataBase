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
public class alarmInfoControllerZYH : ControllerBase
{
    private OracleConnection _connection;

    public alarmInfoControllerZYH(OracleConnection connection)
    {
        _connection = connection;
    }

    [HttpGet("api/alarmInfo")]
    public IActionResult HandleEndpoint([FromQuery] string? alarmID, [FromQuery] DateTimeOffset alarmTime, [FromQuery] string isT, [FromQuery] string? alarmNum, [FromQuery] string? caseID, [FromQuery] string? caseType, [FromQuery] string? policemenID)
    {
        var alarms = new List<object>();
        try
        {
            _connection.Open();
            // 执行 SQL 查询
            using (var command = _connection.CreateCommand())
            {
                command.Connection = _connection;
                command.CommandText = "SELECT * FROM CALLS WHERE 1 = 1";
                StringBuilder whereClause = new StringBuilder();


                if (!string.IsNullOrEmpty(alarmID))
                {
                    whereClause.Append(" AND AUDIO_ID LIKE '%' || :alarmID || '%'");
                    command.Parameters.Add(":alarmID", OracleDbType.Varchar2).Value = alarmID;
                }
                if (isT == "T")
                {
                    DateTime aTime = alarmTime.LocalDateTime;
                    whereClause.Append(" AND TRUNC(CALLS_TIME) = TRUNC(:aTime)");
                    command.Parameters.Add(":aTime", OracleDbType.Varchar2).Value = aTime;
                }
                if (!string.IsNullOrEmpty(alarmNum))
                {
                    whereClause.Append(" AND PHONE_NUMBER LIKE '%' || :alarmNum || '%'");
                    command.Parameters.Add(":alarmNum", OracleDbType.Varchar2).Value = alarmNum;
                }
                if (!string.IsNullOrEmpty(caseType))
                {
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
                }
                if (!string.IsNullOrEmpty(caseID))
                {
                    whereClause.Append(" AND CASE_ID LIKE '%' || :caseID || '%'");
                    command.Parameters.Add(":caseID", OracleDbType.Varchar2).Value = caseID;
                }
                if (!string.IsNullOrEmpty(policemenID))
                {
                    whereClause.Append(" AND ANSWER_NUMBER LIKE '%' || :policemenID || '%'");
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
                        var alarm = new
                        {
                            alarmID = reader.GetString(reader.GetOrdinal("AUDIO_ID")),
                            alarmTime = reader.GetDateTime(reader.GetOrdinal("CALLS_TIME")),
                            alarmNum = reader.GetString(reader.GetOrdinal("PHONE_NUMBER")),
                            caseID = reader.IsDBNull(reader.GetOrdinal("CASE_ID")) ? null : reader.GetString(reader.GetOrdinal("CASE_ID")),
                            policemenID = reader.GetString(reader.GetOrdinal("ANSWER_NUMBER")),
                        };
                        alarms.Add(alarm);
                    }

                    _connection.Close();
                    return Ok(alarms);
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

    [HttpPost("api/addAlarm")]
    public IActionResult AddHandleEndpoint([FromQuery] string? alarmID, [FromQuery] string? alarmNum, [FromQuery] string? policemenID)
    {
        try
        {
            _connection.Open();
            if (string.IsNullOrEmpty(alarmID) || string.IsNullOrEmpty(alarmNum) || string.IsNullOrEmpty(policemenID))
                return Ok("新增接警记录失败！信息不全！");

            if (!Regex.IsMatch(alarmID, @"^\d+$"))
                return Ok("无效的接警编号！");

            if (alarmID.Length < 7)
                return Ok("接警编号过短！请完善编号！");

            if (policemenID.Length < 7)
                return Ok("警员编号过短！请完善编号！");

            if (alarmNum.Length < 11)
                return Ok("无效的电话号码！");

            // 查询接警编号是否存在
            using (var command1 = _connection.CreateCommand())
            {
                command1.Connection = _connection;
                command1.CommandText = "SELECT * FROM CALLS WHERE AUDIO_ID = :alarmID";
                command1.Parameters.Add(":alarmID", OracleDbType.Varchar2).Value = alarmID;
                using (OracleDataReader reader = command1.ExecuteReader())
                    if (reader.HasRows)
                        return Ok("该接警编号已存在！请重新输入！");
            }
            // 查询警员编号是否存在
            using (var command1 = _connection.CreateCommand())
            {
                command1.Connection = _connection;
                command1.CommandText = "SELECT * FROM POLICEMEN WHERE POLICE_NUMBER = :ID";
                command1.Parameters.Add(":ID", OracleDbType.Varchar2).Value = policemenID;
                using (OracleDataReader reader = command1.ExecuteReader())
                    if (!reader.HasRows)
                        return Ok("该警员不存在！请重新输入！");
            }

            string insertQuery = "INSERT INTO CALLS (AUDIO_ID, CALLS_TIME, PHONE_NUMBER, ANSWER_NUMBER) " +
                        "VALUES (:alarmID, :alarmTime, :alarmNum, :policemenID)";

            DateTime rTime = DateTime.Now;
            using (OracleCommand command = new OracleCommand(insertQuery, _connection))
            {
                command.Connection = _connection;
                command.Parameters.Add(":alarmID", OracleDbType.Varchar2).Value = alarmID;
                command.Parameters.Add(":alarmTime", OracleDbType.Date).Value = rTime;
                command.Parameters.Add(":alarmNum", OracleDbType.Varchar2).Value = alarmNum;
                command.Parameters.Add(":policemenID", OracleDbType.Varchar2).Value = policemenID;

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

    [HttpDelete("api/delAlarm")]
    public IActionResult DeleteHandleEndpoint([FromQuery] string? alarmID)
    {
        try
        {
            _connection.Open();

            // 检查接警ID是否规范
            if (string.IsNullOrEmpty(alarmID) || alarmID.Length < 7 || !Regex.IsMatch(alarmID, @"^\d+$"))
                return Ok("接警编号无效！请重新输入！");

            // 查询该接警ID对应的行是否存在
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM CALLS WHERE AUDIO_ID = :alarmID";
                command.Parameters.Add(":alarmID", OracleDbType.Varchar2).Value = alarmID;
                using (OracleDataReader reader = command.ExecuteReader())
                {
                    if (!reader.HasRows)
                        return Ok("未找到该编号的接警记录！");
                }
            }

            // 执行删除接警操作
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM CALLS WHERE AUDIO_ID = :alarmID";
                command.Parameters.Add(":alarmID", OracleDbType.Varchar2).Value = alarmID;
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

    [HttpPut("api/relAlarm")]
    public IActionResult PutHandleEndpoint([FromQuery] string? alarmID, [FromQuery] string? caseID, [FromQuery] string? caseType)
    {
        try
        {
            _connection.Open();

            if (string.IsNullOrEmpty(alarmID) || string.IsNullOrEmpty(caseType) || string.IsNullOrEmpty(caseID))
                return Ok("信息不全！");

            // 检查接警ID是否规范
            if (alarmID.Length < 7 || !Regex.IsMatch(alarmID, @"^\d+$"))
                return Ok("接警编号无效！请重新输入！");

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

            // 查询该接警ID对应的行是否存在
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM CALLS WHERE AUDIO_ID = :alarmID";
                command.Parameters.Add(":alarmID", OracleDbType.Varchar2).Value = alarmID;
                using (OracleDataReader reader = command.ExecuteReader())
                {
                    if (!reader.HasRows)
                        return Ok("未找到该编号的接警记录！请重新输入！");
                }
            }
            // 查询案件是否存在
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM CASES WHERE CASE_ID = :ID";
                command.Parameters.Add(":ID", OracleDbType.Varchar2).Value = caseID;
                using (OracleDataReader reader = command.ExecuteReader())
                {
                    if (!reader.HasRows)
                        return Ok("未找到该编号的案件！关联失败！");
                }
            }

            // 执行更新接警操作
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "UPDATE CALLS SET CASE_ID = :ID WHERE AUDIO_ID = :alarmID";

                command.Parameters.Add(":ID", OracleDbType.Varchar2).Value = caseID;
                command.Parameters.Add(":alarmID", OracleDbType.Varchar2).Value = alarmID;
                command.ExecuteNonQuery();
            }

            // 返回成功的JSON响应
            var response = new { data = "成功" };
            return Ok(JsonSerializer.Serialize(response));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"修改数据时发生错误: {ex.Message}");
            return StatusCode(499, $"修改数据时发生错误: {ex.Message}");
        }
        finally
        {
            _connection.Close();
        }
    }

    [HttpPut("api/updAlarm")]
    public IActionResult PutHandleEndpoint([FromQuery] string? alarmID, [FromQuery] DateTimeOffset alarmTime, [FromQuery] string? alarmNum, [FromQuery] string? policemenID)
    {
        try
        {
            _connection.Open();

            if (string.IsNullOrEmpty(alarmID) || string.IsNullOrEmpty(alarmNum) || string.IsNullOrEmpty(policemenID))
                return Ok("接警信息不全！");

            // 检查接警ID是否规范
            if (alarmID.Length < 7 || !Regex.IsMatch(alarmID, @"^\d+$"))
                return Ok("接警编号无效！请重新输入！");

            if (policemenID.Length < 7 || !Regex.IsMatch(policemenID, @"^\d+$"))
                return Ok("警员编号无效！请重新输入！");

            // 查询该接警ID对应的行是否存在
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM CALLS WHERE AUDIO_ID = :alarmID";
                command.Parameters.Add(":alarmID", OracleDbType.Varchar2).Value = alarmID;
                using (OracleDataReader reader = command.ExecuteReader())
                {
                    if (!reader.HasRows)
                        return Ok("未找到该编号的接警！请重新输入！");
                }
            }

            // 查询警员是否存在
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM POLICEMEN WHERE POLICE_NUMBER = :policemenID";
                command.Parameters.Add(":policemenID", OracleDbType.Varchar2).Value = policemenID;
                using (OracleDataReader reader = command.ExecuteReader())
                {
                    if (!reader.HasRows)
                        return Ok("未找到该编号的警员！请重新输入！");
                }
            }

            // 执行更新接警操作
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "UPDATE CALLS SET CALLS_TIME = :aTime, PHONE_NUMBER = :alarmNum, " +
                    "ANSWER_NUMBER = :policemenID WHERE AUDIO_ID = :alarmID";

                DateTime aTime = alarmTime.LocalDateTime;
                command.Parameters.Add(":aTime", OracleDbType.Date).Value = aTime;
                command.Parameters.Add(":alarmNum", OracleDbType.Varchar2).Value = alarmNum;
                command.Parameters.Add(":policemenID", OracleDbType.Varchar2).Value = policemenID;
                command.Parameters.Add(":alarmID", OracleDbType.Varchar2).Value = alarmID;
                command.ExecuteNonQuery();
            }

            // 返回成功的JSON响应
            var response = new { data = "成功" };
            return Ok(JsonSerializer.Serialize(response));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"修改数据时发生错误: {ex.Message}");
            return StatusCode(499, $"修改数据时发生错误: {ex.Message}");
        }
        finally
        {
            _connection.Close();
        }
    }
}