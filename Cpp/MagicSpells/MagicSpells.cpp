    if (Fireball *fireball = dynamic_cast<Fireball*>(spell))
    {
        fireball->revealFirepower();
    }
    else if (Frostbite *frostbite = dynamic_cast<Frostbite*>(spell))
    {
        frostbite->revealFrostpower();
    }
    else if (Thunderstorm *thunderstorm = dynamic_cast<Thunderstorm*>(spell))
    {
        thunderstorm->revealThunderpower();
    }
    else if (Waterbolt *waterbolt = dynamic_cast<Waterbolt*>(spell))
    {
        waterbolt->revealWaterpower();
    }
    else
    {
        // from https://en.wikipedia.org/wiki/Longest_common_subsequence_problem
        std::string scroll = spell->revealScrollName();
        std::string journal = SpellJournal::read();
        int m = scroll.size();
        int n = journal.size();
        std::vector<int> prevRow;
        prevRow.resize(n + 1);
        std::vector<int> row;
        row.resize(n + 1);
        for (int j = 0; j <= n; ++j)
            row.at(j) = 0;
        for (int i = 1; i <= m; ++i)
        {
            row.swap(prevRow);
            row.at(0) = 0;
            for (int j = 1; j <= n; ++j)
            {
                if (scroll[i - 1] == journal[j -1])
                    row[j] = prevRow[j - 1] + 1;
                else
                    row[j] = max(row[j - 1], prevRow[j]);
            }
        }
        int value = row.at(n);
        std::cout << value << std::endl;
    }