public class NameService
{
    public static String nameMaker(int nameNumber){
        Random rng = new Random();
        List<String> name = new List<string>(nameNumber);
        while(nameNumber-->=0)
            name.Add(wordMaker(rng.Next(2,4),true));
        return name.Aggregate((w1,w2) => w1 + " " + w2);
    }

    public static String wordMaker(int syllableNumber, bool isCap){
        Random rng = new Random();
        List<string> syllables = SyllableMaker(rng.Next(1, syllableNumber));
        String word = syllables.Aggregate((syl1,syl2) => syl1 + syl2);
        return isCap?word.Substring(0,1).ToUpper()+word.Substring(1):word;
    }
    
    public static List<String> SyllableMaker(int reptition = 100) // Criação de sílabas aleatórias
    {
        string[] vowals = { "a", "e", "i", "o", "u" };// Lista de vogais
        string[] consoants = { "b", "c", "d", "f", "g", "h", "j", "l", "m", "n", "p", "q", "r", "s", "t", "v" };// Lista de consoantes comuns
        string[] c_inc = { "k", "w", "x", "y", "z" };//Lista de consoantes incomuns
        int v_len = vowals.Length;
        int c_len = consoants.Length;
        int ci_len = c_inc.Length;
        Random rng = new Random();
        List<string> syllables = new List<string>();
        for (int i = 0; i < reptition; i++) // Definido por usuário, a quantidade de sílabas a serem criadas
        {
            string syllable = "";
            int opt = rng.Next(1, 4);// Esolhe como será construido a sílaba aleatóriamente
            switch (opt)
            {
                case 1://uma consoante e mais uma vogal, e uma chance de ter-se uma letra 'u' no meio delas
                    syllable += consoants[rng.Next(c_len)] + (rng.Next(10) > 9 ? "u" : "") + vowals[rng.Next(v_len)];
                    break;
                case 2:// Uma consoante incomun, uma consoante e uma vogal
                    syllable += c_inc[rng.Next(ci_len)] + consoants[rng.Next(c_len)] + vowals[rng.Next(v_len)];
                    break;
                case 3:// Uma consoante incomun, uma vogal e uma consoante
                    syllable += c_inc[rng.Next(ci_len)] + vowals[rng.Next(v_len)] + consoants[rng.Next(c_len)];
                    break;
                case 4:// Uma consoante incomun, uma vogal, uma consoante e mais uma vogal
                    syllable += c_inc[rng.Next(ci_len)] + vowals[rng.Next(v_len)] + consoants[rng.Next(c_len)] + vowals[rng.Next(v_len)];
                    break;
                case 5:// Uma vogal, uma consoante incomun e uma consoante comum
                    syllable += vowals[rng.Next(v_len)] + c_inc[rng.Next(ci_len)] + consoants[rng.Next(c_len)];
                    break;
            }
            syllables.Add(syllable);
        }
        return syllables;
    }
}