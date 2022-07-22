namespace Zaabee.Dapper.Extensions.TestProject.POCOs;

[Table("my_poco")]
public class MyPoco
{
    [Key] [Column("id")] public Guid Id { get; set; }
    [Column("name")] public string Name { get; set; }
    [Column("gender")] public Gender Gender { get; set; }
    [Column("birthday")] public DateTime Birthday { get; set; }
    [Column("create_time")] public DateTime CreateTime { get; set; }
    // [JsonIgnore][Column("kids")] public List<MySubPoco> Kids { get; set; }
}

public enum Gender : byte
{
    Male,
    Female
}