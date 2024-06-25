﻿using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor.Infrastructure;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;
using System.Numerics;
using web.DTO_group4;

namespace FamilybgCheck
{

    [Route("api/FamilybgCheck")]
    [ApiController]
    public class FamilybgCheck : ControllerBase
    {
        private readonly OracleConnection _connection;
        public FamilybgCheck(OracleConnection connection)
        {
            _connection = connection;
        }
        private bool notExist(string type, List<string> types)
        {
            foreach (string i in types)
            {
                if (type == i)
                    return false;
            }
            return true;
        }
        private void searchParent(ref result output, string targetID)
        {
            string sql = "select citizen.ID_num,citizen_name,gender,case_type " +
                "from citizen inner join related on citizen.ID_num=related.ID_num " +
                "inner join cases on related.case_ID=cases.case_ID " +
                "where citizen.ID_num in (" +
                "select father_ID " +
                "from citizen " +
                "where ID_num=:temp) " +
                "order by case_type";
            try
            {
                OracleCommand command = new OracleCommand(sql, _connection);
                command.Parameters.Add(new OracleParameter("temp", targetID));
                OracleDataReader reader = command.ExecuteReader();
                Info tmp = new Info();
                while (reader.Read())
                {
                    tmp.ID = reader.GetString("ID_num");
                    tmp.name = reader.GetString("citizen_name");
                    tmp.gender = reader.GetString("gender");
                    string readyType = reader.GetString("case_type");
                    if (notExist(readyType, tmp.crimeType))
                    {
                        tmp.crimeType.Add(readyType);
                    }

                }
                output.people.Add(tmp);
                output.relationship.Add("父亲");
            }
            catch (Exception ex)
            {
                return;
            }
            sql = "select citizen.ID_num,citizen_name,gender,case_type " +
                "from citizen inner join related on citizen.ID_num=related.ID_num " +
                "inner join cases on related.case_ID=cases.case_ID " +
                "where citizen.ID_num in (" +
                "select mother_ID " +
                "from citizen " +
                "where ID_num=:temp) " +
                "order by case_type";
            try
            {
                OracleCommand command = new OracleCommand(sql, _connection);
                command.Parameters.Add(new OracleParameter("temp", targetID));
                OracleDataReader reader = command.ExecuteReader();
                Info tmp = new Info();
                while (reader.Read())
                {
                    tmp.ID = reader.GetString("ID_num");
                    tmp.name = reader.GetString("citizen_name");
                    tmp.gender = reader.GetString("gender");
                    string readyType = reader.GetString("case_type");
                    if (notExist(readyType, tmp.crimeType))
                    {
                        tmp.crimeType.Add(readyType);
                    }

                }
                output.people.Add(tmp);
                output.relationship.Add("母亲");
            }
            catch (Exception ex)
            {
                return;
            }
        }
        private void searchChild(ref result output, string targetID)
        {
            string sql = "select citizen.ID_num,citizen_name,gender,case_type " +
                "from citizen inner join related on citizen.ID_num=related.ID_num "+
                "inner join cases on related.case_ID=cases.case_ID  " +
                "where mother_ID=:target or " + "father_ID=:target " +
                "order by citizen.ID_num,case_type";

            try
            {
                OracleCommand command = new OracleCommand(sql, _connection);
                command.Parameters.Add(new OracleParameter(":target", targetID));
                OracleDataReader reader = command.ExecuteReader();
                Info[] tmp = new Info[2]; // 0留存行，1读取行
                tmp[0] = new Info();
                tmp[1] = new Info();
                if (reader.Read())
                {
                    tmp[0].ID = reader.GetString("ID_num");
                    tmp[0].name = reader.GetString("citizen_name");
                    tmp[0].gender = reader.GetString("gender");
                    tmp[0].crimeType.Add(reader.GetString("case_type"));
                }
                while (reader.Read())
                {
                    tmp[1].ID = reader.GetString("ID_num");
                    tmp[1].name = reader.GetString("citizen_name");
                    tmp[1].gender = reader.GetString("gender");
                    string readyType = reader.GetString("case_type");
                    if (tmp[0].ID == tmp[1].ID)
                    {
                        // 还是同一个人
                        if (notExist(readyType, tmp[0].crimeType))
                        {
                            // 出现新犯罪类型，添加
                            tmp[0].crimeType.Add(readyType);
                        }
                    }
                    else
                    {
                        // 换人
                        output.people.Add(tmp[0]);
                        output.relationship.Add(tmp[0].gender == "F" ? "女儿" : "儿子");
                        // 先存储信息到结果，再挪信息
                        tmp[0] = tmp[1];
                        tmp[0].crimeType.Add(readyType);
                    }
                }
                // 全部读完以后，最后一个人在[0]，未写入
                if (tmp[0].name != null)
                {
                    output.people.Add(tmp[0]);
                    output.relationship.Add(tmp[0].gender == "F" ? "女儿" : "儿子");
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }

        [HttpPost]
        public ActionResult<result> HandleFamily(MyRequestData requestData)
        {
            string inputText = requestData.InputText; // 从请求的JSON数据中获取输入的字符串                                
            result queryResult = new result();

            string query = "select citizen.ID_num,citizen_name,gender,case_type " +
                "from citizen inner join related on citizen.ID_num=related.ID_num " +
                "inner join cases on related.case_ID=cases.case_ID " +
                "where citizen.ID_num=:temp and related_type='犯人' " +
                "order by case_type";

            try
            {
                _connection.Open();
                // 创建Oracle命令对象
                OracleCommand command = new OracleCommand(query, _connection);
                command.Parameters.Add(new OracleParameter("temp", inputText));
                OracleDataReader reader = command.ExecuteReader();
                if (reader.HasRows == false)
                {
                    return Ok(new result());
                }
                else
                {
                    Info tmp = new Info();
                    while (reader.Read())
                    { // 必然只有一个人
                        tmp.name = reader.GetString("citizen_name");
                        tmp.ID = reader.GetString("ID_num");
                        tmp.gender = reader.GetString("gender");
                        string readyType = reader.GetString("case_type");
                        if (notExist(readyType, tmp.crimeType))
                        {
                            // 类型不重复
                            tmp.crimeType.Add(readyType);
                        }
                    }
                    queryResult.people.Add(tmp);
                    queryResult.relationship.Add("");
                    searchParent(ref queryResult, tmp.ID);
                    searchChild(ref queryResult, tmp.ID);
                }
            }
            catch (Exception ex)
            {
                return Ok(new result());
            }
            finally
            {
                _connection.Close();
            }
            return Ok(queryResult);

        }
    }
}