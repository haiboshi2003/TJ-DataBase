using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using web.Helpers;
using web.DTO_group2;
using System.Security.Claims;
using System.Drawing;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Authorize]
public class SalaryNewRecord_hyh : ControllerBase
{
    // 数据库连接
    private readonly OracleConnection _connection;

    public SalaryNewRecord_hyh(OracleConnection connection)
    {
        _connection = connection;
    }



    [HttpPost("/salary_newRecord")]
    public IActionResult insertSalary(salaryInfo sInfo)
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
                string query_receive = "SELECT POLICE_NAME FROM POLICEMEN WHERE POLICE_NUMBER = :PoliceNumber";
                string query_station = "SELECT STATION_ID FROM EMPLOY WHERE POLICE_NUMBER = :PoliceNumber";


                // 连接数据库并执行查询
                _connection.Open();

                // 执行第一个查询
                OracleCommand command_receive = new OracleCommand(query_receive, _connection);
                command_receive.Parameters.Add(new OracleParameter("PoliceNumber", sInfo.police_number_receive)); // 请替换 yourPoliceNumberValue 为实际的 PoliceNumber
                object receiveResult = command_receive.ExecuteScalar();

                // 执行第二个查询
                OracleCommand command_station = new OracleCommand(query_station, _connection);
                command_station.Parameters.Add(new OracleParameter("PoliceNumber", sInfo.police_number_receive)); // 请替换 yourPoliceNumberValue 为实际的 PoliceNumber
                object stationResult = command_station.ExecuteScalar();

                // 关闭数据库连接
                _connection.Close();

                string PName;
                string station;
                if (receiveResult == null || stationResult == null)
                {
                    return Ok("员工信息有误");
                }
                else
                {
                    PName = receiveResult.ToString();
                    station =    stationResult.ToString();
                }

                string serialID = SerialnumHelper.GetSerialNum("P");   // 序列号"P"是开头 head，可以换
                string basicSalary = sInfo.basic_amount;              // 基本工资
                string rewardSalary = sInfo.reward_amount;            // 奖金

           

                string description = sInfo.description;            // 描述

                string insertQuery = "INSERT INTO PAYROLL_STUB (PAYROLL_NUMBER, POLICE_NUMBER, POLICE_NAME, STATION_ID, PAY_DAY, SALARY, SUBSIDY，DESCRIPTION, ISSUE_ID) " +
                                                       "VALUES (:PayrollNumber, :PoliceNumber, :PoliceName, :StationID, :PayDay, :Salary, :Subsidy, :Description,:ISSUE_ID )";
                // 创建一个 OracleCommand 对象
                OracleCommand insertCommand = new OracleCommand(insertQuery, _connection);

                // 添加参数
                insertCommand.Parameters.Add(new OracleParameter(":PayrollNumber", serialID));
                insertCommand.Parameters.Add(new OracleParameter(":PoliceNumber", sInfo.police_number_receive));
                insertCommand.Parameters.Add(new OracleParameter(":PoliceName", PName));
                insertCommand.Parameters.Add(new OracleParameter(":StationID", station));
                insertCommand.Parameters.Add(new OracleParameter(":PayDay", DateTime.Now));
                insertCommand.Parameters.Add(new OracleParameter(":Salary", basicSalary));
                insertCommand.Parameters.Add(new OracleParameter(":Subsidy", rewardSalary));
                insertCommand.Parameters.Add(new OracleParameter(":Description", description));
                insertCommand.Parameters.Add(new OracleParameter(":ISSUE_ID", PID_issue));


                // 打开连接并执行插入
                _connection.Open();
                int rowsInserted = insertCommand.ExecuteNonQuery(); // 返回插入的行数
                _connection.Close();

                if (rowsInserted > 0)
                {
                    // 插入成功
                   return Ok("插入成功！");
                }
                else
                {
                    // 插入失败
                    return Ok("插入失败！");
                }

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

