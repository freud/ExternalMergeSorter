using System.Text;

string[] words = {"Abiu", "Açaí", "Acerola", "Akebi", "Ackee", "African Cherry Orange", "American Mayapple", "Apple", "Apricot", "Araza", "Avocado", "Banana", "Bilberry", "Blackberry", "Blackcurrant", "Black sapote", "Blueberry", "Boysenberry", "Breadfruit", "Buddha's hand (fingered citron)", "Cactus pear", "Canistel", "Cashew", "Cempedak", "Cherimoya (Custard Apple)", "Cherry", "Chico fruit", "Cloudberry", "Coco De Mer", "Coconut", "Crab apple", "Cranberry", "Currant", "Damson", "Date", "Dragonfruit (or Pitaya)", "Durian", "Egg Fruit", "Elderberry", "Feijoa", "Fig", "Finger Lime (or Caviar Lime)", "Goji berry", "Gooseberry", "Grape", "Raisin", "Grapefruit", "Grewia asiatica (phalsa or falsa)", "Guava", "Hala Fruit", "Honeyberry", "Huckleberry", "Jabuticaba (Plinia)", "Jackfruit", "Jambul", "Japanese plum", "Jostaberry", "Jujube", "Juniper berry", "Kaffir Lime", "Kiwano (horned melon)", "Kiwifruit", "Kumquat", "Lemon", "Lime", "Loganberry", "Longan", "Loquat", "Lulo", "Lychee", "Magellan Barberry", "Mamey Apple", "Mamey Sapote", "Mango", "Mangosteen", "Marionberry", "Melon", "Cantaloupe", "Galia melon", "Honeydew", "Mouse melon", "Musk melon", "Watermelon", "Galia melon", "Honeydew", "Mouse melon", "Musk melon", "Miracle fruit", "Momordica fruit", "Monstera deliciosa", "Mulberry", "Nance", "Nectarine", "Orange", "Blood orange", "Clementine", "Mandarine", "Tangerine", "Papaya", "Passionfruit", "Pawpaw", "Peach", "Pear", "Persimmon", "Plantain", "Plum", "Prune (dried plum)", "Pineapple", "Pineberry", "Plumcot (or Pluot)", "Pomegranate", "Pomelo", "Purple mangosteen", "Quince", "Raspberry", "Salmonberry", "Salmonberry", "Rambutan (or Mamin Chino)", "Redcurrant", "Rose apple", "Salal berry", "Salak", "Sapodilla", "Sapote", "Satsuma", "Shine Muscat or Vitis Vinifera", "Sloe or Hawthorn Berry", "Soursop", "Star apple", "Star fruit", "Strawberry", "Surinam cherry", "Tamarillo", "Tamarind", "Tangelo", "Tayberry", "Ugli fruit", "White currant", "White sapote", "Ximenia", "Yuzu"};

const string WorkingDir = @"d:\external_sort\";
const int KB = 1024;
const int MB = KB * KB;
const int BufferSize = 100 * KB;
const int DesiredFileSize = 2000 * MB;

var random = new Random();
long size = 0;
var outputPath = Path.Combine(WorkingDir, "test.txt");
var encoding = new UTF8Encoding(false);

using (var sw = new StreamWriter(outputPath, false, encoding, BufferSize))
{
    while (size < DesiredFileSize)
    {
        var number = random.Next(1, 100);
        var text = words[random.Next(0, words.Length)];
        var line = $"{number}. {text}";
        size += encoding.GetByteCount(line) + Environment.NewLine.Length;
        await sw.WriteLineAsync(line);
    }
}

Console.WriteLine($"Written {size} bytes");
Console.WriteLine($"File size {new FileInfo(outputPath).Length}");