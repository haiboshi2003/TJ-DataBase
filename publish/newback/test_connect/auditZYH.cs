using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data.Common;
using System.Reflection.PortableExecutable;
using System.Text;

public class inputAuditInfo
{
    public string myAuthority { get; set; } = "";
    public string myPoliceNumber { get; set; } = "";
}
public class auditZYH
{
    public string OS_USERNAME { get; set; } = "";
    public string USERNAME { get; set; } = "";
    public string OBJ_NAME { get; set; } = "";
    public string ACTION_NAME { get; set; } = "";
    public string ACTION_TIME { get; set; } = "";
}

[ApiController]
[Route("api/audit")]
public class auditZYHController : ControllerBase
{
    private OracleConnection _connection;

    public auditZYHController(OracleConnection connection)
    {
        _connection = connection;
    }

    [HttpPost]
    public IActionResult HandleEndpoint(inputAuditInfo inputInfo) //接收前端的数据
    {
        List<auditZYH> audit = new List<auditZYH>();
        try
        {
            _connection.Open();
            // 执行 SQL 查询
            using (var command = _connection.CreateCommand())
            {
                command.Connection = _connection;
                //SQL语句的主体
                command.CommandText = "SELECT OS_USERNAME,USERNAME, OBJ_NAME, ACTION_NAME, TO_CHAR(TIMESTAMP, 'YYYY-MM-DD HH24:MI:SS') AS ACTION_TIME FROM (SELECT  DISTINCT a.OS_USERNAME, a.USERNAME, a.OBJ_NAME, a.ACTION_NAME, a.TIMESTAMP FROM DBA_AUDIT_TRAIL a WHERE OBJ_NAME IS NOT NULL AND ACTION_NAME IN ('INSERT','UPDATE','DELETE','CREATE','ALTER','DROP','GRANT','REVOKE') AND ROWNUM <= 50000 ORDER BY a.TIMESTAMP DESC ) WHERE ROWNUM <= 50000";              
                using (OracleDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        auditZYH Audit = new auditZYH
                        {
                            OS_USERNAME = reader.GetString(reader.GetOrdinal("OS_USERNAME")),
                            USERNAME = reader.GetString(reader.GetOrdinal("USERNAME")),
                            OBJ_NAME = reader.GetString(reader.GetOrdinal("OBJ_NAME")),
                            ACTION_NAME = reader.GetString(reader.GetOrdinal("ACTION_NAME")),
                            //ACTION_TIME = reader.GetDateTime(reader.GetOrdinal("ACTION_TIME")),
                            ACTION_TIME = reader.GetString(reader.GetOrdinal("ACTION_TIME"))
                        };
                        audit.Add(Audit);
                    }
                    _connection.Close();
                    return Ok(audit);
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
}
