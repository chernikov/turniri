using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateData
{
    public class Team
    {
        public static string[] AdjectiveArr = { "Aero", "Amazon", "Amber", "Apricot", "Aqua",
"Arsenic", "Aureolin", "Azure", "Bazaar", "Beige", "Bisque",
"Black", "Blond", "Blue", "Blush", "Bone", "Brass",
"Bronze", "Buff", "Cadet", "Camel", "Capri", "Cardinal",
"Carnelian", "Carmine", "Ceil", "Celadon", "Cerise", "Cerulean",
"Cherry", "Charm", "Chrome", "Cinnamon", "Coconut", "Citron",
"Denim", "Drab", "Dirt", "Ebony", "Eggplant", "Emerald",
"Fallow", "Fawn", "Firebrick", "Flame", "Flattery", "Flax",
"Folly", "Fulvous", "Gamboge", "Ginger", "Glaucous", "Gray",
"Golden", "Grape", "Harlequin", "Heliotrope", "Honeydew", "Iris",
"Ivory", "Jade", "Jasmine", "Jasper", "Jet", "Jonquil",
"Kobe", "Kobi", "Lava", "Lavender", "Lemon", "Licorice",
"Lilac", "Lime", "Limerick", "Lion", "Liver", "Lumber",
"Lust", "Magenta", "Magnolia", "Mahogany", "Maize", "Malachite",
"Manatee", "Mantis", "Mauve", "Mauvelous", "Melon", "Midori",
"Mint", "Moccasin", "Mulberry", "Mustard", "Navy", "Nyanza",
"Ochre", "Olive", "Onyx", "Olivine", "Orange", "Orchid",
"Patriarch", "Peach", "Pear", "Pearl", "Peridot", "Periwinkle",
"Persimmon", "Peru", "Phlox", "Pink", "Pistachio", "Platinum",
"Plum ", "Prune", "Puce", "Pumpkin", "Quartz", "Rackley",
"Rajah", "Raspberry", "Red", "Redwood", "Regalia", "Rhythm",
"Rose", "Rosewood", "Ruber", "Ruby", "Ruddy", "Rufous",
"Russet", "Rust", "Saffron", "Salmon", "Sand", "Sangria",
"Sapphire", "Scarlet", "Seashell", "Sepia", "Shadow", "Shampoo",
"Sienna", "Silver", "Sinopia", "Skobeloff", "Smalt", "Smitten",
"Smoke", "Snow", "Soap", "Stizza", "Stormcloud", "Straw",
"Strawberry", "Sunglow", "Sunset", "Tan", "Tangelo", "Tangerine",
"Taupe", "Teal", "Telemagenta", "Thistle", "Timberwolf", "Tomato",
"Toolbox", "Topaz", "Tulip", "Turquoise", "Tuscan", "Tuscany",
"Ube", "Ultramarine", "Umber", "Urobilin", "Vanilla", "Verdigris",
"Veronica", "Violet", "Viridian", "Waterspout", "Wenge", "Wheat",
"White", "Wine", "Wisteria", "Xanadu", "Yellow", "Zaffre" };


        public static string[] SubjectiveArr = { "Aardvark", "Albatross", "Alligator", "Alpaca", "Buffalo", 
"Ant", "Anteater", "Antelope", "Ape", "Armadillo", 
"Donkey", "Baboon", "Badger", "Barracuda", "Bat", 
"Bear", "Beaver", "Bee", "Bison", "Boar", 
"Buffalo", "Butterfly", "Camel", "Caribou", "Cat", 
"Caterpillar", "Cattle", "Chamois", "Cheetah", "Chicken", 
"Chimpanzee", "Chinchilla", "Chough", "Clam", "Cobra", 
"Cockroach", "Cod", "Cormorant", "Coyote", "Crab", 
"Crane", "Crocodile", "Crow", "Curlew", "Deer", 
"Dinosaur", "Dog", "Dogfish", "Dolphin", "Donkey", 
"Dotterel", "Dove", "Dragon", "Dragonfly", "Duck", 
"Dugong", "Dunlin", "Eagle", "Echidna", "Eel", 
"Eland", "Elephant", "Elk", "Emu", "Falcon", 
"Ferret", "Finch", "Fish", "Flamingo", "Fly", 
"Fox", "Frog", "Gaur", "Gazelle", "Gerbil", 
"Panda", "Giraffe", "Gnat", "Gnu", "Goat", 
"Goose", "Goldfinch", "Goldfish", "Gopher", "Gorilla", 
"Goshawk", "Grasshopper", "Grouse", "Guanaco", "Guillemot", 
"Gull", "Hamster", "Hare", "Hawk", "Hedgehog", 
"Heron", "Herring", "Hippopotamus", "Hornet", "Horse", 
"Human", "Hummingbird", "Hyena", "Iguana", "Jackal", 
"Jaguar", "Jay", "Jellyfish", "Kangaroo", "Koala", 
"Kouprey", "Kudu", "Lapwing", "Lark", "Lemur", 
"Leopard", "Lion", "Llama", "Lobster", "Locust", 
"Loris", "Louse", "Lyrebird", "Magpie", "Mallard", 
"Manatee", "Marten", "Meerkat", "Mink", "Mole", 
"Monkey", "Moose", "Mouse", "Mosquito", "Mule", 
"Narwhal", "Newt", "Nightingale", "Octopus", "Okapi", 
"Opossum", "Oryx", "Ostrich", "Otter", "Owl", 
"Ox", "Oyster", "Panther", "Parrot", "Partridge", 
"Peafowl", "Pelican", "Penguin", "Pheasant", "Pig", 
"Pigeon", "Platypus", "Polecat", "Pony", "Porcupine", 
"Porpoise", "Quail", "Quelea", "Rabbit", "Raccoon", 
"Rail", "Ram", "Rat", "Rattlesnake", "Raven", 
"Reindeer", "Rhinoceros", "Rook", "Ruff", "Salamander", 
"Salmon", "Sandpiper", "Sardine", "Scorpion", "Seahorse", 
"Seal", "Seastar", "Serval", "Shark", "Sheep", 
"Shrew", "Skunk", "Snail", "Snake", "Spider", 
"Squid", "Squirrel", "Starling", "Stingray", "Stinkbug", 
"Stork", "Swallow", "Swan", "Tapir", "Tarsier", 
"Termite", "Tiger", "Toad", "Trout", "Turkey", 
"Turtle", "Vicuna", "Viper", "Vulture", "Wallaby", 
"Walrus", "Weasel", "Whale", "Wolf", "Wolverine", 
"Wombat", "Woodcock", "Woodpecker", "Worm", "Wren", 
"Yak", "Zebra" };

        private static Random rand = new Random((int)DateTime.Now.Ticks);


        public static string GetRandom()
        {
            var index = rand.Next(AdjectiveArr.Count()); 
            var ajective = AdjectiveArr[index];

            index = rand.Next(SubjectiveArr.Count());

            var subjective = SubjectiveArr[index];

            return string.Format("{0} {1}", ajective, subjective);
        }

        public static string GetRandomAdjective()
        {
            var index = rand.Next(AdjectiveArr.Count());
            var ajective = AdjectiveArr[index];
            return ajective;
        }
    }
}
