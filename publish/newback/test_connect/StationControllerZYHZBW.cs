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
            Console.WriteLine($"stationInfo查询数据时发生错误:{ex.Message}");
            return StatusCode(499, "查询数据时发生错误！");
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
                return Ok("信息不全！新增警局失败！");

            if (!Regex.IsMatch(stationID, @"^\d+$"))
                return Ok("无效的警局编号！新增警局失败！");

            if (stationID.Length < 9)
                return Ok("警局编号过短！新增警局失败！");

            string pattern = @"^.*市.*分局$";
            if (!Regex.IsMatch(stationName, pattern))
            {
                return Ok("警局名称不符规范（XX市XXX分局）!新增警局失败！");
            }

            pattern = @"^.*\p{Lo}+市.*$";
            if (!Regex.IsMatch(stationAddress, pattern))
            {
                return Ok("地址不符规范（XX市XXX）!新增警局失败！");
            }

            // 查询警局编号是否存在
            using (var command1 = _connection.CreateCommand())
            {
                command1.Connection = _connection;
                command1.CommandText = "SELECT * FROM POLICE_STATION WHERE STATION_ID = :stationID";
                command1.Parameters.Add(":stationID", OracleDbType.Varchar2).Value = stationID;
                using (OracleDataReader reader = command1.ExecuteReader())
                    if (reader.HasRows)
                        return Ok("该编号已存在！新增警局失败！");
            }

            // 查询警局名字是否存在
            using (var command1 = _connection.CreateCommand())
            {
                command1.Connection = _connection;
                command1.CommandText = "SELECT * FROM POLICE_STATION WHERE STATION_NAME = :stationName";
                command1.Parameters.Add(":stationName", OracleDbType.Varchar2).Value = stationName;
                using (OracleDataReader reader = command1.ExecuteReader())
                    if (reader.HasRows)
                        return Ok("该名称已存在！新增警局失败！");
            }


            string insertQuery = "INSERT INTO POLICE_STATION (STATION_ID, STATION_NAME, CITY, ADDRESS, BUDGET) " +
                        "VALUES (:stationID, :stationName, :stationCity, :stationAddress, :stationBudget)";

            string stationCity;
            pattern = @"^((\p{Lo}+市)|([^\p{Lo}]*(\p{Lo}+)市))";
            Match match = Regex.Match(stationAddress, pattern);
            if (match.Success)
            {
                stationCity = match.Groups[2].Value != "" ? match.Groups[2].Value : match.Groups[4].Value;
            }
            else
            {
                return Ok("地址不符规范（XX市XXX）!新增警局失败！");
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
            Console.WriteLine($"addStation插入数据时发生错误:{ex.Message}");
            return StatusCode(499, "插入数据时发生错误！");
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
                return Ok("警局编号无效！删除警局失败！");

            // 查询该警局ID对应的行是否存在
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM POLICE_STATION WHERE STATION_ID = :stationID";
                command.Parameters.Add(":stationID", OracleDbType.Varchar2).Value = stationID;

                using (OracleDataReader reader = command.ExecuteReader())
                {
                    if (!reader.HasRows)
                        return Ok("未找到该编号的警局！删除警局失败！");
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
                // 删除EQUIPMENT_USE表中与指定STATION_ID相关联的行
                command.CommandText = "DELETE FROM EQUIPMENT_USE WHERE EQUIPMENT_ID IN (SELECT EQUIPMENT_ID FROM POLICE_EQUIPMENT WHERE STATION_ID = :stationID)";
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
            Console.WriteLine($"delStation删除数据时发生错误: {ex.Message}");
            return StatusCode(499, "删除数据时发生错误！");
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
                return Ok("信息不全！修改警局信息失败！");

            if (!Regex.IsMatch(stationID, @"^\d+$"))
                return Ok("无效的警局编号！修改警局信息失败！");

            if (stationID.Length < 9)
                return Ok("警局编号过短！修改警局信息失败！");

            string pattern = @"^.*市.*分局$";
            if (!Regex.IsMatch(stationName, pattern))
            {
                return Ok("警局名称不符规范（XX市XXX分局）！修改警局信息失败！");
            }

            pattern = @"^.*\p{Lo}+市.*$";
            if (!Regex.IsMatch(stationAddress, pattern))
            {
                return Ok("地址不符规范（XX市XXX）！修改警局信息失败！");
            }

            // 查询警局编号是否存在
            using (var command1 = _connection.CreateCommand())
            {
                command1.Connection = _connection;
                command1.CommandText = "SELECT * FROM POLICE_STATION WHERE STATION_ID = :stationID";
                command1.Parameters.Add(":stationID", OracleDbType.Varchar2).Value = stationID;
                using (OracleDataReader reader = command1.ExecuteReader())
                    if (!reader.HasRows)
                        return Ok("没有该编号的警局！修改警局信息失败！");
            }

            string stationCity;
            pattern = @"^((\p{Lo}+市)|([^\p{Lo}]*(\p{Lo}+)市))";
            Match match = Regex.Match(stationAddress, pattern);
            if (match.Success)
            {
                stationCity = match.Groups[2].Value != "" ? match.Groups[2].Value : match.Groups[4].Value;
            }
            else
            {
                return Ok("地址不符规范（XX市XXX）！修改警局信息失败！");
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
            Console.WriteLine($"updStation更新数据时发生错误:{ex.Message}");
            return StatusCode(499, "插入数据时发生错误！");
        }
        finally
        {
            _connection.Close();
        }
    }

}
