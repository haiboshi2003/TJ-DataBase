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
public class PolicemenInfoControllerZYH : ControllerBase
{
    private OracleConnection _connection;

    public PolicemenInfoControllerZYH(OracleConnection connection)
    {
        _connection = connection;
    }

    [HttpGet("api/policemenInfo")]
    public IActionResult HandleEndpoint([FromQuery] string? policemenID, [FromQuery] string? policemenName, [FromQuery] string? policemenIDNum, [FromQuery] string? policemenYear, [FromQuery] string? policemenSex, [FromQuery] string? policemenNation, [FromQuery] string? policemenStatus, [FromQuery] string? policemenPosition)
    {
        var policemens = new List<object>();
        try
        {
            _connection.Open();
            // 执行 SQL 查询
            using (var command = _connection.CreateCommand())
            {
                command.Connection = _connection;
                command.CommandText = "SELECT * FROM POLICEMEN WHERE 1 = 1";
                StringBuilder whereClause = new StringBuilder();

                if (!string.IsNullOrEmpty(policemenID))
                {
                    whereClause.Append(" AND POLICE_NUMBER LIKE '%' || :policemenID || '%'");
                    command.Parameters.Add(":policemenID", OracleDbType.Varchar2).Value = policemenID;
                }
                if (!string.IsNullOrEmpty(policemenName))
                {
                    whereClause.Append(" AND POLICE_NAME LIKE '%' || :policemenName || '%'");
                    command.Parameters.Add(":policemenName", OracleDbType.Varchar2).Value = policemenName;
                }
                if (!string.IsNullOrEmpty(policemenIDNum))
                {
                    whereClause.Append(" AND ID_NUMBER LIKE '%' || :policemenIDNum || '%'");
                    command.Parameters.Add(":policemenIDNum", OracleDbType.Varchar2).Value = policemenIDNum;
                }
                if (!string.IsNullOrEmpty(policemenYear))
                {
                    whereClause.Append(" AND BIRTHDAY LIKE '%' || :policemenYear || '%'");
                    command.Parameters.Add(":policemenYear", OracleDbType.Varchar2).Value = policemenYear;
                }
                if (!string.IsNullOrEmpty(policemenSex))
                {
                    whereClause.Append(" AND GENDER = :policemenSex");
                    command.Parameters.Add(":policemenSex", OracleDbType.Varchar2).Value = policemenSex;
                }
                if (!string.IsNullOrEmpty(policemenNation))
                {
                    whereClause.Append(" AND NATION '%' || :policemenNation || '%'");
                    command.Parameters.Add(":policemenNation", OracleDbType.Varchar2).Value = policemenNation;
                }
                if (!string.IsNullOrEmpty(policemenStatus))
                {
                    whereClause.Append(" AND STATUS = :policemenStatus");
                    command.Parameters.Add(":policemenStatus", OracleDbType.Varchar2).Value = policemenStatus;
                }
                if (!string.IsNullOrEmpty(policemenPosition))
                {
                    whereClause.Append(" AND POSITION = :policemenPosition");
                    command.Parameters.Add(":policemenPosition", OracleDbType.Varchar2).Value = policemenPosition;
                }

                if (whereClause.Length > 0)
                {
                    command.CommandText += whereClause.ToString();
                }

                using (OracleDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        var policemen = new
                        {
                            pid = reader.GetString(reader.GetOrdinal("POLICE_NUMBER")),
                            name = reader.GetString(reader.GetOrdinal("POLICE_NAME")),
                            idn = reader.GetString(reader.GetOrdinal("ID_NUMBER")),
                            birthday = reader.GetString(reader.GetOrdinal("BIRTHDAY")),
                            gender = reader.GetString(reader.GetOrdinal("GENDER")),
                            nation = reader.GetString(reader.GetOrdinal("NATION")),
                            phone = reader.GetString(reader.GetOrdinal("PHONE_NUMBER")),
                            email = reader.GetString(reader.GetOrdinal("EMAIL")),
                            status = reader.GetString(reader.GetOrdinal("STATUS")),
                            position = reader.GetString(reader.GetOrdinal("POSITION")),
                        };
                        policemens.Add(policemen);
                    }

                    _connection.Close();
                    return Ok(policemens);
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

    [HttpDelete("api/delPolicemen")]
    public IActionResult DeleteHandleEndpoint([FromQuery] string? policemenID, [FromQuery] string? myPolicemenID)
    {
        try
        {
            _connection.Open();

            // 检查警员ID是否规范
            if (string.IsNullOrEmpty(policemenID) || policemenID.Length < 7 || !Regex.IsMatch(policemenID, @"^\d+$"))
                return Ok("警员编号无效！请重新输入！");

            if (string.IsNullOrEmpty(myPolicemenID))
                return Ok("获取用户警号失败！");

            if (policemenID == myPolicemenID)
                return Ok("不能删除自己的信息！");

            // 查询该警员ID对应的行是否存在
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM POLICEMEN WHERE POLICE_NUMBER = :policemenID";
                command.Parameters.Add(":policemenID", OracleDbType.Varchar2).Value = policemenID;

                using (OracleDataReader reader = command.ExecuteReader())
                {
                    if (!reader.HasRows)
                        return Ok("未找到该编号的警员！");
                }
            }

            // 执行删除相关记录的操作
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM CALLS WHERE ANSWER_NUMBER = :policemenID";
                command.Parameters.Add(":policemenID", OracleDbType.Varchar2).Value = policemenID;
                command.ExecuteNonQuery();
            }
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM PAYROLL_STUB WHERE POLICE_NUMBER = :policemenID";
                command.Parameters.Add(":policemenID", OracleDbType.Varchar2).Value = policemenID;
                command.ExecuteNonQuery();
            }
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM RESPONSE WHERE POLICE_NUMBER = :policemenID";
                command.Parameters.Add(":policemenID", OracleDbType.Varchar2).Value = policemenID;
                command.ExecuteNonQuery();
            }
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM EMPLOY WHERE POLICE_NUMBER = :policemenID";
                command.Parameters.Add(":policemenID", OracleDbType.Varchar2).Value = policemenID;
                command.ExecuteNonQuery();
            }
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM VIDEOS WHERE PRINCIPLE_ID = :policemenID";
                command.Parameters.Add(":policemenID", OracleDbType.Varchar2).Value = policemenID;
                command.ExecuteNonQuery();
            }
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM EQUIPMENT_USE WHERE POLICE_NUMBER = :policemenID";
                command.Parameters.Add(":policemenID", OracleDbType.Varchar2).Value = policemenID;
                command.ExecuteNonQuery();
            }
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM POLICE_ACCOUNT WHERE POLICE_NUMBER = :policemenID";
                command.Parameters.Add(":policemenID", OracleDbType.Varchar2).Value = policemenID;
                command.ExecuteNonQuery();
            }
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM PATROL WHERE POLICE_NUMBER = :policemenID";
                command.Parameters.Add(":policemenID", OracleDbType.Varchar2).Value = policemenID;
                command.ExecuteNonQuery();
            }

            // 执行删除警员操作
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM POLICEMEN WHERE POLICE_NUMBER = :policemenID";
                command.Parameters.Add(":policemenID", OracleDbType.Varchar2).Value = policemenID;
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

    [HttpPut("api/depart")]
    public IActionResult DepartHandleEndpoint([FromQuery] string? policemenID, [FromQuery] string? myPolicemenID)
    {
        try
        {
            _connection.Open();

            if (string.IsNullOrEmpty(policemenID) || policemenID.Length < 7 || !Regex.IsMatch(policemenID, @"^\d+$"))
                return Ok("警员编号无效！请重新输入！");


            if (string.IsNullOrEmpty(myPolicemenID))
                return Ok("获取用户警号失败！");

            if (policemenID == myPolicemenID)
                return Ok("不能修改自己的信息！");

            // 查询警员编号是否存在
            using (var command1 = _connection.CreateCommand())
            {
                command1.Connection = _connection;
                command1.CommandText = "SELECT * FROM POLICEMEN WHERE POLICE_NUMBER = :policemenID";
                command1.Parameters.Add(":policemenID", OracleDbType.Varchar2).Value = policemenID;
                using (OracleDataReader reader = command1.ExecuteReader())
                    if (!reader.HasRows)
                        return Ok("该警员编号不存在！请重新输入！");
            }

            using (var command1 = _connection.CreateCommand())
            {
                command1.Connection = _connection;
                command1.CommandText = "SELECT * FROM POLICEMEN WHERE POLICE_NUMBER = :policemenID AND STATUS = '离职'";
                command1.Parameters.Add(":policemenID", OracleDbType.Varchar2).Value = policemenID;
                using (OracleDataReader reader = command1.ExecuteReader())
                    if (reader.HasRows)
                        return Ok("该警员已离职！");
            }

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "UPDATE POLICEMEN SET STATUS = '离职' WHERE POLICE_NUMBER = :policemenID";
                command.Parameters.Add(":policemenID", OracleDbType.Varchar2).Value = policemenID;
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

    [HttpPut("api/updPolicemen")]
    public IActionResult UpdateHandleEndpoint([FromQuery] string? policemenID, [FromQuery] string? policemenName, [FromQuery] string? policemenIDNum, [FromQuery] string? policemenYear, [FromQuery] string? policemenMonth, [FromQuery] string? policemenDay, [FromQuery] string? policemenSex, [FromQuery] string? policemenNation, [FromQuery] string? myPolicemenID, [FromQuery] string? policemenPhone, [FromQuery] string? policemenEmail)
    {
        try
        {
            _connection.Open();

            if (string.IsNullOrEmpty(policemenID) || string.IsNullOrEmpty(policemenName) || string.IsNullOrEmpty(policemenSex) || string.IsNullOrEmpty(policemenYear) || string.IsNullOrEmpty(policemenMonth) || string.IsNullOrEmpty(policemenDay) || string.IsNullOrEmpty(policemenNation) || string.IsNullOrEmpty(policemenIDNum) || string.IsNullOrEmpty(policemenPhone) || string.IsNullOrEmpty(policemenEmail))
                return Ok("修改警员信息失败！信息不全！");
            if (policemenID.Length < 7)
                return Ok("警员编号过短！请完善编号！");

            if (policemenIDNum.Length < 18)
                return Ok("身份证号码过短！请完善编号！");

            if (policemenPhone.Length < 11)
                return Ok("无效的电话号码！");

            if (string.IsNullOrEmpty(myPolicemenID))
                return Ok("获取用户警号失败！");

            if (policemenID == myPolicemenID)
                return Ok("不能修改自己的信息！");

            if (!policemenNation.EndsWith("族"))
                return Ok("民族输入不规范（应为X族）");

            string pattern = @"^[\w\.-]+@[\w\.-]+\.\w+$";
            Regex regex = new Regex(pattern);
            if (!regex.IsMatch(policemenEmail))
               return Ok("无效的邮箱！"); 

            // 查询警员编号是否存在
            using (var command1 = _connection.CreateCommand())
            {
                command1.Connection = _connection;
                command1.CommandText = "SELECT * FROM POLICEMEN WHERE POLICE_NUMBER = :policemenID";
                command1.Parameters.Add(":policemenID", OracleDbType.Varchar2).Value = policemenID;
                using (OracleDataReader reader = command1.ExecuteReader())
                    if (!reader.HasRows)
                        return Ok("该警员编号不存在！请重新输入！");
            }
            using (var command1 = _connection.CreateCommand())
            {
                command1.Connection = _connection;
                command1.CommandText = "SELECT * FROM POLICEMEN WHERE ID_NUMBER = :policemenIDNum AND POLICE_NUMBER <> :policemenID";
                command1.Parameters.Add(":policemenIDNum", OracleDbType.Varchar2).Value = policemenIDNum;
                command1.Parameters.Add(":policemenID", OracleDbType.Varchar2).Value = policemenID;
                using (OracleDataReader reader = command1.ExecuteReader())
                    if (reader.HasRows)
                        return Ok("其他人也登记有该身份证号码！"); 
            }
            string datee = policemenYear + "-" + policemenMonth + "-" + policemenDay;

            using (var command = _connection.CreateCommand())
            { 
                command.CommandText = "UPDATE POLICEMEN SET POLICE_NAME = :policemenName, ID_NUMBER = :policemenIDNum, BIRTHDAY = :datee, GENDER = :policemenSex, NATION = :policemenNation, PHONE_NUMBER = :policemenPhone, EMAIL = :policemenEmail WHERE POLICE_NUMBER = :policemenID";
                command.Parameters.Add(":policemenName", OracleDbType.Varchar2).Value = policemenName; command.Parameters.Add(":policemenIDNum", OracleDbType.Varchar2).Value = policemenIDNum;
                command.Parameters.Add(":datee", OracleDbType.Varchar2).Value = datee;
                command.Parameters.Add(":policemenSex", OracleDbType.Varchar2).Value = policemenSex;
                command.Parameters.Add(":policemenNation", OracleDbType.Varchar2).Value = policemenNation;
                command.Parameters.Add(":policemenPhone", OracleDbType.Char).Value = policemenPhone;
                command.Parameters.Add(":policemenEmail", OracleDbType.Varchar2).Value = policemenEmail;
                command.Parameters.Add(":policemenID", OracleDbType.Varchar2).Value = policemenID;
                command.ExecuteNonQuery();
                // 插入成功，返回 JSON 数据
                var response = new { data = "成功" };
                return Ok(JsonSerializer.Serialize(response));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"更新数据时发生错误:{ex.Message}");
            return StatusCode(499, $"插入数据时发生错误: {ex.Message}");
        }
        finally
        {
            _connection.Close();
        }
    }
}
