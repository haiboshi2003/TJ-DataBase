using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data.Common;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

[ApiController]
[Route("/")]
public class AttendInfoControllerZYH : ControllerBase
{
    private OracleConnection _connection;

    public AttendInfoControllerZYH(OracleConnection connection)
    {
        _connection = connection;
    }

    [HttpGet("api/attendInfo")]
    public IActionResult HandleEndpoint([FromQuery] string? attendID, [FromQuery] string? attendAddress, [FromQuery] DateTimeOffset attendTime, [FromQuery] string? isT)
    {
        var attends = new List<object>();
        try
        {
            _connection.Open();
            // 执行 SQL 查询
            using (var command = _connection.CreateCommand())
            {
                command.Connection = _connection;
                command.CommandText = "SELECT * FROM PATROL WHERE 1 = 1";
                StringBuilder whereClause = new StringBuilder();

                if (!string.IsNullOrEmpty(attendID))
                {
                    whereClause.Append(" AND POLICE_NUMBER LIKE '%' || :attendID || '%'");
                    command.Parameters.Add(":attendID", OracleDbType.Varchar2).Value = attendID;
                }
                if (!string.IsNullOrEmpty(attendAddress))
                {
                    whereClause.Append(" AND AREA LIKE '%' || :attendAddress || '%'");
                    command.Parameters.Add(":attendAddress", OracleDbType.Varchar2).Value = attendAddress;
                }
                Console.WriteLine(isT);
                if (isT == "T")
                {
                    DateTime localDateTime = attendTime.LocalDateTime;
                    whereClause.Append(" AND TRUNC(PATROL_TIME) = TRUNC(:attendTime)");
                    command.Parameters.Add(":attendTime", OracleDbType.Date).Value = localDateTime;
                }

                if (whereClause.Length > 0)
                {
                    command.CommandText += whereClause.ToString();
                }


                using (OracleDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        var attend = new
                        {
                            id = reader.GetString(reader.GetOrdinal("POLICE_NUMBER")),
                            time = reader.GetDateTime(reader.GetOrdinal("PATROL_TIME")),
                            area = reader.GetString(reader.GetOrdinal("AREA")),
                        };
                        attends.Add(attend);
                    }

                    _connection.Close();
                    return Ok(attends);
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

    [HttpPost("api/addAttend")]
    public IActionResult AddHandleEndpoint([FromQuery] string? attendID, [FromQuery] string? attendAddress, [FromQuery] DateTimeOffset attendTime)
    {
        try
        {
            _connection.Open();

            if (string.IsNullOrEmpty(attendID) || string.IsNullOrEmpty(attendAddress))
                return Ok("新增出勤记录失败！信息不全！");

            if (!Regex.IsMatch(attendID, @"^\d+$"))
                return Ok("无效的出勤编号！");

            if (attendID.Length < 7)
                return Ok("警员编号过短！请完善编号！");

            string pattern = @"^.*\p{Lo}+市.*$";
            if (!Regex.IsMatch(attendAddress, pattern))
            {
                return Ok("地址不符规范！（XX市XXX）");
            }

            // 查询出勤编号是否存在
            using (var command1 = _connection.CreateCommand())
            {
                command1.Connection = _connection;
                command1.CommandText = "SELECT * FROM POLICEMEN WHERE POLICE_NUMBER = :attendID";
                command1.Parameters.Add(":attendID", OracleDbType.Varchar2).Value = attendID;
                using (OracleDataReader reader = command1.ExecuteReader())
                    if (!reader.HasRows)
                        return Ok("该警员不存在！请重新输入！");
            }

            DateTime localDateTime = attendTime.LocalDateTime;
            using (var command1 = _connection.CreateCommand())
            {
                command1.Connection = _connection;
                command1.CommandText = "SELECT * FROM PATROL WHERE PATROL_TIME = :time AND AREA = :address";
                command1.Parameters.Add(":time", OracleDbType.Date).Value = localDateTime;
                command1.Parameters.Add(":address", OracleDbType.Varchar2).Value = attendAddress;

                using (OracleDataReader reader = command1.ExecuteReader())
                    if (reader.HasRows)
                        return Ok("重复上传该记录！");
            }

            string insertQuery = "INSERT INTO PATROL (PATROL_TIME, AREA, POLICE_NUMBER) " +
                        "VALUES (:time, :attendAddress, :attendID)";

            using (OracleCommand command = new OracleCommand(insertQuery, _connection))
            {
                command.Connection = _connection;
                command.Parameters.Add(":time", OracleDbType.Date).Value = localDateTime;
                command.Parameters.Add(":attendAddress", OracleDbType.Varchar2).Value = attendAddress;
                command.Parameters.Add(":attendID", OracleDbType.Varchar2).Value = attendID;

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

    [HttpDelete("api/delAttend")]
    public IActionResult DelHandleEndpoint([FromQuery] string? attendID, [FromQuery] string? attendAddress, [FromQuery] DateTimeOffset attendTime)
    {
        try
        {
            _connection.Open();

            if (string.IsNullOrEmpty(attendID) || string.IsNullOrEmpty(attendAddress))
                return Ok("删除出勤记录失败！信息不全！");

            if (!Regex.IsMatch(attendID, @"^\d+$"))
                return Ok("无效的警员编号！");

            if (attendID.Length < 7)
                return Ok("警员编号过短！请完善编号！");

            string pattern = @"^.*\p{Lo}+市.*$";
            if (!Regex.IsMatch(attendAddress, pattern))
            {
                return Ok("地址不符规范！（XX市XXX）");
            }

            // 查询出勤编号是否存在
            using (var command1 = _connection.CreateCommand())
            {
                command1.Connection = _connection;
                command1.CommandText = "SELECT * FROM POLICEMEN WHERE POLICE_NUMBER = :attendID";
                command1.Parameters.Add(":attendID", OracleDbType.Varchar2).Value = attendID;
                using (OracleDataReader reader = command1.ExecuteReader())
                    if (!reader.HasRows)
                        return Ok("该警员不存在！请重新输入！");
            }

            DateTime localDateTime = attendTime.LocalDateTime;
            using (var command1 = _connection.CreateCommand())
            {
                command1.Connection = _connection;
                command1.CommandText = "SELECT * FROM PATROL WHERE PATROL_TIME = :time AND AREA = :address AND POLICE_NUMBER = :attendID";
                command1.Parameters.Add(":time", OracleDbType.Date).Value = localDateTime;
                command1.Parameters.Add(":address", OracleDbType.Varchar2).Value = attendAddress;
                command1.Parameters.Add(":attendID", OracleDbType.Varchar2).Value = attendID;

                using (OracleDataReader reader = command1.ExecuteReader())
                    if (!reader.HasRows)
                        return Ok("没有该记录！删除失败！");
            }

            string insertQuery = "DELETE FROM PATROL WHERE PATROL_TIME = :time AND AREA = :address AND POLICE_NUMBER = :attendID";
            using (OracleCommand command = new OracleCommand(insertQuery, _connection))
            {
                command.Connection = _connection;
                command.Parameters.Add(":time", OracleDbType.Date).Value = localDateTime;
                command.Parameters.Add(":address", OracleDbType.Varchar2).Value = attendAddress;
                command.Parameters.Add(":attendID", OracleDbType.Varchar2).Value = attendID;

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
