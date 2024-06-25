using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data.Common;
using System.Drawing;
using System.Security.Claims;
using web.DTO_group2;



namespace WebApplication2
{
    [ApiController]
    [Route("api/manage")]
    public class PermissionMngController_fhl : ControllerBase
    {
        private OracleConnection _connection;


        public PermissionMngController_fhl(OracleConnection connection)
        {
            _connection = connection;
        }
        [HttpGet]
        public ActionResult<string> DisplayPermission()
        {
            List<permit> permissionManageData = new List<permit>();
            var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            // 检查用户权限，如果权限代码为"5"或以上，则允许查询
            if (userRole != null && int.TryParse(userRole, out int roleCode) && roleCode == 5)
            {
                try
                {
                    _connection.Open();
                    string sql = "SELECT * FROM Permission_Manage";
                    OracleCommand command = new OracleCommand(sql, _connection);
                    OracleDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        permit p = new permit();
                        // 检查并处理每个属性的NULL值
                        p.s_number = reader.IsDBNull(reader.GetOrdinal("change_ID")) ? null : reader.GetString(reader.GetOrdinal("change_ID"));
                        p.h_number = reader.IsDBNull(reader.GetOrdinal("submit_ID")) ? null : reader.GetString(reader.GetOrdinal("submit_ID"));
                        p.F_level = reader.IsDBNull(reader.GetOrdinal("F_level")) ? null : reader.GetString(reader.GetOrdinal("F_level"));
                        p.L_level = reader.IsDBNull(reader.GetOrdinal("L_level")) ? null : reader.GetString(reader.GetOrdinal("L_level"));
                        p.status = reader.IsDBNull(reader.GetOrdinal("status")) ? null : reader.GetString(reader.GetOrdinal("status"));
                        p.reason = reader.IsDBNull(reader.GetOrdinal("reason")) ? null : reader.GetString(reader.GetOrdinal("reason"));
                        permissionManageData.Add(p);
                    }

                    reader.Close();
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

                return Ok(permissionManageData);
            }
            else
            {
                return Unauthorized(); // 权限不足
            }
        }

        [HttpPost]
        public ActionResult<string> ChangePermission(permit permission)
        {
            var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            // 检查用户权限，如果权限代码为"5"或以上，则允许查询
            if (userRole != null && int.TryParse(userRole, out int roleCode) && roleCode == 5)
            {
                try
                {
                    _connection.Open();
                    string sql = "UPDATE PERMISSION_MANAGE SET STATUS = :status WHERE SUBMIT_ID = :h_number and CHANGE_ID = :s_number";
                    OracleCommand command = new OracleCommand(sql, _connection);
                    command.Parameters.Add(new OracleParameter("status", permission.status));
                    command.Parameters.Add(new OracleParameter("h_number", permission.h_number));
                    command.Parameters.Add(new OracleParameter("s_number", permission.s_number));
                    int i = command.ExecuteNonQuery();
                    if (permission.status == "同意")
                    {
                        sql = "UPDATE POLICE_ACCOUNT SET AUTHORITY = :L_level WHERE POLICE_NUMBER = :p_id";
                        OracleCommand command_1 = new OracleCommand(sql, _connection);
                        //command.Parameters.Clear();
                        //command_1.Parameters["level"].Value = int.Parse(permission.L_level);
                        command_1.Parameters.Add(new OracleParameter("L_level", permission.L_level));
                        command_1.Parameters.Add(new OracleParameter("p_id", permission.s_number));
                        i = command_1.ExecuteNonQuery();
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

                return Ok("123");
            }
            else
            {
                return Unauthorized(); // 权限不足
            }
        }

    }
}
