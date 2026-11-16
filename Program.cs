
int choice;
int nextId = 0, age;
string name, lastName;

Dictionary<int, (string name, string lastName, int age)> members =
    new Dictionary<int, (string, string, int)>();

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

(string, string, int) inputNameAndLastNameAndAge()
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

    System.Console.Write("Unesi godine: ");

    while (!int.TryParse(Console.ReadLine(), out age) || age <= 0)
    {
        System.Console.WriteLine("Krivo uneseno!");
        System.Console.Write("Unesi godine: ");
    }

    return (name, lastName, age);
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
                var (name, lastName, age) = inputNameAndLastNameAndAge();
                members[nextId] = (name, lastName, age);

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
                break;
            case 4:
                break;
            case 0:
                break;
            default:
                System.Console.WriteLine("nepoznat izbor! pokusaj ponovo");
                break;
        }

    } while (choice1 != 0);
}
