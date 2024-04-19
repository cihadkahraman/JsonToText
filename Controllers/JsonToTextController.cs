using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Collections.Generic;
using System.IO;

namespace JsonToText.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JsonToTextController : ControllerBase
    {
        [HttpPost]
        public IActionResult JsonTextConvert([FromBody] List<JsonObject> descriptions)
        {
            if (descriptions == null || !descriptions.Any())
            {
                return BadRequest("Girilen Json değeri geçersiz!!");
            }
            List<WordWithCoordinates> wordswithcoordinates = new List<WordWithCoordinates>();
            int wordTopLeftx = 0;
            int wordTopLefty = 0;
            foreach (var item in descriptions)
            {
                int i = 0;
                foreach (var coordinate in item.BoundingPoly.vertices)
                {
                    if (i == 0)
                    {
                        wordTopLeftx = coordinate.X;
                        wordTopLefty = coordinate.Y;
                    }
                    if (i == 2)
                    {
                        wordswithcoordinates.Add(new WordWithCoordinates
                        {
                            Description = item.Description,
                            TopLeftX = wordTopLeftx,
                            TopLeftY = wordTopLefty,
                        });
                    }

                    i++;

                }
            }
            var resultLines = wordswithcoordinates;
            resultLines.RemoveAt(0);
            var result = PrintWordsByLine(resultLines);

            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string fileName = "JsonToText.txt";
            string filePath = Path.Combine(desktopPath, fileName);

            try
            {
                System.IO.File.AppendAllText(filePath, result + Environment.NewLine);
                Console.WriteLine("Dosyaya başarıyla eklendi.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Dosya ekleme işlemi sırasında hata oluştu: " + ex.Message);
            }

            return Ok(result);
        }
        public static string PrintWordsByLine(List<WordWithCoordinates> words)
        {
            const int tolerance = 10; // İki kelimenin aynı satırda sayılabilmesi için y koordinatları arasındaki fark en fazla "tolerance" değeri kadardır.
            var sortedWords = words.OrderBy(w => w.TopLeftY).ThenBy(w => w.TopLeftX).ToList();
            var lines = new List<List<WordWithCoordinates>>();
            var line = new List<WordWithCoordinates>();
            for (int i = 0; i < words.Count; i++)
            {
                if (i == 0)
                {
                    line.Add(sortedWords[i]);
                }

                if (i == sortedWords.Count - 1)
                {
                    lines.Add(new List<WordWithCoordinates>(line));
                    break;
                }

                if (Math.Abs(sortedWords[i].TopLeftY - sortedWords[i + 1].TopLeftY) <= tolerance) // Kelimeler aynı satırda mı?
                {
                    line.Add(sortedWords[i + 1]);
                }
                else
                {
                    lines.Add(new List<WordWithCoordinates>(line));
                    line.Clear();
                    line.Add(sortedWords[i + 1]);


                }
            }
            StringBuilder sb = new StringBuilder();
            foreach (var a in lines)
            {
                foreach (var word in a)
                {
                    sb.Append(word.Description + " ");
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}
