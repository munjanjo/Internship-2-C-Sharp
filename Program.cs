
int choice;
int nextId = 0;
string name, lastName;
DateTime dateOfBirth;

Dictionary<int, (string name, string lastName, DateTime dateOfBirth)> members =
    new Dictionary<int, (string, string, DateTime)>();

do
{
    printMain();
    int.TryParse(Console.ReadLine(), out choice);

    switch (choice)
    {
        case 0:
            break;
        case 1:
            clanoviMenu();
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

void clanoviMenu()
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
                foreach (var member in members)
                {
                    System.Console.WriteLine(
                        $"ID:{member.Key} Name:{member.Value.name} Lastname:{member.Value.lastName} Date of birth:{member.Value.dateOfBirth.ToString("dd.MM.yyyy.")}");
                }
                break;

            case 0:
                break;

            default:
                System.Console.WriteLine("nepoznat izbor! pokusaj ponovo");
                break;
        }

    } while (choice1 != 0);
}
