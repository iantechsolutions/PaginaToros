using PaginaToros.Shared.Models;

namespace PaginaToros.Tests;

public class TransferenciaAnimalImpactoHelperTests
{
    [Theory]
    [InlineData("PHPR", "CC", "VA", "Varede")]
    [InlineData("HVIP", "PR", "VQ", "Vqcsrp")]
    [InlineData("PHPR", "SS", "VQ", "Vqssrd")]
    public void ResolveFieldName_ReturnsExpectedField(string tiphac, string hemsta, string tipohem, string expectedField)
    {
        var field = TransferenciaAnimalImpactoHelper.ResolveFieldName(tiphac, hemsta, tipohem, out var error);

        Assert.Equal(expectedField, field);
        Assert.Null(error);
    }

    [Fact]
    public void ResolveFieldName_RejectsInvalidCombination()
    {
        var field = TransferenciaAnimalImpactoHelper.ResolveFieldName("PHPR", "PR", "VA", out var error);

        Assert.Null(field);
        Assert.False(string.IsNullOrWhiteSpace(error));
    }

    [Fact]
    public void TryBuildImpacto_ProducesBeforeAndAfterValues()
    {
        var transan = new TransanDTO
        {
            Tiphac = "PHPR",
            Hemsta = "CC",
            Tipohem = "VA",
            CantHem = 4
        };

        var origen = new PlantelDTO
        {
            Placod = "0001",
            Varede = 10
        };

        var destino = new PlantelDTO
        {
            Placod = "0002",
            Varede = 2
        };

        var ok = TransferenciaAnimalImpactoHelper.TryBuildImpacto(transan, origen, destino, out var impacto, out var error);

        Assert.True(ok);
        Assert.Null(error);
        Assert.NotNull(impacto);
        Assert.Equal("Varede", impacto!.FieldName);
        Assert.Equal(10d, impacto.OrigenAntes);
        Assert.Equal(6d, impacto.OrigenDespues);
        Assert.Equal(2d, impacto.DestinoAntes);
        Assert.Equal(6d, impacto.DestinoDespues);
    }
}
