using System.Linq;
using System.Text.Json;

public class DatabaseReader
{
    string dbName;
    public DatabaseReader(string dbName){
        this.dbName = dbName;
    }
    public string GetMeasurements(int page, int pageSize)
    {
        using (var db = new DatabaseContext(dbName))
        {
            var m = db.Measurements
                .OrderByDescending(x => x.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return JsonSerializer.Serialize(m);
        }
    }
}
