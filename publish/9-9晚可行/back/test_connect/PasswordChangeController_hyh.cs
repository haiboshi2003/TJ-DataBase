using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using web1.Helpers;
using web.DTO_group2;



namespace PasswordChangeController
{
    [ApiController]
    [Route("api/reset-password")]
    public class PasswordChangeController_hyh : ControllerBase
    {
        private readonly OracleConnection _connection;

        public PasswordChangeController_hyh(OracleConnection connection)
        {
            _connection = connection;
        }

        [HttpPost]
        public IActionResult ResetPassword([FromBody] PasswordChangeRequest request)
        {
            // 获取前端发送的警号和身份证号
            string policeNumber = request.PoliceNumber;
            string idNumber = request.IdNumber;
            string PhoneNumber;
            _connection.Open();

            if (CheckIdentity(policeNumber, idNumber))
            {
                // 身份验证成功，生成令牌
                string token = JwtHelper.CreateToken(policeNumber, "user", 10);
                using (OracleCommand command = _connection.CreateCommand())
                {
                    command.CommandText = "SELECT phone_number FROM POLICEMEN WHERE Police_Number = :PoliceNumber";
                    command.Parameters.Add(new OracleParameter("PoliceNumber", policeNumber));
                    PhoneNumber = command.ExecuteScalar()?.ToString() ?? string.Empty;

                }
                _connection.Close();
                return Ok(new { success = true, phonenumber = PhoneNumber, token });
            }
            else
            {
                return Ok(new { success = false, message = "身份未验证成功" });
            }
        }

        [HttpGet]
        public bool CheckIdentity(string policeNumber, string idNumber)
        {

            using (OracleCommand command = _connection.CreateCommand())
            {
                // 设置查询语句
                command.CommandText = "SELECT COUNT(*) FROM POLICEMEN WHERE Police_Number = :policeNumber";
                command.Parameters.Add(new OracleParameter("policeNumber", policeNumber));

                // 执行查询
                int count = Convert.ToInt32(command.ExecuteScalar());

                if (count == 0)
                {
                    // 警号不存在
                    return false;
                }
                else
                {
                    // 警号存在，继续验证身份证号
                    command.CommandText = "SELECT COUNT(*) FROM POLICEMEN WHERE Police_Number = :policeNumber AND ID_Number = :idNumber";
                    command.Parameters.Add(new OracleParameter("idNumber", idNumber));

                    // 执行查询
                    count = Convert.ToInt32(command.ExecuteScalar());

                    return (count > 0);
                }
            }
        }


    }
}
