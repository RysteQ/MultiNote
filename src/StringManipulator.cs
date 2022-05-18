public class StringManipulator
{
    public int countWords(String toAnalyze)
    {
        int toReturn = 0;

        for (int i = 0; i < toAnalyze.Length - 1; i++)
            if (toAnalyze[i] == ' ' || toAnalyze[i] == '\t' || toAnalyze[i] == '\n')
                if (toAnalyze[i + 1] != ' ' && toAnalyze[i + 1] != '\t' && toAnalyze[i + 1] != '\n')
                    toReturn++;

        return toReturn;
    }

    public String replaceStringPoints(String stringToUse, String[] replaceWith)
    {
        String toReturn = stringToUse;

        for (int i = 0; i < replaceWith.Length; i++)
            toReturn = toReturn.Replace("{" + i.ToString() + "}", replaceWith[i]);

        return toReturn;
    }

    public String getLastWord(String stringToUse, String toSplit)
    {
        return stringToUse.Split(toSplit)[stringToUse.Split(toSplit).Length - 1];
    }
    
    public String getAllButLastWord(String stringToUse, String toSplit)
    {
        String toReturn = "";

        for (int i = 0; i < stringToUse.Split(toSplit).Length - 1; i++)
            toReturn += stringToUse.Split(toSplit)[i] + toSplit;

        return toReturn;
    }
}