using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using web.Helpers;
using web.DTO_group2;
using System.Net;
using System.Text;
using System.Security.Cryptography;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Drawing;

// 自定义数据结构用于保存验证码和创建时间


[Route("/for-password")]
[ApiController]
public class PCComfireController_hyh : ControllerBase
{
    // 数据库连接
    private readonly OracleConnection _connection;

    public PCComfireController_hyh(OracleConnection connection)
    {
        _connection = connection;
    }



    // 用于发送验证码
    class CheckSumBuilder
    {
        // 计算并获取CheckSum
        public static String getCheckSum(String appSecret, String nonce, String curTime)
        {
            byte[] data = Encoding.Default.GetBytes(appSecret + nonce + curTime);
            byte[] result;

            SHA1 sha = new SHA1CryptoServiceProvider();
            // This is one implementation of the abstract class SHA1.
            result = sha.ComputeHash(data);

            return getFormattedText(result);
        }

        // 计算并获取md5值
        public static String getMD5(String requestBody)
        {
            if (requestBody == null)
                return null;

            // Create a new instance of the MD5CryptoServiceProvider object.
            MD5 md5Hasher = MD5.Create();

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(requestBody));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return getFormattedText(Encoding.Default.GetBytes(sBuilder.ToString()));
        }

        private static String getFormattedText(byte[] bytes)
        {
            char[] HEX_DIGITS = { '0', '1', '2', '3', '4', '5',
                    '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f' };
            int len = bytes.Length;
            StringBuilder buf = new StringBuilder(len * 2);
            for (int j = 0; j < len; j++)
            {
                buf.Append(HEX_DIGITS[(bytes[j] >> 4) & 0x0f]);
                buf.Append(HEX_DIGITS[bytes[j] & 0x0f]);
            }
            return buf.ToString();
        }
    }
    // 用于发送验证码
    class HttpClient
    {
        //发起Http请求
        public static string HttpPost(string url, Stream data, IDictionary<object, string> headers = null)
        {
            System.Net.WebRequest request = HttpWebRequest.Create(url);
            request.Method = "POST";
            if (data != null)
                request.ContentLength = data.Length;
            //request.ContentType = "application/x-www-form-urlencoded;charset=utf-8";

            if (headers != null)
            {
                foreach (var v in headers)
                {
                    if (v.Key is HttpRequestHeader)
                        request.Headers[(HttpRequestHeader)v.Key] = v.Value;
                    else
                        request.Headers[v.Key.ToString()] = v.Value;
                }
            }
            HttpWebResponse response = null;
            try
            {
                // Get the response.
                response = (HttpWebResponse)request.GetResponse();
                // Display the status.
                Console.WriteLine(response.StatusDescription);
                // Get the stream containing content returned by the server.
                Stream dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                string responseFromServer = reader.ReadToEnd();


                // Display the content.
                //Console.WriteLine(responseFromServer);
                // Cleanup the streams and the response.
                reader.Close();
                dataStream.Close();
                response.Close();
                return responseFromServer;
            }
            catch (Exception e)
            {
                // Console.WriteLine(e.Message);
                //Console.WriteLine(response.StatusDescription);
                return e.Message;
            }
        }
    }


    //验证码保存机制

    private class VerificationCodeInfo
    {
        public string Code { get; set; }
        public DateTime CreationTime { get; set; }
    }


    private static Dictionary<string, VerificationCodeInfo> verificationCodes = new Dictionary<string, VerificationCodeInfo>();





    [HttpPost("/for-passsword/verification")]
    public IActionResult GetVeriCode(phoneNum PN)
    {
        // 手机号
        string phone_number = PN.phone_number;
         // 从请求的JSON数据中获取输入的字符串
         // 在这里处理接收到的字符串
        String url = "https://api.netease.im/sms/sendcode.action";
        url += $"?templateid=22518746&mobile={phone_number}";//请输入正确的手机号

        //网易云信分配的账号，请替换你在管理后台应用下申请的Appkey
        String appKey = "e4ab5a8dc9255ca0be2d9b883ac38a31";
        //网易云信分配的密钥，请替换你在管理后台应用下申请的appSecret
        String appSecret = "7346e6f6a129";
        //随机数（最大长度128个字符）
        String nonce = "546565464546514";

        TimeSpan ts = DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1);
        Int32 ticks = System.Convert.ToInt32(ts.TotalSeconds);
        //当前UTC时间戳，从1970年1月1日0点0 分0 秒开始到现在的秒数(String)
        String curTime = ticks.ToString();
        //SHA1(AppSecret + Nonce + CurTime),三个参数拼接的字符串，进行SHA1哈希计算，转化成16进制字符(String，小写)
        String checkSum = CheckSumBuilder.getCheckSum(appSecret, nonce, curTime);

        IDictionary<object, String> headers = new Dictionary<object, String>();
        headers["AppKey"] = appKey;
        headers["Nonce"] = nonce;
        headers["CurTime"] = curTime;
        headers["CheckSum"] = checkSum;
        headers["ContentType"] = "application/x-www-form-urlencoded;charset=utf-8";
        //执行Http请求
        string inform = "{obj:2000}";                               // 测试的时候用这个
        //string inform = HttpClient.HttpPost(url, null, headers); 



        // 检查验证码是否发送成功
        JObject json = JObject.Parse(inform);
        string key = "obj";
        string checkCode = "";
        if (json.TryGetValue(key, out JToken value))
        {
            checkCode = value.ToString(); // 将JToken转换为字符串
                
            if (checkCode == "")
                return BadRequest("No Code");
            else
            {
                verificationCodes.Add(phone_number, new VerificationCodeInfo
                {
                    Code = checkCode,
                    CreationTime = DateTime.UtcNow // 使用 UTC 时间以避免时区问题
                });
            }
        }
        else
            return BadRequest("Fail to get code");
        return Ok("验证码发送成功");
    }

    [HttpPost]
    public IActionResult VeriChangePassword(changedPassword cPw)
    {
        string police_number = cPw.police_number;
        string phone_number = cPw.phone_number;
        string userSubmittedCode = cPw.verificationCode;
        string newWord = cPw.newPassword;
            

        if (verificationCodes.TryGetValue(phone_number, out VerificationCodeInfo codeInfo))
        {
            if (userSubmittedCode == codeInfo.Code)
            {
                // 验证码匹配，检查是否在有效期内
                TimeSpan elapsedTime = DateTime.UtcNow - codeInfo.CreationTime;
                if (elapsedTime.TotalMinutes <= 200)             // 验证码有效时间，以分钟为单位，调试的时候可以多一点时间
                {
                    // 验证码在有效期内，进行后续处理
                    try
                    {
                        _connection.Open();
                 
                        // 修改密码
                        string sql = "UPDATE POLICE_ACCOUNT set POLICE_KEY = :newWord WHERE POLICE_NUMBER = :accountID";
                        OracleCommand command = new OracleCommand(sql, _connection);
                        command.Parameters.Add(new OracleParameter("newWord", newWord));
                        command.Parameters.Add(new OracleParameter("accountID", police_number));  //根据警号查询

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            // 密码修改成功，可以继续其他操作
                            return Ok("密码修改成功");
                        }
                        else
                        {
                            return BadRequest("密码修改失败：未找到对应账号");
                        }
                    }
                    catch (Exception ex)
                    {
                        // 处理异常或记录错误日志
                        return BadRequest("密码修改失败：" + ex.Message);
                    }
                    finally
                    {
                        _connection.Close();
                    }
                    
                }
                else
                {
                    // 验证码已过期，处理错误情况
                    return BadRequest("验证码已过期");
                }
            }
            else
            {
                // 验证码不匹配，处理错误情况
                return BadRequest("验证码不匹配");
            }
        }
        else
        {
            // 未找到对应手机号的验证码，处理错误情况
            return BadRequest("请先发送验证码。");
        }

    }
}

