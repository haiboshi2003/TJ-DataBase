﻿using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor.Infrastructure;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;
using System.Numerics;
using web.DTO_group4;



namespace Register
{
    [ApiController]

    public class Register : ControllerBase
    {
        private readonly OracleConnection _connection;
        public Register(OracleConnection connection)
        {
            _connection = connection;
        }
        [HttpPut]
        [Route("api/Register")]
        public ActionResult<string> InsertRegister(RequestData requestData)
        {
            string result = "success";
            string pwd = requestData.police_number; // 密码默认警号
            string query = "insert into policemen " +
                "values(:police_number," +
                ":police_name," +
                ":ID_number," +
                ":birthday," +
                ":gender," +
                ":nation," +
                ":phone_number," +
                ":email," +
                ":status," +
                ":position)";
            decimal author = 0;
            if (requestData.position=="学员")
                author = 0;
            else if (requestData.position=="警员")
                author = 1;
            else if (requestData.position == "警司")
                author = 2;
            else if (requestData.position == "警督")
                author = 3;
            else if (requestData.position == "警监")
                author = 4;
            else if (requestData.position == "总警监")
                author = 5;
            try
            {
                _connection.Open();
                // 创建Oracle命令对象
                OracleCommand command = new OracleCommand(query, _connection);
                command.Parameters.Add(new OracleParameter("police_number", requestData.police_number));
                command.Parameters.Add(new OracleParameter("police_name", requestData.police_name));
                command.Parameters.Add(new OracleParameter("ID_number", requestData.ID_number));
                command.Parameters.Add(new OracleParameter("birthday", requestData.birthday));
                command.Parameters.Add(new OracleParameter("gender", requestData.gender));
                command.Parameters.Add(new OracleParameter("nation", requestData.nation));
                command.Parameters.Add(new OracleParameter("phone_number", requestData.phone_number));
                command.Parameters.Add(new OracleParameter("email", requestData.email));
                command.Parameters.Add(new OracleParameter("status", requestData.status));
                command.Parameters.Add(new OracleParameter("position", requestData.position));
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                result = "fail";
                return Ok(result);
            }
            finally
            {
                _connection.Close();
            }
            // 设置初始密码
            string pwdInsert = "insert into police_account " +
                "values(:police_number, " +
                ":key, " +
                ":authority)";
            try
            {
                _connection.Open();
                OracleCommand orclcmd= new OracleCommand(pwdInsert, _connection);
                orclcmd.Parameters.Add(new OracleParameter("police_number", requestData.police_number));
                orclcmd.Parameters.Add(new OracleParameter("key", pwd));
                orclcmd.Parameters.Add(new OracleParameter("authority", OracleDbType.Decimal));
                orclcmd.Parameters["authority"].Value = author;
                orclcmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                result = "fail";
                return Ok(result);
            }
            finally
            {
                _connection.Close();
            }
        
            return Ok(result);
        }

        [HttpPost]
        [Route("api/Register")]
        public ActionResult<string> SearchLegalInfomation(LegalJudgeMessage legalJudgeMessage)
        {
            string sql = "";
            if (legalJudgeMessage.queryType == "police_number")
                sql = "select police_number " +
                    "from policemen " +
                    "where police_number=:temp";
            else if (legalJudgeMessage.queryType == "ID_number")
                sql = "select ID_number " +
                    "from policemen " +
                    "where ID_number=:temp";

            try
            {
                OracleCommand command = new OracleCommand(sql, _connection);
                command.Parameters.Add(new OracleParameter("temp", legalJudgeMessage.queryContent));
                _connection.Open();
                OracleDataReader reader = command.ExecuteReader();
                if (reader.HasRows == false)
                {
                    return Ok("ok");
                }
                else
                {
                    return Ok("fail");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Ok("not");
            }
            finally
            {
                _connection.Close();
            }

        }
    }
}
