namespace System_EMS_1._0.Data
{
    public class ResponseApi
    {
        public int Code { get; set; }
        public string Status { get; set; }
        public string Hint { get; set; }
        public string Message { get; set; }
        //public List<T> Value { get; set; }
        public dynamic Value { get; set; }
    }
    public class ResponLogout
    {
        public int? Code { get; set; }
        public string? Status { get; set; }
        public string? Hint { get; set; }
        public string? Message { get; set; }
        public object? Value { get; set; }

    }

    public class ResponLogin
    {
        public int? Code { get; set; }
        public string? Status { get; set; }
        public string? Token { get; set; }
        public string? Userid { get; set; }
        public string? Username { get; set; }

        public string? Displayname { get; set; }
        public string? Message { get; set; }
        public dynamic? Role { get; set; }

    }
    public class RoleUser
    {
        public string? Rolename { get; set; }
        public string? Description { get; set; }
    }




    public class DepartmentModel
    {
        public string? Departmentid { get; set; }
        public string? Departmentname { get; set; }
        public string? Departmentdescription { get; set; }
        public string? Departmentsortname { get; set; }
        public string? Datecreate { get; set; }
        public string? Createdby { get; set; }
        public string? Users { get; set; }
    }

    public class ResponSaveDepartment
    {
        public int Code { get; set; }
        public string Status { get; set; }
        public string Hint { get; set; }
        public string Message { get; set; }
        public DepartmentModel Value { get; set; }
    }

    public class GroupModel
    {
        public string Groupid { get; set; }
        public string Groupname { get; set; }
        public string Groupdescription { get; set; }
        public string Datecreate { get; set; }
        public string Createdby { get; set; }

    }

    public class UserModel
    {
        public string Userid { get; set; }
        public string Username { get; set; }
        public bool Status { get; set; }
        public string Displayname { get; set; }
        public string Phonenumber { get; set; }
        public string Email { get; set; }
        public string Departmentid { get; set; }
        public string Deparmentname { get; set; }

    }


    public class UsersinGroup
    {
        public string Userid { get; set; }
        public string Displayname { get; set; }
        public string Phonenumber { get; set; }
        public string Email { get; set; }
        public string Timejoin { get; set; }
    }



    public class DeviceModel
    {
        public int Deviceid { get; set; }
        public string Devicename { get; set; }
        public int Devicetype { get; set; }
        public int Productgroup { get; set; }
        public string Startdate { get; set; }
        public string Devicemodel { get; set; }
        public string Serialnumber { get; set; }
        public string Lr_code { get; set; }
        public string Company { get; set; }
        public int National { get; set; }
        public string Info { get; set; }
        public string Comcode { get; set; }
        public int Maindevice { get; set; }
        public string Buydate { get; set; }
        public string Expdate { get; set; }
        public string Note { get; set; }
    }
    public class ResponNation
    {
        public int Nationid { get; set; }
        public string Nationcode { get; set; }
        public string Nationnamevie { get; set; }
        public string Nationnameeng { get; set; }
    }


    public class ResponDeType
    {
        public int Typeid { get; set; }
        public string Typename { get; set; }

        public string Display { get; set; }

    }

    public class ResponProGroup
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public string Code { get; set; }
        public string Display { get; set; }

    }

}
