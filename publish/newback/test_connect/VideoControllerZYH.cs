using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data.Common;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;


[ApiController]
[Route("/")]
public class videoInfoControllerZYH : ControllerBase
{
    private OracleConnection _connection;

    public videoInfoControllerZYH(OracleConnection connection)
    {
        _connection = connection;
    }

    [HttpGet("api/videoInfo")]
    public IActionResult HandleEndpoint([FromQuery] string? videoID, [FromQuery] string? videoType, [FromQuery] DateTimeOffset recordTime, [FromQuery] string? isT, [FromQuery] string? caseID, [FromQuery] string? caseType, [FromQuery] string? policemenID)
    {
        var videos = new List<object>();
        try
        {
            _connection.Open();
            // 执行 SQL 查询
            using (var command = _connection.CreateCommand())
            {
                command.Connection = _connection;
                command.CommandText = "SELECT * FROM VIDEOS LEFT OUTER JOIN CASE_VIDEO ON VIDEOS.VIDEO_ID = CASE_VIDEO.VIDEO_ID WHERE 1 = 1";
                StringBuilder whereClause = new StringBuilder();


                if (!string.IsNullOrEmpty(videoID))
                {
                    whereClause.Append(" AND VIDEOS.VIDEO_ID  LIKE '%' ||  :videoID || '%'");
                    command.Parameters.Add(":videoID", OracleDbType.Varchar2).Value = videoID;
                }
                if (!string.IsNullOrEmpty(videoType))
                {
                    whereClause.Append(" AND VIDEO_TYPE = :videoType");
                    command.Parameters.Add(":videoType", OracleDbType.Varchar2).Value = videoType;
                }
                if (isT == "T")
                {
                    DateTime localDateTime = recordTime.LocalDateTime;
                    whereClause.Append(" AND TRUNC(RECORD_TIME) = TRUNC(:time)");
                    command.Parameters.Add(":time", OracleDbType.Date).Value = localDateTime;
                }
                if (!string.IsNullOrEmpty(caseType))
                {
                    if (caseType == "强奸")
                        caseType = "A";
                    else if (caseType == "抢劫")
                        caseType = "R";
                    else if (caseType == "故意伤害")
                        caseType = "H";
                    else if (caseType == "盗窃")
                        caseType = "T";
                    else if (caseType == "诈骗")
                        caseType = "C";
                    else if (caseType == "谋杀")
                        caseType = "M";
                    else
                        return Ok("未知的案件类型！获取接警记录失败！");
                    whereClause.Append(" AND CASE_ID LIKE '%' || :caseIDT || '%'");
                    command.Parameters.Add(":caseIDT", OracleDbType.Varchar2).Value = caseType;
                }
                if (!string.IsNullOrEmpty(caseID))
                {
                    whereClause.Append(" AND CASE_ID LIKE '%' || :caseID || '%'");
                    command.Parameters.Add(":caseID", OracleDbType.Varchar2).Value = caseID;
                }
                if (!string.IsNullOrEmpty(policemenID))
                {
                    whereClause.Append(" AND PRINCIPLE_ID LIKE '%' || :policemenID || '%'");
                    command.Parameters.Add(":policemenID", OracleDbType.Varchar2).Value = policemenID;
                }

                if (whereClause.Length > 0)
                {
                    command.CommandText += whereClause.ToString();
                }

                using (OracleDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        var video = new
                        {
                            videoID = reader.GetString(reader.GetOrdinal("VIDEO_ID")),
                            recordTime = reader.GetDateTime(reader.GetOrdinal("RECORD_TIME")),
                            uploadTime = reader.GetDateTime(reader.GetOrdinal("UPLOAD_TIME")),
                            videoType = reader.GetString(reader.GetOrdinal("VIDEO_TYPE")),
                            policemenID = reader.GetString(reader.GetOrdinal("PRINCIPLE_ID")),
                            caseID = reader.IsDBNull(reader.GetOrdinal("CASE_ID")) ? null : reader.GetString(reader.GetOrdinal("CASE_ID")),
                        };
                        videos.Add(video);
                    }

                    _connection.Close();
                    return Ok(videos);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"videoInfo查询数据时发生错误:{ex.Message}");
            return StatusCode(499, "查询数据时发生错误！");
        }
        finally
        {
            _connection.Close();
        }
    }

    [HttpPost("api/addVideo")]
    public IActionResult AddHandleEndpoint([FromQuery] string? videoID, [FromQuery] string? videoType, [FromQuery] DateTimeOffset recordTime, [FromQuery] string? policemenID)
    {
        try
        {
            _connection.Open();

            if (string.IsNullOrEmpty(videoID) || string.IsNullOrEmpty(videoType) || string.IsNullOrEmpty(policemenID))
                return Ok("信息不全！新增录像失败！");

            if (!Regex.IsMatch(videoID, @"^\d+$"))
                return Ok("无效的录像编号！新增录像失败！");

            if (videoID.Length < 5)
                return Ok("录像编号过短！新增录像失败！");

            if (policemenID.Length < 7)
                return Ok("警员编号过短！新增录像失败！");

            // 查询录像编号是否存在
            using (var command1 = _connection.CreateCommand())
            {
                command1.Connection = _connection;
                command1.CommandText = "SELECT * FROM VIDEOS WHERE VIDEO_ID = :videoID";
                command1.Parameters.Add(":videoID", OracleDbType.Varchar2).Value = videoID;
                using (OracleDataReader reader = command1.ExecuteReader())
                    if (reader.HasRows)
                        return Ok("该录像编号已存在！新增录像失败！");
            }
            // 查询警员编号是否存在
            using (var command1 = _connection.CreateCommand())
            {
                command1.Connection = _connection;
                command1.CommandText = "SELECT * FROM POLICEMEN WHERE POLICE_NUMBER = :ID";
                command1.Parameters.Add(":ID", OracleDbType.Varchar2).Value = policemenID;
                using (OracleDataReader reader = command1.ExecuteReader())
                    if (!reader.HasRows)
                        return Ok("该警员不存在！新增录像失败！");
            }

            string insertQuery = "INSERT INTO VIDEOS (VIDEO_ID, VIDEO_TYPE, RECORD_TIME, UPLOAD_TIME, PRINCIPLE_ID) " +
                        "VALUES (:videoID, :videoType, :recordTime, :uploadTime, :policemenID)";

            DateTime rTime = recordTime.LocalDateTime; 
            DateTime uploadTime = DateTime.Now;
            using (OracleCommand command = new OracleCommand(insertQuery, _connection))
            {
                command.Connection = _connection;
                command.Parameters.Add(":videoID", OracleDbType.Varchar2).Value = videoID;
                command.Parameters.Add(":videoType", OracleDbType.Varchar2).Value = videoType;
                command.Parameters.Add(":recordTime", OracleDbType.Date).Value = rTime;
                command.Parameters.Add(":uploadTime", OracleDbType.Date).Value = uploadTime;
                command.Parameters.Add(":policemenID", OracleDbType.Varchar2).Value = policemenID;

                command.ExecuteNonQuery();
                // 插入成功，返回 JSON 数据
                var response = new { data = "成功" };
                return Ok(JsonSerializer.Serialize(response));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"addVideo插入数据时发生错误:{ex.Message}");
            return StatusCode(499, "插入数据时发生错误！");
        }
        finally
        {
            _connection.Close();
        }
    }

    [HttpDelete("api/delVideo")]
    public IActionResult DeleteHandleEndpoint([FromQuery] string? videoID)
    {
        try
        {
            _connection.Open();

            // 检查录像ID是否规范
            if (string.IsNullOrEmpty(videoID) || videoID.Length < 5 || !Regex.IsMatch(videoID, @"^\d+$"))
                return Ok("录像编号无效！删除录像失败！");

            // 查询该录像ID对应的行是否存在
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM VIDEOS WHERE VIDEO_ID = :videoID";
                command.Parameters.Add(":videoID", OracleDbType.Varchar2).Value = videoID;
                using (OracleDataReader reader = command.ExecuteReader())
                {
                    if (!reader.HasRows)
                        return Ok("未找到该编号的录像！删除录像失败！");
                }
            }

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM CASE_VIDEO WHERE VIDEO_ID = :videoID";
                command.Parameters.Add(":videoID", OracleDbType.Varchar2).Value = videoID;
                command.ExecuteNonQuery();
            }
            // 执行删除录像操作
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM VIDEOS WHERE VIDEO_ID = :videoID";
                command.Parameters.Add(":videoID", OracleDbType.Varchar2).Value = videoID;
                command.ExecuteNonQuery();
            }

            // 返回成功的JSON响应
            var response = new { data = "成功" };
            return Ok(JsonSerializer.Serialize(response));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"delVideo删除数据时发生错误: {ex.Message}");
            return StatusCode(499, "删除数据时发生错误！");
        }
        finally
        {
            _connection.Close();
        }
    }


    [HttpPut("api/relVideo")]
    public IActionResult PutHandleEndpoint([FromQuery] string? videoID, [FromQuery] string? caseID, [FromQuery] string? caseType)
    {
        try
        {
            _connection.Open();

            if (string.IsNullOrEmpty(videoID) || string.IsNullOrEmpty(caseType) || string.IsNullOrEmpty(caseID))
                return Ok("信息不全！关联案件失败！");

            // 检查接警ID是否规范
            if (videoID.Length < 5 || !Regex.IsMatch(videoID, @"^\d+$"))
                return Ok("录像编号无效！关联案件失败！");

            if (caseID.Length < 7 || !Regex.IsMatch(caseID, @"^\d+$"))
                return Ok("案件编号无效！关联案件失败！");

            if (caseType == "强奸")
                caseID = "A" + caseID;
            else if (caseType == "抢劫")
                caseID = "R" + caseID;
            else if (caseType == "故意伤害")
                caseID = "H" + caseID;
            else if (caseType == "盗窃")
                caseID = "T" + caseID;
            else if (caseType == "诈骗")
                caseID = "C" + caseID;
            else if (caseType == "谋杀")
                caseID = "M" + caseID;
            else
                return Ok("未知的案件类型！关联案件失败！");

            // 查询该录像ID对应的行是否存在
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM VIDEOS WHERE VIDEO_ID = :videoID";
                command.Parameters.Add(":videoID", OracleDbType.Varchar2).Value = videoID;
                using (OracleDataReader reader = command.ExecuteReader())
                {
                    if (!reader.HasRows)
                        return Ok("未找到该编号的录像！关联案件失败！");
                }
            }
            // 查询案件是否存在
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM CASES WHERE CASE_ID = :ID";
                command.Parameters.Add(":ID", OracleDbType.Varchar2).Value = caseID;
                using (OracleDataReader reader = command.ExecuteReader())
                {
                    if (!reader.HasRows)
                        return Ok("未找到该编号的案件！关联案件失败！");
                }
            }

            // 执行操作
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM CASE_VIDEO WHERE VIDEO_ID = :videoID";
                command.Parameters.Add(":videoID", OracleDbType.Varchar2).Value = videoID;
                using (OracleDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        using (var command1 = _connection.CreateCommand())
                        {
                            command1.CommandText = "UPDATE CASE_VIDEO SET CASE_ID = :caseID WHERE VIDEO_ID = :videoID";
                            command1.Parameters.Add(":caseID", OracleDbType.Varchar2).Value = caseID;
                            command1.Parameters.Add(":videoID", OracleDbType.Varchar2).Value = videoID;
                            command1.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        using (var command1 = _connection.CreateCommand())
                        {
                            command1.CommandText = "INSERT INTO CASE_VIDEO(VIDEO_ID, CASE_id) VALUES (:videoID, :caseID)";
                            command1.Parameters.Add(":videoID", OracleDbType.Varchar2).Value = videoID;
                            command1.Parameters.Add(":caseID", OracleDbType.Varchar2).Value = caseID;
                            command1.ExecuteNonQuery();
                        }
                    }
                }
            }

            // 返回成功的JSON响应
            var response = new { data = "成功" };
            return Ok(JsonSerializer.Serialize(response));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"relVideo修改数据时发生错误: {ex.Message}");
            return StatusCode(499, "修改数据时发生错误！");
        }
        finally
        {
            _connection.Close();
        }
    }


    [HttpPut("api/updVideo")]
    public IActionResult PutHandleEndpoint([FromQuery] string? videoID, [FromQuery] string? videoType, [FromQuery] DateTimeOffset recordTime, [FromQuery] string? policemenID)
    {
        try
        {
            _connection.Open();

            if (string.IsNullOrEmpty(videoID) || string.IsNullOrEmpty(videoType) || string.IsNullOrEmpty(policemenID))
                return Ok("录像信息不全！修改信息失败！");

            // 检查录像ID是否规范
            if (videoID.Length < 5 || !Regex.IsMatch(videoID, @"^\d+$"))
                return Ok("录像编号无效！修改信息失败！");

            if (policemenID.Length < 7 || !Regex.IsMatch(policemenID, @"^\d+$"))
                return Ok("警员编号无效！修改信息失败！");

            // 查询该录像ID对应的行是否存在
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM VIDEOS WHERE VIDEO_ID = :videoID";
                command.Parameters.Add(":videoID", OracleDbType.Varchar2).Value = videoID;
                using (OracleDataReader reader = command.ExecuteReader())
                {
                    if (!reader.HasRows)
                        return Ok("未找到该编号的录像！修改信息失败！");
                }
            }
            // 查询警员是否存在
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM POLICEMEN WHERE POLICE_NUMBER = :policemenID";
                command.Parameters.Add(":policemenID", OracleDbType.Varchar2).Value = policemenID;
                using (OracleDataReader reader = command.ExecuteReader())
                {
                    if (!reader.HasRows)
                        return Ok("未找到该编号的警员！修改信息失败！");
                }
            }

            // 执行更新录像操作
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "UPDATE VIDEOS SET RECORD_TIME = :recordTime, VIDEO_TYPE = :videoType, " +
                    "PRINCIPLE_ID = :policemenID WHERE VIDEO_ID = :videoID";

                DateTime rTime = recordTime.LocalDateTime;
                command.Parameters.Add(":recordTime", OracleDbType.Date).Value = rTime;
                command.Parameters.Add(":videoType", OracleDbType.Varchar2).Value = videoType;
                command.Parameters.Add(":policemenID", OracleDbType.Varchar2).Value = policemenID;
                command.Parameters.Add(":videoID", OracleDbType.Varchar2).Value = videoID;
                command.ExecuteNonQuery();
            }

            // 返回成功的JSON响应
            var response = new { data = "成功" };
            return Ok(JsonSerializer.Serialize(response));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"updVideo修改数据时发生错误: {ex.Message}");
            return StatusCode(499, "修改数据时发生错误！");
        }
        finally
        {
            _connection.Close();
        }
    }
}