using PaginaToros.Shared.Helpers;
using PaginaToros.Shared.Models;

namespace PaginaToros.Tests;

public class DeclaracionServicioHelperTests
{
    [Fact]
    public void ResolvePlantelHistorico_UsesDeclarationYearPrefixWhenAvailable()
    {
        var planteles = new[]
        {
            new PlantelDTO { Placod = "8R976", Anioex = "2018", Fecing = "2018/01/01", Id = 1 },
            new PlantelDTO { Placod = "9R976", Anioex = "2019", Fecing = "2019/01/01", Id = 2 },
            new PlantelDTO { Placod = "7R976", Anioex = "2017", Fecing = "2017/01/01", Id = 3 }
        };

        var resolved = DeclaracionServicioHelper.ResolvePlantelHistorico(planteles, "R976", new DateTime(2019, 8, 1), out var warning);

        Assert.NotNull(resolved);
        Assert.Equal("9R976", resolved!.Placod);
        Assert.True(string.IsNullOrWhiteSpace(warning));
    }

    [Fact]
    public void ResolvePlantelHistorico_FallsBackToAnioexAndCreationDateWhenYearPrefixDoesNotMatch()
    {
        var planteles = new[]
        {
            new PlantelDTO { Placod = "6R976", Anioex = "2016", Fecing = "2016/01/01", Id = 1 },
            new PlantelDTO { Placod = "5R976", Anioex = "2020", Fecing = "2020/01/01", Id = 2 },
            new PlantelDTO { Placod = "4R976", Anioex = "2010", Fecing = "2021/01/01", Id = 3 }
        };

        var resolved = DeclaracionServicioHelper.ResolvePlantelHistorico(planteles, "R976", new DateTime(2019, 8, 1), out var warning);

        Assert.NotNull(resolved);
        Assert.Equal("5R976", resolved!.Placod);
        Assert.False(string.IsNullOrWhiteSpace(warning));
    }
}
