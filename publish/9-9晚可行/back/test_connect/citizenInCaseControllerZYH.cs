using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data.Common;
using System.Reflection.PortableExecutable;
using System.Text;



[ApiController]
[Route("api/citizenInCaseInfo")]
public class citizenInCaseInfoControllerZYH : ControllerBase
{
    private OracleConnection _connection;

    public citizenInCaseInfoControllerZYH(OracleConnection connection)
    {
        _connection = connection;
    }

    [HttpPost]
    public IActionResult HandleEndpoint(inputCitizenInCaseInfoZYH inputInfo) //接收前端的数据
    {
        List<citizenInCaseInfoZYH> cases = new List<citizenInCaseInfoZYH>();
        
        try
        {
            _connection.Open();
            // 执行 SQL 查询
            using (var command = _connection.CreateCommand())
            {
                command.Connection = _connection;
                //先写SQL语句的主体
                command.CommandText = "SELECT CASE_ID,CASE_TYPE,STATUS,REGISTER_TIME,ADDRESS,RANKING,ID_NUM,RELATED_TYPE,CITIZEN_NAME,GENDER" +
                    " FROM CASES NATURAL JOIN RELATED NATURAL JOIN CITIZEN WHERE 1 = 1";
                //下面的whereClause可以接到SQL语句的后面，实现SQL语句的动态变化
                StringBuilder whereClause = new StringBuilder();

                if (!string.IsNullOrEmpty(inputInfo.caseID))
                {
                    //下面的SQL表示筛选出包含inputInfo.caseID的结果
                    whereClause.Append(" AND CASE_ID LIKE '%' || :caseID || '%'");
                    command.Parameters.Add(":caseID", OracleDbType.Varchar2).Value = inputInfo.caseID;
                }
                if (inputInfo.caseType != "全部")
                {
                    whereClause.Append(" AND CASE_TYPE LIKE '%' || :caseType || '%'");
                    command.Parameters.Add(":caseType", OracleDbType.Varchar2).Value = inputInfo.caseType;
                }
                if (inputInfo.status != "全部")
                {
                    whereClause.Append(" AND STATUS LIKE '%' || :status || '%'");
                    command.Parameters.Add(":status", OracleDbType.Varchar2).Value = inputInfo.status;
                }
                if (!string.IsNullOrEmpty(inputInfo.address))
                {
                    whereClause.Append(" AND ADDRESS LIKE '%' || :address || '%'");
                    command.Parameters.Add(":address", OracleDbType.Varchar2).Value = inputInfo.address;
                }
                if (inputInfo.ranking != "全部")
                {
                    whereClause.Append(" AND RANKING = :ranking");
                    command.Parameters.Add(":ranking", OracleDbType.Char).Value = inputInfo.ranking;
                }
                if (!string.IsNullOrEmpty(inputInfo.IDNum))
                {
                    whereClause.Append(" AND ID_NUM LIKE '%' || :IDNum || '%'");
                    command.Parameters.Add(":IDNum", OracleDbType.Varchar2).Value = inputInfo.IDNum;
                }
                if (inputInfo.relatedType != "全部")
                {
                    whereClause.Append(" AND RELATED_TYPE = :relatedType");
                    command.Parameters.Add(":relatedType", OracleDbType.Varchar2).Value = inputInfo.relatedType;
                }

                if (whereClause.Length > 0)
                {
                    command.CommandText += whereClause.ToString();
                }

                Console.WriteLine($"查询数据SQL为:{command.CommandText}");
                using (OracleDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        citizenInCaseInfoZYH Case = new citizenInCaseInfoZYH
                        {
                            caseID = reader.GetString(reader.GetOrdinal("CASE_ID")),
                            caseType = reader.GetString(reader.GetOrdinal("CASE_TYPE")),
                            status = reader.GetString(reader.GetOrdinal("STATUS")),
                            registerTime = reader.GetDateTime(reader.GetOrdinal("REGISTER_TIME")),
                            address = reader.GetString(reader.GetOrdinal("ADDRESS")),
                            ranking = reader.GetString(reader.GetOrdinal("RANKING")),
                            IDNum = reader.GetString(reader.GetOrdinal("ID_NUM")),
                            relatedType = reader.GetString(reader.GetOrdinal("RELATED_TYPE")),
                            citizenName = reader.GetString(reader.GetOrdinal("CITIZEN_NAME")),
                            gender = reader.GetString(reader.GetOrdinal("GENDER"))
                        };
                        cases.Add(Case);
                    }

                    _connection.Close();
                    return Ok(cases);
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