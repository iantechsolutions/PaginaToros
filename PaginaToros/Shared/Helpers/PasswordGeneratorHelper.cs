using System.Security.Cryptography;

namespace PaginaToros.Shared.Helpers
{
    public static class PasswordGeneratorHelper
    {
        private static readonly string[] Words =
        {
            "PAPEL",
            "CARTON",
            "LORO",
            "NUBE",
            "RIEL",
            "MONTE",
            "SILLA",
            "BOLSA",
            "FARO",
            "TIERRA",
            "LAGO",
            "TRIGO",
            "BRISA",
            "ROCA",
            "SELVA",
            "CASCO",
            "RAMA",
            "MANGO",
            "PARED",
            "CLARO",
            "GLOBO",
            "PUERTA",
            "BOSQUE",
            "PLATA",
            "CIELO",
            "CASA",
            "MESA",
            "PERRO",
            "GATO",
            "SOL",
            "LUNA",
            "PAN",
            "FLOR",
            "MAR",
            "RIO",
            "AUTO",
            "TAZA",
            "VASO",
            "CAMA",
            "LIBRO",
            "PATO",
            "PEZ",
            "SAPO",
            "FOCA",
            "OSO",
            "LOBO",
            "TIGRE",
            "VACA",
            "TORO",
            "CABRA",
            "OVEJA",
            "RATON",
            "PUMA",
            "ZORRO",
            "BURRO",
            "ABEJA",
            "MOSCA",
            "PULGA",
            "HOJA",
            "FUEGO",
            "HUMO",
            "ARENA",
            "PLAYA",
            "VALLE",
            "CAMPO",
            "CERRO",
            "CUEVA",
            "POZO",
            "BARCO",
            "TREN",
            "AVION",
            "RUEDA",
            "MOTOR",
            "BOTE",
            "VELA",
            "REMO",
            "ANCLA",
            "BANCO",
            "CAJA",
            "TAPA",
            "CLAVO",
            "CABLE",
            "LLAVE",
            "PINZA",
            "PALA",
            "PICO",
            "BALDE",
            "BROCHA",
            "CINTA",
            "REGLA",
            "GOMA",
            "TIZA",
            "LAPIZ",
            "PLUMA",
            "CARTA",
            "SELLO",
            "FOTO",
            "RADIO",
            "DISCO",
            "RELOJ",
            "MANTA",
            "TOALLA",
            "JABON",
            "PEINE",
            "ROPA",
            "MEDIA",
            "ZAPATO",
            "BOTA",
            "GORRA",
            "ABRIGO",
            "FRUTA",
            "UVA",
            "PERA",
            "MELON",
            "SANDIA",
            "BANANA",
            "LIMON",
            "COCO",
            "QUESO",
            "LECHE",
            "MIEL",
            "SOPA",
            "CARNE",
            "ARROZ",
            "FIDEO",
            "SAL",
            "AZUCAR",
            "VASO",
            "PLATO",
            "TENEDOR",
            "CUCHARA",
            "CUCHILLO"
        };

        public static string GenerateWordPassword(int wordCount = 3)
        {
            if (wordCount < 2)
            {
                throw new ArgumentOutOfRangeException(nameof(wordCount), "Se requieren al menos 2 palabras.");
            }

            if (wordCount > Words.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(wordCount), "La cantidad de palabras solicitada supera el diccionario disponible.");
            }

            var indexes = new List<int>(wordCount);
            while (indexes.Count < wordCount)
            {
                var index = RandomNumberGenerator.GetInt32(Words.Length);
                if (!indexes.Contains(index))
                {
                    indexes.Add(index);
                }
            }

            return string.Join(".", indexes.Select(index => Words[index]));
        }
    }
}
