using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using web.Helpers;
using web.DTO_group2;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;


public class salarySearch
{
    public string police_number { get; set; } = "";
    public string name { get; set; } = "";
    public string year { get; set; } = "";
    public string month { get; set; } = "";
    public string station { get; set; } = "";
}

public class SalaryRecordDTO
{
    public string PAYROLL_NUMBER { get; set; } = "";
    public string POLICE_NUMBER { get; set; } = "";
    public string POLICE_NAME { get; set; } = "";
    public string STATION_ID { get; set; } = "";
    public DateTime PAY_DAY { get; set; }
    public string SALARY { get; set; } = "";
    public string SUBSIDY { get; set; } = "";
    public string DESCRIPTION { get; set; } = "";
    public string ISSUE_ID { get; set; } = "";
    public string ISSUE_NAME { get; set; } = "";
}





[ApiController]
[Authorize]
public class SalarySearchController_hyh : ControllerBase
{
    // 数据库连接
    private readonly OracleConnection _connection;

    public SalarySearchController_hyh(OracleConnection connection)
    {
        _connection = connection;
    }

    [HttpPost("/searchWagesRecord")]
    public IActionResult searchSalary(salarySearch salarySearchInfo)
    {
        var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

        // 检查用户权限，如果权限代码为"4",(财务人员)，则允许查询和修改
        if (userRole != null && int.TryParse(userRole, out int roleCode) && roleCode >= 4)
        {
            var PID_issue = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;   // 发放人ID

            if (PID_issue == null)
            {
                return BadRequest("非法访问");
            }

            try
            {

                var sql = "SELECT p.PAYROLL_NUMBER, p.POLICE_NUMBER, p.POLICE_NAME, p.STATION_ID, p.PAY_DAY, p.SALARY, p.SUBSIDY, p.DESCRIPTION, p.ISSUE_ID, m.POLICE_NAME AS ISSUE_NAME " +
                      "FROM PAYROLL_STUB p " +
                      "LEFT JOIN POLICEMEN m ON p.ISSUE_ID = m.POLICE_NUMBER " +
                      "WHERE 1=1 "; // 初始查询条件

                // 构建动态查询条件
                if (!string.IsNullOrEmpty(salarySearchInfo.police_number))
                    sql += "AND p.POLICE_NUMBER = :police_number ";
                if (!string.IsNullOrEmpty(salarySearchInfo.name))
                    sql += "AND p.POLICE_NAME = :name ";
                if (!string.IsNullOrEmpty(salarySearchInfo.year))
                    sql += "AND EXTRACT(YEAR FROM p.PAY_DAY) = :year ";
                if (!string.IsNullOrEmpty(salarySearchInfo.month))
                    sql += "AND EXTRACT(MONTH FROM p.PAY_DAY) = :month ";
                if (!string.IsNullOrEmpty(salarySearchInfo.station))
                    sql += "AND p.STATION_ID = :station ";

                _connection.Open();
                OracleCommand cmd = new OracleCommand(sql, _connection);
                if (!string.IsNullOrEmpty(salarySearchInfo.police_number))
                    cmd.Parameters.Add(new OracleParameter(":police_number", salarySearchInfo.police_number));
                if (!string.IsNullOrEmpty(salarySearchInfo.name))
                    cmd.Parameters.Add(new OracleParameter(":name", salarySearchInfo.name));
                if (!string.IsNullOrEmpty(salarySearchInfo.year))
                    cmd.Parameters.Add(new OracleParameter(":year", salarySearchInfo.year));
                if (!string.IsNullOrEmpty(salarySearchInfo.month))
                    cmd.Parameters.Add(new OracleParameter(":month", salarySearchInfo.month));
                if (!string.IsNullOrEmpty(salarySearchInfo.station))
                    cmd.Parameters.Add(new OracleParameter(":station", salarySearchInfo.station));

                using (var reader = cmd.ExecuteReader())
                {
                    var searchResults = new List<SalaryRecordDTO>(); // SalaryRecordDTO 是用于存储查询结果的DTO类

                    while (reader.Read())
                    {
                        // 从数据库结果中读取数据并添加到结果列表中
                        var record = new SalaryRecordDTO
                        {
                            PAYROLL_NUMBER = reader["PAYROLL_NUMBER"].ToString(),
                            POLICE_NUMBER = reader["POLICE_NUMBER"].ToString(),
                            POLICE_NAME = reader["POLICE_NAME"].ToString(),
                            STATION_ID = reader["STATION_ID"].ToString(),
                            PAY_DAY = reader["PAY_DAY"] as DateTime? ?? DateTime.MinValue,
                            SALARY = reader["SALARY"].ToString(),
                            SUBSIDY = reader["SUBSIDY"].ToString(),
                            DESCRIPTION = reader["DESCRIPTION"].ToString(),
                            ISSUE_ID = reader["ISSUE_ID"].ToString(),
                            ISSUE_NAME = reader["ISSUE_NAME"].ToString()
                        };
                        searchResults.Add(record);
                    }
                    _connection.Close();
                    return Ok(searchResults);
                }
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"服务器错误: {ex.Message}");
            }

        }
        else
        {
            return Unauthorized(); // 权限不足
        }
    }
}
