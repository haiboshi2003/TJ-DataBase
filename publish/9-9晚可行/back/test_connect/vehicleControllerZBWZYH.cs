using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;


[ApiController]
[Route("/")]
public class vehicleController : ControllerBase
{
    private OracleConnection _connection;
    public vehicleController(OracleConnection connection)
    {
        _connection = connection;
    }
    [HttpGet("api/vehicles")]
    public IActionResult SearchVehicles([FromQuery] string? VID, [FromQuery] string? VTYPE, [FromQuery] string? VST)
    {
        List<Vehicleinfo> Vehiclem = new List<Vehicleinfo>();
        try
        {
            //下面的whereClause可以接到SQL语句的后面，实现SQL语句的动态变化
            _connection.Open();
            using (var command = _connection.CreateCommand())
            {
                string sql = "SELECT * FROM VEHICLE WHERE 1 = 1";
                command.Connection = _connection;
                command.CommandText = sql;
                StringBuilder whereClause = new StringBuilder();
                if (!string.IsNullOrEmpty(VID))
                {
                    whereClause.Append(" AND Vehicle_ID  LIKE '%' || :vehicleID || '%'");
                    command.Parameters.Add(":vehicleID", OracleDbType.Varchar2).Value = VID;
                }
                if (!string.IsNullOrEmpty(VTYPE))
                {
                    whereClause.Append(" AND Vehicle_Type LIKE '%' || :VehicleType || '%'");
                    command.Parameters.Add(":VehicleType", OracleDbType.Varchar2).Value = VTYPE;
                }
                if (!string.IsNullOrEmpty(VST))
                {
                    whereClause.Append(" AND Status = :vStatus");
                    command.Parameters.Add(":vStatus", OracleDbType.Varchar2).Value = VST;
                }
                if (whereClause.Length > 0)
                {
                    command.CommandText += whereClause.ToString();
                }

                using (OracleDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var vehiclea = new Vehicleinfo()
                        {
                            Vehicle_ID = reader.GetString(reader.GetOrdinal("Vehicle_ID")),
                            Vehicle_Type = reader.GetString(reader.GetOrdinal("Vehicle_Type")),
                            Status = reader.GetString(reader.GetOrdinal("Status"))
                        };
                        Vehiclem.Add(vehiclea);
                    }
                    _connection.Close();
                    return Ok(Vehiclem);
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

    [HttpGet("api/vehiclesUseStatistics")]
    public IActionResult UseHandleEndpoint([FromQuery] string? VID)
    {
        List<Vehicleuseinfo> uses = new List<Vehicleuseinfo>();
        try
        {
            _connection.Open();
            // 执行 SQL 查询
            using (var command = _connection.CreateCommand())
            {
                command.Connection = _connection;
                //先写SQL语句的主体
                command.CommandText = "SELECT * FROM VEHICLE_USE WHERE VEHICLE_ID = :vID";
                command.Parameters.Add(":vID", OracleDbType.Varchar2).Value = VID;

                using (OracleDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Vehicleuseinfo ause = new Vehicleuseinfo
                        {
                            VehicleID = reader.GetString(reader.GetOrdinal("VEHICLE_ID")),
                            StationID = reader.GetString(reader.GetOrdinal("STATION_ID")),
                            borrowtime = reader.GetDateTime(reader.GetOrdinal("BORROW_TIME")),
                            returntime = reader.IsDBNull(reader.GetOrdinal("RETURN_TIME")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("RETURN_TIME"))
                        };
                        uses.Add(ause);
                    }

                    _connection.Close();
                    return Ok(uses);
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

    [HttpPost("api/addVehicle")]
    public IActionResult AddHandleEndpoint([FromQuery] string? vehicleID, [FromQuery] string? vehicleType)
    {
        try
        {
            _connection.Open();
            string insertQuery = "INSERT INTO VEHICLE (Vehicle_ID, Vehicle_Type, Status) " +
                        "VALUES (:vehicleID, :vehicleType, '0')";

            if (string.IsNullOrEmpty(vehicleID) || string.IsNullOrEmpty(vehicleType))
                return Ok("新增车辆失败！信息不全！");

            if (!Regex.IsMatch(vehicleID, @"^\d+$"))
                return Ok("无效的警车编号！");

            if (vehicleID.Length < 6)
                return Ok("警车编号过短！请完善编号！");

            // 查询警车编号是否存在
            using (var command1 = _connection.CreateCommand())
            {
                command1.Connection = _connection;
                command1.CommandText = "SELECT * FROM VEHICLE WHERE Vehicle_ID = :vehicleID";
                command1.Parameters.Add(":vehicleID", OracleDbType.Varchar2).Value = vehicleID;
                using (OracleDataReader reader = command1.ExecuteReader())
                    if (reader.HasRows)
                        return Ok("已存在该警车编号！请重新选定编号！");
            }

            using (OracleCommand command = new OracleCommand(insertQuery, _connection))
            {
                command.Connection = _connection;
                command.Parameters.Add(":vehicleID", OracleDbType.Varchar2).Value = vehicleID;
                command.Parameters.Add(":vehicleType", OracleDbType.Varchar2).Value = vehicleType;

                command.ExecuteNonQuery();
                // 插入成功，返回 JSON 数据
                var response = new { data = "成功" };
                return Ok(JsonSerializer.Serialize(response));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"插入数据时发生错误:{ex.Message}");
            return StatusCode(499, $"插入数据时发生错误: {ex.Message}");
        }
        finally
        {
            _connection.Close();
        }
    }

    [HttpDelete("api/delVehicle")]
    public IActionResult DeleteHandleEndpoint([FromQuery] string? vehicleID)
    {
        try
        {
            _connection.Open();

            // 检查车辆ID是否规范
            if (string.IsNullOrEmpty(vehicleID) || vehicleID.Length < 6 || !Regex.IsMatch(vehicleID, @"^\d+$"))
                return Ok("车辆编号无效！请重新输入！");

            // 查询该车辆ID对应的行是否存在
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM VEHICLE WHERE Vehicle_ID = :vehicleID";
                command.Parameters.Add(":vehicleID", OracleDbType.Varchar2).Value = vehicleID;

                using (OracleDataReader reader = command.ExecuteReader())
                {
                    if (!reader.HasRows)
                        return Ok("未找到该编号的车辆！");
                }
            }

            // 执行删除相关记录的操作
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM VEHICLE_USE WHERE Vehicle_ID = :vehicleID";
                command.Parameters.Add(":vehicleID", OracleDbType.Varchar2).Value = vehicleID;
                command.ExecuteNonQuery();
            }

            // 执行删除车辆操作
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM VEHICLE WHERE Vehicle_ID = :vehicleID";
                command.Parameters.Add(":vehicleID", OracleDbType.Varchar2).Value = vehicleID;
                command.ExecuteNonQuery();
            }

            // 返回成功的JSON响应
            var response = new { data = "成功" };
            return Ok(JsonSerializer.Serialize(response));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"删除数据时发生错误: {ex.Message}");
            return StatusCode(499, $"删除数据时发生错误: {ex.Message}");
        }
        finally
        {
            _connection.Close();
        }
    }


    [HttpPost("api/addVehicleUseRecord")]
    public IActionResult AddVehicleUseRecord([FromQuery] string? vehicleID, [FromQuery] string? stationID)
    {
        try
        {
            _connection.Open();
            OracleTransaction transaction = _connection.BeginTransaction(); // 开始数据库事务

            string insertQuery = "INSERT INTO VEHICLE_USE (VEHICLE_ID, STATION_ID, BORROW_TIME, RETURN_TIME) " +
                                 "VALUES (:vehicleID, :stationID, :bTime, :rTime)";

            if (string.IsNullOrEmpty(vehicleID) || string.IsNullOrEmpty(stationID))
                return Ok("新增记录失败！信息不全！");

            if (!Regex.IsMatch(vehicleID, @"^\d+$"))
                return Ok("无效的警车编号！");

            if (!Regex.IsMatch(stationID, @"^\d+$"))
                return Ok("无效的警局编号！");

            if (vehicleID.Length < 6)
                return Ok("警车编号过短！请完善编号！");
            if (stationID.Length < 9)
                return Ok("警局编号过短！请完善编号！");

            // 查询警局编号是否存在
            using (var command1 = _connection.CreateCommand())
            {
                command1.Connection = _connection;
                command1.CommandText = "SELECT * FROM POLICE_STATION WHERE STATION_ID = :stationID";
                command1.Parameters.Add(":stationID", OracleDbType.Varchar2).Value = stationID;
                using (OracleDataReader reader = command1.ExecuteReader())
                    if (!reader.HasRows)
                        return Ok("不存在该警局！请重新输入！");
            }

            // 查询警车编号是否存在且状态为可用
            bool isVehicleInUse;
            using (var command3 = _connection.CreateCommand())
            {
                command3.Connection = _connection;
                command3.CommandText = "SELECT * FROM VEHICLE WHERE Vehicle_ID = :vehicleID FOR UPDATE";
                command3.Parameters.Add(":vehicleID", OracleDbType.Varchar2).Value = vehicleID;
                using (OracleDataReader reader = command3.ExecuteReader())
                {
                    if (!reader.HasRows)
                        return Ok("不存在该警车编号！");
                    reader.Read();
                    int status = Convert.ToInt32(reader["status"]);
                    if (status == 1)
                    {
                        isVehicleInUse = true;
                    }
                    else
                    {
                        isVehicleInUse = false;
                        // 更新警车状态为使用中
                        using (var command4 = _connection.CreateCommand())
                        {
                            command4.Connection = _connection;
                            command4.CommandText = "UPDATE VEHICLE SET status = 1 WHERE Vehicle_ID = :vehicleID";
                            command4.Parameters.Add(":vehicleID", OracleDbType.Varchar2).Value = vehicleID;
                            command4.ExecuteNonQuery();
                        }
                    }
                }
            }

            if (isVehicleInUse)
            {
                transaction.Rollback(); // 车辆正在使用中，回滚事务
                return Ok("该车辆使用中！");
            }

            // 获取当前时间
            DateTime borrowTime = DateTime.Now;

            using (OracleCommand command = new OracleCommand(insertQuery, _connection))
            {
                command.Connection = _connection;
                command.Parameters.Add(":vehicleID", OracleDbType.Varchar2).Value = vehicleID;
                command.Parameters.Add(":stationID", OracleDbType.Varchar2).Value = stationID;
                command.Parameters.Add(":bTime", OracleDbType.Date).Value = borrowTime;
                command.Parameters.Add(":rTime", OracleDbType.Date).Value = DBNull.Value;

                // 执行插入操作
                command.ExecuteNonQuery();

                // 提交事务
                transaction.Commit();

                // 插入成功，返回 JSON 数据
                var response = new { data = "成功" };
                return Ok(JsonSerializer.Serialize(response));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"插入数据时发生错误:{ex.Message}");
            return StatusCode(499, $"插入数据时发生错误: {ex.Message}");
        }
        finally
        {
            _connection.Close();
        }
    }

    [HttpPut("api/returnVehicle")]
    public IActionResult returnVehicle([FromQuery] string? vehicleID, [FromQuery] string? stationID)
    {
        try
        {
            _connection.Open();
            OracleTransaction transaction = _connection.BeginTransaction(); // 开始数据库事务

            string deleteQuery = "DELETE FROM VEHICLE_USE WHERE Vehicle_ID = :vehicleID AND STATION_ID = :stationID AND RETURN_TIME IS NULL";
            string insertQuery = "INSERT INTO VEHICLE_USE (Vehicle_ID, STATION_ID, BORROW_TIME, RETURN_TIME) VALUES (:vehicleID, :stationID, :bTime, :rTime)";
            
            if (string.IsNullOrEmpty(vehicleID) || string.IsNullOrEmpty(stationID))
                return Ok("归还失败！信息不全！");

            if (!Regex.IsMatch(vehicleID, @"^\d+$"))
                return Ok("无效的警车编号！");

            if (!Regex.IsMatch(stationID, @"^\d+$"))
                return Ok("无效的警局编号！");

            if (vehicleID.Length < 6)
                return Ok("警车编号过短！请完善编号！");
            if (stationID.Length < 9)
                return Ok("警局编号过短！请完善编号！");

            // 查询是否有目标警局
            using (var command1 = _connection.CreateCommand())
            {
                command1.Connection = _connection;
                command1.CommandText = "SELECT * FROM POLICE_STATION WHERE STATION_ID = :stationID";
                command1.Parameters.Add(":stationID", OracleDbType.Varchar2).Value = stationID;
                using (OracleDataReader reader = command1.ExecuteReader())
                {
                    if (!reader.HasRows)
                        return Ok("没有该编号的警局！");
                    reader.Read();
                }
            }

            // 查询是否有目标警车和是否真的借出
            using (var command1 = _connection.CreateCommand())
            {
                command1.Connection = _connection;
                command1.CommandText = "SELECT * FROM VEHICLE WHERE VEHICLE_ID = :vehicleID";
                command1.Parameters.Add(":vehicleID", OracleDbType.Varchar2).Value = vehicleID;
                using (OracleDataReader reader = command1.ExecuteReader())
                {
                    if (!reader.HasRows)
                        return Ok("没有该编号的车辆！");
                    reader.Read();

                    string? status = Convert.ToString(reader["STATUS"]);
                    if (status == "0")
                        return Ok("该车辆空闲中！");
                }
            }

            // 查询警车编号是否存在且状态为可用
            using (var command1 = _connection.CreateCommand())
            {
                command1.Connection = _connection;
                command1.CommandText = "SELECT * FROM VEHICLE_USE WHERE VEHICLE_ID = :vehicleID AND STATION_ID = :stationID AND RETURN_TIME IS NULL FOR UPDATE";
                command1.Parameters.Add(":vehicleID", OracleDbType.Varchar2).Value = vehicleID;
                command1.Parameters.Add(":stationID", OracleDbType.Varchar2).Value = stationID;
                using (OracleDataReader reader = command1.ExecuteReader())
                {
                    if (!reader.HasRows)
                        return Ok("没有该警局正在使用该车辆的记录！");
                    
                    // 更新警车状态为使用中
                    using (var command2 = _connection.CreateCommand())
                    {
                        command2.Connection = _connection;
                        command2.CommandText = "UPDATE VEHICLE SET status = 0 WHERE Vehicle_ID = :vehicleID";
                        command2.Parameters.Add(":vehicleID", OracleDbType.Varchar2).Value = vehicleID;
                        command2.ExecuteNonQuery();
                    }
                }
            }

            // 获取当前时间
            DateTime returnTime = DateTime.Now;
            // 查询借用时间
            DateTime borrowTime;
            using (var command1 = _connection.CreateCommand())
            {
                command1.Connection = _connection;
                command1.CommandText = "SELECT BORROW_TIME FROM VEHICLE_USE WHERE Vehicle_ID = :vehicleID AND STATION_ID = :stationID AND RETURN_TIME IS NULL";
                command1.Parameters.Add(":vehicleID", OracleDbType.Varchar2).Value = vehicleID;
                command1.Parameters.Add(":stationID", OracleDbType.Varchar2).Value = stationID;

                object borrowTimeObj = command1.ExecuteScalar();
                if (borrowTimeObj != null && borrowTimeObj != DBNull.Value)
                {
                    borrowTime = Convert.ToDateTime(borrowTimeObj);
                }
                else
                {
                    return Ok("无法获取借用时间！");
                }
            }

            // 执行删除操作
            using (var deleteCommand = _connection.CreateCommand())
            {
                deleteCommand.Connection = _connection;
                deleteCommand.CommandText = deleteQuery;
                deleteCommand.Parameters.Add(":vehicleID", OracleDbType.Varchar2).Value = vehicleID;
                deleteCommand.Parameters.Add(":stationID", OracleDbType.Varchar2).Value = stationID;
                deleteCommand.ExecuteNonQuery();
            }

            // 执行插入操作
            using (var insertCommand = _connection.CreateCommand())
            {
                insertCommand.Connection = _connection;
                insertCommand.CommandText = insertQuery;
                insertCommand.Parameters.Add(":vehicleID", OracleDbType.Varchar2).Value = vehicleID;
                insertCommand.Parameters.Add(":stationID", OracleDbType.Varchar2).Value = stationID;
                insertCommand.Parameters.Add(":bTime", OracleDbType.Date).Value = borrowTime;
                insertCommand.Parameters.Add(":rTime", OracleDbType.Date).Value = returnTime;
                insertCommand.ExecuteNonQuery();
            }

            // 提交事务
            transaction.Commit();

            // 插入成功，返回 JSON 数据
            var response = new { data = "成功" };
            return Ok(JsonSerializer.Serialize(response));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"插入数据时发生错误:{ex.Message}");
            return StatusCode(499, $"插入数据时发生错误: {ex.Message}");
        }
        finally
        {
            _connection.Close();
        }
    }

}
