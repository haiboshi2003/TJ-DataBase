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
public class CitizenInfoControllerZYH : ControllerBase
{
    private OracleConnection _connection;

    public CitizenInfoControllerZYH(OracleConnection connection)
    {
        _connection = connection;
    }

    [HttpGet("api/getCitizenInfo")]
    public IActionResult HandleEndpoint([FromQuery] string? citizenID, [FromQuery] string? citizenName, [FromQuery] string? citizenSex, [FromQuery] string? citizenAddress)
    {
        var citizens = new List<object>();
        try
        {
            _connection.Open();
            // 执行 SQL 查询
            using (var command = _connection.CreateCommand())
            {
                command.Connection = _connection;
                command.CommandText = "SELECT * FROM CITIZEN WHERE 1 = 1";
                StringBuilder whereClause = new StringBuilder();

                if (!string.IsNullOrEmpty(citizenID))
                {
                    whereClause.Append(" AND ID_NUM LIKE '%' || :citizenID || '%'");
                    command.Parameters.Add(":citizenID", OracleDbType.Varchar2).Value = citizenID;
                }
                if (!string.IsNullOrEmpty(citizenName))
                {
                    whereClause.Append(" AND CITIZEN_NAME LIKE '%' || :citizenName || '%'");
                    command.Parameters.Add(":citizenName", OracleDbType.Varchar2).Value = citizenName;
                }
                if (!string.IsNullOrEmpty(citizenSex))
                {
                    whereClause.Append(" AND GENDER = :citizenSex");
                    command.Parameters.Add(":citizenSex", OracleDbType.Varchar2).Value = citizenSex;
                }
                if (!string.IsNullOrEmpty(citizenAddress))
                {
                    whereClause.Append(" AND ADDRESS LIKE '%' || :address || '%'");
                    command.Parameters.Add(":address", OracleDbType.Varchar2).Value = citizenAddress;
                }

                if (whereClause.Length > 0)
                {
                    command.CommandText += whereClause.ToString();
                }

                using (OracleDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        var citizen = new
                        {
                            id = reader.GetString(reader.GetOrdinal("ID_NUM")),
                            name = reader.GetString(reader.GetOrdinal("CITIZEN_Name")),
                            sex = reader.GetString(reader.GetOrdinal("GENDER")),
                            address = reader.GetString(reader.GetOrdinal("ADDRESS")),
                            motherid = reader.IsDBNull(reader.GetOrdinal("MOTHER_ID")) ? "" : reader.GetString(reader.GetOrdinal("MOTHER_ID")),
                            fatherid = reader.IsDBNull(reader.GetOrdinal("FATHER_ID")) ? "" : reader.GetString(reader.GetOrdinal("FATHER_ID")),
                        };
                        citizens.Add(citizen);
                    }

                    _connection.Close();
                    return Ok(citizens);
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

    [HttpPost("api/addCitizen")]
    public IActionResult AddHandleEndpoint([FromQuery] string? citizenID, [FromQuery] string? citizenName, [FromQuery] string? citizenSex, [FromQuery] string? citizenAddress, [FromQuery] string? motherID, [FromQuery] string? fatherID)
    {
        try
        {
            _connection.Open();

            if (string.IsNullOrEmpty(citizenID) || string.IsNullOrEmpty(citizenName) || string.IsNullOrEmpty(citizenSex) || string.IsNullOrEmpty(citizenAddress))
                return Ok("新增公民失败！信息不全！");

            if (citizenID.Length < 18)
                return Ok("身份证号过短！请完善编号！");

            if (!string.IsNullOrEmpty(fatherID) && fatherID.Length < 18)
                return Ok("父亲身份证号过短！请完善编号！");

            if (!string.IsNullOrEmpty(motherID) && motherID.Length < 18)
                return Ok("母亲身份证号过短！请完善编号！");

            string pattern = @"^.*\p{Lo}+市.*$";
            if (!Regex.IsMatch(citizenAddress, pattern))
            {
                return Ok("地址不符规范！（XX市XXX）");
            }

            // 查询公民编号是否存在
            using (var command1 = _connection.CreateCommand())
            {
                command1.Connection = _connection;
                command1.CommandText = "SELECT * FROM CITIZEN WHERE ID_NUM = :citizenID";
                command1.Parameters.Add(":citizenID", OracleDbType.Varchar2).Value = citizenID;
                using (OracleDataReader reader = command1.ExecuteReader())
                    if (reader.HasRows)
                        return Ok("该身份证号码已存在！请重新输入！");
            }
            // 查询母亲公民编号是否存在
            if (!string.IsNullOrEmpty(motherID))
                using (var command1 = _connection.CreateCommand())
                {
                    command1.Connection = _connection;
                    command1.CommandText = "SELECT * FROM CITIZEN WHERE ID_NUM = :citizenID";
                    command1.Parameters.Add(":citizenID", OracleDbType.Varchar2).Value = motherID;
                    using (OracleDataReader reader = command1.ExecuteReader())
                        if (!reader.HasRows)
                            return Ok("母亲身份证号码不存在！请重新输入！");
                }

            // 查询父亲公民编号是否存在
            if (!string.IsNullOrEmpty(fatherID))
                using (var command1 = _connection.CreateCommand())
                {
                    command1.Connection = _connection;
                    command1.CommandText = "SELECT * FROM CITIZEN WHERE ID_NUM = :citizenID";
                    command1.Parameters.Add(":citizenID", OracleDbType.Varchar2).Value = fatherID;
                    using (OracleDataReader reader = command1.ExecuteReader())
                        if (!reader.HasRows)
                            return Ok("父亲身份证号码不存在！请重新输入！");
                }


            if (!string.IsNullOrEmpty(fatherID) || !string.IsNullOrEmpty(fatherID))
                if (citizenID == motherID || citizenID == fatherID || motherID == fatherID)
                    return Ok("不同人的身份证号码不能相等！");

            string insertQuery = "INSERT INTO CITIZEN (ID_NUM, CITIZEN_NAME, GENDER, FATHER_ID, MOTHER_ID, ADDRESS) " +
                        "VALUES (:citizenID, :citizenName, :citizenSex, :fatherID, :motherID, :citizenAddress)";

            using (OracleCommand command = new OracleCommand(insertQuery, _connection))
            {
                command.Connection = _connection;
                command.Parameters.Add(":citizenID", OracleDbType.Varchar2).Value = citizenID;
                command.Parameters.Add(":citizenName", OracleDbType.Varchar2).Value = citizenName;
                command.Parameters.Add(":citizenSex", OracleDbType.Varchar2).Value = citizenSex;
                command.Parameters.Add(":fatherID", OracleDbType.Varchar2).Value = fatherID;
                command.Parameters.Add(":motherID", OracleDbType.Varchar2).Value = motherID;
                command.Parameters.Add(":citizenAddress", OracleDbType.Varchar2).Value = citizenAddress;

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


    [HttpDelete("api/delCitizen")]
    public IActionResult DeleteHandleEndpoint([FromQuery] string? citizenID)
    {
        try
        {
            _connection.Open();

            // 检查公民ID是否规范
            if (string.IsNullOrEmpty(citizenID) || citizenID.Length < 18 || !Regex.IsMatch(citizenID, @"^\d+$"))
                return Ok("公民编号无效！请重新输入！");

            // 查询该公民ID对应的行是否存在
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM CITIZEN WHERE ID_NUM = :citizenID";
                command.Parameters.Add(":citizenID", OracleDbType.Varchar2).Value = citizenID;

                using (OracleDataReader reader = command.ExecuteReader())
                {
                    if (!reader.HasRows)
                        return Ok("未找到该身份证号的公民！");
                }
            }

            // 执行删除相关记录的操作
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM RELATED WHERE ID_NUM = :citizenID";
                command.Parameters.Add(":citizenID", OracleDbType.Varchar2).Value = citizenID;
                command.ExecuteNonQuery();
            }

            // 执行删除公民操作
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM CITIZEN WHERE ID_NUM = :citizenID";
                command.Parameters.Add(":citizenID", OracleDbType.Varchar2).Value = citizenID;
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

    [HttpPut("api/updCitizen")]
    public IActionResult UpdateHandleEndpoint([FromQuery] string? citizenID, [FromQuery] string? citizenName, [FromQuery] string? citizenSex, [FromQuery] string? citizenAddress, [FromQuery] string? motherID, [FromQuery] string? fatherID)
    {
        try
        {
            _connection.Open();

            if (string.IsNullOrEmpty(citizenID) || string.IsNullOrEmpty(citizenName) || string.IsNullOrEmpty(citizenSex) || string.IsNullOrEmpty(citizenAddress))
                return Ok("新增公民失败！信息不全！");

            if (citizenID.Length < 18)
                return Ok("身份证号过短！请完善编号！");

            if (!string.IsNullOrEmpty(fatherID) && fatherID.Length < 18)
                return Ok("父亲身份证号过短！请完善编号！");

            if (!string.IsNullOrEmpty(motherID) && motherID.Length < 18)
                return Ok("母亲身份证号过短！请完善编号！");

            string pattern = @"^.*\p{Lo}+市.*$";
            if (!Regex.IsMatch(citizenAddress, pattern))
            {
                return Ok("地址不符规范！（XX市XXX）");
            }

            // 查询公民编号是否存在
            using (var command1 = _connection.CreateCommand())
            {
                command1.Connection = _connection;
                command1.CommandText = "SELECT * FROM CITIZEN WHERE ID_NUM = :citizenID";
                command1.Parameters.Add(":citizenID", OracleDbType.Varchar2).Value = citizenID;
                using (OracleDataReader reader = command1.ExecuteReader())
                    if (!reader.HasRows)
                        return Ok("该身份证号码不存在！请重新输入！");
            }
            // 查询母亲公民编号是否存在
            if (!string.IsNullOrEmpty(motherID))
                using (var command1 = _connection.CreateCommand())
                {
                    command1.Connection = _connection;
                    command1.CommandText = "SELECT * FROM CITIZEN WHERE ID_NUM = :citizenID";
                    command1.Parameters.Add(":citizenID", OracleDbType.Varchar2).Value = motherID;
                    using (OracleDataReader reader = command1.ExecuteReader())
                        if (!reader.HasRows)
                            return Ok("母亲身份证号码不存在！请重新输入！");
                }

            // 查询父亲公民编号是否存在
            if (!string.IsNullOrEmpty(fatherID))
                using (var command1 = _connection.CreateCommand())
                {
                    command1.Connection = _connection;
                    command1.CommandText = "SELECT * FROM CITIZEN WHERE ID_NUM = :citizenID";
                    command1.Parameters.Add(":citizenID", OracleDbType.Varchar2).Value = fatherID;
                    using (OracleDataReader reader = command1.ExecuteReader())
                        if (!reader.HasRows)
                            return Ok("父亲身份证号码不存在！请重新输入！");
                }


            if (!string.IsNullOrEmpty(fatherID) || !string.IsNullOrEmpty(fatherID))
                if (citizenID == motherID || citizenID == fatherID || motherID == fatherID)
                    return Ok("不同人的身份证号码不能相等！");

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "UPDATE CITIZEN SET ID_NUM = :citizenID, CITIZEN_NAME = :citizenName, GENDER = :citizenSex, ADDRESS = :citizenAddress, FATHER_ID = :fatherID, MOTHER_ID = :motherID WHERE ID_NUM = :citizenID";
                command.Parameters.Add(":citizenID", OracleDbType.Varchar2).Value = citizenID;
                command.Parameters.Add(":citizenName", OracleDbType.Varchar2).Value = citizenName;
                command.Parameters.Add(":citizenSex", OracleDbType.Varchar2).Value = citizenSex;
                command.Parameters.Add(":citizenAddress", OracleDbType.Varchar2).Value = citizenAddress;
                command.Parameters.Add(":motherID", OracleDbType.Varchar2).Value = motherID;
                command.Parameters.Add(":fatherID", OracleDbType.Varchar2).Value = fatherID;
                command.ExecuteNonQuery();

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
