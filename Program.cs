using AlgoritmoHuffman.Model;
using System.Text;

namespace AlgoritmoHuffman
{
    class Program
    {
        static void Main(string[] args)
        {
            var text = ReadInput(args);
            var cardList = CharCounter(text);
            var cardTree = ComputeBinaryListTree(cardList);
            var dictionary = GetDictionary(cardTree);
            var textCompressed = ConvertTextByDictionary(text, dictionary);
            PrintTextAsBytes(text);
            Console.WriteLine($"Texto Comprimido: " + textCompressed);

            //PrintDictionary(dictionary);
            //var charsDictionary = GetDictionary(cardListTree);
            //var cardTree = cardListTree.FirstOrDefault();
            //PrintCardTree(cardTree);
            //PrintTextAsBytes(text);

        }

        public static string ConvertTextByDictionary(string text, Dictionary<char, string> dictionary)
        {
            StringBuilder sb = new StringBuilder();

            foreach (char c in text)
            {
                if (dictionary.ContainsKey(c))
                {
                    sb.Append(dictionary[c]);
                }
                else
                {
                    sb.Append(c);
                }
            }

            return sb.ToString();
        }

        public static Dictionary<char,string> GetDictionary(Card cardTree)
        {
            var dictionary = new Dictionary<char, string>();
            var auxList = new List<Card>();

            var cardList = cardTree.node;

            while (cardList?.Count > 0) 
            {
                foreach (var card in cardList)
                {
                    if (card.character == null && card?.node.Count > 0)
                    {
                        auxList.Add(card.node[0]);
                        auxList.Add(card.node[1]);
                    } else
                    {
                        dictionary.Add((char)card.character, card.binary);
                        Console.WriteLine($"Char: {card.character} | Count: {card.count} | Binary: {card.binary}");
                    }
                }
                cardList = auxList.ToList();
                auxList.Clear();
            }

            return dictionary;
        }

        public static Card ComputeBinaryListTree(List<Card> cardList)
        {
            var newNode = new List<Card>();

            while (cardList.Count > 1)
            {
                newNode = cardList.Take(2).ToList();

                for (int i = 0; i <= 1; i++)
                {
                    newNode[i].binary = $"{i}" + newNode[i].binary;

                    var nodeAux = newNode[i].node;
                    var listAux = new List<Card>();

                    while (nodeAux?.Count > 0)
                    {
                        foreach (var item in nodeAux)
                        {
                            item.binary = $"{i}" + item.binary;
                            if (item.node != null)
                            {
                                listAux.Add(item.node[0]);
                                listAux.Add(item.node[1]);
                            }
                        }
                        nodeAux = listAux.ToList();
                        listAux.Clear();
                        int x = 1;
                    }
                }

                var newNodeCard = new Card()
                {
                    count = newNode[0].count + newNode[1].count,
                    character = null,
                    node = newNode.ToList()
                };

                cardList.RemoveRange(0, 2);
                cardList.Add(newNodeCard);
                cardList = cardList.OrderBy(c => c.count).ToList();

                newNode.Clear();
            }

            return cardList.FirstOrDefault();
        }

        public static void PrintEachCharAsBytes(string text)
        {
            int iterator = 0;

            byte[] bytesASCII = Encoding.ASCII.GetBytes(text);

            foreach (var item in bytesASCII)
            {
                var c = Convert.ToString(item, 2);
                Console.WriteLine(text[iterator] + " - " + c);
                iterator++;
            }
        }

        public static void PrintTextAsBytes(string text)
        {
            StringBuilder sb = new StringBuilder();
            byte[] bytesASCII = Encoding.ASCII.GetBytes(text);

            foreach (var item in bytesASCII)
            {
                var c = Convert.ToString(item, 2);
                sb.Append(c);
            }
            Console.WriteLine($"Texto Original em Bytes: {sb}");
        }

        public static List<Card> CharCounter(string text)
        {
            var cardList = new List<Card>();

            foreach (var character in text)
            {
                if (cardList.Where(c => c.character == character).Any())
                {
                    cardList.Where(c => c.character == character).FirstOrDefault().count++;
                }
                else
                {
                    cardList.Add(new Card { character = character, count = 1, node = null });
                }
            }

            cardList = cardList.OrderBy(c => c.count).ToList();
            return cardList;
        }

        public static string ReadInput(string[]? args)
        {
            if (args.Length < 1)
            {
                Console.Write("Type the text: ");
                return Console.ReadLine();
            }
            else
            {
                return args[0];
            }
        }

        public static void PrintCardsList(List<Card> cardList)
        {
            foreach (var card in cardList)
            {
                Console.WriteLine($"character = {card.character} | count = {card.count}");
            }

        }

        public static void PrintSpaces(int i)
        {
            while (i > 0)
            {
                Console.Write(" ");
                i--;
            }
        }

        public static void PrintDictionary(Dictionary<char, string> dictionary)
        {
            foreach (var item in dictionary)
            {
                Console.WriteLine($"Char: {item.Key} | Binary: {item.Value}");
            }
        }

        public static void PrintCardTreeOld(Card card)
        {
            var nodeList = card.node;
            var nodeChild = new List<Card>();
            float aux = 32;
            int iterator = 1;

            PrintSpaces((int)aux);
            Console.Write($"Node {card.count}");

            while (nodeList.Any())
            {
                Console.WriteLine();
                PrintSpaces((int)aux -(iterator * 8));
                foreach (var cardNode in nodeList)
                {
                    if (cardNode.character == null)
                    {
                        Console.Write($"Node {cardNode.count}");
                        PrintSpaces((int)aux * 2 / (2* iterator));
                    }

                    if (cardNode.node == null)
                    {
                        Console.Write($"'{cardNode.character}' {cardNode.count}");
                        PrintSpaces((int)aux * 2 / (2*iterator));
                        continue;
                    }

                    if (cardNode.node.Any())
                    {
                        foreach (var cardChild in cardNode.node)
                        {
                            nodeChild.Add(cardChild);
                        }
                    }
                }
                nodeList = nodeChild;
                nodeChild = new List<Card>();
                iterator++;
            }
            
        }
    }

}