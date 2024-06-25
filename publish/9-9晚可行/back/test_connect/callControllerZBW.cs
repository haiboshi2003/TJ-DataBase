using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

[ApiController]
[Route("/")]
public class CallInfoController : ControllerBase
{
    private OracleConnection _connection;

    public CallInfoController(OracleConnection connection)
    {
        _connection = connection;
    }

    [HttpPost("api/callInfo")]
    public IActionResult SearchCallInfo(callinfo inputInfo)
    {
        List<callinfo> callInfos = new List<callinfo>();
        Console.WriteLine($"Searching for data: {inputInfo.audioid}\t{inputInfo.calltime}");
        try
        {
            _connection.Open();
            using (var command = _connection.CreateCommand())
            {
                command.Connection = _connection;
                command.CommandText = "SELECT * FROM CALLS WHERE 1 = 1";
                StringBuilder whereClause = new StringBuilder();

                if (!string.IsNullOrEmpty(inputInfo.audioid))
                {
                    whereClause.Append(" AND AUDIO_ID LIKE '%' || :audioid || '%'");
                    command.Parameters.Add(":audioid", OracleDbType.Varchar2).Value = inputInfo.audioid;
                }
                if (!string.IsNullOrEmpty(inputInfo.calltime))
                {
                    whereClause.Append(" AND CALLS_TIME LIKE '%' || :calltime || '%'");
                    command.Parameters.Add(":calltime", OracleDbType.Varchar2).Value = inputInfo.calltime;
                }
                // Add more conditions if needed...

                if (whereClause.Length > 0)
                {
                    command.CommandText += whereClause.ToString();
                }

                Console.WriteLine($"SQL Query: {command.CommandText}");
                using (OracleDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        callinfo callInfo = new callinfo
                        {
                            audioid = reader.GetString(reader.GetOrdinal("AUDIO_ID")),
                            calltime = reader.GetString(reader.GetOrdinal("CALLS_TIME")),
                            caseid = reader.GetString(reader.GetOrdinal("CASE_ID")),
                            phonenumber = reader.GetString(reader.GetOrdinal("PHONE_NUMBER")),
                            answernumber = reader.GetString(reader.GetOrdinal("ANSWER_NUMBER"))
                        };
                        callInfos.Add(callInfo);
                    }

                    _connection.Close();
                    return Ok(callInfos);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while searching data: {ex.Message}");
            return StatusCode(500, $"Error while searching data: {ex.Message}");
        }
        finally
        {
            _connection.Close();
        }
    }

    [HttpPost("api/addCallInfo")]
    public IActionResult AddCallInfo(callinfo newCallInfo)
    {
        try
        {
            _connection.Open();
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO CALLS (AUDIO_ID, CALLS_TIME, CASE_ID, PHONE_NUMBER, ANSWER_NUMBER) " +
                                      "VALUES (:audioid, :calltime, :caseid, :phonenumber, :answernumber)";
                command.Parameters.Add(":audioid", OracleDbType.Varchar2).Value = newCallInfo.audioid;
                string format = "yyyy-MM-ddTHH:mm:ss.fffffffzzz"; // 包含日期、时间、毫秒和时区的格式字符串
                string time = "2021-12-31T16:55:18.000Z";
                DateTime dateTime;
                DateTime.TryParseExact(time, format, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dateTime);

                command.Parameters.Add(":calltime", OracleDbType.Date).Value = dateTime;
                command.Parameters.Add(":caseid", OracleDbType.Varchar2).Value = newCallInfo.caseid;
                command.Parameters.Add(":phonenumber", OracleDbType.Varchar2).Value = newCallInfo.phonenumber;
                command.Parameters.Add(":answernumber", OracleDbType.Varchar2).Value = newCallInfo.answernumber;

                int rowsAffected = command.ExecuteNonQuery();

                _connection.Close();

                if (rowsAffected > 0)
                {
                    return Ok("Record added successfully");
                }
                else
                {
                    return BadRequest("Failed to add record");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while adding record: {ex.Message}");
            return StatusCode(500, $"Error while adding record: {ex.Message}");
        }
        finally
        {
            _connection.Close();
        }
    }

    [HttpPost("api/deleteCallInfo")]
    public IActionResult DeleteCallInfo(string deleteInfo)
    {
        try
        {
            _connection.Open();
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM CALLS WHERE 1 = 1";
                StringBuilder whereClause = new StringBuilder();

                if (!string.IsNullOrEmpty(deleteInfo))
                {
                    whereClause.Append(" AND AUDIOID LIKE '%' || :audioid || '%'");
                    command.Parameters.Add(":audioid", OracleDbType.Varchar2).Value = deleteInfo;
                }
 
                // Add more conditions if needed...

                if (whereClause.Length > 0)
                {
                    command.CommandText += whereClause.ToString();
                }

                int rowsAffected = command.ExecuteNonQuery();

                _connection.Close();

                if (rowsAffected > 0)
                {
                    return Ok("Records deleted successfully");
                }
                else
                {
                    return BadRequest("No records found to delete");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while deleting records: {ex.Message}");
            return StatusCode(500, $"Error while deleting records: {ex.Message}");
        }
        finally
        {
            _connection.Close();
        }
    }
}
