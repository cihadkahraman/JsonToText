namespace JsonToText
{
    public class PrintWordsWithOrder
    {
        public static void PrintWordsByLine(List<WordWithCoordinates> words)
        {
            var lines = words
                .GroupBy(w => w.TopLeftY)  // Y koordinatına göre grupla
                .OrderBy(g => g.Key)       // Grupları Y koordinatına göre sırala
                .Select(g => g.OrderBy(w => w.TopLeftX).ToList())  // Her grubu X koordinatına göre sırala
                .ToList();

            foreach (var line in lines)
            {
                foreach (var word in line)
                {
                    Console.Write(word.Description + " ");
                }
                Console.WriteLine();  // Yeni satıra geç
            }
        }
    }
}
