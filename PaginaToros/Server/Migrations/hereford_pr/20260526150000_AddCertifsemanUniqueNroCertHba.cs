using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaginaToros.Server.Migrations.hereford_pr
{
    public partial class AddCertifsemanUniqueNroCertHba : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
DELETE c
FROM CERTIFSEMEN c
INNER JOIN (
    SELECT id
    FROM (
        SELECT
            id,
            ROW_NUMBER() OVER (
                PARTITION BY COALESCE(TRIM(NRO_CERT), ''), COALESCE(TRIM(HBA), '')
                ORDER BY COALESCE(FCH_USU, '1900-01-01') DESC, id DESC
            ) AS rn
        FROM CERTIFSEMEN
    ) ranked
    WHERE ranked.rn > 1
) duplicates ON duplicates.id = c.id;
");

            migrationBuilder.CreateIndex(
                name: "UX_CERTIFSEMEN_NRO_CERT_HBA",
                table: "CERTIFSEMEN",
                columns: new[] { "NRO_CERT", "HBA" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UX_CERTIFSEMEN_NRO_CERT_HBA",
                table: "CERTIFSEMEN");
        }
    }
}
