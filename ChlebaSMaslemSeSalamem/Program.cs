using System.Text;

const string jde  = "Jde ${pecivo} a potká ";
const string takzeJde = "Takže jde ${pecivo} a potká ${pecivo2}. ";
const string povida = "A ${pecivo} povídá: \"";
const string muzuJitSVami = ", můžu jít s vámi?\", ";
const string muzuJitSTebou = "${pecivo}, můžu jít s tebou?\", ";
const string pricemz = "přičemž ${pecivo} odpoví: \"Jo, můžeš.\". ";
const string pricemzKonec = "přičemž ${pecivo} odpoví: \"Ne.\". Čas na smích. ";

Pecivo[] pecivoList =
[
    new Pecivo("chleba", "chlebe", "chleba"),
    new Pecivo("rohlík", "rohlíku", "rohlík"),
    new Pecivo("houska", "housko", "housku"),
    new Pecivo("veka", "veko", "veku"),
    new Pecivo("loupák", "loupáku", "loupák"),
    new Pecivo("dalamánek", "dalamánku", "dalamánek"),
    new Pecivo("mazanec", "mazanče", "mazanec"),
    new Pecivo("vánočka", "vánočko", "vánočku"),
    new Pecivo("pletýnka", "pletýnko", "pletýnku"),
    new Pecivo("toust", "touste", "toust"),
    new Pecivo("rajče", "rajče", "rajče")
];

List<Pecivo> pecivoListFull = [];

foreach (var pecivo in pecivoList)
{
    pecivoListFull.Add(pecivo);
    
    if (pecivo == pecivoList[^1])
    {
        break;
    }
    
    pecivoListFull.Add(new Pecivo(pecivo.Nominativ + " s máslem", pecivo.Vokativ + " s máslem", pecivo.Akuzativ + " s máslem"));
    pecivoListFull.Add(new Pecivo(pecivo.Nominativ + " s máslem se salámem", pecivo.Vokativ + " s máslem se salámem", pecivo.Akuzativ + " s máslem se salámem"));
}


if (File.Exists("output.txt"))
{
    File.Delete("output.txt");
}

using var fs = File.Create("output.txt");

WriteStringToStream(fs, jde.Replace("${pecivo}", pecivoListFull[0].Nominativ) + pecivoListFull[1].Nominativ + ". ");
WriteStringToStream(fs, povida.Replace("${pecivo}", pecivoListFull[1].Nominativ));
WriteStringToStream(fs, muzuJitSTebou.Replace("${pecivo}", pecivoListFull[0].Vokativ));
WriteStringToStream(fs, pricemz.Replace("${pecivo}", pecivoListFull[0].Nominativ));
WriteStringToStream(fs, "\n");

for (int i = 1; i < pecivoListFull.Count - 1; i++)
{
    WriteStringToStream(fs, takzeJde.Replace("${pecivo}", GetThisAndPreviousPecivoNazev(i)).Replace("${pecivo2}", pecivoListFull[i + 1].Akuzativ));
    WriteStringToStream(fs, povida.Replace("${pecivo}", pecivoListFull[i + 1].Nominativ));
    WriteStringToStream(fs, GetThisAndPreviousPecivoOsloveni(i));
    WriteStringToStream(fs, muzuJitSVami);

    if (i == pecivoListFull.Count - 2)
    {
        WriteStringToStream(fs, pricemzKonec.Replace("${pecivo}", GetThisAndPreviousPecivoNazev(i)));
        WriteStringToStream(fs, "\n");
    }
    else
    {
        WriteStringToStream(fs, pricemz.Replace("${pecivo}", GetThisAndPreviousPecivoNazev(i)));
        WriteStringToStream(fs, "\n");
    }
}

return;

string GetThisAndPreviousPecivoNazev(int index)
{
    StringBuilder builder = new();
    for (int i = 0; i <= index; i++)
    {
        builder.Append(pecivoListFull[i].Nominativ);
        if (i < index)
        {
            builder.Append(", ");
        }
    }

    return builder.ToString();
}

string GetThisAndPreviousPecivoOsloveni(int index)
{
    StringBuilder builder = new();
    for (int i = 0; i <= index; i++)
    {
        builder.Append(pecivoListFull[i].Vokativ);
        if (i < index)
        {
            builder.Append(", ");
        }
    }

    return builder.ToString();
}

void WriteStringToStream(Stream fs, string text)
{
    fs.Write(GetStringBytes(text));
}

byte[] GetStringBytes(string text)
{
    return Encoding.UTF8.GetBytes(text);
}

record Pecivo(string Nominativ, string Vokativ, string Akuzativ);