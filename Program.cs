
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
                break;
            case 2:
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
