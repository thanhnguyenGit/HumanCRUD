namespace ApiStudyFinal.Models
{
    using BSS;
    public class Human
    {
        public int HumanID { get; set; }

        public string ObjectGuid { get; set; }

        public string? FullName { get; set; }

        public int? Age { get; set; }

        public bool? Sex { get; set; }

        public bool? IsDeleted { get; set; }

        public static string GetOne(Guid ObjectGuid, out Human human)
        {
            human = null;

            Console.WriteLine("LOGIC TRIGGER");
            string msg = BSS.DBM.GetOne("usp_Human_GetOne",
                new { ObjectGuid }, out human);
            if (msg.Length > 0) return msg;

            return "";
        }
        public static string TestConnection()
        {
            Console.WriteLine("TEST DB CONNECTION");
            string msg = BSS.DBM.ConnectionString;
            Console.WriteLine($"connection string: {msg}");

            if (msg.Length > 0) return msg;

            return "";
        }
        public static string GetAll(out List<Human> ltHuman)
        {
            ltHuman = null;
            string msg = BSS.DBM.GetList("usp_Human_GetAll", new { }, out ltHuman);
            if (msg.Length > 0) return msg;
            return "";
        }
        public string InsertOrUpdate(DBM dbm)
        {
            string msg = dbm.SetStoreNameAndParams("usp_Human_InsertOrUpdate",
                    new
                    {
                        HumanID,
                        FullName,
                        Age,
                        Sex
                    });
            if (msg.Length > 0) return msg;
            dbm.ExecStore();

            return "";
        }
        public static string Delete(Guid ObjectGuid)
        {
            string msg = BSS.DBM.ExecStore("usp_Human_Delete", new { ObjectGuid });
            if (msg.Length > 0) return msg;
            return "";
        }

        public override string ToString()
        {
            return $"HumanID: {HumanID},ObjectGuid: {ObjectGuid}, FullName: {FullName}, Age: {Age}, Sex: {Sex},IsDeleted: {IsDeleted}";
        }
    }
}
