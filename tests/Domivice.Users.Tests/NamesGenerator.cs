using System;

namespace Domivice.Users.Tests;

public static class NamesGenerator
{
    private static readonly string[] FirstNames =
    {
        "Aaron", "Abner", "Abraham", "Ace", "Adam", "Adrian", "Aesop", "Alan", "Alastair", "Alex", "Alexander", "Alfie",
        "Alfred", "Alvaro", "Anatole", "Ander", "Andrew", "Angus", "Anthony", "Arnold", "Arsenio", "Asher", "August",
        "Axel", "Aziz", "Baldwin", "Barak", "Barry", "Bartholomew", "Basil", "Basir", "Baxter", "Beau", "Benjamin",
        "Benson", "Bilbo", "Blair", "Blaise", "Boris", "Boyd", "Brant", "Brady", "Brandon", "Brian", "Brice", "Brigham",
        "Brogan", "Bruno", "Buster", "Cade", "Caleb", "Calvin", "Carl", "Charles", "Carmine", "Carson", "Cash",
        "Casimir", "Cecil", "Cedric", "Chad", "Chance", "Chase", "Chester", "Chet", "Christopher", "Clarence", "Clark",
        "Claude", "Clayton", "Cliffton", "Cody", "Colby", "Conan", "Connor", "Crispin", "Curtis", "Cyrus", "Dale",
        "Damien", "Daniel", "Darius", "David", "Dawson", "Declan", "Derby", "Dexter", "Dilbert", "Dmitriy", "Dominic",
        "Donald", "Donovan", "Douglas", "Dwayne", "Dwight", "Earl", "Edgar", "Edward", "Edwin", "Egbert", "Elijah",
        "Elliott", "Elmer", "Emilio", "Eric", "Ernest", "Ethan", "Eugene", "Evan", "Everett", "Fabio", "Fahim",
        "Felipe", "Felix", "Fernando", "Finn", "Fletcher", "Flynn", "Forrest", "Forbes", "Francis", "Frank", "Fraser",
        "Freeman", "Gabriel", "Gage", "Galen", "Garrett", "Gaston", "Gavin", "George", "Gerald", "Gideon", "Gilbert",
        "Glen", "Grady", "Graham", "Grant", "Gray", "Gregory", "Griffin", "Guido", "Hadrian", "Hall", "Hamish",
        "Hamlet", "Harold", "Harry", "Harvey", "Heath", "Hector", "Helmut", "Henry", "Herbert", "Herman", "Hershel",
        "Hilaire", "Hiram", "Homer", "Horatio", "Howie", "Hugo", "Hunter", "Ian", "Ichabod", "Ignacio", "Igor",
        "Indigo", "Ingvar", "Inigo", "Irwin", "Irving", "Isaac", "Isaiah", "Italo", "Jack", "Jacob", "Jaden", "Jafar",
        "Jake", "Jamal", "Jamison", "Jarod", "Jason", "Jasper", "Jeremiah", "Jett", "Jesse", "Joachim", "John", "Jonah",
        "Jonas", "Joseph", "Jove", "Jubal", "Judd", "Julio", "Justin", "Kato", "Keanu", "Keaton", "Keegan", "Keith",
        "Kenneth", "Kermit", "Kevin", "Khalil", "Killian", "Kipling", "Klaus", "Knox", "Kwame", "Kyle", "Lachlan",
        "Lambert", "Lane", "Lars", "Laurence", "Lawson", "Leif", "Lennox", "Leo", "Leroy", "Lester", "Liam", "Lincoln",
        "Lloyd", "Logan", "Lorne", "Louis", "Lucius", "Luke", "Luther", "Maddox", "Magnus", "Malcolm", "Marcus", "Mark",
        "Mario", "Marquis", "Marshall", "Martin", "Mason", "Matthew", "Maurice", "Maxim", "Melvin", "Merrick",
        "Michael", "Mickey", "Miles", "Millard", "Ming", "Mitchel", "Mithra", "Mortimer", "Moses", "Myron", "Nathan",
        "Nazario", "Neelam", "Nemo", "Neriah", "Nestor", "Neville", "Nicholas", "Noam", "Nolam", "Noor", "Norman",
        "Norris", "Octavio", "Olaf", "Oliver", "Omar", "Orestes", "Othello", "Owen", "Ozzy", "Pablo", "Palmer", "Paris",
        "Parker", "Patrick", "Paul", "Pavel", "Perry", "Peter", "Phillip", "Phoenix", "Pierce", "Placido", "Plato",
        "Porfirio", "Porter", "Pradeep", "Presley", "Quincy", "Quinlan", "Quinn", "Quinton", "Ragnar", "Rahim", "Ralph",
        "Ramses", "Randall", "Raoul", "Raphael", "Raymond", "Regis", "Reginald", "Remy", "Renato", "Rhett", "Rhys",
        "Richard", "Rio", "Robert", "Rocky", "Roger", "Roland", "Ross", "Royce", "Rufus", "Ryan", "Ryder", "Salman",
        "Salvador", "Samir", "Sawyer", "Sherwin", "Silas", "Simon", "Sky", "Solomon", "Spyridon", "Stanley", "Steven",
        "Stewart", "Sultan", "Sunny", "Suzuki", "Sven", "Sylvester", "Talbot", "Taliesin", "Tancred", "Tanner",
        "Tasgall", "Thaddeus", "Theodore", "Thomas", "Thor", "Timothy", "Tito", "Tobias", "Trey", "Tristan", "Tycho",
        "Tyler", "Tyrone", "Ulric", "Ulysses", "Umberto", "Urban", "Vance", "Vasek", "Vasiliy", "Vaughn", "Vern",
        "Viggo", "Vijay", "Victor", "Vincent", "Virgil", "Vishnu", "Vitus", "Waldo", "Walter", "Warwick", "Webster",
        "Weston", "Whitlock", "Wiley", "Willard", "William", "Willis", "Winslow", "Wyman", "Wynn", "Xander", "Xavier",
        "Xenon", "Xerxes", "Yale", "Yasir", "Yorick", "Yosef", "Yule", "Yuri", "Yves", "Zachary", "Zane", "Zebulon",
        "Zion"
    };

    private static readonly string[] LastNames =
    {
        "Adams", "Allen", "Anderson", "Atkins", "Baker", "Barnes", "Bell", "Bennett", "Berry", "Bishop", "Black",
        "Brown", "Burns", "Campbell", "Carter", "Clark", "Collins", "Cook", "Cooke", "Cooper", "Cox", "Davidson",
        "Davies", "Davis", "Dawson", "Edwards", "Evans", "Fleming", "Forester", "Foster", "Fox", "Gardener", "Gibb",
        "Gibbs", "Gray", "Green", "Hall", "Hamilton", "Harris", "Henderson", "Hill", "Holmes", "Hughes", "Hunt",
        "Jackson", "Johnson", "Jones", "Kelly", "King", "Lee", "Lewis", "Marshall", "Matthews", "McDonald", "Miller",
        "Mitchell", "Moore", "Morris", "Morrison", "Murphy", "Murray", "Nicholson", "Osborne", "Parker", "Paters",
        "Patterson", "Peterson", "Phillips", "Philips", "Porter", "Powell", "Reid", "Reed", "Richards", "Richardson",
        "Roberts", "Robinson", "Rogers", "Russell", "Sanders", "Scott", "Shaw", "Simpson", "Smith", "Smythe", "Snyder",
        "Stevens", "Stephens", "Stewart", "Summers", "Taylor", "Thomas", "Thompson", "Turner", "Walker", "Wallace",
        "Ward", "Warren", "Webster", "White", "Williams", "Wilson", "Wood", "Wright", "Young"
    };

    private static readonly Random Random = new();

    public static string FirstName()
    {
        return FirstNames[Random.Next(FirstNames.Length)];
    }

    public static string LastName()
    {
        return LastNames[Random.Next(LastNames.Length)];
    }
}