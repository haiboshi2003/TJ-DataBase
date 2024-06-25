namespace web.DTO_group2
{
    // all   作用: testController 测试请求
    public class Request
    {
        public string input { get; set; } = "";
    }

    // fhl  作用：被修改人权限的警号
    public class notification
    {
        public string temp { get; set; } = "";
    }

    //hyh fhl  作用:POLICEMEN表的相关属性
    public class Police
    {
        public string police_number { get; set; } = "";
        public string police_name { get; set; } = "";
        public string ID_number { get; set; } = "";
        public string phone_number { get; set; } = "";
        public string gender { get; set; } = "";
        public string position { get; set; } = "";
    }
    //hyh    作用: LoginController里用于接收前端填写的 1.登录账户 2. 登录密码
    public class LoginRequest
    {
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
    }
    //hyh   作用：PasswordChangeController里用于接收前端填写的 1.警号  2.身份证号码ID 
    public class PasswordChangeRequest
    {
        public string PoliceNumber { get; set; } = "";
        public string IdNumber { get; set; } = "";
    }
    //fhl    作用: 用于修改权限 1.被修改警员的警号 2.修改人警号  3.原权限等级 4.修改权限 5.修改原因
    public class permit
    {
        
        public string h_number { get; set; } = "";
        public string s_number { get; set; } = "";
        public string F_level { get; set; } = "";
        public string L_level { get; set; } = "";
        public string status { get; set; } = "待处理";
        public string reason { get; set; } = "";
    }

    public class phoneNum   //用于修改密码时传输验证码
    {
        public string phone_number { get; set; } = "";

    }
    public class changedPassword  // 用于 修改密码 的各种信息
    {
        public string police_number { get; set; } = "";
        public string phone_number { get; set; } = "";
        public string verificationCode { get; set; } = "";
        public string newPassword { get; set; } = "";
    }

    public class searchByName
    {
        public string name { get; set; } = "";
        public string gender { get; set; } = "";
    }

    public class searchByID
    {
        public string ID { get; set; } = "";
    }

    public class salaryInfo
    {
        public string police_number_receive { get; set; } = "";
        public string basic_amount { get; set; } = "";
        public string reward_amount { get; set; } = "";
        public string description { get; set; } = "";
    }

    //罗浩鸣编写使用
    //citiyCrimeStatisticControllerLHM.cs使用本类
    //返回前端城市犯罪信息
    public class CityCrimeInfo
    {
        public string DistrictName { get; set; }
        public int Population { get; set; }
        public int CrimeNum { get; set; }
        public List<int> CrimeTypeStatistic { get; set; }
        public Dictionary<string, int> DistrictCrimeTimeStatistic { get; set; }
    }

    public class inputEmployInfo
    {
        public string selectPolicemenID { get; set; }
        public string selectStationID { get; set; }

        public int salary { get; set; }
    }
    public class outEmployInfo
    {
        public string EmployTime { get; set; }
        public string failReason { get; set; } = "success";
    }

}
