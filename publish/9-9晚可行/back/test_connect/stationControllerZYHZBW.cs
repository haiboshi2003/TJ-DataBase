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
public class StationInfoControllerZYH : ControllerBase
{
    private OracleConnection _connection;

    public StationInfoControllerZYH(OracleConnection connection)
    {
        _connection = connection;
    }

    [HttpGet("api/stationInfo")]
    public IActionResult HandleEndpoint([FromQuery] string? stationID, [FromQuery] string? stationName, [FromQuery] string? stationAddress)
    {
        List<StationInfoZYH> stations = new List<StationInfoZYH>();
        try
        {
            _connection.Open();
            // 执行 SQL 查询
            using (var command = _connection.CreateCommand())
            {
                command.Connection = _connection;
                command.CommandText = "SELECT * FROM POLICE_STATION WHERE 1 = 1";
                StringBuilder whereClause = new StringBuilder();

                if (!string.IsNullOrEmpty(stationID))
                {
                    whereClause.Append(" AND STATION_ID = :stationID");
                    command.Parameters.Add(":stationID", OracleDbType.Varchar2).Value = stationID;
                }
                if (!string.IsNullOrEmpty(stationName))
                {
                    whereClause.Append(" AND STATION_Name LIKE '%' || :stationName || '%'");
                    command.Parameters.Add(":stationName", OracleDbType.Varchar2).Value = stationName;
                }
                if (!string.IsNullOrEmpty(stationAddress))
                {
                    whereClause.Append(" AND ADDRESS LIKE '%' || :address || '%'");
                    command.Parameters.Add(":address", OracleDbType.Varchar2).Value = stationAddress;
                }

                if (whereClause.Length > 0)
                {
                    command.CommandText += whereClause.ToString();
                }

                Console.WriteLine($"查询数据SQL为:{command.CommandText}");
                using (OracleDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        StationInfoZYH station = new StationInfoZYH
                        {
                            stationID = reader.GetString(reader.GetOrdinal("station_ID")),
                            stationName = reader.GetString(reader.GetOrdinal("station_Name")),
                            city = reader.GetString(reader.GetOrdinal("city")),
                            address = reader.GetString(reader.GetOrdinal("address")),
                            budget = reader.GetInt32(reader.GetOrdinal("budget"))
                        };
                        stations.Add(station);
                    }

                    _connection.Close();
                    return Ok(stations);
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

    [HttpPost("api/addStation")]
    public IActionResult AddHandleEndpoint([FromQuery] string? stationID, [FromQuery] string? stationName, [FromQuery] string? stationAddress, [FromQuery] int? stationBudget)
    {
        try
        {
            _connection.Open();

            if (string.IsNullOrEmpty(stationID) || string.IsNullOrEmpty(stationName) || string.IsNullOrEmpty(stationAddress))
                return Ok("新增警局失败！信息不全！");

            if (!Regex.IsMatch(stationID, @"^\d+$"))
                return Ok("无效的警局编号！");

            if (stationID.Length < 9)
                return Ok("警局编号过短！请完善编号！");

            string pattern = @"^.*市.*分局$";
            if (!Regex.IsMatch(stationName, pattern))
            {
                return Ok("警局名称不符规范！（XX市XXX分局）");
            }

            pattern = @"^.*\p{Lo}+市.*$";
            if (!Regex.IsMatch(stationAddress, pattern))
            {
                return Ok("地址不符规范！（XX市XXX）");
            }

            // 查询警局编号是否存在
            using (var command1 = _connection.CreateCommand())
            {
                command1.Connection = _connection;
                command1.CommandText = "SELECT * FROM POLICE_STATION WHERE STATION_ID = :stationID";
                command1.Parameters.Add(":stationID", OracleDbType.Varchar2).Value = stationID;
                using (OracleDataReader reader = command1.ExecuteReader())
                    if (reader.HasRows)
                        return Ok("该编号已存在！请重新输入！");
            }

            // 查询警局名字是否存在
            using (var command1 = _connection.CreateCommand())
            {
                command1.Connection = _connection;
                command1.CommandText = "SELECT * FROM POLICE_STATION WHERE STATION_NAME = :stationName";
                command1.Parameters.Add(":stationName", OracleDbType.Varchar2).Value = stationName;
                using (OracleDataReader reader = command1.ExecuteReader())
                    if (reader.HasRows)
                        return Ok("该名称已存在！请重新输入！");
            }


            string insertQuery = "INSERT INTO POLICE_STATION (STATION_ID, STATION_NAME, CITY, ADDRESS, BUDGET) " +
                        "VALUES (:stationID, :stationName, :stationCity, :stationAddress, :stationBudget)";

            string stationCity;
            pattern = @"^((\p{Lo}+市)|([^\p{Lo}]*(\p{Lo}+)市))";
            Match match = Regex.Match(stationAddress, pattern);
            if (match.Success)
            {
                stationCity = match.Groups[2].Value != "" ? match.Groups[2].Value : match.Groups[4].Value;
                Console.WriteLine(stationCity); // 输出：水市市
            }
            else
            {
                return Ok("地址不符规范！（XX市XXX）");
            }

            using (OracleCommand command = new OracleCommand(insertQuery, _connection))
            {
                command.Connection = _connection;
                command.Parameters.Add(":stationID", OracleDbType.Varchar2).Value = stationID;
                command.Parameters.Add(":stationName", OracleDbType.Varchar2).Value = stationName;
                command.Parameters.Add(":stationCity", OracleDbType.Varchar2).Value = stationCity;
                command.Parameters.Add(":stationAddress", OracleDbType.Varchar2).Value = stationAddress;
                command.Parameters.Add(":stationBudget", OracleDbType.Varchar2).Value = stationBudget;

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


    [HttpDelete("api/delStation")]
    public IActionResult DeleteHandleEndpoint([FromQuery] string? stationID)
    {
        try
        {
            _connection.Open();

            // 检查警局ID是否规范
            if (string.IsNullOrEmpty(stationID) || stationID.Length < 9 || !Regex.IsMatch(stationID, @"^\d+$"))
                return Ok("警局编号无效！请重新输入！");

            // 查询该警局ID对应的行是否存在
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM POLICE_STATION WHERE STATION_ID = :stationID";
                command.Parameters.Add(":stationID", OracleDbType.Varchar2).Value = stationID;

                using (OracleDataReader reader = command.ExecuteReader())
                {
                    if (!reader.HasRows)
                        return Ok("未找到该编号的警局！");
                }
            }

            // 执行删除相关记录的操作
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM EMPLOY WHERE STATION_ID = :stationID";
                command.Parameters.Add(":stationID", OracleDbType.Varchar2).Value = stationID;
                command.ExecuteNonQuery();
            }
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM PAYROLL_STUB WHERE STATION_ID = :stationID";
                command.Parameters.Add(":stationID", OracleDbType.Varchar2).Value = stationID;
                command.ExecuteNonQuery();
            }
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM VEHICLE_USE WHERE STATION_ID = :stationID";
                command.Parameters.Add(":stationID", OracleDbType.Varchar2).Value = stationID;
                command.ExecuteNonQuery();
            }
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM POLICE_EQUIPMENT WHERE STATION_ID = :stationID";
                command.Parameters.Add(":stationID", OracleDbType.Varchar2).Value = stationID;
                command.ExecuteNonQuery();
            }

            // 执行删除警局操作
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM POLICE_STATION WHERE STATION_ID = :stationID";
                command.Parameters.Add(":stationID", OracleDbType.Varchar2).Value = stationID;
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

    [HttpPut("api/updStation")]
    public IActionResult UpdateHandleEndpoint([FromQuery] string? stationID, [FromQuery] string? stationName, [FromQuery] string? stationAddress, [FromQuery] int? stationBudget)
    {
        try
        {
            _connection.Open();

            if (string.IsNullOrEmpty(stationID) || string.IsNullOrEmpty(stationName) || string.IsNullOrEmpty(stationAddress))
                return Ok("修改警局信息失败！信息不全！");

            if (!Regex.IsMatch(stationID, @"^\d+$"))
                return Ok("无效的警局编号！");

            if (stationID.Length < 9)
                return Ok("警局编号过短！请完善编号！");

            string pattern = @"^.*市.*分局$";
            if (!Regex.IsMatch(stationName, pattern))
            {
                return Ok("警局名称不符规范！（XX市XXX分局）");
            }

            pattern = @"^.*\p{Lo}+市.*$";
            if (!Regex.IsMatch(stationAddress, pattern))
            {
                return Ok("地址不符规范！（XX市XXX）");
            }

            // 查询警局编号是否存在
            using (var command1 = _connection.CreateCommand())
            {
                command1.Connection = _connection;
                command1.CommandText = "SELECT * FROM POLICE_STATION WHERE STATION_ID = :stationID";
                command1.Parameters.Add(":stationID", OracleDbType.Varchar2).Value = stationID;
                using (OracleDataReader reader = command1.ExecuteReader())
                    if (!reader.HasRows)
                        return Ok("没有该编号的警局！请重新输入！");
            }

            string stationCity;
            pattern = @"^((\p{Lo}+市)|([^\p{Lo}]*(\p{Lo}+)市))";
            Match match = Regex.Match(stationAddress, pattern);
            if (match.Success)
            {
                stationCity = match.Groups[2].Value != "" ? match.Groups[2].Value : match.Groups[4].Value;
                Console.WriteLine(stationCity); // 输出：水市市
            }
            else
            {
                return Ok("地址不符规范！（XX市XXX）");
            }

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "UPDATE POLICE_STATION SET STATION_NAME = :stationName, CITY = :stationCity, ADDRESS = :stationAddress, BUDGET = :stationBudget WHERE STATION_ID = :stationID";
                command.Parameters.Add(":stationName", OracleDbType.Varchar2).Value = stationName;
                command.Parameters.Add(":stationCity", OracleDbType.Varchar2).Value = stationCity;
                command.Parameters.Add(":stationAddress", OracleDbType.Varchar2).Value = stationAddress;
                command.Parameters.Add(":stationBudget", OracleDbType.Varchar2).Value = stationBudget;
                command.Parameters.Add(":stationID", OracleDbType.Varchar2).Value = stationID;
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
