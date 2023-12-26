namespace Zaabee.Dapper.Extensions.TestProject;

public abstract class UnitTest
{
    protected readonly IConfigurationRoot Config = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .Build();
    protected IDbConnection Conn { get; set; }

    #region sync

    [Fact]
    public void Add()
    {
        var myPoco = CreatePoco();
        var result = Conn.Add(myPoco);

        Assert.Equal(1, result);
    }

    [Fact]
    public void AddRange()
    {
        const int quantity = 10;
        var myPocos = CreatePocos(quantity);
        var result = Conn.AddRange<MyPoco>(myPocos);

        Assert.Equal(quantity, result);
    }

    [Fact]
    public void DeleteById()
    {
        var entity = CreatePoco();
        Conn.Add(entity);
        var result = Conn.DeleteById<MyPoco>(entity.Id);

        Assert.Equal(1, result);
    }

    [Fact]
    public void DeleteByEntity()
    {
        var entity = CreatePoco();
        Conn.Add(entity);
        var result = Conn.DeleteByEntity(entity);

        Assert.Equal(1, result);
    }

    [Fact]
    public void DeleteAllByIds()
    {
        var entities = CreatePocos(10);
        Conn.AddRange<MyPoco>(entities);
        var result = Conn.DeleteByIds<MyPoco>(entities.Select(entity => entity.Id).ToList());

        Assert.Equal(entities.Count, result);
    }

    [Fact]
    public void DeleteAllByEntities()
    {
        var entities = CreatePocos(10);
        Conn.AddRange<MyPoco>(entities);
        var result = Conn.DeleteByEntities<MyPoco>(entities);

        Assert.Equal(entities.Count, result);
    }

    [Fact]
    public void DeleteAll()
    {
        var quantity = Conn.GetAll<MyPoco>().Count;
        var result = Conn.DeleteAll<MyPoco>();

        Assert.Equal(quantity, result);
    }

    [Fact]
    public void Update()
    {
        var entity = CreatePoco();
        Conn.Add(entity);
        entity.Name = "hahahahaha";
        var modifyQuantity = Conn.Update(entity);
        Assert.Equal(1, modifyQuantity);
        //                var result = conn.FirstOrDefault<MyPoco, MySubPoco, MyPoco>(entity.Id, (mypoco, mySubPoco) =>
        //                {
        //                    mypoco.Id = mySubPoco.MyPocoId;
        //                    return mypoco;
        //                });
        var result = Conn.FirstOrDefault<MyPoco>(entity.Id);
        var firstJson = entity.ToJson(
            new JsonSerializerSettings { DateFormatString = "yyyy/MM/dd HH:mm:ss" }
        );
        var secondJson = result.ToJson(
            new JsonSerializerSettings { DateFormatString = "yyyy/MM/dd HH:mm:ss" }
        );
        Assert.Equal(firstJson, secondJson);
    }

    [Fact]
    public void UpdateAll()
    {
        var entities = CreatePocos(10);
        Conn.AddRange<MyPoco>(entities);
        entities.ForEach(entity => entity.Name = "hahahahaha");
        var modifyQuantity = Conn.UpdateAll<MyPoco>(entities);
        Assert.Equal(modifyQuantity, entities.Count);
        var results = Conn.Get<MyPoco>(entities.Select(entity => entity.Id).ToList()).ToList();
        Assert.Equal(
            entities
                .OrderBy(e => e.Id)
                .ToJson(new JsonSerializerSettings { DateFormatString = "yyyy/MM/dd HH:mm:ss" }),
            results
                .OrderBy(r => r.Id)
                .ToJson(new JsonSerializerSettings { DateFormatString = "yyyy/MM/dd HH:mm:ss" })
        );
    }

    [Fact]
    public void FirstOrDefault()
    {
        var entity = CreatePoco();
        Conn.Add(entity);
        var result = Conn.FirstOrDefault<MyPoco>(entity.Id);
        var firstJson = entity.ToJson(
            new JsonSerializerSettings { DateFormatString = "yyyy/MM/dd HH:mm:ss" }
        );
        var secondJson = result.ToJson(
            new JsonSerializerSettings { DateFormatString = "yyyy/MM/dd HH:mm:ss" }
        );
        Assert.Equal(firstJson, secondJson);
    }

    [Fact]
    public void Query()
    {
        var entities = CreatePocos(10);
        Conn.AddRange<MyPoco>(entities);
        var results = Conn.Get<MyPoco>(entities.Select(e => e.Id).ToList());
        Assert.Equal(
            entities
                .OrderBy(e => e.Id)
                .ToJson(new JsonSerializerSettings { DateFormatString = "yyyy/MM/dd HH:mm:ss" }),
            results
                .OrderBy(r => r.Id)
                .ToJson(new JsonSerializerSettings { DateFormatString = "yyyy/MM/dd HH:mm:ss" })
        );
    }

    [Fact]
    public void GetAll()
    {
        var entities = CreatePocos(10);
        Conn.AddRange<MyPoco>(entities);
        var results = Conn.GetAll<MyPoco>();
        Assert.True(results.Count >= entities.Count);
    }

    #endregion

    #region async

    [Fact]
    public async void AddAsync()
    {
        var myPoco = CreatePoco();
        var result = await Conn.AddAsync(myPoco);

        Assert.Equal(1, result);
    }

    [Fact]
    public async void AddRangeAsync()
    {
        const int quantity = 10;
        var myPocos = CreatePocos(quantity);
        var result = await Conn.AddRangeAsync(myPocos);

        Assert.Equal(quantity, result);
    }

    [Fact]
    public async void DeleteByIdAsync()
    {
        var entity = CreatePoco();
        await Conn.AddAsync(entity);
        var result = await Conn.DeleteByIdAsync<MyPoco>(entity.Id);

        Assert.Equal(1, result);
    }

    [Fact]
    public async void DeleteByEntityAsync()
    {
        var entity = CreatePoco();
        await Conn.AddAsync(entity);
        var result = await Conn.DeleteByEntityAsync(entity);

        Assert.Equal(1, result);
    }

    [Fact]
    public async void DeleteAllByIdsAsync()
    {
        var entities = CreatePocos(10);
        await Conn.AddRangeAsync(entities);
        var result = await Conn.DeleteByIdsAsync<MyPoco>(
            entities.Select(entity => entity.Id).ToList()
        );

        Assert.Equal(entities.Count, result);
    }

    [Fact]
    public async void DeleteAllByEntitiesAsync()
    {
        var entities = CreatePocos(10);
        await Conn.AddRangeAsync(entities);
        var result = await Conn.DeleteByEntitiesAsync(entities);

        Assert.Equal(entities.Count, result);
    }

    [Fact]
    public async void DeleteAllAsync()
    {
        var quantity = (await Conn.GetAllAsync<MyPoco>()).Count;
        var result = await Conn.DeleteAllAsync<MyPoco>();

        Assert.Equal(quantity, result);
    }

    [Fact]
    public async void UpdateAsync()
    {
        var entity = CreatePoco();
        await Conn.AddAsync(entity);
        entity.Name = "hahahahaha";
        var modifyQuantity = await Conn.UpdateAsync(entity);
        Assert.Equal(1, modifyQuantity);
        var result = await Conn.FirstOrDefaultAsync<MyPoco>(entity.Id);
        var firstJson = entity.ToJson(
            new JsonSerializerSettings { DateFormatString = "yyyy/MM/dd HH:mm:ss" }
        );
        var secondJson = result.ToJson(
            new JsonSerializerSettings { DateFormatString = "yyyy/MM/dd HH:mm:ss" }
        );
        Assert.Equal(firstJson, secondJson);
    }

    [Fact]
    public async void UpdateAllAsync()
    {
        var entities = CreatePocos(10);
        await Conn.AddRangeAsync(entities);
        entities.ForEach(entity => entity.Name = "hahahahaha");
        var modifyQuantity = await Conn.UpdateAllAsync(entities);
        Assert.Equal(modifyQuantity, entities.Count);
        var results = await Conn.GetAsync<MyPoco>(entities.Select(entity => entity.Id).ToList());
        Assert.Equal(
            entities
                .OrderBy(e => e.Id)
                .ToJson(new JsonSerializerSettings { DateFormatString = "yyyy/MM/dd HH:mm:ss" }),
            results
                .OrderBy(r => r.Id)
                .ToJson(new JsonSerializerSettings { DateFormatString = "yyyy/MM/dd HH:mm:ss" })
        );
    }

    [Fact]
    public async void FirstOrDefaultAsync()
    {
        var entity = CreatePoco();
        await Conn.AddAsync(entity);
        var result = await Conn.FirstOrDefaultAsync<MyPoco>(entity.Id);
        var firstJson = entity.ToJson(
            new JsonSerializerSettings { DateFormatString = "yyyy/MM/dd HH:mm:ss" }
        );
        var secondJson = result.ToJson(
            new JsonSerializerSettings { DateFormatString = "yyyy/MM/dd HH:mm:ss" }
        );
        Assert.Equal(firstJson, secondJson);
    }

    [Fact]
    public async void AllAsync()
    {
        var entities = CreatePocos(10);
        await Conn.AddRangeAsync(entities);
        var results = await Conn.GetAllAsync<MyPoco>();
        Assert.True(results.Count >= entities.Count);
    }

    #endregion

    private static MyPoco CreatePoco(SequentialGuidType? guidType = null)
    {
        var m = new Random().Next();
        var id = guidType is null
            ? Guid.NewGuid()
            : SequentialGuidHelper.GenerateComb(guidType.Value);
        return new MyPoco
        {
            Id = id,
            Name = m % 3 is 0
                ? "apple"
                : m % 2 is 0
                    ? "banana"
                    : "pear",
            Gender = m % 2 is 0 ? Gender.Male : Gender.Female,
            Birthday = DateTime.Now,
            CreateTime = DateTime.UtcNow,
            // Kids = new List<MySubPoco>
            // {
            //     new()
            //     {
            //         Id = guidType is null ? Guid.NewGuid() : SequentialGuidHelper.GenerateComb(guidType.Value),
            //         MyPocoId = id,
            //         Name = m % 3 is 0 ? "apple" : m % 2 is 0 ? "banana" : "pear",
            //         Remark = "This is a sub poco."
            //     }
            // }
        };
    }

    private static List<MyPoco> CreatePocos(int quantity, SequentialGuidType? guidType = null)
    {
        return Enumerable.Range(0, quantity).Select(_ => CreatePoco(guidType)).ToList();
    }
}
