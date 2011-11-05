using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.Globalization;

namespace UniTTT.Logik.Player
{
    public class KIPlayer : AbstractPlayer
    {
        public KI.AbstractKI KI { get; private set; }
        public int Breite { get; private set; }
        public int Hoehe { get; private set; }

        public KIPlayer(int kiZahl, int breite, int hoehe, char kispieler) : base(kispieler)
        {
            if (kiZahl == 1)
                KI = new KIReinforcement();
            else if (kiZahl == 2)
                KI = new KIRecursion(breite, hoehe);
            else if (kiZahl == 3)
                KI = new KIMiniMax(breite, hoehe, 'O');
            else if (kiZahl == 4)
                KI = new KILike(breite, hoehe);
            else if (kiZahl == 5)
                KI = new KIRandom(breite, hoehe);
            else if (kiZahl == 6)
                KI = new KIBot(breite, hoehe, 'O');

            Breite = breite;
            Hoehe = hoehe;
        }

        public override Vector Spiele(Brett brett)
        {
            return BrettHelper.ZugToVector(KI.Spielen(brett.VarBrett, Spieler), Breite, Hoehe);
        }

        public void Lerne()
        {
            KI.Lernen();
        }

        public override string ToString()
        {
            return "KIPlayer";
        }

        class KIStack : KI.AbstractKI
        {
            public KIStack(char spieler, int b, int h) : base(spieler, b, h) { }

            public override void Lernen()
            {
                throw new NotImplementedException();
            }

            public override int Spielen(char[,] brett, char spieler)
            {
                throw new NotImplementedException();
            }

            public override string ToString()
            {
                return "Stack";
            }
        }

        class KIMiniMax : KI.AbstractKI
        {
            #region Fields
            private Brett brett;
            #endregion

            public KIMiniMax(int b, int h, char spieler) : base(spieler, b, h)
            {
                brett = new Brett(Breite, Hoehe);
            }

            public override void Lernen()
            {
                throw new NotImplementedException();
            }

            public override int Spielen(char[,] brett, char spieler)
            {
                string mom_sit_code = SitCodeHelper.Berechnen(brett);
                double[] Felder = ZugWertungBerechnen(mom_sit_code, SitCodeHelper.PlayertoSitCode(spieler));
                return BestenZugAuswaehlen(Felder, mom_sit_code);
            }

            private int BestenZugAuswaehlen(double[] Felder, string mom_sit_code)
            {
                int zug = 0;
                double count = double.MinValue + 0.1;
                for (int i = 0; i < FelderAnzahl; i++)
                {
                    if (mom_sit_code[i] == '1')
                    {
                        if (Felder[i] > count)
                        {
                            count = Felder[i];
                            zug = i;
                        }
                    }
                }
                return zug;
            }

            private double[] ZugWertungBerechnen(string mom_sit_code, char spieler)
            {
                string mom_sit_code_edited;
                double[] Felder = new double[FelderAnzahl];
                double[] Feldertmp = new double[FelderAnzahl];
                double[,] wertungen = new double[FelderAnzahl, 3];
                for (int i = 0; i < FelderAnzahl; i++)
                {
                    if (mom_sit_code[i] == '1')
                    {
                        mom_sit_code_edited = mom_sit_code.Remove(i, 1).Insert(i, spieler.ToString());
                        wertungen[i, 0] = Bewertung(mom_sit_code_edited, i, '1'); // Unentschieden
                        wertungen[i, 1] = Bewertung(mom_sit_code_edited, i, kispieler); // KISpieler Gewonnen
                        wertungen[i, 2] = Bewertung(mom_sit_code_edited, i, SpielerTausch(kispieler)); // MenschGegner Gewonnen

                        Felder[i] = (wertungen[i, 0] * 20.0) + (wertungen[i, 0] * 10.0) - (wertungen[i, 2] * 50.0);
                        if (Felder[i] == 0.0)
                        {
                            Feldertmp = ZugWertungBerechnen(mom_sit_code_edited, SpielerTausch(spieler));
                            for (int y = 0; y < FelderAnzahl; y++)
                                Felder[i] += Feldertmp[y];
                        }
                        else
                            return Felder;
                    }
                }
                return Felder;
            }

            private double Bewertung(string sit_code, int x, char spieler)
            {
                double wertung = 0.0;
                Brett.GameStates state = brett.GetGameState(SitCodeHelper.ToBrett(sit_code, Breite, Hoehe), spieler);

                if (state == Brett.GameStates.Gewonnen)
                    wertung = 10.0 / Convert.ToDouble(x, System.Globalization.CultureInfo.InvariantCulture);
                else if (state == Brett.GameStates.Unentschieden)
                    wertung = Convert.ToDouble(x, System.Globalization.CultureInfo.InvariantCulture);
                return wertung;
            }

            public override string ToString()
            {
                return "MiniMax";
            }
        }

        class KIReinforcement : KI.AbstractKI
        {
            public KIReinforcement() : base('O', 3, 3)
            {
                db = new DB("KI_Reinforcement");
            }

            #region Fields
            private DB db;
            #endregion

            private int Rundefrage()
            {
                Console.WriteLine("Wie viele Runden sollen durchlaufen werden? (ein Int wert bitte)");
                return Int32.Parse(Console.ReadLine(), CultureInfo.CurrentCulture);
            }

            // KI
            public override void Lernen()
            {
                #region Fields
                char player = 'X';
                GewinnPrüfer pruefer = new GewinnPrüfer(3);
                int runden = Rundefrage(), zug;
                string momsitcode = SitCodeHelper.Lerrsetzen(9);
                string[,] sit_codes = new string[runden, 9];
                int[,] zuege = new int[runden, 9];
                int[] wertungen = new int[runden];
                bool gewonnen;
                #endregion

                for (int a = 0; a < runden; a++)
                {
                    for (int i = 0; i < 9; i++)
                    {
                        player = SpielerTausch(player);
                        sit_codes[a, i] = momsitcode;
                        zug = GetRandomZug(sit_codes[a, i]);
                        zuege[a, i] = zug;

                        momsitcode = momsitcode.Remove(zug, 1).Insert(zug, player.ToString());

                        gewonnen = pruefer.Pruefe(player, SitCodeHelper.ToBrett(momsitcode, 3, 3));

                        // Wertungen
                        // Aufwerten
                        if ((gewonnen))
                            wertungen[a] = 2;
                        else if ((i == 8) && (!gewonnen))
                            wertungen[a] = 1;

                        // Ist Spiel Zu Ende?
                        if ((gewonnen) || (i == 8))
                        {
                            momsitcode = SitCodeHelper.Lerrsetzen(9);
                            i = 9;
                            player = 'X';
                        }
                    }
                }
                db.Zuege = zuege;
                db.Sit_Code = sit_codes;
                db.Wertung = wertungen;
                db.Speichern();
            }

            public override int Spielen(char[,] brett, char spieler)
            {
                string sitcode = SitCodeHelper.Berechnen(brett);
                int zug = db.Lesen(sitcode);
                if (zug == -1)
                    zug = GetRandomZug(sitcode);
                return zug;
            }

            public override string ToString()
            {
                return "Reinforcement";
            }

            class DB
            {
                private SQLiteConnection conn = new SQLiteConnection();
                private Datenbank.Verbindung verb;
                public int[,] Zuege;
                public string[,] Sit_Code;
                public int[] Wertung;

                public DB(string dbname)
                {
                    verb = new Datenbank.Verbindung(dbname);
                }

                public void Speichern()
                {
                    if (TabelleErstellen() && verb.Herstellen(ref conn))
                    {

                        using (SQLiteTransaction traction = conn.BeginTransaction())
                        {
                            using (SQLiteCommand sql = new SQLiteCommand(conn))
                            {
                                for (int x = 0; x < Wertung.Length; x++)
                                {
                                    runde_ausgabe(x);
                                    for (int i = 0; i < 9; i++)
                                    {
                                        if (!string.IsNullOrEmpty(Sit_Code[x, i]))
                                        {
                                            sql.CommandText = "SELECT COUNT(ID) FROM test WHERE Sit_Code=" + Sit_Code[x, i];
                                            using (SQLiteDataReader reader = sql.ExecuteReader())
                                            {
                                                if (Convert.ToInt32(reader[0], CultureInfo.CurrentCulture) > 0)
                                                {
                                                    reader.Close();
                                                    sql.CommandText = "UPDATE test SET A" + Zuege[x, i] + "=A" + Zuege[x, i] + "+" + Wertung[x] + " WHERE Sit_Code=" + Sit_Code[x, i];
                                                }
                                                else if ((Convert.ToInt32(reader[0], CultureInfo.CurrentCulture) == 0))
                                                {
                                                    reader.Close();
                                                    sql.CommandText = "INSERT INTO test (Sit_Code, A" + Zuege[x, i] + ") VALUES (" + Sit_Code[x, i] + ", " + Wertung[x] + ")";
                                                }
                                                sql.ExecuteNonQuery();
                                            }
                                        }
                                    }
                                }
                            }
                            traction.Commit();
                        }
                        // Verbindung trennen
                        verb.Trennen(ref conn);
                    }
                }

                public int Lesen(string mom_sit_code)
                {
                    #region Fields
                    int zug = -1;
                    int[] x = new int[9];
                    SQLiteDataReader reader;
                    #endregion

                    // Verbindung Herstellen
                    if (verb.Herstellen(ref conn))
                    {
                        using (SQLiteTransaction traction = conn.BeginTransaction())
                        {
                            using (SQLiteCommand sql = new SQLiteCommand(conn))
                            {
                                try
                                {
                                    sql.CommandText = "SELECT COUNT(*) FROM test WHERE Sit_Code=" + mom_sit_code;
                                    reader = sql.ExecuteReader();
                                    if (Convert.ToInt32(reader[0].ToString(), CultureInfo.CurrentCulture) > 0)
                                    {
                                        reader.Close();
                                        sql.CommandText = "SELECT A0, A1, A2, A3, A4, A5, A6, A7, A8 FROM test WHERE Sit_Code=" + mom_sit_code;
                                        reader = sql.ExecuteReader();
                                        int count = 0;
                                        for (int i = 0; i < 9; i++)
                                        {
                                            if ((Convert.ToInt32(reader[i].ToString(), CultureInfo.CurrentCulture) > count) && (mom_sit_code[Convert.ToInt32(reader[i].ToString(), CultureInfo.CurrentCulture)] == '1'))
                                            {
                                                count = Convert.ToInt32(reader[i].ToString(), CultureInfo.CurrentCulture);
                                                zug = i;
                                            }
                                        }
                                    }
                                    reader.Close();
                                    reader.Dispose();
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex);
                                    System.Threading.Thread.Sleep(2500);
                                    System.Environment.Exit(1);
                                }
                            }
                            traction.Commit();
                        }
                        verb.Trennen(ref conn);
                    }
                    return zug;
                }

                public bool TabelleErstellen(string tabname = null)
                {
                    tabname = "test";
                    bool rt_bll = false;
                    if (verb.Herstellen(ref conn))
                    {
                        using (SQLiteTransaction traction = conn.BeginTransaction())
                        {
                            using (SQLiteCommand sql = new SQLiteCommand(conn))
                            {
                                sql.CommandText = string.Format(CultureInfo.InvariantCulture,
                                    "CREATE TABLE IF NOT EXISTS {0} ( ID INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, Sit_Code INT NOT NULL DEFAULT 0, A0 INT DEFAULT 0, A1 INT DEFAULT 0, A2 INT DEFAULT 0, A3 INT DEFAULT 0, A4 INT NOT NULL DEFAULT 0, A5 INT NOT NULL DEFAULT 0, A6 INT NOT NULL DEFAULT 0, A7 INT NOT NULL DEFAULT 0, A8 INT NOT NULL DEFAULT 0);", tabname);
                                sql.ExecuteNonQuery();
                                rt_bll = conn.ResultCode() == 0;
                            }
                            traction.Commit();
                        }
                        verb.Trennen(ref conn);
                    }
                    return rt_bll;
                }

                private void runde_ausgabe(int runde)
                {
                    if ((runde + 1) % 500 == 0)
                        Console.WriteLine(runde + 1);
                }

                public override string ToString()
                {
                    return "DB_Recursion";
                }
            }
        }

        class KIRecursion : KI.Recursive
        {
            #region Fields
            private DB db = new DB("KI_Recursion");
            #endregion

            public KIRecursion(int b, int h) : base(b, h) { }

            // KI
            public override void Lernen()
            {
                Console.WriteLine("Prüfe, ob Tabelle existiert..");
                if (!db.TabelleExistent("Felder_" + FelderAnzahl))
                {
                    Console.WriteLine("Keine Tabelle gefunden, dh. Berechnen Sinvoll.");
                    Console.WriteLine("Erstelle die DatenTabelle..");
                    db.TabelleErstellen("Felder_" + FelderAnzahl); // TODO: Fehlerüberprüfung einbauen
                    Console.WriteLine();
                    Console.WriteLine("Fertig mit dem Erstellen der Tabelle.");
                    System.Threading.Thread.Sleep(2500);
                    Console.Clear();
                    Console.WriteLine("Beginne mit dem Berechnen..");
                    Recursion(FelderAnzahl, SitCodeHelper.Lerrsetzen(FelderAnzahl), '3');
                    Recursion(FelderAnzahl, SitCodeHelper.Lerrsetzen(FelderAnzahl), '2');
                    db.Sit_Code = SitCodes;
                    db.Wertung = Wertungen;
                    Console.WriteLine();
                    Console.WriteLine("Fertig mit dem Berechnen.");
                    System.Threading.Thread.Sleep(3000);
                    Console.Clear();
                    Console.WriteLine("Beginne mit den Abspeichern der Werte..");
                    db.Speichern("Felder_" + FelderAnzahl);
                    System.Threading.Thread.Sleep(2000);
                    Console.Clear();
                    Console.WriteLine("Fertig mit den Abspeichern der Werte.");
                }
                else
                {
                    Console.WriteLine("Tabelle existent, keine Neuschreibung notwendig..");
                }
                Console.WriteLine("Programm wird Beendet. (Taste drücken)");
                Console.ReadKey();
            }

            // TODO: Überarbeiten
            public override int Spielen(char[,] brett, char spieler)
            {
                int[] Felder = new int[FelderAnzahl];
                string mom_sit_code = SitCodeHelper.Berechnen(brett);
                Felder = WertungenBerechnen(mom_sit_code, spieler);

                return BestenZugAuswaehlen(Felder, mom_sit_code);
            }

            private int[] WertungenBerechnen(string mom_sit_code, char spieler)
            {
                int[,] wertungen = new int[FelderAnzahl, 3];
                int[] Felder = new int[FelderAnzahl];
                string mom_sit_code_edited = mom_sit_code;

                for (int i = 0; i < FelderAnzahl; i++)
                {
                    if (mom_sit_code[i] == '1')
                    {
                        mom_sit_code_edited = mom_sit_code.Remove(i, 1).Insert(i, SitCodeHelper.PlayertoSitCode(spieler).ToString());
                        wertungen[i, 0] = db.Lesen(Datenbank.DB.ToDBLike(mom_sit_code_edited, FelderAnzahl), '1', "Felder_" + FelderAnzahl); // Unentschieden
                        wertungen[i, 1] = db.Lesen(Datenbank.DB.ToDBLike(mom_sit_code_edited, FelderAnzahl), SitCodeHelper.PlayertoSitCode(spieler), "Felder_" + FelderAnzahl); // Spieler Gewonnen
                        wertungen[i, 2] = db.Lesen(Datenbank.DB.ToDBLike(mom_sit_code_edited, FelderAnzahl), SpielerTausch(SitCodeHelper.PlayertoSitCode(spieler)), "Felder_" + FelderAnzahl); // Gegner

                        Felder[i] = (wertungen[i, 0] + wertungen[i, 1]) - (wertungen[i, 2] * 5);
                    }
                }
                return Felder;
            }

            public override string ToString()
            {
                return "Recursion";
            }

            class DB
            {
                public List<string> Sit_Code { get; set; }
                public List<int> Wertung { get; set; }
                public Datenbank.Verbindung verb;
                private SQLiteConnection conn = new SQLiteConnection();

                // Konstruktor
                public DB(string dbname)
                {
                    Wertung = new List<int>();
                    Sit_Code = new List<string>();
                    verb = new Datenbank.Verbindung(dbname);
                }

                public void Speichern(string tabname)
                {
                    int count = 0;
                    if (verb.Herstellen(ref conn))
                    {
                        using (SQLiteTransaction traction = conn.BeginTransaction())
                        {
                            using (SQLiteCommand sql = new SQLiteCommand(conn))
                            {
                                for (int x = 0; x < Wertung.Count; x++, count++)
                                {
                                    RundeAusgabe(x);
                                    for (; (count < Sit_Code.Count) && (Sit_Code[count] != "END"); count++)
                                    {
                                        sql.CommandText = string.Format(CultureInfo.InvariantCulture, "Select Count(ID) From {0} Where Sit_Code='{1}' AND Wertung={2}", tabname, Sit_Code[count], Wertung[x]);
                                        using (SQLiteDataReader reader = sql.ExecuteReader())
                                        {
                                            if (int.Parse(reader[0].ToString(), CultureInfo.CurrentCulture) == 0)
                                            {
                                                reader.Close();
                                                sql.CommandText = string.Format(CultureInfo.InvariantCulture, "Insert Into {0} (Sit_Code, Wertung) Values ('{1}', {2})", tabname, Sit_Code[count], Wertung[x]);
                                                sql.ExecuteNonQuery();
                                            }
                                        }
                                    }
                                }
                            }
                            traction.Commit();
                        }
                        // Verbindung trennen
                        verb.Trennen(ref conn);
                    }
                }

                public int Lesen(string mom_sit_code, char bedingung, string tabname)
                {
                    #region Fields
                    int rt_int = 0;
                    SQLiteDataReader reader;
                    #endregion

                    // Verbindung Herstellen
                    if (verb.Herstellen(ref conn))
                    {
                        using (SQLiteTransaction traction = conn.BeginTransaction())
                        {
                            using (SQLiteCommand sql = new SQLiteCommand(conn))
                            {
                                sql.CommandText = string.Format(CultureInfo.InvariantCulture, "SELECT COUNT(*) FROM {0} WHERE Sit_Code LIKE '{1}' AND Wertung={2}", tabname, mom_sit_code, bedingung);
                                reader = sql.ExecuteReader();
                                rt_int = int.Parse(reader[0].ToString(), CultureInfo.CurrentCulture);
                                reader.Close();
                                reader.Dispose();
                            }
                            traction.Commit();
                        }
                        verb.Trennen(ref conn);
                    }
                    return rt_int;
                }

                public bool TabelleExistent(string name)
                {
                    bool rt_bool = false;
                    if (verb.Herstellen(ref conn))
                    {
                        using (SQLiteTransaction traction = conn.BeginTransaction())
                        {
                            using (SQLiteCommand sql = new SQLiteCommand(conn))
                            {
                                sql.CommandText = string.Format(CultureInfo.InvariantCulture, "Select count(*) From 'sqlite_master' where name='{0}'", name);
                                SQLiteDataReader reader = sql.ExecuteReader();
                                rt_bool = int.Parse(reader[0].ToString(), CultureInfo.CurrentCulture) > 0;
                            }
                            traction.Commit();
                        }
                        verb.Trennen(ref conn);
                    }
                    return rt_bool;
                }

                public bool TabelleErstellen(string tabname)
                {
                    bool rt_bll = false;
                    if (verb.Herstellen(ref conn))
                    {
                        using (SQLiteTransaction traction = conn.BeginTransaction())
                        {
                            using (SQLiteCommand sql = new SQLiteCommand(conn))
                            {
                                sql.CommandText = string.Format(CultureInfo.InvariantCulture, "CREATE TABLE IF NOT EXISTS {0} ( ID INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, Sit_Code Varchar(100), Wertung Varchar(10));", tabname);
                                sql.ExecuteNonQuery();
                                rt_bll = conn.ResultCode() == 0;
                            }
                            traction.Commit();
                        }
                        verb.Trennen(ref conn);
                    }
                    return rt_bll;
                }

                private void RundeAusgabe(int runde)
                {
                    if ((runde + 1) % 500 == 0)
                        Console.WriteLine(runde + 1);
                }

                public override string ToString()
                {
                    return "DB_Recursion";
                }
            }
        }
        
        class KILike : KI.Recursive
        {

            public KILike(int b, int h) : base(b, h) 
            {
                Recursion(FelderAnzahl, SitCodeHelper.Lerrsetzen(FelderAnzahl), '3');
                Recursion(FelderAnzahl, SitCodeHelper.Lerrsetzen(FelderAnzahl), '2');
            }

            // KI
            public override void Lernen()
            {
                throw new NotImplementedException();
            }

            // TODO: Überarbeiten
            public override int Spielen(char[,] brett, char spieler)
            {
                string mom_sit_code = SitCodeHelper.Berechnen(brett);
                int[] Felder = new int[FelderAnzahl];

                Felder = WertungenBerechnen(mom_sit_code, spieler);

                return BestenZugAuswaehlen(Felder, mom_sit_code);
            }

            private int WertungenZugZuordnen(List<int> list, int bedingung)
            {
                int rt_int = 0, count = 0;
                for (int i = 0; i < Wertungen.Count; i++, count++)
                {
                    for (; count < (SitCodes.Count) && (SitCodes[count] != "END"); count++)
                    {
                        if (count == list[0])
                        {
                            list.RemoveAt(0);
                            if (Wertungen[i] == bedingung)
                                rt_int++;
                            else
                                break;
                            if (list.Count == 0)
                            {
                                i = Wertungen.Count;
                                break;
                            }
                        }
                    }
                }
                return rt_int;
            }

            private int[] WertungenBerechnen(string mom_sit_code, char spieler)
            {
                int[,] wertungen = new int[FelderAnzahl, 3];
                int[] Felder = new int[FelderAnzahl];
                string mom_sit_code_edited = mom_sit_code;

                for (int i = 0; i < FelderAnzahl; i++)
                {
                    if (mom_sit_code[i] == '1')
                    {
                        mom_sit_code_edited = mom_sit_code.Remove(i, 1).Insert(i, SitCodeHelper.PlayertoSitCode(spieler).ToString());
                        wertungen[i, 0] = WertungenZugZuordnen(Datenbank.DB.Like(SitCodes, Datenbank.DB.ToVBLike(mom_sit_code_edited)), '1' - 48); // unentschieden
                        wertungen[i, 1] = WertungenZugZuordnen(Datenbank.DB.Like(SitCodes, Datenbank.DB.ToVBLike(mom_sit_code_edited)), SitCodeHelper.PlayertoSitCode(spieler) - 48); // Spieler Gewonnen
                        wertungen[i, 2] = WertungenZugZuordnen(Datenbank.DB.Like(SitCodes, Datenbank.DB.ToVBLike(mom_sit_code_edited)), SpielerTausch(SitCodeHelper.PlayertoSitCode(spieler)) - 48); // Gegner

                        Felder[i] = (wertungen[i, 0] + wertungen[i, 1]) - (wertungen[i, 2] * 5);
                    }
                }
                return Felder;
            }

            public override string ToString()
            {
                return "Like";
            }
        }

        class KIBot : KI.AbstractKI
        {
            public KIBot(int breite, int hoehe, char spieler) : base(spieler, breite, hoehe) { }

            public override void Lernen()
            {
                base.Lernen();
            }

            public override int Spielen(char[,] brett, char spieler)
            {
                string sitcode = SitCodeHelper.Berechnen(brett);
                int win_zug = TestOneWin(sitcode);
                int block_zug = TestHumanBlock(sitcode);
                int zug = GetRandomZug(sitcode);

                if (win_zug != -1)
                    return win_zug;
                else if (block_zug != -1)
                    return block_zug;
                else
                    return zug;
            }

            private int TestOneWin(string sitcode)
            {
                string momsitcode;
                int win_zug = -1;
                for (int playerpos = 0; (playerpos < sitcode.Length) && (win_zug == -1); playerpos++)
                {
                    if (sitcode[playerpos] == '1')
                    {
                        momsitcode = sitcode.Remove(playerpos, 1).Insert(playerpos, SitCodeHelper.PlayertoSitCode(kispieler).ToString());
                        if ((pruefer.Pruefe(SitCodeHelper.PlayertoSitCode(kispieler), SitCodeHelper.ToBrett(momsitcode, Breite, Hoehe))) && (win_zug == -1))
                            win_zug = playerpos;
                    }
                }
                return win_zug;
            }

            private int TestHumanBlock(string sitcode)
            {
                string momsitcode;
                int block_zug = -1;
                char humanplayer = SitCodeHelper.PlayertoSitCode(kispieler) == '3' ? '2' : '3';
                for (int playerpos = 0; (playerpos < sitcode.Length) && (block_zug == -1); playerpos++)
                {
                    if (sitcode[playerpos] == '1')
                    {
                        momsitcode = sitcode.Remove(playerpos, 1).Insert(playerpos, humanplayer.ToString());
                        if ((pruefer.Pruefe(humanplayer, SitCodeHelper.ToBrett(momsitcode, Breite, Hoehe))) && (block_zug == -1))
                            block_zug = playerpos;
                    }
                }
                return block_zug;
            }

            public override string ToString()
            {
                return "Bot";
            }
        }

        class KIRandom : KI.AbstractKI
        {
            public KIRandom(int breite, int hoehe) : base('O', breite, hoehe) { }

            public override void Lernen()
            {
                base.Lernen();
            }

            public override int Spielen(char[,] brett, char spieler)
            {
                string sitcode = SitCodeHelper.Berechnen(brett);
                int zug = GetRandomZug(sitcode);
                return zug;
            }

            public override string ToString()
            {
                return "Random";
            }
        }
    }
}