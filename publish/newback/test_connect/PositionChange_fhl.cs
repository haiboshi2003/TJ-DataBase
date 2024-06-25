using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data.Common;
using System.Drawing;
using web.DTO_group2;



namespace WebApplication3
{
    [ApiController]
    [Route("api/position")]
    public class PositionManage : ControllerBase
    {
        private OracleConnection _connection;


        public PositionManage(OracleConnection connection)
        {
            _connection = connection;
        }
        
        [HttpPost]
        public ActionResult<string> ChangePoaition(Police p)
        {
            try
            {
                _connection.Open();
                string sql = "UPDATE POLICEMEN SET POSITION = :temp1 WHERE POLICE_NUMBER = :tmep2";
                OracleCommand command = new OracleCommand(sql, _connection);
                command.Parameters.Add(new OracleParameter("temp1", p.position));
                command.Parameters.Add(new OracleParameter("temp2", p.police_number));
                int i = command.ExecuteNonQuery();
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

            return Ok("123");
        }

    }
}
