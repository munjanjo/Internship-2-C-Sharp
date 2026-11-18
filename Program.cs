using System.Globalization;

Console.OutputEncoding = System.Text.Encoding.UTF8;

const int LoanPeriodDays = 21;
const decimal LateFeePerDay = 0.50m;

int choice;
int nextIdMembers = 4;
int nextIdLoans = 7;

Dictionary<int, (string name, string lastName, DateTime dateOfBirth,
    Dictionary<int, (string title, string author, DateTime dateOfLoan, DateTime? dateOfReturn, int? daysLate, decimal? lateFee)> loans)> members =
    new Dictionary<int, (string, string, DateTime, Dictionary<int, (string, string, DateTime, DateTime?, int?, decimal?)>)>
    {
        { 1, ("Ivo", "Munitic", new DateTime(2003, 9, 18), new Dictionary<int, (string, string, DateTime, DateTime?, int?, decimal?)>
            {
                { 1, ("Na Drini ćuprija", "Ivo Andrić", new DateTime(2026, 9, 1), new DateTime(2026, 9, 30), null, null) },
                { 2, ("1984", "George Orwell", new DateTime(2026, 11, 5), null, null, null) }
            })
        },
        { 2, ("Marko", "Maric", new DateTime(2010, 3, 12), new Dictionary<int, (string, string, DateTime, DateTime?, int?, decimal?)>
            {
                { 3, ("Mali princ", "Antoine de Saint-Exupéry", new DateTime(2026, 10, 3), new DateTime(2026, 10, 20), null, null) },
                { 4, ("Stranac", "Albert Camus", new DateTime(2026, 10, 10), null, null, null) }
            })
        },
        { 3, ("Ante", "Anic", new DateTime(2000, 7, 25), new Dictionary<int, (string, string, DateTime, DateTime?, int?, decimal?)>
            {
                { 5, ("Zločin i kazna", "Fjodor Dostojevski", new DateTime(2026, 9, 15), new DateTime(2026, 10, 20), null, null) },
                { 6, ("Proces", "Franz Kafka", new DateTime(2026, 11, 2), new DateTime(2026, 11, 12), null, null) }
            })
        }
    };

UpdateAllLateFees();

do
{
    PrintMainMenu();
    if (!int.TryParse(Console.ReadLine(), out choice))
        choice = -1;

    switch (choice)
    {
        case 0:
            Console.WriteLine("Izlaz iz aplikacije!");
            break;
        case 1:
            RunMembersMenu();
            break;
        case 2:
            RunLoansMenu();
            break;
        default:
            Console.WriteLine("nepoznat izbor! pokusaj ponovo");
            break;
    }

} while (choice != 0);

void PrintMainMenu()
{
    Console.WriteLine();
    Console.WriteLine("Glavni izbornik(1,2,0):");
    Console.WriteLine("1) Clanovi");
    Console.WriteLine("2) Posudbe");
    Console.WriteLine("0) Exit");
    Console.Write("Unesi:");
}

void PrintMembersMenu()
{
    Console.WriteLine();
    Console.WriteLine("Clanovi izbornik(1,2,3,4,0):");
    Console.WriteLine("1) Unos novog clana");
    Console.WriteLine("2) Brisanje clana");
    Console.WriteLine("3) Uredivanje clana");
    Console.WriteLine("4) Pregled svih clanova");
    Console.WriteLine("0) Povratak na glavni izbornik");
    Console.Write("Unesi:");
}

void PrintLoansMenu()
{
    Console.WriteLine();
    Console.WriteLine("Posudbe izbornik(1,2,3,4,5,6,0):");
    Console.WriteLine("1) Unos nove posudbe");
    Console.WriteLine("2) Vracanje knjige");
    Console.WriteLine("3) Brisanje posudbe");
    Console.WriteLine("4) Uredivanje postojece posudbe");
    Console.WriteLine("5) Pregled svih posudbi");
    Console.WriteLine("6) Izvjestaji i analize");
    Console.WriteLine("0) Povratak na glavni izbornik");
    Console.Write("Unesi:");
}

string ReadText(string prompt)
{
    Console.Write(prompt);
    string input = (Console.ReadLine() ?? "").Trim();

    while (string.IsNullOrWhiteSpace(input))
    {
        Console.Write("Unos ne smije biti prazan! " + prompt);
        input = (Console.ReadLine() ?? "").Trim();
    }

    return input;
}

DateTime ReadDate(string prompt)
{
    Console.Write(prompt);
    DateTime date;

    while (!DateTime.TryParseExact(Console.ReadLine(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
    {
        Console.Write("Neispravan format datuma! " + prompt);
    }

    return date;
}

bool IsConfirmed(string question)
{
    Console.Write($"{question} (d/n): ");
    string answer = (Console.ReadLine() ?? "").Trim().ToLower();

    while (answer != "d" && answer != "n")
    {
        Console.Write("Upisi d ili n: ");
        answer = (Console.ReadLine() ?? "").Trim().ToLower();
    }

    return answer == "d";
}

int CalculateAge(DateTime dateOfBirth)
{
    int age = DateTime.Today.Year - dateOfBirth.Year;
    if (dateOfBirth.Date > DateTime.Today.AddYears(-age))
        age--;
    return age;
}

int CalculateDaysLate(DateTime dateOfLoan, DateTime? dateOfReturn)
{
    DateTime dueDate = dateOfLoan.AddDays(LoanPeriodDays);
    DateTime endDate = dateOfReturn ?? DateTime.Today;
    return Math.Max(0, (endDate - dueDate).Days);
}

decimal CalculateLateFee(int daysLate)
{
    return daysLate * LateFeePerDay;
}

void UpdateLateFee(int memberId, int loanId)
{
    var loan = members[memberId].loans[loanId];
    loan.daysLate = CalculateDaysLate(loan.dateOfLoan, loan.dateOfReturn);
    loan.lateFee = CalculateLateFee(loan.daysLate.Value);
    members[memberId].loans[loanId] = loan;
}

void UpdateAllLateFees()
{
    foreach (var memberId in members.Keys)
    {
        foreach (var loanId in members[memberId].loans.Keys.ToList())
        {
            UpdateLateFee(memberId, loanId);
        }
    }
}

bool HasActiveLoans(int id)
{
    return members[id].loans.Values.Any(l => l.dateOfReturn == null);
}

bool IsReturned(int memberId, int loanId)
{
    return members[memberId].loans[loanId].dateOfReturn != null;
}

int FindMemberIdByLoanId(int loanId)
{
    return members.FirstOrDefault(m => m.Value.loans.ContainsKey(loanId)).Key;
}

int FindMemberIdByName(string name, string lastName)
{
    return members.FirstOrDefault(m =>
        m.Value.name.Equals(name, StringComparison.OrdinalIgnoreCase) &&
        m.Value.lastName.Equals(lastName, StringComparison.OrdinalIgnoreCase)).Key;
}

int SelectMember()
{
    string input = ReadText("Odaberite clana (id ili ime i prezime): ");

    if (int.TryParse(input, out int id))
        return members.ContainsKey(id) ? id : 0;

    string[] nameAndLastName = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
    if (nameAndLastName.Length != 2)
        return 0;

    return FindMemberIdByName(nameAndLastName[0], nameAndLastName[1]);
}

void PrintMember(int id)
{
    var m = members[id];
    Console.WriteLine($"[{id}] {m.lastName}, {m.name} ({CalculateAge(m.dateOfBirth)}) - broj posudbi: {m.loans.Count}");
}

void PrintLoan(int loanId, (string title, string author, DateTime dateOfLoan, DateTime? dateOfReturn, int? daysLate, decimal? lateFee) loan)
{
    string returned = loan.dateOfReturn?.ToString("yyyy-MM-dd") ?? "nije vraćeno";

    Console.WriteLine($"[{loanId}] \"{loan.title}\" - {loan.author} | posuđeno: {loan.dateOfLoan:yyyy-MM-dd} | vraćeno: {returned} | kašnjenje: {loan.daysLate} dana | zakasnina: {loan.lateFee:0.00} €");
}

void PrintAllLoans()
{
    foreach (var loan in members.SelectMany(m => m.Value.loans).OrderBy(l => l.Key))
        PrintLoan(loan.Key, loan.Value);
}

void AddLoan()
{
    int memberId = SelectMember();
    if (memberId == 0)
    {
        Console.WriteLine("Clan nije pronaden!");
        return;
    }

    string title = ReadText("Unesite naslov: ");
    string author = ReadText("Unesite autora: ");
    DateTime dateOfLoan = ReadDate("Unesite datum posudbe (YYYY-MM-DD): ");

    while (dateOfLoan > DateTime.Today)
    {
        Console.WriteLine("Datum posudbe ne moze biti u buducnosti!");
        dateOfLoan = ReadDate("Unesite datum posudbe (YYYY-MM-DD): ");
    }

    members[memberId].loans[nextIdLoans] = (title, author, dateOfLoan, null, null, null);
    UpdateLateFee(memberId, nextIdLoans);
    nextIdLoans++;

    Console.WriteLine($"Posudba uspjesno dodana! Rok za vracanje: {dateOfLoan.AddDays(LoanPeriodDays):yyyy-MM-dd}");
}

void ReturnLoan()
{
    Console.Write("Unesite id posudbe: ");
    int.TryParse(Console.ReadLine(), out int loanId);

    int memberId = FindMemberIdByLoanId(loanId);
    if (memberId == 0)
    {
        Console.WriteLine("Ne postoji posudba s tim id-jem!");
        return;
    }

    if (IsReturned(memberId, loanId))
    {
        Console.WriteLine("Ova knjiga je vec vracena!");
        return;
    }

    var loan = members[memberId].loans[loanId];
    DateTime dateOfReturn = ReadDate("Unesite datum vracanja (YYYY-MM-DD): ");

    while (dateOfReturn < loan.dateOfLoan || dateOfReturn > DateTime.Today)
    {
        Console.WriteLine("Datum vracanja ne moze biti prije datuma posudbe ni u buducnosti!");
        dateOfReturn = ReadDate("Unesite datum vracanja (YYYY-MM-DD): ");
    }

    loan.dateOfReturn = dateOfReturn;
    members[memberId].loans[loanId] = loan;
    UpdateLateFee(memberId, loanId);

    var returnedLoan = members[memberId].loans[loanId];
    Console.WriteLine($"Knjiga vracena. Kasnjenje: {returnedLoan.daysLate} dana, zakasnina: {returnedLoan.lateFee:0.00} €");
}

(string, string, DateTime) ReadMemberData()
{
    string name = ReadText("Unesi ime: ");
    string lastName = ReadText("Unesi prezime: ");

    Console.Write("Unesi datum rodenja (dd.MM.yyyy.): ");
    DateTime dateOfBirth;
    while (!DateTime.TryParse(Console.ReadLine(), out dateOfBirth) || dateOfBirth > DateTime.Today)
    {
        Console.Write("Krivo! Datum ne smije biti u buducnosti. Unesi datum rodenja (dd.MM.yyyy.): ");
    }

    return (name, lastName, dateOfBirth);
}

void DeleteMember(int id)
{
    if (HasActiveLoans(id))
    {
        Console.WriteLine("Clan ima nevracenu knjigu i ne moze se obrisati!");
        return;
    }

    var member = members[id];
    if (!IsConfirmed($"Zelite li zaista obrisati clana {member.name} {member.lastName}?"))
    {
        Console.WriteLine("Brisanje otkazano.");
        return;
    }

    members.Remove(id);
    Console.WriteLine($"Uspjesno obrisan korisnik {member.name} {member.lastName}");
}

void RunLoansMenu()
{
    int loanChoice;
    do
    {
        PrintLoansMenu();
        if (!int.TryParse(Console.ReadLine(), out loanChoice))
            loanChoice = -1;

        switch (loanChoice)
        {
            case 1:
                AddLoan();
                break;
            case 2:
                ReturnLoan();
                break;
            case 3:
                DeleteLoan();
                break;
            case 4:
                EditLoan();
                break;
            case 5:
                PrintAllLoans();
                break;
            case 6:
                break;
            case 0:
                break;
            default:
                Console.WriteLine("Nepostojeci odabir! Pokusaj ponovo!");
                break;
        }

    } while (loanChoice != 0);
}
DateTime? ReadReturnDate(DateTime dateOfLoan)
{
    while (true)
    {
        Console.Write("Unesite datum vracanja (YYYY-MM-DD, prazno ako nije vracena): ");
        string input = (Console.ReadLine() ?? "").Trim();

        if (string.IsNullOrWhiteSpace(input))
            return null;

        if (!DateTime.TryParseExact(input, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
            Console.WriteLine("Neispravan format datuma!");
        else if (date < dateOfLoan || date > DateTime.Today)
            Console.WriteLine("Datum vracanja ne moze biti prije datuma posudbe ni u buducnosti!");
        else
            return date;
    }
}

void EditLoan()
{
    Console.Write("Unesite id posudbe: ");
    int.TryParse(Console.ReadLine(), out int loanId);

    int memberId = FindMemberIdByLoanId(loanId);
    if (memberId == 0)
    {
        Console.WriteLine("Ne postoji posudba s tim id-jem!");
        return;
    }

    PrintLoan(loanId, members[memberId].loans[loanId]);

    if (!IsConfirmed("Zelite li zaista urediti ovu posudbu?"))
    {
        Console.WriteLine("Uredivanje otkazano.");
        return;
    }

    string title = ReadText("Unesite naslov: ");
    string author = ReadText("Unesite autora: ");

    DateTime dateOfLoan = ReadDate("Unesite datum posudbe (YYYY-MM-DD): ");
    while (dateOfLoan > DateTime.Today)
    {
        Console.WriteLine("Datum posudbe ne moze biti u buducnosti!");
        dateOfLoan = ReadDate("Unesite datum posudbe (YYYY-MM-DD): ");
    }

    DateTime? dateOfReturn = ReadReturnDate(dateOfLoan);

    members[memberId].loans[loanId] = (title, author, dateOfLoan, dateOfReturn, null, null);
    UpdateLateFee(memberId, loanId);

    Console.WriteLine("Posudba uspjesno uredena!");
    PrintLoan(loanId, members[memberId].loans[loanId]);
}
void DeleteLoan()
{
    int deleteLoanChoice;
    do
    {
        System.Console.WriteLine("Kako zelite brisati?(0,1,2)");
        System.Console.WriteLine("1)Po ID-u");
        System.Console.WriteLine("2)Sve vracene posudbe odabranog clana");
        System.Console.WriteLine("3)Svih posudbi sa zakasninom vecom od unesenog iznosa");
        System.Console.WriteLine("0)Korak unazad");
        int.TryParse(Console.ReadLine(), out deleteLoanChoice);
        switch (deleteLoanChoice)
        {
            case 1:
                Console.Write("Unesi id posudbe: ");
                int.TryParse(Console.ReadLine(), out int loanId);
                int memberId = FindMemberIdByLoanId(loanId);

                if (memberId == 0)
                {
                    Console.WriteLine("Ne postoji posudba s tim ID-jem!");
                    break;
                }

                PrintLoan(loanId, members[memberId].loans[loanId]);

                if (!IsConfirmed("Zelite li zaista obrisati ovu posudbu?"))
                {
                    Console.WriteLine("Brisanje otkazano.");
                    break;
                }

                members[memberId].loans.Remove(loanId);
                Console.WriteLine("Posudba uspjesno obrisana!");
                break;
            case 2:
                int selectedMemberId = SelectMember();
                if (selectedMemberId == 0)
                {
                    Console.WriteLine("Clan nije pronaden!");
                    break;
                }

                var returnedLoanIds = members[selectedMemberId].loans
                    .Where(l => l.Value.dateOfReturn != null)
                    .Select(l => l.Key)
                    .ToList();

                if (returnedLoanIds.Count == 0)
                {
                    Console.WriteLine("Clan nema vracenih posudbi.");
                    break;
                }

                foreach (var id in returnedLoanIds)
                    PrintLoan(id, members[selectedMemberId].loans[id]);

                var member = members[selectedMemberId];
                if (!IsConfirmed($"Zelis li izbrisati {returnedLoanIds.Count} vracenih posudbi od {member.name} {member.lastName}?"))
                {
                    Console.WriteLine("Brisanje otkazano.");
                    break;
                }

                foreach (var id in returnedLoanIds)
                    members[selectedMemberId].loans.Remove(id);

                Console.WriteLine($"Uspjesno obrisano {returnedLoanIds.Count} posudbi!");
                break;
            case 3:
                Console.Write("Unesi iznos zakasnine (npr. 3.50): ");
                decimal minLateFee;
                while (!decimal.TryParse((Console.ReadLine() ?? "").Replace(',', '.'), NumberStyles.Number, CultureInfo.InvariantCulture, out minLateFee) || minLateFee < 0)
                {
                    Console.Write("Neispravan iznos! Unesi pozitivan broj (npr. 3.50): ");
                }

                var loansToDelete = members
                    .SelectMany(m => m.Value.loans
                        .Where(l => l.Value.lateFee > minLateFee)
                        .Select(l => (memberId: m.Key, loanId: l.Key)))
                    .ToList();

                if (loansToDelete.Count == 0)
                {
                    Console.WriteLine($"Nema posudbi sa zakasninom vecom od {minLateFee:0.00} €.");
                    break;
                }

                foreach (var loan in loansToDelete)
                    PrintLoan(loan.loanId, members[loan.memberId].loans[loan.loanId]);

                if (!IsConfirmed($"Zelis li izbrisati {loansToDelete.Count} posudbi sa zakasninom vecom od {minLateFee:0.00} €?"))
                {
                    Console.WriteLine("Brisanje otkazano.");
                    break;
                }

                foreach (var loan in loansToDelete)
                    members[loan.memberId].loans.Remove(loan.loanId);

                Console.WriteLine($"Uspjesno obrisano {loansToDelete.Count} posudbi!");
                break;
            case 0:
                break;
            default:
                System.Console.WriteLine("ne postoji taj izbor! pokusaj ponovo!");
                break;
        }
    } while (deleteLoanChoice != 0);
}

void RunMembersMenu()
{
    int memberChoice;

    do
    {
        PrintMembersMenu();
        if (!int.TryParse(Console.ReadLine(), out memberChoice))
            memberChoice = -1;

        switch (memberChoice)
        {
            case 1:
                var (name, lastName, dateOfBirth) = ReadMemberData();

                members[nextIdMembers] = (name, lastName, dateOfBirth, new Dictionary<int, (string, string, DateTime, DateTime?, int?, decimal?)>());

                Console.WriteLine($"korisnik {name} {lastName} id:{nextIdMembers} uspjesno dodan!");

                nextIdMembers++;
                break;

            case 2:
                int deleteChoice;

                do
                {
                    Console.WriteLine("Odaberite kako zelite brisati(1,2,0):");
                    Console.WriteLine("1) Po ID-ju");
                    Console.WriteLine("2) Po imenu i prezimenu");
                    Console.WriteLine("0) Exit");
                    Console.Write("Unesi:");

                    if (!int.TryParse(Console.ReadLine(), out deleteChoice))
                        deleteChoice = -1;

                    switch (deleteChoice)
                    {
                        case 1:
                            Console.Write("upisi id:");
                            int.TryParse(Console.ReadLine(), out int id);

                            if (members.ContainsKey(id))
                                DeleteMember(id);
                            else
                                Console.WriteLine("Ne postoji osoba s tim ID-jem!");
                            break;

                        case 2:
                            string deleteName = ReadText("Unesi ime: ");
                            string deleteLastName = ReadText("Unesi prezime: ");

                            int memberId = FindMemberIdByName(deleteName, deleteLastName);

                            if (memberId != 0)
                                DeleteMember(memberId);
                            else
                                Console.WriteLine("Korisnik nije pronaden!");
                            break;

                        case 0:
                            Console.WriteLine("Korak unazad!");
                            break;

                        default:
                            Console.WriteLine("nepoznat izbor! pokusaj ponovo");
                            break;
                    }

                } while (deleteChoice != 0);
                break;

            case 3:
                Console.Write("Unesi id cije podatke zelis mjenjati: ");
                int.TryParse(Console.ReadLine(), out int updateId);

                if (!members.ContainsKey(updateId))
                {
                    Console.WriteLine("Ne postoji clan s tim ID-jem!");
                    break;
                }

                PrintMember(updateId);
                if (!IsConfirmed("Zelite li zaista urediti ovog clana?"))
                {
                    Console.WriteLine("Uredivanje otkazano.");
                    break;
                }

                var (updateName, updateLastName, updatedDateOfBirth) = ReadMemberData();
                members[updateId] = (updateName, updateLastName, updatedDateOfBirth, members[updateId].loans);
                Console.WriteLine($"Korisnik {updateName} {updateLastName} uspjesno uredjen!");
                break;

            case 4:
                int printChoice;
                do
                {
                    Console.WriteLine("Kako zelis sortirano(1,2,3,0):");
                    Console.WriteLine("1) Abecedno");
                    Console.WriteLine("2) Mladji od 18 godina");
                    Console.WriteLine("3) Svi sa nevracenom knjigom");
                    Console.WriteLine("0) Korak nazad");
                    Console.Write("Unesi:");

                    if (!int.TryParse(Console.ReadLine(), out printChoice))
                        printChoice = -1;

                    switch (printChoice)
                    {
                        case 1:
                            foreach (var m in members.OrderBy(m => m.Value.lastName).ThenBy(m => m.Value.name))
                                PrintMember(m.Key);
                            break;
                        case 2:
                            foreach (var m in members.Where(m => CalculateAge(m.Value.dateOfBirth) < 18))
                                PrintMember(m.Key);
                            break;
                        case 3:
                            foreach (var m in members.Where(m => HasActiveLoans(m.Key)))
                                PrintMember(m.Key);
                            break;
                        case 0:
                            Console.WriteLine("Korak unazad!");
                            break;
                        default:
                            Console.WriteLine("nepoznat izbor! pokusaj ponovo");
                            break;
                    }
                } while (printChoice != 0);
                break;

            case 0:
                Console.WriteLine("Korak unazad!");
                break;

            default:
                Console.WriteLine("nepoznat izbor! pokusaj ponovo");
                break;
        }

    } while (memberChoice != 0);
}