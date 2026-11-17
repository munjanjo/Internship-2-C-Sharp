
int choice;
int nextId = 4;
string name, lastName;
DateTime dateOfBirth;

Dictionary<int, (string name, string lastName, DateTime dateOfBirth)> members =
    new Dictionary<int, (string, string, DateTime)>
    {
        { 1, ("Ivo", "Munitic", new DateTime(2003, 9, 18)) },
        { 2, ("Marko", "Maric", new DateTime(2010, 3, 12)) },
        { 3, ("Ante", "Anic", new DateTime(2000, 7, 25)) }
    };

do
{
    printMain();
    int.TryParse(Console.ReadLine(), out choice);

    switch (choice)
    {
        case 0:
            System.Console.WriteLine("Izlaz iz aplikacije!");
            break;
        case 1:
            memberMenu();
            break;
        case 2:
            break;
        default:
            System.Console.WriteLine("nepoznat izbor! pokusaj ponovo");
            break;
    }

} while (choice != 0);

void printMain()
{
    System.Console.WriteLine("Glavni izbornik(1,2,0):");
    System.Console.WriteLine("1) Clanovi");
    System.Console.WriteLine("2) Posudbe");
    System.Console.WriteLine("0) Exit");
    System.Console.Write("Unesi:");
}

void printMembers()
{
    System.Console.WriteLine("Clanovi izbornik(1,2,3,4,0):");
    System.Console.WriteLine("1) Unos novog clana");
    System.Console.WriteLine("2) Brisanje clana");
    System.Console.WriteLine("3) Uredivanje clana");
    System.Console.WriteLine("4) Pregled svih clanova");
    System.Console.WriteLine("0) Povratak na glavni izbornik");
    System.Console.Write("Unesi:");
}

(string, string, DateTime) inputNameAndLastNameAndDateOfBirth()
{
    System.Console.Write("Unesi ime: ");
    name = Console.ReadLine().Trim();

    while (string.IsNullOrWhiteSpace(name))
    {
        System.Console.Write("Unesi ime: ");
        name = Console.ReadLine().Trim();
    }

    System.Console.Write("Unesi prezime: ");
    lastName = Console.ReadLine().Trim();

    while (string.IsNullOrWhiteSpace(lastName))
    {
        System.Console.Write("Unesi prezime: ");
        lastName = Console.ReadLine().Trim();
    }
    System.Console.Write("Unesi datum rodenja (dd.MM.yyyy.): ");
    while (!DateTime.TryParse(Console.ReadLine(), out dateOfBirth))
    {
        System.Console.Write("Krivo! Unesi datum rodenja (dd.MM.yyyy.): ");
    }



    return (name, lastName, dateOfBirth);
}

void memberMenu()
{
    int choice1;

    do
    {
        printMembers();
        int.TryParse(Console.ReadLine(), out choice1);

        switch (choice1)
        {
            case 1:
                var (name, lastName, dateOfBirth) = inputNameAndLastNameAndDateOfBirth();

                members[nextId] = (name, lastName, dateOfBirth);

                System.Console.WriteLine($"korisnik {name} {lastName} id:{nextId} uspjesno dodan!");

                name = string.Empty;
                lastName = string.Empty;
                nextId++;
                break;

            case 2:
                int choice2;

                do
                {
                    System.Console.WriteLine("Odaberite kako zelite brisati(1,2,0):");
                    System.Console.WriteLine("1) Po ID-ju");
                    System.Console.WriteLine("2) Po imenu i prezimenu");
                    System.Console.WriteLine("0) Exit");
                    System.Console.Write("Unesi:");

                    int.TryParse(Console.ReadLine(), out choice2);

                    switch (choice2)
                    {
                        case 1:
                            System.Console.Write("upisi id:");
                            int.TryParse(Console.ReadLine(), out int id);

                            if (members.ContainsKey(id))
                            {
                                members.Remove(id);
                                System.Console.WriteLine($"Uspjesno obrisan korisnik s id:{id}");
                            }
                            else
                            {
                                System.Console.WriteLine("Ne postoji osoba s tim ID-jem!");
                            }
                            break;

                        case 2:
                            break;

                        case 0:
                            System.Console.WriteLine("Korak unazad!");
                            break;

                        default:
                            System.Console.WriteLine("nepoznat izbor! pokusaj ponovo");
                            break;
                    }

                } while (choice2 != 0);
                break;

            case 3:
                System.Console.Write("Unesi id cije podatke zelis mjenjati: ");
                int.TryParse(Console.ReadLine(), out int updateId);

                if (members.ContainsKey(updateId))
                {
                    var (UpdateName, UpdateLastName, UpdatedDateOfBirth) = inputNameAndLastNameAndDateOfBirth();

                    members[updateId] = (UpdateName, UpdateLastName, UpdatedDateOfBirth);

                    System.Console.WriteLine($"Korisnik {UpdateName} {UpdateLastName} uspjesno uredjen!");
                }
                else
                {
                    System.Console.WriteLine("Ne postoji clan s tim ID-jem!");
                }
                break;

            case 4:
                int choice3;
                System.Console.WriteLine("Kako zelis sortirano(1,2,3,0):");
                do
                {
                    System.Console.WriteLine("1) Abecedno");
                    System.Console.WriteLine("2) Mladji od 18 godina");
                    System.Console.WriteLine("3) Svi sa nevracenom knjigom");
                    System.Console.WriteLine("0) Korak nazad");
                    int.TryParse(Console.ReadLine(), out choice3);
                    switch (choice3)
                    {
                        case 1:
                            var membersSorted = members.OrderBy(m => m.Value.lastName);
                            foreach (var m in membersSorted)
                            {
                                System.Console.WriteLine($"{m.Key} {m.Value.name} {m.Value.lastName} - broj posudbi: ");

                            }

                            break;
                        case 2:
                            var membersUnderAge = members.Where(m => (m.Value.dateOfBirth.AddYears(18) > DateTime.Now));
                            foreach (var m in membersUnderAge)
                            {
                                System.Console.WriteLine($"{m.Key} {m.Value.name} {m.Value.lastName} - broj posudbi: ");
                            }

                            break;
                        case 3:
                            System.Console.WriteLine("treba dodati funkcionalnost");

                            break;
                        case 0:
                            System.Console.WriteLine("Korak unazad!");
                            break;
                        default:
                            System.Console.WriteLine("nepoznat izbor! pokusaj ponovo");
                            break;
                    }
                } while (choice3 != 0);
                break;

            case 0:
                System.Console.WriteLine("Korak unazad!");
                break;

            default:
                System.Console.WriteLine("nepoznat izbor! pokusaj ponovo");
                break;
        }

    } while (choice1 != 0);
}
