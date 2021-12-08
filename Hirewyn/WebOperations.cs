using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Linq;

namespace Hirewyn
{
    public class WebOperations
    {
        public static Spell DownloadSpellData(string spellName)
        {
            return ByPassDndBeyond(spellName);
            string url = "https://www.dndbeyond.com/spells/";
            string spellUrl = spellName.Replace(' ', '-').Replace('/', '-').Replace("'", "").ToLower();
            string pageSource = String.Empty;
            Spell spell = new Spell();
            using (WebClient client = new WebClient())
            {
                pageSource = client.DownloadString(url + spellUrl);
            }
            if (!pageSource.Contains("Use your Twitch account or create one to sign in"))
            {
                spell = DownloadSpellDataFromDnDBeyond(pageSource);
            }
            else
            {
                url = "https://www.dnd-spells.com/spell/";
                spellUrl = spellName.Replace(' ', '-').Replace("/", "").Replace("'", "").ToLower();
                pageSource = String.Empty;
                using (WebClient client = new WebClient())
                {
                    pageSource = client.DownloadString(url + spellUrl);
                }
                if (pageSource.Contains("Looking for another spell "))
                {
                    spell = DownloadSpellDataFromDnDSpells(pageSource);
                    spell.Ritual = false;
                }
                else
                {
                    spellUrl += "-ritual";
                    pageSource = String.Empty;
                    using (WebClient client = new WebClient())
                    {
                        pageSource = client.DownloadString(url + spellUrl);
                    }
                    spell = DownloadSpellDataFromDnDSpells(pageSource);
                    spell.Ritual = true;
                    spell.Name = spell.Name.Substring(0, spell.Name.Length - 9);
                }
            }
            return spell;
        }
        public static Spell ByPassDndBeyond(string spellName)
        {
            string url = "https://www.dnd-spells.com/spell/";
            string spellUrl = spellName.Replace(' ', '-').Replace("/", "").Replace("'", "").ToLower();
            string pageSource = String.Empty;
            Spell spell = new Spell();
            using (WebClient client = new WebClient())
            {
                pageSource = client.DownloadString(url + spellUrl);
            }
            if (pageSource.Contains("Looking for another spell "))
            {
                spell = DownloadSpellDataFromDnDSpells(pageSource);
                spell.Ritual = false;
            }
            else
            {
                spellUrl += "-ritual";
                pageSource = String.Empty;
                using (WebClient client = new WebClient())
                {
                    pageSource = client.DownloadString(url + spellUrl);
                }
                spell = DownloadSpellDataFromDnDSpells(pageSource);
                spell.Ritual = true;
                spell.Name = spell.Name.Substring(0, spell.Name.Length - 9);
            }
            return spell;
        }
        public static Spell DownloadSpellDataFromDnDSpells(string pageSource)
        {
            Spell spell = new Spell();
            string[] lines = pageSource.Replace("\r\n", "\r").Replace('\n', '\r').Split('\r');
            int lineIndex = 0;
            foreach (string line in lines)
            {
                if (line.Contains("classic-title"))
                {
                    break;
                }
                lineIndex++;
            }
            spell.Name = GetValues(lines[lineIndex]).FirstOrDefault().Replace("&rsquo;", "'");

            lineIndex++;
            spell.School = GetValue(lines[lineIndex]);

            lineIndex += 2;
            if (lines[lineIndex].Contains("Cantrip"))
            {
                spell.Level = 0;
            }
            else
            {
                spell.Level = Convert.ToInt32(GetValues(lines[lineIndex]).LastOrDefault());
            }

            lineIndex++;
            //spell.CastingTime = GetValues(lines[lineIndex]).LastOrDefault().ToLower();

            lineIndex++;
            //spell.Range = GetValues(lines[lineIndex]).LastOrDefault();

            lineIndex++;
            spell.Components = GetValues(lines[lineIndex]).LastOrDefault().Replace("&rsquo;", "'");

            lineIndex++;
            //spell.Duration = ExtractDurationFromDnDSpells(GetValues(lines[lineIndex]).LastOrDefault());

            spell.Concentration = ExtractConcentrationFromDnDSpells(GetValues(lines[lineIndex]).LastOrDefault());

            int pageIndex = 0;
            foreach (string line in lines)
            {
                if (line.Contains("Page: "))
                {
                    break;
                }
                pageIndex++;
            }

            lineIndex += 4;
            bool isDescription = true;
            while (lineIndex < pageIndex)
            {
                if (lines[lineIndex].Contains("<h4 class=\"classic-title\"><span>At higher level</span></h4>"))
                {
                    isDescription = false;
                }
                else if (!string.IsNullOrEmpty(CleanUpValue(lines[lineIndex])) && isDescription)
                {
                    spell.Description += CleanUpValue(lines[lineIndex]) + "\r\n\r\n";
                }
                else if (!string.IsNullOrEmpty(CleanUpValue(lines[lineIndex])))
                {
                    spell.HigherLevels += CleanUpValue(lines[lineIndex]) + "\r\n\r\n";
                }
                lineIndex++;
            }
            spell.Description = spell.Description.Remove(spell.Description.LastIndexOf("\r\n\r\n"));
            if (!string.IsNullOrEmpty(spell.HigherLevels))
            {
                spell.HigherLevels = spell.HigherLevels.Remove(spell.HigherLevels.LastIndexOf("\r\n\r\n"));
            }

            string[] sourceLines = lines[lineIndex].Trim().Split(' ');
            for (int sourceLineIndex = 2; sourceLineIndex < sourceLines.Length; sourceLineIndex++)
            {
                if (string.IsNullOrEmpty(sourceLines[sourceLineIndex]) && string.IsNullOrEmpty(sourceLines[sourceLineIndex + 1]))
                {
                    break;
                }
                spell.Source += sourceLines[sourceLineIndex] + " ";
            }
            spell.Source = spell.Source.Remove(spell.Source.LastIndexOf(" ")).Trim();

            lineIndex += 2;
            string nextString = String.Empty;
            for (int i = lineIndex; i < lines.Length; i++)
            {
                nextString += lines[i];
            }
            string a = GetElement(nextString, "p>");
            a = a.Replace(",", "");
            List<string> b = GetValues(a);
            b.RemoveAt(0);
            b.RemoveAt(b.Count - 1);
            spell.Classes = b;


            return spell;
        }
        public static string ExtractDurationFromDnDSpells(string durationString)
        {
            return durationString.Replace("Concentration, up to ", "");
        }
        public static bool ExtractConcentrationFromDnDSpells(string concentrationString)
        {
            return concentrationString.Contains("Concentration");
        }


        public static Spell DownloadSpellDataFromDnDBeyond(string pageSource)
        {
            Spell spell = new Spell();
            spell.Name = GetValue(GetElement(pageSource, "page-title"));
            spell.School = GetSourceElementFromDndBeyond(pageSource, "school");
            spell.Classes = ExtractClasses(GetValues(GetElement(pageSource, "tags available-for")));
            spell.Level = Convert.ToInt32(ExtractLevel(GetSourceElementFromDndBeyond(pageSource, "level")));
            spell.Ritual = IsRitual(pageSource);
            //spell.CastingTime = GetValues(GetElement(GetElement(pageSource, "ddb-statblock-item ddb-statblock-item-casting-time"), "ddb-statblock-item-value")).FirstOrDefault().ToLower();
            //spell.Range = GetValues(GetElement(GetElement(pageSource, "ddb-statblock-item ddb-statblock-item-range-area"), "ddb-statblock-item-value")).FirstOrDefault().Replace("ft", "feet");
            spell.Components = ExtractComponents(pageSource);
            //spell.Duration = GetValues(GetElement(GetElement(pageSource, "ddb-statblock-item ddb-statblock-item-duration"), "ddb-statblock-item-value")).LastOrDefault();
            spell.Concentration = IsConcentration(pageSource);
            spell.Effect = String.Empty;
            DecipherDescription(ref spell, pageSource);
            spell.Source = ExtractSource(pageSource);

            return spell;
        }
        public static List<string> ExtractClasses(List<string> classes)
        {
            classes.RemoveAt(0);
            return classes;
        }
        public static string ExtractLevel(string level)
        {
            return level.Remove(1);
        }
        public static bool IsRitual(string pageSource)
        {
            return pageSource.Contains("i-ritual");
        }
        public static string ExtractComponents(string pageSource)
        {
            string components = String.Empty;
            components = GetValue(GetElement(pageSource, "component-asterisks"));
            if (components.Contains("*"))
            {
                string materialComponents = GetValue(GetElement(pageSource, "components-blurb")).Substring(4);
                components = components.Replace("*", materialComponents);

            }
            return components;
        }
        public static bool IsConcentration(string pageSource)
        {
            return pageSource.Contains("i-concentration");
        }
        public static void DecipherDescription(ref Spell spell, string pageSource)
        {
            string description = GetElement(pageSource, "more-info-content");
            description = PurifyDescription(description);

            List<string> descriptionStrings = GetValues(description);
            bool isDescription = true;
            foreach (string descriptionString in descriptionStrings)
            {
                if (descriptionString.Contains("* - ("))
                {

                }
                else if (descriptionString == "At Higher Levels.")
                {
                    isDescription = false;
                }
                else if (isDescription)
                {
                    spell.Description += descriptionString + "\r\n\r\n";
                }
                else if (!isDescription && !descriptionString.Contains("* - ("))
                {
                    spell.HigherLevels += descriptionString + "\r\n\r\n";
                }
            }
            spell.Description = spell.Description.Remove(spell.Description.LastIndexOf("\r\n\r\n"));
            if (!string.IsNullOrEmpty(spell.HigherLevels))
            {
                spell.HigherLevels = spell.HigherLevels.Remove(spell.HigherLevels.LastIndexOf("\r\n\r\n"));
            }
        }
        public static string ExtractSource(string pageSource)
        {
            string source = GetValues(GetElement(pageSource, "source spell-source")).FirstOrDefault();
            if (source == "Basic Rules")
            {
                source = "Player's Handbook";
            }
            return source;
        }
        public static string PurifyDescription(string description)
        {
            string purifiedDescription = String.Empty;
            bool isPurifying = false;
            for (int descriptionIndex = 0; descriptionIndex < description.Length; descriptionIndex++)
            {
                if ((description[descriptionIndex] == '<' && description[descriptionIndex + 1] == 'a') || (description[descriptionIndex] == '<' && description[descriptionIndex + 1] == '/' && description[descriptionIndex + 2] == 'a'))
                {
                    isPurifying = true;
                }
                else if (description[descriptionIndex] == '>' && isPurifying)
                {
                    isPurifying = false;
                }
                else if (!isPurifying)
                {
                    purifiedDescription += description[descriptionIndex];
                }
            }
            return purifiedDescription;
        }

        public static List<string> GetValues(string valuesBlock)
        {
            int iterationCounter = 0;
            bool endOfValue = false;
            string value = String.Empty;
            List<string> values = new List<string>();
            for (int index = 0; index < valuesBlock.Length; index++)
            {
                if (valuesBlock[index] == '<')
                {
                    iterationCounter++;
                }
                else if (valuesBlock[index] == '>')
                {
                    iterationCounter--;
                }
                if (iterationCounter == 0)
                {
                    value += valuesBlock[index];
                    endOfValue = true;
                }
                else
                {
                    if (endOfValue)
                    {
                        value = value.Substring(1);
                        value = CleanUpValue(value);
                        if (!string.IsNullOrEmpty(value))
                        {
                            values.Add(value);
                            value = String.Empty;
                        }
                        endOfValue = false;
                    }
                }
            }
            return values;
        }

        public static string GetSourceElementFromDndBeyond(string pageSource, string infoName)
        {
            string firstElementBlock = GetElement(pageSource, "ddb-statblock-item ddb-statblock-item-" + infoName);
            string secondElementBlock = GetElement(firstElementBlock, "ddb-statblock-item-value");
            string elementValue = GetValue(secondElementBlock);
            string cleanValue = CleanUpValue(elementValue);
            return cleanValue;
        }

        public static string CleanUpValue(string value)
        {
            return value.Replace("\n", "").Replace("<p>", "").Replace("</p>", "").Replace(" <br />", "").Replace("<br />", "").Replace("&nbsp;", "").Replace("<hr>", "").Trim();
        }

        private static string GetValue(string elementBlock)
        {
            string elementValue = String.Empty;

            int elementValueStartIndex = elementBlock.IndexOf('>') + 1;
            int elementValueEndIndex = elementBlock.LastIndexOf('<');

            return CleanUpValue(elementBlock.Substring(elementValueStartIndex, elementValueEndIndex - elementValueStartIndex));
        }
        public static string GetElement(string pageSource, string uniqueIdentifierString)
        {
            string elementBlock = String.Empty;

            int currentIndex = pageSource.IndexOf(uniqueIdentifierString);
            char currentChar = 'a';
            while (currentChar != '<')
            {
                currentIndex--;
                currentChar = pageSource[currentIndex];
            }
            int startOfElementIndex = currentIndex;
            string elementStart = String.Empty;
            while (pageSource[currentIndex] != ' ')
            {
                elementStart += pageSource[currentIndex];
                currentIndex++;
            }
            string elementEnd = "</" + elementStart.Substring(1);
            int iterationCounter = 1;
            while (iterationCounter != 0)
            {
                int elementStartIndex = currentIndex + pageSource.Substring(currentIndex).IndexOf(elementStart);

                int elementEndIndex = currentIndex + pageSource.Substring(currentIndex).IndexOf(elementEnd);
                if (elementStartIndex < elementEndIndex && elementStartIndex > currentIndex)
                {
                    iterationCounter++;
                    currentIndex = elementStartIndex + 1;
                }
                else
                {
                    iterationCounter--;
                    currentIndex = elementEndIndex + 1;
                }
            }
            currentIndex += elementEnd.Length;
            elementBlock = pageSource.Substring(startOfElementIndex, currentIndex - startOfElementIndex);

            return elementBlock;
        }
    }
}
