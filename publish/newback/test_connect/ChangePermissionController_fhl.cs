using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data.Common;
using System.Drawing;
using System.Security.Claims;
using web.DTO_group2;



namespace WebApplication1
{
    [Authorize] // 添加Authorize特性，表示需要进行身份验证
    [ApiController]
    [Route("api/permit")]
    public class ChangePermissionController_fhl:ControllerBase
    {
        private  OracleConnection _connection;

        
        public ChangePermissionController_fhl(OracleConnection connection)
        {
            _connection = connection;
        }
        [HttpPost]
        public ActionResult<string> ChangePermission(permit P)
        {
            var policeNO = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;//通过token获取申请人警号
            P.h_number = policeNO;
            int i = 0;
            int n = 0;
            try
            {
                _connection.Open();
                int s_level = 0;
                string sql = "SELECT * FROM police_account WHERE police_number = :temp";
                
                OracleCommand command1 = new OracleCommand(sql, _connection);
                command1.Parameters.Add(new OracleParameter("temp", P.s_number));//搜索被修改人权限等级
                using (OracleDataReader reader1 = command1.ExecuteReader())
                {
                    while (reader1.Read())
                    {
                        s_level = reader1.GetInt32(reader1.GetOrdinal("AUTHORITY"));//获取被修改人权限等级
                    }
                }
                P.F_level = s_level.ToString();

                //先检查是否已经有相同的申请
                sql = "SELECT COUNT(*) FROM permission_manage WHERE submit_ID = :temp1 AND change_ID = :temp2 AND F_level = :temp3 AND L_level = :temp4";
                OracleCommand command2 = new OracleCommand(sql, _connection);
                command2.Parameters.Add(new OracleParameter("temp1", P.h_number));
                command2.Parameters.Add(new OracleParameter("temp2", P.s_number));
                command2.Parameters.Add(new OracleParameter("temp3", P.F_level));
                command2.Parameters.Add(new OracleParameter("temp4", P.L_level));
                
                using (OracleDataReader reader2 = command2.ExecuteReader())
                { 
                    while (reader2.Read())
                    {
                        n = reader2.GetInt32(0); // 获取查询结果的整数值
                    }
                }
                //没有相同申请则创建该申请
                if (n == 0)
                {
                    sql = "INSERT INTO permission_manage(submit_ID, change_ID, F_level, L_level, status, reason) VALUES(:submitID, :changeID, :Flevel, :Llevel, :status, :reason)";
                    //string sql1 = "INSERT INTO permission_manage VALUES('3406524','9726541','1', '4', '待处理', 'test1')";
                    OracleCommand command = new OracleCommand(sql, _connection);
                    command.Parameters.Add(new OracleParameter("submitID", P.h_number));//申请修改人警号
                    command.Parameters.Add(new OracleParameter("changeID", P.s_number));//被修改人警号
                    command.Parameters.Add(new OracleParameter("Flevel", s_level));//被修改人权限等级
                    command.Parameters.Add(new OracleParameter("Llevel", P.L_level));//修改等级
                    command.Parameters.Add(new OracleParameter("status", "待处理"));//修改状态
                    command.Parameters.Add(new OracleParameter("reason", P.reason));//修改原因
                    i = command.ExecuteNonQuery();
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

            if (i == 0 && n == 0)
            {
                return BadRequest("权限修改失败");
            }
            else if (n == 1)
                return ("已存在该申请");
            return Ok("申请提交成功");
        }
    }
}
