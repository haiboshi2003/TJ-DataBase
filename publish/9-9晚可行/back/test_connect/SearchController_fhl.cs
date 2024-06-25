using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using web.DTO_group2;
using System.Collections.Generic;
using System.Data.Common;



[ApiController]
public class SearchController_fhl : ControllerBase
{
    private readonly OracleConnection _connection;

    public SearchController_fhl(OracleConnection connection)
    {
        _connection = connection;
    }
    [HttpPost]
    [Route("api/search")]
    
    public ActionResult<string> ChangePermission(notification notice)
    {
        int i = 0;

        try
        {
            _connection.Open();

            string sql = "SELECT * FROM policemen WHERE police_number = :temp";
            OracleCommand command = new OracleCommand(sql, _connection);
            command.Parameters.Add(new OracleParameter("temp", notice.temp));

            using (OracleDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    i++;
                    var police = new Police
                    {
                        police_number = reader.GetString(reader.GetOrdinal("police_number")),
                        police_name = reader.GetString(reader.GetOrdinal("police_name")),
                        ID_number = reader.GetString(reader.GetOrdinal("ID_number")),
                        phone_number = reader.GetString(reader.GetOrdinal("phone_number")),
                        position = reader.GetString(reader.GetOrdinal("position"))
                    };
                    return Ok(police);
                }
            }
        }
        catch (Exception ex)
        {
            // 处理异常或记录错误日志
            return BadRequest("权限修改失败：" + ex.Message);
        }
        finally
        {
            _connection.Close();
        }

        if (i == 0)
        {
            return ("未找到警员信息");
        }

        return Ok("权限修改成功");
    }

    
}


