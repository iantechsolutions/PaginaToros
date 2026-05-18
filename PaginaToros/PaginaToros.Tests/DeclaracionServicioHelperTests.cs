using PaginaToros.Shared.Helpers;
using PaginaToros.Shared.Models;

namespace PaginaToros.Tests;

public class DeclaracionServicioHelperTests
{
    [Fact]
    public void ResolvePlantelHistorico_UsesClosestHistoricalYearWhenAvailable()
    {
        var planteles = new[]
        {
            new PlantelDTO { Placod = "4RE06", Anioex = "2014", Fecing = "2014/01/01", Id = 1 },
            new PlantelDTO { Placod = "4RE06", Anioex = "2004", Fecing = "2004/01/01", Id = 2 },
            new PlantelDTO { Placod = "4RE06", Anioex = "2024", Fecing = "2024/01/01", Id = 3 },
            new PlantelDTO { Placod = "5RE06", Anioex = "2025", Fecing = "2025/12/31", Id = 4 }
        };

        var resolved = DeclaracionServicioHelper.ResolvePlantelHistorico(planteles, "RE06", new DateTime(2025, 8, 1), out var warning);

        Assert.NotNull(resolved);
        Assert.Equal("4RE06", resolved!.Placod);
        Assert.True(string.IsNullOrWhiteSpace(warning));
    }

    [Fact]
    public void ResolvePlantelHistorico_PrefersNearestYearEvenWhenHistoricPrefixDoesNotMatch()
    {
        var planteles = new[]
        {
            new PlantelDTO { Placod = "6R976", Anioex = "2016", Fecing = "2016/01/01", Id = 1 },
            new PlantelDTO { Placod = "5R976", Anioex = "2020", Fecing = "2020/01/01", Id = 2 },
            new PlantelDTO { Placod = "4R976", Anioex = "2010", Fecing = "2021/01/01", Id = 3 }
        };

        var resolved = DeclaracionServicioHelper.ResolvePlantelHistorico(planteles, "R976", new DateTime(2019, 8, 1), out var warning);

        Assert.NotNull(resolved);
        Assert.Equal("6R976", resolved!.Placod);
        Assert.True(string.IsNullOrWhiteSpace(warning));
    }
}
