using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using web.Helpers;
using web.DTO_group2;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;


[ApiController]
[Authorize] // 添加Authorize特性，表示需要进行身份验证
public class searchCivilianController : ControllerBase
{

    private readonly OracleConnection _connection;

    public searchCivilianController(OracleConnection connection)
    {
        _connection = connection;
    }


    [HttpPost("searchCivilian/ByName")]
    public IActionResult SearchByName(searchByName nameInfo)
    {
        var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

        // 检查用户权限，如果权限代码为"3"或以上，则允许查询
        if (userRole != null && int.TryParse(userRole, out int roleCode) && roleCode >= 3)
        {

            // 根据姓名查询公民信息
            // 可以根据传入的 searchByName 对象的属性来执行查询操作
            try
            {
                string query = "SELECT ID_NUM, CITIZEN_NAME, GENDER FROM CITIZEN WHERE ";
                bool addCondition = false;

                if (!string.IsNullOrWhiteSpace(nameInfo.name))
                {
                    query += $"CITIZEN_NAME = '{nameInfo.name}'";
                    addCondition = true;
                }


                if (nameInfo.gender == "male" || nameInfo.gender == "female")
                {
                    if (addCondition)
                    {
                        query += " AND ";
                        if (nameInfo.gender == "male")
                            query += "GENDER = 'M'";
                        else
                            query += "GENDER = 'F'";
                    }
                }
                

                // 连接数据库并执行查询
                
                _connection.Open();
                OracleCommand command = new OracleCommand(query, _connection);
                List<string> results = new List<string>();

                using (OracleDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        results.Add($"idCard:{reader["ID_NUM"]},name:{reader["CITIZEN_NAME"]},gender:{reader["GENDER"]}");
                    }
                }
                _connection.Close();
                // 处理结果
                if (results.Count == 0)
                    return Ok("");
                else
                    return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"查询数据时发生错误: {ex.Message}");
            }
        }
        else
        {
            return Unauthorized(); // 权限不足
        }
    }

    [HttpPost("searchCivilian/ByID")]
    public IActionResult SearchByID(searchByID idInfo)
    {
        var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

        // 检查用户权限，如果权限代码为"3"或以上，则允许查询
        if (userRole != null && int.TryParse(userRole, out int roleCode) && roleCode >= 3)
        {
            try
            {
                string query = "SELECT ID_NUM, CITIZEN_NAME, GENDER FROM CITIZEN WHERE ";
               

                if (!string.IsNullOrWhiteSpace(idInfo.ID))
                {
                    query += $"ID_NUM = '{idInfo.ID}'";
                   
                }

                // 连接数据库并执行查询

                _connection.Open();
                OracleCommand command = new OracleCommand(query, _connection);
                List<string> results = new List<string>();

                using (OracleDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        results.Add($"{reader["ID_NUM"]} {reader["CITIZEN_NAME"]} {reader["GENDER"]}");
                    }
                }
                _connection.Close();
                // 处理结果
                if (results.Count == 0)
                    return Ok("");
                else
                    return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"查询数据时发生错误: {ex.Message}");
            }
        }
        else
        {
            return Unauthorized(); // 权限不足
        }
    }
}

